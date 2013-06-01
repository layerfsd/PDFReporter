#ifndef _PDFPAGERESOURCES_
#define _PDFPAGERESOURCES_

#include "PdfBaseObject.h"
#include "PdfFactory.h"
#include "PdfDocument.h"

struct PdfFont;

// Describes resources of page.
struct PdfPageResources
{
	struct PdfBaseObject base;
	struct DLList *fonts; // page font resources
	struct DLList *images; // page font resources
	struct DLList *shadings; // page shading resources
};

DLLEXPORT_TEST_FUNCTION struct PdfPageResources* PdfPageResources_Create(struct PdfDocument *document);
/* Create PdfPageResources object. */

DLLEXPORT_TEST_FUNCTION void PdfPageResources_Init(struct PdfPageResources *self, struct PdfDocument *document);
/* Initializes PdfPageResources struct. */

void PdfPageResources_Cleanup(struct PdfPageResources *self);
/* Cleanup PdfPageResources struct. */

void PdfPageResources_Destroy(struct PdfPageResources *self);
/* Destroy PdfPageResources struct. */

DLLEXPORT_TEST_FUNCTION void PdfPageResources_AddFont(struct PdfPageResources *self, struct PdfFont *font);
/* Add font to PdfPageResources object. */

DLLEXPORT_TEST_FUNCTION void PdfPageResources_AddImage(struct PdfPageResources *self, struct PdfImage *image);
/* Add Image to PdfPageResources object. */

DLLEXPORT_TEST_FUNCTION void PdfPageResources_AddShadingDictionary(struct PdfPageResources *self, struct PdfShadingDictionary *shDict);
/* Add Shad. Dict. to PdfPageResources object. */

DLLEXPORT_TEST_FUNCTION void PdfPageResources_Write(struct PdfPageResources *self);
/* Write PdfPageResources object to file. */

#endif
