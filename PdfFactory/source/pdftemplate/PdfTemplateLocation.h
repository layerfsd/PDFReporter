/*
PdfTemplateLocation.h

Author: Marko Vranjkovic
Date: 19.08.2008.	

*/

#ifndef _PDFTEMPLATELOCATION_
#define _PDFTEMPLATELOCATION_

#include "PdfFactory.h"
#include <libxml/parser.h>
#include <libxml/xmlmemory.h>


struct PdfTemplateLocation
{	
	char *positionX;
	char *positionY;
};

DLLEXPORT_TEST_FUNCTION void PdfTemplateLocation_Write();

DLLEXPORT_TEST_FUNCTION struct PdfTemplateLocation* PdfTemplateLocation_Create();
DLLEXPORT_TEST_FUNCTION struct PdfTemplateLocation* PdfTemplateLocation_CreateFromXml(xmlNode *node);

DLLEXPORT_TEST_FUNCTION void PdfTemplateLocation_Init(struct PdfTemplateLocation* self);

DLLEXPORT_TEST_FUNCTION void PdfTemplateLocation_Destroy(struct PdfTemplateLocation* self);


#endif