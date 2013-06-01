#include "PdfTemplateItemStaticImage.h"
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
#include "GraphicWriter.h"
#include "PdfPageResources.h"
#include "PdfImage.h"
#include "Logger.h"
#include <libxml/parser.h>
#include <libxml/xmlmemory.h>



DLLEXPORT_TEST_FUNCTION struct PdfTemplateItemStaticImage* PdfTemplateItemStaticImage_CreateFromXml(xmlNode *node)
{
	struct PdfTemplateItemStaticImage *ret;
	char *embeddedImageData;
	xmlNode *foundNode;

	ret = (struct PdfTemplateItemStaticImage *)MemoryManager_Alloc(sizeof(struct PdfTemplateItemStaticImage));

	PdfTemplateBalloonItem_InitFromXml((struct PdfTemplateBalloonItem*)ret, node, PdfTemplateItemStaticImage_Destroy, PdfTemplateItemStaticImage_Process);

	foundNode = PdfTemplate_FindNode(node, SRC);
	if (foundNode)
	{
		ret->name = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(foundNode, SRC_NAME));
	}
	else
	{
		Logger_LogErrorMessage("PdfTemplateItemStaticImage_CreateFromXml: Src node is not found");
		PdfTemplateItemStaticImage_Destroy((struct PdfTemplateBalloonItem*)ret);
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

	foundNode = PdfTemplate_FindNode(node, "EmbeddedImage");
	if (foundNode)
	{
		// Load content into memory
		ret->imageDataLength = PdfTemplate_LoadIntAttribute(foundNode, "EmbeddedDecodedImageLength");
		if (ret->imageDataLength > 0)
		{
			ret->imageData = MemoryManager_Alloc(ret->imageDataLength);
			embeddedImageData = PdfTemplate_LoadTextContent(foundNode);		
			Base64Decode(embeddedImageData, ret->imageData, ret->imageDataLength);			
		}
	}

	return ret;
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemStaticImage_Cleanup(struct PdfTemplateItemStaticImage* self)
{
	// cleanup this code
	if (self->name)
	{
		MemoryManager_Free(self->name);
		self->name = 0;
	}	
	if (self->imageData)
	{
		MemoryManager_Free(self->imageData);
		self->imageData = 0;
		self->imageDataLength = 0;
	}

	// cleanup parent
	PdfTemplateBalloonItem_Cleanup((struct PdfTemplateBalloonItem*)self);
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemStaticImage_Destroy(struct PdfTemplateBalloonItem* self)
{
	PdfTemplateItemStaticImage_Cleanup((struct PdfTemplateItemStaticImage*)self);

	// destroy 
	MemoryManager_Free((struct PdfTemplateItemStaticImage *)self);
}

int PdfTemplateItemStaticImage_Process(struct PdfTemplateBalloonItem *self, struct PdfGenerator *generator, struct PdfGeneratedBalloon *balloon, struct StreamWriter *streamWriter)
{
	struct PdfGraphicWriter *graphicWriter;
	struct PdfImage *image;
	struct PdfTemplateItemStaticImage *item; 
	struct TransformationMatrix *transMat;
	
	Logger_LogNoticeMessage("PdfTemplateItemStaticImage_Process: BEGIN");
	// cast balloon item
	item = (struct PdfTemplateItemStaticImage*)self;

	// write info to pdf page
	graphicWriter = PdfGraphicWriter_Create(streamWriter);
	{	
		image = PdfDocument_FindImage(generator->document, item->name);
		if (!image)
		{	
			// make image object
			image = PdfImage_Create(generator->document, item->name);			
			if (!PdfImage_Write(image, TRUE, item->imageData, item->imageDataLength))
			{
				return TRUE;
			}
		}		

		// add to resources
		PdfPageResources_AddImage(generator->currentPage->properties.resources, image);
		Logger_LogNoticeMessage("PdfTemplateItemStaticImage_Process: Added image to resources");

		transMat = TransformationMatrix_Create();
		PdfTemplateBalloonItem_GetFullTransformation((struct PdfTemplateBalloonItem*)self, balloon, generator->currentPage, transMat, FALSE);
		PdfGraphicWriter_SetImageWithTransformation(graphicWriter, image, transMat->a, transMat->b, transMat->c, transMat->d, transMat->e, transMat->f);				
	}
	
	PdfGraphicWriter_Destroy(graphicWriter);	

	Logger_LogNoticeMessage("PdfTemplateItemStaticImage_Process: END");
	return TRUE;
}




