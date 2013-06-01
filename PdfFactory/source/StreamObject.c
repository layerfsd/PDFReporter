#include "StreamObject.h"
#include "DictionaryObject.h"
#include "NumberObject.h"
#include "PdfBaseObject.h"
#include "MemoryManager.h"
#include "MemoryWriter.h"
#include "PdfFilter.h"

DLLEXPORT_TEST_FUNCTION void StreamObject_Init(struct StreamObject *self, struct StreamWriter *streamWriter)
{	
	self->streamWriter = streamWriter;
	self->beginStreamOffset = 0;
	self->endStreamOffset = 0;
	self->length = 0;
	self->beginInMemory = 0;
}

/*  
  stream
  ... stream data
  endstream
  
*/
DLLEXPORT_TEST_FUNCTION struct StreamObject* StreamObject_Begin(struct StreamWriter *streamWriter)
{
	struct StreamObject *stream;
	stream = (struct StreamObject*)MemoryManager_Alloc(sizeof(struct StreamObject));
	if(streamWriter == NULL) streamWriter = (struct StreamWriter*)MemoryWriter_Create();

	StreamObject_Init(stream, streamWriter);	

	stream->streamWriter->WriteData(stream->streamWriter, "stream");
	stream->streamWriter->WriteNewLine(stream->streamWriter);
	stream->beginStreamOffset = stream->streamWriter->GetPosition(stream->streamWriter);

	return stream;
}


DLLEXPORT_TEST_FUNCTION void StreamObject_End(struct StreamObject *self)
{
	self->endStreamOffset = self->streamWriter->GetPosition(self->streamWriter);
	self->length = self->endStreamOffset - self->beginStreamOffset;

	if(self->beginInMemory) 
	{ 
		StreamObject_ApplyFilter(self); 
		return; 
	}
 
	self->streamWriter->WriteNewLine(self->streamWriter);
	self->streamWriter->WriteData(self->streamWriter, "endstream");	
	self->streamWriter->WriteNewLine(self->streamWriter);
}


DLLEXPORT_TEST_FUNCTION void StreamObject_Destroy(struct StreamObject *self)
{	
	MemoryManager_Free(self);
}


DLLEXPORT_TEST_FUNCTION struct StreamObject* StreamObject_BeginInMemory()
{
	struct StreamObject *stream;
	struct StreamWriter *streamWriter;

	streamWriter = (struct StreamWriter*)MemoryWriter_Create();	
	stream = (struct StreamObject*)MemoryManager_Alloc(sizeof(struct StreamObject));
	
	StreamObject_Init(stream, streamWriter);
	stream->beginInMemory = 1;
	
	return stream;
}



void StreamObject_ApplyFilter(struct StreamObject *self)
{
	struct StreamWriter *streamWriter;
	struct MemoryWriter *memWriter;

	PdfFilter_FlateDecode(self);

	memWriter = (struct MemoryWriter*)self->streamWriter;
	streamWriter = (struct StreamWriter*)MemoryWriter_Create();

	streamWriter->WriteData(streamWriter, "stream");
	streamWriter->WriteNewLine(streamWriter);

	streamWriter->WriteBinaryData(streamWriter, memWriter->memory, memWriter->size);

	streamWriter->WriteNewLine(streamWriter);
	streamWriter->WriteData(streamWriter, "endstream");
	streamWriter->WriteNewLine(streamWriter);

	MemoryManager_Free(self->streamWriter);
	self->streamWriter = streamWriter;
}