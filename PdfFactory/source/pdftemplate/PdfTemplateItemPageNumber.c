#include "PdfTemplateItemPageNumber.h"
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


DLLEXPORT_TEST_FUNCTION struct PdfTemplateItemPageNumber* PdfTemplateItemPageNumber_CreateFromXml(xmlNode *node)
{
	struct PdfTemplateItemPageNumber *ret;
	xmlNode *foundNode;

	ret = (struct PdfTemplateItemPageNumber *)MemoryManager_Alloc(sizeof(struct PdfTemplateItemPageNumber));

	PdfTemplateBalloonItem_InitFromXml((struct PdfTemplateBalloonItem*)ret, node, PdfTemplateItemPageNumber_Destroy, PdfTemplateItemPageNumber_Process);

	foundNode = PdfTemplate_FindNode(node, FONT);
	if (foundNode)
	{
		ret->font = PdfTemplateFont_CreateFromXml(foundNode);	
	}
	else
	{
		Logger_LogErrorMessage("PdfTemplateItemPageNumber_CreateFromXml: Font node is not found");
		PdfTemplateItemPageNumber_Destroy((struct PdfTemplateBalloonItem*)ret);
		return NULL;
	}


	foundNode = PdfTemplate_FindNode(node, FORMAT);
	if (foundNode)
	{
		ret->format = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(foundNode, VALUE));	
	}
	else
	{
		Logger_LogErrorMessage("PdfTemplateItemPageNumber_CreateFromXml: Format node is not found");
		PdfTemplateItemPageNumber_Destroy((struct PdfTemplateBalloonItem*)ret);
		return NULL;
	}
	
	ret->text = 0;
	return ret;
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemPageNumber_Destroy(struct PdfTemplateItemPageNumber* self)
{
	PdfTemplateItemPageNumber_Cleanup(self);
	MemoryManager_Free(self);
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemPageNumber_Cleanup(struct PdfTemplateItemPageNumber* self)
{
	if(self->font)
	{
		PdfTemplateFont_Destroy(self->font);
		self->font = 0;
	}
	if(self->format)
	{
		MemoryManager_Free(self->format);
		self->format = 0;
	}
	if(self->text)
	{
		MemoryManager_Free(self->text);
		self->text = 0;
	}
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemPageNumber_Update(struct PdfTemplateItemPageNumber* self)
{
	
}

int PdfTemplateItemPageNumber_Process(struct PdfTemplateBalloonItem *self, struct PdfGenerator *generator, struct PdfGeneratedBalloon *balloon, struct StreamWriter *streamWriter)
{
	struct PdfTextWriter *textWriter;
	struct PdfFont *font;
	struct PdfTemplateItemPageNumber *item; 	
	struct TransformationMatrix *transMat;

	Logger_LogNoticeMessage("PdfTemplateItemPageNumber_Process: BEGIN");
	// make text from format...
	PdfTemplateItemPageNumber_MakeText(self, generator);

	// cast balloon item
	item = (struct PdfTemplateItemPageNumber*)self;

	// write info to pdf page
	textWriter = PdfTextWriter_Begin(streamWriter);
	{	
		font = PdfDocument_FindFontBySaveId(generator->document, item->font->saveId);
		if (!font)
		{	
			Logger_LogErrorMessage("PdfTemplateItemPageNumber_Process: Font with saveId %d is missing", item->font->saveId);
			return FALSE;			
		}		
		// add to resources
		PdfPageResources_AddFont(generator->currentPage->properties.resources, font);

		PdfTextWriter_SetFontTemplated(textWriter, font, item->font);
		transMat = TransformationMatrix_Create();
		PdfTemplateBalloonItem_GetFullTransformation((struct PdfTemplateBalloonItem*)self, balloon, generator->currentPage, transMat, FALSE);
		PdfTextWriter_SetTextMatrix(textWriter, transMat->a, transMat->b, transMat->c, transMat->d, transMat->e, transMat->f);		
		PdfTextWriter_WriteText(textWriter, item->text);		
		Logger_LogNoticeMessage("PdfTemplateItemPageNumber_Process: Wrote result %s", item->text);
	}
	PdfTextWriter_EndText(textWriter);	

	Logger_LogNoticeMessage("PdfTemplateItemPageNumber_Process: END");
	return TRUE;
}


DLLEXPORT_TEST_FUNCTION void PdfTemplateItemPageNumber_MakeText(struct PdfTemplateItemPageNumber* self, struct PdfGenerator* generator)
{
	char tmpText[100];
	char tmpReplaceText[20];
	char tmpChr[2];
	char replace, done;
	unsigned int i;
	unsigned int size;

	replace = 0;
	done = 0;
	size = strlen(self->format);
	tmpText[0] = '\0';
	tmpChr[0] = '\0';

	for(i=0; i<size; i++)
	{
		if(replace == 1)
		{
			if(self->format[i] == 'P')
			{
				sprintf(tmpReplaceText,"%d", generator->pageNumber);
				strcat(tmpText, tmpReplaceText);
			}
			if(self->format[i] == 'p')
			{
				sprintf(tmpReplaceText," "); //This need to be changed when Number of pages is madded
				strcat(tmpText, tmpReplaceText);
			}
		}

		if(self->format[i] == '{')
		{
			replace = 1;
		}

		if(self->format[i] == '}')
		{ 
			done = 1; 
		}else if(replace == 0){
			MemoryManager_MemCpy(&tmpChr, self->format + i, 1);
			tmpChr[1] = '\0';
			strcat(tmpText, &tmpChr);
			tmpChr[0] = '\0';
		}

		if(done == 1) { replace = 0; done = 0; }
	}

	if(self->text){ MemoryManager_Free(self->text); self->text = 0; }

	self->text = MemoryManager_StrDup(tmpText);
}