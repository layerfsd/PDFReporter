#include "PdfGeneratorDataStreamField.h"
#include "DLList.h"
#include "MemoryManager.h"
#include <stdlib.h>

struct PdfGeneratorDataStreamField *PdfGeneratorDataStreamField_Create(struct PdfGeneratorDataStreamColumn *column, void *value)
{
	struct PdfGeneratorDataStreamField *x;
	x = (struct PdfGeneratorDataStreamField *)MemoryManager_Alloc(sizeof(struct PdfGeneratorDataStreamField));
	PdfGeneratorDataStreamField_Init(x, column, value);	
	return x;
}

void PdfGeneratorDataStreamField_Init(struct PdfGeneratorDataStreamField *self, struct PdfGeneratorDataStreamColumn *column, void *value)
{
	self->value = value;
	self->column = column; 
}

void PdfGeneratorDataStreamField_Destroy(struct PdfGeneratorDataStreamField *self)
{
	MemoryManager_Free(self);
}
