#include "PdfPrecalculatedFunction.h"
#include "MemoryManager.h"
#include "Logger.h"

DLLEXPORT_TEST_FUNCTION struct PdfPrecalculatedFunction *PdfPrecalculatedFunction_Create(int type, const char *datastream, const char *column)
{
	struct PdfPrecalculatedFunction *ret;

	ret = (struct PdfPrecalculatedFunction *)MemoryManager_Alloc(sizeof(struct PdfPrecalculatedFunction));
	ret->type = type;
	ret->data = NULL;
	ret->result = 0.0;
	ret->avgResult = 0.0;
	ret->count = 0;
	ret->start = 0;
	ret->end = 0;

	ret->dataStream = MemoryManager_StrDup(datastream);
	ret->columnName = MemoryManager_StrDup(column);

	Logger_LogNoticeMessage("PdfPrecalculatedFunction: CREATE");
	return ret;
}



DLLEXPORT_TEST_FUNCTION void PdfPrecalculatedFunction_SetStringPosition(struct PdfPrecalculatedFunction *self, int start, int end)
{
	if(!self)
	{
		return;
	}

	self->start = start;
	self->end = end;
}




DLLEXPORT_TEST_FUNCTION void PdfPrecalculatedFunction_Destroy(struct PdfPrecalculatedFunction *self)
{
	PdfPrecalculatedFunction_Cleanup(self);
	MemoryManager_Free(self);
}






DLLEXPORT_TEST_FUNCTION void PdfPrecalculatedFunction_Cleanup(struct PdfPrecalculatedFunction *self)
{
	if(self->data)
	{
		MemoryManager_Free(self->data);
		self->data = NULL;
	}
}






DLLEXPORT_TEST_FUNCTION double PdfPrecalculatedFunction_GetValue(struct PdfPrecalculatedFunction *self, char *retValue)
{	
	if(retValue != NULL)
	{
		sprintf(retValue,"%.2f",self->result);
	}
	
	return self->result;
}

DLLEXPORT_TEST_FUNCTION void PdfPrecalculatedFunction_ResetValue(struct PdfPrecalculatedFunction *self)
{
	self->avgResult = 0;
	self->count = 0;
	self->result = 0;	
}

DLLEXPORT_TEST_FUNCTION void PdfPrecalculatedFunction_AddValue(struct PdfPrecalculatedFunction *self, float inValue)
{
	int tmpVal = 0, i = 0;
	float tmpRes = 0.0f, value;
	char tmpFinal[100], tmpFinal1[100];
	int count = 0;
	
	tmpFinal[0] = '\0';
	tmpFinal1[0] = '\0';
	value = inValue;

	switch (self->type)
	{
	case PDF_PRECALCULATED_FUNCTION_TYPE_SUM:
		self->result += value;
		break;

	case PDF_PRECALCULATED_FUNCTION_TYPE_AVG:
		self->avgResult += value;
		self->count++;
		self->result = (self->avgResult / self->count);
		break;

	case PDF_PRECALCULATED_FUNCTION_TYPE_MIN:
		if(value < self->result || self->count == 0)
		{
			self->result = value;
			self->count++;
		}
		break;

	case PDF_PRECALCULATED_FUNCTION_TYPE_MAX:
		if(value > self->result || self->count == 0)
		{
			self->result = value;
			self->count++;
		}
		break;

	case PDF_PRECALCULATED_FUNCTION_TYPE_COUNT:
		self->result++;
		break;

	case PDF_PRECALCULATED_FUNCTION_TYPE_DECTOHEX:
		if(self->data == NULL)
		{
			self->data = MemoryManager_Alloc(8);
		}
		self->result += value;
		sprintf(self->data, "%#x", (int)self->result);
		break;

	case PDF_PRECALCULATED_FUNCTION_TYPE_DECTOBIN:
		self->result += value;
		tmpRes = self->result;
		
		//Convert to binary
		{
			while(tmpVal != 1)
			{
				tmpRes = tmpRes / 2;
				tmpVal = (int)tmpRes;
				if(tmpVal == tmpRes)
				{
					tmpFinal[count] = '0';
				}
				else if(tmpRes > tmpVal)
				{
					tmpFinal[count] = '1';
				}
				if(tmpVal == 1) 
				{
					tmpFinal[count + 1] = '1';
					tmpFinal[count + 2] = '\0';
				}else{
					tmpFinal[count + 1] = '\0';
				}
				count++;
				tmpRes = (double)tmpVal;
			}
		}

		//make binary output...
		if(self->data == NULL)
		{
			self->data = MemoryManager_Alloc(100);
		}
		for(i=count; i>=0; i--)
		{
			tmpFinal1[count - i] = tmpFinal[i];// reverse calculation... <-
		}
		tmpFinal1[count+1]='\0';
		sprintf(self->data,"%s",tmpFinal1);
		break;
	}
}