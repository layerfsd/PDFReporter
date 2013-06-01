/* 
PdfTemplateInformationDictionary.h

Author: Tomislav Kukic
Date: 20.2.2010.	

Used for reading information dictionary from template file
*/

#ifndef _PDFTEMPLATEINFORMATIONDICTIONARY_
#define _PDFTEMPLATEINFORMATIONDICTIONARY_

#include "PdfFactory.h"
#include <libxml/tree.h>

struct PdfTemplateInformationDictionary
{	
	char *Title;
	char *Author;
	char *Subject;
	char *Producer;
};

DLLEXPORT_TEST_FUNCTION void PdfTemplateInformationDictionary_Write();

DLLEXPORT_TEST_FUNCTION struct PdfTemplateInformationDictionary* PdfTemplateInformationDictionary_Create();

DLLEXPORT_TEST_FUNCTION struct PdfTemplateInformationDictionary* PdfTemplateInformationDictionary_CreateFromXml(xmlNode *node);

DLLEXPORT_TEST_FUNCTION void PdfTemplateInformationDictionary_Init(struct PdfTemplateInformationDictionary* self, char *Title, char *Author, char *Subject, char *Producer);

DLLEXPORT_TEST_FUNCTION void PdfTemplateInformationDictionary_Destroy(struct PdfTemplateInformationDictionary* self);

DLLEXPORT_TEST_FUNCTION void PdfTemplateInformationDictionary_Cleanup(struct PdfTemplateInformationDictionary* self);


#endif