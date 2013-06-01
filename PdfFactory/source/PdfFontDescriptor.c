#include "PdfFontDescriptor.h"
#include "PdfTemplateEmbeddedFont.h"
#include "MemoryManager.h"
#include "ArrayObject.h"
#include "StringObject.h"
#include "NameObject.h"
#include "NumberObject.h"
#include "DictionaryObject.h"
#include "PdfEmbeddedFont.h"
#include "IndirectReference.h"
#include "PdfFont.h"


DLLEXPORT_TEST_FUNCTION struct PdfFontDescriptor *PdfFontDescriptor_Create(struct PdfDocument *document)
{
	struct PdfFontDescriptor *x;
	x = (struct PdfFontDescriptor*)MemoryManager_Alloc(sizeof(struct PdfFontDescriptor));
	PdfFontDescriptor_Init(x, document);
	return x;
}




DLLEXPORT_TEST_FUNCTION void PdfFontDescriptor_Init(struct PdfFontDescriptor *self, struct PdfDocument *document)
{
	PdfBaseObject_Init(&self->base, document);

	self->ascent = 0;
	self->parentFont = 0;
	self->capHeight = 0;
	self->descent = 0;
	self->flags = 32;
	self->fontBBox = 0;	
	self->fontName = 0;	
	self->italicAngle = -11;
	self->stemV = 0;	
	self->fontWeight = 400;
	self->xHeight = 0;
	self->fontStretch = MemoryManager_StrDup("Normal");
	self->trueTypeEmbeddedFont = 0;
	self->type1EmbeddedFont = 0;
	PdfFontDescriptor_SetFontBBox(self, 0, 0, 0, 0);	
}




DLLEXPORT_TEST_FUNCTION void PdfFontDescriptor_Cleanup(struct PdfFontDescriptor *self)
{
	if(self->fontBBox)
	{
		MemoryManager_Free(self->fontBBox);
		self->fontBBox = 0;
	}

	if (self->fontStretch)
	{
		MemoryManager_Free(self->fontStretch);
		self->fontStretch = 0;
	}
	if(self->fontName)
	{
		MemoryManager_Free(self->fontName);
		self->fontName = 0;
	}

}




DLLEXPORT_TEST_FUNCTION void PdfFontDescriptor_Destroy(struct PdfFontDescriptor *self)
{
	PdfFontDescriptor_Cleanup(self);
	MemoryManager_Free(self);
}




DLLEXPORT_TEST_FUNCTION void PdfFontDescriptor_Write(struct PdfFontDescriptor *self)
{
	struct DictionaryObject *dict;
	struct NameObject *name;
	struct NumberObject *number;
	struct ArrayObject *tmpArray;
	struct PdfEmbeddedFont *embeddedFont; 
	struct IndirectReference *indirectReference;
	int embeddedFontId;

	if (self->trueTypeEmbeddedFont)
	{
		// Embedd true type font. TODO: Make acutal embedding of real font
		embeddedFont = PdfEmbeddedFont_Create(self->base.document, 
			self->parentFont->templateEmbeddedFont->fontData, self->parentFont->templateEmbeddedFont->fontDataLength);
		PdfEmbeddedFont_WriteTrueType(embeddedFont);
		embeddedFontId = embeddedFont->base.objectId;			
	}
	else if(self->type1EmbeddedFont)
	{
		// NOT IMPLEMENTED YET
	}

	PdfDocument_BeginNewObject((struct PdfBaseObject*)self);
	{	
		dict = DictionaryObject_Begin(self->base.document->streamWriter);

		DictionaryObject_WriteKey(dict, "Type");
		name = NameObject_Create(self->base.document->streamWriter, "FontDescriptor");
		NameObject_Write(name);
		NameObject_Destroy(name);

		DictionaryObject_WriteKey(dict, "FontName");
		name = NameObject_Create(self->base.document->streamWriter, self->fontName);
		NameObject_Write(name);
		NameObject_Destroy(name);

		DictionaryObject_WriteKey(dict, "Flags");
		number = NumberObject_Create(self->base.document->streamWriter, self->flags);
		NumberObject_Write(number);
		NumberObject_Destroy(number);

		DictionaryObject_WriteKey(dict, "FontBBox");
		tmpArray = ArrayObject_BeginArray(self->base.document->streamWriter);
		self->base.document->streamWriter->WriteData(self->base.document->streamWriter, self->fontBBox);
		ArrayObject_EndArray(tmpArray);

		DictionaryObject_WriteKey(dict, "ItalicAngle");
		number = NumberObject_Create(self->base.document->streamWriter, self->italicAngle);
		NumberObject_Write(number);
		NumberObject_Destroy(number);


		DictionaryObject_WriteKey(dict, "Descent");
		number = NumberObject_Create(self->base.document->streamWriter, self->descent);
		NumberObject_Write(number);
		NumberObject_Destroy(number);

		DictionaryObject_WriteKey(dict, "Ascent");
		number = NumberObject_Create(self->base.document->streamWriter, self->ascent);
		NumberObject_Write(number);
		NumberObject_Destroy(number);

		DictionaryObject_WriteKey(dict, "CapHeight");
		number = NumberObject_Create(self->base.document->streamWriter, self->capHeight);
		NumberObject_Write(number);
		NumberObject_Destroy(number);

		DictionaryObject_WriteKey(dict, "StemV");
		number = NumberObject_Create(self->base.document->streamWriter, self->stemV);
		NumberObject_Write(number);
		NumberObject_Destroy(number);	

		DictionaryObject_WriteKey(dict, "XHeight");
		number = NumberObject_Create(self->base.document->streamWriter, self->xHeight);
		NumberObject_Write(number);
		NumberObject_Destroy(number);	

		DictionaryObject_WriteKey(dict, "FontWeight");
		number = NumberObject_Create(self->base.document->streamWriter, self->fontWeight);
		NumberObject_Write(number);
		NumberObject_Destroy(number);	


		if (self->trueTypeEmbeddedFont)
		{
			DictionaryObject_WriteKey(dict, "FontFile2");
			indirectReference = IndirectReference_Create(self->base.document->streamWriter, embeddedFontId, 0);
			IndirectReference_Write(indirectReference);
			IndirectReference_Destroy(indirectReference);
		}
		else if (self->type1EmbeddedFont)
		{
			DictionaryObject_WriteKey(dict, "FontFile");		
			indirectReference = IndirectReference_Create(self->base.document->streamWriter, embeddedFontId, 0);
			IndirectReference_Write(indirectReference);
			IndirectReference_Destroy(indirectReference);
		}

		DictionaryObject_End(dict);
	}
	PdfDocument_EndObject((struct PdfBaseObject*)self);	
}




DLLEXPORT_TEST_FUNCTION void PdfFontDescriptor_SetFontBBox(struct PdfFontDescriptor *self, int x, int y, int w, int h)
{
	char tmpX[100];

	sprintf(tmpX," %d %d %d %d ",x,y,w,h);
	self->fontBBox = MemoryManager_StrDup(tmpX);
}


DLLEXPORT_TEST_FUNCTION void PdfFontDescriptor_SetFontName(struct PdfFontDescriptor *self, char *name)
{
	self->fontName = MemoryManager_StrDup(name);
}


