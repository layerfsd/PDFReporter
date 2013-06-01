#include "PdfTemplateItemDateTime.h"
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

DLLEXPORT_TEST_FUNCTION struct PdfTemplateItemDateTime* PdfTemplateItemDateTime_CreateFromXml(xmlNode *node)
{
	struct PdfTemplateItemDateTime *ret;
	xmlNode *foundNode;

	ret = (struct PdfTemplateItemDateTime *)MemoryManager_Alloc(sizeof(struct PdfTemplateItemDateTime));

	PdfTemplateBalloonItem_InitFromXml((struct PdfTemplateBalloonItem*)ret, node, PdfTemplateItemDateTime_Destroy, PdfTemplateItemDateTime_Process);
	
	foundNode = PdfTemplate_FindNode(node, FONT);
	if (foundNode)
	{
		ret->font = PdfTemplateFont_CreateFromXml(foundNode);	
	}
	else
	{
		Logger_LogErrorMessage("PdfTemplateItemDateTime_CreateFromXml: Font node is not found");
		PdfTemplateItemDateTime_Destroy((struct PdfTemplateBalloonItem*)ret);
		return NULL;
	}


	foundNode = PdfTemplate_FindNode(node, FORMAT);
	if (foundNode)
	{
		ret->format = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(foundNode, VALUE));	
	}
	else
	{
		Logger_LogErrorMessage("PdfTemplateItemDateTime_CreateFromXml: Format node is not found");
		PdfTemplateItemDateTime_Destroy((struct PdfTemplateBalloonItem*)ret);
		return NULL;
	}

	ret->text = 0;
	PdfTemplateItemDateTime_Init(ret);

	return ret;
}


DLLEXPORT_TEST_FUNCTION void PdfTemplateItemDateTime_Init(struct PdfTemplateItemDateTime* self)
{
	struct tm time;
	
	//initialize
	__int64 itime;
	_time64(&itime);

	//pickup time
	_localtime64_s(&time, &itime);

	//parse from format
	PdfTemplateItemDateTime_MakeText(self, &time);
	//MemoryManager_Free(&time);
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemDateTime_Destroy(struct PdfTemplateItemDateTime* self)
{
	PdfTemplateItemDateTime_Cleanup(self);
	MemoryManager_Free(self);
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemDateTime_Cleanup(struct PdfTemplateItemDateTime* self)
{
	if(self->font)
	{
		PdfTemplateFont_Destroy(self->font);
	}
	if(self->text)
	{
		MemoryManager_Free(self->text);
		self->text = 0;
	}
	if(self->format)
	{
		MemoryManager_Free(self->format);
		self->format = 0;
	}
}

//DateTime parser...
void PdfTemplateItemDateTime_MakeText(struct PdfTemplateItemDateTime *self, struct tm *time)
{
	char *out;
	char outFormat[200];
	unsigned int size;
	unsigned int i;
	char isParsed;
	char tmpChr[2];

	size = strlen(self->format);
	isParsed = 0;
	outFormat[0] = '\0';
	tmpChr[0] = '\0';
	out = MemoryManager_Alloc(200);

	for(i=0; i<size; i++)
	{
		if(self->format[i] == 'd')
		{
			if(self->format[i+1] == 'd' && self->format[i+2] == 'd' && self->format[i+3] == 'd')
			{
				strcat(outFormat,"%A");
				i+=3;
				isParsed = 1;
			}else if(self->format[i+1] == 'd')
			{
				strcat(outFormat,"%d");
				i+=1;
				isParsed = 1;
			}
		}

		if(self->format[i] == 'M')
		{
			if(self->format[i+1] == 'M' && self->format[i+2] == 'M' && self->format[i+3] == 'M')
			{
				strcat(outFormat,"%B");
				i+=3;
				isParsed = 1;
			}else if(self->format[i+1] == 'M')
			{
				strcat(outFormat,"%m");
				i+=1;
				isParsed = 1;
			}
		}

		if(self->format[i] == 'y')
		{
			if(self->format[i+1] == 'y' && self->format[i+2] == 'y' && self->format[i+3] == 'y')
			{
				strcat(outFormat,"%Y");
				i+=3;
				isParsed = 1;
			}else if(self->format[i+1] == 'y')
			{
				strcat(outFormat,"%y");
				i+=1;
				isParsed = 1;
			}
		}

		if(self->format[i] == 'H')
		{
			if(self->format[i+1] == 'H')
			{
				strcat(outFormat,"%H");
				i+=1;
				isParsed = 1;
			}
		}

		if(self->format[i] == 'm')
		{
			if(self->format[i+1] == 'm')
			{
				strcat(outFormat,"%M");
				i+=1;
				isParsed = 1;
			}
		}

		if(self->format[i] == 's')
		{
			if(self->format[i+1] == 's')
			{
				strcat(outFormat,"%S");
				strcat(outFormat, &tmpChr);
				i+=1;
				isParsed = 1;
			}
		}

		if(!isParsed)
		{
			MemoryManager_MemCpy(&tmpChr, self->format + i, 1);
			tmpChr[1] = '\0';
			strcat(outFormat, &tmpChr);
			tmpChr[0] = '\0';
		}

		isParsed = 0;
	}

	size = strftime(out,200,outFormat,time);
	self->text = MemoryManager_StrDup(out);
	//MemoryManager_MemCpy(self->text, out, size);
	//tmpChr[0] = '\0';
	//MemoryManager_MemCpy(self->text + size, tmpChr, 1);
	MemoryManager_Free(out);
}




int PdfTemplateItemDateTime_Process(struct PdfTemplateBalloonItem *self, struct PdfGenerator *generator, struct PdfGeneratedBalloon *balloon, struct StreamWriter *streamWriter)
{
	struct PdfTextWriter *textWriter;
	struct PdfFont *font;
	struct PdfTemplateItemDateTime *item; 	
	struct TransformationMatrix *transMat;

	Logger_LogNoticeMessage("PdfTemplateItemDateTime_Process: BEGIN");
	// cast balloon item
	item = (struct PdfTemplateItemDateTime*)self;

	// write info to pdf page
	textWriter = PdfTextWriter_Begin(streamWriter);
	{	
		font = PdfDocument_FindFontBySaveId(generator->document, item->font->saveId);
		if (!font)
		{	
			Logger_LogErrorMessage("PdfTemplateItemDateTime_Process: Font with saveId %d is missing", item->font->saveId);
			return FALSE;			
		}		
		// add to resources
		PdfPageResources_AddFont(generator->currentPage->properties.resources, font);


		PdfTextWriter_SetFontTemplated(textWriter, font, item->font);
		transMat = TransformationMatrix_Create();
		PdfTemplateBalloonItem_GetFullTransformation((struct PdfTemplateBalloonItem*)self, balloon, generator->currentPage, transMat, FALSE);
		PdfTextWriter_SetTextMatrix(textWriter, transMat->a, transMat->b, transMat->c, transMat->d, transMat->e, transMat->f);		
		PdfTextWriter_WriteText(textWriter, item->text);		
		Logger_LogNoticeMessage("PdfTemplateItemDateTime_Process: Wrote result %s", item->text);
	}
	PdfTextWriter_EndText(textWriter);	


	Logger_LogNoticeMessage("PdfTemplateItemDateTime_Process: END");
	return TRUE;
}
