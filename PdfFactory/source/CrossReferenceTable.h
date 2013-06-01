/*
CrossReferenceTable.h

Author: Nebojsa Vislavski
Date: 30.6.2008.	

Writes cross reference table to pdf
*/


#ifndef _PDFCROSSREFERENCETABLE_
#define _PDFCROSSREFERENCETABLE_

#include "PdfDocument.h"
#include "PdfFactory.h"

struct CrossReferenceTable
{
	struct PdfDocument *document;
	int beginOffset; // offset in file where this cross reference table begins
};

DLLEXPORT_TEST_FUNCTION struct CrossReferenceTable* CrossReferenceTable_Create(struct PdfDocument *document);
DLLEXPORT_TEST_FUNCTION void CrossReferenceTable_Write(struct CrossReferenceTable *self);
DLLEXPORT_TEST_FUNCTION void CrossReferenceTable_Destroy(struct CrossReferenceTable *self);

#endif
