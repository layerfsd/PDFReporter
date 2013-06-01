/*
IndirectReference.h

Author: Radovan Obradovic
Date: 3.7.2008.

Indirect reference syntax.

*/



#include "IndirectReference.h"
#include "MemoryManager.h"
#include <stdio.h>
#include <stdlib.h>

DLLEXPORT_TEST_FUNCTION void IndirectReference_Write(struct IndirectReference *self)
{
	char tmp[100];
	sprintf(tmp, "%d %d R ", self->objectId, self->generationNumber);
	self->streamWriter->WriteData(self->streamWriter, tmp);	
}

DLLEXPORT_TEST_FUNCTION struct IndirectReference* IndirectReference_Create(struct StreamWriter *streamWriter,
	int objectId, int generationNumber)
{
	struct IndirectReference *x;
	x = (struct IndirectReference*)MemoryManager_Alloc(sizeof(struct IndirectReference));
	IndirectReference_Init(x, streamWriter, objectId, generationNumber);

	return x;
}

DLLEXPORT_TEST_FUNCTION void IndirectReference_Init(struct IndirectReference *self, struct StreamWriter *streamWriter,
	int objectId, int generationNumber)
{
	self->streamWriter = streamWriter;
	self->objectId = objectId;
	self->generationNumber = generationNumber;
}

DLLEXPORT_TEST_FUNCTION void IndirectReference_Destroy(struct IndirectReference *self)
{	
	MemoryManager_Free(self);
}
