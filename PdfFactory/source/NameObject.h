/*
NameObject.h

Author: Nebojsa Vislavski
Date: 30.6.2008.	

Name object syntax

*/

#ifndef _NAMEOBJECT_
#define _NAMEOBJECT_

#include "StreamWriter.h"
#include "PdfFactory.h"

struct NameObject
{
	struct StreamWriter *streamWriter;
	char *value;
};

DLLEXPORT_TEST_FUNCTION void NameObject_Write(struct NameObject *name);
/* Write name object to file */

DLLEXPORT_TEST_FUNCTION struct NameObject* NameObject_Create(struct StreamWriter *streamWriter, char *value);
/* Create name object */

DLLEXPORT_TEST_FUNCTION void NameObject_Init(struct NameObject *self, struct StreamWriter *streamWriter, char *value);
/* Initializes NumberObject struct. */

DLLEXPORT_TEST_FUNCTION void NameObject_Destroy(struct NameObject *name);
/* Destroy name object */

#endif
