#include "RectangleNormal.h"
#include "MemoryManager.h"

DLLEXPORT_TEST_FUNCTION struct RectangleNormal* RectangleNormal_Create(float x, float y, float width, float height)
{
	struct RectangleNormal *xRect;
	xRect = (struct RectangleNormal*)MemoryManager_Alloc(sizeof(struct RectangleNormal));
	RectangleNormal_Init(xRect, x, y, width, height);
	return xRect;
}


DLLEXPORT_TEST_FUNCTION void RectangleNormal_Init(struct RectangleNormal *self, float x, float y, float width, float height)
{
	self->top = y;
	self->left = x;
	self->bottom = self->top + height;
	self->right = self->left + width;
}



DLLEXPORT_TEST_FUNCTION void RectangleNormal_Destroy(struct RectangleNormal *self)
{
	MemoryManager_Free(self);
}


DLLEXPORT_TEST_FUNCTION int RectangleNormal_Intersect(struct RectangleNormal *self, struct RectangleNormal *rect)
{
	return !((self->left - rect->right >= -0.001f)
		|| (rect->left - self->right >= -0.001f)
		|| (self->top - rect->bottom >= -0.001f)
		|| (rect->top - self->bottom >= -0.001f));

	/*return ! ( self->left >= rect->right
		|| rect->left >= self->right
		|| self->top >= rect->bottom
		|| rect->top >= self->bottom);*/
}


DLLEXPORT_TEST_FUNCTION int RectangleNormal_Contains(struct RectangleNormal *self, struct RectangleNormal *rect)
{
	return ((rect->left - self->left >= -0.001f)
		&& (self->right - rect->right >= -0.001f)
		&& (rect->top - self->top >= -0.001f)
		&& (self->bottom - rect->bottom >= -0.001f));

	/*return (rect->left >= self->left 
		&& rect->right <= self->right 
		&& rect->top >= self->top 
		&& rect->bottom <= self->bottom);*/
}



DLLEXPORT_TEST_FUNCTION void RectangleNormal_MoveTo(struct RectangleNormal *self, float locX, float locY)
{
	float w, h;
	w = self->right - self->left;
	h = self->bottom - self->top;

	self->left = locX;
	self->top = locY;

	self->right = self->left + w;
	self->bottom = self->top + h;	
}