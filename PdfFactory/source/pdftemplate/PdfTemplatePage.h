/*
PdfTemplatePage.h

Author: Marko Vranjkovic
Date: 25.07.2008.	

*/

#ifndef _PDFTEMPLATEPAGE_
#define _PDFTEMPLATEPAGE_

#include "PdfTemplateBalloon.h"
#include "PdfTemplatePageInfo.h"
#include "PdfTemplatePageSize.h"
#include "PdfTemplateInformationDictionary.h"
#include <libxml/tree.h>


struct PdfTemplatePage
{
	char *version;
	struct DLList* balloons;
	struct DLList *items; // items that are not inside balloons. Used for watermarking, etc.
	struct PdfTemplatePageInfo* info;
	struct PdfTemplatePageSize* size;
	struct PdfTemplateInformationDictionary* infoDict;

	// display properties
	float fillColorR;
	float fillColorG;
	float fillColorB;
	float fillColorA;
	char *imageData;
	int imageDataLength;
	char *imageName;
};

DLLEXPORT_TEST_FUNCTION void PdfTemplatePage_Write();

DLLEXPORT_TEST_FUNCTION struct PdfTemplatePage* PdfTemplatePage_Create();

DLLEXPORT_TEST_FUNCTION struct PdfTemplatePage* PdfTemplatePage_CreateFromXml(xmlNode *node);

DLLEXPORT_TEST_FUNCTION void PdfTemplatePage_Init(struct PdfTemplatePage* self, char* versionParam, struct PdfTemplatePageInfo *infoParam, struct PdfTemplatePageSize *sizeParam);

DLLEXPORT_TEST_FUNCTION void PdfTemplatePage_Destroy(struct PdfTemplatePage* self);

DLLEXPORT_TEST_FUNCTION void PdfTemplatePage_AddBalloon(struct PdfTemplatePage *self, struct PdfTemplateBalloon *balloon);

DLLEXPORT_TEST_FUNCTION void PdfTemplatePage_GetFullTransformation(struct PdfTemplatePage *self, struct PdfPage *page, const struct TransformationMatrix *outTransformation, int anchorIsCenter);


#endif