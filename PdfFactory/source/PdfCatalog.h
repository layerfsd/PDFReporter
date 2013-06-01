/*
PdfCatalog.h

Author: Nebojsa Vislavski
Date: 4.7.2008.	

Used for writting catalog dictionary in pdf

*/

#ifndef _PDFCATALOG_
#define _PDFCATALOG_

#include "PdfFactory.h"
#include "PdfDocument.h"
#include "PdfBaseObject.h"

struct PdfCatalog
{
	struct PdfBaseObject base;

	struct PdfOutlines *outlines;
	struct PdfPages *pages;
};

DLLEXPORT_TEST_FUNCTION struct PdfCatalog* PdfCatalog_Create(struct PdfDocument *document);
/* create new catalog object */

DLLEXPORT_TEST_FUNCTION void PdfCatalog_Init(struct PdfCatalog *selft, struct PdfDocument *document);
/* init catalog object */


DLLEXPORT_TEST_FUNCTION void PdfCatalog_Write(struct PdfCatalog *self);
/* write catalog object to file */

DLLEXPORT_TEST_FUNCTION void PdfCatalog_Destroy(struct PdfCatalog *self);
/* destroy catalog */


#endif
