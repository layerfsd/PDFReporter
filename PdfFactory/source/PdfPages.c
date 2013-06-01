#include "PdfPages.h"
#include "DictionaryObject.h"
#include "NameObject.h"
#include "NumberObject.h"
#include "ArrayObject.h"
#include "PdfBaseObject.h"
#include "DLList.h"
#include "IndirectReference.h"
#include "PdfPage.h"
#include "MemoryManager.h"

DLLEXPORT_TEST_FUNCTION struct PdfPages* PdfPages_Create(struct PdfDocument *document)
{
	struct PdfPages *x;
	x = (struct PdfPages*)MemoryManager_Alloc(sizeof(struct PdfPages));
	PdfPages_Init(x, document);	
	return x;
}

DLLEXPORT_TEST_FUNCTION void PdfPages_Init(struct PdfPages *self, struct PdfDocument *document)
{
	PdfBaseObject_Init((struct PdfBaseObject*)self, document);

	self->pages = DLList_Create();
	self->kidsCount = 0;
}

/*
  Writes something like this
  20 0 obj
  << 
  /Type /Pages
  /Kids [23 0 R 24 0 R]
  /Count 2
  >>
  endobj
*/
DLLEXPORT_TEST_FUNCTION void PdfPages_Write(struct PdfPages *self)
{
	struct DictionaryObject *dict;
	struct NameObject *name;
	struct NumberObject *num;
	struct DLListNode *currentObject;
	struct PdfPage *page;
	struct IndirectReference *ref;
	struct ArrayObject *arr;
	int tmpLoc;

	PdfDocument_BeginNewObject((struct PdfBaseObject*)self);
	{		
		dict = DictionaryObject_Begin(self->base.document->streamWriter);
		{
			DictionaryObject_WriteKey(dict, "Type");
			name = NameObject_Create(self->base.document->streamWriter, "Pages");
			NameObject_Write(name);
			NameObject_Destroy(name);
			
			// add /Kids
			DictionaryObject_WriteKey(dict, "Kids");
			arr = ArrayObject_BeginArray(self->base.document->streamWriter);
			{			
				currentObject = DLList_Begin(self->pages);
				while (currentObject != DLList_End(self->pages))
				{
					// write object offset, generation number and n
					page = (struct PdfPage *)currentObject->data;
					ref = IndirectReference_Create(self->base.document->streamWriter, page->base.objectId, page->base.generationNumber);
					IndirectReference_Write(ref);
					IndirectReference_Destroy(ref);

					// write parent reference to page part
					tmpLoc = self->base.document->streamWriter->GetPosition(self->base.document->streamWriter);					
					self->base.document->streamWriter->Seek(self->base.document->streamWriter, page->parentOffsetPlace);
					
					ref = IndirectReference_Create(self->base.document->streamWriter, self->base.objectId, self->base.generationNumber);
					IndirectReference_Write(ref);
					IndirectReference_Destroy(ref);

					self->base.document->streamWriter->Seek(self->base.document->streamWriter, tmpLoc);					
					currentObject = currentObject->next;
				}
			}
			ArrayObject_EndArray(arr);


			DictionaryObject_WriteKey(dict, "Count");
			num = NumberObject_Create(self->base.document->streamWriter, self->pages->size);
			NumberObject_Write(num);
			NumberObject_Destroy(num);		
		}
		DictionaryObject_End(dict);
	}
	PdfDocument_EndObject((struct PdfBaseObject*)self);
}


DLLEXPORT_TEST_FUNCTION void PdfPages_Destroy(struct PdfPages *self)
{
	// destroy all pages
	while(self->pages->size > 0)
	{
		struct PdfPage *obj;
		obj = (struct PdfPage *)DLList_Back(self->pages);
		DLList_PopBack(self->pages);
		PdfPage_Destroy(obj);
	}
	DLList_Destroy(self->pages);
	
	MemoryManager_Free(self);
}
