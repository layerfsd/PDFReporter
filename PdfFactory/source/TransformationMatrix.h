#ifndef _TRANSMATRIX_H
#define _TRANSMATRIX_H

#include "PdfFactory.h"

struct TransformationMatrix
{
	float a;
	float b;
	float c;
	float d;
	float e;
	float f;
};

DLLEXPORT_TEST_FUNCTION struct TransformationMatrix* TransformationMatrix_Create();
DLLEXPORT_TEST_FUNCTION void TransformationMatrix_Init(struct TransformationMatrix *self, float a, float b, float c, float d, float e, float f);
DLLEXPORT_TEST_FUNCTION void TransformationMatrix_Rotate(struct TransformationMatrix *self, float angle);
DLLEXPORT_TEST_FUNCTION void TransformationMatrix_Scale(struct TransformationMatrix *self, float width, float height);
DLLEXPORT_TEST_FUNCTION void TransformationMatrix_Translate(struct TransformationMatrix *self, float posX, float posY);
DLLEXPORT_TEST_FUNCTION void TransformationMatrix_Destroy(struct TransformationMatrix *self);

DLLEXPORT_TEST_FUNCTION void TransformationMatrix_Identity(struct TransformationMatrix *self);
DLLEXPORT_TEST_FUNCTION void TransformationMatrix_Multiply(struct TransformationMatrix *a, struct TransformationMatrix *b, struct TransformationMatrix *result);
/* Multiply matrices  */




#endif 