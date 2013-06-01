/*
PdfCatalog.c

Author: Nebojsa Vislavski
Date: 4.7.2008.	

Used for writting catalog dictionary in pdf

*/

#include "PdfCatalog.h"
#include "DictionaryObject.h"
#include "NameObject.h"
#include "IndirectReference.h"
#include "PdfOutlines.h"
#include "PdfPages.h"
#include "PdfBaseObject.h"
#include "MemoryManager.h"
#include <stdlib.h>

DLLEXPORT_TEST_FUNCTION struct PdfCatalog* PdfCatalog_Create(struct PdfDocument *document)
{
	struct PdfCatalog *x;
	x = (struct PdfCatalog*)MemoryManager_Alloc(sizeof(struct PdfCatalog));
	PdfCatalog_Init(x, document);
	return x;
}

DLLEXPORT_TEST_FUNCTION void PdfCatalog_Init(struct PdfCatalog *self, struct PdfDocument *document)
{
	PdfBaseObject_Init((struct PdfBaseObject*)self, document);

	self->outlines = PdfOutlines_Create(document);
	self->pages = PdfPages_Create(document);
}

/*
  writes something like this + other catalog properties
  1 0 obj 
  << 
  /Type /Catalog
  /Outlines 2 0 R
  /Pages 3 0 R
  >>
  endobj
*/
DLLEXPORT_TEST_FUNCTION void PdfCatalog_Write(struct PdfCatalog *self)
{
	struct DictionaryObject *dict;
	struct NameObject *name;
	struct IndirectReference *ref;

	
	// write Pages
	PdfPages_Write(self->pages);	
	// Write outlines
	PdfOutlines_Write(self->outlines);

	PdfDocument_BeginNewObject((struct PdfBaseObject*)self);
	{	
		dict = DictionaryObject_Begin(self->base.document->streamWriter);
		{
			DictionaryObject_WriteKey(dict, "Type");
			name = NameObject_Create(self->base.document->streamWriter, "Catalog");
			NameObject_Write(name);
			NameObject_Destroy(name);
			
			DictionaryObject_WriteKey(dict, "Outlines");
			ref = IndirectReference_Create(self->base.document->streamWriter, self->outlines->base.objectId, self->outlines->base.generationNumber);
			IndirectReference_Write(ref);
			IndirectReference_Destroy(ref);

			DictionaryObject_WriteKey(dict, "Pages");
			ref = IndirectReference_Create(self->base.document->streamWriter, self->pages->base.objectId, self->pages->base.generationNumber);
			IndirectReference_Write(ref);
			IndirectReference_Destroy(ref);
		}
		DictionaryObject_End(dict);
	}
	PdfDocument_EndObject((struct PdfBaseObject*)self);
}

DLLEXPORT_TEST_FUNCTION void PdfCatalog_Destroy(struct PdfCatalog *self)
{
	PdfPages_Destroy(self->pages);
	PdfOutlines_Destroy(self->outlines);	
	MemoryManager_Free(self);
}
