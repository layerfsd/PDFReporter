/*
PdfTemplateBalloon.c

Author: Marko Vranjkovic
Date: 25.07.2008.	

*/

#include "PdfTemplateBalloon.h"
#include "PdfTemplateElements.h"
#include "PdfTemplateItemBorder.h"
#include "PdfTemplateLocation.h"
#include "DLList.h"
#include "Logger.h"
#include "MemoryManager.h"
#include "UnitConverter.h"
#include "PdfTemplate.h"
#include <libxml/parser.h>
#include <libxml/xmlmemory.h>


DLLEXPORT_TEST_FUNCTION void PdfTemplateBalloon_Write()
{
}

DLLEXPORT_TEST_FUNCTION struct PdfTemplateBalloon* PdfTemplateBalloon_Create(char *name, char *type, char *dataStream, char *version)
{
	struct PdfTemplateBalloon *ret;
	ret = (struct PdfTemplateBalloon*)MemoryManager_Alloc(sizeof(struct PdfTemplateBalloon));	
	PdfTemplateBalloon_Init(ret, name, type, dataStream, version);
	return ret;
}



int PdfTemplateBalloon_LoadItems(struct PdfTemplateBalloon *self, xmlNode *node)
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
					Logger_LogErrorMessage("PdfTemplateBalloon: LoadItems: Loading of balloon item failed");
					return FALSE;
				}
			}
		}
	}

	return TRUE;
}

/*
This will load all balloons
*/
int PdfTemplateBalloon_LoadBalloons(struct PdfTemplateBalloon *self, xmlNode *node)
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
					Logger_LogErrorMessage("PdfTemplateBalloon: LoadBalloons: Balloons are not loaded");
					return FALSE;
				}
				else 
				{
					DLList_PushBack(self->balloons, balloon);	
					balloon->parentBalloon = self;
				}
			}
		}
	}
	return TRUE;
}



/*
	Load balloon items
*/
int PdfTemplateBalloon_LoadVersion1(struct PdfTemplateBalloon *self, xmlNode *node)
{
	xmlNode *currentNode = NULL;
	xmlNode *foundNode = NULL;
	xmlAttr *currentNodeProperty = NULL;
	char *dockPosition;

	self->availableOnEveryPage = PdfTemplate_LoadBooleanValue(node, AVAILABLE_ON_EVERY_PAGE);
	self->fillingGeneratesNew = PdfTemplate_LoadBooleanValue(node, FILLING_GENERATES_NEW);
	self->fillCapacity = PdfTemplate_LoadIntValue(node, FILL_CAPACITY);
	self->canGrow = PdfTemplate_LoadBooleanValue(node, CAN_GROW);
	self->fitToContent = PdfTemplate_LoadBooleanValue(node, FIT_TO_CONTENT);

	// Load docking information
	foundNode = PdfTemplate_FindNode(node, DOCK_POSITION);
	if (foundNode)
	{
		dockPosition = PdfTemplate_LoadStringAttribute(foundNode, DOCK);
		if (strcmp(dockPosition, DOCK_BOTTOM_STRING) == 0) self->dockPosition = DOCK_BOTTOM;
		if (strcmp(dockPosition, DOCK_LEFT_STRING) == 0)  self->dockPosition = DOCK_LEFT;
		if (strcmp(dockPosition, DOCK_TOP_STRING) == 0)  self->dockPosition = DOCK_TOP;
		if (strcmp(dockPosition, DOCK_RIGHT_STRING) == 0)  self->dockPosition = DOCK_RIGHT;
		if (strcmp(dockPosition, DOCK_NONE_STRING) == 0)  self->dockPosition = DOCK_NONE;
	}

	// load fill color
	foundNode = PdfTemplate_FindNode(node, FILLCOLOR);
	if(foundNode)
	{
		self->fillColorR = (float)PdfTemplate_LoadDoubleAttribute(foundNode, RGBCOLOR_R_COMPONENT) / 255.0f;
		self->fillColorG = (float)PdfTemplate_LoadDoubleAttribute(foundNode, RGBCOLOR_G_COMPONENT) / 255.0f;
		self->fillColorB = (float)PdfTemplate_LoadDoubleAttribute(foundNode, RGBCOLOR_B_COMPONENT) / 255.0f;		
		self->fillColorA = (float)PdfTemplate_LoadDoubleAttribute(foundNode, RGBCOLOR_A_COMPONENT) / 255.0f;		
	}else{
		self->fillColorR = 1.0;
		self->fillColorG = 1.0;
		self->fillColorB = 1.0;
		self->fillColorA = 1.0;
	}

	// Load Borders
	Logger_LogNoticeMessage("Loading Balloon Border");
	foundNode = PdfTemplate_FindNode(node, TOP_BORDER);
	if (foundNode)
	{
		PdfTemplateItemBorder_LoadFromXml(&self->topBorder, foundNode);
	}
	foundNode = PdfTemplate_FindNode(node, BOTTOM_BORDER);
	if (foundNode)
	{
		PdfTemplateItemBorder_LoadFromXml(&self->bottomBorder, foundNode);
	}
	foundNode = PdfTemplate_FindNode(node, LEFT_BORDER);
	if (foundNode)
	{
		PdfTemplateItemBorder_LoadFromXml(&self->leftBorder, foundNode);
	}
	foundNode = PdfTemplate_FindNode(node, RIGHT_BORDER);
	if (foundNode)
	{
		PdfTemplateItemBorder_LoadFromXml(&self->rightBorder, foundNode);
	}
	// end loading borders


	foundNode = PdfTemplate_FindNode(node, LOCATION);
	if (foundNode)
	{
		self->location = PdfTemplateLocation_CreateFromXml(foundNode);
		Logger_LogNoticeMessage("Loading Balloon Location SUCCESS");
	}
	else 
	{
		Logger_LogErrorMessage("Loading Balloon Location: FAILED");
		return FALSE;
	}

	foundNode = PdfTemplate_FindNode(node, SHAPE);
	if (foundNode)
	{		
		self->shape = PdfTemplateBalloonShape_CreateFromXml(foundNode);
			Logger_LogNoticeMessage("Loading Balloon Shape SUCCESS");
	}
	else 
	{
		Logger_LogErrorMessage("Loading Balloon Shape: FAILED");
		return FALSE;
	}
	
	// load balloon sub balloons
	foundNode = PdfTemplate_FindNode(node, BALLOONS);
	if (foundNode)
	{
		if (!PdfTemplateBalloon_LoadBalloons(self, foundNode))
		{
		    Logger_LogErrorMessage("Loading Balloon Sub-Balloons: FAILED");
			return FALSE;
		}
	}

	// Load balloon items
	foundNode = PdfTemplate_FindNode(node, ITEMS);
	if (foundNode)
	{
		if (!PdfTemplateBalloon_LoadItems(self, foundNode))
		{
			Logger_LogErrorMessage("Loading Balloon Items: FAILED");
			return FALSE;
		}
	}

	return TRUE;
}


DLLEXPORT_TEST_FUNCTION struct PdfTemplateBalloon* PdfTemplateBalloon_CreateFromXml(xmlNode *node)
{
	struct PdfTemplateBalloon *newBallon;
	char *name, *type, *version, *dataStream = NULL;	
	xmlAttr *currentNodeProperty = node->properties;

	newBallon = (struct PdfTemplateBalloon*)MemoryManager_Alloc(sizeof(struct PdfTemplateBalloon));			

	while (currentNodeProperty != NULL)
	{
		if (strcmp(currentNodeProperty->name, NAME) == 0)
		{
			name = currentNodeProperty->children->content;			
		}
		else if (strcmp(currentNodeProperty->name, TYPE) == 0)
		{
			type = currentNodeProperty->children->content;			
		}
		else if (strcmp(currentNodeProperty->name, BALLOON_VERSION) == 0)
		{
			version = currentNodeProperty->children->content;			
		}
		else if (strcmp(currentNodeProperty->name, BALLOON_DATA_STREAM) == 0)
		{
			dataStream = currentNodeProperty->children->content;			
		}
		currentNodeProperty = currentNodeProperty->next;	
	}	
	PdfTemplateBalloon_Init(newBallon, name, type, dataStream, version);


	// load balloon items from node
	if (strcmp(version, "1.0") == 0)
	{
		if (!PdfTemplateBalloon_LoadVersion1(newBallon, node))
		{	
			Logger_LogErrorMessage("PdfTemplateBalloon: CreateFromXml: Properties of balloon are not loaded for balloon");
			PdfTemplateBalloon_Destroy(newBallon);
			return NULL;
		}
	}	
	return newBallon;
}


DLLEXPORT_TEST_FUNCTION void PdfTemplateBalloon_Init(struct PdfTemplateBalloon* self, char *name, char *type, char *dataStream, char *version)
{
	self->items = DLList_Create();
	self->balloons = DLList_Create();
	self->name = MemoryManager_StrDup(name);
	self->type = MemoryManager_StrDup(type);
	if(dataStream != NULL)
	{
		self->dataStream = MemoryManager_StrDup(dataStream);
	}else{
		self->dataStream = NULL;
	}
	self->version = MemoryManager_StrDup(version);
	self->shape = NULL;
	self->location = NULL;
	self->currentBalloonLocationX = -1;
	self->currentBalloonLocationY = -1;
	self->parentBalloon = NULL;
	self->skipDataReadMarker = FALSE;
	self->lastGeneratedPageNumber = 1;
	self->hasPrevDynamicTopDocked = FALSE;

	self->topBorder.enabled = FALSE;
	self->leftBorder.enabled = FALSE;
	self->rightBorder.enabled = FALSE;
	self->bottomBorder.enabled = FALSE;

	self->dockPosition = DOCK_NONE;

	if (strcmp(self->type, PDFTEMPLATE_BALLOON_TYPE_STATIC) == 0)
	{
		self->isStatic = TRUE;
	}
	else if (strcmp(self->type, PDFTEMPLATE_BALLOON_TYPE_DYNAMIC) == 0)
	{
		self->isStatic = FALSE;
	}
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateBalloon_Destroy(struct PdfTemplateBalloon* self)
{
	PdfTemplateBalloon_Cleanup(self);	
	PdfTemplateBalloonShape_Destroy(self->shape);
	PdfTemplateLocation_Destroy(self->location);
	MemoryManager_Free(self);
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateBalloon_Cleanup(struct PdfTemplateBalloon* self)
{
	if (self->name)
	{
		MemoryManager_Free(self->name);
		self->name = 0;
	}
	if (self->dataStream)
	{
		MemoryManager_Free(self->dataStream);
		self->dataStream = 0;
	}
	if (self->type)
	{
		MemoryManager_Free(self->type);
		self->type = 0;
	}
	if (self->version)
	{
		MemoryManager_Free(self->version);
		self->version = 0;
	}

	// destroy all balloons
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
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateBalloon_AddItem(struct PdfTemplateBalloon* self, struct PdfTemplateBalloonItem *item)
{
	DLList_PushBack(self->items, item);	
}

DLLEXPORT_TEST_FUNCTION float PdfTemplateBalloon_GetFitToContentWidth(struct PdfTemplateBalloon* self)
{
	struct UnitConverter *unitConverter;	
	float res; 

	unitConverter = UnitConverter_Create();
	UnitConverter_AddCommonUnits(unitConverter);
	res = UnitConverter_ConvertToPoints(unitConverter, self->shape->dimensions->width);
	UnitConverter_Destroy(unitConverter);
	return res;
}

DLLEXPORT_TEST_FUNCTION float PdfTemplateBalloon_GetFitToContentHeight(struct PdfTemplateBalloon* self)
{
	float res = 0;
	struct DLListNode *iter;	
	struct PdfTemplateBalloon *balloon;
	struct UnitConverter *unitConverter;	
	float tmpRes;
	float locY;
	float height;
	float heightToAdd = 0;
	
	unitConverter = UnitConverter_Create();
	UnitConverter_AddCommonUnits(unitConverter);
   
	if (self->fitToContent)
    {
        // if it is empty and has no other child balloons then result should be its own height
		if (self->balloons->size == 0)
        {
			res = UnitConverter_ConvertToPoints(unitConverter, self->shape->dimensions->height);
        }
        else
        {
            // count child static balloons and width should be the right most one
			for(iter = DLList_Begin(self->balloons); iter != DLList_End(self->balloons); iter = iter->next)
			{
				balloon = (struct PdfTemplateBalloon*)iter->data;
				if (balloon->isStatic)
				{
					if (balloon->dockPosition == DOCK_BOTTOM)
					{
						height = UnitConverter_ConvertToPoints(unitConverter, balloon->shape->dimensions->height);
						heightToAdd += height;
					}
					else 
					{
						locY = UnitConverter_ConvertToPoints(unitConverter, balloon->location->positionY);
						tmpRes = locY + PdfTemplateBalloon_GetFitToContentHeight(balloon);
						if (tmpRes > res)
						{
							res = tmpRes;
						}
					}
				}
			}
        }
    }
    else
    {
        // FitToContent is false, so just return height
		res = UnitConverter_ConvertToPoints(unitConverter, self->shape->dimensions->height);
    }

	UnitConverter_Destroy(unitConverter);

	return res + heightToAdd;
}

