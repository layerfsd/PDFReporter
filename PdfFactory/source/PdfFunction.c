//-----------------------------------------------------------------------------
// Name:	PdfFunction.c
// Author:	Tomislav Kukic
// Date:	28.12.2008
//-----------------------------------------------------------------------------

#include "PdfFunction.h"
#include "PdfDocument.h"
#include "MemoryManager.h"
#include "DLList.h"
#include "DictionaryObject.h"
#include "NumberObject.h"
#include "ArrayObject.h"
#include "IndirectReference.h"


DLLEXPORT_TEST_FUNCTION struct PdfFunction *PdfFunction_Create(struct PdfDocument *document, int type, double shadingFaktor)
{
	struct PdfFunction *ret;

	ret = (struct PdfFunction*)MemoryManager_Alloc(sizeof(struct PdfFunction));
	PdfFunction_Init(ret, document, type, shadingFaktor);
	return ret;
}




DLLEXPORT_TEST_FUNCTION void PdfFunction_Init(struct PdfFunction *self, struct PdfDocument *document, int type, double shadingFaktor)
{
	PdfBaseObject_Init(&self->base, document);
	self->functionType = type;
	self->cIn = 0.0;
	self->mIn = 0.0;
	self->yIn = 0.0;
	self->kIn = 0.0;
	self->cOut = 0.0;
	self->mOut = 0.0;
	self->yOut = 0.0;
	self->kOut = 0.0;
	self->rIn = 0.0;
	self->gIn = 0.0;
	self->bIn = 0.0;
	self->rOut = 0.0;
	self->gOut = 0.0;
	self->bOut = 0.0;
	self->shadingFaktor = shadingFaktor;
	self->numOfEncodings = 0;
	if(type == 3)
	{
		self->functions = DLList_Create();
	}else{
		self->functions = 0;
	}
}




DLLEXPORT_TEST_FUNCTION void PdfFunction_Destroy(struct PdfFunction *self)
{
	PdfFunction_Cleanup(self);
	MemoryManager_Free(self);
}





DLLEXPORT_TEST_FUNCTION void PdfFunction_Cleanup(struct PdfFunction *self)
{
	//TO DO: if needed place some cleanup code here...
	struct DLListNode *iterator;
	struct PdfFunction *tmpFunction;

	if(self->functions)
	{
		for(iterator = DLList_Begin(self->functions); iterator != DLList_End(self->functions); iterator = iterator->next)
		{
			tmpFunction = (struct PdfFunction *)iterator->data;
			PdfFunction_Destroy(tmpFunction);
			iterator->data = 0;
			tmpFunction = 0;
		}
		DLList_Destroy(self->functions);
	}	
}




DLLEXPORT_TEST_FUNCTION void PdfFunction_Write(struct PdfFunction *self, struct StreamWriter *streamWriter, int useCMYK)
{
	struct DictionaryObject *dO;
	struct ArrayObject *arr;
	struct NumberObject *number;
	struct IndirectReference *indRef;
	struct PdfFunction *tmpFunction;
	struct DLListNode *iterator;
	char tmpIn[100], tmpOut[100];
	int isBreak, i;

	tmpFunction = 0;
	isBreak = 0;
	i = 0;

	if(self->functionType == 3)
	{
		//If multi function object then this object is actually a list of functions nothing more...
	
		for(iterator = DLList_Begin(self->functions); iterator != DLList_End(self->functions); iterator = iterator->next)
		{
			tmpFunction = (struct PdfFunction *)iterator->data;
			PdfFunction_Write(tmpFunction, streamWriter, useCMYK);
		}

		PdfDocument_BeginNewObject(&self->base);
		{
			dO = DictionaryObject_Begin(streamWriter);

			DictionaryObject_WriteKey(dO, "FunctionType");
			number = NumberObject_Create(streamWriter, (double)self->functionType);
			NumberObject_Write(number);
			NumberObject_Destroy(number);

			DictionaryObject_WriteKey(dO, "Domain");
			arr = ArrayObject_BeginArray(streamWriter);
			streamWriter->WriteData(streamWriter, "0.0 1.0");
			ArrayObject_EndArray(arr);

			DictionaryObject_WriteKey(dO, "Bounds");
			arr = ArrayObject_BeginArray(streamWriter);
			streamWriter->WriteData(streamWriter, "0.5");
			ArrayObject_EndArray(arr);

			DictionaryObject_WriteKey(dO, "Encode");
			arr = ArrayObject_BeginArray(streamWriter);
			for(i=0; i<self->numOfEncodings; i++)
			{
				streamWriter->WriteData(streamWriter, " 0.0 1.0");
			}
			ArrayObject_EndArray(arr);

			DictionaryObject_WriteKey(dO, "Functions");
			arr = ArrayObject_BeginArray(streamWriter);
			{
				for(iterator = DLList_Begin(self->functions); iterator != DLList_End(self->functions); iterator = iterator->next)
				{
					tmpFunction = (struct PdfFunction *)iterator->data;
					indRef = IndirectReference_Create(streamWriter, tmpFunction->base.objectId, tmpFunction->base.generationNumber);
					IndirectReference_Write(indRef);
					IndirectReference_Destroy(indRef);
				}
			}
			ArrayObject_EndArray(arr);

			DictionaryObject_End(dO);	
		}	
		PdfDocument_EndObject(&self->base);
	}
	else if(self->functionType == 2)
	{
		if(useCMYK)
		{
			sprintf(tmpIn,"%f %f %f %f", self->cIn, self->mIn, self->yIn, self->kIn);
			sprintf(tmpOut,"%f %f %f %f", self->cOut, self->mOut, self->yOut, self->kOut);
		}
		else
		{
			sprintf(tmpIn, "%f %f %f", self->rIn, self->gIn, self->bIn);
			sprintf(tmpOut, "%f %f %f", self->rOut, self->gOut, self->bOut);
		}


		PdfDocument_BeginNewObject(&self->base);
		{
			dO = DictionaryObject_Begin(streamWriter);
			
			DictionaryObject_WriteKey(dO, "FunctionType");
			number = NumberObject_Create(streamWriter, (double)self->functionType);
			NumberObject_Write(number);
			NumberObject_Destroy(number);

			DictionaryObject_WriteKey(dO, "Domain");
			arr = ArrayObject_BeginArray(streamWriter);
			streamWriter->WriteData(streamWriter, "0.0 1.0");
			ArrayObject_EndArray(arr);

			DictionaryObject_WriteKey(dO, "C0");
			arr = ArrayObject_BeginArray(streamWriter);
			streamWriter->WriteData(streamWriter, tmpIn);
			ArrayObject_EndArray(arr);

			DictionaryObject_WriteKey(dO, "C1");
			arr = ArrayObject_BeginArray(streamWriter);
			streamWriter->WriteData(streamWriter, tmpOut);
			ArrayObject_EndArray(arr);

			DictionaryObject_WriteKey(dO, "N");
			number = NumberObject_Create(streamWriter, 1.0);
			NumberObject_Write(number);
			NumberObject_Destroy(number);

			DictionaryObject_End(dO);	
		}	
		PdfDocument_EndObject(&self->base);
	}
}




DLLEXPORT_TEST_FUNCTION void PdfFunction_SetRGBStartColor(struct PdfFunction *self, double r, double g, double b)
{
	self->rIn = r;
	self->gIn = g;
	self->bIn = b;
}




DLLEXPORT_TEST_FUNCTION void PdfFunction_SetRGBEndColor(struct PdfFunction *self, double r, double g, double b)
{
	self->rOut = r;
	self->gOut = g;
	self->bOut = b;
}




DLLEXPORT_TEST_FUNCTION void PdfFunction_SetCMYKStartColor(struct PdfFunction *self, double c, double m, double y, double k)
{
	self->cIn = c;
	self->mIn = m;
	self->yIn = y;
	self->kIn = k;
}




DLLEXPORT_TEST_FUNCTION void PdfFunction_SetCMYKEndColor(struct PdfFunction *self, double c, double m, double y, double k)
{
	self->cOut = c;
	self->mOut = m;
	self->yOut = y;
	self->kOut = k;
}




DLLEXPORT_TEST_FUNCTION void PdfFunction_AddNewFunction(struct PdfFunction *self, struct PdfFunction *newFunction)
{
	if(self->functionType == 3)
	{
		if(self->functions)
		{
			DLList_PushBack(self->functions, (void*)newFunction);
			self->numOfEncodings++;
		}
	}
}