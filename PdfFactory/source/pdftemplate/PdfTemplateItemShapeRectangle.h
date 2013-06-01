//-----------------------------------------------------------------------------
// Name:	PdfTemplateItemShapeRectangle.h
// Author:	Tomislav Kukic
// Date:	19.12.2008
//-----------------------------------------------------------------------------



#ifndef _PDFTEMPLATE_ITEM_SHAPERECTANGLE_
#define _PDFTEMPLATE_ITEM_SHAPERECTANGLE_

#include "PdfFactory.h"
#include "PdfTemplateBalloonItem.h"
#include <libxml/tree.h>

struct PdfTemplateItemShapeRectangle
{
	struct PdfTemplateBalloonItem base;
	char *width;
	char *height;
	float fillColor_R;
	float fillColor_G;
	float fillColor_B;
	float strokeColor_R;
	float strokeColor_G;
	float strokeColor_B;
	float fillColor_C;
	float fillColor_M;
	float fillColor_Y;
	float fillColor_K;
	float strokeColor_C;
	float strokeColor_M;
	float strokeColor_Y;
	float strokeColor_K;
	float strokeWidth;
	int useCMYKColor;
	
	short useShading;
	short useStroke;
	
	struct PdfTemplateShadingItem *shading;
};


DLLEXPORT_TEST_FUNCTION struct PdfTemplateItemShapeRectangle* PdfTemplateItemShapeRectangle_CreateFromXml(xmlNode *node);

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemShapeRectangle_Init(struct PdfTemplateItemShapeRectangle* self, char *type, float version);

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemShapeRectangle_Destroy(struct PdfTemplateBalloonItem* self);

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemShapeRectangle_Cleanup(struct PdfTemplateItemShapeRectangle* self);

int PdfTemplateItemShapeRectangle_Process(struct PdfTemplateBalloonItem *self, struct PdfGenerator *generator, struct PdfGeneratedBalloon *balloon, struct StreamWriter *streamWriter);


#endif