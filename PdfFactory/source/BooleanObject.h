/*
BoleanObject.h

Author: Radovan Obradovic
Date: 3.7.2008.

Boolean object syntax

*/

#ifndef _BOOLEANOBJECT_
#define _BOOLEANOBJECT_

#include "PdfDocument.h"
#include "PdfFactory.h"
#include "StreamWriter.h"

struct BooleanObject
{
	struct StreamWriter *streamWriter;
	int value;
};

DLLEXPORT_TEST_FUNCTION void BooleanObject_Write(struct BooleanObject *self);
/* Write boolean object to file. */

DLLEXPORT_TEST_FUNCTION struct BooleanObject* BooleanObject_Create(struct StreamWriter *streamWriter, int value);
/* Create boolean object. */

DLLEXPORT_TEST_FUNCTION void BooleanObject_Init(struct BooleanObject *self, struct StreamWriter *streamWriter, int value);
/* Initializes BooleanObject struct. */

DLLEXPORT_TEST_FUNCTION void BooleanObject_Destroy(struct BooleanObject *self);
/* Destroy BooleanObject struct. */

#endif
