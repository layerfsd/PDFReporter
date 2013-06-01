#ifndef _PdfFontDescriptor_
#define _PdfFontDescriptor_


#include "PdfBaseObject.h"
#include "Rectangle.h"


struct PdfFontDescriptor
{
	struct PdfBaseObject base;

	char *fontName;
	int flags;
	char *fontBBox;
	struct PdfFont *parentFont;
	int italicAngle;	
	int descent;
	int ascent;
	int capHeight;	
	int stemV;		
	int fontWeight;
	int xHeight;
	char *fontStretch;
	short trueTypeEmbeddedFont;
	short type1EmbeddedFont; // not implemented yet	
};


DLLEXPORT_TEST_FUNCTION struct PdfFontDescriptor *PdfFontDescriptor_Create(struct PdfDocument *document);

DLLEXPORT_TEST_FUNCTION void PdfFontDescriptor_Init(struct PdfFontDescriptor *self, struct PdfDocument *document);

DLLEXPORT_TEST_FUNCTION void PdfFontDescriptor_Cleanup(struct PdfFontDescriptor *self);

DLLEXPORT_TEST_FUNCTION void PdfFontDescriptor_Destroy(struct PdfFontDescriptor *self);

DLLEXPORT_TEST_FUNCTION void PdfFontDescriptor_Write(struct PdfFontDescriptor *self);

DLLEXPORT_TEST_FUNCTION void PdfFontDescriptor_SetFontBBox(struct PdfFontDescriptor *self, int x, int y, int w, int h);

DLLEXPORT_TEST_FUNCTION void PdfFontDescriptor_SetFontName(struct PdfFontDescriptor *self, char *name);



#endif