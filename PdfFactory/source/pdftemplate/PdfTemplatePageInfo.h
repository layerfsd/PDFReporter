/*
PdfTemplatePageInfo.h

Author: Marko Vranjkovic
Date: 18.08.2008.	

*/

#ifndef _PDFTEMPLATEPAGEINFO_
#define _PDFTEMPLATEPAGEINFO_

#include "PdfFactory.h"
#include <libxml\tree.h>

struct PdfTemplatePageInfo
{
	char* description;
};

DLLEXPORT_TEST_FUNCTION void PdfTemplatePageInfo_Write();

DLLEXPORT_TEST_FUNCTION struct PdfTemplatePageInfo* PdfTemplatePageInfo_Create();

DLLEXPORT_TEST_FUNCTION struct PdfTemplatePageInfo* PdfTemplatePageInfo_CreateFromXml(xmlNode *node);

DLLEXPORT_TEST_FUNCTION void PdfTemplatePageInfo_Init(struct PdfTemplatePageInfo* self, char * descriptionParam);

DLLEXPORT_TEST_FUNCTION void PdfTemplatePageInfo_Destroy(struct PdfTemplatePageInfo* self);

DLLEXPORT_TEST_FUNCTION void PdfTemplatePageInfo_Cleanup(struct PdfTemplatePageInfo* self);


#endif