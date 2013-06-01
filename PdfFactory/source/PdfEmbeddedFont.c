#include "PdfEmbeddedFont.h"
#include "MemoryManager.h"
#include "PdfDocument.h"
#include "DictionaryObject.h"
#include "NameObject.h"
#include "NumberObject.h"
#include "StreamObject.h"
#include "MemoryWriter.h"
#include <string.h>
#include <stdlib.h>


DLLEXPORT_TEST_FUNCTION struct PdfEmbeddedFont *PdfEmbeddedFont_Create(struct PdfDocument *document, char *fontData, int fontDataSize)
{
	struct PdfEmbeddedFont *x;
	x = (struct PdfEmbeddedFont*)MemoryManager_Alloc(sizeof(struct PdfEmbeddedFont));
	PdfEmbeddedFont_Init(x, document);

	x->fontData = fontData;
	x->fontDataSize = fontDataSize;
	return x;
}

DLLEXPORT_TEST_FUNCTION void PdfEmbeddedFont_Init(struct PdfEmbeddedFont *self, struct PdfDocument *document)
{
	PdfBaseObject_Init(&self->base, document);
	self->fileName = 0;
	self->fontData = 0;
	self->fontDataSize = 0;

	DLList_PushBack(document->embeddedFonts, self);  // add self to documents list
}

void PdfEmbeddedFont_Cleanup(struct PdfEmbeddedFont *self)
{
	if (self->fileName)
	{
		MemoryManager_Free(self->fileName);
		self->fileName = 0;
	}
}

void PdfEmbeddedFont_Destroy(struct PdfEmbeddedFont *self)
{
	PdfEmbeddedFont_Cleanup(self);
	MemoryManager_Free(self);
}

DLLEXPORT_TEST_FUNCTION void PdfEmbeddedFont_WriteTrueType(struct PdfEmbeddedFont *self)
{
	struct DictionaryObject *dict;
	struct NameObject *name;
	struct StreamObject *fontStream;
	struct MemoryWriter *memoryWriter;
	char tmp[20];	

	// Write font information

	PdfDocument_BeginNewObject((struct PdfBaseObject*)self);
	dict = DictionaryObject_Begin(self->base.document->streamWriter);
	{		
		// this will make in memory font stream and compress it automatically
		fontStream = StreamObject_BeginInMemory();
		fontStream->streamWriter->WriteBinaryData(fontStream->streamWriter, self->fontData, self->fontDataSize);
		StreamObject_End(fontStream);
		memoryWriter = (struct MemoryWritter*)fontStream->streamWriter;
		
		self->base.document->streamWriter->WriteNewLine(self->base.document->streamWriter);
		self->base.document->streamWriter->WriteData(self->base.document->streamWriter, "/Filter /FlateDecode");

		DictionaryObject_WriteKey(dict, "Length");
		sprintf(tmp, "%d", fontStream->length);
		// write length
		self->base.document->streamWriter->WriteData(self->base.document->streamWriter, tmp);

		DictionaryObject_WriteKey(dict, "Length1");
		sprintf(tmp, "%d", self->fontDataSize);
		// write length
		self->base.document->streamWriter->WriteData(self->base.document->streamWriter, tmp);
	}
	DictionaryObject_End(dict);

	// write data to stream		
	self->base.document->streamWriter->WriteBinaryData(self->base.document->streamWriter, memoryWriter->memory, memoryWriter->size);
	PdfDocument_EndObject((struct PdfBaseObject*)self);
}

// This is test method
DLLEXPORT_TEST_FUNCTION void PdfEmbeddedFont_LoadFromFile(struct PdfEmbeddedFont *self, char *fileName)
{
	FILE *f;
	f = fopen(fileName, "rb");
	if (f)
	{
		fseek(f, 0, SEEK_END);
		self->fontDataSize = ftell(f);
		fseek(f, 0, SEEK_SET);
		self->fontData = MemoryManager_Alloc(self->fontDataSize);
		// read all file into memory
		fread(self->fontData, 1, self->fontDataSize, f);
	}
	fclose(f);
}