/*
PdfTemplateBalloonItem.h

Author: Marko Vranjkovic
Date: 25.07.2008.	

*/

#ifndef _PDFTEMPLATEBALLOONITEM_
#define _PDFTEMPLATEBALLOONITEM_

#include "PdfFactory.h"
#include <libxml/tree.h>
#include "StreamWriter.h"
#include "PdfTemplateScale.h"
#include "PdfTemplateTransformation.h"


#define BALLOON_ITEMTYPE_STATICTEXT "StaticText"
#define BALLOON_ITEMTYPE_DYNAMICTEXT "DynamicText"
#define BALLOON_ITEMTYPE_STATICIMAGE "StaticImage"
#define BALLOON_ITEMTYPE_COUNTER "Counter"
#define BALLOON_ITEMTYPE_DATETIME "DateTime"
#define BALLOON_ITEMTYPE_SHAPELINE "ShapeLine"
#define BALLOON_ITEMTYPE_SHAPERECTANGLE "ShapeRectangle"
#define BALLOON_ITEMTYPE_PRECALCULATEDITEM "Precalculated"
#define BALLOON_ITEMTYPE_PAGENUMBER "PageNumberItem"
#define BALLOON_ITEMTYPE_DYNAMICIMAGE "DynamicImage"


typedef void (*PdfBalloonItem_Destroy_Method)(struct PdfTemplateBalloonItem *self);
typedef int (*PdfBalloonItem_Process_Method)(struct PdfTemplateBalloonItem *self, struct PdfGenerator *generator, struct PdfGeneratedBalloon *generatedBalloon, struct StreamWriter *streamWriter);
/* Process item by generating it into streamWriter. All items are responsible for creating and writing necessary add-in objets like images, fonts, ... */

struct PdfTemplateBalloonItem
{	
	float version;
	char *type;	
	struct PdfTemplateLocation *location;	
	struct PdfTemplateScale *scale;
	struct PdfTemplateTransformation *transformation;

	PdfBalloonItem_Destroy_Method destroy; // virtual destroy method
	PdfBalloonItem_Process_Method process; // virtual process method
};

DLLEXPORT_TEST_FUNCTION struct PdfTemplateBalloonItem* PdfTemplateBalloonItem_CreateFromXml(xmlNode *node);

DLLEXPORT_TEST_FUNCTION void PdfTemplateBalloonItem_Init(struct PdfTemplateBalloonItem* self, char *type, float version, struct PdfTemplateLocation *location, PdfBalloonItem_Destroy_Method destroy, PdfBalloonItem_Process_Method process);
DLLEXPORT_TEST_FUNCTION void PdfTemplateBalloonItem_InitFromXml(struct PdfTemplateBalloonItem* self, xmlNode *node, PdfBalloonItem_Destroy_Method destroy, PdfBalloonItem_Process_Method process);

DLLEXPORT_TEST_FUNCTION void PdfTemplateBalloonItem_GetFullTransformation(struct PdfTemplateBalloonItem *self, struct PdfGeneratedBalloon *generatedBalloon, struct PdfPage *page, 
															const struct TransformationMatrix *outTransformation, int anchorIsCenter);
/* return full transformation for this item */

DLLEXPORT_TEST_FUNCTION void PdfTemplateBalloonItem_Destroy(struct PdfTemplateBalloonItem* self);
DLLEXPORT_TEST_FUNCTION void PdfTemplateBalloonItem_Cleanup(struct PdfTemplateBalloonItem* self);


#endif