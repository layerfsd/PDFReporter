
#ifndef _PDFEMBEDDEDTEMPLATEFONT_
#define _PDFEMBEDDEDTEMPLATEFONT_

#include "PdfFactory.h"
#include "RectangleNormal.h"
#include <libxml/tree.h>

struct PdfTemplateEmbeddedFont
{
	char *name;
	char *fontType; // TrueType by default. Currently this one is only supported
	char *encoding; // if encoding is used
	
	int saveId;
	int emSize;
	struct RectangleNormal fontBBox;
	int ascent;
	int descent;
	int bold;
	int italic;
	int italicAngle;
	int stemV;
	int flags; // font flag. Check PDF reference for this one. Used in FontDescriptor
	int firstChar;
	int lastChar;
	char *widths; 


	char *fontData;
	int fontDataLength;
};

DLLEXPORT_TEST_FUNCTION struct PdfTemplateEmbeddedFont* PdfTemplateEmbeddedFont_Create();
DLLEXPORT_TEST_FUNCTION struct PdfTemplateEmbeddedFont* PdfTemplateEmbeddedFont_CreateFromXml(xmlNode *node);

DLLEXPORT_TEST_FUNCTION void PdfTemplateEmbeddedFont_Init(struct PdfTemplateEmbeddedFont* self);

DLLEXPORT_TEST_FUNCTION void PdfTemplateEmbeddedFont_Cleanup(struct PdfTemplateEmbeddedFont* self);

DLLEXPORT_TEST_FUNCTION void PdfTemplateEmbeddedFont_Destroy(struct PdfTemplateEmbeddedFont* self);



#endif