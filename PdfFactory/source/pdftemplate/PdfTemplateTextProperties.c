/*
PdfTemplateTextProperties.c

Author: Marko Vranjkovic
Date: 25.07.2008.	

*/

#include "PdfTemplateTextProperties.h"
#include "PdfTemplateLocation.h"
#include "PdfTemplateFont.h"
#include "MemoryManager.h"

DLLEXPORT_TEST_FUNCTION void PdfTemplateTextProperties_Write()
{
}

DLLEXPORT_TEST_FUNCTION struct PdfTemplateTextProperties* PdfTemplateTextProperties_Create()
{
	struct PdfTemplateTextProperties *ret;
	ret = (struct PdfTemplateTextProperties*)MemoryManager_Alloc(sizeof(struct PdfTemplateTextProperties));
	ret->font = NULL;
	ret->location = NULL;
	return ret;
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateTextProperties_Init(struct PdfTemplateTextProperties* self)
{
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateTextProperties_Destroy(struct PdfTemplateTextProperties* self)
{
	PdfTemplateLocation_Destroy(self->location);
	PdfTemplateFont_Destroy(self->font);
	MemoryManager_Free(self);
}