#include "MemoryWriter.h"
#include "MemoryManager.h"
#include <stdio.h>

DLLEXPORT_TEST_FUNCTION struct MemoryWriter* MemoryWriter_Create()
{
	struct MemoryWriter *x;
	x = (struct MemoryWriter*)MemoryManager_Alloc(sizeof(struct MemoryWriter));
	// set values
	MemoryWriter_Init(x);
	return x;
}

DLLEXPORT_TEST_FUNCTION void MemoryWriter_Init(struct MemoryWriter *self)
{
	StreamWriter_Init((struct StreamWriter*)self);	
	self->memory = 0;
	self->size = 0;
	self->position = 0;
	self->base.Destroy = MemoryWriter_Destroy;
	self->base.WriteBinaryData = MemoryWriter_WriteBinaryData;
	self->base.WriteData = MemoryWriter_WriteData;
	self->base.WriteNewLine = MemoryWriter_WriteNewLine;
	self->base.GetPosition = MemoryWriter_GetPosition;
	self->base.Seek = MemoryWriter_Seek;
}

DLLEXPORT_TEST_FUNCTION void MemoryWriter_Cleanup(struct MemoryWriter *self)
{
	if (self->memory)
	{
		MemoryManager_Free(self->memory);		
		self->memory = 0;
		self->size = 0;
		self->position = 0;
	}	
}

DLLEXPORT_TEST_FUNCTION void MemoryWriter_Destroy(struct StreamWriter *self)
{
	struct MemoryWriter *memoryWriter = (struct MemoryWriter *)self;
	MemoryWriter_Cleanup(memoryWriter);

	MemoryManager_Free(memoryWriter);	
}


DLLEXPORT_TEST_FUNCTION void MemoryWriter_WriteData(struct StreamWriter *self, char *string)
{	
   unsigned int len;   
   unsigned int pos;   
   int toAlloc;
   struct MemoryWriter *writer = (struct MemoryWriter *)self;
   
   len = strlen(string);
   
   
   toAlloc = len - (writer->size - writer->position);
   if (toAlloc > 0)
   {   
		writer->size += toAlloc;   
		writer->memory = (char *)MemoryManager_ReAlloc((void *)(writer->memory), writer->size);
   }
   
   pos = writer->memory + writer->position;

   // add data 
   MemoryManager_MemCpy((void *)pos, (void*)string, len);
   writer->position += len;   
}

DLLEXPORT_TEST_FUNCTION void MemoryWriter_WriteNewLine(struct StreamWriter *self)
{
	int pos;	
	int toAlloc;
	char c;
	struct MemoryWriter *writer = (struct MemoryWriter *)self;

	c = '\n';	
	toAlloc = 1 - (writer->size - writer->position);
	if (toAlloc > 0)
	{	
		writer->size += toAlloc;		
		writer->memory = (char *)MemoryManager_ReAlloc((void *)(writer->memory), writer->size);
	}
	
	pos = writer->memory + writer->position;	

	// add data 
	MemoryManager_MemCpy((void *)pos, (void*)&c, 1);
	writer->position++;
}

DLLEXPORT_TEST_FUNCTION void MemoryWriter_WriteBinaryData(struct StreamWriter *self, char *data, int size)
{		
	void* pos;
	int toAlloc;
	struct MemoryWriter *writer = (struct MemoryWriter *)self;

	toAlloc = size - (writer->size - writer->position);	
	if (toAlloc > 0)
	{
		writer->size += toAlloc;	
		writer->memory = (char *)MemoryManager_ReAlloc((void *)(writer->memory), writer->size);
	}
	pos = writer->memory + writer->position;

	// add data 
	MemoryManager_MemCpy((void *)pos, (void*)data, size);
	writer->position += size;
}

DLLEXPORT_TEST_FUNCTION void MemoryWriter_Seek(struct StreamWriter *self, int offset)
{
	struct MemoryWriter *writer = (struct MemoryWriter *)self;
	if (offset > writer->size)
	{
		writer->position = writer->size;
	}
	else
	{
		writer->position = offset;
	}	
}

DLLEXPORT_TEST_FUNCTION int MemoryWriter_GetPosition(struct StreamWriter *self)
{
	struct MemoryWriter *writer = (struct MemoryWriter *)self;
	return writer->position;
}
