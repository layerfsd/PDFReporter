
#include "PdfFont.h"
#include "PdfDocument.h"
#include "DictionaryObject.h"
#include "NameObject.h"
#include "MemoryManager.h"
#include "IndirectReference.h"
#include "PdfFontDescriptor.h"
#include "PdfTemplateFont.h"
#include "PdfTemplateEmbeddedFont.h"
#include "NumberObject.h"
#include <string.h>
#include <stdlib.h>

DLLEXPORT_TEST_FUNCTION struct PdfFont *PdfFont_Create(struct PdfDocument *document)
{
	struct PdfFont *x;
	x = (struct PdfFont*)MemoryManager_Alloc(sizeof(struct PdfFont));
	PdfFont_Init(x, document);
	return x;
}

DLLEXPORT_TEST_FUNCTION struct PdfFont *PdfFont_CreateFromTemplate(struct PdfDocument *document, struct PdfTemplateEmbeddedFont *templateEmbeddedFont)
{
	char fontBBox[200];

	struct PdfFont *x;
	x = (struct PdfFont*)MemoryManager_Alloc(sizeof(struct PdfFont));
	PdfFont_Init(x, document);

	x->fontDescriptor->ascent = templateEmbeddedFont->ascent;
	x->fontDescriptor->descent = templateEmbeddedFont->descent;
	x->fontDescriptor->capHeight = templateEmbeddedFont->ascent;

	x->fontDescriptor->italicAngle = templateEmbeddedFont->italicAngle;		
	x->fontDescriptor->stemV = templateEmbeddedFont->stemV;
	x->fontDescriptor->flags = 6; //templateEmbeddedFont->flags;
	x->fontDescriptor->parentFont = x;
	x->fontDescriptor->xHeight = 303;
	x->fontDescriptor->fontStretch = MemoryManager_StrDup("Normal");
	x->fontDescriptor->fontWeight = 400;
	
	

	//sprintf(fontBBox, "-205 -386 1426 896");
	//sprintf(fontBBox, "0 0 0 0");

	sprintf(fontBBox, "%4.0f %4.0f %4.0f %4.0f\0", templateEmbeddedFont->fontBBox.left, templateEmbeddedFont->fontBBox.top, 
		templateEmbeddedFont->fontBBox.right, templateEmbeddedFont->fontBBox.bottom);
	x->fontDescriptor->fontBBox = MemoryManager_StrDup(fontBBox);

	x->baseFont = MemoryManager_StrDup(templateEmbeddedFont->name);
	x->widths = templateEmbeddedFont->widths; // just take widths string from template. We don't need to copy it
	x->firstChar = templateEmbeddedFont->firstChar;
	x->lastChar = templateEmbeddedFont->lastChar;
	x->bold = templateEmbeddedFont->bold;
	x->italic = templateEmbeddedFont->italic;	
	x->subtype = MemoryManager_StrDup(templateEmbeddedFont->fontType);
	x->encoding = MemoryManager_StrDup(templateEmbeddedFont->encoding);
	x->templateEmbeddedFont = templateEmbeddedFont;

	// set true type params if this is true type font
	if (strcmp(x->subtype, "TrueType") == 0)
	{
		PdfFont_SetTrueTypeFont(x);
	}

	//PdfFontDescriptor_SetFontBBox(x->fontDescriptor, templateFont->fontBBox.left, templateFont->fontBBox.right, templateFont->fontBBox.top, templateFont->fontBBox.bottom);
	
	return x;
}



DLLEXPORT_TEST_FUNCTION void PdfFont_Init(struct PdfFont *self, struct PdfDocument *document)
{
	char tmp[100];
	PdfBaseObject_Init(&self->base, document);
	self->subtype = 0;
	self->baseFont = 0;	
	self->encoding = 0;
	self->fontDescriptor = PdfFontDescriptor_Create(document);
	self->toUnicode = 0;
	self->descendantFonts = 0;
	self->widths = 0;
	self->firstChar = 0;
	self->lastChar = 0;
	self->bold = 0;
	self->italic = 0;	
	self->templateEmbeddedFont = 0;
	DLList_PushBack(document->fonts, self);  // add self to document list

	sprintf(tmp, "F%d", document->nextFontId);
	document->nextFontId++;
	self->name = MemoryManager_StrDup(tmp);
}

void PdfFont_Cleanup(struct PdfFont *self)
{
	if (self->encoding)
	{
		MemoryManager_Free(self->encoding);
		self->encoding = 0;
	}
	if (self->subtype)
	{		
		MemoryManager_Free(self->subtype);
		self->subtype = 0;
	}
	if (self->baseFont)
	{		
		MemoryManager_Free(self->baseFont);
		self->baseFont = 0;
	}
	if (self->name)
	{		
		MemoryManager_Free(self->name);		
		self->name = 0;		
	}
}

void PdfFont_Destroy(struct PdfFont *self)
{
	PdfFont_Cleanup(self);
	MemoryManager_Free(self);	
}

DLLEXPORT_TEST_FUNCTION void PdfFont_Write(struct PdfFont *self)
{
	struct DictionaryObject *dict;
	struct NameObject *name;
	struct NumberObject *number;
	struct IndirectReference *indRef;
	int widthObjectId;

	// Write font descriptor
	if (self->fontDescriptor)
	{
		PdfFontDescriptor_Write((struct PdfBaseObject*)self->fontDescriptor);
	}

	// Write widths

	if (self->widths)
	{
		PdfDocument_BeginNewObject((struct PdfBaseObject*)self);
		// write widths array 
		widthObjectId = self->base.objectId;
		self->base.document->streamWriter->WriteData(self->base.document->streamWriter, self->widths);			
		PdfDocument_EndObject((struct PdfBaseObject*)self);
	}

	
	// Write font information

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

	DictionaryObject_WriteKey(dict, "Name");
	name = NameObject_Create(self->base.document->streamWriter, self->name);
	NameObject_Write(name);
	NameObject_Destroy(name);

	DictionaryObject_WriteKey(dict, "BaseFont");
	name = NameObject_Create(self->base.document->streamWriter, self->baseFont);
	NameObject_Write(name);
	NameObject_Destroy(name);		 	

	DictionaryObject_WriteKey(dict, "FirstChar");
	number = NumberObject_Create(self->base.document->streamWriter, self->firstChar);
	NumberObject_Write(number);
	NumberObject_Destroy(number);

	DictionaryObject_WriteKey(dict, "LastChar");
	number = NumberObject_Create(self->base.document->streamWriter, self->lastChar);
	NumberObject_Write(number);
	NumberObject_Destroy(number);


	if (self->widths)
	{	
		DictionaryObject_WriteKey(dict, "Widths");
		indRef = IndirectReference_Create(self->base.document->streamWriter, widthObjectId, 0);
		IndirectReference_Write(indRef);
		IndirectReference_Destroy(indRef);		
	}

	if(strcmp(MemoryManager_StrDup(self->subtype),"Type0 "))
	{
		if(self->toUnicode != 0)
		{
			DictionaryObject_WriteKey(dict, "ToUnicode");
			indRef = IndirectReference_Create(self->base.document->streamWriter, self->toUnicode, 0);
			IndirectReference_Write(indRef);
			IndirectReference_Destroy(indRef);
		}
		
		if(self->descendantFonts != 0)
		{
			DictionaryObject_WriteKey(dict, "DescendantFonts");
			indRef = IndirectReference_Create(self->base.document->streamWriter, self->descendantFonts, 0);
			IndirectReference_Write(indRef);
			IndirectReference_Destroy(indRef);
		}
	}

	if (self->fontDescriptor)
	{	
		DictionaryObject_WriteKey(dict, "FontDescriptor");
		indRef = IndirectReference_Create(self->base.document->streamWriter, self->fontDescriptor->base.objectId, 0);
		IndirectReference_Write(indRef);
		IndirectReference_Destroy(indRef);
	}

	DictionaryObject_WriteKey(dict, "Encoding");
	{
		name = NameObject_Create(self->base.document->streamWriter, self->encoding);
		NameObject_Write(name);
		NameObject_Destroy(name);
	}

	DictionaryObject_End(dict);
	PdfDocument_EndObject((struct PdfBaseObject*)self);
}

char* StrToLower(const char *source)
{
	char *newString;
	unsigned int i;
	unsigned int len;

	newString = MemoryManager_StrDup(source);
	len = strlen(source);
	for(i = 0; i < len; i++)
	{
		newString[i] = tolower(source[i]);
	}
	return newString;
}

// Check if this is standard font. If not then it needs to be embeeded
DLLEXPORT_TEST_FUNCTION short PdfFont_IsStandardFont(char *fontName)
{
	char *fontString;
	fontString = StrToLower(fontName);
	if ((strcmp(fontString, "times-roman") == 0) || 
		(strcmp(fontString, "times-bold") == 0) || 
		(strcmp(fontString, "times-bolditalic") == 0) || 
		(strcmp(fontString, "times-italic") == 0) || 
		(strcmp(fontString, "helvetica") == 0) || 
		(strcmp(fontString, "helvetica-bold") == 0) || 
		(strcmp(fontString, "helvetica-oblique") == 0) || 
		(strcmp(fontString, "helvetica-boldoblique") == 0) || 
		(strcmp(fontString, "courier") == 0) || 
		(strcmp(fontString, "courier-bold") == 0) || 
		(strcmp(fontString, "courier-boldoblique") == 0) || 
		(strcmp(fontString, "courier-oblique") == 0) || 
		(strcmp(fontString, "symbol") == 0) || 
		(strcmp(fontString, "zapf-dingbats") == 0))
	{
		MemoryManager_Free(fontString);
		return TRUE;
	}
	else 
	{
		MemoryManager_Free(fontString);
		return FALSE;
	}	
}


DLLEXPORT_TEST_FUNCTION void PdfFont_SetType1Font(struct PdfFont *self)
{
	char fontName[200];

	if (self->subtype)
		MemoryManager_Free(self->subtype);		
	self->subtype = MemoryManager_StrDup("Type1");


}

DLLEXPORT_TEST_FUNCTION void PdfFont_SetTrueTypeFont(struct PdfFont *self)
{
	char fontName[200];

	if (self->subtype)
	{
		MemoryManager_Free(self->subtype);		
	}
	self->subtype = MemoryManager_StrDup("TrueType");


	if (self->bold && !self->italic)
	{
		sprintf(fontName, "%s,Bold", self->baseFont);
	}
	else if (self->italic && !self->bold)
	{
		sprintf(fontName, "%s,Italic", self->baseFont);
	}
	else if (self->italic && self->bold)
	{
		sprintf(fontName, "%s,BoldItalic", self->baseFont);
	}
	else 
	{
		sprintf(fontName, "%s", self->baseFont);
	}

	if (self->baseFont)		
	{
		MemoryManager_Free(self->baseFont);
		self->baseFont = 0;
	}

	self->baseFont = MemoryManager_StrDup(fontName);

	if (self->fontDescriptor)
	{
		MemoryManager_Free(self->fontDescriptor->fontName);
		self->fontDescriptor->fontName = MemoryManager_StrDup(self->baseFont);
		if (self->templateEmbeddedFont->fontDataLength > 0 && self->templateEmbeddedFont->fontData != NULL)
		{
			self->fontDescriptor->trueTypeEmbeddedFont = TRUE;
		}
		else 
		{
			self->fontDescriptor->trueTypeEmbeddedFont = FALSE;
		}
	}
}


DLLEXPORT_TEST_FUNCTION void PdfFont_SetEncoding(struct PdfFont *self, int encoding)
{
	self->encoding = encoding;
}

DLLEXPORT_TEST_FUNCTION void PdfFont_SetToUnicode(struct PdfFont *self, int toCMapReference)
{
	self->toUnicode = toCMapReference;
}


DLLEXPORT_TEST_FUNCTION void PdfFont_SetDescendantFonts(struct PdfFont *self, int toCIDFontReference)
{
	self->descendantFonts = toCIDFontReference;
}
