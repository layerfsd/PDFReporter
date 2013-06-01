#ifndef _PDFTEMPLATE_SCALE
#define _PDFTEMPLATE_SCALE

#include "PdfFactory.h"
#include <libxml/tree.h>

struct PdfTemplateScale
{
	char *x;
	char *y;
};

DLLEXPORT_TEST_FUNCTION struct PdfTemplateScale* PdfTemplateScale_Create(char *width, char *height);

DLLEXPORT_TEST_FUNCTION struct PdfTemplateScale* PdfTemplateScale_CreateFromXml(xmlNode *node);

DLLEXPORT_TEST_FUNCTION void PdfTemplateScale_Init(struct PdfTemplateScale* self, char *width, char *height);

DLLEXPORT_TEST_FUNCTION void PdfTemplateScale_Destroy(struct PdfTemplateScale* self);

DLLEXPORT_TEST_FUNCTION void PdfTemplateScale_Cleanup(struct PdfTemplateScale* self);


#endif