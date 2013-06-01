#ifndef _PDFTEMPLATE_ITEM_STATICIMAGE_
#define _PDFTEMPLATE_ITEM_STATICIMAGE_

#include "PdfFactory.h"
#include "PdfTemplateBalloonItem.h"
#include <libxml/tree.h>

struct PdfTemplateItemStaticImage
{
	struct PdfTemplateBalloonItem base;
	char *name;  // this is complete image file name	
	char *colorSpace;
	char *bitsPerComponent;
	char *imageData;
	int imageDataLength;
};


DLLEXPORT_TEST_FUNCTION struct PdfTemplateItemStaticImage* PdfTemplateItemStaticImage_CreateFromXml(xmlNode *node);

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemStaticImage_Init(struct PdfTemplateItemStaticImage* self, char *type, float version);

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemStaticImage_Destroy(struct PdfTemplateBalloonItem* self);

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemStaticImage_Cleanup(struct PdfTemplateItemStaticImage* self);

int PdfTemplateItemStaticImage_Process(struct PdfTemplateBalloonItem *self, struct PdfGenerator *generator, struct PdfGeneratedBalloon *balloon, struct StreamWriter *streamWriter);


#endif