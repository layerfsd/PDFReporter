/*
PdfTemplateItemDataStream.h

Author: Marko Vranjkovic
Date: 23.08.2008.	

*/

#ifndef _PDFTEMPLATEITEMDATASTREAM_
#define _PDFTEMPLATEITEMDATASTREAM_

#include "PdfFactory.h"

struct PdfTemplateItemDataStream
{
	char *name;
	char *column;
};

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemDataStream_Write();

DLLEXPORT_TEST_FUNCTION struct PdfTemplateItemDataStream* PdfTemplateItemDataStream_Create();

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemDataStream_Init(struct PdfTemplateItemDataStream* self);

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemDataStream_Destroy(struct PdfTemplateItemDataStream* self);


#endif