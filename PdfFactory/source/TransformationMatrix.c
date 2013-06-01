#include "TransformationMatrix.h"
#include "MemoryManager.h"
#include <math.h>

DLLEXPORT_TEST_FUNCTION struct TransformationMatrix* TransformationMatrix_Create()
{
	struct TransformationMatrix *x = MemoryManager_Alloc(sizeof(struct TransformationMatrix));
	TransformationMatrix_Identity(x);
	return x;
}

DLLEXPORT_TEST_FUNCTION void TransformationMatrix_Init(struct TransformationMatrix *self, float a, float b, float c, float d, float e, float f)
{
	self->a = a;
	self->b = b;
	self->c = c;
	self->d = d;
	self->e = e;
	self->f = f;
}

DLLEXPORT_TEST_FUNCTION void TransformationMatrix_Rotate(struct TransformationMatrix *self, float angle)
{
	float cosAngle;
	float sinAngle;

	angle = (3.14159f * angle) / 180.0f;
	cosAngle = cosf(angle);
	sinAngle = sinf(angle);
	TransformationMatrix_Identity(self);
	self->a = cosAngle;
	self->b = sinAngle;
	self->c = -sinAngle;
	self->d = cosAngle;
}

DLLEXPORT_TEST_FUNCTION void TransformationMatrix_Scale(struct TransformationMatrix *self, float width, float height)
{
	TransformationMatrix_Identity(self);
	self->a = width;
	self->d = height;
}

DLLEXPORT_TEST_FUNCTION void TransformationMatrix_Translate(struct TransformationMatrix *self, float posX, float posY)
{
	TransformationMatrix_Identity(self);
	self->e = posX;
	self->f = posY;
}

DLLEXPORT_TEST_FUNCTION void TransformationMatrix_Destroy(struct TransformationMatrix *self)
{
	MemoryManager_Free(self);
}

DLLEXPORT_TEST_FUNCTION void TransformationMatrix_Identity(struct TransformationMatrix *self)
{
	self->a = 1;
	self->b = 0;
	self->c = 0;
	self->d = 1;
	self->e = 0;
	self->f = 0;
}

DLLEXPORT_TEST_FUNCTION void TransformationMatrix_Multiply(struct TransformationMatrix *matA, struct TransformationMatrix *matB, struct TransformationMatrix *result)
{
	result->a = matA->a * matB->a + matA->b*matB->c;
	result->b = matA->a * matB->b + matA->b*matB->d;
	result->c = matA->c * matB->a + matA->d*matB->c;
	result->d = matA->c * matB->b + matA->d*matB->d;
	result->e = matA->e * matB->a + matA->f*matB->c + matB->e;
	result->f = matA->e * matB->b + matA->f*matB->d + matB->f;
}

