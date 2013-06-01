#include "PdfContentStream.h"
#include "StreamObject.h"
#include "PdfBaseObject.h"
#include "DictionaryObject.h"
#include "DLList.h"
#include "MemoryManager.h"
#include "MemoryWriter.h"
#include <string.h>
#include <stdlib.h>


DLLEXPORT_TEST_FUNCTION void PdfContentStream_Init(struct PdfContentStream *self, struct PdfDocument *document)
{
	PdfBaseObject_Init((struct PdfBaseObject*)self, document);
	self->lengthOffsetPlace = 0;	
	self->stream = 0;
	self->enableCompression = 1;
	//self->streamWriter = 0;
}

DLLEXPORT_TEST_FUNCTION struct PdfContentStream* PdfContentStream_Begin(struct PdfDocument *document, char useCompression)
{
	struct PdfContentStream *content;	
	content = (struct PdfContentStream*)MemoryManager_Alloc(sizeof(struct PdfContentStream));
	PdfContentStream_Init(content, document);	
	DLList_PushBack(document->contentStreams, content);

	content->writeStarted = TRUE;

	// begin stream, choose to compress or not
	if(useCompression)
	{
		content->stream = StreamObject_BeginInMemory(NULL);					//With	  compression
		content->enableCompression = 1;
	}else{
		content->stream = StreamObject_Begin(NULL);						//Without compression
		content->enableCompression = 0;
	}
	

	return content;
}

/***********************************************************************
Write content from memory to document
/************************************************************************/
DLLEXPORT_TEST_FUNCTION void PdfContentStream_Write(struct PdfContentStream *self)
{
	struct DictionaryObject *dict;
	struct MemoryWriter *memoryWriter = (struct MemoryWriter*)self->stream->streamWriter;
	char tmp[10];


	
	if (!self->writeStarted)
	{
		// write content stream to document
		PdfDocument_BeginNewObject((struct PdfBaseObject*)self);
		{
			dict = DictionaryObject_Begin(self->base.document->streamWriter);
			{
				DictionaryObject_WriteKey(dict, "Length");
				sprintf(tmp, "%d", self->stream->length);
				// write length
				self->base.document->streamWriter->WriteData(self->base.document->streamWriter, tmp);
				
				if(self->enableCompression) // If there is compressed data put decoding information
				{
					self->base.document->streamWriter->WriteNewLine(self->base.document->streamWriter);
					self->base.document->streamWriter->WriteData(self->base.document->streamWriter, "/Filter /FlateDecode");
				}
			
			}
			DictionaryObject_End(dict);
			
			// write stream object stored in memory to document
			self->base.document->streamWriter->WriteBinaryData(self->base.document->streamWriter, memoryWriter->memory, memoryWriter->size);

		}
		PdfDocument_EndObject((struct PdfBaseObject*)self);	
	}
	else 
	{
#ifdef _DEBUG
		printf("PdfContentStream_Write: Cannot save content stream while writting\n");
#endif
	}
}




DLLEXPORT_TEST_FUNCTION void PdfContentStream_End(struct PdfContentStream *self)
{	
	// end stream
	StreamObject_End(self->stream);
	self->writeStarted = FALSE;
}


DLLEXPORT_TEST_FUNCTION void PdfContentStream_Destroy(struct PdfContentStream *self)
{
	StreamObject_Destroy(self->stream);	
//	self->streamWriter->Destroy(self->streamWriter);
	MemoryManager_Free(self);
}

