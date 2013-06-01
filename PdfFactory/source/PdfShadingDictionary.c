//-----------------------------------------------------------------------------
// Name:	PdfShadingDictionary.c
// Author:	Tomislav Kukic
// Date:	27.12.2008
//-----------------------------------------------------------------------------


#include "PdfShadingDictionary.h"
#include "MemoryManager.h"
#include "PdfDocument.h"
#include "PdfFunction.h"
#include "DictionaryObject.h"
#include "NumberObject.h"
#include "IndirectReference.h"


DLLEXPORT_TEST_FUNCTION struct PdfShadingDictionary *PdfShadingDictionary_Create(struct PdfDocument *document, int shadingType, int functionType, int antiAliasing, int useCMYK, double shadingFactor)
{
	struct PdfShadingDictionary *ret;

	ret = (struct PdfShadingDictionary*)MemoryManager_Alloc(sizeof(struct PdfShadingDictionary));
	PdfShadingDictionary_Init(ret, document, shadingType, functionType, antiAliasing, useCMYK, shadingFactor);

	return ret;
}




DLLEXPORT_TEST_FUNCTION void PdfShadingDictionary_Init(struct PdfShadingDictionary *self, struct PdfDocument *document, int shadingType, int functionType, int antiAliasing, int useCMYK, double shadingFactor)
{
	char tmp[100];
	struct PdfFunction;	

	PdfBaseObject_Init(&self->base, document);
	self->antiAliasing = antiAliasing;
	self->shadingType = shadingType;
	self->useCMYK = useCMYK;
	if(useCMYK == 0)
	{
		self->colorSpace = MemoryManager_StrDup("DeviceRGB");
	}else{
		self->colorSpace = MemoryManager_StrDup("DeviceCMYK");
	}

	sprintf(tmp, "Sh%d", document->nextShadingId);
	document->nextShadingId++;
	self->name = MemoryManager_StrDup(tmp);

	self->function = PdfFunction_Create(document, functionType, shadingFactor);
	self->lastAddedFunction = self->function;

	DLList_PushBack(document->shadings, self);  // add self to document's shadings list

	sprintf(self->radialCoords,"%s", "0.0 0.0 0.0 0.0 0.0 1.0");
	sprintf(self->axialCoords, "%s", "0.0 0.0 0.0 1.0");
}





DLLEXPORT_TEST_FUNCTION void PdfShadingDictionary_Destroy(struct PdfShadingDictionary *self)
{
	PdfShadingDictionary_Cleanup(self);
	MemoryManager_Free(self);
}





DLLEXPORT_TEST_FUNCTION void PdfShadingDictionary_Cleanup(struct PdfShadingDictionary *self)
{
	if(self->colorSpace)
	{
		MemoryManager_Free(self->colorSpace);
		self->colorSpace = 0;
	}
	if(self->function)
	{
		PdfFunction_Destroy(self->function);
		self->function = 0;
	}
	if (self->name)
	{
		MemoryManager_Free(self->name);
		self->name = 0;
	}
}




DLLEXPORT_TEST_FUNCTION void PdfShadingDictionary_AddNextFunction(struct PdfShadingDictionary *self, struct PdfFunction *nextFunction)
{
	if(self->function)
	{
		if(self->function->functionType == 2)
		{
			return;
		}
		if(nextFunction)
		{
			PdfFunction_AddNewFunction(self->function, nextFunction);
			self->lastAddedFunction = nextFunction;
		}else{
			return;
		}
	}
}




DLLEXPORT_TEST_FUNCTION void PdfShadingDictionary_Write(struct PdfShadingDictionary *self, struct StreamWriter *streamWriter)
{
	struct DictionaryObject *dO;
	struct ArrayObject *arr;
	struct NameObject *name;
	struct NumberObject *number;
	struct IndirectReference *indRef;

	PdfFunction_Write(self->function, streamWriter, self->useCMYK);

	PdfDocument_BeginNewObject(&self->base);
	{
		dO = DictionaryObject_Begin(streamWriter);

		DictionaryObject_WriteKey(dO, "ShadingType");
		number = NumberObject_Create(streamWriter, (double)self->shadingType);
		NumberObject_Write(number);
		NumberObject_Destroy(number);

		DictionaryObject_WriteKey(dO, "ColorSpace");
		name = NameObject_Create(streamWriter, self->colorSpace);
		NameObject_Write(name);
		NameObject_Destroy(name);

		DictionaryObject_WriteKey(dO, "Function");
		indRef = IndirectReference_Create(streamWriter, self->function->base.objectId, 0);
		IndirectReference_Write(indRef);
		IndirectReference_Destroy(indRef);

		/*DictionaryObject_WriteKey(dO, "Domain");
		arr = ArrayObject_BeginArray(streamWriter);
		streamWriter->WriteData(streamWriter, "1.0 0.0");
		ArrayObject_EndArray(arr);*/

		if(self->shadingType == 3)
		{
			DictionaryObject_WriteKey(dO, "Coords");
			arr = ArrayObject_BeginArray(streamWriter);
			streamWriter->WriteData(streamWriter, self->radialCoords);
			ArrayObject_EndArray(arr);
		}

		if(self->shadingType == 2)
		{
			DictionaryObject_WriteKey(dO, "Coords");
			arr = ArrayObject_BeginArray(streamWriter);
			streamWriter->WriteData(streamWriter, self->axialCoords);
			ArrayObject_EndArray(arr);
		}

		DictionaryObject_WriteKey(dO, "Extend");
		arr = ArrayObject_BeginArray(streamWriter);
		streamWriter->WriteData(streamWriter, "true true");
		ArrayObject_EndArray(arr);

		DictionaryObject_End(dO);	
	}	
	PdfDocument_EndObject(&self->base);
}



DLLEXPORT_TEST_FUNCTION void PdfShadingDictionary_SetRGBStartColor(struct PdfShadingDictionary *self, double r, double g, double b)
{
	if(self->function)
	{
		PdfFunction_SetRGBStartColor(self->lastAddedFunction, r, g, b);
	}
}



DLLEXPORT_TEST_FUNCTION void PdfShadingDictionary_SetRGBEndColor(struct PdfShadingDictionary *self, double r, double g, double b)
{
	if(self->function)
	{
		PdfFunction_SetRGBEndColor(self->lastAddedFunction, r, g, b);
	}
}



DLLEXPORT_TEST_FUNCTION void PdfShadingDictionary_SetCMYKStartColor(struct PdfShadingDictionary *self, double c, double m, double y, double k)
{
	if(self->function)
	{
		PdfFunction_SetCMYKStartColor(self->lastAddedFunction, c, m, y, k);
	}
}



DLLEXPORT_TEST_FUNCTION void PdfShadingDictionary_SetCMYKEndColor(struct PdfShadingDictionary *self, double c, double m, double y, double k)
{
	if(self->function)
	{
		PdfFunction_SetCMYKEndColor(self->lastAddedFunction, c, m, y, k);
	}
}



DLLEXPORT_TEST_FUNCTION void PdfShadingDictionary_SetAxialCoords(struct PdfShadingDictionary *self, double startX, double startY, double endX, double endY)
{
	sprintf(self->axialCoords, "%f %f %f %f", startX, startY, endX, endY);
}




DLLEXPORT_TEST_FUNCTION void PdfShadingDictionary_SetRadialCoords(struct PdfShadingDictionary *self, double startX, double startY, double startRadius, double endX, double endY, double endRadius)
{
	sprintf(self->radialCoords, "%f %f %f %f %f %f", startX, startY, startRadius, endX, endY, endRadius);
}