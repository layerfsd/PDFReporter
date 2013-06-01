#include "PdfPage.h"
#include "PdfBaseObject.h"
#include "NameObject.h"
#include "DictionaryObject.h"
#include "IndirectReference.h"
#include "PdfContentStream.h"
#include "PdfPageResources.h"
#include "MemoryManager.h"
#include <stdlib.h>

DLLEXPORT_TEST_FUNCTION struct PdfPage* PdfPage_Create(struct PdfDocument *document, struct PdfPageProperties *properties)
{
	struct PdfPage *x;
	x = (struct PdfPage*)MemoryManager_Alloc(sizeof(struct PdfPage));
	PdfPage_Init(x, document, properties);	
	return x;
}

DLLEXPORT_TEST_FUNCTION void PdfPage_Init(struct PdfPage *self, struct PdfDocument *document, struct PdfPageProperties *properties)
{
	PdfBaseObject_Init((struct PdfBaseObject*)self, document);
	self->parent = 0;
	self->parentOffsetPlace = 0;
	self->properties = *properties;
	self->properties.mediaBox.streamWriter = document->streamWriter;
	self->userRect.lowerLeftX = properties->mediaBox.lowerLeftX;
	self->userRect.upperRightX = properties->mediaBox.upperRightX;
	self->userRect.lowerLeftY = properties->mediaBox.upperRightY;	
	self->userRect.upperRightY = properties->mediaBox.lowerLeftY;

}

/*
 write something like this
 12 0 obj
 << 
 /Type Page
 /Parent 2 0 R
 /Content 10 0 R
 /MediaBox [0 0 10 10]
 /Resources 
    <<
	  /ProcSet 6 0 R 
	>>
 >>
 endobj
*/
DLLEXPORT_TEST_FUNCTION void PdfPage_Write(struct PdfPage *self)
{
	struct DictionaryObject *dict;
	struct NameObject *name;
	struct IndirectReference *ref;
	
	char tmp[16];
	int i;

	for(i = 0; i < 15; i++)
	{
		tmp[i] = ' ';
	}
	tmp[15] = 0;

	PdfDocument_BeginNewObject((struct PdfBaseObject*)self);
	{
		dict = DictionaryObject_Begin(self->base.document->streamWriter);
		{
			// TODO: other page properties should be written here. And usage of properties should be replaced by something better

			DictionaryObject_WriteKey(dict, "Type");
			name = NameObject_Create(self->base.document->streamWriter, "Page");
			NameObject_Write(name);
			NameObject_Destroy(name);

			DictionaryObject_WriteKey(dict, "Parent");
			self->parentOffsetPlace = self->base.document->streamWriter->GetPosition(self->base.document->streamWriter);			
			self->base.document->streamWriter->WriteData(self->base.document->streamWriter, tmp);			

			if (self->properties.contentStream)
			{			
				DictionaryObject_WriteKey(dict, "Contents");
				ref = IndirectReference_Create(self->base.document->streamWriter, self->properties.contentStream->base.objectId, self->properties.contentStream->base.generationNumber);
				IndirectReference_Write(ref);
				IndirectReference_Destroy(ref);
			}

			DictionaryObject_WriteKey(dict, "MediaBox");
			Rectangle_Write(&self->properties.mediaBox);

			if (self->properties.resources)
			{			
				DictionaryObject_WriteKey(dict, "Resources");				
				ref = IndirectReference_Create(self->base.document->streamWriter, self->properties.resources->base.objectId, self->properties.resources->base.generationNumber);
				IndirectReference_Write(ref);
				IndirectReference_Destroy(ref);
			}
		}
		DictionaryObject_End(dict);
	}
	PdfDocument_EndObject((struct PdfBaseObject*)self);
}


DLLEXPORT_TEST_FUNCTION void PdfPage_Destroy(struct PdfPage *self)
{	
	MemoryManager_Free(self);
}


