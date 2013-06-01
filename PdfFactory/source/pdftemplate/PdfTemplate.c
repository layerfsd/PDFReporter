/*
PdfTemplate.c

Author: Marko Vranjkovic
Date: 25.07.2008.	

*/

#include "PdfTemplate.h"
#include "PdfTemplateElements.h"
#include "PdfTemplateDataStreams.h"
#include "PdfTemplateHeader.h"
#include "PdfTemplatePage.h"
#include "PdfTemplateColumns.h"
#include "PdfTemplateEmbeddedFont.h"
#include "MemoryManager.h"
#include "Logger.h"
#include "DLList.h"
#include "PdfTemplateInformationDictionary.h"
#include <libxml/parser.h>
#include <libxml/xmlmemory.h>


DLLEXPORT_TEST_FUNCTION void PdfTemplate_Write()
{
}

DLLEXPORT_TEST_FUNCTION struct PdfTemplate* PdfTemplate_Create()
{
	struct PdfTemplate *ret;
	ret = (struct PdfTemplate*)MemoryManager_Alloc(sizeof(struct PdfTemplate));
	PdfTemplate_Init(ret);
	CurrentTemplate = ret;
	return ret;
}

DLLEXPORT_TEST_FUNCTION void PdfTemplate_Init(struct PdfTemplate *self)
{
	self->header = PdfTemplateHeader_Create();		
	self->embeddedTemplateFonts = DLList_Create();
}

DLLEXPORT_TEST_FUNCTION void PdfTemplate_Destroy(struct PdfTemplate *self)
{	
	// destroy all content streams	
	while(self->embeddedTemplateFonts->size > 0)
	{
		struct PdfTemplateEmbeddedFont *obj;
		obj = (struct PdfTemplateEmbeddedFont*)DLList_Back(self->embeddedTemplateFonts);
		DLList_PopBack(self->embeddedTemplateFonts);
		PdfTemplateEmbeddedFont_Destroy(obj);
	}
	DLList_Destroy(self->embeddedTemplateFonts); // destroy list itself


	PdfTemplateDataStreams_Destroy(self->dataStreams);
	PdfTemplateHeader_Destroy(self->header);
	PdfTemplatePage_Destroy(self->page);
	MemoryManager_Free(self);
	CurrentTemplate = 0; // we no longer have valid current template
}

DLLEXPORT_TEST_FUNCTION int PdfTemplate_LoadFromData(struct PdfTemplate *self, char *templateData, int templateSize)
{
	xmlDoc *doc = NULL;
    xmlNode *rootElement = NULL;
	xmlNode *currentNode = NULL;
	int res = TRUE;
	
	doc = xmlReadMemory(templateData, templateSize, NULL, 0, 0);    
	Logger_LogNoticeMessage("PdfTemplate_LoadFromData: xmlReadMemory SUCCESS");

    if (doc == NULL) return FALSE;	
    rootElement = xmlDocGetRootElement(doc);
	self->currentDataStream = NULL;
	self->currentBalloon = NULL;
	self->currentItem = NULL;
	self->currentTextProperties = NULL;
	
    if (!PdfTemplate_InitXmlElements(self, rootElement))
	{
		res = FALSE;
	}
	xmlFreeDoc(doc);
	xmlCleanupParser();	    
	if (res)
	{
		Logger_LogNoticeMessage("PdfTemplate_Load: Initialize Xml elements SUCCESS");
	}
	else 
	{
		Logger_LogErrorMessage("PdfTemplate_Load: Initialize Xml elements FAILED");
	}

	return res;
}


//Returns 1 if success, 0 else
DLLEXPORT_TEST_FUNCTION int PdfTemplate_Load(struct PdfTemplate *self, char *fileName)
{
	xmlDoc *doc = NULL;
    xmlNode *rootElement = NULL;
	xmlNode *currentNode = NULL;
	int res = TRUE;
	
    doc = xmlReadFile(fileName, NULL, 0);
	Logger_LogNoticeMessage("PdfTemplate_Load: xmlReadFile SUCCESS");

    if (doc == NULL) return FALSE;

	
    rootElement = xmlDocGetRootElement(doc);

	self->currentDataStream = NULL;
	self->currentBalloon = NULL;
	self->currentItem = NULL;
	self->currentTextProperties = NULL;
	
    if (!PdfTemplate_InitXmlElements(self, rootElement))
	{
		res = FALSE;
	}
	xmlFreeDoc(doc);
	xmlCleanupParser();	    
	if (res)
	{
		Logger_LogNoticeMessage("PdfTemplate_Load: Initialize Xml elements SUCCESS");
	}
	else 
	{
		Logger_LogErrorMessage("PdfTemplate_Load: Initialize Xml elements FAILED");
	}

	return res;
}



DLLEXPORT_TEST_FUNCTION int PdfTemplate_InitXmlElements(struct PdfTemplate *self, xmlNode * initNode)
{
    xmlNode *currentNode = NULL;	
	struct PdfTemplateDataStream* tempDataStream = NULL;
	struct PdfTemplateBalloon *tempBalloon = NULL;
	struct PdfTemplateBalloonItem *tempItem = NULL;
	struct PdfTemplateTextProperties *tempTextProperties = NULL;

    for (currentNode = initNode->children; currentNode; currentNode = currentNode->next) {
        
		if (currentNode->type == XML_ELEMENT_NODE)
		{
			if (strcmp(currentNode->name, HEADER) == 0)
			{				
				PdfTemplate_LoadHeader(self, currentNode);
			}				
			else if (strcmp(currentNode->name, DATA_STREAMS) == 0)
			{
				// skip loading of data streams as we don't need them anymore
				//PdfTemplate_LoadDataStreams(self, currentNode);
			}
			else if (strcmp(currentNode->name, FONTS) == 0)
			{
				PdfTemplate_LoadTemplateFonts(self, currentNode);
			}
			else if (strcmp(currentNode->name, PAGE) == 0)
			{
				if (!PdfTemplate_LoadPage(self, currentNode))
				{
					Logger_LogErrorMessage("PdfTemplate_InitXmlElements: LoadPage FAILED");
					return FALSE;
				}
			}			
		}
	}	
	Logger_LogNoticeMessage("PdfTemplate_InitXmlElements: SUCCESS");
	return TRUE;
}

DLLEXPORT_TEST_FUNCTION struct PdfTemplateEmbeddedFont *PdfTemplate_FindFont(struct PdfTemplate *self, int saveId)
{
	struct DLListNode *iterator;
	struct PdfTemplateEmbeddedFont *font; 
	for(iterator = DLList_Begin(self->embeddedTemplateFonts); iterator != DLList_End(self->embeddedTemplateFonts); iterator = iterator->next)
	{
		font = (struct PdfTemplateEmbeddedFont*)iterator->data;
		if (font->saveId == saveId)
		{
			return font;
		}
	}
	return NULL;
}


///	Load fonts from xml template
int PdfTemplate_LoadTemplateFonts(struct PdfTemplate *self, xmlNode* node)
{	
	xmlNode *currentNode = NULL;
	struct PdfTemplateEmbeddedFont *templateFont;
	
	for (currentNode = node->children; currentNode; currentNode = currentNode->next) 
	{
		if (currentNode->type == XML_ELEMENT_NODE)
		{
			if (strcmp(currentNode->name, FONT) == 0)
			{				
				templateFont = PdfTemplateEmbeddedFont_CreateFromXml(currentNode);
				DLList_PushBack(self->embeddedTemplateFonts, templateFont);
			}
		}
	}
}

int PdfTemplate_LoadHeader(struct PdfTemplate *self, xmlNode* node)
{	
	xmlAttr *currentNodeProperty = node->properties;
	xmlNode *foundNode = NULL;		
	self->header->version = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(node, VERSION));
		
	foundNode = PdfTemplate_FindNode(node, TEMPLATE_INFO);
	if (foundNode)
	{	
		self->header->info->author = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(foundNode, AUTHOR));
		self->header->info->date = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(foundNode, DATE));
		self->infoDict = PdfTemplateInformationDictionary_CreateFromXml(foundNode);
	}	

	Logger_LogNoticeMessage("PdfTemplate_LoadHeader: Loading template header SUCCESS");
	return TRUE;
}



int PdfTemplate_LoadDataStreams(struct PdfTemplate *self, xmlNode* node)
{
	xmlAttr *currentNodeProperty = node->properties;

	self->dataStreams = PdfTemplateDataStreams_CreateFromXml(node);		
	if (!self->dataStreams)
	{
		return FALSE;
	}

	return TRUE;
}



int PdfTemplate_LoadPage(struct PdfTemplate *self, xmlNode* node)
{
	xmlAttr *currentNodeProperty = node->properties;
	struct PdfTemplatePage *page;

	page = PdfTemplatePage_CreateFromXml(node);
	if (page)
	{
		self->page = page;	
	}
	else 
	{
		Logger_LogErrorMessage("Loading page from xml template. FAILED");
		return FALSE;
	}
	
	Logger_LogNoticeMessage("PdfTemplate_LoadPage: Loading page from xml template SUCCESS");
	return TRUE;
}



/*
returns 1 for TRUE, 0 FALSE, -1 error
*/
DLLEXPORT_TEST_FUNCTION int PdfTemplate_LoadBooleanValue(xmlNode *node, char *nodeName)
{
	xmlNode *currentNode = NULL;
	xmlAttr *currentNodeProperty = NULL;

	for(currentNode = node->children; currentNode; currentNode = currentNode->next)
	{
		if (currentNode->type == XML_ELEMENT_NODE)
		{
			if (strcmp(currentNode->name, nodeName) == 0)
			{
				for(currentNodeProperty = currentNode->properties; currentNodeProperty; currentNodeProperty = currentNodeProperty->next)
				{
					if (strcmp(currentNodeProperty->name, VALUE) == 0)
					{
						if (strcmp(currentNodeProperty->children->content, "True") == 0)
						{
							return TRUE;
						}
						else
						{
							return FALSE;
						}
					}					
				}			
			}
		}
	}
	return FALSE;	
}

DLLEXPORT_TEST_FUNCTION int PdfTemplate_LoadIntValue(xmlNode *node, char *nodeName)
{
	xmlNode *currentNode = NULL;
	xmlAttr *currentNodeProperty = NULL;

	for(currentNode = node->children; currentNode; currentNode = currentNode->next)
	{
		if (currentNode->type == XML_ELEMENT_NODE)
		{
			if (strcmp(currentNode->name, nodeName) == 0)
			{
				for(currentNodeProperty = currentNode->properties; currentNodeProperty; currentNodeProperty = currentNodeProperty->next)
				{
					if (strcmp(currentNodeProperty->name, VALUE) == 0)
					{
						return atoi(currentNodeProperty->children->content);						
					}					
				}			
			}
		}
	}
	return 0;	
}

/* 
	This will not duplicate text content by MemoryManager_StrDup
*/
DLLEXPORT_TEST_FUNCTION char* PdfTemplate_LoadTextContent(xmlNode *node)
{
	xmlNode *currentNode;
    for (currentNode = node->children; currentNode; currentNode = currentNode->next) 
	{       
		if (currentNode->type == XML_TEXT_NODE)
		{
			return currentNode->content;
		}
	}
	return 0;
}


DLLEXPORT_TEST_FUNCTION int PdfTemplate_LoadIntAttribute(xmlNode *node, char *attrName)
{
	xmlAttr *currentNodeProperty = node->properties;	
	for (currentNodeProperty = node->properties; currentNodeProperty; currentNodeProperty = currentNodeProperty->next)
	{
		if (strcmp(currentNodeProperty->name, attrName) == 0)
		{
			return atoi(currentNodeProperty->children->content);
		}		
	}
	return 0;
}

DLLEXPORT_TEST_FUNCTION double PdfTemplate_LoadDoubleAttribute(xmlNode *node, char *attrName)
{
	unsigned int i, len;	
	xmlAttr *currentNodeProperty = node->properties;	
	for (currentNodeProperty = node->properties; currentNodeProperty; currentNodeProperty = currentNodeProperty->next)
	{
		if (strcmp(currentNodeProperty->name, attrName) == 0)
		{
			len = strlen(currentNodeProperty->children->content);
			for(i = 0; i < len; i++)
			{
				if (currentNodeProperty->children->content[i] == ',')
				{
					currentNodeProperty->children->content[i] = '.';
				}
			}	
			return atof(currentNodeProperty->children->content);
		}		
	}
	return 0;
}

DLLEXPORT_TEST_FUNCTION short PdfTemplate_LoadBooleanAttribute(xmlNode *node, char *attrName)
{
	xmlAttr *currentNodeProperty = node->properties;	
	for (currentNodeProperty = node->properties; currentNodeProperty; currentNodeProperty = currentNodeProperty->next)
	{
		if (strcmp(currentNodeProperty->name, attrName) == 0)
		{
			if (strcmp(currentNodeProperty->children->content, "True") == 0)
			{
				return TRUE;
			}
			else 
			{
				return FALSE;
			}			
		}
	}
	return 0;
}


DLLEXPORT_TEST_FUNCTION char* PdfTemplate_LoadStringAttribute(xmlNode *node, char *attrName)
{
	xmlAttr *currentNodeProperty = node->properties;	
	for (currentNodeProperty = node->properties; currentNodeProperty; currentNodeProperty = currentNodeProperty->next)
	{
		if (strcmp(currentNodeProperty->name, attrName) == 0)
		{
			return MemoryManager_StrDup(currentNodeProperty->children->content);
		}
	}
	return NULL;
}

DLLEXPORT_TEST_FUNCTION xmlNode *PdfTemplate_FindNode(xmlNode *parent, char *nodeName)
{
	xmlNode *node;
	for(node = parent->children; node; node = node->next)
	{
		if (node->type == XML_ELEMENT_NODE)
		{
			if (strcmp(node->name, nodeName) == 0)
			{
				return node;
			}
		}
	}
	return NULL;
}


