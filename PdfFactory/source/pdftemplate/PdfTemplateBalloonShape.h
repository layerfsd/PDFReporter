/*
PdfTemplateBalloonShape.h

Author: Marko Vranjkovic
Date: 25.07.2008.	

*/

#ifndef _PDFTEMPLATEBALLOONSHAPE_
#define _PDFTEMPLATEBALLOONSHAPE_

#include "PdfFactory.h"
#include "PdfTemplateDimensions.h"

#define BALLOON_SHAPE_RECTANGLE 1
#define BALLOON_SHAPE_CIRCLE 2 // not implemented
#define BALLOON_SHAPE_POLYGON 3 // not implemented


struct PdfTemplateBalloonShape
{
	int structType; // not used yet

	char *type;
	struct PdfTemplateDimensions * dimensions;
};

DLLEXPORT_TEST_FUNCTION void PdfTemplateBalloonShape_Write();

DLLEXPORT_TEST_FUNCTION struct PdfTemplateBalloonShape* PdfTemplateBalloonShape_Create();
DLLEXPORT_TEST_FUNCTION struct PdfTemplateBalloonShape* PdfTemplateBalloonShape_CreateFromXml(xmlNode *node);

DLLEXPORT_TEST_FUNCTION void PdfTemplateBalloonShape_Init(struct PdfTemplateBalloonShape* self);

DLLEXPORT_TEST_FUNCTION void PdfTemplateBalloonShape_Destroy(struct PdfTemplateBalloonShape* self);


#endif