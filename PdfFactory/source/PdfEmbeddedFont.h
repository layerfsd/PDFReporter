/*
PDFTextWriter.h

Used for writing texts to content streams
*/


#ifndef _PDFEMBEDDEDFONT_
#define _PDFEMBEDDEDFONT_


#include "PdfFactory.h"
#include "PdfDocument.h"


// This is embedded font. It is used to write objects for embedded fonts
struct PdfEmbeddedFont
{
	struct PdfBaseObject base;
	char *fileName;
	char *fontData; // font data is not released after writting
	int fontDataSize;
	
};

DLLEXPORT_TEST_FUNCTION struct PdfEmbeddedFont *PdfEmbeddedFont_Create(struct PdfDocument *document, char *fontData, int fontDataSize);

DLLEXPORT_TEST_FUNCTION void PdfEmbeddedFont_Init(struct PdfEmbeddedFont *self, struct PdfDocument *document);

void PdfEmbeddedFont_Cleanup(struct PdfEmbeddedFont *self);

void PdfEmbeddedFont_Destroy(struct PdfEmbeddedFont *self);

DLLEXPORT_TEST_FUNCTION void PdfEmbeddedFont_WriteTrueType(struct PdfEmbeddedFont *self);

// This is test method
DLLEXPORT_TEST_FUNCTION void PdfEmbeddedFont_LoadFromFile(struct PdfEmbeddedFont *self, char *fileName);

#endif