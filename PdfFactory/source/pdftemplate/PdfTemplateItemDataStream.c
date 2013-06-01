/*
PdfTemplateItemDataStream.c

Author: Marko Vranjkovic
Date: 23.08.2008.	

*/

#include "PdfFactory.h"
#include "PdfTemplateItemDataStream.h"
#include "MemoryManager.h"

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemDataStream_Write()
{
}

DLLEXPORT_TEST_FUNCTION struct PdfTemplateItemDataStream* PdfTemplateItemDataStream_Create()
{
	struct PdfTemplateItemDataStream *ret;
	ret = (struct PdfTemplateItemDataStream*)MemoryManager_Alloc(sizeof(struct PdfTemplateItemDataStream));
	return ret;
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemDataStream_Init(struct PdfTemplateItemDataStream* self)
{
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemDataStream_Destroy(struct PdfTemplateItemDataStream* self)
{
	MemoryManager_Free(self);
}