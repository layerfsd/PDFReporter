#include "PdfGeneratorDataStreamColumn.h"
#include "MemoryManager.h"
#include <stdlib.h>

struct PdfGeneratorDataStreamColumn* PdfGeneratorDataStreamColumn_Create(int type, int length, char *name)
{
	struct PdfGeneratorDataStreamColumn *x;
	x = (struct PdfGeneratorDataStreamColumn*)MemoryManager_Alloc(sizeof(struct PdfGeneratorDataStreamColumn));
	PdfGeneratorDataStreamColumn_Init(x, type, length, name);
	return x;
}

void PdfGeneratorDataStreamColumn_Init(struct PdfGeneratorDataStreamColumn *self, int type, int length, char *name)
{
	self->length = length;
	self->type = type;
	strcpy(self->name, name);
}

void PdfGeneratorDataStreamColumn_Destroy(struct PdfGeneratorDataStreamColumn *self)
{
	MemoryManager_Free(self);
}

