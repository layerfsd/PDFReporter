#include "PdfCIDFont.h"
#include "PdfDocument.h"
#include "IndirectReference.h"
#include "NameObject.h"
#include "NumberObject.h"
#include "MemoryManager.h"
#include "DictionaryObject.h"




DLLEXPORT_TEST_FUNCTION struct PdfCIDFont *PdfCIDFont_Create(struct PdfDocument *document)
{
	struct PdfCIDFont *x;
	x = (struct PdfCIDFont*)MemoryManager_Alloc(sizeof(struct PdfCIDFont));
	PdfCIDFont_Init(x, document);
	return x;
}




DLLEXPORT_TEST_FUNCTION void PdfCIDFont_Init(struct PdfCIDFont *self, struct PdfDocument *document)
{
	PdfBaseObject_Init(&self->base, document);
	
	self->baseFont = MemoryManager_StrDup(" ");
	self->cidSystemInfo = 0;
	self->defaultWidth = 1000;
	self->fontDescriptor = 0;
	self->subtype = MemoryManager_StrDup(PDF_CID_FONT_TYPE_2);
}




DLLEXPORT_TEST_FUNCTION void PdfCIDFont_Cleanup(struct PdfCIDFont *self)
{
	if(self->baseFont)
	{
		MemoryManager_Free(self->baseFont);
		self->baseFont = 0;
	}

	if(self->subtype)
	{
		MemoryManager_Free(self->subtype);
		self->subtype = 0;
	}
}





DLLEXPORT_TEST_FUNCTION void PdfCIDFont_Destroy(struct PdfCIDFont *self)
{
	PdfCIDFont_Cleanup(self);
	MemoryManager_Free(self);
}





DLLEXPORT_TEST_FUNCTION void PdfCIDFont_Write(struct PdfCIDFont *self)
{
	struct DictionaryObject *dict;
	struct NameObject *name;
	struct NumberObject *number;
	struct IndirectReference *indRef;
	

	PdfDocument_BeginNewObject((struct PdfBaseObject*)self);
	dict = DictionaryObject_Begin(self->base.document->streamWriter);

	DictionaryObject_WriteKey(dict, "Type");
	name = NameObject_Create(self->base.document->streamWriter, "Font");
	NameObject_Write(name);
	NameObject_Destroy(name);

	DictionaryObject_WriteKey(dict, "Subtype");
	name = NameObject_Create(self->base.document->streamWriter, self->subtype);
	NameObject_Write(name);
	NameObject_Destroy(name);

	DictionaryObject_WriteKey(dict, "BaseFont");
	name = NameObject_Create(self->base.document->streamWriter, self->baseFont);
	NameObject_Write(name);
	NameObject_Destroy(name);

	DictionaryObject_WriteKey(dict, "DW");
	number = NumberObject_Create(self->base.document->streamWriter, self->defaultWidth);
	NumberObject_Write(number);
	NumberObject_Destroy(number);

	DictionaryObject_WriteKey(dict, "CIDSystemInfo");
	indRef = IndirectReference_Create(self->base.document->streamWriter, self->cidSystemInfo, 0);
	IndirectReference_Write(indRef);
	IndirectReference_Destroy(indRef);

	DictionaryObject_WriteKey(dict, "FontDescriptor");
	indRef = IndirectReference_Create(self->base.document->streamWriter, self->fontDescriptor, 0);
	IndirectReference_Write(indRef);
	IndirectReference_Destroy(indRef);

	DictionaryObject_End(dict);
	PdfDocument_EndObject((struct PdfBaseObject*)self);
}




DLLEXPORT_TEST_FUNCTION void PdfCIDFont_SetBaseFont(struct PdfCIDFont *self, char *baseFontName)
{
	self->baseFont = MemoryManager_StrDup(baseFontName);
}




DLLEXPORT_TEST_FUNCTION void PdfCIDFont_SetFontDescriptor(struct PdfCIDFont *self, int toFontDescriptor)
{
	self->fontDescriptor = toFontDescriptor;
}




DLLEXPORT_TEST_FUNCTION void PdfCIDFont_SetCIDSystemInfo(struct PdfCIDFont *self, int toCIDSystemInfo)
{
	self->cidSystemInfo = toCIDSystemInfo;
}
