/*
PdfGenerator.c

Author: Nebojsa Vislavski
Date: 12.08.2008.	

*/

#include "pdfgenerator.h"
#include "PdfTemplate.h"
#include "PdfGeneratorDataStream.h"
#include "DLList.h"
#include "PdfDocument.h"
#include "NameObject.h"
#include "ArrayObject.h"
#include "DictionaryObject.h"
#include "PdfContentStream.h"
#include "Rectangle.h"
#include "PdfPage.h"
#include "PdfTemplatePage.h"
#include "StringObject.h"
#include "PdfFont.h"
#include "PdfTextWriter.h"
#include "PdfPageResources.h"
#include "UnitConverter.h"
#include "StreamWriter.h"
#include "MemoryWriter.h"
#include "PdfImage.h"
#include "MemoryManager.h"
#include "PdfGeneratedBalloon.h"
#include "GraphicWriter.h"
#include "PdfTextWriter.h"
#include "TransformationMatrix.h"
#include "Logger.h"
#include "md5.h"

DLLEXPORT_TEST_FUNCTION struct PdfGenerator* PdfGenerator_Create()
{
	struct PdfGenerator *x = (struct PdfGenerator*)MemoryManager_Alloc(sizeof(struct PdfGenerator));
	PdfGenerator_Init(x);
	return x;
}

DLLEXPORT_TEST_FUNCTION void PdfGenerator_Init(struct PdfGenerator *self)
{
	self->useCompression = FALSE;
	self->progressCallback = NULL;
	self->pdfTemplate = NULL;
	self->templateLoaded = 0;
	self->dataStreams  = DLList_Create();
	self->readDataCallback = NULL;
	self->resetDataStreamCallback = NULL;
	self->requestDataCallback = NULL;
	self->initializeDataStreamCallback = NULL;
	self->currentPage = NULL;
	self->currentContentStream = NULL;
	self->rectMatrix = DLList_Create();
	self->pageBalloons = DLList_Create();
	self->currentPageBalloon = NULL;
	self->pageSkipDataReadMarker = FALSE;
	self->pageNumber = 0;
	self->validSerial = FALSE;
	self->document = 0;
}

DLLEXPORT_TEST_FUNCTION void PdfGenerator_Destroy(struct PdfGenerator *self)
{
	// clear page balloons
	while(self->pageBalloons->size > 0)
	{
		struct PdfGeneratedBalloon *obj;
		obj = (struct PdfGeneratedBalloon *)DLList_Back(self->pageBalloons);
		DLList_PopBack(self->pageBalloons);
		PdfGeneratedBalloon_Destroy(obj);
	}
	DLList_Destroy(self->pageBalloons); 

	// clear rect matrix
	while(self->rectMatrix->size > 0)
	{
		struct Rectangle *obj;
		obj = (struct Rectangle *)DLList_Back(self->rectMatrix);
		DLList_PopBack(self->rectMatrix);
		Rectangle_Destroy(obj);
	}
	DLList_Destroy(self->rectMatrix); 

	// destroy all data streams
	PdfGenerator_ClearDataStreams(self);	
	MemoryManager_Free(self);	
}

void PdfGenerator_ClearDataStreams(struct PdfGenerator* self)
{
	while(self->dataStreams->size > 0)
	{
		struct PdfGeneratorDataStream *obj;
		obj = (struct PdfGeneratorDataStream *)DLList_Back(self->dataStreams);
		DLList_PopBack(self->dataStreams);
		PdfGeneratorDataStream_Destroy(obj);
	}
	DLList_Destroy(self->dataStreams); // destroy list itself

}


DLLEXPORT_TEST_FUNCTION int PdfGenerator_AttachTemplateFromFile(struct PdfGenerator* self, char *templateName)
{
	// todo: add code that will make PdfTemplate from file 
	self->pdfTemplate = PdfTemplate_Create();
	Logger_LogNoticeMessage("Loading template file...");
	if (!PdfTemplate_Load(self->pdfTemplate, templateName))
	{
		Logger_LogNoticeMessage("Loading template file... FAILED");
		return FALSE;
	}
	self->templateLoaded = 1;
	Logger_LogNoticeMessage("Loading template file... SUCCESS");
	return TRUE;
}


DLLEXPORT_TEST_FUNCTION int PdfGenerator_AttachTemplateFromMemory(struct PdfGenerator* self, char *templateData, int templateSize)
{
	// TODO: not yet implemented
	self->pdfTemplate = PdfTemplate_Create();
	Logger_LogNoticeMessage("Loading template from data...");
	if (!PdfTemplate_LoadFromData(self->pdfTemplate, templateData, templateSize))
	{
		Logger_LogNoticeMessage("Loading template from data... FAILED");
		return FALSE;
	}
	self->templateLoaded = 1;
	Logger_LogNoticeMessage("Loading template data... SUCCESS");
	return TRUE;	
	
}


DLLEXPORT_TEST_FUNCTION void PdfGenerator_SetProgressCallback(struct PdfGenerator* self, GenerateProgressCallback callback)
{
	self->progressCallback = callback;
}

DLLEXPORT_TEST_FUNCTION void PdfGenerator_SetReadDataCallback(struct PdfGenerator* self, ReadDataCallback callback)
{
	self->readDataCallback = callback;	
}

DLLEXPORT_TEST_FUNCTION void PdfGenerator_SetResetDataStreamCallback(struct PdfGenerator* self, ResetDataStreamCallback callback)
{
	self->resetDataStreamCallback = callback;
}

DLLEXPORT_TEST_FUNCTION void PdfGenerator_SetRequestDataCallback(struct PdfGenerator* self, RequestDataCallback callback)
{
	self->requestDataCallback = callback;
}

DLLEXPORT_TEST_FUNCTION void PdfGenerator_SetInitializeDataStreamCallback(struct PdfGenerator* self, InitializeDataStreamCallback callback)
{
	self->initializeDataStreamCallback = callback;
}




// This will check if connections in streams and other things are ok. return true if everything is ok.
int PdfGenerator_CheckCorrectness(struct PdfGenerator *self)
{
	return TRUE;
}



void PdfGenerator_PrepareNewPage(struct PdfGenerator *self)
{

}

void PdfGenerator_WriteNewPage(struct PdfGenerator *self)
{

}

/* 
This will generate pages from template page description 
*/
struct PdfPage* PdfGenerator_GeneratePageFromTemplate(struct PdfGenerator *self, struct PdfTemplatePage *pageTemplate)
{
	// 1. make page resources and page properties struct. PreparePage
	struct PdfPageProperties pageProps;
	struct PdfPageResources *resources;
	struct PdfPage *page;
	struct UnitConverter *unitConverter;

	
	unitConverter = UnitConverter_Create();
	UnitConverter_AddCommonUnits(unitConverter);

	//create resources for page so they can be modified later
	resources = PdfPageResources_Create(self->document);	

	// fill page props
    pageProps.resources = resources;
    pageProps.mediaBox.lowerLeftX = 0;
    pageProps.mediaBox.lowerLeftY = 0;		
    pageProps.mediaBox.upperRightX = UnitConverter_ConvertToPoints(unitConverter, pageTemplate->size->width);
    pageProps.mediaBox.upperRightY = UnitConverter_ConvertToPoints(unitConverter, pageTemplate->size->height);	
	page = PdfDocument_CreatePage(self->document, &pageProps);	

	// create page balloon
	self->currentPageBalloon = PdfGeneratedBalloon_Create();
	self->currentPageBalloon->containerRect.lowerLeftX = 0;
	self->currentPageBalloon->containerRect.lowerLeftY = 0;
	self->currentPageBalloon->containerRect.upperRightX = pageProps.mediaBox.upperRightX;
	self->currentPageBalloon->containerRect.upperRightY = pageProps.mediaBox.upperRightY;
	self->currentPageBalloon->currentPage = page;

	self->currentPageBalloon->height = (float)pageProps.mediaBox.upperRightY;
	self->currentPageBalloon->width = (float)pageProps.mediaBox.upperRightX;
	self->currentPageBalloon->positionRect.top = 0;
	self->currentPageBalloon->positionRect.left = 0;
	self->pageNumber++;

	DLList_PushBack(self->pageBalloons, self->currentPageBalloon);

	UnitConverter_Destroy(unitConverter);
	Logger_LogNoticeMessage("PdfGenerator_GeneratePageFromTemplate: SUCCESS");
	return page;
}


struct PdfGeneratorDataStream* PdfGenerator_FindDataStream(struct PdfGenerator *self, char *dataStreamName)
{
	struct PdfGeneratorDataStream *stream;
	struct DLListNode *iter;
	for(iter = DLList_Begin(self->dataStreams); iter != DLList_End(self->dataStreams); iter = iter->next)
	{
		stream = (struct PdfGeneratorDataStream*)iter->data;		
		if (dataStreamName && strcmp(stream->name, dataStreamName) == 0)
		{
			return stream;
		}
	}

	Logger_LogNoticeMessage("PdfGenerator_FindDataStream: Data stream %s not found", dataStreamName);
	return 0;
}


int PdfGenerator_InitializeDataStream(struct PdfGenerator *self, char *parentDataStreamName, char *dataStreamName)
{
	// check if data stream is initialized and if not initialize it
	struct PdfGeneratorDataStream *stream;


	stream = PdfGenerator_FindDataStream(self, dataStreamName);
	if (stream)
	{
		if (!stream->initialized)
		{
			if (self->initializeDataStreamCallback)
			{				
				if (!self->initializeDataStreamCallback(parentDataStreamName, dataStreamName, &stream->itemsCount))
				{
					Logger_LogErrorMessage("InitializeDataStreamcallback failed for DataStream %s", dataStreamName);
					return FALSE;
				}
				if(self->resetDataStreamCallback)
				{
					self->resetDataStreamCallback(dataStreamName);
				}
			}
			else 
			{
				Logger_LogNoticeMessage("InitializeDataStreamcallback: There is no callback set for init data stream");
				return FALSE;
			}
			stream->initialized = TRUE;
		}
	}
	else 
	{
		stream = PdfGeneratorDataStream_Create(dataStreamName);
		if (self->initializeDataStreamCallback)
		{						
			if (dataStreamName && !self->initializeDataStreamCallback(parentDataStreamName, dataStreamName, &stream->itemsCount))
			{
				Logger_LogErrorMessage("InitializeDataStreamcallback failed for DataStream %s", dataStreamName);
				return FALSE;
			}
			if(self->resetDataStreamCallback)
			{
				self->resetDataStreamCallback(dataStreamName);
			}
		}
		else 
		{
			PdfGeneratorDataStream_Destroy(stream);
			Logger_LogErrorMessage("InitializeDataStreamcallback is not set for PdfGenerator. DataStream was %s", dataStreamName);
			return FALSE;
		}
		stream->initialized = TRUE;
		DLList_PushBack(self->dataStreams, stream);
	}

	return TRUE;
}



int PdfGenerator_DrawBalloon(struct PdfGenerator *self, struct PdfTemplateBalloon *balloon, struct PdfGeneratedBalloon *generatedBalloon)
{
	struct PdfTemplateBalloonItem *item;
	struct DLListNode *iter;

	// draw background for generated balloon
	PdfGeneratedBalloon_DrawBackground(generatedBalloon);

	for(iter = DLList_Begin(balloon->items); iter != DLList_End(balloon->items); iter = iter->next)
	{
		item = (struct PdfTemplateBalloonItem*)iter->data;			
		if (!item->process(item, self, generatedBalloon, self->currentContentStream->stream->streamWriter))
		{
			Logger_LogErrorMessage("PdfGenerator_DrawBalloon: Calling process on on item FAILED, please check column names");			
		}
	}

	return TRUE;
}


void PdfGenerator_DrawDemo(struct PdfGenerator *self)
{
	struct PdfTextWriter *textWriter;
	struct PdfFont *font;
	struct DLListNode *iterator; 
	struct TransformationMatrix *transMat;
	float pointX, pointY;
	char tmp[100];

	pointX = self->currentPage->properties.mediaBox.upperRightX / 10.0f;
	pointY = self->currentPage->properties.mediaBox.upperRightY / 10.0f;
	
	textWriter = PdfTextWriter_Begin(self->currentContentStream->stream->streamWriter);
	
	iterator = DLList_Begin(self->document->fonts);
	if (iterator)
	{
		font = (struct PdfFont*)iterator->data;
		if (!font)
		{
			return;
		}
	}
	// add to resources
	PdfPageResources_AddFont(self->currentPage->properties.resources, font);			

	sprintf(tmp, "%3.3f", self->currentPage->properties.mediaBox.upperRightX / 10);
	PdfTextWriter_SetFont(textWriter, font, tmp);

	transMat = TransformationMatrix_Create();	
	PdfTextWriter_SetTextMatrix(textWriter, 0.707f, 0.707f, -0.707f, 0.707f, pointX, pointY);				
	PdfTextWriter_SetRGBColor(textWriter, 200, 200, 200);
	PdfTextWriter_WriteText(textWriter, "THIS IS DEMO VERSION");		

	PdfTextWriter_EndText(textWriter);
}

// Create balloon from template 
struct PdfGeneratedBalloon* PdfGenerator_CreateBalloon(struct PdfGenerator *self, struct PdfTemplateBalloon *balloon, 
														struct PdfTemplatePage *templatePage, short isStaticBalloon)
{
	struct PdfGeneratedBalloon *newGeneratedBalloon;
	float relLocationX;
	float relLocationY;
	float width;
	float height;
	struct UnitConverter *unitConverter;	

	unitConverter = UnitConverter_Create();
	UnitConverter_AddCommonUnits(unitConverter);

	// create generated balloon item and place it inside parent
	newGeneratedBalloon = PdfGeneratedBalloon_Create();		
	newGeneratedBalloon->currentPage = self->currentPage;
	newGeneratedBalloon->templateBalloon = balloon;
	newGeneratedBalloon->canGrow = balloon->canGrow;	
	newGeneratedBalloon->fillColorR = balloon->fillColorR;
	newGeneratedBalloon->fillColorG = balloon->fillColorG;
	newGeneratedBalloon->fillColorB = balloon->fillColorB;
	newGeneratedBalloon->fillColorA = balloon->fillColorA;

	if (isStaticBalloon)
	{
		relLocationX = UnitConverter_ConvertToPoints(unitConverter, balloon->location->positionX);
		relLocationY = UnitConverter_ConvertToPoints(unitConverter, balloon->location->positionY);
		width =  PdfTemplateBalloon_GetFitToContentWidth(balloon);
		height = PdfTemplateBalloon_GetFitToContentHeight(balloon);

		newGeneratedBalloon->containerRect.lowerLeftX = relLocationX;
		newGeneratedBalloon->containerRect.lowerLeftY = relLocationY + height;
		newGeneratedBalloon->containerRect.upperRightX = relLocationX + width;
		newGeneratedBalloon->containerRect.upperRightY = relLocationY;
		newGeneratedBalloon->positionRect.top = relLocationY;
		newGeneratedBalloon->positionRect.left = relLocationX;
		newGeneratedBalloon->width = width;
		newGeneratedBalloon->height = height;		
	}
	else 
	{
		relLocationX = UnitConverter_ConvertToPoints(unitConverter, balloon->location->positionX);
		relLocationY = UnitConverter_ConvertToPoints(unitConverter, balloon->location->positionY);
		width =  PdfTemplateBalloon_GetFitToContentWidth(balloon);
	    height = PdfTemplateBalloon_GetFitToContentHeight(balloon);

		newGeneratedBalloon->width = width;
		newGeneratedBalloon->height = height;
		newGeneratedBalloon->positionRect.top = relLocationY;
		newGeneratedBalloon->positionRect.left = relLocationX;
	}
	return newGeneratedBalloon;
}

// This will draw balloon and write its contents to page
struct PdfGeneratedBalloon* PdfGenerator_WriteBalloon(struct PdfGenerator *self, struct PdfTemplateBalloon *balloon, 
														struct PdfTemplatePage *templatePage, struct PdfGeneratedBalloon *parentGeneratedBalloon, 
														int isStaticBalloon, short drawChildren, short staticBottomDockedFlag)
{
	struct PdfGeneratedBalloon *newGeneratedBalloon;
	
	Logger_LogNoticeMessage("PdfGenerator_WriteBalloon: BEGIN");
	// if we don't have current content stream begin new one
	if (!self->currentContentStream)
	{
		self->currentContentStream = PdfContentStream_Begin(self->document, self->useCompression);
		Logger_LogNoticeMessage("PdfGenerator_WriteBalloon: Content Stream BEGIN...");
	}

	// if this balloon is static then place it exactly where it should go
	if (isStaticBalloon)
	{
		newGeneratedBalloon = PdfGenerator_CreateBalloon(self, balloon, templatePage, isStaticBalloon);
		// we don't check if static balloons fall inside parents as they should always be included. Just allocate space for it in parent
		PdfGeneratedBalloon_AddChild(parentGeneratedBalloon, newGeneratedBalloon, TRUE, staticBottomDockedFlag);		
		// draw items to content stream
		if (drawChildren)
		{
			PdfGenerator_DrawBalloon(self, balloon, newGeneratedBalloon);
		}
	}
	else 
	{
		newGeneratedBalloon = PdfGenerator_CreateBalloon(self, balloon, templatePage, isStaticBalloon);
		// in case of dynamic balloons we do next thing
		if (!PdfGeneratedBalloon_AddChild(parentGeneratedBalloon, newGeneratedBalloon, FALSE, staticBottomDockedFlag))
		{
			// we failed generating new balloon
			PdfGeneratedBalloon_Destroy(newGeneratedBalloon);
			Logger_LogNoticeMessage("PdfGenerator_WriteBalloon: Returned Null Balloon END");
			return NULL;
		}		
		else 
		{
			// draw items to content stream
			if (drawChildren)
			{
				PdfGenerator_DrawBalloon(self, balloon, newGeneratedBalloon);
			}
		}
	}		
	
	Logger_LogNoticeMessage("PdfGenerator_WriteBalloon: Returned new Balloon END");
	return newGeneratedBalloon;	
}


#define GENERATE_BALLOONS_FAILED 0
#define GENERATE_BALLOONS_NOT_ENOUGH_SPACE 1
#define GENERATE_BALLOONS_OK 2


void PdfGenerator_DrawBalloonBorders(struct PdfGenerator *self, struct PdfTemplateBalloon *balloon, struct PdfGeneratedBalloon *generatedBalloon)
{
	struct PdfGraphicWriter *graphWriter; 	
	double absX, absY;
	double halfWidth;

	absX = PdfGeneratedBalloon_GetAbsoluteLocationX(generatedBalloon);
	absY = PdfGeneratedBalloon_GetAbsoluteLocationY(generatedBalloon);

	
	graphWriter = PdfGraphicWriter_Create(self->currentContentStream->stream->streamWriter);			
	{
		// top border
		if (balloon->topBorder.enabled)
		{		
			halfWidth = balloon->topBorder.width / 2.0;
			PdfGraphicWriter_SetLineWidth(graphWriter, balloon->topBorder.width);
			PdfGraphicWriter_SetRGBStrokeColor(graphWriter, balloon->topBorder.r, balloon->topBorder.g, balloon->topBorder.b);
			PdfGraphicWriter_PaintRGBStroke(graphWriter);
			PdfGraphicWriter_DrawLine(graphWriter, absX, absY-halfWidth, (absX + generatedBalloon->width), absY-halfWidth);		
		}
		// bottom border
		if (balloon->bottomBorder.enabled)
		{		
			halfWidth = balloon->bottomBorder.width / 2.0;
			PdfGraphicWriter_SetLineWidth(graphWriter, balloon->bottomBorder.width);
			PdfGraphicWriter_SetRGBStrokeColor(graphWriter, balloon->bottomBorder.r, balloon->bottomBorder.g, balloon->bottomBorder.b);
			PdfGraphicWriter_PaintRGBStroke(graphWriter);
			PdfGraphicWriter_DrawLine(graphWriter, absX, absY-generatedBalloon->height+halfWidth, absX + generatedBalloon->width, absY-generatedBalloon->height+halfWidth);		
		}

		// left border
		if (balloon->leftBorder.enabled)
		{		
			halfWidth = balloon->leftBorder.width / 2.0;
			PdfGraphicWriter_SetLineWidth(graphWriter, balloon->leftBorder.width);
			PdfGraphicWriter_SetRGBStrokeColor(graphWriter, balloon->leftBorder.r, balloon->leftBorder.g, balloon->leftBorder.b);
			PdfGraphicWriter_PaintRGBStroke(graphWriter);
			PdfGraphicWriter_DrawLine(graphWriter, absX+halfWidth, absY, absX+halfWidth, absY-generatedBalloon->height);		
		}

		// right border
		if (balloon->rightBorder.enabled)
		{		
			halfWidth = balloon->rightBorder.width / 2.0;
			PdfGraphicWriter_SetLineWidth(graphWriter, balloon->rightBorder.width);
			PdfGraphicWriter_SetRGBStrokeColor(graphWriter, balloon->rightBorder.r, balloon->rightBorder.g, balloon->rightBorder.b);
			PdfGraphicWriter_PaintRGBStroke(graphWriter);
			PdfGraphicWriter_DrawLine(graphWriter, absX+generatedBalloon->width-halfWidth, absY, absX + generatedBalloon->width - halfWidth, absY-generatedBalloon->height);		
		}
	}
	PdfGraphicWriter_Destroy(graphWriter);
}

// This returns false when something is wrong
// parentBalloon can be null. This means top level balloons should be used
int PdfGenerator_GenerateBalloons(struct PdfGenerator *self, struct PdfTemplateBalloon *parentBalloon, 
									struct PdfGeneratedBalloon *parentGeneratedBalloon, int level)
{
	struct PdfTemplate *pdfTemplate = self->pdfTemplate;
	struct PdfTemplatePage *currentPageTemplate = self->pdfTemplate->page;
	struct DLListNode *iter;
	struct DLList *balloons;
	struct DLList *newBottomDockedBalloons;
	struct PdfTemplateBalloon *balloon;
	struct PdfGeneratorDataStream *stream;
	struct PdfGeneratedBalloon *newGeneratedBalloon;
	int tmpI = 0;
	int genRes;
	int dynamicBalloonProcessed = FALSE;
	char *tmpStr;
	short generatorResult = GENERATE_BALLOONS_OK;
	short generatorResultStatic = GENERATE_BALLOONS_OK;


	newBottomDockedBalloons = DLList_Create();

	if (!parentBalloon)
	{
		Logger_LogNoticeMessage("PdfGenerator_GenerateBalloons: BEGIN NO PARENT BALLOON");
		balloons = currentPageTemplate->balloons;
	} 
	else 
	{
		Logger_LogNoticeMessage("PdfGenerator_GenerateBalloons: BEGIN HAS PARENT BALLOON");
		balloons = parentBalloon->balloons;
	}
	

	// 1. generate static balloons
	for(iter = DLList_Begin(balloons); iter != DLList_End(balloons); iter = iter->next)
	{
		balloon = (struct PdfTemplateBalloon*)iter->data;

		// skip balloons that are already generated and that are not available on every page
		if (!balloon->availableOnEveryPage && balloon->lastGeneratedPageNumber < self->pageNumber)
		{
			Logger_LogNoticeMessage("PdfGenerator_GenerateBalloons: Skip balloons - if (!balloon->availableOnEveryPage && balloon->lastGeneratedPageNumber < self->pageNumber)");
			continue;
		}

		// Skip ballons that are top docked and have prev dynamic balloon top docked
		if (balloon->isStatic && balloon->dockPosition == DOCK_TOP && balloon->hasPrevDynamicTopDocked)
		{
			continue;
		}

		// if this is balloon on page that is bottom docked then just take place for it or 
		// if this is balloon inside ballon that cannot grow
		if ((balloon->isStatic && balloon->dockPosition == DOCK_BOTTOM && parentBalloon == NULL) ||
			(balloon->isStatic && balloon->dockPosition == DOCK_BOTTOM && balloon->parentBalloon != NULL && !balloon->parentBalloon->canGrow))
		{
            // create it as standard static balloon but do not draw children items yet just allocate space for it
			newGeneratedBalloon = PdfGenerator_WriteBalloon(self, balloon, currentPageTemplate, parentGeneratedBalloon, TRUE, FALSE, TRUE);
			DLList_PushBack(newBottomDockedBalloons, newGeneratedBalloon);
		}
		//	If this ballon is static and not docked bottom then generate it
		else if (balloon->isStatic && balloon->dockPosition != DOCK_BOTTOM)
		{
			// Check its data stream. If not initialized call InitializeDataStreamCallback. if failed stop 
			stream = PdfGenerator_FindDataStream(self, balloon->dataStream);
			if (!balloon->skipDataReadMarker && !self->pageSkipDataReadMarker)
			{	
				if (parentBalloon)
				{
					tmpStr = parentBalloon->dataStream;
				}
				else
				{
					tmpStr = NULL;
				}

				// It is ok to have no data stream to initialize for static ballons
				if (balloon->dataStream)
				{
					if (!PdfGenerator_InitializeDataStream(self, tmpStr, balloon->dataStream))
					{
						Logger_LogErrorMessage("PdfGenerator_GenerateBalloons: Initializing data stream on static balloon. DataStream: %s FAILED", balloon->dataStream);
						return FALSE;
					}	
					else 
					{
						Logger_LogNoticeMessage("PdfGenerator_GenerateBalloons: Initializing data stream on static balloon. DataStream: %s SUCCESS", balloon->dataStream);
					}
				}
			}

			// Call ReadDataCallback if required
			if (balloon->dataStream)
			{
				if (self->readDataCallback)
				{				
					if (!balloon->skipDataReadMarker && !self->pageSkipDataReadMarker)
					{	
						if (!stream || (stream && !stream->initialized))
						{
							self->readDataCallback(balloon->dataStream);
							Logger_LogNoticeMessage("PdfGenerator_GenerateBalloons: Read data callback for DataStream: %s SUCCESS", balloon->dataStream);
						}	
						else
						{
							Logger_LogNoticeMessage("PdfGenerator_GenerateBalloons: Read data callback for DataStream %s not done. Stream Not initialized or null", balloon->dataStream);
						}
					}
					else 
					{
						Logger_LogNoticeMessage("PdfGenerator_GenerateBalloons: Read Data not done for DataStream %s as skipDataReadMarker is true", balloon->dataStream);
					}
				}
				else
				{
					Logger_LogErrorMessage("PdfGenerator_GenerateBalloons: Read data callback is missing for DataStream: %s FAILED", balloon->dataStream);
					return FALSE;
				}				
			}

			newGeneratedBalloon = PdfGenerator_WriteBalloon(self, balloon, currentPageTemplate, parentGeneratedBalloon, TRUE, TRUE, FALSE);
			
			// 8.    Recursive call to 1. with parent balloon set as this
			genRes = PdfGenerator_GenerateBalloons(self, balloon, newGeneratedBalloon, level+1);		

			// write balloon borders
			PdfGenerator_DrawBalloonBorders(self, balloon, newGeneratedBalloon);
			balloon->lastGeneratedPageNumber = self->pageNumber;

			if (genRes == GENERATE_BALLOONS_NOT_ENOUGH_SPACE)
			{
				Logger_LogNoticeMessage("PdfGenerator_GenerateBalloons: Static balloon returned NOT ENOUGH SPACE END");
				generatorResultStatic = GENERATE_BALLOONS_NOT_ENOUGH_SPACE;				
			}		
			else if (genRes == GENERATE_BALLOONS_FAILED)
			{
				Logger_LogNoticeMessage("PdfGenerator_GenerateBalloons: Returned GENERATE_BALLOONS_FAILED END", balloon->dataStream);
				return genRes;
			}
		}
	} // foreach static balloon

	generatorResult = GENERATE_BALLOONS_OK;

	// 2. Generate Dynamic balloons 
	for(iter = DLList_Begin(balloons); iter != DLList_End(balloons); iter = iter->next)
	{
		balloon = (struct PdfTemplateBalloon*)iter->data;

		if (!balloon->isStatic)
		{
			// 2.    Check its data stream. If not initialized call InitializeDataStreamCallback. if failed stop 
			if (!balloon->skipDataReadMarker && !self->pageSkipDataReadMarker)
			{			
				if (parentBalloon)
				{
					tmpStr = parentBalloon->dataStream;
				}
				else
				{
					tmpStr = NULL;
				}
				
				if (!PdfGenerator_InitializeDataStream(self, tmpStr, balloon->dataStream))
				{
					Logger_LogErrorMessage("PdfGenerator_GenerateBalloons: InitializeDataStream failed for dynamic balloon. DataStream: %s FAILED", balloon->dataStream);
					return FALSE;
				}
				else 
				{
					Logger_LogNoticeMessage("PdfGenerator_GenerateBalloons: Initializing data stream on dynamic balloon. DataStream: %s SUCCESS", balloon->dataStream);
				}
			}
			
			stream = PdfGenerator_FindDataStream(self, balloon->dataStream);

			if (self->readDataCallback)
			{
				while(1)								
				{
					if (!balloon->skipDataReadMarker && !self->pageSkipDataReadMarker)
					{
						if (!balloon->dataStream)
						{
							break;
						}
						if (!self->readDataCallback(balloon->dataStream))
						{
							break;
						}
					}
					newGeneratedBalloon = PdfGenerator_WriteBalloon(self, balloon, currentPageTemplate, parentGeneratedBalloon, FALSE, TRUE, FALSE);
					if (!newGeneratedBalloon)
					{	
						Logger_LogNoticeMessage("PdfGenerator_GenerateBalloons: Creating new generated balloon failed. DataStream: %s", balloon->dataStream);
						if (parentBalloon)
						{
							// mark parent not to move data and this balloon as we already read some data that was not written
							parentBalloon->skipDataReadMarker = TRUE;
							balloon->skipDataReadMarker = TRUE;
						}		
						else 
						{
							self->pageSkipDataReadMarker = TRUE;
							balloon->skipDataReadMarker = TRUE;
						}
						// we failed generating new balloon as not possible to write new one inside parent Generated Balloon. We need new parent. 
						Logger_LogNoticeMessage("PdfGenerator_GenerateBalloons: DataStream: %s, Returned NOT ENOUGH SPACE END", balloon->dataStream);
						generatorResult = GENERATE_BALLOONS_NOT_ENOUGH_SPACE;
						goto GenerateBottomDocked;						
					}
					else 
					{
						// remove skip data markers when we generated new item correctly
						Logger_LogNoticeMessage("PdfGenerator_GenerateBalloons: DataStream: %s, New generated ballon created.", balloon->dataStream);
						balloon->skipDataReadMarker = FALSE;
						self->pageSkipDataReadMarker = FALSE;
					}					
					// Recursive call to 1. with parent balloon set as this		
					genRes = PdfGenerator_GenerateBalloons(self, balloon, newGeneratedBalloon, level+1);
					// write balloon borders
					PdfGenerator_DrawBalloonBorders(self, balloon, newGeneratedBalloon);
					balloon->lastGeneratedPageNumber = self->pageNumber;

					if (genRes == GENERATE_BALLOONS_NOT_ENOUGH_SPACE)
					{
						// we should continue to next iteration without checking condition and reading data
						Logger_LogNoticeMessage("PdfGenerator_GenerateBalloons: Not Enough Space, continue.", balloon->dataStream);
						continue;
					}	
					else if (genRes == GENERATE_BALLOONS_FAILED)
					{
						Logger_LogNoticeMessage("PdfGenerator_GenerateBalloons: DataStream: %s, Returned GENERATE_FAILED END", balloon->dataStream);
						return genRes;
					}
				}
			}
			else 
			{
				Logger_LogNoticeMessage("PdfGenerator_GenerateBalloons: DataStream: %s, No ReadData callback. Returned GENERATE_FAILED END", balloon->dataStream);
				return GENERATE_BALLOONS_FAILED;
			}

			if (stream)
			{
				stream->initialized = FALSE;
			}
			else 
			{
				Logger_LogNoticeMessage("PdfGenerator_GenerateBalloons: DataStream: %s, No Stream. Returned GENERATE_FAILED END", balloon->dataStream);
				return GENERATE_BALLOONS_FAILED;
			}
			
		}
	}


	// 3. generate bottom docked static balloons
	// move to the last element and perform reverse until end
	iter = DLList_End(balloons); 
	iter = iter->prev;
	for( ; iter != DLList_End(balloons) ; iter = iter->prev)
	{
		balloon = (struct PdfTemplateBalloon*)iter->data;
		// if balloon is static, bottom docked and its parent balloon is not null and can grow
		if (balloon->isStatic && balloon->dockPosition == DOCK_BOTTOM && parentBalloon != NULL && parentBalloon->canGrow)
		{
			// write new balloon but handle it as dynamic balloon
			newGeneratedBalloon = PdfGenerator_WriteBalloon(self, balloon, currentPageTemplate, parentGeneratedBalloon, FALSE, TRUE, TRUE);                    

			// if this balloon is not created that means we are missing it 
			if (!newGeneratedBalloon)
			{	
				Logger_LogNoticeMessage("PdfGenerator_GenerateBalloons: Creating new generated balloon failed. DataStream: %s", balloon->dataStream);
				if (parentBalloon)
				{
					// mark parent not to move data and this balloon as we already read some data that was not written
					parentBalloon->skipDataReadMarker = TRUE;
					balloon->skipDataReadMarker = TRUE;
				}		
				else 
				{
					self->pageSkipDataReadMarker = TRUE;
					balloon->skipDataReadMarker = TRUE;
				}

				if (stream)
				{
					// keep stream initialized as we want more items
					stream->initialized = TRUE;
				}

				// we failed generating new balloon as not possible to write new one inside parent Generated Balloon. We need new parent. 
				Logger_LogNoticeMessage("PdfGenerator_GenerateBalloons: DataStream: %s, Returned NOT ENOUGH SPACE END", balloon->dataStream);
				generatorResult = GENERATE_BALLOONS_NOT_ENOUGH_SPACE;
				goto GenerateBottomDocked;						
			}
			else 
			{
				// remove skip data markers when we generated new item correctly
				Logger_LogNoticeMessage("PdfGenerator_GenerateBalloons: DataStream: %s, New generated ballon created.", balloon->dataStream);
				balloon->skipDataReadMarker = FALSE;
				self->pageSkipDataReadMarker = FALSE;
			}			

			// if this newDynamicBalloon is not generated to be bottom one move it down	        
			genRes = PdfGenerator_GenerateBalloons(self, balloon, newGeneratedBalloon, level+1);			
			PdfGenerator_DrawBalloonBorders(self, balloon, newGeneratedBalloon);
			balloon->lastGeneratedPageNumber = self->pageNumber;

			if (genRes == GENERATE_BALLOONS_NOT_ENOUGH_SPACE)
			{
				Logger_LogNoticeMessage("PdfGenerator_GenerateBalloons - Generate Bottom Docked static ones: DataStream: %s, Returned NOT ENOUGH SPACE END", balloon->dataStream);
			}
			else if (genRes == GENERATE_BALLOONS_FAILED)
			{
				Logger_LogNoticeMessage("PdfGenerator_GenerateBalloons - Generate Bottom Docked static ones: DataStream: %s, No Stream. Returned GENERATE_FAILED END", balloon->dataStream);
				return genRes;
			}
		}
	} // foreach static balloon docked bottom

	
	// 4. Generate top docked balloons
	for(iter = DLList_Begin(balloons); iter != DLList_End(balloons); iter = iter->next)
	{
		balloon = (struct PdfTemplateBalloon*)iter->data;
		// if balloon is static, bottom docked and its parent balloon is not null and can grow
		if (balloon->isStatic && balloon->dockPosition == DOCK_TOP && balloon->hasPrevDynamicTopDocked)
		{
			// write new balloon but handle it as dynamic balloon
			newGeneratedBalloon = PdfGenerator_WriteBalloon(self, balloon, currentPageTemplate, parentGeneratedBalloon, FALSE, TRUE, FALSE);                    

			// if this balloon is not created that means we are missing it 
			if (!newGeneratedBalloon)
			{	
				Logger_LogNoticeMessage("PdfGenerator_GenerateBalloons: Creating new generated balloon failed. DataStream: %s", balloon->dataStream);
				if (parentBalloon)
				{
					// mark parent not to move data and this balloon as we already read some data that was not written
					parentBalloon->skipDataReadMarker = TRUE;
					balloon->skipDataReadMarker = TRUE;
				}		
				else 
				{
					self->pageSkipDataReadMarker = TRUE;
					balloon->skipDataReadMarker = TRUE;
				}

				if (stream)
				{
					// keep stream initialized as we want more items
					stream->initialized = TRUE;
				}
				
				// we failed generating new balloon as not possible to write new one inside parent Generated Balloon. We need new parent. 
				Logger_LogNoticeMessage("PdfGenerator_GenerateBalloons: DataStream: %s, Returned NOT ENOUGH SPACE END", balloon->dataStream);
				generatorResult = GENERATE_BALLOONS_NOT_ENOUGH_SPACE;
				goto GenerateBottomDocked;						
			}
			else 
			{
				// remove skip data markers when we generated new item correctly
				Logger_LogNoticeMessage("PdfGenerator_GenerateBalloons: DataStream: %s, New generated ballon created.", balloon->dataStream);
				balloon->skipDataReadMarker = FALSE;
				self->pageSkipDataReadMarker = FALSE;
			}			

			// if this newDynamicBalloon is not generated to be bottom one move it down	        
			genRes = PdfGenerator_GenerateBalloons(self, balloon, newGeneratedBalloon, level+1);			
			PdfGenerator_DrawBalloonBorders(self, balloon, newGeneratedBalloon);
			balloon->lastGeneratedPageNumber = self->pageNumber;

			if (genRes == GENERATE_BALLOONS_NOT_ENOUGH_SPACE)
			{
				Logger_LogNoticeMessage("PdfGenerator_GenerateBalloons - Generate Bottom Docked static ones: DataStream: %s, Returned NOT ENOUGH SPACE END", balloon->dataStream);
			}
			else if (genRes == GENERATE_BALLOONS_FAILED)
			{
				Logger_LogNoticeMessage("PdfGenerator_GenerateBalloons - Generate Bottom Docked static ones: DataStream: %s, No Stream. Returned GENERATE_FAILED END", balloon->dataStream);
				return genRes;
			}
		}
	} // foreach static balloon docked top


GenerateBottomDocked:

	PdfGenerator_DrawRemainingBalloons(self, newBottomDockedBalloons);
	DLList_Destroy(newBottomDockedBalloons);	

	// return not enough space if static requested more space
	if (generatorResultStatic == GENERATE_BALLOONS_NOT_ENOUGH_SPACE)
	{
		Logger_LogNoticeMessage("PdfGenerator_GenerateBalloons: Returned NOT ENOUGH SPACE because of static");
		return generatorResultStatic;
	}
	else 
	{
		if (generatorResult == GENERATE_BALLOONS_NOT_ENOUGH_SPACE)
		{
			Logger_LogNoticeMessage("PdfGenerator_GenerateBalloons: Returned NOT ENOUGH SPACE");
		}
		else 
		{
			Logger_LogNoticeMessage("PdfGenerator_GenerateBalloons: Returned OK");
		}
		return generatorResult;
	}
}


/* 
	Draw list of balloons. This is intended to be used by static bottom docked ones
*/
void PdfGenerator_DrawRemainingBalloons(struct PdfGenerator *self, struct DLList *remainingBalloons)
{
	struct DLListNode *iter;
	struct PdfGeneratedBalloon *generatedBalloon;

    // 4. generate bottom docked static balloons that are on page             
	for(iter = DLList_Begin(remainingBalloons); iter != DLList_End(remainingBalloons); iter = iter->next)
    {
		generatedBalloon = (struct PdfGeneratedBalloon*)iter->data;
		PdfGenerator_DrawBalloon(self,  generatedBalloon->templateBalloon, generatedBalloon);        
		//	Write balloon borders
		PdfGenerator_DrawBalloonBorders(self, generatedBalloon->templateBalloon, generatedBalloon);

	}
}


/*
  Draw items that are stored on page and are not inside any balloon
*/
int PdfGenerator_DrawPageItems(struct PdfGenerator *self, struct PdfTemplatePage *templatePage, struct PdfPage *page)
{
	struct PdfTemplateBalloonItem *item;
	struct PdfGraphicWriter *graphicWriter;
	struct DLListNode *iter;
	struct TransformationMatrix *transMat;
	struct PdfImage *image;

	// if we don't have current content stream and there are really page items begin new one
	if (!self->currentContentStream)
	{
		self->currentContentStream = PdfContentStream_Begin(self->document, self->useCompression);
	}

	// draw background if available
	if (templatePage->fillColorR != 1.0f || templatePage->fillColorG != 1.0f || templatePage->fillColorB != 1.0f)
	{
		graphicWriter = PdfGraphicWriter_Create(self->currentContentStream->stream->streamWriter);
			PdfGraphicWriter_SetRGBFillColor(graphicWriter, templatePage->fillColorR, templatePage->fillColorG, templatePage->fillColorB);		
			PdfGraphicWriter_SaveGraphicState(graphicWriter);
			PdfGraphicWriter_DrawRectangle(graphicWriter, &self->currentPageBalloon->containerRect, TRUE, FALSE);
			PdfGraphicWriter_RestoreGraphicState(graphicWriter);
		PdfGraphicWriter_Destroy(graphicWriter);
	}

	// draw background image if available
	if (templatePage->imageData && templatePage->imageDataLength > 0)
	{
		// write info to pdf page
		graphicWriter = PdfGraphicWriter_Create(self->currentContentStream->stream->streamWriter);
		{	
			// make image object
			image = PdfDocument_FindImage(self->document, templatePage->imageName);
			if (!image)
			{	
				image = PdfImage_Create(self->document, templatePage->imageName);			
				if (!PdfImage_Write(image, TRUE, templatePage->imageData, templatePage->imageDataLength))
				{
					return TRUE;
				}
			}
			// add to resources
			PdfPageResources_AddImage(self->currentPage->properties.resources, image);
			Logger_LogNoticeMessage("PdfGenerator_DrawPageItems: Added background image to resources");

			transMat = TransformationMatrix_Create();
			PdfGraphicWriter_SaveGraphicState(graphicWriter);
			PdfTemplatePage_GetFullTransformation(templatePage, self->currentPage, transMat, TRUE);
			PdfGraphicWriter_SetImage(graphicWriter, image, 0, page->properties.mediaBox.upperRightY, 
				page->properties.mediaBox.upperRightX, page->properties.mediaBox.upperRightY);
			PdfGraphicWriter_RestoreGraphicState(graphicWriter);
		}
		
		PdfGraphicWriter_Destroy(graphicWriter);	
	}


	for(iter = DLList_Begin(templatePage->items); iter != DLList_End(templatePage->items); iter = iter->next)
	{
		item = (struct PdfTemplateBalloonItem*)iter->data;	
		if (!item->process(item, self, self->currentPageBalloon, self->currentContentStream->stream->streamWriter))
		{
#ifdef _DEBUG
			printf("PdfGenerator: DrawBalloon: Drawing balloon item failed.\n");
#endif
			return FALSE;
		}
	}

	return TRUE;
}

/*
	This will generate fonts and their descriptors. Fonts are already prepared in templates
*/
void PdfGenerator_GenerateFonts(struct PdfGenerator *self)
{
	struct PdfTemplateEmbeddedFont *templateEmbeddedFont;
	struct PdfFont *font;
	struct DLListNode *iterator; 

	// write fonts 
	for(iterator = DLList_Begin(self->pdfTemplate->embeddedTemplateFonts); iterator != DLList_End(self->pdfTemplate->embeddedTemplateFonts); iterator = iterator->next)
	{
		templateEmbeddedFont = (struct PdfTemplateEmbeddedFont*)iterator->data;
		font = PdfFont_CreateFromTemplate(self->document, templateEmbeddedFont);
		// write the font and its descriptors and embedded fonts if required
		PdfFont_Write(font);
	}
}


/***********************************************************************
Algorithm works like this:

***********************************************************************/
int PdfGenerator_GenerateAlgorithm(struct PdfGenerator *self)
{
	int generateEnd = FALSE;
	struct PdfTemplate *pdfTemplate = self->pdfTemplate;
	struct PdfTemplatePage *currentPageTemplate = self->pdfTemplate->page;
	int dynamicBalloonProcessed = FALSE;
	int genRes = GENERATE_BALLOONS_OK;

	
	self->stopGenerating = FALSE;

	// Write all fonts from template and make their descriptors
	Logger_LogNoticeMessage("PdfGenerator_GenerateAlgorithm: Writing fonts and descriptors");
	PdfGenerator_GenerateFonts(self);

	// =================================================
	// NEW PIECE OF CODE USED FOR GENERATOR	

	Logger_LogNoticeMessage("PdfGenerator_GenerateAlgorithm: BEGIN");
	self->currentPage = PdfGenerator_GeneratePageFromTemplate(self, currentPageTemplate);	
	PdfGenerator_DrawPageItems(self, currentPageTemplate, self->currentPage);

	do 
	{
		genRes = PdfGenerator_GenerateBalloons(self, NULL, self->currentPageBalloon, 0);
		if (genRes == GENERATE_BALLOONS_FAILED)
		{
			break;
		}
		else if (genRes == GENERATE_BALLOONS_NOT_ENOUGH_SPACE)
		{
			// if demo then write BACKGROUND TEXT
			if (!self->validSerial)
			{
				PdfGenerator_DrawDemo(self);
			}

			// finish previous page
			PdfContentStream_End(self->currentContentStream);
			PdfContentStream_Write(self->currentContentStream);

			self->currentPage->properties.contentStream = self->currentContentStream;
			PdfPageResources_Write(self->currentPage->properties.resources);
			PdfPage_Write(self->currentPage);

			// set content stream to NULL so it can be later recreated in algorithm
			self->currentContentStream = NULL;

			// need to create new page.
			self->currentPage = PdfGenerator_GeneratePageFromTemplate(self, currentPageTemplate);			
			// draw page items
			PdfGenerator_DrawPageItems(self, currentPageTemplate, self->currentPage);
		}
	} while (genRes != GENERATE_BALLOONS_OK);

	// finish previous page if required
	if (self->currentContentStream)
	{	
		// if demo then write BACKGROUND TEXT
		if (!self->validSerial)
		{
			PdfGenerator_DrawDemo(self);
		}

		PdfContentStream_End(self->currentContentStream);
		PdfContentStream_Write(self->currentContentStream);

		self->currentPage->properties.contentStream = self->currentContentStream;
		PdfPageResources_Write(self->currentPage->properties.resources);
		PdfPage_Write(self->currentPage);
	}


	Logger_LogNoticeMessage("PdfGenerator_GenerateAlgorithm: END");
	return TRUE;
}

/*
	Generate pdf to memory only and return it. 
*/
DLLEXPORT_TEST_FUNCTION char* PdfGenerator_GenerateToMemory(struct PdfGenerator* self, int *outDataSizeFilled)
{
	int res;
	char *resultData;
	struct MemoryWriter *memoryWriter;
	
	*outDataSizeFilled = 0;
	Logger_LogNoticeMessage("PdfGenerator_GenerateToMemory: Generating PDF from template BEGIN");

#ifndef BUILD_DEMO
	if(self->validSerial == FALSE)
	{
		//return TRUE;
	}
#endif

	res = TRUE;

	// check template and data stream correctness
	if (!PdfGenerator_CheckCorrectness(self))
	{
		Logger_LogErrorMessage("PdfGenerator_GenerateToMemory: Invalid Generator FAILED");
		return NULL;
	}

	self->document = PdfDocument_CreatePdfDocumentInMemory();
	PdfDocument_Open(self->document, 0);
	PdfDocument_SetInformationDictionary(self->document, self->pdfTemplate->infoDict);

	if (!PdfGenerator_GenerateAlgorithm(self))
	{
		Logger_LogErrorMessage("PdfGenerator_GenerateToMemory: Generating FAILED");
		res = NULL;
	}

	PdfDocument_Close(self->document);

	// take memory after document is closed as it is full now with complete document
	memoryWriter = (struct MemoryWriter*)self->document->streamWriter;
	resultData = memoryWriter->memory;
	*outDataSizeFilled = memoryWriter->size;

	PdfDocument_Destroy(self->document);

	Logger_LogNoticeMessage("PdfGenerator_GenerateToMemory: Generating PDF from template END");
	return resultData;
}

DLLEXPORT_TEST_FUNCTION int PdfGenerator_GenerateToFile(struct PdfGenerator* self, char* outputFileName)
{		
	int res;

	Logger_LogNoticeMessage("PdfGenerator_GenerateToFile: Generating PDF from template BEGIN");

#ifndef BUILD_DEMO
	if(self->validSerial == FALSE)
	{
		//return TRUE;
	}
#endif

	res = TRUE;

	// check template and data stream correctness
	if (!PdfGenerator_CheckCorrectness(self))
	{
		Logger_LogErrorMessage("PdfGenerator_GenerateToFile: Invalid Generator FAILED");
		return FALSE;
	}

	self->document = PdfDocument_CreatePdfDocument(outputFileName);
	PdfDocument_Open(self->document, 0);
	PdfDocument_SetInformationDictionary(self->document, self->pdfTemplate->infoDict);

	if (!PdfGenerator_GenerateAlgorithm(self))
	{
		Logger_LogErrorMessage("PdfGenerator_GenerateToFile: Generating FAILED");
		res = FALSE;
	}
	PdfDocument_Close(self->document);
	PdfDocument_Destroy(self->document);

	Logger_LogNoticeMessage("PdfGenerator_GenerateToFile: Generating PDF from template END");
	return res;
}




DLLEXPORT_TEST_FUNCTION char PdfGenerator_GetErrors(struct PdfGenerator* self)
{
	return 0;
}


DLLEXPORT void SetResetDataStreamCallback(ResetDataStreamCallback resetDataStreamCallback)
{
	PdfGenerator_SetResetDataStreamCallback(localInstance, resetDataStreamCallback);
}

/* set callbacks */
DLLEXPORT void SetRequestDataCallback(RequestDataCallback requestDataCallback)
{
	PdfGenerator_SetRequestDataCallback(localInstance, requestDataCallback);
}
/* set callbacks */
DLLEXPORT void SetReadDataCallback(ReadDataCallback readDataCallback)
{
	PdfGenerator_SetReadDataCallback(localInstance, readDataCallback);
}

/* set callbacks */
DLLEXPORT void SetInitializeDataStreamCallback(InitializeDataStreamCallback initializeDataStreamCallback)
{
	PdfGenerator_SetInitializeDataStreamCallback(localInstance, initializeDataStreamCallback);
}
/* set callbacks */



DLLEXPORT_TEST_FUNCTION int CheckSerial(char *companyName, char *serial)
{
	char *tmpCh;
	char *tmpStr20;
	char *tmpStr2;
	char *company;
	char c;
	unsigned int firstCheck = 0, secondCheck = 0, thirdCheck = 0, check;
	int i = 0;
	int size = 0;	
	unsigned int check2 = 0;
	MD5_CTX md5;

	// If we dont have company name or serial return false
	if (!companyName || !serial)
	{
		return FALSE;
	}

	Logger_LogNoticeMessage("Checking Serial");
	tmpCh = MemoryManager_StrDup("         ");
	tmpStr2 = MemoryManager_StrDup("            ");
	tmpStr20 = MemoryManager_StrDup("                       ");

	company = MemoryManager_StrDup(companyName);
	for(i = 0; i < strlen(companyName); i++)
	{
		company[i] = tolower(company[i]);
	}

	MD5Init(&md5);
	MD5Update(&md5, company, strlen(company));
	MD5Final(&md5);

	//	Check if first 16 is exactly MD5 of company	  
	for (i = 2; i < 18; i++)
	{	
		c = serial[i];
		if ((atoi(&c)) != (int)(md5.digest[17-i] % 10))
		{
			return FALSE;
		}		
	}			


	while(tmpCh[0] != '\0')
	{
		tmpCh[0] = serial[i];
		i++;
	}
	size = i;



	if(size <= 23)
	{
		return FALSE;
	}
	
	// first check
	for(i=0; i<3; i++)
	{
		tmpStr2[i] = serial[2+i];
	}
	firstCheck = atoi(tmpStr2);

	MemoryManager_Free(tmpStr2);
	tmpStr2 = MemoryManager_StrDup("         ");
	// second check
	for(i=0; i<2; i++)
	{
		tmpStr2[i] = serial[8+i];
	}
	secondCheck = atoi(tmpStr2);

	MemoryManager_Free(tmpStr2);
	tmpStr2 = MemoryManager_StrDup("         ");
	// third check
	for(i=0; i<4; i++)
	{
		tmpStr2[i] = serial[18+i];
	}
	thirdCheck = atoi(tmpStr2);


	check = (firstCheck*firstCheck) + (secondCheck*secondCheck) + (thirdCheck*thirdCheck);
	check = ~check;
	check = check % 100000;

	i = 0;
	while (serial[i+22] != '\0')
	{
		tmpStr20[i] = serial[22+i];
		i++;
	}
	
	check2 = atoi(tmpStr20);
	if(check == check2)
	{
		return TRUE;
	}

	return FALSE;
}


DLLEXPORT void SetLogging(short enable, short logLevel)
{	
	Logger_Initialize(logLevel);
	Logger_EnableLogging(enable);
}

DLLEXPORT short InitializeGenerator(char *companyName, char *serial)
{
	Logger_LogNoticeMessage("Initializing Generator");
	localInstance = PdfGenerator_Create();
	localInstance->useCompression = TRUE;

	if(CheckSerial(companyName, serial) == TRUE)
	{
		Logger_LogNoticeMessage("Serial Checked and is valid");
		localInstance->validSerial = TRUE;
	}else{
		Logger_LogNoticeMessage("Serial Checked and is invalid");
		localInstance->validSerial = FALSE;
	}

	return localInstance->validSerial;
}



DLLEXPORT void ShutdownGenerator()
{
	PdfGenerator_Destroy(localInstance);
	Logger_LogNoticeMessage("Generator shutdown");
}

DLLEXPORT int AttachTemplateFromFile(char *templateName)
{
	return PdfGenerator_AttachTemplateFromFile(localInstance, templateName);
}

DLLEXPORT int AttachTemplateFromMemory(char *templateData, int templateDataSize)
{
	return PdfGenerator_AttachTemplateFromMemory(localInstance, templateData, templateDataSize);
}


DLLEXPORT int GenerateToFile(char* outputFileName)
{	
	return PdfGenerator_GenerateToFile(localInstance, outputFileName);
}

DLLEXPORT char* GenerateToMemory(int *outDataSize)
{
	return PdfGenerator_GenerateToMemory(localInstance, outDataSize);
}
