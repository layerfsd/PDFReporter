/*
PdfTemplatePageSize.h

Author: Marko Vranjkovic
Date: 18.08.2008.	

*/

#ifndef _PDFTEMPLATEPAGESIZE_
#define _PDFTEMPLATEPAGESIZE_

#include "PdfFactory.h"
#include <libxml/tree.h>

struct PdfTemplatePageSize
{	
	char *width;
	char *height;
};

DLLEXPORT_TEST_FUNCTION void PdfTemplatePageSize_Write();

DLLEXPORT_TEST_FUNCTION struct PdfTemplatePageSize* PdfTemplatePageSize_Create();

DLLEXPORT_TEST_FUNCTION struct PdfTemplatePageSize* PdfTemplatePageSize_CreateFromXml(xmlNode *node);

DLLEXPORT_TEST_FUNCTION void PdfTemplatePageSize_Init(struct PdfTemplatePageSize* self, char *width, char *height);

DLLEXPORT_TEST_FUNCTION void PdfTemplatePageSize_Destroy(struct PdfTemplatePageSize* self);

DLLEXPORT_TEST_FUNCTION void PdfTemplatePageSize_Cleanup(struct PdfTemplatePageSize* self);


#endif