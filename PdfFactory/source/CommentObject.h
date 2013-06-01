/*
CommentObject.h

Author: Radovan Obradovic
Date: 3.7.2008.

Comment object syntax

*/

#ifndef _COMMENTOBJECT_
#define _COMMENTOBJECT_

#include "StreamWriter.h"
#include "PdfFactory.h"

struct CommentObject
{
	struct StreamWriter *streamWriter;
	char *value;
};

DLLEXPORT_TEST_FUNCTION void CommentObject_Write(struct CommentObject *self);
/* Write string object to file. */

DLLEXPORT_TEST_FUNCTION struct CommentObject* CommentObject_Create(struct StreamWriter *streamWriter, char *value);
/* Create string object. */

DLLEXPORT_TEST_FUNCTION struct CommentObject* CommentObject_CreateUnicode(struct StreamWriter *streamWriter, short *value);
/* Create string object, from unicode string. */

DLLEXPORT_TEST_FUNCTION void CommentObject_Init(struct CommentObject *self, struct StreamWriter *streamWriter, char *value);
/* Initializes CommentObject struct. */

DLLEXPORT_TEST_FUNCTION void CommentObject_Cleanup(struct CommentObject *self);
/* Cleanup CommentObject struct. */

DLLEXPORT_TEST_FUNCTION void CommentObject_Destroy(struct CommentObject *self);
/* Destroy CommentObject struct. */

#endif
