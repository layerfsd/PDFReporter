/*
PdfTemplateBalloonItem.c
*/

#include "PdfTemplateBalloonItem.h"
#include "PdfTemplateLocation.h"
#include "PdfTemplateTextProperties.h"
#include "PdfTemplate.h"
#include "UnitConverter.h"
//#include "PdfTemplateItemStaticText.h"
#include <libxml/tree.h>
#include "PdfTemplateElements.h"
#include "MemoryManager.h"
#include "PdfPage.h"
#include "PdfTemplateBalloon.h"
#include "TransformationMatrix.h"
#include "PdfTemplateItemCounter.h"
#include "PdfTemplateItemDateTime.h"
#include "PdfTemplateItemShapeLine.h"
#include "PdfTemplateItemShapeRectangle.h"
#include "PdfTemplateItemPageNumber.h"
#include "PdfGeneratedBalloon.h"
#include "PdfPrecalculatedItem.h"
#include "PdfTemplateItemDynamicImage.h"
#include "PdfTemplateItemStaticImage.h"



DLLEXPORT_TEST_FUNCTION struct PdfTemplateBalloonItem* PdfTemplateBalloonItem_CreateFromXml(xmlNode *node)
{	
	struct PdfTemplateBalloonItem *ret = NULL;
	struct PdfTemplateShadingItem *tmpItem = NULL;
	char *type;

	type = PdfTemplate_LoadStringAttribute(node, TYPE);	

	if (strcmp(type, BALLOON_ITEMTYPE_STATICTEXT) == 0)
	{
		ret = (struct PdfTemplateBalloonItem *)PdfTemplateItemStaticText_CreateFromXml(node);
	}
	else if (strcmp(type, BALLOON_ITEMTYPE_DYNAMICTEXT) == 0)
	{
		ret = (struct PdfTemplateBalloonItem *)PdfTemplateItemDynamicText_CreateFromXml(node);
	}
	else if (strcmp(type, BALLOON_ITEMTYPE_STATICIMAGE) == 0)
	{
		ret = (struct PdfTemplateBalloonItem *)PdfTemplateItemStaticImage_CreateFromXml(node);
	}
	else if (strcmp(type, BALLOON_ITEMTYPE_COUNTER) == 0)
	{
		ret = (struct PdfTemplateBalloonItem *)PdfTemplateItemCounter_CreateFromXml(node);
	}
	else if (strcmp(type, BALLOON_ITEMTYPE_DATETIME) == 0)
	{
		ret = (struct PdfTemplateBalloonItem *)PdfTemplateItemDateTime_CreateFromXml(node);
	}
	else if (strcmp(type, BALLOON_ITEMTYPE_SHAPELINE) == 0)
	{
		ret = (struct PdfTemplateBalloonItem *)PdfTemplateItemShapeLine_CreateFromXml(node);
	}
	else if (strcmp(type, BALLOON_ITEMTYPE_SHAPERECTANGLE) == 0)
	{
		ret = (struct PdfTemplateBalloonItem *)PdfTemplateItemShapeRectangle_CreateFromXml(node);
	}
	else if (strcmp(type, BALLOON_ITEMTYPE_PRECALCULATEDITEM) == 0)
	{
		ret = (struct PdfTemplateBalloonItem *)PdfPrecalculatedItem_CreateFromXml(node);
	}
	else if (strcmp(type, BALLOON_ITEMTYPE_PAGENUMBER) == 0)
	{
		ret = (struct PdfTemplateBalloonItem *)PdfTemplateItemPageNumber_CreateFromXml(node);
	}
	else if (strcmp(type, BALLOON_ITEMTYPE_DYNAMICIMAGE) == 0)
	{
		ret = (struct PdfTemplateBalloonItem *)PdfTemplateItemDynamicImage_CreateFromXml(node);
	}
	return ret;
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateBalloonItem_Init(struct PdfTemplateBalloonItem* self, char *type, float version, struct PdfTemplateLocation *location, PdfBalloonItem_Destroy_Method destroy, PdfBalloonItem_Process_Method process)
{
	self->type = MemoryManager_StrDup(type);
	self->version = version;
	self->location = location;
	self->destroy = destroy;
	self->process = process;
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateBalloonItem_InitFromXml(struct PdfTemplateBalloonItem* self, xmlNode *node, PdfBalloonItem_Destroy_Method destroy, PdfBalloonItem_Process_Method process)
{
	xmlNode *foundNode;

	self->type = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(node, TYPE));	
	self->version = (float)atof(PdfTemplate_LoadStringAttribute(node, VERSION));		
	foundNode = PdfTemplate_FindNode(node, LOCATION);
	if (foundNode)
	{
		self->location = PdfTemplateLocation_CreateFromXml(foundNode);
	}
	else
	{
		self->location = PdfTemplateLocation_Create();
	}
	
	foundNode = PdfTemplate_FindNode(node, SCALE);
	if (foundNode)
	{
		self->scale = PdfTemplateScale_CreateFromXml(foundNode);
	}
	else
	{
		// default scale applied if this property is missing
		self->scale = PdfTemplateScale_Create("1", "1");		
	}

	foundNode = PdfTemplate_FindNode(node, TRANSFORMATION);
	if (foundNode)
	{
		self->transformation = PdfTemplateTransformation_CreateFromXml(foundNode);
	}
	else
	{
		// default scale applied if this property is missing
		self->transformation = PdfTemplateTransformation_Create(1,0,0,1);
	}

	self->destroy = destroy;
	self->process = process;
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateBalloonItem_Cleanup(struct PdfTemplateBalloonItem* self)
{
	if (self->type)
	{
		MemoryManager_Free(self->type);
		self->type = 0;
	}		
	if (self->location)
	{
		MemoryManager_Free(self->location);
		self->location = 0;
	}
	if (self->scale)
	{
		PdfTemplateScale_Destroy(self->scale);
		self->scale = 0;
	}
	if (self->transformation)
	{
		PdfTemplateTransformation_Destroy(self->transformation);
		self->transformation = 0;
	}
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateBalloonItem_Destroy(struct PdfTemplateBalloonItem* self)
{	
	if (self->destroy)
	{
		self->destroy(self);
	}
	else
	{
		MemoryManager_Free(self);
	}	
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateBalloonItem_GetFullTransformation(struct PdfTemplateBalloonItem *self, struct PdfGeneratedBalloon *generatedBalloon, struct PdfPage *page, const struct TransformationMatrix *outTransformation, int anchorIsCenter)
{
	struct UnitConverter *unitConverter;
	struct TransformationMatrix *translation, *scale, *rot, *resMat, *resMat2;
	float locX = 0, locY = 0;
	float scaleX, scaleY;
	float absX, absY;
	
	// create unit converter
	unitConverter = UnitConverter_Create();
	UnitConverter_AddCommonUnits(unitConverter);

	scale = TransformationMatrix_Create();
	rot = TransformationMatrix_Create();
	resMat = TransformationMatrix_Create();
	resMat2 = TransformationMatrix_Create();
	translation = TransformationMatrix_Create();

	scaleX = UnitConverter_ConvertToPoints(unitConverter, self->scale->x);	
	scaleY = UnitConverter_ConvertToPoints(unitConverter, self->scale->y);	
	
	// Make scale in unit space
	if (anchorIsCenter)
	{
		TransformationMatrix_Translate(translation, -scaleX/2, scaleY/2);
	}		

	TransformationMatrix_Scale(scale, scaleX, scaleY);
	TransformationMatrix_Multiply(scale, translation, resMat2);	

	// Make rotation
	rot->a = self->transformation->a;
	rot->b = self->transformation->b;
	rot->c = self->transformation->c;
	rot->d = self->transformation->d;
	
	// Make location in unit space
	absX = PdfGeneratedBalloon_GetAbsoluteLocationX(generatedBalloon);
	absY = /*page->properties.mediaBox.upperRightY -*/  PdfGeneratedBalloon_GetAbsoluteLocationY(generatedBalloon); 

	locX = absX + UnitConverter_ConvertToPoints(unitConverter, self->location->positionX);	
	locY = absY - UnitConverter_ConvertToPoints(unitConverter, self->location->positionY);	

	// Multiply and make result
	TransformationMatrix_Multiply(resMat2, rot, resMat);		
	TransformationMatrix_Translate(translation, locX, locY);
	TransformationMatrix_Multiply(resMat, translation, outTransformation);

	UnitConverter_Destroy(unitConverter);
	TransformationMatrix_Destroy(rot);
	TransformationMatrix_Destroy(translation);
	TransformationMatrix_Destroy(scale);
	TransformationMatrix_Destroy(resMat);
}
