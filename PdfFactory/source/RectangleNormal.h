#ifndef _RECTANGLE_NORMAL_
#define _RECTANGLE_NORMAL_

#include "PdfFactory.h"


// This is rectangle in top-left coordinate system. Where origin is topmost,leftmost coordinate
struct RectangleNormal
{
	float top;
	float left;
	float right;
	float bottom;
};

DLLEXPORT_TEST_FUNCTION struct RectangleNormal* RectangleNormal_Create(float x, float y, float width, float height);
/* Create PdfPage object. */

DLLEXPORT_TEST_FUNCTION void RectangleNormal_Init(struct RectangleNormal *self, float x, float y, float width, float height);
/* Initializes PdfPage struct. */


DLLEXPORT_TEST_FUNCTION void RectangleNormal_Destroy(struct RectangleNormal *self);
/* Destroy PdfPage struct. */

DLLEXPORT_TEST_FUNCTION int RectangleNormal_Intersect(struct RectangleNormal *self, struct RectangleNormal *rect);
/* Return true if two rectangles intersect */

DLLEXPORT_TEST_FUNCTION int RectangleNormal_Contains(struct RectangleNormal *self, struct RectangleNormal *rect);
/* Return true if self completely contains rect  */

DLLEXPORT_TEST_FUNCTION void RectangleNormal_MoveTo(struct RectangleNormal *self, float locX, float locY);

#endif
