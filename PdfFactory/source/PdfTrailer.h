/*
PdFTrailer.h

Author: Nebojsa Vislavski
Date: 2.7.2008.	

Used for writting trailer to pdf

*/


#ifndef _PDF_TRAILER_
#define _PDF_TRAILER_


#include "PdfFactory.h"
#include "PdfDocument.h"

struct PdfTrailer
{
	struct PdfDocument *document;
};

DLLEXPORT_TEST_FUNCTION struct PdfTrailer* PdfTrailer_Create(struct PdfDocument *document);
DLLEXPORT_TEST_FUNCTION void PdfTrailer_Write(struct PdfTrailer *self);
DLLEXPORT_TEST_FUNCTION void PdfTrailer_Destroy(struct PdfTrailer *self);


#endif
