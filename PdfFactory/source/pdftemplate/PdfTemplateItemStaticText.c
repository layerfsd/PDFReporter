
#include "PdfTemplateItemStaticText.h"
#include "PdfTemplateElements.h"
#include "PdfTemplateFont.h"
#include "PdfTemplateEmbeddedFont.h"
#include "PdfTemplate.h"
#include "PdfGenerator.h"
#include "PdfTemplateBalloon.h"
#include "PdfTextWriter.h"
#include "PdfPage.h"
#include "PdfFont.h"
#include "UnitConverter.h"
#include "MemoryManager.h"
#include "TransformationMatrix.h"
#include "PdfGeneratedBalloon.h"
#include "PdfPageResources.h"
#include "Logger.h"
#include <libxml/parser.h>
#include <libxml/xmlmemory.h>


DLLEXPORT_TEST_FUNCTION struct PdfTemplateItemStaticText* PdfTemplateItemStaticText_CreateFromXml(xmlNode *node)
{
	struct PdfTemplateItemStaticText *ret;
	xmlNode *foundNode;

	ret = (struct PdfTemplateItemStaticText *)MemoryManager_Alloc(sizeof(struct PdfTemplateItemStaticText));

	PdfTemplateBalloonItem_InitFromXml((struct PdfTemplateBalloonItem*)ret, node, PdfTemplateItemStaticText_Destroy, PdfTemplateItemStaticText_Process);
	
	foundNode = PdfTemplate_FindNode(node, FONT);
	if (foundNode)
	{
		ret->font = PdfTemplateFont_CreateFromXml(foundNode);	
	}
	else
	{
		Logger_LogErrorMessage("PdfTemplateItemStaticText_CreateFromXml: Font node is not found");
		PdfTemplateItemStaticText_Destroy((struct PdfTemplateBalloonItem*)ret);
		return NULL;
	}
	foundNode = PdfTemplate_FindNode(node, TEXT);
	if (foundNode)
	{
		ret->text = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(foundNode, VALUE));	
	}
	else
	{
		Logger_LogErrorMessage("PdfTemplateItemStaticText_CreateFromXml: Text node is not found");
		PdfTemplateItemStaticText_Destroy((struct PdfTemplateBalloonItem*)ret);
		return NULL;		
	}		

	return ret;
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemStaticText_Cleanup(struct PdfTemplateItemStaticText* self)
{
	// cleanup this code
	if (self->text)
	{
		MemoryManager_Free(self->text);
		self->text = 0;
	}
	if (self->font)
	{
		PdfTemplateFont_Destroy(self->font);
		self->font = 0;
	}

	// cleanup parent
	PdfTemplateBalloonItem_Cleanup((struct PdfTemplateBalloonItem*)self);
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemStaticText_Destroy(struct PdfTemplateBalloonItem* self)
{
	PdfTemplateItemStaticText_Cleanup((struct PdfTemplateItemStaticText*)self);

	// destroy 
	MemoryManager_Free((struct PdfTemplateItemStaticText*)self);
}


/************************************************************************/
/* Method that will document itself. All needed stuff is taken from generator
/************************************************************************/
int PdfTemplateItemStaticText_Process(struct PdfTemplateBalloonItem *self, struct PdfGenerator *generator, struct PdfGeneratedBalloon *generatedBalloon, struct StreamWriter *streamWriter)
{
	struct PdfTextWriter *textWriter;
	struct PdfFont *font;
	struct PdfTemplateItemStaticText *item; 	
	struct TransformationMatrix *transMat;
	
	Logger_LogNoticeMessage("PdfTemplateItemStaticText_Process: BEGIN");
	// cast balloon item
	item = (struct PdfTemplateItemStaticText*)self;

	// write info to pdf page
	textWriter = PdfTextWriter_Begin(streamWriter);
	{	
		font = PdfDocument_FindFontBySaveId(generator->document, item->font->saveId);
		if (!font)
		{	
			Logger_LogErrorMessage("PdfTemplateItemStaticText_Process: Font with saveId %d is missing", item->font->saveId);
			return FALSE;			
		}
		// add to resources
		PdfPageResources_AddFont(generator->currentPage->properties.resources, font);


		PdfTextWriter_SetFontTemplated(textWriter, font, item->font);
		transMat = TransformationMatrix_Create();
		PdfTemplateBalloonItem_GetFullTransformation((struct PdfTemplateBalloonItem*)self, generatedBalloon, generator->currentPage, transMat, FALSE);
		PdfTextWriter_SetTextMatrix(textWriter, transMat->a, transMat->b, transMat->c, transMat->d, transMat->e, transMat->f);		
		PdfTextWriter_WriteText(textWriter, item->text);		
		Logger_LogNoticeMessage("PdfTemplateItemStaticText_Process: Wrote result: %s", item->text);
	}
	PdfTextWriter_EndText(textWriter);	

	Logger_LogNoticeMessage("PdfTemplateItemStaticText_Process: END");
	return TRUE;
}
