/*
   AxiomCoders (c) 
*/

#ifndef _PDFTEMPLATEITEMBORDER_
#define _PDFTEMPLATEITEMBORDER_

#include "PdfFactory.h"
#include <libxml/tree.h>

struct PdfTemplateItemBorder
{
	float width;
	float r;
	float g;
	float b;
	short enabled;
};

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemBorder_LoadFromXml(struct PdfTemplateItemBorder *self, xmlNode *node);


#endif