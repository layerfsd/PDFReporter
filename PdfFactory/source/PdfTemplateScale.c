#include "PdfTemplateScale.h"
#include "PdfTemplateElements.h"
#include "MemoryManager.h"
#include "PdfTemplate.h"

DLLEXPORT_TEST_FUNCTION struct PdfTemplateScale* PdfTemplateScale_Create(char *x, char *y)
{
	struct PdfTemplateScale *ret;
	ret = (struct PdfTemplateScale*)MemoryManager_Alloc(sizeof(struct PdfTemplateScale));
	PdfTemplateScale_Init(ret, x, y);
	return ret;
}

DLLEXPORT_TEST_FUNCTION struct PdfTemplateScale* PdfTemplateScale_CreateFromXml(xmlNode *node)
{
	struct PdfTemplateScale *ret;
	ret = (struct PdfTemplateScale*)MemoryManager_Alloc(sizeof(struct PdfTemplateScale));

	ret->x = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(node, "x"));
	ret->y = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(node, "y"));

	return ret;
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateScale_Init(struct PdfTemplateScale* self, char *width, char *height)
{
	self->x = MemoryManager_StrDup(width);
	self->y = MemoryManager_StrDup(height);
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateScale_Destroy(struct PdfTemplateScale* self)
{
	PdfTemplateScale_Cleanup(self);
	MemoryManager_Free(self);
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateScale_Cleanup(struct PdfTemplateScale* self)
{
	if (self->x)
	{
		MemoryManager_Free(self->x);
		self->x = 0;
	}
	if (self->y)
	{
		MemoryManager_Free(self->y);
		self->y = 0;
	}
}
