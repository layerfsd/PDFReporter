/*
NameObject.c

Author: Nebojsa Vislavski
Date: 30.6.2008.	

Name object syntax

*/


#include "NameObject.h"
#include <string.h>
#include <stdlib.h>
#include "MemoryManager.h"

DLLEXPORT_TEST_FUNCTION void NameObject_Write(struct NameObject *name)
{
	name->streamWriter->WriteData(name->streamWriter, "/");
	name->streamWriter->WriteData(name->streamWriter, name->value);
	name->streamWriter->WriteData(name->streamWriter, " ");	
}

DLLEXPORT_TEST_FUNCTION struct NameObject* NameObject_Create(struct StreamWriter *streamWriter, char *value)
{
	struct NameObject *name;
	name = (struct NameObject*)MemoryManager_Alloc(sizeof(struct NameObject));
	// set values
	NameObject_Init(name, streamWriter, value);
	
	return name;
}

DLLEXPORT_TEST_FUNCTION void NameObject_Init(struct NameObject *self, struct StreamWriter *streamWriter, char *value)
{
	self->streamWriter = streamWriter;	
	//self->value = (char*)MemoryManager_Alloc(strlen(value)+1);
	//strcpy(self->value, value);		
	self->value = MemoryManager_StrDup(value);


}


DLLEXPORT_TEST_FUNCTION void NameObject_Destroy(struct NameObject *name)
{	
	MemoryManager_Free(name->value);
	MemoryManager_Free(name);
	
}
