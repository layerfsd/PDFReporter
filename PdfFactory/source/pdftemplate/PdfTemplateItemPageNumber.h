#ifndef _PDFTEMPLATE_ITEM_PAGENUMBER_
#define _PDFTEMPLATE_ITEM_PAGENUMBER_

#include "PdfFactory.h"
#include "PdfTemplateBalloonItem.h"
#include <libxml/tree.h>

struct PdfTemplateItemPageNumber
{
	struct PdfTemplateBalloonItem base;
	struct PdfTemplateFont *font;
	char *format;
	char *text;
};


DLLEXPORT_TEST_FUNCTION struct PdfTemplateItemPageNumber* PdfTemplateItemPageNumber_CreateFromXml(xmlNode *node);

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemPageNumber_Init(struct PdfTemplateItemPageNumber* self, char *type, float version);

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemPageNumber_Destroy(struct PdfTemplateItemPageNumber* self);

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemPageNumber_Cleanup(struct PdfTemplateItemPageNumber* self);

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemPageNumber_Update(struct PdfTemplateItemPageNumber* self);

int PdfTemplateItemPageNumber_Process(struct PdfTemplateBalloonItem *self, struct PdfGenerator *generator, struct PdfGeneratedBalloon *balloon, struct StreamWriter *streamWriter);

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemPageNumber_MakeText(struct PdfTemplateItemPageNumber* self, struct PdfGenerator *generator);


#endif