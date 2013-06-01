//-----------------------------------------------------------------------------
// Name:	PdfTemplateItemShapeLine.h
// Author:	Tomislav Kukic
// Date:	18.12.2008
//-----------------------------------------------------------------------------


#ifndef _PDFTEMPLATE_ITEM_SHAPELINE_
#define _PDFTEMPLATE_ITEM_SHAPELINE_

#include "PdfFactory.h"
#include "PdfTemplateBalloonItem.h"
#include <libxml/tree.h>

struct PdfTemplateItemShapeLine
{
	struct PdfTemplateBalloonItem base;
	char *X2;
	char *Y2;
	int Width;
};


DLLEXPORT_TEST_FUNCTION struct PdfTemplateItemShapeLine* PdfTemplateItemShapeLine_CreateFromXml(xmlNode *node);

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemShapeLine_Init(struct PdfTemplateItemShapeLine* self, char *type, float version);

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemShapeLine_Destroy(struct PdfTemplateBalloonItem* self);

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemShapeLine_Cleanup(struct PdfTemplateItemShapeLine* self);

int PdfTemplateItemShapeLine_Process(struct PdfTemplateBalloonItem *self, struct PdfGenerator *generator, struct PdfGeneratedBalloon *balloon, struct StreamWriter *streamWriter);


#endif