#ifndef _PDFTEMPLATE_ITEM_STATICTEXT_
#define _PDFTEMPLATE_ITEM_STATICTEXT_

#include "PdfFactory.h"
#include "PdfTemplateBalloonItem.h"
#include <libxml/tree.h>

struct PdfTemplateItemStaticText
{
	struct PdfTemplateBalloonItem base;
	char *text;
	struct PdfTemplateFont *font;	
};


DLLEXPORT_TEST_FUNCTION struct PdfTemplateItemStaticText* PdfTemplateItemStaticText_CreateFromXml(xmlNode *node);

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemStaticText_Init(struct PdfTemplateItemStaticText* self, char *type, float version);

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemStaticText_Destroy(struct PdfTemplateBalloonItem* self);

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemStaticText_Cleanup(struct PdfTemplateItemStaticText* self);

int PdfTemplateItemStaticText_Process(struct PdfTemplateBalloonItem *self, struct PdfGenerator *generator, struct PdfGeneratedBalloon *generatedBalloon, struct StreamWriter *streamWriter);


#endif