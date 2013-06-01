#ifndef _PDFOUTLINES_
#define _PDFOUTLINES_

#include "PdfFactory.h"
#include "PdfDocument.h"
#include "PdfBaseObject.h"

struct PdfOutlines
{
	struct PdfBaseObject base;
	int count; // currently 0 .. not yet implementeds
};

DLLEXPORT_TEST_FUNCTION void PdfOutlines_Write(struct PdfOutlines *self);
/* Write PdfOutline object to file. */

DLLEXPORT_TEST_FUNCTION struct PdfOutlines* PdfOutlines_Create(struct PdfDocument *document);
/* Create PdfOutline object. */

DLLEXPORT_TEST_FUNCTION void PdfOutlines_Init(struct PdfOutlines *self, struct PdfDocument *document);
/* Initializes PdfOutline struct. */

DLLEXPORT_TEST_FUNCTION void PdfOutlines_Destroy(struct PdfOutlines *self);
/* Destroy IndirectReference struct. */

#endif
