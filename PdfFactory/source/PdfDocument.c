/*
PdfDocument.c

Author: Nebojsa Vislavski
Date: 30.6.2008.	

This functions are used as main thing when working with pdf

*/

#include "PdfDocument.h"
#include "DLList.h"
#include "IndirectObject.h"
#include "PdfHeader.h"
#include "PdfBaseObject.h"
#include "PdfPages.h"
#include "PdfPage.h"
#include "PdfContentStream.h"
#include "PdfFont.h"
#include "PdfImage.h"
#include "PdfPageResources.h"
#include "PdfShadingDictionary.h"
#include "MemoryManager.h"
#include "MemoryWriter.h"
#include "FileWriter.h"
#include "PdfInformationDictionary.h"
#include "PdfTemplateInformationDictionary.h"
#include "PdfTemplateEmbeddedFont.h"
#include "Logger.h"
#include <stdlib.h>


DLLEXPORT_TEST_FUNCTION struct PdfDocument* PdfDocument_CreatePdfDocument(char *fileName)
{
	struct PdfDocument *result;
	result = (struct PdfDocument*) MemoryManager_Alloc(sizeof(struct PdfDocument));
	PdfDocument_Init(result);
	// fill not default values
	result->documentType = PDFDOCUMENT_TYPE_FILE;
	result->streamWriter = (struct StreamWriter*)FileWriter_Create();	
	strcpy(result->fileName, fileName);
	return result;
}

DLLEXPORT_TEST_FUNCTION void PdfDocument_Init(struct PdfDocument *self)
{
	self->currentObjectId = 0;
	self->nextObjectId = 1;
	self->majorVersion = 1;
	self->minorVersion = 7;
	self->rootObjectId = 1;	
	self->objectsCount = 0;
	self->crossReferenceTable = CrossReferenceTable_Create(self);
	self->indirectObjectList = DLList_Create();
	self->contentStreams = DLList_Create();
	self->informationDictionary = PdfInformationDictionary_Create(self);
	self->trailer = PdfTrailer_Create(self);
	self->catalog = PdfCatalog_Create(self);
	
	self->fonts = DLList_Create();
	self->embeddedFonts = DLList_Create();
	self->images = DLList_Create();
	self->resources = DLList_Create();
	self->shadings = DLList_Create();
	self->nextFontId = 1;
	self->nextImageId = 1;
	self->nextShadingId = 1;		
	self->streamWriter = 0;	
}


DLLEXPORT_TEST_FUNCTION struct PdfDocument* PdfDocument_CreatePdfDocumentInMemory()
{
	struct PdfDocument *result;
	result = (struct PdfDocument*) MemoryManager_Alloc(sizeof(struct PdfDocument));
	PdfDocument_Init(result);
	// fill not default values
	result->documentType = PDFDOCUMENT_TYPE_MEMORY;
	result->streamWriter = (struct StreamWriter*)MemoryWriter_Create();
	
	return result;	
}


// Remove and free document
DLLEXPORT_TEST_FUNCTION void PdfDocument_Destroy(struct PdfDocument* self)
{		
	PdfCatalog_Destroy(self->catalog);
	PdfTrailer_Destroy(self->trailer);
	CrossReferenceTable_Destroy(self->crossReferenceTable);

	// destroy all content streams	
	while(self->contentStreams->size > 0)
	{
		struct PdfContentStream *obj;
		obj = (struct PdfContentStream *)DLList_Back(self->contentStreams);
		DLList_PopBack(self->contentStreams);
		PdfContentStream_Destroy(obj);
	}
	DLList_Destroy(self->contentStreams); // destroy list itself

	// destroy all the indirect objects
	while(self->indirectObjectList->size > 0)
	{
		struct IndirectObject *obj;
		obj = (struct IndirectObject *)DLList_Back(self->indirectObjectList);
		DLList_PopBack(self->indirectObjectList);
		IndirectObject_Destroy(obj);
	}
	DLList_Destroy(self->indirectObjectList); // destroy list itself

	// destroy all fonts	
	while(self->fonts->size > 0)
	{
		struct PdfFont *obj;
		obj = (struct PdfFont *)DLList_Back(self->fonts);
		DLList_PopBack(self->fonts);
		PdfFont_Destroy(obj);
	}
	DLList_Destroy(self->fonts); // destroy list itself

	// destroy all embedded fonts
	while(self->embeddedFonts->size > 0)
	{
		struct PdfEmbeddedFont *obj;
		obj = (struct PdfEmbeddedFont *)DLList_Back(self->embeddedFonts);
		DLList_PopBack(self->embeddedFonts);
		PdfEmbeddedFont_Destroy(obj);
	}
	DLList_Destroy(self->embeddedFonts); // destroy list itself


	// destroy all images	
	while(self->images->size > 0)
	{
		struct PdfImage *obj;
		obj = (struct PdfImage *)DLList_Back(self->images);
		DLList_PopBack(self->images);
		PdfImage_Destroy(obj);
	}
	DLList_Destroy(self->images); // destroy list itself


	// destroy all resources	
	while(self->resources->size > 0)
	{
		struct PdfPageResources *obj;
		obj = (struct PdfPageResources *)DLList_Back(self->resources);
		DLList_PopBack(self->resources);
		PdfPageResources_Destroy(obj);
	}
	DLList_Destroy(self->resources); // destroy list itself

	// destroy all shadings
	while(self->shadings->size > 0)
	{
		struct PdfShadingDictionary *obj;
		obj = (struct PdfShadingDictionary *)DLList_Back(self->shadings);
		DLList_PopBack(self->shadings);
		PdfShadingDictionary_Destroy(obj);
	}
	DLList_Destroy(self->shadings); // destroy list itself

	// we destroy stream writer if it is file type. But if it is memory we dont destroy as 
	// this is needed to be returned from generator. This needs to be deleted manually
	if (self->documentType == PDFDOCUMENT_TYPE_FILE)
	{
		self->streamWriter->Destroy(self->streamWriter);
	}
	free(self);	
}

DLLEXPORT_TEST_FUNCTION int PdfDocument_Open(struct PdfDocument* document, int overwrite)
{	
	if (document != NULL)
	{
		if (document->documentType == PDFDOCUMENT_TYPE_FILE)
		{
			if (!FileWriter_Open((struct FileWriter*)document->streamWriter, document->fileName, "wb+"))		
			{
				Logger_LogErrorMessage("PdfDocument_Open: Failed opening fileName %s FAILED", document->fileName);
				return FALSE;
			}
			else
			{
				// write header
				PdfHeader_WriteHeader(document);
				Logger_LogNoticeMessage("PdfDocument_Open: Opened file %s SUCCESS", document->fileName);
				return TRUE;
			}
		} 
		else if (document->documentType == PDFDOCUMENT_TYPE_MEMORY)
		{
			// write header
			PdfHeader_WriteHeader(document);
			Logger_LogNoticeMessage("PdfDocument_Open: Opened pdf writing to memory");
			return TRUE;
		}
	}
	else 
	{
		Logger_LogErrorMessage("PdfDocument_Open: No document object FAILED");
		return FALSE;
	}
}

DLLEXPORT_TEST_FUNCTION int PdfDocument_Close(struct PdfDocument* document)
{
	if (document != NULL)
	{
		// write InformationDictionary object
		PdfInformationDictionary_Write(document->informationDictionary);
		// write Catalog object
		PdfCatalog_Write(document->catalog);
		// write cross reference table			
		CrossReferenceTable_Write(document->crossReferenceTable);
		// write trailer
		PdfTrailer_Write(document->trailer);

		if (document->documentType == PDFDOCUMENT_TYPE_FILE)
		{
			FileWriter_Close((struct FileWriter*)document->streamWriter);			
		}

		Logger_LogNoticeMessage("PdfDocument_Close: Close document SUCCESS");
		return TRUE;
	}
	else 
	{
		Logger_LogErrorMessage("PdfDocument_Close: No document to close FAILED");
		return FALSE;
	}
}

DLLEXPORT_TEST_FUNCTION void PdfDocument_BeginNewObject(struct PdfBaseObject *baseObject)
{
	struct IndirectObject *obj = IndirectObject_Create(baseObject->document);
	IndirectObject_BeginNewObject(obj);
	DLList_PushBack(baseObject->document->indirectObjectList, (void *)obj);

	baseObject->objectId = baseObject->document->currentObjectId;
	baseObject->generationNumber = 0;
}

DLLEXPORT_TEST_FUNCTION void PdfDocument_EndObject(struct PdfBaseObject *baseObject)
{
	struct IndirectObject *obj = (struct IndirectObject *)DLList_Back(baseObject->document->indirectObjectList);
	
	IndirectObject_EndObject(obj);
	baseObject->document->objectsCount++;
}


DLLEXPORT_TEST_FUNCTION struct PdfPage* PdfDocument_CreatePage(struct PdfDocument *self, struct PdfPageProperties *props)
{
	struct PdfPage *page;
	page = PdfPage_Create(self, props);
	DLList_PushBack(self->catalog->pages->pages, page);
	return page;
}


DLLEXPORT_TEST_FUNCTION struct PdfFont* PdfDocument_FindFont(struct PdfDocument *self, char *fontName)
{
	struct DLListNode *iter;
	for(iter = DLList_Begin(self->fonts); iter != DLList_End(self->fonts); iter = iter->next)
	{
		struct PdfFont *font;
		font = (struct PdfFont *)iter->data;
		if (strcmp(font->baseFont, fontName) == 0)
		{
			return font;
		}
	}
	return 0;
}

DLLEXPORT_TEST_FUNCTION struct PdfFont* PdfDocument_FindFontBySaveId(struct PdfDocument *self, int saveId)
{
	struct DLListNode *iter;
	for(iter = DLList_Begin(self->fonts); iter != DLList_End(self->fonts); iter = iter->next)
	{
		struct PdfFont *font;
		font = (struct PdfFont *)iter->data;
		if (font->templateEmbeddedFont->saveId == saveId)
		{
			return font;
		}
	}
	return 0;
}



DLLEXPORT_TEST_FUNCTION struct PdfImage* PdfDocument_FindImage(struct PdfDocument *self, char *imageName)
{
	struct DLListNode *iter;
	for(iter = DLList_Begin(self->images); iter != DLList_End(self->images); iter = iter->next)
	{
		struct PdfImage *image;
		image = (struct PdfImage *)iter->data;
		if (strcmp(image->baseImage, imageName) == 0)
		{
			return image;
		}
	}
	return 0;
}

DLLEXPORT_TEST_FUNCTION struct PdfShadingDictionary* PdfDocument_FindShading(struct PdfDocument *self, char *shadingName)
{
	struct DLListNode *iter;
	for(iter = DLList_Begin(self->shadings); iter != DLList_End(self->shadings); iter = iter->next)
	{
		struct PdfShadingDictionary *shading;
		shading = (struct PdfShadingDictionary *)iter->data;
		if (strcmp(shading->name, shadingName) == 0)
		{
			return shading;
		}
	}
	return 0;
}

DLLEXPORT_TEST_FUNCTION void PdfDocument_SetInformationDictionary(struct PdfDocument * self, struct PdfTemplateInformationDictionary* infoDict)
{
	self->informationDictionary->author = MemoryManager_StrDup(infoDict->Author);
	self->informationDictionary->subject = MemoryManager_StrDup(infoDict->Subject);
	self->informationDictionary->title = MemoryManager_StrDup(infoDict->Title);
	self->informationDictionary->producer = MemoryManager_StrDup(infoDict->Producer);
}

