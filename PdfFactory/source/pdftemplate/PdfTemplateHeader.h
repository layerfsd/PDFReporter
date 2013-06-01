/*
PdfTemplateHeader.h

Author: Marko Vranjkovic
Date: 25.07.2008.	

*/

#ifndef _PDFTEMPLATEHEADER_
#define _PDFTEMPLATEHEADER_

#include "PdfFactory.h"
#include "PdfTemplateInfo.h"


struct PdfTemplateHeader
{
	char *version;
	struct PdfTemplateInfo* info;
};

DLLEXPORT_TEST_FUNCTION void PdfTemplateHeader_Write();

DLLEXPORT_TEST_FUNCTION struct PdfTemplateHeader* PdfTemplateHeader_Create();

DLLEXPORT_TEST_FUNCTION void PdfTemplateHeader_Init(struct PdfTemplateHeader* self, char *versionParam);

DLLEXPORT_TEST_FUNCTION void PdfTemplateHeader_Destroy(struct PdfTemplateHeader* self);

#endif