#include "PdfBaseObject.h"

DLLEXPORT_TEST_FUNCTION void PdfBaseObject_Init(struct PdfBaseObject *self, struct PdfDocument *document)
{
	self->document = document;
	self->objectId = 0;
	self->generationNumber = 0;
	self->type = 0;
}
