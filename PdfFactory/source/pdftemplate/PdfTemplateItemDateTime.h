#ifndef _PDFTEMPLATE_ITEM_DATETIME_
#define _PDFTEMPLATE_ITEM_DATETIME_

#include "PdfFactory.h"
#include "PdfTemplateBalloonItem.h"
#include <libxml/tree.h>
#include <time.h>

struct PdfTemplateItemDateTime
{
	struct PdfTemplateBalloonItem base;
	struct PdfTemplateFont *font;
	char *format;
	char *text;
};


DLLEXPORT_TEST_FUNCTION struct PdfTemplateItemDateTime* PdfTemplateItemDateTime_CreateFromXml(xmlNode *node);

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemDateTime_Init(struct PdfTemplateItemDateTime* self);

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemDateTime_Destroy(struct PdfTemplateItemDateTime* self);

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemDateTime_Cleanup(struct PdfTemplateItemDateTime* self);

int PdfTemplateItemDateTime_Process(struct PdfTemplateBalloonItem *self, struct PdfGenerator *generator, struct PdfGeneratedBalloon *balloon, struct StreamWriter *streamWriter);

void PdfTemplateItemDateTime_MakeText(struct PdfTemplateItemDateTime *self, struct tm *time);

#endif



