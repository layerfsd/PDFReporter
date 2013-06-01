#ifndef _PdfCIDFont_
#define _PdfCIDFont_

#include "PdfBaseObject.h"

#define PDF_CID_FONT_TYPE_2 "CIDFontType2"
#define PDF_CID_FONT_TYPE_0 "CIDFontType0"

struct PdfCIDFont
{
	struct PdfBaseObject base;
	char *subtype;
	int fontDescriptor;
	char *baseFont;
	int cidSystemInfo;
	int defaultWidth;    //DW   (U kasnijim verzijama ubacice se mogucnost sirine ponaosob)
};



DLLEXPORT_TEST_FUNCTION struct PdfCIDFont *PdfCIDFont_Create(struct PdfDocument *document);

DLLEXPORT_TEST_FUNCTION void PdfCIDFont_Init(struct PdfCIDFont *self, struct PdfDocument *document);

DLLEXPORT_TEST_FUNCTION void PdfCIDFont_Cleanup(struct PdfCIDFont *self);

DLLEXPORT_TEST_FUNCTION void PdfCIDFont_Destroy(struct PdfCIDFont *self);

DLLEXPORT_TEST_FUNCTION void PdfCIDFont_Write(struct PdfCIDFont *self);

DLLEXPORT_TEST_FUNCTION void PdfCIDFont_SetBaseFont(struct PdfCIDFont *self, char *baseFontName);

DLLEXPORT_TEST_FUNCTION void PdfCIDFont_SetFontDescriptor(struct PdfCIDFont *self, int toFontDescriptor);

DLLEXPORT_TEST_FUNCTION void PdfCIDFont_SetCIDSystemInfo(struct PdfCIDFont *self, int toCIDSystemInfo);


#endif