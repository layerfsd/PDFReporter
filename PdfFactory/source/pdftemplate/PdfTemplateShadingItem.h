//-------------------------------------------
//- File: PdfTemplateShadingItem.h			-
//- Author: Tomislav Kukic					-
//- Date: 9.1.2009							-
//-------------------------------------------



#ifndef _PDF_TEMPLATE_SHADING_ITEM_
#define _PDF_TEMPLATE_SHADING_ITEM_

#include "PdfTemplateBalloonItem.h"
#include <libxml/tree.h>

struct PdfTemplateShadingItem
{
	//struct PdfTemplateBalloonItem base;
	struct PdfShadingDictionary *shadingDictionary;
	int type;
	int useCMYK;
	int functionType;
	double shadingFactor;	
	double coordsX1, coordsX2, coordsY1, coordsY2;

	double fromR;
	double fromG;
	double fromB;

	double toR;
	double toG;
	double toB;

	double fromC, fromM, fromY, fromK;
	double toC, toM, toY, toK;

	int written;
};


DLLEXPORT_TEST_FUNCTION struct PdfTemplateShadingItem *PdfTemplateShadingItem_CreateFromXml(xmlNode *node);
DLLEXPORT_TEST_FUNCTION void PdfTemplateShadingItem_Init(struct PdfTemplateShadingItem *self);
DLLEXPORT_TEST_FUNCTION void PdfTemplateShadingItem_Cleanup(struct PdfTemplateShadingItem *self);
DLLEXPORT_TEST_FUNCTION void PdfTemplateShadingItem_Destroy(struct PdfTemplateShadingItem *self);
DLLEXPORT_TEST_FUNCTION int  PdfTemplateShadingItem_Process(struct PdfTemplateShadingItem *self, struct PdfGenerator *generator, struct PdfGeneratedBalloon *balloon, struct StreamWriter *streamWriter);




#endif
