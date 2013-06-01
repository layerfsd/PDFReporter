/*
PdfTemplateFont.c

Author: Marko Vranjkovic
Date: 21.08.2008.	

*/

#include "PdfFactory.h"
#include "PdfTemplateFont.h"
#include "PdfTemplateElements.h"
#include "PdfTemplate.h"
#include "PdfTemplateEmbeddedFont.h"
#include "MemoryManager.h"


DLLEXPORT_TEST_FUNCTION struct PdfTemplateFont* PdfTemplateFont_Create()
{
	struct PdfTemplateFont *ret;
	ret = (struct PdfTemplateFont*)MemoryManager_Alloc(sizeof(struct PdfTemplateFont));
	PdfTemplateFont_Init(ret);
	return ret;
}

DLLEXPORT_TEST_FUNCTION struct PdfTemplateFont* PdfTemplateFont_CreateFromXml(xmlNode *node)
{
	struct PdfTemplateFont *ret;
	unsigned int i;
	
	ret = (struct PdfTemplateFont*)MemoryManager_Alloc(sizeof(struct PdfTemplateFont));
	PdfTemplateFont_Init(ret);
	
	ret->saveId = PdfTemplate_LoadIntAttribute(node, FONT_SAVE_ID);
	ret->size = PdfTemplate_LoadStringAttribute(node, FONT_SIZE);
	
	// Load embedded font if it contains it
	ret->embeddedFont = PdfTemplate_FindFont(CurrentTemplate, ret->saveId);

	// make r,g,b from color
	ret->colorR = PdfTemplate_LoadIntAttribute(node, FONT_COLOR_R);
	ret->colorG = PdfTemplate_LoadIntAttribute(node, FONT_COLOR_G);
	ret->colorB = PdfTemplate_LoadIntAttribute(node, FONT_COLOR_B);

	return ret;	
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateFont_Init(struct PdfTemplateFont* self)
{
	self->size = 0;
	self->embeddedFont = 0;
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateFont_Cleanup(struct PdfTemplateFont* self)
{
	if (self->size)
	{
		MemoryManager_Free(self->size);
		self->size = 0;
	}

	// we dont destroy embedded fonts as this is done somewhere else
	self->embeddedFont = 0;
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateFont_Destroy(struct PdfTemplateFont* self)
{
	PdfTemplateFont_Cleanup(self);
	MemoryManager_Free(self);
}