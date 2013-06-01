/*
PdfTemplateHeader.c

Author: Marko Vranjkovic
Date: 25.07.2008.	

*/

#include "PdfTemplateHeader.h"
#include "MemoryManager.h"

DLLEXPORT_TEST_FUNCTION void PdfTemplateHeader_Write()
{
}

DLLEXPORT_TEST_FUNCTION struct PdfTemplateHeader* PdfTemplateHeader_Create()
{
	struct PdfTemplateHeader *ret;
	ret = (struct PdfTemplateHeader*)MemoryManager_Alloc(sizeof(struct PdfTemplateHeader));
	PdfTemplateHeader_Init(ret, "1.0");
	return ret;
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateHeader_Init(struct PdfTemplateHeader* self, char *versionParam)
{
	self->version = MemoryManager_StrDup(versionParam);
	self->info = PdfTemplateInfo_Create();
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateHeader_Destroy(struct PdfTemplateHeader* self)
{
	MemoryManager_Free(self->version);

	PdfTemplateInfo_Destroy(self->info);
	MemoryManager_Free(self);
}