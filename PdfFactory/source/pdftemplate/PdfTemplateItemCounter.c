#include "PdfTemplateItemCounter.h"
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

DLLEXPORT_TEST_FUNCTION struct PdfTemplateItemCounter* PdfTemplateItemCounter_CreateFromXml(xmlNode *node)
{
	struct PdfTemplateItemCounter *ret;
	xmlNode *foundNode;

	ret = (struct PdfTemplateItemCounter *)MemoryManager_Alloc(sizeof(struct PdfTemplateItemCounter));

	PdfTemplateBalloonItem_InitFromXml((struct PdfTemplateBalloonItem*)ret, node, PdfTemplateItemCounter_Destroy, PdfTemplateItemCounter_Process);
	
	foundNode = PdfTemplate_FindNode(node, FONT);
	if (foundNode)
	{
		ret->font = PdfTemplateFont_CreateFromXml(foundNode);	
	}
	else
	{
#ifdef _DEBUG
		printf("PdfTemplateItemCounter: CreateFromXml: Font node is not found\n");
#endif
		PdfTemplateItemCounter_Destroy((struct PdfTemplateBalloonItem*)ret);
		return NULL;
	}


	foundNode = PdfTemplate_FindNode(node, FORMAT);
	if (foundNode)
	{
		ret->format = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(foundNode, VALUE));	
	}
	else
	{
#ifdef _DEBUG
		printf("PdfTemplateItemCounter: CreateFromXml: Format node is not found\n");
#endif
		PdfTemplateItemCounter_Destroy((struct PdfTemplateBalloonItem*)ret);
		return NULL;
	}


	ret->start = PdfTemplate_LoadIntValue(node, START);
	ret->end = PdfTemplate_LoadIntValue(node, END);
	ret->interval = PdfTemplate_LoadIntValue(node, INTERVAL);
	ret->loop = PdfTemplate_LoadBooleanValue(node, LOOP);
	ret->counter = ret->start;
	ret->text = 0;

	return ret;
}




DLLEXPORT_TEST_FUNCTION void PdfTemplateItemCounter_Destroy(struct PdfTemplateItemCounter* self)
{
	PdfTemplateItemCounter_Cleanup(self);
	MemoryManager_Free(self);
}




DLLEXPORT_TEST_FUNCTION void PdfTemplateItemCounter_Cleanup(struct PdfTemplateItemCounter* self)
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



DLLEXPORT_TEST_FUNCTION void PdfTemplateItemCounter_Update(struct PdfTemplateItemCounter* self)
{
	self->counter += self->interval;

	if(self->end != 0)
	{
		if(self->interval > 0)
		{
			if(self->counter > self->end)
			{
				if(self->loop)
				{
					self->counter = self->start;
				}else{
					self->counter = self->end;
				}
			}
		}else if(self->interval < 0)
		{
			if(self->counter < self->end)
			{
				if(self->loop)
				{
					self->counter = self->start;
				}else{
					self->counter = self->end;
				}
			}
		}
	}
}



int PdfTemplateItemCounter_Process(struct PdfTemplateBalloonItem *self, struct PdfGenerator *generator, struct PdfGeneratedBalloon *balloon, struct StreamWriter *streamWriter)
{
	struct PdfTextWriter *textWriter;
	struct PdfFont *font;
	struct PdfTemplateItemCounter *item; 	
	struct TransformationMatrix *transMat;

	// make text from format...
	PdfTemplateItemCounter_MakeText(self);
	
	// cast balloon item
	item = (struct PdfTemplateItemCounter*)self;

	// write info to pdf page
	textWriter = PdfTextWriter_Begin(streamWriter);
	{	
		font = PdfDocument_FindFontBySaveId(generator->document, item->font->saveId);
		if (!font)
		{	
			Logger_LogErrorMessage("PdfTemplateItemCounter_Process: Font with saveId %d is missing", item->font->saveId);
			return FALSE;			
		}		
		// add to resources
		PdfPageResources_AddFont(generator->currentPage->properties.resources, font);


		PdfTextWriter_SetFontTemplated(textWriter, font, item->font);
		transMat = TransformationMatrix_Create();
		PdfTemplateBalloonItem_GetFullTransformation((struct PdfTemplateBalloonItem*)self, balloon, generator->currentPage, transMat, FALSE);
		PdfTextWriter_SetTextMatrix(textWriter, transMat->a, transMat->b, transMat->c, transMat->d, transMat->e, transMat->f);		
		PdfTextWriter_WriteText(textWriter, item->text);		
	}
	PdfTextWriter_EndText(textWriter);	
	PdfTemplateItemCounter_Update(self);

	return TRUE;
}


DLLEXPORT_TEST_FUNCTION void PdfTemplateItemCounter_MakeText(struct PdfTemplateItemCounter* self)
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
			if(self->format[i] == 'C')
			{
				sprintf(tmpReplaceText,"%d",self->counter);
				strcat(tmpText, tmpReplaceText);
			}
			if(self->format[i] == 'S')
			{
				sprintf(tmpReplaceText,"%d",self->start);
				strcat(tmpText, tmpReplaceText);
			}
			if(self->format[i] == 'E')
			{
				sprintf(tmpReplaceText,"%d",self->end);
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