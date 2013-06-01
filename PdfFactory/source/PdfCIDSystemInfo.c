#include "PdfCIDSystemInfo.h"
#include "PdfDocument.h"
#include "StringObject.h"
#include "NumberObject.h"
#include "MemoryManager.h"
#include "DictionaryObject.h"


DLLEXPORT_TEST_FUNCTION struct PdfCIDSystemInfo *PdfCIDSystemInfo_Create(struct PdfDocument *document)
{
	struct PdfCIDSystemInfo *x;
	x = (struct PdfCIDSystemInfo*)MemoryManager_Alloc(sizeof(struct PdfCIDSystemInfo));
	PdfCIDSystemInfo_Init(x, document, 0, "Identity", "Adobe");
	return x;
}

DLLEXPORT_TEST_FUNCTION void PdfCIDSystemInfo_Init(struct PdfCIDSystemInfo *self, struct PdfDocument *document, int supplement, char *ordering, char *registry)
{
	PdfBaseObject_Init(&self->base, document);
	self->supplement = supplement;
	self->ordering = MemoryManager_StrDup(ordering);
	self->registry = MemoryManager_StrDup(registry);
}

DLLEXPORT_TEST_FUNCTION void PdfCIDSystemInfo_Cleanup(struct PdfCIDSystemInfo *self)
{
	if(self->ordering)
	{
		MemoryManager_Free(self->ordering);
		self->ordering = 0;
	}

	if(self->registry)
	{
		MemoryManager_Free(self->registry);
		self->registry = 0;
	}
}

DLLEXPORT_TEST_FUNCTION void PdfCIDSystemInfo_Destroy(struct PdfCIDSystemInfo *self)
{
	PdfCIDSystemInfo_Cleanup(self);
	MemoryManager_Free(self);
}

DLLEXPORT_TEST_FUNCTION void PdfCIDSystemInfo_Write(struct PdfCIDSystemInfo *self)
{
	struct DictionaryObject *dict;
	struct StringObject *tmpStr;
	struct NumberObject *number;
	

	PdfDocument_BeginNewObject((struct PdfBaseObject*)self);
dict = DictionaryObject_Begin(self->base.document->streamWriter);

	DictionaryObject_WriteKey(dict, "Supplement");
	number = NumberObject_Create(self->base.document->streamWriter, self->supplement);
	NumberObject_Write(number);
	NumberObject_Destroy(number);

	DictionaryObject_WriteKey(dict, "Ordering");
	tmpStr = StringObject_Create(self->base.document->streamWriter, self->ordering);
	StringObject_Write(tmpStr);
	StringObject_Destroy(tmpStr);

	DictionaryObject_WriteKey(dict, "Registry");
	tmpStr = StringObject_Create(self->base.document->streamWriter, self->registry);
	StringObject_Write(tmpStr);
	StringObject_Destroy(tmpStr);

	DictionaryObject_End(dict);
	PdfDocument_EndObject((struct PdfBaseObject*)self);
}