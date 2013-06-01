#include "PdfOutlines.h"
#include "DictionaryObject.h"
#include "NameObject.h"
#include "NumberObject.h"
#include "ArrayObject.h"
#include "MemoryManager.h"

DLLEXPORT_TEST_FUNCTION struct PdfOutlines* PdfOutlines_Create(struct PdfDocument *document)
{
	struct PdfOutlines *x;
	x = (struct PdfOutlines*)MemoryManager_Alloc(sizeof(struct PdfOutlines));
	PdfOutlines_Init(x, document);	
	return x;
}

DLLEXPORT_TEST_FUNCTION void PdfOutlines_Init(struct PdfOutlines *self, struct PdfDocument *document)
{
	PdfBaseObject_Init((struct PdfBaseObject*)self, document);
	self->count = 0;
}

/*
  Write something similar to this
	2 0 obj
	<< 
	/Type /Outlines
	/Count 0
	>>
	endobj
*/
DLLEXPORT_TEST_FUNCTION void PdfOutlines_Write(struct PdfOutlines *self)
{
	struct DictionaryObject *dict;
	struct NameObject *name;
	struct NumberObject *num;
	
	PdfDocument_BeginNewObject((struct PdfBaseObject*)self);
	{			

		dict = DictionaryObject_Begin(self->base.document->streamWriter);
		{
			DictionaryObject_WriteKey(dict, "Type");
			name = NameObject_Create(self->base.document->streamWriter, "Outlines");
			NameObject_Write(name);
			NameObject_Destroy(name);

			DictionaryObject_WriteKey(dict, "Count");
			num = NumberObject_Create(self->base.document->streamWriter, 0);
			NumberObject_Write(num);
			NumberObject_Destroy(num);		
		}
		DictionaryObject_End(dict);
	}
	PdfDocument_EndObject((struct PdfBaseObject*)self);
}


DLLEXPORT_TEST_FUNCTION void PdfOutlines_Destroy(struct PdfOutlines *self)
{	
	MemoryManager_Free(self);
}
