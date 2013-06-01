#ifndef _PdfCMap_
#define _PdfCMap_


#include "PdfbaseObject.h"


struct PdfCMap
{
	struct PdfBaseObject base;
	
	int length;
};


DLLEXPORT_TEST_FUNCTION struct PdfCMap *PdfCMap_Create(struct PdfDocument *document);

DLLEXPORT_TEST_FUNCTION void PdfCMap_Init(struct PdfCMap *self, struct PdfDocument *document);

DLLEXPORT_TEST_FUNCTION void PdfCMap_Cleanup(struct PdfCMap *self);

DLLEXPORT_TEST_FUNCTION void PdfCMap_Destroy(struct PdfCMap *self);

DLLEXPORT_TEST_FUNCTION void PdfCMap_Write(struct PdfCMap *self);


#endif
