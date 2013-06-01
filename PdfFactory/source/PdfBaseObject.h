#ifndef _PDFBASEOBJECT_
#define _PDFBASEOBJECT_

#include "PdfFactory.h"


// base for all objects. 
struct PdfBaseObject
{
	int type; // not used yet. set to 0
    struct PdfDocument *document;
	int objectId;
	int generationNumber;
};

DLLEXPORT_TEST_FUNCTION void PdfBaseObject_Init(struct PdfBaseObject *self, struct PdfDocument *document);
/* Initializes struct. */

#endif
