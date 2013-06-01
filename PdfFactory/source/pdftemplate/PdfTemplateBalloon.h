/*
PdfTemplateBalloon.h

*/

#ifndef _PDFTEMPLATEBALLOON_
#define _PDFTEMPLATEBALLOON_

#include "PdfFactory.h"
#include "PdfTemplateLocation.h"
#include "PdfTemplateBalloonShape.h"
#include "PdfTemplateBalloonItem.h"
#include "PdfTemplateItemBorder.h"
#include <libxml/tree.h>

#define PDFTEMPLATE_BALLOON_TYPE_STATIC "Static"
#define PDFTEMPLATE_BALLOON_TYPE_DYNAMIC "Dynamic"

#define DOCK_NONE 0
#define DOCK_BOTTOM 1
#define DOCK_LEFT 2
#define DOCK_RIGHT 3
#define DOCK_TOP 4

struct PdfTemplateBalloon
{
	char *name;
	char *type;
	char *dataStream;
	char *version;
	int availableOnEveryPage;
	int fillingGeneratesNew;
	int fillCapacity;
	int canGrow;
	short fitToContent;
	short isStatic;
	short dockPosition; 
	short hasPrevDynamicTopDocked; //	This is set on loading. True if there is dynamic balloon before this one that is top docked. Valid only if this is static balloon
	struct PdfTemplateItemBorder topBorder;
	struct PdfTemplateItemBorder leftBorder;
	struct PdfTemplateItemBorder bottomBorder;
	struct PdfTemplateItemBorder rightBorder;

	int skipDataReadMarker; // required for generator. This is marker 
	int lastGeneratedPageNumber; // required for generator. Affect AvailableOnEveryPage property

	double absLocationX; // absolute location on page  Used by PdfGenerator class
	double absLocationY; // absolute location on page. Used by pdfGenerator class

	double currentBalloonLocationX; // relative location on where to draw balloon now. Used in dynamic balloons
	double currentBalloonLocationY; // relative location on where to draw balloon now. Used in dynamic balloons

	struct DLList *items;
	struct PdfTemplateBalloonShape *shape;
	struct PdfTemplateLocation *location;
	struct Rectangle *containerRect; // this is container rect (0,0, width, heigh)
	struct DLList *rectMatrix; // list of rects inside this balloon

	struct PdfTemplateBalloon *parentBalloon;
	struct DLList *balloons;

	// fill Color
	float fillColorR;
	float fillColorG;
	float fillColorB;
	float fillColorA;
};

DLLEXPORT_TEST_FUNCTION void PdfTemplateBalloon_Write();

DLLEXPORT_TEST_FUNCTION struct PdfTemplateBalloon* PdfTemplateBalloon_Create(char *name, char *type, char *dataStream, char *version);

DLLEXPORT_TEST_FUNCTION struct PdfTemplateBalloon* PdfTemplateBalloon_CreateFromXml(xmlNode *node);
/* create ballon from xml node or return null on error */

DLLEXPORT_TEST_FUNCTION void PdfTemplateBalloon_Init(struct PdfTemplateBalloon* self, char *name, char *type, char *dataStream, char *version);

DLLEXPORT_TEST_FUNCTION void PdfTemplateBalloon_Destroy(struct PdfTemplateBalloon* self);

DLLEXPORT_TEST_FUNCTION void PdfTemplateBalloon_Cleanup(struct PdfTemplateBalloon* self);

DLLEXPORT_TEST_FUNCTION void PdfTemplateBalloon_AddItem(struct PdfTemplateBalloon* self, struct PdfTemplateBalloonItem *item);

DLLEXPORT_TEST_FUNCTION float PdfTemplateBalloon_GetFitToContentWidth(struct PdfTemplateBalloon* self);
DLLEXPORT_TEST_FUNCTION float PdfTemplateBalloon_GetFitToContentHeight(struct PdfTemplateBalloon* self);

#endif