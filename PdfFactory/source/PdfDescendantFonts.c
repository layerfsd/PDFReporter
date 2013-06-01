#include "PdfDescendantFonts.h"
#include "PdfDocument.h"
#include "ArrayObject.h"
#include "MemoryManager.h"
#include "IndirectReference.h"
#include "DictionaryObject.h"


DLLEXPORT_TEST_FUNCTION struct PdfDescendantFonts *PdfDescendantFonts_Create(struct PdfDocument *document)
{
	struct PdfDescendantFonts *x;
	x = (struct PdfDescendantFonts*)MemoryManager_Alloc(sizeof(struct PdfDescendantFonts));
	PdfDescendantFonts_Init(x, document);
	return x;
}





DLLEXPORT_TEST_FUNCTION void PdfDescendantFonts_Init(struct PdfDescendantFonts *self, struct PdfDocument *document)
{
	int i;

	PdfBaseObject_Init(&self->base, document);
	self->refCounter = 0;
	for(i=0; i<10; i++)	self->references[i] = 0;
}




DLLEXPORT_TEST_FUNCTION void PdfDescendantFonts_AddReference(struct PdfDescendantFonts *self, int descendantFontRef)
{
	if(self->refCounter == 10) return;

	self->references[self->refCounter] = descendantFontRef;
	self->refCounter++;
}





DLLEXPORT_TEST_FUNCTION void PdfDescendantFonts_Cleanup(struct PdfDescendantFonts *self)
{
	MemoryManager_Free(self->references);
}




DLLEXPORT_TEST_FUNCTION void PdfDescendantFonts_Destroy(struct PdfDescendantFonts *self)
{
	PdfDescendantFonts_Cleanup(self);
	MemoryManager_Free(self);
}





DLLEXPORT_TEST_FUNCTION void PdfDescendantFonts_Write(struct PdfDescendantFonts *self)
{
	struct DictionaryObject *dict;
	struct IndirectReference *indRef;
	struct ArrayObject *arrayObj;
	int i;

	PdfDocument_BeginNewObject((struct PdfBaseObject*)self);
	dict = DictionaryObject_Begin(self->base.document->streamWriter);

	arrayObj = ArrayObject_BeginArray(self->base.document->streamWriter);

	for(i=0; i<self->refCounter; i++)
	{
		indRef = IndirectReference_Create(self->base.document->streamWriter, self->references[i], 0);
		IndirectReference_Write(indRef);
	}

	ArrayObject_EndArray(arrayObj);

	DictionaryObject_End(dict);
	PdfDocument_EndObject((struct PdfBaseObject*)self);
}