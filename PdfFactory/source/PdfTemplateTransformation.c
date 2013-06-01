#include "PdfTemplateTransformation.h"
#include "PdfTemplateElements.h"
#include "MemoryManager.h"
#include "PdfTemplate.h"

DLLEXPORT_TEST_FUNCTION struct PdfTemplateTransformation* PdfTemplateTransformation_Create(float a, float b, float c, float d)
{
	struct PdfTemplateTransformation *ret;
	ret = (struct PdfTemplateTransformation*)MemoryManager_Alloc(sizeof(struct PdfTemplateTransformation));
	PdfTemplateTransformation_Init(ret, a, b, c, d);
	return ret;
}

DLLEXPORT_TEST_FUNCTION struct PdfTemplateTransformation* PdfTemplateTransformation_CreateFromXml(xmlNode *node)
{
	struct PdfTemplateTransformation *ret;
	ret = (struct PdfTemplateTransformation*)MemoryManager_Alloc(sizeof(struct PdfTemplateTransformation));

	ret->a = (float)PdfTemplate_LoadDoubleAttribute(node, "a");
	ret->b = (float)-PdfTemplate_LoadDoubleAttribute(node, "b");
	ret->c = (float)-PdfTemplate_LoadDoubleAttribute(node, "c");
	ret->d = (float)PdfTemplate_LoadDoubleAttribute(node, "d");

	return ret;
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateTransformation_Init(struct PdfTemplateTransformation* self, float a, float b, float c, float d)
{
	self->a = a;
	self->b = b;
	self->c = c;
	self->d = d;
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateTransformation_Destroy(struct PdfTemplateTransformation* self)
{
	PdfTemplateTransformation_Cleanup(self);
	MemoryManager_Free(self);
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateTransformation_Cleanup(struct PdfTemplateTransformation* self)
{
}
