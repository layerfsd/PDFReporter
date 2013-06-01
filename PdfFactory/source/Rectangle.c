#include "Rectangle.h"
#include "ArrayObject.h"
#include "NumberObject.h"
#include "MemoryManager.h"

DLLEXPORT_TEST_FUNCTION struct Rectangle* Rectangle_Create(struct StreamWriter *streamWriter, float llx, float lly, float urx, float ury)
{
	struct Rectangle *x;
	x = (struct Rectangle*)MemoryManager_Alloc(sizeof(struct Rectangle));
	Rectangle_Init(x, streamWriter, llx, lly, urx, ury);
	return x;
}


DLLEXPORT_TEST_FUNCTION void Rectangle_Init(struct Rectangle *self, struct StreamWriter *streamWriter, float llx, float lly, float urx, float ury)
{
	self->streamWriter = streamWriter;
	self->lowerLeftX = llx;
	self->lowerLeftY = lly;
	self->upperRightX = urx;
	self->upperRightY = ury;
}


DLLEXPORT_TEST_FUNCTION void Rectangle_SetWidth(struct Rectangle *self, float newWidth)
{
	self->upperRightX = self->lowerLeftX + newWidth;
}

DLLEXPORT_TEST_FUNCTION void Rectangle_SetHeight(struct Rectangle *self, float newHeight)
{
	self->upperRightY = self->lowerLeftY + newHeight;
}

DLLEXPORT_TEST_FUNCTION double Rectangle_GetWidth(struct Rectangle *self)
{
	return self->upperRightX - self->lowerLeftX;
}

DLLEXPORT_TEST_FUNCTION double Rectangle_GetHeight(struct Rectangle *self)
{
	return self->upperRightY - self->lowerLeftY;
}

DLLEXPORT_TEST_FUNCTION double Rectangle_GetTop(struct Rectangle *self)
{
	return self->upperRightY;
}

DLLEXPORT_TEST_FUNCTION double Rectangle_GetLeft(struct Rectangle *self)
{
	return self->lowerLeftX;
}



DLLEXPORT_TEST_FUNCTION void Rectangle_Write(struct Rectangle *self)
{
	struct ArrayObject *arr;
	struct NumberObject *num;
	arr = ArrayObject_BeginArray(self->streamWriter);

	num = NumberObject_Create(self->streamWriter, self->lowerLeftX);
	NumberObject_Write(num);
	NumberObject_Destroy(num);

	num = NumberObject_Create(self->streamWriter, self->lowerLeftY);
	NumberObject_Write(num);
	NumberObject_Destroy(num);

	num = NumberObject_Create(self->streamWriter, self->upperRightX);
	NumberObject_Write(num);
	NumberObject_Destroy(num);

	num = NumberObject_Create(self->streamWriter, self->upperRightY);
	NumberObject_Write(num);
	NumberObject_Destroy(num);

	ArrayObject_EndArray(arr);
}


DLLEXPORT_TEST_FUNCTION void Rectangle_Destroy(struct Rectangle *self)
{	
	MemoryManager_Free(self);
}

/************************************************************************/
/* If any of four point of rect falls inside area of self then we have intersection
/************************************************************************/
int Rectangle_Intersect(struct Rectangle *self, struct Rectangle *rect)
{	
	if (rect->lowerLeftX >= self->lowerLeftX && rect->lowerLeftX < self->upperRightX)
	{
		if (rect->lowerLeftY <= self->lowerLeftY && rect->lowerLeftY > self->upperRightY)
		{
			return TRUE;
		}
	}
	if (rect->lowerLeftX >= self->lowerLeftX && rect->lowerLeftX < self->upperRightX)
	{
		if (rect->upperRightY < self->lowerLeftY && rect->upperRightY >= self->upperRightY)
		{
			return TRUE;
		}
	}
	if (rect->upperRightX >= self->lowerLeftX && rect->upperRightX < self->upperRightX)
	{
		if (rect->lowerLeftY < self->lowerLeftY && rect->lowerLeftY >= self->upperRightY)
		{
			return TRUE;
		}
	}
	if (rect->upperRightX >= self->lowerLeftX && rect->upperRightX < self->upperRightX)
	{
		if (rect->upperRightY < self->lowerLeftY && rect->upperRightY >= self->upperRightY)
		{
			return TRUE;
		}
	}
	return FALSE;
}

/************************************************************************/
/* Location is topleft corner of rectangle
/************************************************************************/
void Rectangle_MoveTo(struct Rectangle *self, float locX, float locY)
{
	float width, height;
	width = self->upperRightX - self->lowerLeftX;
	height = self->lowerLeftY - self->upperRightY;

	self->lowerLeftX = locX;
	self->upperRightY = locY;


	self->lowerLeftY = self->upperRightY + height;
	self->upperRightX = self->lowerLeftX + width;
}

int Rectangle_Contains(struct Rectangle *self, struct Rectangle *rect)
{
	// if rect X falls inside x range of this
	int res = TRUE;
	// check lx,ly are outside
	if (rect->lowerLeftX < self->lowerLeftX || rect->lowerLeftX > self->upperRightX)
	{
		res = FALSE;
	}
	if (rect->lowerLeftY > self->lowerLeftY || rect->lowerLeftY < self->upperRightY)
	{
		res = FALSE;
	}
	if (rect->upperRightX < self->lowerLeftX || rect->upperRightX > self->upperRightX)
	{
		res = FALSE;
	}
	if (rect->upperRightY > self->lowerLeftY || rect->upperRightY < self->upperRightY)
	{
		res = FALSE;
	}	

	return res;
}
