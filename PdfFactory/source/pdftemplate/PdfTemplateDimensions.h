/*
PdfTemplateDimensions.h

Author: Marko Vranjkovic
Date: 19.08.2008.	

*/

#ifndef _PDFTEMPLATEDIMENSIONS_
#define _PDFTEMPLATEDIMENSIONS_

#include "PdfFactory.h"
#include <libxml/parser.h>
#include <libxml/xmlmemory.h>

struct PdfTemplateDimensions
{
	char *width;
	char *height;
};

DLLEXPORT_TEST_FUNCTION void PdfTemplateDimensions_Write();

DLLEXPORT_TEST_FUNCTION struct PdfTemplateDimensions* PdfTemplateDimensions_Create();
DLLEXPORT_TEST_FUNCTION struct PdfTemplateDimensions* PdfTemplateDimensions_CreateFromXml(xmlNode *node);

DLLEXPORT_TEST_FUNCTION void PdfTemplateDimensions_Init(struct PdfTemplateDimensions* self, char *widthParam, char *heightParam);

DLLEXPORT_TEST_FUNCTION void PdfTemplateDimensions_Destroy(struct PdfTemplateDimensions* self);


#endif