#ifndef _PdfImage_
#define _PdfImage_

#include "PdfBaseObject.h"

#define DEVICE_GRAY "DeviceGray"
#define DEVICE_RGB "DeviceRGB"
#define DEVICE_CMYK "DeviceCMYK"

struct PdfImage
{
	struct PdfBaseObject base;
	int  width;
	int  height;
	int bitsPerComponent;
	int length;
	char *subType;
	char *name;	
	char *colorSpace;
	char *baseImage; // image file name from where it comes
	char *fFilter;
	char *memoryImageExtension;
};




DLLEXPORT_TEST_FUNCTION struct PdfImage *PdfImage_Create(struct PdfDocument *document, const char *baseImage);
/* Creates new pdf image object. */

DLLEXPORT_TEST_FUNCTION void PdfImage_Init(struct PdfImage *self, struct PdfDocument *document, const char *baseImage);
/* Initializes struct. */

void PdfImage_Cleanup(struct PdfImage *self);
/* Destroy PdfImage struct. */

void PdfImage_Destroy(struct PdfImage *self);
/* Destroy PdfImage struct. */

DLLEXPORT_TEST_FUNCTION int PdfImage_Write(struct PdfImage *self, int fromData, char* data, int dataSize);
/* Writes PdfImage object. */


#endif 
