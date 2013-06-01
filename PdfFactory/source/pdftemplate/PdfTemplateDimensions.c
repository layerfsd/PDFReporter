/*
PdfTemplateDimensions.c

Author: Marko Vranjkovic
Date: 19.08.2008.	

*/

#include "PdfTemplateDimensions.h"
#include "PdfTemplateElements.h"
#include "MemoryManager.h"
#include "PdfTemplate.h"
#include <libxml/parser.h>
#include <libxml/xmlmemory.h>

DLLEXPORT_TEST_FUNCTION void PdfTemplateDimensions_Write()
{
}

DLLEXPORT_TEST_FUNCTION struct PdfTemplateDimensions* PdfTemplateDimensions_Create()
{
	struct PdfTemplateDimensions *ret;
	ret = (struct PdfTemplateDimensions*)MemoryManager_Alloc(sizeof(struct PdfTemplateDimensions));
	return ret;
}

DLLEXPORT_TEST_FUNCTION struct PdfTemplateDimensions* PdfTemplateDimensions_CreateFromXml(xmlNode *node)
{
	struct PdfTemplateDimensions *ret;
	ret = (struct PdfTemplateDimensions*)MemoryManager_Alloc(sizeof(struct PdfTemplateDimensions));

	ret->width = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(node, DIMENSIONS_WIDTH));
	ret->height = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(node, DIMENSIONS_HEIGHT));

	return ret;	
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateDimensions_Init(struct PdfTemplateDimensions* self, char * widthParam, char* heightParam)
{
	self->width = widthParam;
	self->height = heightParam;
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateDimensions_Destroy(struct PdfTemplateDimensions* self)
{
	if (self->width)
	{
		MemoryManager_Free(self->width);
		self->width = 0;
	}
	if (self->height)
	{
		MemoryManager_Free(self->height);
		self->height = 0;
	}
	MemoryManager_Free(self);
}