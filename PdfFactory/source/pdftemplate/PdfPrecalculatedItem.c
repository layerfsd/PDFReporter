#include "PdfPrecalculatedItem.h"
#include "PdfPrecalculatedFunction.h"
#include "PdfTextWriter.h"
#include "DLList.h"
#include "PdfTemplate.h"
#include "PdfMathParser.h"
#include "PdfTemplateFont.h"
#include "PdfTemplateEmbeddedFont.h"
#include "PdfFont.h"
#include "TransformationMatrix.h"
#include "PdfGenerator.h"
#include "PdfGeneratedBalloon.h"
#include "PdfPage.h"
#include "MemoryManager.h"
#include "PdfPageResources.h"
#include "Logger.h"
#include <stdio.h>
#include <stdlib.h>

struct DLList *precalculatedItems = NULL;

DLLEXPORT_TEST_FUNCTION void PdfPrecalculatedItem_Init(struct PdfPrecalculatedItem* self, const char *formula)
{
	self->formula = MemoryManager_StrDup(formula);
	self->functions = DLList_Create();
	self->parsedFormula[0] = '\0';
	self->font = NULL;
	PdfPrecalculatedItem_ParseItem(self, self->formula);
}

DLLEXPORT_TEST_FUNCTION struct PdfPrecalculatedItem* PdfPrecalculatedItem_Create(const char *formula)
{
	struct PdfPrecalculatedItem *ret;
	ret = (struct PdfPrecalculatedItem *)MemoryManager_Alloc(sizeof(struct PdfPrecalculatedItem));
	PdfPrecalculatedItem_Init(ret, formula);

	return ret;
}


DLLEXPORT_TEST_FUNCTION struct PdfPrecalculatedItem* PdfPrecalculatedItem_CreateFromXml(xmlNode *node)
{
	struct PdfPrecalculatedItem *ret;
	double tmpvalue = 0.0;
	xmlNode *foundNode;

	ret = (struct PdfPrecalculatedItem *)MemoryManager_Alloc(sizeof(struct PdfPrecalculatedItem));

	PdfTemplateBalloonItem_InitFromXml((struct PdfTemplateBalloonItem*)ret, node, PdfPrecalculatedItem_Destroy, PdfPrecalculatedItem_Process);
	ret->functions = DLList_Create();
	ret->formula = NULL;
	ret->parsedFormula[0] = '\0';
	ret->font = NULL;
	
	foundNode = PdfTemplate_FindNode(node, "Font");
	if(foundNode)
	{
		ret->font = PdfTemplateFont_CreateFromXml(foundNode);
	}


	foundNode = PdfTemplate_FindNode(node, "Formula");
	if(foundNode)
	{		
		ret->formula = PdfTemplate_LoadStringAttribute(foundNode, "Value");
		PdfPrecalculatedItem_ParseItem(ret, ret->formula);	
	}

	Logger_LogNoticeMessage("PdfPrecalculatedItem_CreateFromXml: SUCCESS");

	// Add to processing list
	PdfPrecalculatedItem_Static_AddPrecalculatedItem(ret);
	
	return ret;
}

void PdfPrecalculatedItem_Static_AddPrecalculatedItem(struct PdfPrecalculatedItem *newItem)
{
	if (!precalculatedItems)
	{
		precalculatedItems = DLList_Create();
	}

	DLList_PushBack(precalculatedItems, newItem);	
}

void PdfPrecalculatedItem_Static_RemovePrecalculatedItem(struct PdfPrecalculatedItem *item)
{
	struct PdfPrecalculatedItem *precItem = NULL;
	struct DLListNode *iterator = NULL;

	if (precalculatedItems)
	{
		// TODO: Should remove item from list
		for (iterator = DLList_Begin(precalculatedItems); iterator != DLList_End(precalculatedItems); iterator = iterator->next)
		{
			precItem = (struct PdfPrecalculatedItem *)iterator->data;
			if (precItem == item)
			{
				// remove item from list. This will not delete precalculated item memory, just remove item from list
				DLList_Erase(precalculatedItems, iterator);
			}
		}

		if (precalculatedItems->size <= 0)
		{
			DLList_Destroy(precalculatedItems);
			precalculatedItems = NULL;
		}
	}
}

void PdfPrecalculatedItem_Static_AddValue(const char *dataStreamName, const char *columnName, float value)
{
	struct PdfPrecalculatedItem *precItem = NULL;
	struct DLListNode *iterator = NULL;
		
	if (precalculatedItems)
	{
		Logger_LogErrorMessage("PdfPrecalculatedItem_Static_AddValue: Add new value for stream %s, column: %s value: %3.3f", dataStreamName, columnName, value);

		// Call value on each precalculated item we have
		for (iterator = DLList_Begin(precalculatedItems); iterator != DLList_End(precalculatedItems); iterator = iterator->next)
		{
			precItem = (struct PdfPrecalculatedItem *)iterator->data;
			PdfPrecalculatedItem_AddValue(precItem, dataStreamName, columnName, value);		
		}
	}
}


DLLEXPORT_TEST_FUNCTION void PdfPrecalculatedItem_Destroy(struct PdfTemplateBalloonItem* self)
{
	PdfPrecalculatedItem_Static_RemovePrecalculatedItem(self);
	PdfPrecalculatedItem_Cleanup(self);
	MemoryManager_Free(self);
}

DLLEXPORT_TEST_FUNCTION void PdfPrecalculatedItem_Cleanup(struct PdfPrecalculatedItem* self)
{
	if(self->functions)
	{
		DLList_Destroy(self->functions);
		self->functions = NULL;
	}
	if (self->formula)
	{
		MemoryManager_Free(self->formula);
		self->formula = NULL;
	}
}

DLLEXPORT_TEST_FUNCTION void PdfPrecalculatedItem_AddValue(struct PdfPrecalculatedItem* self, const char *dataStreamName, const char *columnName, float value)
{
	struct PdfPrecalculatedFunction *precFunc = NULL;
	struct DLListNode *iterator = NULL;
	int found = 0;

	Logger_LogErrorMessage("PdfPrecalculatedItem_AddValue: Add new value for precalculated item; value: %3.3f", dataStreamName, columnName, value);


	for (iterator = DLList_Begin(self->functions); iterator != DLList_End(self->functions); iterator = iterator->next)
	{
		precFunc = (struct PdfPrecalculatedFunction *)iterator->data;
		if (strcmp(precFunc->dataStream, dataStreamName) == 0 && strcmp(precFunc->columnName, columnName) == 0)
		{
			PdfPrecalculatedFunction_AddValue(precFunc, value);
		}
	}
}

DLLEXPORT_TEST_FUNCTION char* PdfPrecalculatedItem_GetResult(struct PdfPrecalculatedItem *self)
{
	char* result;
	struct Parser *parser = NULL;

	PdfPrecalculatedItem_ReplaceFormulaWithResults(self);
	parser = Parser_Parser();
	result = (char *)MemoryManager_StrDup(Parser_parse(parser, self->parsedFormula));
	return result;	
}

int PdfPrecalculatedItem_Process(struct PdfTemplateBalloonItem *self, struct PdfGenerator *generator, struct PdfGeneratedBalloon *balloon, struct StreamWriter *streamWriter)
{
	struct PdfTextWriter *textWriter;
	struct PdfPrecalculatedItem *item;
	struct PdfFont *font;
	struct TransformationMatrix *transMat;
	struct Parser *tmpP = NULL;
	struct DLListNode *iterator;
	struct PdfPrecalculatedFunction *precFunction;
	char *result = NULL;

	Logger_LogNoticeMessage("PdfPrecalculatedItem_Process: BEGIN");
	item = (struct PdfPrecalculatedItem *)self;
	PdfPrecalculatedItem_ReplaceFormulaWithResults(item);
	tmpP = Parser_Parser();

	textWriter = PdfTextWriter_Begin(streamWriter);
	{	
		font = PdfDocument_FindFontBySaveId(generator->document, item->font->saveId);
		if (!font)
		{	
			Logger_LogErrorMessage("PdfTemplatePrecalculatedItem_Process: Font with saveId %d is missing", item->font->saveId);
			return FALSE;			
		}
		// add to resources
		PdfPageResources_AddFont(generator->currentPage->properties.resources, font);
		result = (char *)MemoryManager_StrDup(Parser_parse(tmpP, item->parsedFormula));
		PdfTextWriter_SetFontTemplated(textWriter, font, item->font);
		transMat = TransformationMatrix_Create();
		PdfTemplateBalloonItem_GetFullTransformation((struct PdfTemplateBalloonItem*)self, balloon, generator->currentPage, transMat, FALSE);
		PdfTextWriter_SetTextMatrix(textWriter, transMat->a, transMat->b, transMat->c, transMat->d, transMat->e, transMat->f);						
		Logger_LogNoticeMessage("PdfPrecalculatedItem_Process: Write result %s", result);
		PdfTextWriter_WriteText(textWriter, result);	

		MemoryManager_Free(result);
	}
	PdfTextWriter_EndText(textWriter);	

	// reset values for all assosicated functions

	for (iterator = DLList_Begin(item->functions); iterator != DLList_End(item->functions); iterator = iterator->next)
	{
		precFunction = (struct PdfPrecalculatedFunction *)iterator->data;
		PdfPrecalculatedFunction_ResetValue(precFunction);		
	}
	

	Logger_LogNoticeMessage("PdfPrecalculatedItem_Process: END");
	return TRUE;
}

void PdfPrecalculatedItem_ParseItem(struct PdfPrecalculatedItem *self, const char *value)
{
	char sourceStr[255];
	unsigned int size = 0, i, iOffset = 0;

	sourceStr[0] = '\0';
	sprintf(sourceStr, "%s", value);
	size = strlen(sourceStr);

	for(i=0; i<size; i++)
	{
		if(strchr("ABCDEFGHIJKLMNOPQRSTUVWXYZ",sourceStr[i]))
		{
			iOffset = PdfPrecalculatedItem_FindFunction(self, sourceStr, i);
			if(iOffset != -1)
			{
				i = iOffset;
			}
		}
	}
}

int PdfPrecalculatedItem_FindFunction(struct PdfPrecalculatedItem *self, char *input, int startOfString)
{
	char tmpInput[255];
	char tmpFind[10];
	char dataStreamName[100];
	char columnName[100];
	struct PdfPrecalculatedFunction *precFunc = NULL;

	tmpFind[0] = '\0';
	dataStreamName[0] = '\0';
	columnName[0] = '\0';
	tmpInput[0] = '\0';

	sprintf(tmpInput,"%s",input);

	if(tmpInput[startOfString + 3] == '(')
	{
		if (tmpInput[startOfString] == 'S' && tmpInput[startOfString + 1] == 'U' && tmpInput[startOfString + 2] == 'M')
		{
			sprintf(dataStreamName, "%s", GetName(tmpInput, startOfString + 4));
			sprintf(columnName, "%s", GetName(tmpInput, startOfString + 4 + strlen(dataStreamName) + 1));
			precFunc = PdfPrecalculatedFunction_Create(PDF_PRECALCULATED_FUNCTION_TYPE_SUM, dataStreamName, columnName);
			PdfPrecalculatedFunction_SetStringPosition(precFunc, startOfString, (startOfString + 4 + strlen(dataStreamName) + strlen(columnName) + 1));
			DLList_PushBack(self->functions,(void*)precFunc);
			precFunc = NULL;

			return (startOfString + 4 + strlen(dataStreamName) + strlen(columnName) + 1);
		}
		if (tmpInput[startOfString] == 'A' && tmpInput[startOfString + 1] == 'V' && tmpInput[startOfString + 2] == 'G')
		{
			sprintf(dataStreamName, "%s", GetName(tmpInput, startOfString + 4));
			sprintf(columnName, "%s", GetName(tmpInput, startOfString + 4 + strlen(dataStreamName) + 1));
			precFunc = PdfPrecalculatedFunction_Create(PDF_PRECALCULATED_FUNCTION_TYPE_AVG, dataStreamName, columnName);
			PdfPrecalculatedFunction_SetStringPosition(precFunc, startOfString, startOfString + 4 + strlen(dataStreamName) + strlen(columnName) + 1);
			DLList_PushBack(self->functions,(void*)precFunc);
			precFunc = NULL;

			return (startOfString + 4 + strlen(dataStreamName) + strlen(columnName) + 1);
		}
		if (tmpInput[startOfString] == 'M' && tmpInput[startOfString + 1] == 'I' && tmpInput[startOfString + 2] == 'N')
		{
			sprintf(dataStreamName, "%s", GetName(tmpInput, startOfString + 4));
			sprintf(columnName, "%s", GetName(tmpInput, startOfString + 4 + strlen(dataStreamName) + 1));
			precFunc = PdfPrecalculatedFunction_Create(PDF_PRECALCULATED_FUNCTION_TYPE_MIN, dataStreamName, columnName);
			PdfPrecalculatedFunction_SetStringPosition(precFunc, startOfString, startOfString + 4 + strlen(dataStreamName) + strlen(columnName) + 1);
			DLList_PushBack(self->functions,(void*)precFunc);
			precFunc = NULL;

			return (startOfString + 4 + strlen(dataStreamName) + strlen(columnName) + 1);
		}
		if (tmpInput[startOfString] == 'M' && tmpInput[startOfString + 1] == 'A' && tmpInput[startOfString + 2] == 'X')
		{
			sprintf(dataStreamName, "%s", GetName(tmpInput, startOfString + 4));
			sprintf(columnName, "%s", GetName(tmpInput, startOfString + 4 + strlen(dataStreamName) + 1));
			precFunc = PdfPrecalculatedFunction_Create(PDF_PRECALCULATED_FUNCTION_TYPE_MAX, dataStreamName, columnName);
			PdfPrecalculatedFunction_SetStringPosition(precFunc, startOfString, startOfString + 4 + strlen(dataStreamName) + strlen(columnName) + 1);
			DLList_PushBack(self->functions,(void*)precFunc);
			precFunc = NULL;

			return (startOfString + 4 + strlen(dataStreamName) + strlen(columnName) + 1);
		}
	}

	if(tmpInput[startOfString + 5] == '(')
	{
		if (tmpInput[startOfString]     == 'C' && tmpInput[startOfString + 1] == 'O' && tmpInput[startOfString + 2] == 'U' &&\
			tmpInput[startOfString + 3] == 'N' && tmpInput[startOfString + 4] == 'T')
		{
			sprintf(dataStreamName, "%s", GetName(tmpInput, startOfString + 6));
			sprintf(columnName, "%s", GetName(tmpInput, startOfString + 6 + strlen(dataStreamName) + 1));
			precFunc = PdfPrecalculatedFunction_Create(PDF_PRECALCULATED_FUNCTION_TYPE_COUNT, dataStreamName, columnName);
			PdfPrecalculatedFunction_SetStringPosition(precFunc, startOfString, startOfString + 6 + strlen(dataStreamName) + strlen(columnName) + 1);
			DLList_PushBack(self->functions,(void*)precFunc);
			precFunc = NULL;

			return (startOfString + 6 + strlen(dataStreamName) + strlen(columnName) + 1);
		}
	}

	if(tmpInput[startOfString + 8] == '(')
	{
		if (tmpInput[startOfString]     == 'D' && tmpInput[startOfString + 1] == 'E' && tmpInput[startOfString + 2] == 'C' && \
			tmpInput[startOfString + 3] == 'T' && tmpInput[startOfString + 4] == 'O' &&\
			tmpInput[startOfString + 5] == 'H' && tmpInput[startOfString + 6] == 'E' && tmpInput[startOfString + 7] == 'X')
		{
			sprintf(dataStreamName, "%s", GetName(tmpInput, startOfString + 9));
			sprintf(columnName, "%s", GetName(tmpInput, startOfString + 9 + strlen(dataStreamName) + 1));
			precFunc = PdfPrecalculatedFunction_Create(PDF_PRECALCULATED_FUNCTION_TYPE_DECTOHEX, dataStreamName, columnName);
			PdfPrecalculatedFunction_SetStringPosition(precFunc, startOfString, startOfString + 9 + strlen(dataStreamName) + strlen(columnName) + 1);
			DLList_PushBack(self->functions,(void*)precFunc);
			precFunc = NULL;

			return (startOfString + 9 + strlen(dataStreamName) + strlen(columnName) + 1);
		}
		if (tmpInput[startOfString]     == 'D' && tmpInput[startOfString + 1] == 'E' && tmpInput[startOfString + 2] == 'C' && \
			tmpInput[startOfString + 3] == 'T' && tmpInput[startOfString + 4] == 'O' &&\
			tmpInput[startOfString + 5] == 'B' && tmpInput[startOfString + 6] == 'I' && tmpInput[startOfString + 7] == 'N')
		{
			sprintf(dataStreamName, "%s", GetName(tmpInput, startOfString + 9));
			sprintf(columnName, "%s", GetName(tmpInput, startOfString + 9 + strlen(dataStreamName) + 1));
			precFunc = PdfPrecalculatedFunction_Create(PDF_PRECALCULATED_FUNCTION_TYPE_DECTOBIN, dataStreamName, columnName);
			PdfPrecalculatedFunction_SetStringPosition(precFunc, startOfString, startOfString + 9 + strlen(dataStreamName) + strlen(columnName) + 1);
			DLList_PushBack(self->functions,(void*)precFunc);
			precFunc = NULL;

			return (startOfString + 9 + strlen(dataStreamName) + strlen(columnName) + 1);
		}
	}

	
	return -1;
}

char *GetName(char *inputStr, int offset)
{
	char ret[255];
	char tmpStr[255];
	int count = 0;
	char *result;

	ret[0] = '\0';
	tmpStr[0] = '\0';

	sprintf(tmpStr, "%s", inputStr);

	while(1)
	{
		if (tmpStr[offset + count] == '.' || tmpStr[offset + count] == ')')
		{
			break;
		}
		if (count > 254)
		{
			ret[0] = '\0';
			break;
		}

		ret[count] = tmpStr[offset + count];

		count++;
	}

	if(strlen(ret) != 0)
	{
		ret[count] = '\0';
	}

	result = MemoryManager_StrDup(ret);
	return result;
}

void PdfPrecalculatedItem_ReplaceFormulaWithResults(struct PdfPrecalculatedItem *self)
{
	unsigned int i = 0, j = 0, len = 0, len2 = 0, count = 0;
	char functValue[20];
	struct PdfPrecalculatedFunction *funct = NULL;

	len = strlen(self->formula);
	functValue[0] = '\0';

	for(i = 0; i < len; i++)
	{
		funct = PdfPrecalculatedItem_GetFunction(self, i);
		if(funct)
		{
			PdfPrecalculatedFunction_GetValue(funct, functValue);
			len2 = strlen(functValue);
			for(j = 0; j < len2; j++)
			{
				self->parsedFormula[count] = functValue[j];
				count++;
			}
			i = funct->end;
			funct = NULL;
		}else{
			self->parsedFormula[count] = self->formula[i];
			count++;
		}
	}

	self->parsedFormula[count] = '\0';
}

struct PdfPrecalculatedFunction* PdfPrecalculatedItem_GetFunction(struct PdfPrecalculatedItem *self, int start)
{
	struct DLListNode *iterator = NULL;
	struct PdfPrecalculatedFunction *tmpFunc;

	for (iterator = DLList_Begin(self->functions); iterator != DLList_End(self->functions); iterator = iterator->next)
	{
		tmpFunc = (struct PdfPrecalculatedFunction*)iterator->data;
		if(tmpFunc->start == start)
		{
			return tmpFunc;
		}
	}

	return NULL;
}