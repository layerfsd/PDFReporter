#ifndef _PdfCIDSystemInfo_
#define _PdfCIDSystemInfo_


#include "PdfBaseObject.h"


struct PdfCIDSystemInfo
{
	struct PdfBaseObject base;

	int supplement;
	char *ordering;
	char *registry;
};


DLLEXPORT_TEST_FUNCTION struct PdfCIDSystemInfo *PdfCIDSystemInfo_Create(struct PdfDocument *document);

DLLEXPORT_TEST_FUNCTION void PdfCIDSystemInfo_Init(struct PdfCIDSystemInfo *self, struct PdfDocument *document, int supplement, char *ordering, char *registry);

DLLEXPORT_TEST_FUNCTION void PdfCIDSystemInfo_Cleanup(struct PdfCIDSystemInfo *self);

DLLEXPORT_TEST_FUNCTION void PdfCIDSystemInfo_Destroy(struct PdfCIDSystemInfo *self);

DLLEXPORT_TEST_FUNCTION void PdfCIDSystemInfo_Write(struct PdfCIDSystemInfo *self);


#endif