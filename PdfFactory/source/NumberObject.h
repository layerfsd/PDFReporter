#ifndef __NUMBER_OBJECT_H
#define __NUMBER_OBJECT_H


#include "PdfFactory.h"
#include "StreamWriter.h"
#include <stdlib.h>

struct NumberObject
{
	struct StreamWriter *streamWriter;
	double value;
};

DLLEXPORT_TEST_FUNCTION struct NumberObject *NumberObject_Create(struct StreamWriter *streamWriter, double value);
/* Creates new NumberObject struct on heap. */

DLLEXPORT_TEST_FUNCTION void NumberObject_Init(struct NumberObject *self, struct StreamWriter *streamWriter, double value);
/* Initializes NumberObject struct. */

DLLEXPORT_TEST_FUNCTION void NumberObject_Write(struct NumberObject *self);
/* Writes NumberObject struct into PDF file. */

DLLEXPORT_TEST_FUNCTION void NumberObject_Destroy(struct NumberObject *self);
/* Destructor for number object */


#endif
