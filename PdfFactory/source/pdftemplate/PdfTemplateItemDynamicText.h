#ifndef _PDFTEMPLATE_ITEM_DYNAMICTEXT_
#define _PDFTEMPLATE_ITEM_DYNAMICTEXT_

#include "PdfFactory.h"
#include "PdfTemplateBalloonItem.h"
#include <libxml/tree.h>

struct PdfTemplateItemDynamicText
{
	struct PdfTemplateBalloonItem base;
	char *sourceColumn;
	char *dataSourceName;
	struct PdfTemplateFont *font;
	double angle; // angle of rotation
};


DLLEXPORT_TEST_FUNCTION struct PdfTemplateItemDynamicText* PdfTemplateItemDynamicText_CreateFromXml(xmlNode *node);

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemDynamicText_Init(struct PdfTemplateItemDynamicText* self, char *type, float version);

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemDynamicText_Destroy(struct PdfTemplateBalloonItem* self);

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemDynamicText_Cleanup(struct PdfTemplateItemDynamicText* self);

int PdfTemplateItemDynamicText_Process(struct PdfTemplateBalloonItem *self, struct PdfGenerator *generator, struct PdfGeneratedBalloon *balloon, struct StreamWriter *streamWriter);



#endif