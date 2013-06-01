/*
NullObject.h

Author: Radovan Obradovic
Date: 3.7.2008.

Null object syntax

*/

#ifndef _BOOLEANOBJECT_
#define _BOOLEANOBJECT_

#include "PdfDocument.h"
#include "PdfFactory.h"
#include <stdlib.h>

struct NullObject
{
	struct PdfDocument *document;
	int value;
};

DLLEXPORT_TEST_FUNCTION void NullObject_Write(struct NullObject *self);
/* Write null object to file. */

DLLEXPORT_TEST_FUNCTION struct NullObject* NullObject_Create(struct PdfDocument *document);
/* Create null object. */

DLLEXPORT_TEST_FUNCTION void NullObject_Init(struct NullObject *self, struct PdfDocument *document);
/* Initializes NullObject struct. */

DLLEXPORT_TEST_FUNCTION void NullObject_Destroy(struct NullObject *self);
/* Destroy NullObject struct. */

#endif
