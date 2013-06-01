#ifndef _PDFPAGE_
#define _PDFPAGE_

#include "PdfFactory.h"
#include "PdfDocument.h"
#include "PdfBaseObject.h"
#include "Rectangle.h"


// Describes properties of page. Needed to create page
struct PdfPageProperties
{
	struct PdfContentStream *contentStream;  // content steam already written
	struct PdfPageResources *resources; // resources that are written 
	struct Rectangle mediaBox;  // size of page
};

struct PdfPage
{
	struct PdfBaseObject base;

	struct PdfPages *parent; // parent page	
	int parentOffsetPlace; // place where parent reference should be put
	struct PdfPageProperties properties; // other properties
	struct Rectangle userRect; // this is in monitor coordinate system, y goes down, x goes right. It is autoinitialized on creation
};

DLLEXPORT_TEST_FUNCTION struct PdfPage* PdfPage_Create(struct PdfDocument *document, struct PdfPageProperties *properties);
/* Create PdfPage object. */

DLLEXPORT_TEST_FUNCTION void PdfPage_Init(struct PdfPage *self, struct PdfDocument *document, struct PdfPageProperties *properties);
/* Initializes PdfPage struct. */

DLLEXPORT_TEST_FUNCTION void PdfPage_Write(struct PdfPage *self);
/* Write PdfPage object to file. */

DLLEXPORT_TEST_FUNCTION void PdfPage_Destroy(struct PdfPage *self);
/* Destroy PdfPage struct. */


#endif
