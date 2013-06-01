/*
PdfTemplateFont.h

Date: 21.08.2008.	

*/

#ifndef _PDFTEMPLATEFONT_
#define _PDFTEMPLATEFONT_

#include "PdfFactory.h"
#include "RectangleNormal.h"
#include <libxml/tree.h>

struct PdfTemplateFont
{
	char *size;
	int saveId;

	struct PdfTemplateEmbeddedFont *embeddedFont;

	int colorR;
	int colorG;
	int colorB;
};


DLLEXPORT_TEST_FUNCTION struct PdfTemplateFont* PdfTemplateFont_Create();
DLLEXPORT_TEST_FUNCTION struct PdfTemplateFont* PdfTemplateFont_CreateFromXml(xmlNode *node);

DLLEXPORT_TEST_FUNCTION void PdfTemplateFont_Init(struct PdfTemplateFont* self);

DLLEXPORT_TEST_FUNCTION void PdfTemplateFont_Cleanup(struct PdfTemplateFont* self);

DLLEXPORT_TEST_FUNCTION void PdfTemplateFont_Destroy(struct PdfTemplateFont* self);


#endif