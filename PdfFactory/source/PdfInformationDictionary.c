/*
PdfInformationDictionary.c

Author: Tomislav Kukic
Date: 15.02.2010.

Used to store information about PDF document.
*/



#include "PdfInformationDictionary.h"
#include "DictionaryObject.h"
#include "NameObject.h"
#include "IndirectReference.h"
#include "PdfOutlines.h"
#include "PdfPages.h"
#include "PdfBaseObject.h"
#include "MemoryManager.h"
#include <stdlib.h>



DLLEXPORT_TEST_FUNCTION struct PdfInformationDictionary* PdfInformationDictionary_Create(struct PdfDocument *document)
{
	struct PdfInformationDictionary *x;
	x = (struct PdfInformationDictionary*)MemoryManager_Alloc(sizeof(struct PdfInformationDictionary));
	PdfInformationDictionary_Init(x, document);
	return x;
}




DLLEXPORT_TEST_FUNCTION void PdfInformationDictionary_Init(struct PdfInformationDictionary *self, struct PdfDocument *document)
{
	PdfBaseObject_Init((struct PdfBaseObject*)self, document);

	self->producer = MemoryManager_StrDup("AxiomCoders - PdfFactory (www.axiomcoders.com)");
	self->author = MemoryManager_StrDup("Unnamed");
	self->subject = MemoryManager_StrDup("AxiomCoders - PdfFactory (www.axiomcoders.com)");
	self->title = MemoryManager_StrDup("Untitled");
}




DLLEXPORT_TEST_FUNCTION void PdfInformationDictionary_Write(struct PdfInformationDictionary *self)
{
	struct DictionaryObject *dict;	
	struct StringObject *text;
	
	PdfDocument_BeginNewObject((struct PdfBaseObject*)self);
	{	
		dict = DictionaryObject_Begin(self->base.document->streamWriter);
		{
			DictionaryObject_WriteKey(dict, "Title");
			text = StringObject_Create(self->base.document->streamWriter, self->title);
			StringObject_Write(text);
			StringObject_Destroy(text);

			DictionaryObject_WriteKey(dict, "Author");
			text = StringObject_Create(self->base.document->streamWriter, self->author);
			StringObject_Write(text);
			StringObject_Destroy(text);

			DictionaryObject_WriteKey(dict, "Subject");
			text = StringObject_Create(self->base.document->streamWriter, self->subject);
			StringObject_Write(text);
			StringObject_Destroy(text);

			DictionaryObject_WriteKey(dict, "Producer");
			text = StringObject_Create(self->base.document->streamWriter, self->producer);
			StringObject_Write(text);
			StringObject_Destroy(text);
		}
		DictionaryObject_End(dict);
	}
	PdfDocument_EndObject((struct PdfBaseObject*)self);
}


DLLEXPORT_TEST_FUNCTION void PdfInformationDictionary_Cleanup(struct PdfInformationDictionary *self)
{
	if (self->author)
	{
		MemoryManager_Free(self->author); 
		self->author = NULL;
	}
	if (self->producer)
	{
		MemoryManager_Free(self->producer); 
		self->producer = NULL;
	}
	if (self->subject) 
	{
		MemoryManager_Free(self->subject); 
		self->subject = NULL;
	}
	if (self->title)
	{
		MemoryManager_Free(self->title); 
		self->title = NULL;
	}

}

DLLEXPORT_TEST_FUNCTION void PdfInformationDictionary_Destroy(struct PdfInformationDictionary *self)
{
	PdfInformationDictionary_Cleanup(self);
	MemoryManager_Free(self);
}



DLLEXPORT_TEST_FUNCTION void PdfInformationDictionary_SetParams(struct PdfInformationDictionary *self, char* title, char* author, char* subject)
{
	PdfInformationDictionary_Cleanup(self);
	self->title = MemoryManager_StrDup(title);
	self->author = MemoryManager_StrDup(author);
	self->subject = MemoryManager_StrDup(subject);
}