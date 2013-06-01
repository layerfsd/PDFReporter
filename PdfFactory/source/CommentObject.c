/*
CommentObject.h

Author: Radovan Obradovic
Date: 3.7.2008.

Comment object syntax

*/


#include "CommentObject.h"
#include "MemoryManager.h"
#include <string.h>
#include <stdlib.h>

DLLEXPORT_TEST_FUNCTION void CommentObject_Write(struct CommentObject *self)
{
	self->streamWriter->WriteData(self->streamWriter, "%");	
	self->streamWriter->WriteData(self->streamWriter, self->value);	
}

DLLEXPORT_TEST_FUNCTION struct CommentObject* CommentObject_Create(struct StreamWriter *streamWriter, char *value)
{
	struct CommentObject *x;
	x = (struct CommentObject*)MemoryManager_Alloc(sizeof(struct CommentObject));
	// set values
	CommentObject_Init(x, streamWriter, value);

	return x;
}

DLLEXPORT_TEST_FUNCTION void CommentObject_Init(struct CommentObject *self, struct StreamWriter *streamWriter, char *value)
{
	self->streamWriter = streamWriter;
	self->value = (char*)MemoryManager_Alloc(strlen(value)+1);
	strcpy(self->value, value);		
}

DLLEXPORT_TEST_FUNCTION struct CommentObject* CommentObject_CreateUnicode(struct StreamWriter *streamWriter, short *value)
{
	// TODO: Implement this method.
	// TODO: Which unicode library to use?
	return 0;
}

DLLEXPORT_TEST_FUNCTION void CommentObject_Cleanup(struct CommentObject *self)
{	
	MemoryManager_Free(self->value);
}

DLLEXPORT_TEST_FUNCTION void CommentObject_Destroy(struct CommentObject *self)
{
	CommentObject_Cleanup(self);
	MemoryManager_Free(self);	
}
