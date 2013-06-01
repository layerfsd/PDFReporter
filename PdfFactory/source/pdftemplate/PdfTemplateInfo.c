/*
PdfTemplateInfo.c

Author: Marko Vranjkovic
Date: 25.07.2008.	

*/

#include "PdfTemplateInfo.h"
#include "MemoryManager.h"

DLLEXPORT_TEST_FUNCTION void PdfTemplateInfo_Write()
{
}

DLLEXPORT_TEST_FUNCTION struct PdfTemplateInfo* PdfTemplateInfo_Create()
{
	struct PdfTemplateInfo *ret;
	ret = (struct PdfTemplateInfo*)MemoryManager_Alloc(sizeof(struct PdfTemplateInfo));
	ret->author = 0;
	ret->date = 0;
	return ret;
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateInfo_Init(struct PdfTemplateInfo* self, char* author, char* date)
{
	self->author = MemoryManager_StrDup(author);
	self->date = MemoryManager_StrDup(date);
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateInfo_Destroy(struct PdfTemplateInfo* self)
{
	PdfTemplateInfo_Cleanup(self);
	MemoryManager_Free(self);
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateInfo_Cleanup(struct PdfTemplateInfo* self)
{
	if (self->author)
	{
		MemoryManager_Free(self->author);
		self->author = 0;
	}
	if (self->date)
	{
		MemoryManager_Free(self->date);
		self->date = 0;
	}
}