/*
StringObject.h

Author: Radovan Obradovic
Date: 3.7.2008.

String object syntax

*/

#ifndef _STRINGOBJECT_
#define _STRINGOBJECT_

#include "StreamWriter.h"
#include "PdfFactory.h"
#include <stdlib.h>

struct StringObject
{
	struct StreamWriter *streamWriter;
	char *value;
	int isUnicode;
};

DLLEXPORT_TEST_FUNCTION void StringObject_Write(struct StringObject *self);
/* Write string object to file. */

DLLEXPORT_TEST_FUNCTION struct StringObject* StringObject_Create(struct StreamWriter *streamWriter, char *value);
/* Create string object. */

DLLEXPORT_TEST_FUNCTION struct StringObject* StringObject_CreateUnicode(struct StreamWriter *streamWriter, short *value);
/* Create string object, from unicode string. */

DLLEXPORT_TEST_FUNCTION void StringObject_Init(struct StringObject *self, struct StreamWriter *streamWriter, char *value);
/* Initializes StringObject struct. */

DLLEXPORT_TEST_FUNCTION void StringObject_Cleanup(struct StringObject *self);
/* Cleanup StringObject struct. */

DLLEXPORT_TEST_FUNCTION void StringObject_Destroy(struct StringObject *self);
/* Destroy StringObject struct. */

#endif
