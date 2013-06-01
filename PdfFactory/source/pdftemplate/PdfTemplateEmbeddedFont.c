
#include "PdfFactory.h"
#include "PdfTemplateEmbeddedFont.h"
#include "PdfTemplateElements.h"
#include "MemoryManager.h"
#include "base64decoder.h"

DLLEXPORT_TEST_FUNCTION struct PdfTemplateEmbeddedFont* PdfTemplateEmbeddedFont_Create()
{	
	struct PdfTemplateEmbeddedFont *ret; 
	ret = (struct PdfTemplateEmbeddedFont*)MemoryManager_Alloc(sizeof(struct PdfTemplateEmbeddedFont));	
	PdfTemplateEmbeddedFont_Init(ret);

	return ret;
}

DLLEXPORT_TEST_FUNCTION struct PdfTemplateEmbeddedFont* PdfTemplateEmbeddedFont_CreateFromXml(xmlNode *node)
{	
	struct PdfTemplateEmbeddedFont *ret; 
	unsigned int i;
	char *embeddedFontText;
	
	ret = (struct PdfTemplateEmbeddedFont*)MemoryManager_Alloc(sizeof(struct PdfTemplateEmbeddedFont));	
	PdfTemplateEmbeddedFont_Init(ret);
	
	ret->name = PdfTemplate_LoadStringAttribute(node, FONT_NAME);
	ret->saveId = PdfTemplate_LoadIntAttribute(node, FONT_SAVE_ID);

	// replace spaces in font name with underscores
	for(i = 0; i < strlen(ret->name); i++)
	{
		if (ret->name[i] == ' ')
		{
			ret->name[i] = '_';
		}
	}

	// Load ascent, descent, em size info
	ret->ascent = PdfTemplate_LoadIntAttribute(node, "Ascent");
	ret->descent = PdfTemplate_LoadIntAttribute(node, "Descent");
	ret->emSize = PdfTemplate_LoadIntAttribute(node, "EmHeight");
	ret->bold = PdfTemplate_LoadIntAttribute(node, "Bold");
	ret->italic = PdfTemplate_LoadIntAttribute(node, "Italic");
	ret->italicAngle = PdfTemplate_LoadIntAttribute(node, "ItalicAngle");

	ret->fontBBox.left = PdfTemplate_LoadIntAttribute(node, "BBoxLeft");
	ret->fontBBox.right = PdfTemplate_LoadIntAttribute(node, "BBoxRight");
	ret->fontBBox.top = PdfTemplate_LoadIntAttribute(node, "BBoxTop");
	ret->fontBBox.bottom = PdfTemplate_LoadIntAttribute(node, "BBoxBottom");
	ret->stemV = PdfTemplate_LoadIntAttribute(node, "StemV");
	ret->flags = PdfTemplate_LoadIntAttribute(node, "Flags");
	ret->firstChar = PdfTemplate_LoadIntAttribute(node, "FirstChar");
	ret->lastChar = PdfTemplate_LoadIntAttribute(node, "LastChar");
	ret->widths = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(node, "Widths"));
	
	// Load content into memory
	ret->fontDataLength = PdfTemplate_LoadIntAttribute(node, "EmbeddedDecodedFontLength");
	if (ret->fontDataLength > 0)
	{
		ret->fontData = MemoryManager_Alloc(ret->fontDataLength);
		embeddedFontText = PdfTemplate_LoadTextContent(node);		
		Base64Decode(embeddedFontText, ret->fontData, ret->fontDataLength);
	}

	return ret;
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateEmbeddedFont_Init(struct PdfTemplateEmbeddedFont* self)
{
	self->fontData = 0;
	self->fontDataLength = 0;
	self->name = 0;
	self->fontType = MemoryManager_StrDup("TrueType");
	self->encoding = MemoryManager_StrDup("WinAnsiEncoding");
	self->widths = 0;
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateEmbeddedFont_Cleanup(struct PdfTemplateEmbeddedFont* self)
{
	if (self->encoding)
	{
		MemoryManager_Free(self->encoding);
		self->encoding = 0;
	}
	if (self->fontType)
	{
		MemoryManager_Free(self->fontType);
		self->fontType = 0;
	}
	if (self->fontData)
	{
		MemoryManager_Free(self->fontData);
		self->fontData = 0;
		self->fontDataLength = 0;
	}
		if (self->name)
	{
		MemoryManager_Free(self->name);
		self->name = 0;
	}
	if (self->widths)
	{
		MemoryManager_Free(self->widths);
		self->widths = 0;
	}
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateEmbeddedFont_Destroy(struct PdfTemplateEmbeddedFont* self)
{
	PdfTemplateEmbeddedFont_Cleanup(self);
	MemoryManager_Free(self);
}
