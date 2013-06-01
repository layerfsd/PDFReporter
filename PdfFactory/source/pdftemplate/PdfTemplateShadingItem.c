//-------------------------------------------
//- File: PdfTemplateShadingItem.c			-
//- Author: Tomislav Kukic					-
//- Date: 9.1.2009							-
//-------------------------------------------


#include "PdfTemplateShadingItem.h"
#include "PdfTemplate.h"
#include "PdfShadingDictionary.h"
#include "PdfGenerator.h"
#include "PdfPage.h"
#include "PdfFunction.h"
#include "PdfGeneratedBalloon.h"
#include <libxml/parser.h>
#include <libxml/xmlmemory.h>
#include "MemoryManager.h"
#include "UnitConverter.h"
#include "PdfPageResources.h"



DLLEXPORT_TEST_FUNCTION struct PdfTemplateShadingItem *PdfTemplateShadingItem_CreateFromXml(xmlNode *node)
{
	struct PdfTemplateShadingItem *ret;
	xmlNode *foundNode;

	ret = (struct PdfTemplateShadingItem *)MemoryManager_Alloc(sizeof(struct PdfTemplateShadingItem));
	//PdfTemplateBalloonItem_InitFromXml((struct PdfTemplateBalloonItem*)ret, node, PdfTemplateShadingItem_Destroy, PdfTemplateShadingItem_Process);

	ret->fromR = 0;
	ret->fromG = 0;
	ret->fromB = 0;
	ret->toR = 1.0;
	ret->toG = 1.0;
	ret->toB = 1.0;
	ret->useCMYK = FALSE;
	ret->written = FALSE;
	ret->shadingDictionary = NULL;

	/*foundNode = PdfTemplate_FindNode(node, "StartLocation");
	if(foundNode)
	{
		ret->startX = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(foundNode, "StartX"));
		ret->startY = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(foundNode, "StartY"));
	}*/

	
	foundNode = PdfTemplate_FindNode(node, "Shading");
	if(foundNode)
	{
		 ret->type = PdfTemplate_LoadIntAttribute(foundNode, "Type");
		 //ret->size = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(foundNode, "Size"));
		 //ret->useCMYK = PdfTemplate_LoadIntAttribute(foundNode, "UseCMYK");		
		 //ret->shadingFactor = PdfTemplate_LoadDoubleAttribute(foundNode, "Factor");
	}
	else
	{
		return NULL;
	}

	// Load Axial attributes
	if(ret->type == 2)
	{
		foundNode = PdfTemplate_FindNode(node, "AxialCoords");
		if(foundNode)
		{
			ret->coordsX1 = PdfTemplate_LoadDoubleAttribute(foundNode, "FromX");
			ret->coordsY1 = PdfTemplate_LoadDoubleAttribute(foundNode, "FromY");
			ret->coordsX2 = PdfTemplate_LoadDoubleAttribute(foundNode, "ToX");
			ret->coordsY2 = PdfTemplate_LoadDoubleAttribute(foundNode, "ToY");
		}
	}

	// Load radial gradient
	/*if(ret->type == 3)
	{
		foundNode = PdfTemplate_FindNode(node, "RadialCoords");
		if(foundNode)
		{
			ret->fromX = PdfTemplate_LoadDoubleAttribute(foundNode, "FromX");
			ret->fromY = PdfTemplate_LoadDoubleAttribute(foundNode, "FromY");
			ret->fromR = PdfTemplate_LoadDoubleAttribute(foundNode, "FromR");
			ret->toX = PdfTemplate_LoadDoubleAttribute(foundNode, "ToX");
			ret->toY = PdfTemplate_LoadDoubleAttribute(foundNode, "ToY");
			ret->toR = PdfTemplate_LoadDoubleAttribute(foundNode, "ToR");
		}
	}*/


	foundNode = PdfTemplate_FindNode(node, "FunctionType");
	if(foundNode)
	{
		ret->functionType = PdfTemplate_LoadIntAttribute(foundNode, "Type");
	}else{
		return NULL;
	}

	
	if(!ret->useCMYK)
	{
		foundNode = PdfTemplate_FindNode(node, "Color");
		if(foundNode)
		{
			ret->fromR = PdfTemplate_LoadDoubleAttribute(foundNode, "FromR") / 255.0;
			ret->fromG = PdfTemplate_LoadDoubleAttribute(foundNode, "FromG") / 255.0;
			ret->fromB = PdfTemplate_LoadDoubleAttribute(foundNode, "FromB") / 255.0;

			ret->toR = PdfTemplate_LoadDoubleAttribute(foundNode, "ToR") / 255.0;
			ret->toG = PdfTemplate_LoadDoubleAttribute(foundNode, "ToG") / 255.0;
			ret->toB = PdfTemplate_LoadDoubleAttribute(foundNode, "ToB") / 255.0;
		}else{
			return NULL;
		}
	}else if(ret->useCMYK == 1)
	{
		foundNode = PdfTemplate_FindNode(node, "Color");
		if(foundNode)
		{
			ret->fromC = PdfTemplate_LoadDoubleAttribute(foundNode, "FromC");
			ret->fromM = PdfTemplate_LoadDoubleAttribute(foundNode, "FromM");
			ret->fromY = PdfTemplate_LoadDoubleAttribute(foundNode, "FromY");
			ret->fromK = PdfTemplate_LoadDoubleAttribute(foundNode, "FromK");

			ret->toC = PdfTemplate_LoadDoubleAttribute(foundNode, "ToC");
			ret->toM = PdfTemplate_LoadDoubleAttribute(foundNode, "ToM");
			ret->toY = PdfTemplate_LoadDoubleAttribute(foundNode, "ToY");
			ret->toK = PdfTemplate_LoadDoubleAttribute(foundNode, "ToK");
		}else{
			return NULL;
		}
	}	

	return ret;
}






DLLEXPORT_TEST_FUNCTION void PdfTemplateShadingItem_Cleanup(struct PdfTemplateShadingItem *self)
{
}




DLLEXPORT_TEST_FUNCTION void PdfTemplateShadingItem_Destroy(struct PdfTemplateShadingItem *self)
{
	PdfTemplateShadingItem_Cleanup(self);
	MemoryManager_Free(self);
}





DLLEXPORT_TEST_FUNCTION int  PdfTemplateShadingItem_Process(struct PdfTemplateShadingItem *self, struct PdfGenerator *generator, struct PdfGeneratedBalloon *balloon, struct StreamWriter *streamWriter)
{
	struct UnitConverter *uC;

	if (!self->shadingDictionary)
	{	
		uC = UnitConverter_Create();
		UnitConverter_AddCommonUnits(uC);

		self->shadingDictionary = PdfShadingDictionary_Create(generator->document, self->type, self->functionType, TRUE, self->useCMYK, self->shadingFactor);
		PdfShadingDictionary_SetAxialCoords(self->shadingDictionary, self->coordsX1, self->coordsY1, self->coordsX2, self->coordsY2);

		// only function type 2 is supported. This is linear interpolation
		if(self->functionType == 2)
		{
			PdfShadingDictionary_SetRGBStartColor(self->shadingDictionary, self->fromR, self->fromG, self->fromB);
			PdfShadingDictionary_SetRGBEndColor(self->shadingDictionary, self->toR, self->toG, self->toB);
		}
		/*else if(self->functionType == 3)
		{
			tmpFunction = PdfFunction_Create(generator->document, PDF_FUNCTION_TYPE_SIMPLEFUNCTION, self->shadingFactor);
			PdfShadingDictionary_AddNextFunction(self->shadingDictionary, tmpFunction);
			if(self->useCMYK == 0)
			{
				PdfShadingDictionary_SetRGBStartColor(self->shadingDictionary, self->fromR1, self->fromG1, self->fromB1);
				PdfShadingDictionary_SetRGBEndColor(self->shadingDictionary, self->toR1, self->toG1, self->toB1);
			}else if(self->useCMYK == 1)
			{
				PdfShadingDictionary_SetCMYKStartColor(self->shadingDictionary, self->fromC1, self->fromM1, self->fromY1, self->fromK1);
				PdfShadingDictionary_SetCMYKEndColor(self->shadingDictionary, self->toC1, self->toM1, self->toY1, self->toK1);
			}
			tmpFunction = 0;

			tmpFunction = PdfFunction_Create(generator->document, PDF_FUNCTION_TYPE_SIMPLEFUNCTION, self->shadingFactor);
			PdfShadingDictionary_AddNextFunction(self->shadingDictionary, tmpFunction);
			if(self->useCMYK == 0)
			{
				PdfShadingDictionary_SetRGBStartColor(self->shadingDictionary, self->fromR2, self->fromG2, self->fromB2);
				PdfShadingDictionary_SetRGBEndColor(self->shadingDictionary, self->toR2, self->toG2, self->toB2);
			}else if(self->useCMYK == 1)
			{
				PdfShadingDictionary_SetCMYKStartColor(self->shadingDictionary, self->fromC2, self->fromM2, self->fromY2, self->fromK2);
				PdfShadingDictionary_SetCMYKEndColor(self->shadingDictionary, self->toC2, self->toM2, self->toY2, self->toK2);
			}
			tmpFunction = 0;
		}*/		
	
		// if not written then write this shading item
		PdfShadingDictionary_Write(self->shadingDictionary, generator->document->streamWriter);				

		UnitConverter_Destroy(uC);
	}

	// add this to page resource. 
	PdfPageResources_AddShadingDictionary(generator->currentPage->properties.resources, self->shadingDictionary);

	return TRUE;
}

