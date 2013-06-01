#include <NumberObject.h>
#include "MemoryManager.h"
#include <stdlib.h>
#include <math.h>
#include <stdio.h>

DLLEXPORT_TEST_FUNCTION struct NumberObject *NumberObject_Create(struct StreamWriter *streamWriter, double value)
{
	struct NumberObject *x;
	x = (struct NumberObject *)MemoryManager_Alloc(sizeof(struct NumberObject));
	NumberObject_Init(x, streamWriter, value);
	return x;
}

DLLEXPORT_TEST_FUNCTION void NumberObject_Init(struct NumberObject *self, struct StreamWriter *streamWriter, double value)
{
	self->streamWriter = streamWriter;
	self->value = value;
}

DLLEXPORT_TEST_FUNCTION void NumberObject_Destroy(struct NumberObject *self)
{	
	MemoryManager_Free(self);
}

DLLEXPORT_TEST_FUNCTION void NumberObject_Write(struct NumberObject *self)
{
	char tmp[100];
    if (floor(self->value) == self->value)
	{
		sprintf(tmp, "%d ", (int)self->value);
	}
	else 
	{
		sprintf(tmp, "%f ", self->value);
	}
	self->streamWriter->WriteData(self->streamWriter, tmp);	
}
