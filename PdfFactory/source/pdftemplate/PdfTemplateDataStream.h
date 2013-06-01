/*
PdfTemplateDataStream.h

Author: Marko Vranjkovic
Date: 25.07.2008.	

*/

#ifndef _PDFTEMPLATEDATASTREAM_
#define _PDFTEMPLATEDATASTREAM_

#include "PdfTemplateColumns.h"
#include <libxml/tree.h>

struct PdfTemplateDataStream
{
	char *name;
	struct DLList* columns;
};

DLLEXPORT_TEST_FUNCTION void PdfTemplateDataStream_Write(struct PdfTemplateDataStream* self);

DLLEXPORT_TEST_FUNCTION struct PdfTemplateDataStream* PdfTemplateDataStream_Create(char *name);
DLLEXPORT_TEST_FUNCTION struct PdfTemplateDataStream* PdfTemplateDataStream_CreateFromXml(xmlNode *node);

DLLEXPORT_TEST_FUNCTION void PdfTemplateDataStream_Init(struct PdfTemplateDataStream* self, char* name);

DLLEXPORT_TEST_FUNCTION void PdfTemplateDataStream_Destroy(struct PdfTemplateDataStream* self);

DLLEXPORT_TEST_FUNCTION void PdfTemplateDataStream_Cleanup(struct PdfTemplateDataStream* self);

DLLEXPORT_TEST_FUNCTION void PdfTemplateDataStream_AddColumn(struct PdfTemplateDataStream* self, char* nameOfColumn, char* typeOfColumn);

#endif