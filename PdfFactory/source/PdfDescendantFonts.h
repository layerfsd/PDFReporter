#ifndef _PdfDescendantFonts_
#define _PdfDescendantFonts_

#include "PdfBaseObject.h"

struct PdfDescendantFonts     // Basicly it's a list of references that points to DescendantFonts
{
	struct PdfBaseObject base;
	int references[10];
	int refCounter;
};


DLLEXPORT_TEST_FUNCTION struct PdfDescendantFonts *PdfDescendantFonts_Create(struct PdfDocument *document);

DLLEXPORT_TEST_FUNCTION void PdfDescendantFonts_Init(struct PdfDescendantFonts *self, struct PdfDocument *document);

DLLEXPORT_TEST_FUNCTION void PdfDescendantFonts_AddReference(struct PdfDescendantFonts *self, int descendantFontRef);

DLLEXPORT_TEST_FUNCTION void PdfDescendantFonts_Cleanup(struct PdfDescendantFonts *self);

DLLEXPORT_TEST_FUNCTION void PdfDescendantFonts_Destroy(struct PdfDescendantFonts *self);

DLLEXPORT_TEST_FUNCTION void PdfDescendantFonts_Write(struct PdfDescendantFonts *self);


#endif
