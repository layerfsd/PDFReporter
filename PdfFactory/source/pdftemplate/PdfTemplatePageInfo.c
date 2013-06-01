/*
PdfTemplatePageInfo.c

Author: Marko Vranjkovic
Date: 18.08.2008.	

*/

#include "PdfTemplatePageInfo.h"
#include "PdfTemplate.h"
#include "PdfTemplateElements.h"
#include "MemoryManager.h"

DLLEXPORT_TEST_FUNCTION void PdfTemplatePageInfo_Write()
{
}

DLLEXPORT_TEST_FUNCTION struct PdfTemplatePageInfo* PdfTemplatePageInfo_Create()
{
	struct PdfTemplatePageInfo *ret;
	ret = (struct PdfTemplatePageInfo*)MemoryManager_Alloc(sizeof(struct PdfTemplatePageInfo));
	return ret;
}

DLLEXPORT_TEST_FUNCTION struct PdfTemplatePageInfo* PdfTemplatePageInfo_CreateFromXml(xmlNode *node)
{
	struct PdfTemplatePageInfo *ret;
	ret = (struct PdfTemplatePageInfo*)MemoryManager_Alloc(sizeof(struct PdfTemplatePageInfo));
	ret->description = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(node, DESCRIPTION));
	return ret;
}

DLLEXPORT_TEST_FUNCTION void PdfTemplatePageInfo_Init(struct PdfTemplatePageInfo* self, char * descriptionParam)
{
	self->description = MemoryManager_StrDup(descriptionParam);
	
}

DLLEXPORT_TEST_FUNCTION void PdfTemplatePageInfo_Destroy(struct PdfTemplatePageInfo* self)
{
	PdfTemplatePageInfo_Cleanup(self);
	MemoryManager_Free(self);
}

DLLEXPORT_TEST_FUNCTION void PdfTemplatePageInfo_Cleanup(struct PdfTemplatePageInfo* self)
{
	if (self->description)
	{
		MemoryManager_Free(self->description);
		self->description = 0;
	}
}