/*
PdfInformationDictionary.h

Author: Tomislav Kukic
Date: 15.02.2010.

Used to store information about PDF document.
*/



#ifndef _PDF_PDFINFORMATIONDICTIONARY_
#define _PDF_PDFINFORMATIONDICTIONARY_


#include "PdfFactory.h"
#include "PdfDocument.h"
#include "PdfBaseObject.h"


struct PdfInformationDictionary
{
	struct PdfBaseObject base;

	struct PdfDocument *document;

	char *title;		//Title of the document
	char *author;		//Author of the document
	char *subject;		//Subject of the document 
	char *producer;		//Application that is produced PDF document, this is always "AxiomCoders - PdfFactory (www.axiomcoders.com)"
};


DLLEXPORT_TEST_FUNCTION struct PdfInformationDictionary* PdfInformationDictionary_Create(struct PdfDocument *document);
DLLEXPORT_TEST_FUNCTION void PdfInformationDictionary_Init(struct PdfInformationDictionary *self, struct PdfDocument *document);
DLLEXPORT_TEST_FUNCTION void PdfInformationDictionary_Write(struct PdfInformationDictionary *self);
DLLEXPORT_TEST_FUNCTION void PdfInformationDictionary_Destroy(struct PdfInformationDictionary *self);
DLLEXPORT_TEST_FUNCTION void PdfInformationDictionary_Cleanup(struct PdfInformationDictionary *self);

DLLEXPORT_TEST_FUNCTION void PdfInformationDictionary_SetParams(struct PdfInformationDictionary *self, char* title, char* author, char* subject);


#endif
