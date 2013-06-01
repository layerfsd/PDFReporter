/*
PdfTemplateInfo.h

Author: Marko Vranjkovic
Date: 29.07.2008.	

*/

#ifndef _PDFTEMPLATEINFO_
#define _PDFTEMPLATEINFO_

#include "PdfFactory.h"


struct PdfTemplateInfo
{
	char* author;
	char* date;
};

DLLEXPORT_TEST_FUNCTION void PdfTemplateInfo_Write();

DLLEXPORT_TEST_FUNCTION struct PdfTemplateInfo* PdfTemplateInfo_Create();

DLLEXPORT_TEST_FUNCTION void PdfTemplateInfo_Init(struct PdfTemplateInfo* self, char* author, char* date);

DLLEXPORT_TEST_FUNCTION void PdfTemplateInfo_Destroy(struct PdfTemplateInfo *self);

DLLEXPORT_TEST_FUNCTION void PdfTemplateInfo_Cleanup(struct PdfTemplateInfo *self);

#endif