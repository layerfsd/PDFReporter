#ifndef _MEMORYWRITER_H_
#define _MEMORYWRITER_H_

#include "PdfFactory.h"
#include "StreamWriter.h"

struct MemoryWriter
{
	struct StreamWriter base;
	char *memory;
	int size; // size of memory buffer
	int position;
};



DLLEXPORT_TEST_FUNCTION struct MemoryWriter* MemoryWriter_Create();
DLLEXPORT_TEST_FUNCTION void MemoryWriter_Init(struct MemoryWriter *self);
DLLEXPORT_TEST_FUNCTION void MemoryWriter_Cleanup(struct MemoryWriter *self);
DLLEXPORT_TEST_FUNCTION void MemoryWriter_Destroy(struct StreamWriter *self);

DLLEXPORT_TEST_FUNCTION void MemoryWriter_WriteData(struct StreamWriter *self, char *string);
DLLEXPORT_TEST_FUNCTION void MemoryWriter_WriteNewLine(struct StreamWriter *self);
DLLEXPORT_TEST_FUNCTION void MemoryWriter_WriteBinaryData(struct StreamWriter *self, char *data, int size);
DLLEXPORT_TEST_FUNCTION void MemoryWriter_Seek(struct StreamWriter *self, int offset);
DLLEXPORT_TEST_FUNCTION int  MemoryWriter_GetPosition(struct StreamWriter *self);

#endif