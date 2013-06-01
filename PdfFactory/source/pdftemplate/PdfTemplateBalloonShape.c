/*
PdfTemplateBalloonShape.c

Author: Marko Vranjkovic
Date: 25.07.2008.	

*/

#include "PdfTemplateBalloonShape.h"
#include "PdfTemplateElements.h"
#include "MemoryManager.h"
#include "PdfTemplate.h"
#include <libxml/parser.h>
#include <libxml/xmlmemory.h>

DLLEXPORT_TEST_FUNCTION void PdfTemplateBalloonShape_Write()
{
}

DLLEXPORT_TEST_FUNCTION struct PdfTemplateBalloonShape* PdfTemplateBalloonShape_Create()
{
	struct PdfTemplateBalloonShape *ret;
	ret = (struct PdfTemplateBalloonShape*)MemoryManager_Alloc(sizeof(struct PdfTemplateBalloonShape));
	ret->dimensions = NULL;
	ret->type = NULL;
	return ret;
}

DLLEXPORT_TEST_FUNCTION struct PdfTemplateBalloonShape* PdfTemplateBalloonShape_CreateFromXml(xmlNode *node)
{
	struct PdfTemplateBalloonShape *ret;
	xmlNode *foundNode;
	ret = (struct PdfTemplateBalloonShape*)MemoryManager_Alloc(sizeof(struct PdfTemplateBalloonShape));
	ret->dimensions = NULL;
	ret->type = NULL;

	// TODO: Should be rewritten to support other shapes
	ret->type = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(node, TYPE));
	foundNode = PdfTemplate_FindNode(node, DIMENSIONS);
	if (foundNode)
	{
		ret->dimensions = PdfTemplateDimensions_CreateFromXml(foundNode);
	}
	else 
	{
		PdfTemplateBalloonShape_Destroy(ret);
		return NULL;
	}	

	return ret;
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateBalloonShape_Init(struct PdfTemplateBalloonShape* self)
{
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateBalloonShape_Destroy(struct PdfTemplateBalloonShape* self)
{
	PdfTemplateDimensions_Destroy(self->dimensions);
	MemoryManager_Free(self);
}