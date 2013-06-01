#ifndef _PDFPAGES_
#define _PDFPAGES_

#include "PdfFactory.h"
#include "PdfDocument.h"
#include "PdfBaseObject.h"

struct PdfPages
{
	struct PdfBaseObject base;

	int kidsCount; // number of kids
	struct DLList *pages; // list of all pages . Item is PdfPage

};

DLLEXPORT_TEST_FUNCTION void PdfPages_Write(struct PdfPages *self);
/* Write PdfOutline object to file. */

DLLEXPORT_TEST_FUNCTION struct PdfPages* PdfPages_Create(struct PdfDocument *document);
/* Create PdfOutline object. */

DLLEXPORT_TEST_FUNCTION void PdfPages_Init(struct PdfPages *self, struct PdfDocument *document);
/* Initializes PdfOutline struct. */

DLLEXPORT_TEST_FUNCTION void PdfPages_Destroy(struct PdfPages *self);
/* Destroy IndirectReference struct. */



#endif
