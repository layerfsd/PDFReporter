/*
PdfTemplatePage.c

Author: Marko Vranjkovic
Date: 25.07.2008.	

*/

#include "PdfTemplatePage.h"
#include "DLList.h"
#include "PdfTemplateElements.h"
#include "PdfTemplate.h"
#include "MemoryManager.h"
#include "PdfPage.h"
#include "Logger.h"

DLLEXPORT_TEST_FUNCTION void PdfTemplatePage_Write()
{
}

DLLEXPORT_TEST_FUNCTION struct PdfTemplatePage* PdfTemplatePage_Create()
{
	struct PdfTemplatePage *ret;
	ret = (struct PdfTemplatePage*)MemoryManager_Alloc(sizeof(struct PdfTemplatePage));
	ret->balloons = DLList_Create();
	ret->items = DLList_Create();
	ret->info = PdfTemplatePageInfo_Create();
	ret->size = PdfTemplatePageSize_Create();
	ret->version = NULL;
	return ret;
}

/*
This will load all balloons
*/
int PdfTemplatePage_LoadBalloons(struct PdfTemplatePage *page, xmlNode *node)
{
	xmlNode *currentNode = NULL;
	struct PdfTemplateBalloon *balloon;

	currentNode = node->children;

	for(currentNode = node->children; currentNode; currentNode = currentNode->next)
	{
		if (currentNode->type == XML_ELEMENT_NODE)
		{
			if (strcmp(currentNode->name, BALLOON) == 0)
			{
				balloon = PdfTemplateBalloon_CreateFromXml(currentNode);
				if (!balloon)
				{
					// issue on loading balloon
					Logger_LogErrorMessage("PdfTemplate: LoadBalloons: Balloons are not loaded FAILED");
					return FALSE;
				}
				else 
				{
					PdfTemplatePage_AddBalloon(page, balloon);
				}
			}
		}
	}
	Logger_LogNoticeMessage("PdfTemplatePage: LoadBalloons: SUCCESS");
	return TRUE;
}

/*
   Loads items that are not stored inside any balloon
*/
int PdfTemplatePage_LoadItems(struct PdfTemplatePage *self, xmlNode *node)
{
	xmlNode *currentNode;
	struct PdfTemplateBalloonItem *item;

	
	for(currentNode = node->children; currentNode; currentNode = currentNode->next)
	{
		if (currentNode->type == XML_ELEMENT_NODE)
		{
			if (strcmp(currentNode->name, ITEM) == 0)
			{
				// create item from xml
				item = PdfTemplateBalloonItem_CreateFromXml(currentNode);
				if (item)
				{
					DLList_PushBack(self->items, item);
				}
				else
				{
					Logger_LogErrorMessage("PdfTemplatePage: LoadItems: Loading of page items FAILED");
					return FALSE;
				}
			}
		}
	}

	return TRUE;
}



DLLEXPORT_TEST_FUNCTION void PdfTemplatePage_GetFullTransformation(struct PdfTemplatePage *self, struct PdfPage *page, const struct TransformationMatrix *outTransformation, int anchorIsCenter)
{
	struct UnitConverter *unitConverter;
	struct TransformationMatrix *translation, *scale, *resMat2;
	float locX = 0, locY = 0;
	float scaleX, scaleY;
	float absX, absY;
	
	// create unit converter
	unitConverter = UnitConverter_Create();
	UnitConverter_AddCommonUnits(unitConverter);

	scale = TransformationMatrix_Create();	
	resMat2 = TransformationMatrix_Create();
	translation = TransformationMatrix_Create();

	scaleX = 100; //page->properties.mediaBox.upperRightX;
	scaleY = 100; //page->properties.mediaBox.upperRightY;
	
	// Make scale in unit space
	if (anchorIsCenter)
	{
		TransformationMatrix_Translate(translation, -scaleX/2, scaleY/2);
	}		

	TransformationMatrix_Scale(scale, scaleX, scaleY);
	TransformationMatrix_Multiply(scale, translation, resMat2);	
		
	// Make location in unit space
	absX = 0;
	absY = page->properties.mediaBox.upperRightY; 

	
	// Multiply and make result	
	TransformationMatrix_Translate(translation, absX, absY);
	TransformationMatrix_Multiply(resMat2, translation, outTransformation);

	UnitConverter_Destroy(unitConverter);	
	TransformationMatrix_Destroy(scale);
	TransformationMatrix_Destroy(translation);
	TransformationMatrix_Destroy(resMat2);
}

void PdfTemplatePage_FindHasPrevDynamicTopDocked(struct PdfTemplatePage *self, struct PdfTemplateBalloon *balloon)
{
	struct DLListNode *iter;
	struct PdfTemplateBalloon* templateBalloon;

	if (balloon->parentBalloon == NULL)
	{
		for(iter = DLList_Begin(self->balloons); iter != DLList_End(self->balloons); iter = iter->next)
		{
			templateBalloon = (struct PdfTemplateBalloon*)iter->data;

			if (!templateBalloon->isStatic && templateBalloon->dockPosition == DOCK_TOP)
			{
				balloon->hasPrevDynamicTopDocked = TRUE;
				return;
			}
			if (templateBalloon == balloon)
			{
				balloon->hasPrevDynamicTopDocked = FALSE;
				return;
			}
		}
	}
	else 
	{
		for(iter = DLList_Begin(balloon->parentBalloon->balloons); iter != DLList_End(balloon->parentBalloon->balloons); iter = iter->next)
		{
			templateBalloon = (struct PdfTemplateBalloon*)iter->data;

			if (!templateBalloon->isStatic && templateBalloon->dockPosition == DOCK_TOP)
			{
				balloon->hasPrevDynamicTopDocked = TRUE;
				return;
			}
			if (templateBalloon == balloon)
			{
				balloon->hasPrevDynamicTopDocked = FALSE;
				return;
			}
		}
	}
}

void PdfTemplatePage_UpdateBalloonsRecursive(struct PdfTemplatePage *self, struct PdfTemplateBalloon *parent)
{
	struct DLListNode *node;
	struct PdfTemplateBalloon *templateBalloon;

	if (parent)
	{
		for(node = DLList_Begin(parent->balloons); node != DLList_End(parent->balloons); node = node->next)
		{
			templateBalloon = (struct PdfTemplateBalloon*)node->data;
			if (templateBalloon->isStatic && templateBalloon->dockPosition == DOCK_TOP)
			{
				PdfTemplatePage_FindHasPrevDynamicTopDocked(self, templateBalloon);
			}

			PdfTemplatePage_UpdateBalloonsRecursive(self, templateBalloon);
		}
	}
}

//	Update balloon properties as we have complete hierarchy loaded now
void PdfTemplatePage_UpdateBalloons(struct PdfTemplatePage *self)
{
	struct DLListNode *node;
	struct PdfTemplateBalloon *templateBalloon;

	for(node = DLList_Begin(self->balloons); node != DLList_End(self->balloons); node = node->next)
	{
		templateBalloon = (struct PdfTemplateBalloon*)node->data;
		if (templateBalloon->isStatic && templateBalloon->dockPosition == DOCK_TOP)
		{
			PdfTemplatePage_FindHasPrevDynamicTopDocked(self, templateBalloon);
		}

		PdfTemplatePage_UpdateBalloonsRecursive(self, templateBalloon);
	}	
}

DLLEXPORT_TEST_FUNCTION struct PdfTemplatePage* PdfTemplatePage_CreateFromXml(xmlNode *node)
{
	struct PdfTemplatePage *ret;
	char *embeddedImageData;
	xmlNode *foundNode;

	ret = (struct PdfTemplatePage*)MemoryManager_Alloc(sizeof(struct PdfTemplatePage));
	ret->balloons = DLList_Create();
	ret->items = DLList_Create();
	ret->imageData = 0;
	ret->imageDataLength = 0;
	ret->version = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(node, VERSION));
	
	foundNode = PdfTemplate_FindNode(node, PAGE_INFO);
	if (foundNode)
	{
		ret->info = PdfTemplatePageInfo_CreateFromXml(foundNode);
		Logger_LogNoticeMessage("PdfTemplatePage: Loading Template Page Info SUCCESS");
	}
	else 
	{
		Logger_LogErrorMessage("PdfTemplatePage: CreateFromXml: failed to find page info node");
		return FALSE;
	}

	foundNode = PdfTemplate_FindNode(node, PAGE_SIZE);
	if (foundNode)
	{
		ret->size = PdfTemplatePageSize_CreateFromXml(foundNode);
		Logger_LogNoticeMessage("PdfTemplatePage: Loading Template Page Size SUCCESS");
	}
	else 
	{
		Logger_LogErrorMessage("PdfTemplatePage: CreateFromXml: failed to find page info node");
		return FALSE;
	}

	// load fill color
	foundNode = PdfTemplate_FindNode(node, FILLCOLOR);
	if(foundNode)
	{
		ret->fillColorR = (float)PdfTemplate_LoadDoubleAttribute(foundNode, RGBCOLOR_R_COMPONENT) / 255.0f;
		ret->fillColorG = (float)PdfTemplate_LoadDoubleAttribute(foundNode, RGBCOLOR_G_COMPONENT) / 255.0f;
		ret->fillColorB = (float)PdfTemplate_LoadDoubleAttribute(foundNode, RGBCOLOR_B_COMPONENT) / 255.0f;		
	}else{
		ret->fillColorR = 1.0;
		ret->fillColorG = 1.0;
		ret->fillColorB = 1.0;
	}

	foundNode = PdfTemplate_FindNode(node, PAGE_BACKGROUND_IMAGE);
	if (foundNode)
	{
		ret->imageName = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(foundNode, SRC_NAME));
	}
	
	// Load background image
	foundNode = PdfTemplate_FindNode(node, "EmbeddedImage");
	if (foundNode)
	{
		// Load content into memory
		ret->imageDataLength = PdfTemplate_LoadIntAttribute(foundNode, "EmbeddedDecodedImageLength");
		if (ret->imageDataLength > 0)
		{
			ret->imageData = MemoryManager_Alloc(ret->imageDataLength);
			embeddedImageData = PdfTemplate_LoadTextContent(foundNode);		
			Base64Decode(embeddedImageData, ret->imageData, ret->imageDataLength);			
		}
	}

	foundNode = PdfTemplate_FindNode(node, BALLOONS);
	if (foundNode)
	{
		if (!PdfTemplatePage_LoadBalloons(ret, foundNode))
		{
			Logger_LogNoticeMessage("PdfTemplatePage: Loaded: FAILED");
			return FALSE;
		}
	}
	else 
	{
		Logger_LogErrorMessage("PdfTemplatePage: Balloons Node not found FAILED");
		return FALSE;
	}

	// UpdateBalloon properties as we now have hierarchy of balloons loaded
	PdfTemplatePage_UpdateBalloons(ret);

	// load items that are not part of any balloon
	// Load balloon items
	foundNode = PdfTemplate_FindNode(node, ITEMS);
	if (foundNode)
	{
		if (!PdfTemplatePage_LoadItems(ret, foundNode))
		{			
			Logger_LogErrorMessage("PdfTemplatePage: Watermark items not loaded correctly FAILED.");
			return FALSE;
		}
	}

	Logger_LogNoticeMessage("PdfTemplatePage: Loaded: SUCCESS");
	return ret;
}

DLLEXPORT_TEST_FUNCTION void PdfTemplatePage_Init(struct PdfTemplatePage* self, char *versionParam, struct PdfTemplatePageInfo *infoParam, struct PdfTemplatePageSize *sizeParam)
{
	self->version = MemoryManager_StrDup(versionParam);
	*self->info = *infoParam;
	*self->size = *sizeParam;	
}

DLLEXPORT_TEST_FUNCTION void PdfTemplatePage_Destroy(struct PdfTemplatePage* self)
{
	// remove memory for image
	if (self->imageData)
	{
		MemoryManager_Free(self->imageData);
		self->imageData = 0;
		self->imageDataLength = 0;
	}

	// destroy all content streams	
	while(self->balloons->size > 0)
	{
		struct PdfTemplateBalloon *obj;
		obj = (struct PdfTemplateBalloon*)DLList_Back(self->balloons);
		DLList_PopBack(self->balloons);
		PdfTemplateBalloon_Destroy(obj);
	}
	DLList_Destroy(self->balloons); // destroy list itself

	// destroy all items
	while(self->items->size > 0)
	{
		struct PdfTemplateBalloonItem *obj;
		obj = (struct PdfTemplateBalloonItem*)DLList_Back(self->items);
		DLList_PopBack(self->items);
		PdfTemplateBalloonItem_Destroy(obj);
	}
	DLList_Destroy(self->items); // destroy list itself


	MemoryManager_Free(self->version);
	self->version = NULL;
	PdfTemplatePageInfo_Destroy(self->info);
	PdfTemplatePageSize_Destroy(self->size);
	MemoryManager_Free(self);
}

DLLEXPORT_TEST_FUNCTION void PdfTemplatePage_AddBalloon(struct PdfTemplatePage *self, struct PdfTemplateBalloon *balloon)
{
	DLList_PushBack(self->balloons, balloon);	
}