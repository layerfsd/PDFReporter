/*
PdfTemplateTextProperties.h

Author: Marko Vranjkovic
Date: 25.07.2008.	

*/

#ifndef _PDFTEMPLATETEXTPROPERTIES_
#define _PDFTEMPLATETEXTPROPERTIES_

#include "PdfFactory.h"

struct PdfTemplateTextProperties
{
	int textMatrix[9];
	struct PdfTemplateLocation *location;
	struct PdfTemplateFont *font;
};

DLLEXPORT_TEST_FUNCTION void PdfTemplateTextProperties_Write();

DLLEXPORT_TEST_FUNCTION struct PdfTemplateTextProperties* PdfTemplateTextProperties_Create();

DLLEXPORT_TEST_FUNCTION void PdfTemplateTextProperties_Init(struct PdfTemplateTextProperties* self);

DLLEXPORT_TEST_FUNCTION void PdfTemplateTextProperties_Destroy(struct PdfTemplateTextProperties* self);


#endif