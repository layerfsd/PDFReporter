
#include "PdfImage.h"
#include "PdfDocument.h"
#include "DictionaryObject.h"
#include "NameObject.h"
#include "NumberObject.h"
#include "StringObject.h"
#include "StreamWriter.h"
#include "StreamObject.h"
#include "MemoryManager.h"
#include "Logger.h"
#include "MemoryWriter.h"

#include <string.h>
#include <stdlib.h>
#include <stdio.h>
#include <gd.h>




DLLEXPORT_TEST_FUNCTION struct PdfImage *PdfImage_Create(struct PdfDocument *document, const char *baseImage)
{
	struct PdfImage *tmpImage;
	tmpImage = (struct PdfImage*)MemoryManager_Alloc(sizeof(struct PdfImage));
	PdfImage_Init(tmpImage, document, baseImage);
	return tmpImage;
}




DLLEXPORT_TEST_FUNCTION void PdfImage_Init(struct PdfImage *self, struct PdfDocument *document, const char *baseImage)
{
	char tmp[100];
	PdfBaseObject_Init(&self->base, document);
	self->subType = MemoryManager_StrDup("Image");
	self->width = 0;
	self->height = 0;
	self->colorSpace = MemoryManager_StrDup(DEVICE_RGB);
	self->bitsPerComponent = 8;
	self->length = 0;
	self->baseImage = MemoryManager_StrDup(baseImage);
	self->fFilter = MemoryManager_StrDup("DTCDecode");
	self->memoryImageExtension = MemoryManager_StrDup("jpg");

	sprintf(tmp, "Im%d", document->nextImageId);
	document->nextImageId++;
	self->name = MemoryManager_StrDup(tmp);

	DLList_PushBack(document->images, self);  // add self to document list
}



void PdfImage_Cleanup(struct PdfImage *self)
{
	if (self->subType)
	{
		MemoryManager_Free(self->subType);
		self->subType = 0;
	}
	if (self->colorSpace)
	{
		MemoryManager_Free(self->colorSpace);
		self->colorSpace = 0;
	}
	if (self->baseImage)
	{
		MemoryManager_Free(self->baseImage);
		self->baseImage = 0;
	}
	if (self->fFilter)
	{
		MemoryManager_Free(self->fFilter);
		self->fFilter= 0;
	}
	if (self->name)
	{
		MemoryManager_Free(self->name);
		self->name = 0;
	}
}




void PdfImage_Destroy(struct PdfImage *self)
{
	PdfImage_Cleanup(self);
	MemoryManager_Free(self);
}


void getFileExtenstion(const char *fileName, char *outExtension)
{
	unsigned int i;
	unsigned int len;
	int extStarted = 0;
	int extIndex = 0;

	len = strlen(fileName);
	for(i = 0; i < len; i++)
	{
		if (fileName[i] == '.')
		{
			extStarted = 1;
			extIndex = 0;
		}
		else if (extStarted)
		{
			outExtension[extIndex] = tolower(fileName[i]);
			extIndex++;
		}						
	}
	outExtension[extIndex] = '\0';
}


DLLEXPORT_TEST_FUNCTION int PdfImage_Write(struct PdfImage *self, int fromData, char* data, int dataSize)
{
	struct DictionaryObject *dict;
	struct NameObject *name;
	struct NumberObject *number;	
	struct StreamObject *stream;
	struct MemoryWriter *memoryWriter;
	int pos, tmpPos, i, j, k;
	FILE *f;
	char *imageArr;
	char *newImage; 
	char fileExtension[10];

	gdImagePtr image;

	if(!fromData)
	{
		f = fopen(self->baseImage, "rb");
		if (f)
		{
			getFileExtenstion(self->baseImage, fileExtension);		
			image = gdImageCreateFromJpeg(f);
			if (!image)
			{
				fseek(f, 0, 0);
				image = gdImageCreateFromGif(f);
			}
			if (!image)
			{
				fseek(f, 0, 0);
				image = gdImageCreateFromPng(f);
			}
			if (!image)
			{
				fseek(f, 0, 0);
				image = gdImageCreateFromWBMP(f);
			}	
			
			if (!image)
			{
				Logger_LogErrorMessage("PdfImage_Write: Image not Created or unsupported image format! FAILED");
			}

			self->width = image->sx;
			self->height = image->sy;		
			fclose(f);
		}
		else 
		{
			Logger_LogWarningMessage("PdfImage_Write: Image not Created as filename is missing! FAILED");
			return FALSE;
		}
	}
	else
	{
		image = gdImageCreateFromJpegPtr(dataSize, data);
		if (!image)
		{
			image = gdImageCreateFromGifPtr(dataSize, data);
		}
		if (!image)
		{
			image = gdImageCreateFromPngPtr(dataSize, data);			
		}
		if (!image)
		{
			image = gdImageCreateFromWBMPPtr(dataSize, data);
		}		

		if(image)
		{
			self->width = image->sx;
			self->height = image->sy;	
		}else
		{
			Logger_LogErrorMessage("PdfImage_Write: Error working with picture data! (%s) FAILED", fileExtension);
			return FALSE;
		}
	}


	PdfDocument_BeginNewObject((struct PdfBaseObject*)self);
	dict = DictionaryObject_Begin(self->base.document->streamWriter);
	
	DictionaryObject_WriteKey(dict, "Type");
	name = NameObject_Create(self->base.document->streamWriter, "XObject");
	NameObject_Write(name);
	NameObject_Destroy(name);

	DictionaryObject_WriteKey(dict, "Subtype");
	name = NameObject_Create(self->base.document->streamWriter, self->subType);
	NameObject_Write(name);
	NameObject_Destroy(name);

	DictionaryObject_WriteKey(dict, "Name");
	name = NameObject_Create(self->base.document->streamWriter, self->name);
	NameObject_Write(name);
	NameObject_Destroy(name);
	
	DictionaryObject_WriteKey(dict, "Width");
	number = NumberObject_Create(self->base.document->streamWriter, self->width);
	NumberObject_Write(number);
	NumberObject_Destroy(number);

	DictionaryObject_WriteKey(dict, "Height");
	number = NumberObject_Create(self->base.document->streamWriter, self->height);
	NumberObject_Write(number);
	NumberObject_Destroy(number);

	DictionaryObject_WriteKey(dict, "ColorSpace");
	name = NameObject_Create(self->base.document->streamWriter, self->colorSpace);
	NameObject_Write(name);
	NameObject_Destroy(name);

	DictionaryObject_WriteKey(dict, "BitsPerComponent");
	number = NumberObject_Create(self->base.document->streamWriter, self->bitsPerComponent);
	NumberObject_Write(number);
	NumberObject_Destroy(number);


	DictionaryObject_WriteKey(dict, "ImageMask");
	self->base.document->streamWriter->WriteData(self->base.document->streamWriter, "false");	

	DictionaryObject_WriteKey(dict, "Length");

	pos = self->base.document->streamWriter->GetPosition(self->base.document->streamWriter);
	self->base.document->streamWriter->WriteData(self->base.document->streamWriter, "          ");	

	DictionaryObject_WriteKey(dict, "Filter");
	self->base.document->streamWriter->WriteData(self->base.document->streamWriter, "/FlateDecode");

	DictionaryObject_End(dict);
	
	// we should compress image in memory and then store it to file
	stream = StreamObject_BeginInMemory();
	if (image)
	{
		if (image->trueColor)
		{
			newImage = MemoryManager_Alloc(image->sx*3*image->sy);
			k = 0;
		}
		for(i = image->sy-1; i >= 0; i--)
		{
			if (image->trueColor)
			{
				imageArr = (char *)(image->tpixels[i]);
				for (j = 0; j < image->sx*4; j+=4)
				{
					newImage[k++] = imageArr[j+2];
					newImage[k++] = imageArr[j+1];
					newImage[k++] = imageArr[j];					
				}								
			}
			else 
			{
				stream->streamWriter->WriteBinaryData(stream->streamWriter, image->pixels[i], image->sx);
			}
		}		
		if (image->trueColor)
		{
			stream->streamWriter->WriteBinaryData(stream->streamWriter, newImage, image->sx*3*image->sy);				
			MemoryManager_Free(newImage);
		}
		gdImageDestroy(image);
	}

	StreamObject_End(stream);
	memoryWriter = (struct MemoryWritter*)stream->streamWriter;	
	self->base.document->streamWriter->WriteBinaryData(self->base.document->streamWriter, memoryWriter->memory, memoryWriter->size);
	// -- end of writting image stream to memory and compressing

	// go and write length again
	tmpPos = self->base.document->streamWriter->GetPosition(self->base.document->streamWriter);
	self->base.document->streamWriter->Seek(self->base.document->streamWriter, pos);
	// write length of image
	self->length = stream->length;
	number = NumberObject_Create(self->base.document->streamWriter, self->length);
	NumberObject_Write(number);
	NumberObject_Destroy(number);

	self->base.document->streamWriter->Seek(self->base.document->streamWriter, tmpPos);

	StreamObject_Destroy(stream);
	PdfDocument_EndObject((struct PdfBaseObject*)self);
	Logger_LogNoticeMessage("PdfImage_Write: SUCCESS");
	return TRUE;
}



