#include "StreamWriter.h"
#include "MemoryManager.h"

DLLEXPORT_TEST_FUNCTION struct StreamWriter* StreamWriter_Create()
{
	struct StreamWriter *x;
	x = (struct StreamWriter*)MemoryManager_Alloc(sizeof(struct StreamWriter));
	// set values
	StreamWriter_Init(x);
	return x;
}

DLLEXPORT_TEST_FUNCTION void StreamWriter_Init(struct StreamWriter *self)
{
	self->WriteBinaryData = StreamWriter_WriteBinaryData;
	self->WriteNewLine = StreamWriter_WriteNewLine;
	self->WriteData = StreamWriter_WriteData;
	self->Destroy = StreamWriter_Destroy;
	self->Seek = StreamWriter_Seek;
	self->GetPosition = StreamWriter_GetPosition;
}

DLLEXPORT_TEST_FUNCTION void StreamWriter_Cleanup(struct StreamWriter *self)
{
	
}

DLLEXPORT_TEST_FUNCTION void StreamWriter_Destroy(struct StreamWriter *self)
{
	StreamWriter_Cleanup(self);
	MemoryManager_Free(self);
}


void StreamWriter_WriteData(struct StreamWriter *self, char *string)
{

}

void StreamWriter_WriteNewLine(struct StreamWriter *self)
{

}

void StreamWriter_WriteBinaryData(struct StreamWriter *self, char *data, int size)
{

}

void StreamWriter_Seek(struct StreamWriter *self, int offset)
{

}

int StreamWriter_GetPosition(struct StreamWriter *self)
{
	return 0;
}
