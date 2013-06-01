/*
IndirectObject.c

Author: Nebojsa Vislavski
Date: 2.7.2008.	

Writes indirect objects
*/

#include "IndirectObject.h"
#include "MemoryManager.h"
#include <stdlib.h>

DLLEXPORT_TEST_FUNCTION struct IndirectObject* IndirectObject_Create(struct PdfDocument *document)
{
	struct IndirectObject *x;
	x = (struct IndirectObject*)MemoryManager_Alloc(sizeof(struct IndirectObject));
	// set values
	IndirectObject_Init(x, document);

	return x;
}

DLLEXPORT_TEST_FUNCTION void IndirectObject_Init(struct IndirectObject *self, struct PdfDocument *document)
{
	self->document = document;
	self->objectId = 0;
	self->generationNumber = 0;
	self->beginObjectOffset = 0;
	self->endObjectOffset = 0;
}

DLLEXPORT_TEST_FUNCTION void IndirectObject_Destroy(struct IndirectObject *self)
{	
	MemoryManager_Free(self);
}

DLLEXPORT_TEST_FUNCTION void IndirectObject_BeginNewObject(struct IndirectObject *self)
{
	char tmp[100];

	self->objectId = self->document->nextObjectId;
	self->document->nextObjectId++;
	self->document->currentObjectId++;
	self->beginObjectOffset = self->document->streamWriter->GetPosition(self->document->streamWriter);
	sprintf(tmp, "%d %d obj", self->objectId, self->generationNumber);

	self->document->streamWriter->WriteData(self->document->streamWriter, tmp);
	self->document->streamWriter->WriteNewLine(self->document->streamWriter);	
}

DLLEXPORT_TEST_FUNCTION void IndirectObject_EndObject(struct IndirectObject *self)
{
	self->document->streamWriter->WriteData(self->document->streamWriter, "endobj");	
	self->endObjectOffset = self->document->streamWriter->GetPosition(self->document->streamWriter);
	self->document->streamWriter->WriteNewLine(self->document->streamWriter);	
}
