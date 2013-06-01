/*
PdfTemplateColumn.c

Author: Marko Vranjkovic
Date: 30.07.2008.	

*/

#include "PdfTemplateColumn.h"
#include "PdfTemplateElements.h"
#include "PdfTemplate.h"
#include "MemoryManager.h"

DLLEXPORT_TEST_FUNCTION void PdfTemplateColumn_Write(struct PdfTemplateColumn* self)
{
}

DLLEXPORT_TEST_FUNCTION struct PdfTemplateColumn* PdfTemplateColumn_Create(char *name, char *type)
{
	struct PdfTemplateColumn *ret;
	ret = (struct PdfTemplateColumn*)MemoryManager_Alloc(sizeof(struct PdfTemplateColumn));
	PdfTemplateColumn_Init(ret, name, type);
	return ret;
}

DLLEXPORT_TEST_FUNCTION struct PdfTemplateColumn* PdfTemplateColumn_CreateFromXml(xmlNode *node)
{
	struct PdfTemplateColumn *ret;
	ret = (struct PdfTemplateColumn*)MemoryManager_Alloc(sizeof(struct PdfTemplateColumn));
	
	ret->name = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(node, NAME));
	ret->type = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(node, TYPE));

	return ret;
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateColumn_Init(struct PdfTemplateColumn* self, char* name, char* type)
{	
	self->name = MemoryManager_StrDup(name);
	self->type = MemoryManager_StrDup(type);	
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateColumn_Destroy(struct PdfTemplateColumn* self)
{
	PdfTemplateColumn_Cleanup(self);
	MemoryManager_Free(self);
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateColumn_Cleanup(struct PdfTemplateColumn* self)
{
	if (self->name)
	{
		MemoryManager_Free(self->name);
		self->name = 0;
	}
	if (self->type)
	{
		MemoryManager_Free(self->type);
		self->type = 0;
	}
}