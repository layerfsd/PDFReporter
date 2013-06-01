#ifndef _PdfFont_
#define _PdfFont_

#include "PdfBaseObject.h"

#define PDF_MAC_ROMAN_ENCODING  0
#define PDF_MAC_EXPERT_ENCODING 1
#define PDF_WIN_ANSI_ENCODING   2
#define PDF_IDENTITY_H          3


struct PdfFont
{
	struct PdfBaseObject base;
	char *subtype;
	char *baseFont;
	char *encoding;
	char *name;
	struct PdfFontDescriptor *fontDescriptor; // this is created with font
	struct PdfTemplateEmbeddedFont *templateEmbeddedFont; // this is object containing everything related to loaded font from template file
	int bold;
	int italic;	
	int firstChar;
	int lastChar;
	char *widths;

	// This is temp part for now
	int toUnicode;        //Treba da bude objectID CMap objecta
	int descendantFonts;  //Treba da bude objectID CIDFont objekta
};

DLLEXPORT_TEST_FUNCTION struct PdfFont *PdfFont_CreateFromTemplate(struct PdfDocument *document, struct PdfTemplateEmbeddedFont *embeddedTemplateFont);
/* Creates new pdf font object from template font. */

DLLEXPORT_TEST_FUNCTION struct PdfFont *PdfFont_Create(struct PdfDocument *document);
/*  Creates new pdf font object. */

DLLEXPORT_TEST_FUNCTION void PdfFont_Init(struct PdfFont *self, struct PdfDocument *document);
/* Initializes struct. */

void PdfFont_Cleanup(struct PdfFont *self);
/* Destroy PdfFont struct. */

void PdfFont_Destroy(struct PdfFont *self);
/* Destroy PdfFont struct. */

DLLEXPORT_TEST_FUNCTION void PdfFont_Write(struct PdfFont *self);
/* Writes PdfFont object. */

DLLEXPORT_TEST_FUNCTION void PdfFont_SetType1Font(struct PdfFont *self);
/* Sets Type1 font. */

DLLEXPORT_TEST_FUNCTION void PdfFont_SetTrueTypeFont(struct PdfFont *self);
/* Sets TypeType font. */

DLLEXPORT_TEST_FUNCTION void PdfFont_SetToUnicode(struct PdfFont *self, int toCMapReference);
/* Sets reference to CMap object. */

DLLEXPORT_TEST_FUNCTION void PdfFont_SetDescendantFonts(struct PdfFont *self, int toCIDFontReference);
/* Sets reference to object that holds list of all CIDFonts. */

DLLEXPORT_TEST_FUNCTION void PdfFont_SetEncoding(struct PdfFont *self, int encoding);
/* Sets standard font encoding. */

#endif 
