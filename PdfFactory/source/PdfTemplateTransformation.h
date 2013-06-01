#ifndef _PDFTEMPLATE_TRANSFORMATION_
#define _PDFTEMPLATE_TRANSFORMATION_


#include "PdfFactory.h"
#include <libxml/tree.h>

struct PdfTemplateTransformation
{
	float a;
	float b;
	float c;
	float d;	
};

DLLEXPORT_TEST_FUNCTION struct PdfTemplateTransformation* PdfTemplateTransformation_Create(float a, float b, float c, float d);

DLLEXPORT_TEST_FUNCTION struct PdfTemplateTransformation* PdfTemplateTransformation_CreateFromXml(xmlNode *node);

DLLEXPORT_TEST_FUNCTION void PdfTemplateTransformation_Init(struct PdfTemplateTransformation* self, float a, float b, float c, float d);

DLLEXPORT_TEST_FUNCTION void PdfTemplateTransformation_Destroy(struct PdfTemplateTransformation* self);

DLLEXPORT_TEST_FUNCTION void PdfTemplateTransformation_Cleanup(struct PdfTemplateTransformation* self);



#endif