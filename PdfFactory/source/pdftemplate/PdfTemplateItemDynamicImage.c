#include "PdfTemplateItemDynamicImage.h"
#include "PdfTemplateElements.h"
#include "PdfTemplate.h"
#include "PdfGenerator.h"
#include "PdfTemplateBalloon.h"
#include "PdfPage.h"
#include "UnitConverter.h"
#include "MemoryManager.h"
#include "StreamWriter.h"
#include "TransformationMatrix.h"
#include "PdfGeneratedBalloon.h"
#include "PdfImage.h"
#include <libxml/parser.h>
#include <libxml/xmlmemory.h>
#include "GraphicWriter.h"
#include "PdfPageResources.h"
#include "Logger.h"



DLLEXPORT_TEST_FUNCTION struct PdfTemplateItemDynamicImage* PdfTemplateItemDynamicImage_CreateFromXml(xmlNode *node)
{
	struct PdfTemplateItemDynamicImage *ret;
	xmlNode *foundNode;

	ret = (struct PdfTemplateItemDynamicImage *)MemoryManager_Alloc(sizeof(struct PdfTemplateItemDynamicImage));

	PdfTemplateBalloonItem_InitFromXml((struct PdfTemplateBalloonItem*)ret, node, PdfTemplateItemDynamicImage_Destroy, PdfTemplateItemDynamicImage_Process);

	foundNode = PdfTemplate_FindNode(node, TEXT);
	if (foundNode)
	{
		ret->dataSourceName = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(foundNode, DATA_STREAM));
		ret->sourceColumn = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(foundNode, SOURCE_COLUMN));
		ret->imageType = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(foundNode, IMAGE_TYPE));
	}
	else
	{
		Logger_LogErrorMessage("PdfTemplateItemDynamicImage_CreateFromXml: Src node is not found");
		PdfTemplateItemDynamicImage_Destroy((struct PdfTemplateBalloonItem*)ret);
		return NULL;		
	}

	foundNode = PdfTemplate_FindNode(node, WIDTH);
	if (foundNode)
	{
		ret->base.scale->x = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(foundNode, VALUE));
	}

	foundNode = PdfTemplate_FindNode(node, HEIGHT);
	if (foundNode)
	{
		ret->base.scale->y = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(foundNode, VALUE));
	}


	return ret;
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemDynamicImage_Cleanup(struct PdfTemplateItemDynamicImage* self)
{
	// cleanup this code
	if (self->name)
	{
		MemoryManager_Free(self->name);
		self->name = 0;
	}	

	// cleanup parent
	PdfTemplateBalloonItem_Cleanup((struct PdfTemplateBalloonItem*)self);
}


DLLEXPORT_TEST_FUNCTION void PdfTemplateItemDynamicImage_Destroy(struct PdfTemplateBalloonItem* self)
{
	PdfTemplateItemDynamicImage_Cleanup((struct PdfTemplateItemDynamicImage*)self);

	// destroy 
	MemoryManager_Free((struct PdfTemplateItemDynamicImage *)self);
}

int PdfTemplateItemDynamicImage_Process(struct PdfTemplateBalloonItem *self, struct PdfGenerator *generator, struct PdfGeneratedBalloon *balloon, struct StreamWriter *streamWriter)
{
	struct PdfGraphicWriter *graphicWriter;
	struct PdfImage *image;
	struct PdfTemplateItemDynamicImage *item; 
	struct TransformationMatrix *transMat;
	char*  value = NULL;
	int	   dataSize;


	Logger_LogNoticeMessage("PdfTemplateItemDynamicImage_Process: BEGIN");
	// cast balloon item
	item = (struct PdfTemplateItemDynamicImage*)self;

	if (generator->requestDataCallback)
	{
		Logger_LogNoticeMessage("PdfTemplateItemDynamicImage_Process: Request data for data source: %s, column: %s", item->dataSourceName, item->sourceColumn);
		value = generator->requestDataCallback(item->dataSourceName, item->sourceColumn, &dataSize);		
		if (!value)
		{
			Logger_LogNoticeMessage("PdfTemplateItemDynamicImage_Process: Value failed to be retrieved");
			return FALSE;
		}
	}

	// write info to pdf page
	graphicWriter = PdfGraphicWriter_Create(streamWriter);
	{	
		if(strcmp(item->imageType, IMAGE_TYPE_FILESYSTEM) == 0)
		{
			Logger_LogNoticeMessage("PdfTemplateItemDynamicImage_Process: Write Image from File System: %s", value);
			image = PdfDocument_FindImage(generator->document, value);
			if (!image)
			{	
				// make image object
				image = PdfImage_Create(generator->document, value);			
				PdfImage_Write(image,0,NULL,0);			
			}
		}
		else if(strcmp(item->imageType, IMAGE_TYPE_DATA) == 0)
		{
			// make image object
			Logger_LogNoticeMessage("PdfTemplateItemDynamicImage_Process: Write Image");
			image = PdfImage_Create(generator->document, "DATA");
			//image->memoryImageExtension = MemoryManager_StrDup("jpg");
			PdfImage_Write(image, 1, value, dataSize);			
		}
		//else if(strcmp(item->imageType, IMAGE_TYPE_DATABMP) == 0)
		//{
		//	// make image object
		//	Logger_LogNoticeMessage("PdfTemplateItemDynamicImage_Process: Write Image BMP");
		//	image = PdfImage_Create(generator->document, "DATA");
		//	image->memoryImageExtension = MemoryManager_StrDup("bmp");
		//	PdfImage_Write(image, 1, value, dataSize);			
		//}				
		//else if(strcmp(item->imageType, IMAGE_TYPE_DATAGIF) == 0)
		//{
		//	// make image object
		//	Logger_LogNoticeMessage("PdfTemplateItemDynamicImage_Process: Write GIF");
		//	image = PdfImage_Create(generator->document, "DATA");
		//	image->memoryImageExtension = MemoryManager_StrDup("gif");
		//	PdfImage_Write(image, 1, value, dataSize);			
		//}
		//else if(strcmp(item->imageType, IMAGE_TYPE_DATAPNG) == 0)
		//{
		//	// make image object
		//	Logger_LogNoticeMessage("PdfTemplateItemDynamicImage_Process: Write Image from PNG");
		//	image = PdfImage_Create(generator->document, "DATA");
		//	image->memoryImageExtension = MemoryManager_StrDup("png");
		//	PdfImage_Write(image, 1, value, dataSize);			
		//}

		// add to resources
		PdfPageResources_AddImage(generator->currentPage->properties.resources, image);

		transMat = TransformationMatrix_Create();
		PdfTemplateBalloonItem_GetFullTransformation((struct PdfTemplateBalloonItem*)self, balloon, generator->currentPage, transMat, FALSE);
		PdfGraphicWriter_SetImageWithTransformation(graphicWriter, image, transMat->a, transMat->b, transMat->c, transMat->d, transMat->e, transMat->f);				
	}

	PdfGraphicWriter_Destroy(graphicWriter);	

	Logger_LogNoticeMessage("PdfTemplateItemDynamicImage_Process: END");
	return TRUE;
}




