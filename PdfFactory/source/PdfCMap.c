#include "PdfCMap.h"
#include "PdfDocument.h"
#include "MemoryManager.h"
#include "StringObject.h"
#include "NumberObject.h"
#include "NameObject.h"
#include "StreamObject.h"
#include "DictionaryObject.h"


DLLEXPORT_TEST_FUNCTION struct PdfCMap *PdfCMap_Create(struct PdfDocument *document)
{
	struct PdfCMap *x;
	x = (struct PdfCMap*)MemoryManager_Alloc(sizeof(struct PdfCMap));
	PdfCMap_Init(x, document);
	return x;
}

DLLEXPORT_TEST_FUNCTION void PdfCMap_Init(struct PdfCMap *self, struct PdfDocument *document)
{
	PdfBaseObject_Init(&self->base, document);
	self->length = 0;
}

DLLEXPORT_TEST_FUNCTION void PdfCMap_Cleanup(struct PdfCMap *self)
{
}

DLLEXPORT_TEST_FUNCTION void PdfCMap_Destroy(struct PdfCMap *self)
{
	PdfCMap_Cleanup(self);
	MemoryManager_Free(self);
}

DLLEXPORT_TEST_FUNCTION void PdfCMap_Write(struct PdfCMap *self)
{
	struct DictionaryObject *dict;
	struct StreamObject *stream;
	struct NameObject *name;
	struct NumberObject *number;
	int pos, tmpPos;


	PdfDocument_BeginNewObject((struct PdfBaseObject*)self);
	dict = DictionaryObject_Begin(self->base.document->streamWriter);

	DictionaryObject_WriteKey(dict, "Length");
	pos = self->base.document->streamWriter->GetPosition(self->base.document->streamWriter);
	self->base.document->streamWriter->WriteData(self->base.document->streamWriter, "          ");

	DictionaryObject_End(dict);



	stream = StreamObject_Begin(self->base.document->streamWriter);
	
	DictionaryObject_WriteKey(dict, "CIDInit");
	name = NameObject_Create(self->base.document->streamWriter, "ProcSet findresource begin");
	NameObject_Write(name);
	NameObject_Destroy(name);

	self->base.document->streamWriter->WriteNewLine(self->base.document->streamWriter);
	self->base.document->streamWriter->WriteData(self->base.document->streamWriter,"12 dict begin");
	self->base.document->streamWriter->WriteNewLine(self->base.document->streamWriter);
	self->base.document->streamWriter->WriteData(self->base.document->streamWriter,"begincmap");
	self->base.document->streamWriter->WriteNewLine(self->base.document->streamWriter);
	self->base.document->streamWriter->WriteData(self->base.document->streamWriter,"CIDSystemInfo");
	self->base.document->streamWriter->WriteNewLine(self->base.document->streamWriter);
	self->base.document->streamWriter->WriteData(self->base.document->streamWriter,"<< /Registry (Adobe)");
	self->base.document->streamWriter->WriteNewLine(self->base.document->streamWriter);
	self->base.document->streamWriter->WriteData(self->base.document->streamWriter,"/Ordering (UCS) /Suplement 0 >> def");
	self->base.document->streamWriter->WriteNewLine(self->base.document->streamWriter);
	self->base.document->streamWriter->WriteData(self->base.document->streamWriter,"/CMapName /Adobe-Identity-UCS def");
	self->base.document->streamWriter->WriteNewLine(self->base.document->streamWriter);
	self->base.document->streamWriter->WriteData(self->base.document->streamWriter,"/CMapType 2 def");
	self->base.document->streamWriter->WriteNewLine(self->base.document->streamWriter);
	self->base.document->streamWriter->WriteData(self->base.document->streamWriter,"1 begincodespacerange");
	self->base.document->streamWriter->WriteNewLine(self->base.document->streamWriter);
	self->base.document->streamWriter->WriteData(self->base.document->streamWriter,"<0000> <FFFF>");
	self->base.document->streamWriter->WriteNewLine(self->base.document->streamWriter);
	self->base.document->streamWriter->WriteData(self->base.document->streamWriter,"endcodespacerange");
	self->base.document->streamWriter->WriteNewLine(self->base.document->streamWriter);
	self->base.document->streamWriter->WriteData(self->base.document->streamWriter,"15 beginbfchar");
	self->base.document->streamWriter->WriteNewLine(self->base.document->streamWriter);

	self->base.document->streamWriter->WriteData(self->base.document->streamWriter,"<038D> <FE8D>");
	self->base.document->streamWriter->WriteNewLine(self->base.document->streamWriter);
	self->base.document->streamWriter->WriteData(self->base.document->streamWriter,"<0391> <FE91>");
	self->base.document->streamWriter->WriteNewLine(self->base.document->streamWriter);
	self->base.document->streamWriter->WriteData(self->base.document->streamWriter,"<0394> <FE94>");
	self->base.document->streamWriter->WriteNewLine(self->base.document->streamWriter);
	self->base.document->streamWriter->WriteData(self->base.document->streamWriter,"<03A9> <FEA9>");
	self->base.document->streamWriter->WriteNewLine(self->base.document->streamWriter);
	self->base.document->streamWriter->WriteData(self->base.document->streamWriter,"<03AE> <FEAE>");
	self->base.document->streamWriter->WriteNewLine(self->base.document->streamWriter);
	self->base.document->streamWriter->WriteData(self->base.document->streamWriter,"<03B4> <FEB4>");
	self->base.document->streamWriter->WriteNewLine(self->base.document->streamWriter);
	self->base.document->streamWriter->WriteData(self->base.document->streamWriter,"<03CC> <FECC>");
	self->base.document->streamWriter->WriteNewLine(self->base.document->streamWriter);
	self->base.document->streamWriter->WriteData(self->base.document->streamWriter,"<03DC> <FEDC>");
	self->base.document->streamWriter->WriteNewLine(self->base.document->streamWriter);
	self->base.document->streamWriter->WriteData(self->base.document->streamWriter,"<03DF> <FEDF>");
	self->base.document->streamWriter->WriteNewLine(self->base.document->streamWriter);
	self->base.document->streamWriter->WriteData(self->base.document->streamWriter,"<03E0> <FEE0>");
	self->base.document->streamWriter->WriteNewLine(self->base.document->streamWriter);
	self->base.document->streamWriter->WriteData(self->base.document->streamWriter,"<03E3> <FEE3>");
	self->base.document->streamWriter->WriteNewLine(self->base.document->streamWriter);
	self->base.document->streamWriter->WriteData(self->base.document->streamWriter,"<03E4> <FEE4>");
	self->base.document->streamWriter->WriteNewLine(self->base.document->streamWriter);
	self->base.document->streamWriter->WriteData(self->base.document->streamWriter,"<03EE> <FEEE>");
	self->base.document->streamWriter->WriteNewLine(self->base.document->streamWriter);
	self->base.document->streamWriter->WriteData(self->base.document->streamWriter,"<03F3> <FEF3>");
	self->base.document->streamWriter->WriteNewLine(self->base.document->streamWriter);
	self->base.document->streamWriter->WriteData(self->base.document->streamWriter,"<03F4> <FEF4>");
	self->base.document->streamWriter->WriteNewLine(self->base.document->streamWriter);

	self->base.document->streamWriter->WriteData(self->base.document->streamWriter,"endbfchar");
	self->base.document->streamWriter->WriteNewLine(self->base.document->streamWriter);
	self->base.document->streamWriter->WriteData(self->base.document->streamWriter,"endcmap CMapName currentdict /CMap defineresource pop end end");
	self->base.document->streamWriter->WriteNewLine(self->base.document->streamWriter);
	
	StreamObject_End(stream);



	tmpPos = self->base.document->streamWriter->GetPosition(self->base.document->streamWriter);
	self->base.document->streamWriter->Seek(self->base.document->streamWriter, pos);
	//Write stream length
	self->length = stream->length;
	number = NumberObject_Create(self->base.document->streamWriter, self->length);
	NumberObject_Write(number);
	NumberObject_Destroy(number);
	self->base.document->streamWriter->Seek(self->base.document->streamWriter, tmpPos);

	StreamObject_Destroy(stream);
	PdfDocument_EndObject((struct PdfBaseObject*)self);
}