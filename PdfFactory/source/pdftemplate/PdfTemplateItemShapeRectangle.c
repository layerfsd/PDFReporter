//-----------------------------------------------------------------------------
// Name:	PdfTemplateItemShapeRectangle.c
// Author:	Tomislav Kukic
// Date:	19.12.2008
//-----------------------------------------------------------------------------


#include "PdfTemplateItemShapeRectangle.h"
#include "PdfTemplateElements.h"
#include "PdfTemplate.h"
#include "PdfGenerator.h"
#include "PdfTemplateBalloon.h"
#include "PdfTemplateBalloonItem.h"
#include "GraphicWriter.h"
#include "PdfPage.h"
#include "UnitConverter.h"
#include "MemoryManager.h"
#include "TransformationMatrix.h"
#include "Rectangle.h"
#include "PdfTemplateShadingItem.h"
#include "PdfShadingDictionary.h"
#include <libxml/parser.h>
#include <libxml/xmlmemory.h>
#include "PdfGeneratedBalloon.h"
#include "Logger.h"

DLLEXPORT_TEST_FUNCTION struct PdfTemplateItemShapeRectangle* PdfTemplateItemShapeRectangle_CreateFromXml(xmlNode *node)
{
	struct PdfTemplateItemShapeRectangle *ret;
	char *tmp;

	xmlNode *foundNode;

	ret = (struct PdfTemplateItemShapeRectangle *)MemoryManager_Alloc(sizeof(struct PdfTemplateItemShapeRectangle));
	PdfTemplateBalloonItem_InitFromXml((struct PdfTemplateBalloonItem*)ret, node, PdfTemplateItemShapeRectangle_Destroy, PdfTemplateItemShapeRectangle_Process);
	
	//Load dimensions
	foundNode = PdfTemplate_FindNode(node, WIDTH);
	if(foundNode)
	{
		ret->width = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(foundNode, VALUE));
	}
	else
	{
		Logger_LogErrorMessage("PdfTemplateItemShapeRectangle_CreateFromXml: Cannot find WIDTH node");
		return NULL;
	}


	foundNode = PdfTemplate_FindNode(node, HEIGHT);
	if(foundNode)
	{
		ret->height = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(foundNode, VALUE));
	}
	else
	{
		Logger_LogErrorMessage("PdfTemplateItemShapeRectangle_CreateFromXml: Cannot find HEIGHT node");
		return NULL;
	}

	foundNode = PdfTemplate_FindNode(node, "UseShading");
	if (foundNode)
	{
		tmp = PdfTemplate_LoadStringAttribute(foundNode, VALUE);
		if (strcmp(tmp, "True") == 0)
		{
			ret->useShading = TRUE;
		}
		else
		{
			ret->useShading = FALSE;
		}
	} else 
	{
		ret->useShading = FALSE;
	}

	foundNode = PdfTemplate_FindNode(node, "UseStroke");
	if (foundNode)
	{
		tmp = PdfTemplate_LoadStringAttribute(foundNode, VALUE);
		if (strcmp(tmp, "True") == 0)
		{
			ret->useStroke = TRUE;
		}
		else
		{
			ret->useStroke = FALSE;
		}
	} else 
	{
		ret->useStroke = FALSE;
	}


	foundNode = PdfTemplate_FindNode(node, "ShadingItem");
	if(foundNode)
	{
		ret->shading = PdfTemplateShadingItem_CreateFromXml(foundNode);
	}else{
		ret->shading = 0;
	}
	
	foundNode = PdfTemplate_FindNode(node, STROKEWIDTH);
	if(foundNode)
	{
		ret->strokeWidth = (float)PdfTemplate_LoadDoubleAttribute(foundNode, VALUE);
	}else{
		ret->strokeWidth = 1;
	}

	foundNode = PdfTemplate_FindNode(node, USECMYKCOLOR);
	if(foundNode)
	{
		ret->useCMYKColor = PdfTemplate_LoadIntAttribute(foundNode, VALUE);
	}else{
		ret->useCMYKColor = 0;
	}

	if(!ret->useCMYKColor)
	{
		//Load RGB Stuff
		foundNode = PdfTemplate_FindNode(node, STROKECOLOR);
		if(foundNode)
		{
			ret->strokeColor_R = (float)PdfTemplate_LoadDoubleAttribute(foundNode, RGBCOLOR_R_COMPONENT) / 255.0f;
			ret->strokeColor_G = (float)PdfTemplate_LoadDoubleAttribute(foundNode, RGBCOLOR_G_COMPONENT) / 255.0f;
			ret->strokeColor_B = (float)PdfTemplate_LoadDoubleAttribute(foundNode, RGBCOLOR_B_COMPONENT) / 255.0f;
		}else{
			ret->strokeColor_R = 0;
			ret->strokeColor_G = 0;
			ret->strokeColor_B = 0;
		}

		foundNode = PdfTemplate_FindNode(node, FILLCOLOR);
		if(foundNode)
		{
			ret->fillColor_R = (float)PdfTemplate_LoadDoubleAttribute(foundNode, RGBCOLOR_R_COMPONENT) / 255.0f;
			ret->fillColor_G = (float)PdfTemplate_LoadDoubleAttribute(foundNode, RGBCOLOR_G_COMPONENT) / 255.0f;
			ret->fillColor_B = (float)PdfTemplate_LoadDoubleAttribute(foundNode, RGBCOLOR_B_COMPONENT) / 255.0f;
		}else{
			ret->fillColor_R = 1.0;
			ret->fillColor_G = 1.0;
			ret->fillColor_B = 1.0;
		}

		// Null everything else just to be safe...
		ret->strokeColor_C = 0;
		ret->strokeColor_M = 0;
		ret->strokeColor_Y = 0;
		ret->strokeColor_K = 0;

		ret->fillColor_C = 0;
		ret->fillColor_M = 0;
		ret->fillColor_Y = 0;
		ret->fillColor_K = 0;
	}else{
		//Load CMYK Stuff
		foundNode = PdfTemplate_FindNode(node, STROKECOLORCMYK);
		if(foundNode)
		{
			ret->strokeColor_C = PdfTemplate_LoadIntAttribute(foundNode, CMYKCOLOR_C_COMPONENT);
			ret->strokeColor_M = PdfTemplate_LoadIntAttribute(foundNode, CMYKCOLOR_M_COMPONENT);
			ret->strokeColor_Y = PdfTemplate_LoadIntAttribute(foundNode, CMYKCOLOR_Y_COMPONENT);
			ret->strokeColor_K = PdfTemplate_LoadIntAttribute(foundNode, CMYKCOLOR_K_COMPONENT);
		}else{
			ret->strokeColor_C = 0;
			ret->strokeColor_M = 0;
			ret->strokeColor_Y = 0;
			ret->strokeColor_K = 0;
		}

		foundNode = PdfTemplate_FindNode(node, FILLCOLORCMYK);
		if(foundNode)
		{
			ret->fillColor_C = PdfTemplate_LoadIntAttribute(foundNode, CMYKCOLOR_C_COMPONENT);
			ret->fillColor_M = PdfTemplate_LoadIntAttribute(foundNode, CMYKCOLOR_M_COMPONENT);
			ret->fillColor_Y = PdfTemplate_LoadIntAttribute(foundNode, CMYKCOLOR_Y_COMPONENT);
			ret->fillColor_K = PdfTemplate_LoadIntAttribute(foundNode, CMYKCOLOR_K_COMPONENT);
		}else{
			ret->fillColor_C = 255;
			ret->fillColor_M = 255;
			ret->fillColor_Y = 255;
			ret->fillColor_K = 255;
		}

		//Null everything else just to be safe...
		ret->fillColor_R = 0;
		ret->fillColor_G = 0;
		ret->fillColor_B = 0;

		ret->fillColor_R = 0;
		ret->fillColor_G = 0;
		ret->fillColor_B = 0;
	}
	

	return ret;
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemShapeRectangle_Destroy(struct PdfTemplateBalloonItem* self)
{
	PdfTemplateItemShapeRectangle_Cleanup((struct PdfTemplateItemShapeRectangle*)self);

	// destroy 
	MemoryManager_Free((struct PdfTemplateItemShapeRectangle*)self);
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemShapeRectangle_Cleanup(struct PdfTemplateItemShapeRectangle* self)
{
	if(self->width)
	{
		MemoryManager_Free(self->width);
		self->width = 0;
	}

	if(self->height)
	{
		MemoryManager_Free(self->height);
		self->height = 0;
	}


	//cleanup parent
	PdfTemplateBalloonItem_Cleanup((struct PdfTemplateBalloonItem*)self);
}

int PdfTemplateItemShapeRectangle_Process(struct PdfTemplateBalloonItem *self, struct PdfGenerator *generator, struct PdfGeneratedBalloon *balloon, struct StreamWriter *streamWriter)
{
	struct PdfGraphicWriter* gW;
	struct PdfTemplateItemShapeRectangle* item;
	struct TransformationMatrix* tM;
	struct UnitConverter *uC;
	struct Rectangle *rect;
	float tmpX, tmpY, tmpWidth, tmpHeight;

	Logger_LogNoticeMessage("PdfTemplateItemShapeRectangle_Process: BEGIN");
	item = (struct PdfTemplateItemShapeRectangle*)self;
	tM = TransformationMatrix_Create();
	uC = UnitConverter_Create();
	UnitConverter_AddCommonUnits(uC);
	

	PdfTemplateBalloonItem_GetFullTransformation(self, balloon, generator->currentPage, tM, FALSE);
	tmpX = UnitConverter_ConvertToPoints(uC, item->base.location->positionX);
	tmpY = UnitConverter_ConvertToPoints(uC, item->base.location->positionY);
	tmpWidth = UnitConverter_ConvertToPoints(uC, item->width);
	tmpHeight = UnitConverter_ConvertToPoints(uC, item->height);

	//rect = Rectangle_Create(streamWriter, tmpX, generator->currentPage->properties.mediaBox.upperRightY - tmpY - tmpHeight, tmpX + tmpWidth, generator->currentPage->properties.mediaBox.upperRightY - tmpY);
	rect = Rectangle_Create(streamWriter, 0, -tmpHeight, tmpWidth, 0);
	

	gW = PdfGraphicWriter_Create(streamWriter);
	if(!item->useCMYKColor)
	{
		PdfGraphicWriter_SetRGBFillColor(gW, item->fillColor_R, item->fillColor_G, item->fillColor_B);
		PdfGraphicWriter_SetRGBStrokeColor(gW, item->strokeColor_R, item->strokeColor_G, item->strokeColor_B);
	}
	else
	{	
		PdfGraphicWriter_SetCMYKFillColor(gW, item->fillColor_C, item->fillColor_M, item->fillColor_Y, item->fillColor_K);
		PdfGraphicWriter_SetCMYKStrokeColor(gW, item->strokeColor_C, item->strokeColor_M, item->strokeColor_Y, item->strokeColor_K);
	}

	PdfGraphicWriter_SetLineWidth(gW, item->strokeWidth);

	if(item->shading && item->useShading)
	{		
		Logger_LogNoticeMessage("PdfTemplateItemShapeRectangle_Process: Item is using shading BEGIN");
		PdfGraphicWriter_SaveGraphicState(gW);

		PdfGraphicWriter_SetTransformation(gW, tM->a, tM->b, tM->c, tM->d, tM->e, tM->f);
		PdfGraphicWriter_DrawRectangle(gW, rect, 0, 0);
		PdfGraphicWriter_SetClippingPath(gW);

		PdfTemplateShadingItem_Process(item->shading, generator, balloon, streamWriter); //Process ShadingItem because it isn't regular item...

		PdfGraphicWriter_SaveGraphicState(gW);
		PdfGraphicWriter_SetTransformation(gW, tmpWidth, 0, 0, -tmpHeight, 0, 0);
		PdfGraphicWriter_SetShading(gW, item->shading->shadingDictionary->name);
		PdfGraphicWriter_RestoreGraphicState(gW);

		PdfGraphicWriter_RestoreGraphicState(gW);
		Logger_LogNoticeMessage("PdfTemplateItemShapeRectangle_Process: Item is using shading END");
	}
	else
	{
		Logger_LogNoticeMessage("PdfTemplateItemShapeRectangle_Process: Draw not shaded rectangle");
		PdfGraphicWriter_SaveGraphicState(gW);
		PdfGraphicWriter_SetTransformation(gW, tM->a, tM->b, tM->c, tM->d, tM->e, tM->f);
		PdfGraphicWriter_DrawRectangle(gW, rect, 1, item->useStroke);
		PdfGraphicWriter_RestoreGraphicState(gW);
	}

	PdfGraphicWriter_Destroy(gW);
	TransformationMatrix_Destroy(tM);
	UnitConverter_Destroy(uC);

	Logger_LogNoticeMessage("PdfTemplateItemShapeRectangle_Process: END");
	return TRUE;
}
