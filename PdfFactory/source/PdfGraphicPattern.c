//-----------------------------------------------------------------------------
// Name:	PdfGraphicPattern.c
// Author:	Tomislav Kukic
// Date:	22.12.2008
//-----------------------------------------------------------------------------


#include "PdfGraphicPattern.h"
#include "PdfBaseObject.h"
#include "MemoryManager.h"
#include "DictionaryObject.h"
#include "PdfDocument.h"
#include "IndirectReference.h"
#include "ArrayObject.h"
#include "IndirectReference.h"
#include "NumberObject.h"



DLLEXPORT_TEST_FUNCTION struct PdfGraphicPattern *PdfGraphicPattern_Create(int shadingDictionary, struct PdfDocument *document)
{
	struct PdfGraphicPattern *ret;
	
	ret = (struct PdfGraphicPattern*)MemoryManager_Alloc(sizeof(struct PdfGraphicPattern));
	PdfGraphicPattern_Init(ret, document, "Pattern", 2, shadingDictionary);
	return ret;
}





DLLEXPORT_TEST_FUNCTION void PdfGraphicPattern_Init(struct PdfGraphicPattern *self, struct PdfDocument *document, const char* type, int patternType, int shadingDictionary)
{
	PdfBaseObject_Init(&self->base, document);
	self->type = MemoryManager_StrDup(type);
	self->patternType = patternType;
	self->shading = shadingDictionary;
}





DLLEXPORT_TEST_FUNCTION void PdfGraphicPattern_Destroy(struct PdfGraphicPattern *self)
{
	PdfGraphicPattern_Cleanup(self);
	MemoryManager_Free(self);
}





DLLEXPORT_TEST_FUNCTION void PdfGraphicPattern_Cleanup(struct PdfGraphicPattern *self)
{
	if(self->type)
	{
		MemoryManager_Free(self->type);
		self->type = 0;
	}
}





DLLEXPORT_TEST_FUNCTION void PdfGraphicPattern_Write(struct PdfGraphicPattern *self, struct StreamWriter *streamWriter)
{
	struct DictionaryObject *dO;
	struct ArrayObject *arr;
	struct NameObject *name;
	struct NumberObject *number;
	struct IndirectReference *indRef;

	PdfDocument_BeginNewObject(&self->base);
	{
		dO = DictionaryObject_Begin(streamWriter);

		DictionaryObject_WriteKey(dO, "Type");
		name = NameObject_Create(streamWriter, self->type);
		NameObject_Write(name);
		NameObject_Destroy(name);

		DictionaryObject_WriteKey(dO, "PatternType");
		number = NumberObject_Create(streamWriter, (double)self->patternType);
		NumberObject_Write(number);
		NumberObject_Destroy(number);

		DictionaryObject_WriteKey(dO, "Shading");
		indRef = IndirectReference_Create(streamWriter, self->shading, 0);
		IndirectReference_Write(indRef);
		IndirectReference_Destroy(indRef);
		
		DictionaryObject_WriteKey(dO, "Matrix");
		arr = ArrayObject_BeginArray(streamWriter);
		streamWriter->WriteData(streamWriter, "1 0 0 1 0 0");
		ArrayObject_EndArray(arr);

		DictionaryObject_End(dO);
	}
	PdfDocument_EndObject(&self->base);
}