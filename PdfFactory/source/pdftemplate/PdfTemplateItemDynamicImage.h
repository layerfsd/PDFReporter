#ifndef _PDFTEMPLATE_ITEM_DYNAMICIMAGE_
#define _PDFTEMPLATE_ITEM_DYNAMICIMAGE_

#include "PdfFactory.h"
#include "PdfTemplateBalloonItem.h"
#include <libxml/tree.h>

struct PdfTemplateItemDynamicImage
{
	struct PdfTemplateBalloonItem base;
	char *name;  // this is complete image file name
	char *imageType;
	char *sourceColumn;
	char *dataSourceName;
	char *colorSpace;
	char *bitsPerComponent;
};


DLLEXPORT_TEST_FUNCTION struct PdfTemplateItemDynamicImage* PdfTemplateItemDynamicImage_CreateFromXml(xmlNode *node);

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemDynamicImage_Init(struct PdfTemplateItemDynamicImage* self, char *type, float version);

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemDynamicImage_Destroy(struct PdfTemplateBalloonItem* self);

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemDynamicImage_Cleanup(struct PdfTemplateItemDynamicImage* self);

int PdfTemplateItemDynamicImage_Process(struct PdfTemplateBalloonItem *self, struct PdfGenerator *generator, struct PdfGeneratedBalloon *balloon, struct StreamWriter *streamWriter);


#endif