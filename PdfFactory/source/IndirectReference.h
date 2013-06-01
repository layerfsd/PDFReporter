/*
IndirectReference.h

Author: Radovan Obradovic
Date: 3.7.2008.

Indirect reference syntax.

*/

#ifndef _BOOLEANOBJECT_
#define _BOOLEANOBJECT_

#include "StreamWriter.h"
#include "PdfFactory.h"

struct IndirectReference
{
	struct StreamWriter *streamWriter;
	int objectId;
	int generationNumber;
};

DLLEXPORT_TEST_FUNCTION void IndirectReference_Write(struct IndirectReference *self);
/* Write IndirectReference object to file. */

DLLEXPORT_TEST_FUNCTION struct IndirectReference* IndirectReference_Create(struct StreamWriter *streamWriter,
	int objectId, int generationNumber);
/* Create IndirectReference object. */

DLLEXPORT_TEST_FUNCTION void IndirectReference_Init(struct IndirectReference *self, struct StreamWriter *streamWriter,
	int objectId, int generationNumber);
/* Initializes IndirectReference struct. */

DLLEXPORT_TEST_FUNCTION void IndirectReference_Destroy(struct IndirectReference *self);
/* Destroy IndirectReference struct. */

#endif
