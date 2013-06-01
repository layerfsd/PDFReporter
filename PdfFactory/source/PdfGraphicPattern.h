//-----------------------------------------------------------------------------
// Name:	PdfGraphicPattern.c
// Author:	Tomislav Kukic
// Date:	22.12.2008
//-----------------------------------------------------------------------------


#ifndef _PDFGRAPHICPATTERN_
#define _PDFGRAPHICPATTERN_

#include "ArrayObject.h"


struct PdfGraphicPattern
{
	struct PdfBaseObject base;
	char *type;            // (Optional)
	int patternType;
	int shading;           // dictionary or stream... defining the shading patterns gradient fill
	//struct ArrayObject *matrix;   // default identity [1 0 0 1 0 0]   (Optional)
};


DLLEXPORT_TEST_FUNCTION struct PdfGraphicPattern *PdfGraphicPattern_Create(int shadingDictionary, struct PdfDocument *document);
DLLEXPORT_TEST_FUNCTION void PdfGraphicPattern_Init(struct PdfGraphicPattern *self, struct PdfDocument *document, const char* type, int patternType, int shadingDictionary);
DLLEXPORT_TEST_FUNCTION void PdfGraphicPattern_Destroy(struct PdfGraphicPattern *self);
DLLEXPORT_TEST_FUNCTION void PdfGraphicPattern_Cleanup(struct PdfGraphicPattern *self);
DLLEXPORT_TEST_FUNCTION void PdfGraphicPattern_Write(struct PdfGraphicPattern *self, struct StreamWriter *streamWriter);




#endif

