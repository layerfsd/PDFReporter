#ifndef _PDFTEMPLATE_ITEM_COUNTER_
#define _PDFTEMPLATE_ITEM_COUNTER_

#include "PdfFactory.h"
#include "PdfTemplateBalloonItem.h"
#include <libxml/tree.h>

struct PdfTemplateItemCounter
{
	struct PdfTemplateBalloonItem base;
	struct PdfTemplateFont *font;
	char *format;
	char *text;
	int start;
	int end;
	int counter;
	char loop;
	int interval;
};


DLLEXPORT_TEST_FUNCTION struct PdfTemplateItemCounter* PdfTemplateItemCounter_CreateFromXml(xmlNode *node);

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemCounter_Init(struct PdfTemplateItemCounter* self, char *type, float version);

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemCounter_Destroy(struct PdfTemplateItemCounter* self);

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemCounter_Cleanup(struct PdfTemplateItemCounter* self);

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemCounter_Update(struct PdfTemplateItemCounter* self);

int PdfTemplateItemCounter_Process(struct PdfTemplateBalloonItem *self, struct PdfGenerator *generator, struct PdfGeneratedBalloon *balloon, struct StreamWriter *streamWriter);

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemCounter_MakeText(struct PdfTemplateItemCounter* self);


#endif