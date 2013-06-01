/*
IndirectObject.h

Author: Nebojsa Vislavski
Date: 2.7.2008.	

Writes indirect objects
*/

#ifndef _INDIRECTOBJECT_
#define _INDIRECTOBJECT_

#include "PdfFactory.h"
#include "PdfDocument.h"

struct IndirectObject
{
	struct PdfDocument *document;
	int objectId;
	unsigned long beginObjectOffset; // where in file is this object
	unsigned long endObjectOffset; // offset in bytes where object is ended
    unsigned short generationNumber; // generation number
};


DLLEXPORT_TEST_FUNCTION struct IndirectObject* IndirectObject_Create(struct PdfDocument *document);
/* create new indirect object and take objectid and such things from document */

DLLEXPORT_TEST_FUNCTION void IndirectObject_Init(struct IndirectObject *self, struct PdfDocument *document);
/* Initializes IndirectObject struct. */

DLLEXPORT_TEST_FUNCTION void IndirectObject_Destroy(struct IndirectObject *self);
/* destructor */

DLLEXPORT_TEST_FUNCTION void IndirectObject_BeginNewObject(struct IndirectObject *self);
/* Writte 'iii gg obj'.  It will set beginObjectOffset to correct value. */

DLLEXPORT_TEST_FUNCTION void IndirectObject_EndObject(struct IndirectObject *self);
/* Writte 'endobj'.  It will set beginObjectOffset to correct value. */

#endif
