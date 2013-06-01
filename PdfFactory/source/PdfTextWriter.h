/*
PDFTextWriter.h

Used for writing texts to content streams
*/


#ifndef _PDFTEXTWRITER_
#define _PDFTEXTWRITER_


#include "PdfFactory.h"
#include "PdfDocument.h"
#include "StreamWriter.h"

struct PdfFont;

struct PdfTextWriter
{
	struct StreamWriter *streamWriter;
	// some text states maybe should be placed here
};


DLLEXPORT_TEST_FUNCTION struct PdfTextWriter* PdfTextWriter_Begin(struct StreamWriter *streamWriter);
/* create and begin writing text */

DLLEXPORT_TEST_FUNCTION void PdfTextWriter_WriteText(struct PdfTextWriter *self, char *text);
DLLEXPORT_TEST_FUNCTION void PdfTextWriter_WriteUnicodeText(struct PdfTextWriter *self, short *text);

/* Use this function in tests. You should never use this in template items */
DLLEXPORT_TEST_FUNCTION void PdfTextWriter_SetFont(struct PdfTextWriter *self, struct PdfFont *font, char *size);

/* This functions is used normally in template items */
DLLEXPORT_TEST_FUNCTION void PdfTextWriter_SetFontTemplated(struct PdfTextWriter *self, struct PdfFont *font, struct PdfTemplateFont *templateFont);


DLLEXPORT_TEST_FUNCTION void PdfTextWriter_SetRGBColor(struct PdfTextWriter *self, unsigned char red, unsigned char green, unsigned char blue);
/* Sets font from resources */

DLLEXPORT_TEST_FUNCTION void PdfTextWriter_NewLine(struct PdfTextWriter *self);
/* go to new line */

DLLEXPORT_TEST_FUNCTION void PdfTextWriter_SetTextMatrix(struct PdfTextWriter *self,
	float a, float b, float c, float d, float e, float f);
/*
  Sets the following text matrix 
    [ a b 0 
	  c d 0 
	  e f 1 ]
*/

DLLEXPORT_TEST_FUNCTION void PdfTextWriter_SetPosition(struct PdfTextWriter *self, int x, int y);
/* Set position where text should be printed */

DLLEXPORT_TEST_FUNCTION void PdfTextWriter_EndText(struct PdfTextWriter *self);
/* Call this to end text */

void PdfTextWriter_Destroy(struct PdfTextWriter *self);
/* destructor */

char *CheckText(const char *text);


#endif
