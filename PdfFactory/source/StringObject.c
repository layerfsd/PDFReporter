/*
StringObject.h

Author: Radovan Obradovic
Date: 3.7.2008.

String object syntax

*/


#include "StringObject.h"
#include "MemoryManager.h"
#include <string.h>
#include <ctype.h>

DLLEXPORT_TEST_FUNCTION void StringObject_Write(struct StringObject *self)
{
	// TODO: Take care of special chars.
	// TODO: Implement unicode.
	self->streamWriter->WriteData(self->streamWriter, "(");
	self->streamWriter->WriteData(self->streamWriter, self->value);
	self->streamWriter->WriteData(self->streamWriter, ")");	
}

DLLEXPORT_TEST_FUNCTION struct StringObject* StringObject_Create(struct StreamWriter *streamWriter, char *value)
{
	struct StringObject *x;
	x = (struct StringObject*)MemoryManager_Alloc(sizeof(struct StringObject));
	// set values
	StringObject_Init(x, streamWriter, value);

	return x;
}

DLLEXPORT_TEST_FUNCTION void StringObject_Init(struct StringObject *self, struct StreamWriter *streamWriter, char *value)
{
	self->streamWriter = streamWriter;	
	self->value = (char*)MemoryManager_Alloc(strlen(value)+1);
	strcpy(self->value, value);		
}

DLLEXPORT_TEST_FUNCTION struct StringObject* StringObject_CreateUnicode(struct StreamWriter *streamWriter, short *value)
{
	// TODO: Implement this method.
	// TODO: Which unicode library to use?
	return 0;
}

DLLEXPORT_TEST_FUNCTION void StringObject_Cleanup(struct StringObject *self)
{	
	MemoryManager_Free(self->value);
}

DLLEXPORT_TEST_FUNCTION void StringObject_Destroy(struct StringObject *self)
{
	StringObject_Cleanup(self);	
	MemoryManager_Free(self);
}
