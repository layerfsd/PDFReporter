/*
PdfTemplateColumns.h

Author: Marko Vranjkovic
Date: 30.07.2008.	

*/

#ifndef _PDFTEMPLATECOLUMNS_
#define _PDFTEMPLATECOLUMNS_

#include "PdfFactory.h"
#include "PdfTemplateColumn.h"


struct PdfTemplateColumns
{
	struct PdfTemplateColumn* elements;
	char* name;
};

DLLEXPORT_TEST_FUNCTION void PdfTemplateColumns_Write();

DLLEXPORT_TEST_FUNCTION struct PdfTemplateColumns* PdfTemplateColumns_Create();

DLLEXPORT_TEST_FUNCTION void PdfTemplateColumns_Init(struct PdfTemplateColumns* self);

DLLEXPORT_TEST_FUNCTION void PdfTemplateColumns_Destroy(struct PdfTemplateColumns* self);

DLLEXPORT_TEST_FUNCTION void PdfTemplateColumns_AddColumn(struct PdfTemplateColumns* self);

#endif