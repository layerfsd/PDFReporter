/*
DictionaryObject.c

Author: Nebojsa Vislavski
Date: 30.6.2008.	

DictionaryObject syntax

*/

#include "DictionaryObject.h"
#include "MemoryManager.h"
#include "NameObject.h"
#include <stdlib.h>

DLLEXPORT_TEST_FUNCTION struct DictionaryObject* DictionaryObject_Begin(struct StreamWriter *streamWriter)
{
	struct DictionaryObject *dict;
	dict = (struct DictionaryObject*)MemoryManager_Alloc(sizeof(struct DictionaryObject));
	// set values
	dict->streamWriter = streamWriter;
	dict->streamWriter->WriteData(dict->streamWriter, "<<");
	
	return dict;
}

DLLEXPORT_TEST_FUNCTION void DictionaryObject_End(struct DictionaryObject *self)
{	
	self->streamWriter->WriteNewLine(self->streamWriter);
	self->streamWriter->WriteData(self->streamWriter, ">>"); 	
	self->streamWriter->WriteNewLine(self->streamWriter);
	MemoryManager_Free(self);	
}

DLLEXPORT_TEST_FUNCTION void DictionaryObject_WriteKey(struct DictionaryObject *dictionary, char *key)
{
	struct NameObject *name;
	name = NameObject_Create(dictionary->streamWriter, key);
	dictionary->streamWriter->WriteNewLine(dictionary->streamWriter);	
	NameObject_Write(name);
	NameObject_Destroy(name);
}


