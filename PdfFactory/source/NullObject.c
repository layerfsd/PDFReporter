/*
NullObject.h

Author: Radovan Obradovic
Date: 3.7.2008.

Null object syntax

*/


#include "NullObject.h"
#include "MemoryManager.h"

DLLEXPORT_TEST_FUNCTION void NullObject_Write(struct NullObject *self)
{
	// NOT IMPLEMENTED YET
}

DLLEXPORT_TEST_FUNCTION struct NullObject* NullObject_Create(struct PdfDocument *document)
{
	struct NullObject *x;
	x = (struct NullObject*)MemoryManager_Alloc(sizeof(struct NullObject));
	NullObject_Init(x, document);

	return x;
}

DLLEXPORT_TEST_FUNCTION void NullObject_Init(struct NullObject *self, struct PdfDocument *document)
{
	self->document = document;
}

DLLEXPORT_TEST_FUNCTION void NullObject_Destroy(struct NullObject *self)
{
	MemoryManager_Free(self);
}
