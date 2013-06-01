/*
PdfTemplateColumn.h

Author: Marko Vranjkovic
Date: 30.07.2008.	

*/

#ifndef _PDFTEMPLATECOLUMN_
#define _PDFTEMPLATECOLUMN_

#include "PdfFactory.h"
#include <libxml/tree.h>

struct PdfTemplateColumn
{
	char* name;
	char* type;	
};

DLLEXPORT_TEST_FUNCTION void PdfTemplateColumn_Write(struct PdfTemplateColumn* self);

DLLEXPORT_TEST_FUNCTION struct PdfTemplateColumn* PdfTemplateColumn_Create(char *name, char *type);
DLLEXPORT_TEST_FUNCTION struct PdfTemplateColumn* PdfTemplateColumn_CreateFromXml(xmlNode *node);

DLLEXPORT_TEST_FUNCTION void PdfTemplateColumn_Init(struct PdfTemplateColumn* self, char* name, char* type);

DLLEXPORT_TEST_FUNCTION void PdfTemplateColumn_Destroy(struct PdfTemplateColumn* self);

DLLEXPORT_TEST_FUNCTION void PdfTemplateColumn_Cleanup(struct PdfTemplateColumn* self);

#endif