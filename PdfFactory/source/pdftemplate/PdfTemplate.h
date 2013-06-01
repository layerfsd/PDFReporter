/*
PdfTemplate.h

Author: Marko Vranjkovic
Date: 25.07.2008.	

*/

#ifndef _PDFTEMPLATE_
#define _PDFTEMPLATE_

#include "PdfFactory.h"
#include <PdfTemplateHeader.h>
#include <PdfTemplateDataStreams.h>
#include <PdfTemplatePage.h>
#include <PdfTemplateBalloonItem.h>
#include <PdfTemplateItemStaticText.h>
#include <PdfTemplateItemDynamicText.h>
#include <PdfTemplateFont.h>
#include <DLList.h>
#include <libxml/tree.h>

struct PdfTemplate
{
	struct PdfTemplateHeader *header;
	struct PdfTemplateDataStreams *dataStreams;
	struct PdfTemplatePage *page;	

	struct DLList *embeddedTemplateFonts; 

	struct PdfTemplateDataStream* currentDataStream;
	struct PdfTemplateBalloon *currentBalloon;
	struct PdfTemplateBalloonItem *currentItem;
	struct PdfTemplateTextProperties *currentTextProperties;
	struct PdfTemplateInformationDictionary *infoDict;
};

// current template used. This is like static instance as template is singletone
struct PdfTemplate* CurrentTemplate; 

DLLEXPORT_TEST_FUNCTION void PdfTemplate_Write();

DLLEXPORT_TEST_FUNCTION struct PdfTemplate* PdfTemplate_Create();

DLLEXPORT_TEST_FUNCTION void PdfTemplate_Init(struct PdfTemplate *self);

DLLEXPORT_TEST_FUNCTION void PdfTemplate_Destroy(struct PdfTemplate *self);

DLLEXPORT_TEST_FUNCTION int PdfTemplate_Load(struct PdfTemplate *self, char *fileName);
DLLEXPORT_TEST_FUNCTION int PdfTemplate_LoadFromData(struct PdfTemplate *self, char *templateData, int templateSize);


DLLEXPORT_TEST_FUNCTION int PdfTemplate_InitXmlElements(struct PdfTemplate *self, xmlNode * initNode);
/* this will return false if error on loading */

DLLEXPORT_TEST_FUNCTION int PdfTemplate_LoadBooleanValue(xmlNode *node, char *nodeName);
/* Loads boolean value from xml, 0 or 1 */

DLLEXPORT_TEST_FUNCTION int PdfTemplate_LoadIntValue(xmlNode *node, char *nodeName);
/* Loads boolean value from xml */

DLLEXPORT_TEST_FUNCTION int PdfTemplate_LoadIntAttribute(xmlNode *node, char *attrName);
/* Loads int value from xml */

/* Load Text Content but will not duplicate string retrieved */
DLLEXPORT_TEST_FUNCTION char* PdfTemplate_LoadTextContent(xmlNode *node);

DLLEXPORT_TEST_FUNCTION short PdfTemplate_LoadBooleanAttribute(xmlNode *node, char *attrName);

DLLEXPORT_TEST_FUNCTION double PdfTemplate_LoadDoubleAttribute(xmlNode *node, char *attrName);
/* Loads double value from xml */

DLLEXPORT_TEST_FUNCTION struct PdfTemplateEmbeddedFont *PdfTemplate_FindFont(struct PdfTemplate *self, int saveId);

DLLEXPORT_TEST_FUNCTION char* PdfTemplate_LoadStringAttribute(xmlNode *node, char *attrName);

DLLEXPORT_TEST_FUNCTION xmlNode *PdfTemplate_FindNode(xmlNode *parent, char *nodeName);

int PdfTemplate_LoadHeader(struct PdfTemplate *self, xmlNode* node);
int PdfTemplate_LoadPage(struct PdfTemplate *self, xmlNode* node);


#endif