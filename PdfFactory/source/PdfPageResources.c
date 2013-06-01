#include "PdfPage.h"
#include "PdfBaseObject.h"
#include "NameObject.h"
#include "DictionaryObject.h"
#include "IndirectReference.h"
#include "PdfContentStream.h"
#include "PdfPageResources.h"
#include "PdfShadingDictionary.h"
#include "PdfFont.h"
#include "PdfImage.h"
#include "DLList.h"
#include "MemoryManager.h"
#include <stdlib.h>

DLLEXPORT_TEST_FUNCTION struct PdfPageResources* PdfPageResources_Create(struct PdfDocument *document)
{
	struct PdfPageResources *x;
	x = (struct PdfPageResources*)MemoryManager_Alloc(sizeof(struct PdfPageResources));
	PdfPageResources_Init(x, document);
	return x;
}

DLLEXPORT_TEST_FUNCTION void PdfPageResources_Init(struct PdfPageResources *self, struct PdfDocument *document)
{
	PdfBaseObject_Init(&self->base, document);
	DLList_PushBack(document->resources, self); // automatically add self to list of resources in document
	self->fonts = DLList_Create();
	self->images = DLList_Create();
	self->shadings = DLList_Create();
}

void PdfPageResources_Cleanup(struct PdfPageResources *self)
{	
	DLList_Destroy(self->fonts); // destroy list itself. Fonts will be removed by PdfDocument
	DLList_Destroy(self->images); // destroy list itself. Images will be removed by PdfDocument
	DLList_Destroy(self->shadings); // destroy list itself. Images will be removed by PdfDocument
}

void PdfPageResources_Destroy(struct PdfPageResources *self)
{
	PdfPageResources_Cleanup(self);	
	MemoryManager_Free(self);
}

DLLEXPORT_TEST_FUNCTION void PdfPageResources_AddFont(struct PdfPageResources *self, struct PdfFont *font)
{
	// TODO: check if font exists
	struct DLListNode *iterator;
	struct PdfFont *im;
	int exists = 0;

	for(iterator = DLList_Begin(self->fonts); iterator != DLList_End(self->fonts); iterator = iterator->next)
	{
		im = (struct PdfFont *)iterator->data;
		if (im == font)
		{
			exists = 1;
		}
	}
	if (!exists)
	{
		DLList_PushBack(self->fonts, font);
	}		
}

DLLEXPORT_TEST_FUNCTION void PdfPageResources_AddImage(struct PdfPageResources *self, struct PdfImage *image)
{
	// TODO: check if font exists
	struct DLListNode *iterator;
	struct PdfImage *im;
	int exists = 0;

	for(iterator = DLList_Begin(self->images); iterator != DLList_End(self->images); iterator = iterator->next)
	{
		im = (struct PdfImage *)iterator->data;
		if (im == image)
		{
			exists = 1;
		}
	}
	if (!exists)
	{
		DLList_PushBack(self->images, image);
	}	
}


DLLEXPORT_TEST_FUNCTION void PdfPageResources_AddShadingDictionary(struct PdfPageResources *self, struct PdfShadingDictionary *shDict)
{
	// TODO: check if font exists
	struct DLListNode *iterator;
	struct PdfShadingDictionary *shDictionary;
	int exists = 0;

	for(iterator = DLList_Begin(self->shadings); iterator != DLList_End(self->shadings); iterator = iterator->next)
	{
		shDictionary = (struct PdfShadingDictionary *)iterator->data;
		if (shDictionary == shDict)
		{
			exists = 1;
		}
	}
	if (!exists)
	{
		DLList_PushBack(self->shadings, shDict);
	}	
}





DLLEXPORT_TEST_FUNCTION void PdfPageResources_Write(struct PdfPageResources *self)
{
	struct DictionaryObject *dict, *fontsDict, *imagesDict, *shadingDict;
	struct DLListNode *it;
	struct IndirectReference *ref;

	PdfDocument_BeginNewObject((struct PdfBaseObject*)self);
	{	
		dict = DictionaryObject_Begin(self->base.document->streamWriter);
		
		DictionaryObject_WriteKey(dict, "Font");
		fontsDict = DictionaryObject_Begin(self->base.document->streamWriter);
		{
			for (it = DLList_Begin(self->fonts); it != DLList_End(self->fonts); it = it->next)
			{
				struct PdfFont *font = (struct PdfFont *)it->data;
				DictionaryObject_WriteKey(dict, font->name);
				ref = IndirectReference_Create(self->base.document->streamWriter, font->base.objectId, font->base.generationNumber);
				IndirectReference_Write(ref);
				IndirectReference_Destroy(ref);
			}
		}
		DictionaryObject_End(fontsDict);
		
		
		if (self->shadings->size > 0 || self->images->size > 0)
		{		
			//=================== Write image =========================================
			DictionaryObject_WriteKey(dict, "XObject");			
			imagesDict = DictionaryObject_Begin(self->base.document->streamWriter);
			{
				for (it = DLList_Begin(self->images); it != DLList_End(self->images); it = it->next)
				{
					struct PdfImage *image = (struct PdfImage *)it->data;
					DictionaryObject_WriteKey(dict, image->name);
					ref = IndirectReference_Create(self->base.document->streamWriter, image->base.objectId, image->base.generationNumber);
					IndirectReference_Write(ref);
					IndirectReference_Destroy(ref);
				}
			}
			DictionaryObject_End(imagesDict);
			//=========================================================================			

			DictionaryObject_WriteKey(dict, "Shading");
			shadingDict = DictionaryObject_Begin(self->base.document->streamWriter);
			{
				for (it = DLList_Begin(self->shadings); it != DLList_End(self->shadings); it = it->next)
				{
					struct PdfShadingDictionary *shDict = (struct PdfShadingDictionary *)it->data;
					DictionaryObject_WriteKey(dict, shDict->name);
					ref = IndirectReference_Create(self->base.document->streamWriter, shDict->base.objectId, shDict->base.generationNumber);
					IndirectReference_Write(ref);
					IndirectReference_Destroy(ref);
				}
			}
			DictionaryObject_End(shadingDict);
		}

		DictionaryObject_End(dict);
	}
	PdfDocument_EndObject((struct PdfBaseObject*)self);
}
