/*
PdfTemplatePageSize.c

Author: Marko Vranjkovic
Date: 18.08.2008.	

*/

#include "PdfTemplatePageSize.h"
#include "PdfTemplateElements.h"
#include "MemoryManager.h"
#include "PdfTemplate.h"

DLLEXPORT_TEST_FUNCTION void PdfTemplatePageSize_Write()
{
}

DLLEXPORT_TEST_FUNCTION struct PdfTemplatePageSize* PdfTemplatePageSize_Create()
{
	struct PdfTemplatePageSize *ret;
	ret = (struct PdfTemplatePageSize*)MemoryManager_Alloc(sizeof(struct PdfTemplatePageSize));
	return ret;
}

DLLEXPORT_TEST_FUNCTION struct PdfTemplatePageSize* PdfTemplatePageSize_CreateFromXml(xmlNode *node)
{
	struct PdfTemplatePageSize *ret;
	ret = (struct PdfTemplatePageSize*)MemoryManager_Alloc(sizeof(struct PdfTemplatePageSize));

	ret->width = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(node, WIDTH));
	ret->height = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(node, HEIGHT));

	return ret;
}

DLLEXPORT_TEST_FUNCTION void PdfTemplatePageSize_Init(struct PdfTemplatePageSize* self, char *width, char *height)
{
	self->width = MemoryManager_StrDup(width);
	self->height = MemoryManager_StrDup(height);
}

DLLEXPORT_TEST_FUNCTION void PdfTemplatePageSize_Destroy(struct PdfTemplatePageSize* self)
{
	PdfTemplatePageSize_Cleanup(self);
	MemoryManager_Free(self);
}

DLLEXPORT_TEST_FUNCTION void PdfTemplatePageSize_Cleanup(struct PdfTemplatePageSize* self)
{
	if (self->width)
	{
		MemoryManager_Free(self->width);
		self->width = 0;
	}
	if(self->height)
	{
		MemoryManager_Free(self->height);
		self->height = 0;
	}
}