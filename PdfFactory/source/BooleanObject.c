/*
BoleanObject.h

Author: Radovan Obradovic
Date: 3.7.2008.

Bolean object syntax

*/


#include "BooleanObject.h"
#include "MemoryManager.h"
#include <stdlib.h>

DLLEXPORT_TEST_FUNCTION void BooleanObject_Write(struct BooleanObject *self)
{
	if (self->value != FALSE)
	{
		self->streamWriter->WriteData(self->streamWriter, "true");		
	}
	else
	{
		self->streamWriter->WriteData(self->streamWriter, "false");		
	}
}

DLLEXPORT_TEST_FUNCTION struct BooleanObject* BooleanObject_Create(struct StreamWriter *streamWriter, int value)
{
	struct BooleanObject *x;
	x = (struct BooleanObject*)MemoryManager_Alloc(sizeof(struct BooleanObject));
	// set values
	BooleanObject_Init(x, streamWriter, value);

	return x;
}

DLLEXPORT_TEST_FUNCTION void BooleanObject_Init(struct BooleanObject *self, struct StreamWriter *streamWriter, int value)
{
	self->streamWriter = streamWriter;
	self->value = value;
}

DLLEXPORT_TEST_FUNCTION void BooleanObject_Destroy(struct BooleanObject *self)
{
	MemoryManager_Free(self);	
}
