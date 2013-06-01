
#include "PdfTemplateItemDynamicText.h"
#include "PdfTemplateElements.h"
#include "PdfPrecalculatedItem.h"
#include "PdfTemplateFont.h"
#include "PdfTemplateEmbeddedFont.h"
#include "PdfTemplate.h"
#include "PdfGenerator.h"
#include "PdfTemplateBalloon.h"
#include "PdfGeneratorDataStream.h"
#include "PdfGeneratorDataStreamRow.h"
#include "PdfTextWriter.h"
#include "UnitConverter.h"
#include "PdfPage.h"
#include "PdfFont.h"
#include <libxml/parser.h>
#include <libxml/xmlmemory.h>
#include "MemoryManager.h"
#include "TransformationMatrix.h"
#include "PdfGeneratedBalloon.h"
#include "PdfPageResources.h"
#include "GraphicWriter.h"
#include "Logger.h"

DLLEXPORT_TEST_FUNCTION struct PdfTemplateItemDynamicText* PdfTemplateItemDynamicText_CreateFromXml(xmlNode *node)
{
	struct PdfTemplateItemDynamicText *ret;
	xmlNode *foundNode;

	ret = (struct PdfTemplateItemDynamicText *)MemoryManager_Alloc(sizeof(struct PdfTemplateItemDynamicText));

	PdfTemplateBalloonItem_InitFromXml((struct PdfTemplateBalloonItem*)ret, node, PdfTemplateItemDynamicText_Destroy, PdfTemplateItemDynamicText_Process);

	foundNode = PdfTemplate_FindNode(node, FONT);
	if (foundNode)
	{
		ret->font = PdfTemplateFont_CreateFromXml(foundNode);	
	}
	else
	{
		Logger_LogErrorMessage("PdfTemplateItemDynamicTExt_CreateFromXml: Font not is not found");
		PdfTemplateItemDynamicText_Destroy((struct PdfTemplateBalloonItem*)ret);
		return NULL;
	}
	foundNode = PdfTemplate_FindNode(node, TEXT);
	if (foundNode)
	{
		ret->sourceColumn = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(foundNode, SOURCE_COLUMN));	
		ret->dataSourceName = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(foundNode, DATA_STREAM));	
	}
	else
	{
		Logger_LogErrorMessage("PdfTemplateItemDynamicTExt_CreateFromXml: Text node is not found");
		PdfTemplateItemDynamicText_Destroy((struct PdfTemplateBalloonItem*)ret);
		return NULL;		
	}


	foundNode = PdfTemplate_FindNode(node, ROTATION);
	if (foundNode)
	{
		ret->angle = PdfTemplate_LoadDoubleAttribute(foundNode, ANGLE);
	}
	else
	{
		ret->angle = 0;
	}

	return ret;
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemDynamicText_Cleanup(struct PdfTemplateItemDynamicText* self)
{
	// cleanup this code
	if (self->sourceColumn)
	{
		MemoryManager_Free(self->sourceColumn);
		self->sourceColumn = 0;
	}
	if (self->font)
	{
		PdfTemplateFont_Destroy(self->font);
		self->font = 0;
	}

	// cleanup parent
	PdfTemplateBalloonItem_Cleanup((struct PdfTemplateBalloonItem*)self);
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemDynamicText_Destroy(struct PdfTemplateBalloonItem* self)
{
	PdfTemplateItemDynamicText_Cleanup((struct PdfTemplateItemDynamicText*)self);

	// destroy 
	MemoryManager_Free((struct PdfTemplateItemDynamicText*)self);
}

int PdfTemplateItemDynamicText_Process(struct PdfTemplateBalloonItem *self, struct PdfGenerator *generator, struct PdfGeneratedBalloon *balloon, struct StreamWriter *streamWriter)
{
	struct PdfTextWriter *textWriter;
	struct PdfFont *font;
	struct PdfTemplateItemDynamicText *item; 	
	struct TransformationMatrix *transMat;
	struct PdfGraphicWriter *graphicWriter;
	char *value;
	float fValue = 0;
	int dataSize = 0;
	int i;
	char *tmpValue;

	Logger_LogNoticeMessage("PdfTemplateItemDynamicText_Process: BEGIN");
	
	// cast balloon item
	item = (struct PdfTemplateItemDynamicText*)self;

	// request for data for this column
	if (generator->requestDataCallback)
	{
		Logger_LogNoticeMessage("PdfTemplateItemDynamicText_Process: Requesting data for data source: %s, column: %s", item->dataSourceName, item->sourceColumn);
 		value = generator->requestDataCallback(item->dataSourceName, item->sourceColumn, &dataSize);		
		Logger_LogNoticeMessage("PdfTemplateItemDynamicText_Process: Requesting data for data source: %s, column: %s RETRIEVED", item->dataSourceName, item->sourceColumn);

		if (!value)
		{
			Logger_LogErrorMessage("PdfTemplateItemDynamicText_Process: Failed getting data for data source: %s, column: %s FAILED", item->dataSourceName, item->sourceColumn);
			return FALSE;
		}

		// As we are using dataStream and column name Send this to PrecalculatedItem
		tmpValue = MemoryManager_StrDup(value);
		for(i = 0; i < strlen(tmpValue); i++)
		{
			if (tmpValue[i] == ',')
			{
				tmpValue[i] = '.';
			}
		}
		fValue = (float)atof(tmpValue);
		PdfPrecalculatedItem_Static_AddValue(item->dataSourceName, item->sourceColumn, fValue);
		MemoryManager_Free(tmpValue);
	}
	
	
	// write info to pdf page
	textWriter = PdfTextWriter_Begin(streamWriter);
	{	
		font = PdfDocument_FindFontBySaveId(generator->document, item->font->saveId);
		if (!font)
		{	
			Logger_LogErrorMessage("PdfTemplateItemDynamicText_Process: Font with saveId %d is missing", item->font->saveId);		
			return FALSE;			
		}		

		// add to resources
		PdfPageResources_AddFont(generator->currentPage->properties.resources, font);			

	
		PdfTextWriter_SetFontTemplated(textWriter, font, item->font);

		transMat = TransformationMatrix_Create();
		PdfTemplateBalloonItem_GetFullTransformation((struct PdfTemplateBalloonItem*)self, balloon, generator->currentPage, transMat, FALSE);
		PdfTextWriter_SetTextMatrix(textWriter, transMat->a, transMat->b, transMat->c, transMat->d, transMat->e, transMat->f);				
		if (value)
		{
			Logger_LogNoticeMessage("PdfTemplateItemDynamicText_Process: Writing result %s", value);
			PdfTextWriter_WriteText(textWriter, value);				
		}
		else 
		{
			Logger_LogNoticeMessage("PdfTemplateItemDynamicText_Process: Writing Not defined result");
			PdfTextWriter_WriteText(textWriter, "NOT DEFINED!");		
		}

		
	}
	PdfTextWriter_EndText(textWriter);	
	Logger_LogNoticeMessage("PdfTemplateItemDynamicText_Process: END");
	
	return TRUE;
}