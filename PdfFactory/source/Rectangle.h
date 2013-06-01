#ifndef _RECTANGLE_
#define _RECTANGLE_

#include "PdfFactory.h"
#include "StreamWriter.h"

struct Rectangle
{
	struct StreamWriter *streamWriter;
	float lowerLeftX;
	float lowerLeftY;
	float upperRightX;
	float upperRightY;	
	// TODO: 
	// add some measures information so it can support milimeters, inches, cm, ... 
};

DLLEXPORT_TEST_FUNCTION struct Rectangle* Rectangle_Create(struct StreamWriter *streamWriter, float llx, float lly, float urx, float ury);
/* Create PdfPage object. */

DLLEXPORT_TEST_FUNCTION void Rectangle_Init(struct Rectangle *self, struct StreamWriter *streamWriter, float llx, float lly, float urx, float ury);
/* Initializes PdfPage struct. */

DLLEXPORT_TEST_FUNCTION void Rectangle_Write(struct Rectangle *self);
/* Write PdfPage object to file. */

DLLEXPORT_TEST_FUNCTION void Rectangle_Destroy(struct Rectangle *self);
/* Destroy PdfPage struct. */

//int Rectangle_Intersect(struct Rectangle *self, struct Rectangle *rect);
/* Return true if two rectangles intersect */

//int Rectangle_Contains(struct Rectangle *self, struct Rectangle *rect);
/* Return true if self completely contains rect  */

/*DLLEXPORT_TEST_FUNCTION void Rectangle_SetWidth(struct Rectangle *self, double newWidth);
DLLEXPORT_TEST_FUNCTION void Rectangle_SetHeight(struct Rectangle *self, double newHeight);
DLLEXPORT_TEST_FUNCTION double Rectangle_GetWidth(struct Rectangle *self);
DLLEXPORT_TEST_FUNCTION double Rectangle_GetHeight(struct Rectangle *self);
DLLEXPORT_TEST_FUNCTION double Rectangle_GetTop(struct Rectangle *self);
DLLEXPORT_TEST_FUNCTION double Rectangle_GetLeft(struct Rectangle *self);

void Rectangle_MoveTo(struct Rectangle *self, double locX, double locY);*/

#endif
