
#include <PdfFactory.h>
#include <PdfDocument.h>
#include <NameObject.h>
#include <ArrayObject.h>
#include <DictionaryObject.h>
#include <PdfContentStream.h>
#include <Rectangle.h>
#include <PdfPage.h>
#include <StringObject.h>
#include <PdfFont.h>
#include <PdfCIDFont.h>
#include <PdfCIDSystemInfo.h>
#include <PdfFontDescriptor.h>
#include <PdfDescendantFonts.h>
#include <PdfCMap.h>
#include <PdfTextWriter.h>
#include <PdfPageResources.h>
#include <stdio.h>
#include <stdlib.h>
#include <PdfTemplate.h>
#include <UnitConverter.h>
#include <PdfGeneratorDataStream.h>
#include <PdfGeneratorDataStreamField.h>
#include <PdfGenerator.h>
#include <GraphicWriter.h>
#include <MemoryWriter.h>
#include <MemoryManager.h>
#include <DLList.h>
#include <string.h>
#include <assert.h>
#include <PdfTemplateItemStaticImage.h>
#include <PdfTemplateItemDateTime.h>
#include <PdfTemplateItemCounter.h>
#include <PdfImage.h>
#include <PdfBaseObject.h>
#include <PdfGraphicPattern.h>
#include <PdfShadingDictionary.h>
#include <PdfFunction.h>
#include <PdfGeneratedBalloon.h>
#include <RectangleNormal.h>
#include <Logger.h>
#include <PdfPrecalculatedItem.h>
#include <Base64Decoder.h>
#include "zlib.h"
#include <md5.h>
#include <time.h>

#if defined(MSDOS) || defined(OS2) || defined(WIN32) || defined(__CYGWIN__)
#  include <fcntl.h>
#  include <io.h>
#  define SET_BINARY_MODE(file) setmode(fileno(file), O_BINARY)
#else
#  define SET_BINARY_MODE(file)
#endif

#define CHUNK 16384


struct PdfDocument* document;

void TestPageResources()
{
	struct PdfFont *font;
	struct PdfPageResources *resources;
	document = PdfDocument_CreatePdfDocument("test_page_resources.pdf");
	PdfDocument_Open(document, 0);

	font = PdfFont_Create(document);
	PdfFont_SetType1Font(font, "Helvetica");
	PdfFont_SetEncoding(font, PDF_MAC_ROMAN_ENCODING);
	PdfFont_Write(font);

	resources = PdfPageResources_Create(document);
	PdfPageResources_AddFont(resources, font);
	PdfPageResources_Write(resources);
	
	PdfDocument_Close(document);
	PdfDocument_Destroy(document);
	printf("Success page resources!\n");
}

void TestFont()
{
	struct PdfFont *font;
	document = PdfDocument_CreatePdfDocument("test_font.pdf");
	PdfDocument_Open(document, 0);

	font = PdfFont_Create(document);
	PdfFont_SetType1Font(font, "Helvetica");
	PdfFont_SetEncoding(font, PDF_MAC_ROMAN_ENCODING);
	PdfFont_Write(font);

	PdfDocument_Close(document);
	PdfDocument_Destroy(document);
	printf("Success font!\n");
}

void TestRectangle()
{
	struct Rectangle *rect;
	document = PdfDocument_CreatePdfDocument("test_rectangle.pdf");
	PdfDocument_Open(document, 0);

	rect = Rectangle_Create(document, 0, 0, 600, 600);
	Rectangle_Write(rect);
	Rectangle_Destroy(rect);
	

	PdfDocument_Close(document);
	PdfDocument_Destroy(document);
	printf("Success rectangle!\n");
}

/*
  Test page creation with usage of TextWriter

*/
void TestPage()
{
	struct PdfContentStream *content;
	struct PdfPage *page;
	struct PdfPageProperties pageProps, pageProps2;
	struct PdfTextWriter *writer;
	struct PdfGraphicWriter *gWriter;
	struct PdfFont *font;
	struct PdfPageResources *resources;
	struct PdfCIDFont *CIDFont;
	struct PdfFontDescriptor *descriptor;
	struct PdfImage *image;
	int i, px, py;
	
	document = PdfDocument_CreatePdfDocument("test_page.pdf");
	PdfDocument_Open(document, 0);

		// create font
	printf("Create font for pdf document\n");
	font = PdfFont_Create(document);
	PdfFont_SetType1Font(font, "Arial");
	PdfFont_SetEncoding(font, PDF_WIN_ANSI_ENCODING);
	PdfFont_Write(font);

	/*image = PdfImage_Create(document, "himen.jpg");
	PdfImage_Write(image);*/
	
	
	printf("Creating content streams...\n");
	content = PdfContentStream_Begin(document, TRUE);
	{
		//gWriter = PdfGraphicWriter_Create(content->streamWriter);
		//PdfGraphicWriter_SetImageTransformed(gWriter, image, 300, 300, 300, 300, 45);
		//PdfGraphicWriter_Destroy(gWriter);
		writer = PdfTextWriter_Begin(content->stream->streamWriter);
		{
			PdfTextWriter_SetFont(writer, font, "24pt");
			PdfTextWriter_SetPosition(writer, 400, 500);
			PdfTextWriter_WriteText(writer, "TOPIC");
			

			PdfTextWriter_SetFont(writer, font, "12pt");
			// write ten lines of text
			px = 0;
			py = -14;
			PdfTextWriter_SetPosition(writer, -250, -50);
			for(i = 0; i < 20; i++)
			{
				PdfTextWriter_SetPosition(writer, px, py);
				PdfTextWriter_WriteText(writer, "Hello World! This is some text written here");	
			}
			
		}
		PdfTextWriter_EndText(writer);	
	
	}
	PdfContentStream_End(content);	
	PdfContentStream_Write(content);


	printf("Create page resources...\n");
	resources = PdfPageResources_Create(document);
	PdfPageResources_AddFont(resources, font);
	//PdfPageResources_AddImage(resources, image);
	PdfPageResources_Write(resources);
		
	pageProps2.contentStream = content;
	pageProps2.resources = resources;
	pageProps2.mediaBox.lowerLeftX = 0;
	pageProps2.mediaBox.lowerLeftY = 0;
	pageProps2.mediaBox.upperRightX = 600;
	pageProps2.mediaBox.upperRightY = 600;	
	
	page = PdfDocument_CreatePage(document, &pageProps2);
	PdfPage_Write(page);

	PdfDocument_Close(document);
	PdfDocument_Destroy(document);	
	printf("Success page!\n");
}

void TestGraphicWriter()
{
	struct PdfContentStream *content;
	struct PdfPage *page;
	struct PdfPageProperties pageProps;
	struct PdfGraphicWriter *writer;
	struct PdfPageResources *resources;
	struct PdfTextWriter *textWriter;
	struct PdfImage *Image;
	
	
	printf("TestGraphuc Writer...\n");
	document = PdfDocument_CreatePdfDocument("test_graphic_writer.pdf");
	PdfDocument_Open(document, 0);
	
	//============== Add by Toma Static Image =========================
	

	Image = PdfImage_Create(document, "himen.jpg");
	//PdfImage_Init(Image,document,283,283,DEVICE_RGB,8,"logo.bmp");
	/*Image->baseImage = "logo.bmp";
	Image->Name = "Im1";
	Image->Width = 10;
	Image->Height = 10;
	Image->ColorSpace = "DeviceRGB";
	Image->BitsPerComponent = 8;
	Image->Length = 0;
	Image->Subtype = "Image";
	Image->FFilter = "DTCDecode";*/

	writer = PdfGraphicWriter_Create(document);
	PdfImage_Write(Image,0,NULL,0);
	//==================================================================
	
	printf("Creating content streams...\n");
	content = PdfContentStream_Begin(document, 0);
	
	printf("Seting Image...\n");
	PdfGraphicWriter_SetImage(writer, Image, 10, 10, Image->width, Image->height);

	printf("Drawing line...\n");
	writer = PdfGraphicWriter_Create(content->stream->streamWriter);
	PdfGraphicWriter_DrawLine(writer, 50, 250, 150, 300);

	printf("Drawing circle...");
	{
		PdfGraphicWriter_SetRGBStrokeColor(writer, 1.0, 0.0, 0.0);
		//PdfGraphicWriter_SetCMYKStrokeColor(writer, 0.0, 0.0, 1.0, 0.0);
		PdfGraphicWriter_SetRGBFillColor(writer, 0.5, 0.75, 1.0);
		PdfGraphicWriter_SetCMYKFillColor(writer, 0.0, 0.0, 1.0, 0.0);
		//Ponisti boje
		//PdfGraphicWriter_ResetColors(writer);
		//PdfGraphicWriter_DrawCircle(writer, 200, 200, 150);
		PdfGraphicWriter_SetLineWidth(writer, 0);
		PdfGraphicWriter_DrawRectangle(writer, Rectangle_Create(document, 0, 300, 100, 0), 0, 1);

		PdfGraphicWriter_Destroy(writer);
	}	
	PdfContentStream_End(content);
	PdfContentStream_Write(content,0);

	printf("Create page resources...\n");
	resources = PdfPageResources_Create(document);
	PdfPageResources_Write(resources);
			
	printf("Creating page for content stream..\n");
	pageProps.contentStream = content;
	pageProps.resources = resources;
	pageProps.mediaBox.lowerLeftX = 0;
	pageProps.mediaBox.lowerLeftY = 0;
	pageProps.mediaBox.upperRightX = 400;
	pageProps.mediaBox.upperRightY = 400;

	page = PdfDocument_CreatePage(document, &pageProps);
	PdfPage_Write(page);	

	PdfDocument_Close(document);
	PdfDocument_Destroy(document);
	printf("TestGraphuc Writer PASSED!\n");

}




void TestSyntaxFactory()
{
	struct StringObject *s;

	document = PdfDocument_CreatePdfDocument("test_syntaxfactory.pdf");
	PdfDocument_Open(document, 0);

	s = StringObject_Create(document, "(ovo je moje");
	StringObject_Write(s);
	StringObject_Destroy(s);

	PdfDocument_Close(document);
	PdfDocument_Destroy(document);
	printf("Success rectangle!\n");
}


void PrintElementNames(xmlNode * root)
{
    xmlNode *currentNode = NULL;
	xmlAttr *currentNodeProperty = NULL;

    for (currentNode = root; currentNode; currentNode = currentNode->next) 
	{
        if (currentNode->type == XML_ELEMENT_NODE)
		{
			
            printf("\n%s \n", currentNode->name);
								
			if (currentNode->properties != NULL)
			{
				currentNodeProperty = currentNode->properties;
				while (currentNodeProperty != NULL)
				{
					printf("\t%s : ", currentNodeProperty->name);
					printf("%s \n", currentNodeProperty->children->content);					
					currentNodeProperty = currentNodeProperty->next;	
				}
			}
		}
        PrintElementNames(currentNode->children);
    }
}

/*
  Test if data stream works correctly
*/
void TestDataStream()
{
	struct PdfGeneratorDataStream *stream; 
	struct PdfGeneratorDataStreamRow *row;
	struct PdfGeneratorDataStreamField *field;
	struct PdfGeneratorDataStreamColumn *col, *colName, *colTel, *colId;	
	struct DLListNode *iter;
	char *tmp;

	printf("Testing DataStream...\n");

	stream = PdfGeneratorDataStream_Create("Persons");

	// make persons table (stream). 

	colName = PdfGeneratorDataStream_AddColumn(stream, COLUMN_TYPE_STRING, 100, "Name");
	colTel = PdfGeneratorDataStream_AddColumn(stream, COLUMN_TYPE_STRING, 100, "Tel");
	colId = PdfGeneratorDataStream_AddColumn(stream, COLUMN_TYPE_STRING, 10, "Id");

	// add rows 
	row = PdfGeneratorDataStream_AddRow(stream);
	PdfGeneratorDataStreamRow_AddFieldByColumn(row, colName, "Djole");
	PdfGeneratorDataStreamRow_AddField(row, stream, "Tel", "654-554");
	PdfGeneratorDataStreamRow_AddField(row, stream, "Id", "1");

	row = PdfGeneratorDataStream_AddRow(stream);
	PdfGeneratorDataStreamRow_AddField(row, stream, "Name", "Pera");
	PdfGeneratorDataStreamRow_AddField(row, stream, "Tel", "555-111");
	PdfGeneratorDataStreamRow_AddField(row, stream, "Id", "2");

	row = PdfGeneratorDataStream_AddRow(stream);
	PdfGeneratorDataStreamRow_AddField(row, stream, "Name", "Mika");
	PdfGeneratorDataStreamRow_AddField(row, stream, "Tel", "063/33455-55");
	PdfGeneratorDataStreamRow_AddField(row, stream, "Id", "3");

	// check and print data 
	// print columns
	for(iter = DLList_Begin(stream->columns); iter != DLList_End(stream->columns); iter = iter->next)
	{
		col = (struct PdfGeneratorDataStreamColumn *)iter->data;
		printf("%s \t", col->name);
	}
	printf("\n");

	PdfGeneratorDataStream_First(stream);
	while (!PdfGeneratorDataStream_End(stream))
	{
		row = PdfGeneratorDataStream_GetCurrentRow(stream);
		// print fields
		for(iter = DLList_Begin(row->fields); iter != DLList_End(row->fields); iter = iter->next)
		{
			field = (struct PdfGeneratorDataStreamField *)iter->data;
			printf("%s\t", (char *)field->value);
		}
		printf("\n");
		PdfGeneratorDataStream_Next(stream);
	}

	row = PdfGeneratorDataStream_GetRow(stream, 2);
	
	tmp = PdfGeneratorDataStreamRow_GetFieldValue(row, "Name");
	if (strcmp(tmp, "Mika") != 0)
	{
		printf("Row value was %s, and expected was: %s\n", tmp, "Mika");
		printf("DataStream Test FAILED!\n");	
		
	}
	else 
	{
		printf("DataStream Test PASSED!\n");	
	}
	
}


void TestPdfTemplate()
{
	struct PdfTemplate* temp;
	struct DLListNode *iter, *iter2;
	struct PdfTemplateDataStream* dataStream;
	struct PdfTemplateColumn* column;
	struct PdfTemplateConnection* con;
	struct PdfTemplateBalloon* balloon;
	struct PdfTemplateBalloonItem *item;
	struct PdfTemplateItemStaticText *itemStaticText;
	struct PdfTemplateItemDynamicText *itemDynamicText;
	struct PdfTemplateItemStaticImage *itemStaticImage;
	int i;
	
	temp = PdfTemplate_Create();
	
	if (!PdfTemplate_Load(temp, "fonttest.prtp"))
	{
		printf("Cannot load file fonttest.prtp\n");
		getch();
		return;
	}
	
/*	printf("HEADER\n");
	printf("\tversion: %f\n\n", temp->header->version);
	printf("\tTEMPLATE INFO\n"); 
	printf("\t\tauthor: %s\n", temp->header->info->author);
	printf("\t\tdate: %s\n\n", temp->header->info->date);
	printf("DATA_STREAMS\n");
	printf("\tversion: %f\n\n", temp->dataStreams->version);
	//Ispisivanje svih DataStream-ova
	for(iter = DLList_Begin(temp->dataStreams->streams); iter != DLList_End(temp->dataStreams->streams); iter = iter->next)
	{
		dataStream = (struct PdfTemplateDataStream *)iter->data;
		printf("\tDATA_STREAM\n");
		printf("\t\tname: %s\n", dataStream->name);
		for(iter2 = DLList_Begin(dataStream->columns); iter2 != DLList_End(dataStream->columns); iter2 = iter2->next)
		{
			column = (struct PdfTemplateColumn*)iter2->data;
			printf("\t\tCOLUMN\n");
			printf("\t\t\tname: %s\n", column->name);
			printf("\t\t\ttype: %s\n", column->type);
		}
	}	
	
	//Ispisivanje connection-a
	for(iter = DLList_Begin(temp->dataStreams->connections); iter != DLList_End(temp->dataStreams->connections); iter=  iter->next)
	{
		con = (struct PdfTemplateConnection*)iter->data;
		printf("\t\tCONNECTION\n");
		printf("\t\t\tparent data stream: %s\n", con->parentDataStream);
		printf("\t\t\tparent column: %s\n", con->parentColumn);
		printf("\t\t\tchild data stream: %s\n", con->childDataStream);
		printf("\t\t\tchild column: %s\n", con->childColumn);
	}


	//Page
	printf("\tPAGE\n");
	printf("\t\tversion: %f\n", temp->page->version);
	printf("\t\tINFO\n");
	printf("\t\t\tdescription: %s\n", temp->page->info->description);
	printf("\t\tSIZE\n");
	printf("\t\t\tWidth: %s\n", temp->page->size->width);
	printf("\t\t\tHeight: %s\n", temp->page->size->height);

	//Ispisivanje balloon-a
	printf("\tBALLOONS\n");		
	for(iter = DLList_Begin(temp->page->balloons); iter != DLList_End(temp->page->balloons); iter= iter->next)
	{
		balloon = (struct PdfTemplateBalloon*)iter->data;		
		printf("\t\tBALLOON\n");
		printf("\t\t\tname: %s\n", balloon->name);
		printf("\t\t\ttype: %s\n", balloon->type);
		printf("\t\t\tdata stream: %s\n", balloon->dataStream);
		printf("\t\t\tversion: %s\n", balloon->version);
		printf("\t\t\tavailable on every page: %d\n", balloon->availableOnEveryPage);
		printf("\t\t\tfilling generates new: %d\n", balloon->fillingGeneratesNew);
		printf("\t\t\tfill capacity: %d\n", balloon->fillCapacity);
		printf("\t\t\tcan grow: %d\n\n", balloon->canGrow);
		printf("\t\t\tLOACTION\n");
		printf("\t\t\t\tposition x: %s\n", balloon->location->positionX);
		printf("\t\t\t\tposition y: %s\n", balloon->location->positionY);
		printf("\t\t\tSHAPE\n");
		printf("\t\t\t\ttype: %s\n\n", balloon->shape->type);
		printf("\t\t\t\tDIMENSIONS\n");		
		printf("\t\t\t\t\twidth: %s\n", balloon->shape->dimensions->width);
		printf("\t\t\t\t\theight: %s\n", balloon->shape->dimensions->height);
		printf("\t\t\tITEMS\n");
		for(iter2 = DLList_Begin(balloon->items); iter2 != DLList_End(balloon->items); iter2 = iter2->next)
		{		
			item = (struct PdfTemplateBalloonItem*)iter2->data;
			if (strcmp(item->type, BALLOON_ITEMTYPE_STATICTEXT) == 0)
			{
				itemStaticText = (struct PdfTemplateItemStaticText*)item;
				printf("\t\t\t\tITEM\n");
				printf("\t\t\t\t\ttype: %s\n", item->type);
				printf("\t\t\t\t\tversion: %f\n", item->version);
				if (itemStaticText->text != NULL)
					printf("\t\t\t\t\ttext: %s\n", itemStaticText->text);
				printf("\n\t\t\t\t\tLOCATION\n");
				printf("\t\t\t\t\t\tposition x: %s\n", item->location->positionX);
				printf("\t\t\t\t\t\tposition y: %s\n", item->location->positionY);
				printf("\n\n\t\t\t\t\tFONT\n");
				printf("\t\t\t\t\t\tname: %s\n", itemStaticText->font->name);			
				printf("\t\t\t\t\t\tsize: %s\n", itemStaticText->font->size);
				printf("\t\t\t\t\t\tcolorR: %d\n", itemStaticText->font->colorR);
				printf("\t\t\t\t\t\tcolorG: %d\n", itemStaticText->font->colorG);
				printf("\t\t\t\t\t\tcolorB: %d\n", itemStaticText->font->colorB);
				printf("\t\t\t\t\t\n");				
			}	
			if (strcmp(item->type, BALLOON_ITEMTYPE_DYNAMICTEXT) == 0)
			{
				itemDynamicText = (struct PdfTemplateItemDynamicText*)item;
				printf("\t\t\t\tITEM\n");
				printf("\t\t\t\t\ttype: %s\n", item->type);
				printf("\t\t\t\t\tversion: %f\n", item->version);
				if (itemDynamicText->sourceColumn != NULL)
					printf("\t\t\t\t\tSource Column: %s\n", itemDynamicText->sourceColumn);
				printf("\n\t\t\t\t\tLOCATION\n");
				printf("\t\t\t\t\t\tposition x: %s\n", item->location->positionX);
				printf("\t\t\t\t\t\tposition y: %s\n", item->location->positionY);
				printf("\n\n\t\t\t\t\tFONT\n");
				printf("\t\t\t\t\t\tname: %s\n", itemDynamicText->font->name);			
				printf("\t\t\t\t\t\tsize: %s\n", itemDynamicText->font->size);
				printf("\t\t\t\t\t\tcolorR: %d\n", itemDynamicText->font->colorR);
				printf("\t\t\t\t\t\tcolorG: %d\n", itemDynamicText->font->colorG);
				printf("\t\t\t\t\t\tcolorB: %d\n", itemDynamicText->font->colorB);

				printf("\t\t\t\t\t\n");				
			}	
			//====== Add by Toma Static Image =======================================
			if (strcmp(item->type, BALLOON_ITEMTYPE_STATICIMAGE) == 0)
			{
				itemStaticImage = (struct PdfTemplateItemStaticImage*)item;
				printf("\t\t\t\tITEM\n");
				printf("\t\t\t\t\ttype: %s\n", item->type);
				printf("\t\t\t\t\tversion: %f\n", item->version);
				printf("\n\t\t\t\t\tLOCATION\n");
				printf("\t\t\t\t\t\tposition x: %s\n", item->location->positionX);
				printf("\t\t\t\t\t\tposition y: %s\n", item->location->positionY);
				printf("\n\t\t\t\t\tIMAGE DESCRIPTION\n");
				printf("\t\t\t\t\t\tSRC NAME: %s\n", itemStaticImage->name);
				printf("\t\t\t\t\t\tCOLOR SPACE: %s\n", itemStaticImage->colorSpace);
				printf("\t\t\t\t\t\tBITS PER COMPONENT: %s\n", itemStaticImage->bitsPerComponent);
			}	
			//=======================================================================
		}
	}	

	PdfTemplate_Destroy(temp);*/
	printf("Test PdfTemplate PASSED!\n");
	getch();
}


void TestXMLParser()
{
    xmlDoc *doc = NULL;
    xmlNode *rootElement = NULL;
  
    doc = xmlReadFile("test.xml", NULL, 0);

    if (doc == NULL) {
        printf("error: could not parse file test.xml\n");
    }

    /*Get the root element node */
    rootElement = xmlDocGetRootElement(doc);

    PrintElementNames(rootElement);

    /*free the document */
    xmlFreeDoc(doc);

    /*
     *Free the global variables that may
     *have been allocated by the parser.
     */
    xmlCleanupParser();

	printf("\nPress any key to exit.");
	getch();    
}

void TestUnitConverter()
{
	struct UnitConverter *uc = UnitConverter_Create();
	double x;
	double eps = 1e-6;
	double delta;
	double r1 = 2.0 * (72.0 / 0.0241);
	double r2 = 1.3 * (72.0 / 24.1);

	printf("testing UnitConverter...\n");

	UnitConverter_AddCommonUnits(uc);
	x = UnitConverter_ConvertToPoints(uc, "2m");
	delta = x - r1;
	delta = delta >= 0 ? delta : -delta;
	if (delta > eps)
	{
		printf("UnitConverter test 1 FAILED!\n");
		exit(1);
	}
	x = UnitConverter_ConvertToPoints(uc, "1.3 mm");
	delta = x - r2;
	delta = delta >= 0 ? delta : -delta;
	if (delta > eps)
	{
		printf("UnitConverter test 2 FAILED!\n");
		exit(1);
	}

	x = UnitConverter_ConvertToPoints(uc, "24pt");
	if (x != 24.0f)
	{
		printf("UnitConverter test 3 FAILED!\n");
		exit(1);
	}

	x = UnitConverter_ConvertToPoints(uc, "12in");
	if (x != (12.0*72.0))
	{
		printf("Unit Converter test 4 FAILED: x was %3.3f!\n", x);
		exit(1);
	}

	UnitConverter_Destroy(uc);
	printf("UnitConverter test PASSED!\n");
}


#define GENERATE_ITEMS_PERSONS 10
#define GENERATE_ITEMS_PHONES 3


char generatorTestValuesPerson[GENERATE_ITEMS_PERSONS][30];
int generatorTestPosPerson = 0;
char generatorTestValuesPhone[GENERATE_ITEMS_PHONES][30];
int generatorTestPosPhone = 0;

char * _stdcall RequestData(char *streamName, char *column, int *dataSize);
int  _stdcall InitializeDataStream(char *parent, char *streamName, int *count);
int _stdcall ReadData(char *streamName);


int _stdcall ReadData(char *streamName)
{
	//Varijanta 1
	if(strcmp(streamName, "Phonebook") == 0)
	{
		generatorTestPosPhone++;
		if (generatorTestPosPhone == GENERATE_ITEMS_PHONES)
		{
			return FALSE;
		}

		generatorTestPosPerson++;
		if (generatorTestPosPerson == GENERATE_ITEMS_PERSONS)
		{
			return FALSE;
		}
	}

	//Varijanta 2
	if (strcmp(streamName, "Phone") == 0)
	{
		generatorTestPosPhone++;
		if (generatorTestPosPhone == GENERATE_ITEMS_PHONES)
		{
			return FALSE;
		}
	}
	else if (strcmp(streamName, "Person") == 0)
	{
		generatorTestPosPerson++;
		if (generatorTestPosPerson == GENERATE_ITEMS_PERSONS)
		{
			return FALSE;
		}
	}	
	return TRUE;
}


char * _stdcall RequestData(char *streamName, char *column, int *dataSize)
{	
	//Varijanta 1
	if(strcmp(streamName, "Phonebook") == 0)
	{
		if (strcmp(column, "Phone") == 0)
		{
			return generatorTestValuesPhone[generatorTestPosPhone];
		}
		else if (strcmp(column, "Person") == 0)
		{
			return generatorTestValuesPerson[generatorTestPosPerson];
		}	
	}

	//Varijanta 2
	if (strcmp(streamName, "Phone") == 0)
	{
		return generatorTestValuesPhone[generatorTestPosPhone];
	}
	else if (strcmp(streamName, "Person") == 0)
	{
		return generatorTestValuesPerson[generatorTestPosPerson];
	}	
	return NULL;
}

int  _stdcall InitializeDataStream(char *parent, char *streamName, int *count)
{
	int i;

	//Varijanta 2
	if (strcmp(streamName, "Phone") == 0)
	{
		// you could for example do here something like: 
		// "Select * from phone where userId= " + currentUser["id"].ToString()
		for(i = 0; i < GENERATE_ITEMS_PHONES; i++)
		{
			sprintf(generatorTestValuesPhone[i], "Person %d, Phone %d", generatorTestPosPerson, i);		
		}
		generatorTestPosPhone = -1;		
	}
	else if (strcmp(streamName, "Person") == 0)
	{
		// for example could go something like this
		// "Select * from person"

		for(i = 0; i < GENERATE_ITEMS_PERSONS; i++)
		{
			sprintf(generatorTestValuesPerson[i], "Person %d", i);		
		}
		generatorTestPosPerson = -1;		
	}	


	//Varijanta 1
	if(strcmp(streamName, "Phonebook") == 0)
	{
		for(i = 0; i < GENERATE_ITEMS_PHONES; i++)
		{
			sprintf(generatorTestValuesPhone[i], "Phone %d", i);		
		}
		generatorTestPosPhone = -1;	

		for(i = 0; i < GENERATE_ITEMS_PERSONS; i++)
		{
			sprintf(generatorTestValuesPerson[i], "Person %d", i);		
		}
		generatorTestPosPerson = -1;
	}
	
	return TRUE;
}

void TestPdfGenerator()
{
	char *tmpSerial;
	char *resultMemory;
	int resultSize;
	char *templateMemory;
	int fLen;
	char fileName[100];
	FILE *f;
	struct PdfGenerator *generator;
	struct PdfGeneratorDataStream *stream;
	struct PdfGeneratorDataStreamRow *row;
	struct PdfGeneratorDataStreamColumn *col;
	
	printf("TestPdfGenerator...\n");	

	//tmpSerial = MemoryManager_StrDup("AC128088826709609258610237");
	//InitializeGenerator(FALSE, tmpSerial);
	generator = PdfGenerator_Create();
	generator->useCompression = FALSE;
	PdfGenerator_SetInitializeDataStreamCallback(generator, InitializeDataStream);
	PdfGenerator_SetReadDataCallback(generator, ReadData);
	PdfGenerator_SetRequestDataCallback(generator, RequestData);
	// input name of project
	printf("Project Template File (with extension):");
	scanf("%s", &fileName);	
	f = fopen(fileName, "rb");
	fseek(f, 0, SEEK_END);
	fLen = ftell(f);
	fseek(f, 0, SEEK_SET);
	templateMemory = malloc(fLen);
	fread(templateMemory, fLen, 1, f);
	fclose(f);

	if (PdfGenerator_AttachTemplateFromMemory(generator, templateMemory, fLen))
	{		
		strcat(fileName, ".pdf");
		//PdfGenerator_GenerateToFile(generator, fileName);
		resultMemory = PdfGenerator_GenerateToMemory(generator, &resultSize);
		if (resultMemory)
		{
			f = fopen(fileName, "wb+");
			fwrite(resultMemory, resultSize, 1, f);
			fflush(f);
			fclose(f);
		}
	}
	

	PdfGenerator_Destroy(generator);

	free(templateMemory);
	printf("TestPdfGenerator test PASSED!\n");
}




int def(FILE *source, FILE *dest, int level)
{
    int ret, flush;
    unsigned have;
    z_stream strm;
    unsigned char in[CHUNK];
    unsigned char out[CHUNK];

    /* allocate deflate state */
    strm.zalloc = Z_NULL;
    strm.zfree = Z_NULL;
    strm.opaque = Z_NULL;
    ret = deflateInit(&strm, level);
    if (ret != Z_OK)
        return ret;

     do {
        strm.avail_in = fread(in, 1, CHUNK, source);
        if (ferror(source)) {
            (void)deflateEnd(&strm);
            return Z_ERRNO;
        }
        flush = feof(source) ? Z_FINISH : Z_NO_FLUSH;
        strm.next_in = in;

        /* run deflate() on input until output buffer not full, finish
           compression if all of source has been read in */
        do {
            strm.avail_out = CHUNK;
            strm.next_out = out;
            ret = deflate(&strm, flush);    /* no bad return value */
            if (ret == Z_STREAM_ERROR) return Z_STREAM_ERROR;
            have = CHUNK - strm.avail_out;
            if (fwrite(out, 1, have, dest) != have || ferror(dest)) {
                (void)deflateEnd(&strm);
                return Z_ERRNO;
            }
        } while (strm.avail_out == 0);
        assert(strm.avail_in == 0);     /* all input will be used */

        /* done when last data in file processed */
    } while (flush != Z_FINISH);
    assert(ret == Z_STREAM_END);        /* stream will be complete */

   (void)deflateEnd(&strm);
    return Z_OK;

}


int inf(FILE *source, FILE *dest)
{
    int ret;
    unsigned have;
    z_stream strm;
    unsigned char in[CHUNK];
    unsigned char out[CHUNK];

    /* allocate inflate state */
    strm.zalloc = Z_NULL;
    strm.zfree = Z_NULL;
    strm.opaque = Z_NULL;
    strm.avail_in = 0;
    strm.next_in = Z_NULL;
    ret = inflateInit(&strm);
    if (ret != Z_OK)
        return ret;
	

    /* decompress until deflate stream ends or end of file */
    do {
        strm.avail_in = fread(in, 1, CHUNK, source);
        if (ferror(source)) {
            (void)inflateEnd(&strm);
            return Z_ERRNO;
        }
        if (strm.avail_in == 0)
            break;
        strm.next_in = in;

        /* run inflate() on input until output buffer not full */
        do {
            strm.avail_out = CHUNK;
            strm.next_out = out;
            ret = inflate(&strm, Z_NO_FLUSH);
            assert(ret != Z_STREAM_ERROR);  /* state not clobbered */
            switch (ret) {
            case Z_NEED_DICT:
                ret = Z_DATA_ERROR;     /* and fall through */
            case Z_DATA_ERROR:
            case Z_MEM_ERROR:
                (void)inflateEnd(&strm);
                return ret;
            }
            have = CHUNK - strm.avail_out;
            if (fwrite(out, 1, have, dest) != have || ferror(dest)) {
                (void)inflateEnd(&strm);
                return Z_ERRNO;
            }
        } while (strm.avail_out == 0);

        /* done when inflate() says it's done */
    } while (ret != Z_STREAM_END);

    /* clean up and return */
    (void)inflateEnd(&strm);
    return ret == Z_STREAM_END ? Z_OK : Z_DATA_ERROR;
}

/* report a zlib or i/o error */
void zerr(int error)
{
    switch (error) 
	{
		case Z_ERRNO:
			if (ferror(stdin))
				printf("error reading stdin\n");
			if (ferror(stdout))
				printf("error writing stdout\n");
			break;
		case Z_STREAM_ERROR:
			printf("invalid compression level\n");
			break;
		case Z_DATA_ERROR:
			printf("invalid or incomplete deflate data\n");
			break;
		case Z_MEM_ERROR:
			printf("out of memory\n");
			break;
		case Z_VERSION_ERROR:
			printf("zlib version mismatch!\n");
	}
}

int fileSize(FILE *file)
{
	int pos;
	int end;

	pos = ftell (file);
	fseek (file, 0, SEEK_END);
	end = ftell (file);
	fseek (file, pos, SEEK_SET);
	
	return end;
}

void TestZlib(const char * inputFileName, const char * zlibFileName, const char * outputFileName, int isBinary)
{
	int errorCode;
	char * readMode, * writeMode;
	FILE * inputFile, * zlibFile, * outputFile;
	if (isBinary)
	{
		readMode = "rb";
		writeMode = "wb";
	}else
	{
		readMode = "r";
		writeMode = "w";
	}
	inputFile = fopen(inputFileName, readMode);
	zlibFile = fopen(zlibFileName, "wb");
	outputFile = fopen(outputFileName, writeMode);
	if (!inputFile)
	{
		printf("Fail to open file...\n");
		getch();
		return;		
	}
	
	/* avoid end-of-line conversions */
    SET_BINARY_MODE(stdin);
    SET_BINARY_MODE(stdout);

	printf("Compressing from input to zlib file...\n");
	errorCode = def(inputFile, zlibFile, Z_DEFAULT_COMPRESSION);
        if (errorCode != Z_OK)
            zerr(errorCode);
        
	fclose(zlibFile);
	zlibFile = fopen("zlib.zlib", "rb");

	printf("Decompressing from zlib to output file...\n\n");
	errorCode = inf(zlibFile, outputFile);
        if (errorCode != Z_OK)
            zerr(errorCode);

	printf("Size of input file : %d bytes\n", fileSize(inputFile));
	printf("Size of zlib file : %d bytes\n", fileSize(zlibFile));
	printf("Size of output file : %d bytes\n", fileSize(outputFile));
        		
	fclose(outputFile);
	fclose(inputFile);
	fclose(zlibFile);
	getch();
}


void TestStreams()
{
	int i;
	int pos;
	struct StreamWriter *writer;
	struct MemoryWriter *memoryWriter;
	

	printf("Testing Streams...\n");
	writer = (struct StreamWriter *)MemoryWriter_Create();
	
	printf("memoryWriter:\n");
	writer->WriteData(writer, "1");
	writer->WriteNewLine(writer);
	writer->WriteData(writer, "2");
	writer->WriteNewLine(writer);
	writer->WriteBinaryData(writer, "1234567890\n", 11);
	pos = writer->GetPosition(writer);
	writer->Seek(writer, 0);
	writer->WriteData(writer, "9");
	writer->Seek(writer, pos);
	writer->WriteData(writer, "Jos jedan red\n");

	

	memoryWriter = (struct MemoryWriter *)writer;


	for(i = 0; i < memoryWriter->size; i++)
	{
		printf("%c", memoryWriter->memory[i]);
	}			
	writer->Destroy(writer);	

	writer = (struct StreamWriter *)FileWriter_CreateFromFile("test.txt", "wt+");
	writer->WriteData(writer, "1");
	writer->WriteNewLine(writer);
	writer->WriteData(writer, "2");
	writer->WriteNewLine(writer);
	writer->WriteBinaryData(writer, "1234567890\n", 11);
	writer->Destroy(writer);

	printf("Testing Streams DONE!\n");
}

void TestImageWriter()
{
	struct PdfContentStream *content;
	struct PdfPage *page;
	struct PdfPageProperties pageProps;
	struct PdfGraphicWriter *writer;
	struct PdfPageResources *resources;
	struct PdfTextWriter *textWriter;
	struct PdfImage *Image;
	
	
	printf("Test Image Writer...\n");
	document = PdfDocument_CreatePdfDocument("test_image_writer.pdf");
	PdfDocument_Open(document, 0);
		
	Image = PdfImage_Create(document, "logo.jpg");
	
	PdfImage_Write(Image,0,NULL,0);
	
	printf("Creating content streams...\n");
	content = PdfContentStream_Begin(document, 0);
	
	printf("Seting Image...\n");
	writer = PdfGraphicWriter_Create(content->stream->streamWriter);
	//PdfGraphicWriter_SetImage(writer, Image, 0, 400);

	
	PdfContentStream_End(content);
	PdfContentStream_Write(content,0);

	printf("Create page resources...\n");
	resources = PdfPageResources_Create(document);
	//PdfPageResources_AddImage(resources, Image);
	PdfPageResources_Write(resources);
			
	printf("Creating page for content stream..\n");
	pageProps.contentStream = content;
	pageProps.resources = resources;
	pageProps.mediaBox.lowerLeftX = 0;
	pageProps.mediaBox.lowerLeftY = 0;
	pageProps.mediaBox.upperRightX = 400;
	pageProps.mediaBox.upperRightY = 400;

	page = PdfDocument_CreatePage(document, &pageProps);
	PdfPage_Write(page);	

	PdfDocument_Close(document);
	PdfDocument_Destroy(document);
	printf("Test Image Writer PASSED!\n");
}



void TestUnicode()
{
	/*struct PdfContentStream *content;
	struct PdfPage *page;
	struct PdfPageProperties pageProps, pageProps2;
	struct PdfTextWriter *writer;
	struct PdfFont *font;
	struct PdfPageResources *resources;
	struct PdfCIDFont *CIDFont;
	struct PdfFontDescriptor *descriptor;
	struct PdfCIDSystemInfo *cidSysInfo;
	struct PdfDescendantFonts *desFonts;
	struct PdfBaseObject base;
	struct DictinaryObject *dict;
	struct PdfCMap *cMap;
	struct StreamObject *stream;
	struct NumberObject *number;
	void *data;
	int size, pos, tmpPos, length, tmpUse;
	char tmpCUse[4];
	FILE *f;
	

	document = PdfDocument_CreatePdfDocument("test_unicode.pdf");
	PdfDocument_Open(document, 0);

	printf("Create DescendantFonts List...\n");
	desFonts = PdfDescendantFonts_Create(document);

	printf("Create FontDescriptor\n");
	descriptor = PdfFontDescriptor_Create(document);
	PdfFontDescriptor_SetFontBBox(descriptor, -600, 0, 600, 300);
	PdfFontDescriptor_SetFontFamily(descriptor, "Arial");
	PdfFontDescriptor_SetFontName(descriptor, "RMOROF+ArialMT");
	PdfFontDescriptor_SetFontFile2(descriptor, 1);
	PdfFontDescriptor_Write(descriptor);

	
	printf("Create CIDSystemInfo...\n");
	cidSysInfo = PdfCIDSystemInfo_Create(document);
	PdfCIDSystemInfo_Write(cidSysInfo);
	
	printf("Create CIDFont\n");
	CIDFont = PdfCIDFont_Create(document);
	PdfCIDFont_SetBaseFont(CIDFont, "RMOROF+ArialMT");
	PdfCIDFont_SetCIDSystemInfo(CIDFont, cidSysInfo->base.objectId);  // reference to PdfCIDSystemInfo object
	PdfCIDFont_SetFontDescriptor(CIDFont, descriptor->base.objectId); // reference to PdfFontDescriptor object
	PdfCIDFont_Write(CIDFont);

	PdfDescendantFonts_AddReference(desFonts, CIDFont->base.objectId); // reference to PdfCIDFont object
	PdfDescendantFonts_Write(desFonts);

	printf("Create CMap...\n");
	cMap = PdfCMap_Create(document);
	PdfCMap_Write(cMap);


	printf("Create font type 0...\n");
	font = PdfFont_Create(document);
	PdfFont_SetType0Font(font, "RMOROF+ArialMT");
	PdfFont_SetToUnicode(font, cMap->base.objectId);			  // reference to PdfCMap object
	PdfFont_SetDescendantFonts(font, desFonts->base.objectId);    // reference to PdfDescendantFonts object
	PdfFont_SetEncoding(font, PDF_IDENTITY_H);
	PdfFont_Write(font);

	printf("Writing content streams...\n");
	content = PdfContentStream_Begin(document, 0);
	{
		writer = PdfTextWriter_Begin(content->stream->streamWriter);
		{
			PdfTextWriter_SetFont(writer, font, "24pt");
			PdfTextWriter_SetPosition(writer, 200, 500);
			//PdfTextWriter_WriteUnicodeText(writer, "TOPIC");
		}
		
		//======this should be placed in WriteUnicodeText()==================
		content->stream->streamWriter->WriteData(content->stream->streamWriter,"<038D> Tj");
		content->stream->streamWriter->WriteNewLine(content->stream->streamWriter);
		//===================================================================
		
		PdfTextWriter_EndText(writer);
	}
	
	PdfContentStream_End(content);	
	PdfContentStream_Write(content,0);


	printf("Create page resources...\n");
	resources = PdfPageResources_Create(document);
	//PdfPageResources_AddFont(resources, font);
	PdfPageResources_Write(resources);
		
	pageProps2.contentStream = content;
	pageProps2.resources = resources;
	pageProps2.mediaBox.lowerLeftX = 0;
	pageProps2.mediaBox.lowerLeftY = 0;
	pageProps2.mediaBox.upperRightX = 600;
	pageProps2.mediaBox.upperRightY = 600;	
	
	printf("Creating content stream...\n");
	pageProps.contentStream = content;
	pageProps.resources = resources;
	pageProps.mediaBox.lowerLeftX = 0;
	pageProps.mediaBox.lowerLeftY = 0;
	pageProps.mediaBox.upperRightX = 1600;
	pageProps.mediaBox.upperRightY = 1600;
	
	page = PdfDocument_CreatePage(document, &pageProps2);
	PdfPage_Write(page);

	PdfDocument_Close(document);
	PdfDocument_Destroy(document);
	printf("Success UNICODE!\n");*/
}



void TestTime()
{
	struct PdfTemplate* temp;
	struct DLListNode *iter, *iter2;
	struct PdfTemplateColumn* column;
	struct PdfTemplateBalloon* balloon;
	struct PdfTemplateBalloonItem *item;
	struct PdfTemplateItemCounter *itemCounter;
	struct PdfTemplateItemDateTime *itemDateTime;

	int i;
	
	temp = PdfTemplate_Create();
	
	if (!PdfTemplate_Load(temp, "template2.xml"))
	{
		printf("Cannot load file template2.xml\n");
		getch();
		return;
	}

	printf("HEADER\n");
	printf("\tversion: %f\n\n", temp->header->version);
	printf("\tTEMPLATE INFO\n"); 
	printf("\t\tauthor: %s\n", temp->header->info->author);
	printf("\t\tdate: %s\n\n", temp->header->info->date);


	//Page
	printf("\tPAGE\n");
	printf("\t\tversion: %f\n", temp->page->version);
	printf("\t\tINFO\n");
	printf("\t\t\tdescription: %s\n", temp->page->info->description);
	printf("\t\tSIZE\n");
	printf("\t\t\tWidth: %s\n", temp->page->size->width);
	printf("\t\t\tHeight: %s\n", temp->page->size->height);

	//Ispisivanje balloon-a
	printf("\tBALLOONS\n");		
	for(iter = DLList_Begin(temp->page->balloons); iter != DLList_End(temp->page->balloons); iter= iter->next)
	{
		balloon = (struct PdfTemplateBalloon*)iter->data;		
		for(iter2 = DLList_Begin(balloon->items); iter2 != DLList_End(balloon->items); iter2 = iter2->next)
		{		
			item = (struct PdfTemplateBalloonItem*)iter2->data;
			if (strcmp(item->type, BALLOON_ITEMTYPE_DATETIME) == 0)
			{
				itemDateTime = (struct PdfTemplateItemDateTime*)item;
				printf("\t\t\t\tITEM\n");
				printf("\t\t\t\t\ttype: %s\n", item->type);
				printf("\t\t\t\t\tversion: %f\n", item->version);
				printf("\n\t\t\t\t\tLOCATION\n");
				printf("\t\t\t\t\t\tposition x: %s\n", item->location->positionX);
				printf("\t\t\t\t\t\tposition y: %s\n", item->location->positionY);
				printf("\n\t\t\tTEXT: %s\n\n", itemDateTime->text);
			}	
			if (strcmp(item->type, BALLOON_ITEMTYPE_COUNTER) == 0)
			{
				itemCounter = (struct PdfTemplateItemCounter*)item;
				printf("\t\t\t\tITEM\n");
				printf("\t\t\t\t\ttype: %s\n", item->type);
				printf("\t\t\t\t\tversion: %f\n", item->version);
				printf("\n\t\t\t\t\tLOCATION\n");
				printf("\t\t\t\t\t\tposition x: %s\n", item->location->positionX);
				printf("\t\t\t\t\t\tposition y: %s\n", item->location->positionY);
				printf("\t\t\t\t\t\t   Start: %d\n", itemCounter->start);
				printf("\t\t\t\t\t\t     End: %d\n", itemCounter->end);
				printf("\t\t\t\t\t\t    Loop: %d\n", itemCounter->loop);
				printf("\t\t\t\t\t\tInterval: %d\n", itemCounter->interval);
				PdfTemplateItemCounter_Update(itemCounter);
				printf("\t\t\t\t\t\tUPDATING COUNTER x1\n\n");
				PdfTemplateItemCounter_MakeText(itemCounter);
				printf("\t\t\tTEXT: %s\n\n", itemCounter->text);
			}
		}
	}	

	//PdfTemplate_Destroy(temp);
	printf("Test \"DateTime\" PASSED!\n");
	getch();
}


void TestCaracterVariations()
{
	struct PdfContentStream *content;
	struct PdfPage *page;
	struct PdfPageProperties pageProps2;
	struct PdfTextWriter *writer;
	struct PdfFont *font;
	struct PdfPageResources *resources;
	struct ArrayObject *arr;
	int i, px, py;
	
	document = PdfDocument_CreatePdfDocument("test_variations.pdf");
	PdfDocument_Open(document, 0);

		// create font
	printf("Create font for pdf document\n");
	font = PdfFont_Create(document);
	PdfFont_SetType1Font(font, "Helvetica");
	PdfFont_SetEncoding(font, PDF_WIN_ANSI_ENCODING);
	PdfFont_Write(font);

	
	printf("Creating content streams...\n");
	content = PdfContentStream_Begin(document, FALSE);
	{
		writer = PdfTextWriter_Begin(content->stream->streamWriter);
		{
			PdfTextWriter_SetFont(writer, font, "20pt");
			PdfTextWriter_SetPosition(writer, 1, 500);
			PdfTextWriter_WriteText(writer, "\"Toma test!\" ~`!@#$%^&*()_+-=,./<>?'\\|{}[]");
		}
		PdfTextWriter_EndText(writer);	
	}
	PdfContentStream_End(content);	
	PdfContentStream_Write(content,1);


	printf("Create page resources...\n");
	resources = PdfPageResources_Create(document);
	PdfPageResources_AddFont(resources, font);
	PdfPageResources_Write(resources);
		
	pageProps2.contentStream = content;
	pageProps2.resources = resources;
	pageProps2.mediaBox.lowerLeftX = 0;
	pageProps2.mediaBox.lowerLeftY = 0;
	pageProps2.mediaBox.upperRightX = 600;
	pageProps2.mediaBox.upperRightY = 600;	
	
	page = PdfDocument_CreatePage(document, &pageProps2);
	PdfPage_Write(page);

	PdfDocument_Close(document);
	PdfDocument_Destroy(document);	
	printf("Success page!\n");
}



void TestPdfGraphicPattern()
{
	struct PdfContentStream *content;
	struct PdfPage *page;
	struct PdfPageProperties pageProps, pageProps2;
	struct PdfPageResources *resources;
	struct PdfGraphicWriter *gWrite;
	struct PdfGraphicPattern *gPattern;
	struct PdfShadingDictionary *shadingDict, *shadingDict2;
	struct Rectangle *rect;
	struct PdfFunction *tmpFunction;
	int i, px, py;
	
	document = PdfDocument_CreatePdfDocument("test_shadings.pdf");
	PdfDocument_Open(document, 0);


	shadingDict = PdfShadingDictionary_Create(document, PDF_SHADING_TYPE_RADIAL, PDF_FUNCTION_TYPE_LISTOFFUNCTIONS, 0, 0, 1.0);
	tmpFunction = PdfFunction_Create(document, PDF_FUNCTION_TYPE_SIMPLEFUNCTION, 1.0); // Create actual function type 2 and set params
	PdfShadingDictionary_AddNextFunction(shadingDict, tmpFunction);
	PdfShadingDictionary_SetRGBStartColor(shadingDict, 0.0, 0.0, 0.0);
	PdfShadingDictionary_SetRGBEndColor(shadingDict, 1.0, 1.0, 1.0);
	tmpFunction = 0;

	tmpFunction = PdfFunction_Create(document, PDF_FUNCTION_TYPE_SIMPLEFUNCTION, 1.0);
	PdfShadingDictionary_AddNextFunction(shadingDict, tmpFunction);
	PdfShadingDictionary_SetRGBStartColor(shadingDict, 1.0, 1.0, 1.0);
	PdfShadingDictionary_SetRGBEndColor(shadingDict, 0.0, 0.0, 0.0);
	tmpFunction = 0;

	

	//PdfShadingDictionary_SetRadialCoords(shadingDict, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0);// please the shading position and size difference
	PdfShadingDictionary_Write(shadingDict, document->streamWriter, FALSE); // Write this shadings in PDF file

	printf("Creating content streams...\n");
	content = PdfContentStream_Begin(document, FALSE);
	{
		gWrite = PdfGraphicWriter_Create(content->stream->streamWriter);
		rect = Rectangle_Create(content->stream->streamWriter, 0.0, 0.0, 300.0, 200.0); 

		PdfGraphicWriter_DrawRectangle(gWrite, rect, 0, 0); //It is important to set color fill and stroke to 0, or it won't work!!!
		PdfGraphicWriter_SetClippingPath(gWrite); // Draw rectangle as clipping path and not as object to be seen.

		PdfGraphicWriter_SetShading(gWrite, shadingDict->name, 150.0, 100.0, 200.0);// Actual position and size of shadings.
		PdfGraphicWriter_Destroy(gWrite);
	}
	PdfContentStream_End(content);	
	PdfContentStream_Write(content,1);


	printf("Create page resources...\n");
	resources = PdfPageResources_Create(document);
	PdfPageResources_AddShadingDictionary(resources, shadingDict); // must be added to resource list!
	PdfPageResources_Write(resources);
		
	pageProps2.contentStream = content;
	pageProps2.resources = resources;
	pageProps2.mediaBox.lowerLeftX = 0;
	pageProps2.mediaBox.lowerLeftY = 0;
	pageProps2.mediaBox.upperRightX = 600;
	pageProps2.mediaBox.upperRightY = 600;	
	
	page = PdfDocument_CreatePage(document, &pageProps2);
	PdfPage_Write(page);

	PdfDocument_Close(document);
	PdfDocument_Destroy(document);	
	printf("Success page!\n");
}



void TestPrecalculatedItem()
{
	double value;
	char *result;
	struct PdfPrecalculatedItem *precItem;

	
	precItem = PdfPrecalculatedItem_Create("SUM(Stream.Column) + (MAX(Stream.Column)) * 10.5");
	PdfPrecalculatedItem_AddValue(precItem, "Stream", "Column", 100.0f);
	PdfPrecalculatedItem_AddValue(precItem, "Stream", "Column", 105.0f);
	PdfPrecalculatedItem_AddValue(precItem, "Stream", "Column", 10.0f);	

	result = PdfPrecalculatedItem_GetResult(precItem);
	printf("%s\n", result);	
}










int _stdcall ReData(char *streamName)
{
	//if (strcmp(streamName, "Data1") == 0)
	//{
	//	generatorTestPosPhone++;
	//	if (generatorTestPosPhone == GENERATE_ITEMS_PHONES)
	//	{
	//		return FALSE;
	//	}
	//}
	//else 
	if (strcmp(streamName, "Data1") == 0)
	{
		generatorTestPosPerson++;
		if (generatorTestPosPerson == GENERATE_ITEMS_PERSONS)
		{
			return FALSE;
		}
	}
	return TRUE;
}


char * _stdcall ReqData(char *streamName, char *column, int *dataSize)
{	
	/*if (strcmp(streamName, "Phone") == 0)
	{
		return generatorTestValuesPhone[generatorTestPosPhone];
	}
	else	*/
	int size = 0;
	char* tmpData = NULL;
	FILE *f = NULL;
	int i;
	int j = 0;

	if (strcmp(streamName, "Data1") == 0 && strcmp(column, "Column1") == 0)
	{
		//return generatorTestValuesPerson[0];
		i = fopen_s(&f,"Proba.bmp","rb");

		if(f)
		{
			fseek(f, 0, SEEK_END);
			size = (int)ftell(f);
			fseek(f, 0, SEEK_SET);
			tmpData = MemoryManager_Alloc(size);
			fread(tmpData, size, 1, f);
			fclose(f);
			
			*dataSize = size;
			return tmpData;
		}else
		{
			return NULL;
		}

		
	}

	if (strcmp(streamName, "Data1") == 0 && strcmp(column, "Column2") == 0)
	{
		//return generatorTestValuesPerson[0];
		i = fopen_s(&f,"Proba.jpg","rb");

		if(f)
		{
			fseek(f, 0, SEEK_END);
			size = (int)ftell(f);
			fseek(f, 0, SEEK_SET);
			tmpData = MemoryManager_Alloc(size);
			fread(tmpData, size, 1, f);
			fclose(f);

			*dataSize = size;
			return tmpData;
		}else
		{
			return NULL;
		}


	}

	if (strcmp(streamName, "Data1") == 0 && strcmp(column, "Column3") == 0)
	{
		//return generatorTestValuesPerson[0];
		i = fopen_s(&f,"Proba.png","rb");

		if(f)
		{
			fseek(f, 0, SEEK_END);
			size = (int)ftell(f);
			fseek(f, 0, SEEK_SET);
			tmpData = MemoryManager_Alloc(size);
			fread(tmpData, size, 1, f);
			fclose(f);

			*dataSize = size;
			return tmpData;
		}else
		{
			return NULL;
		}


	}

	if (strcmp(streamName, "Data1") == 0 && strcmp(column, "Column4") == 0)
	{
		//return generatorTestValuesPerson[0];
		i = fopen_s(&f,"Proba.gif","rb");

		if(f)
		{
			fseek(f, 0, SEEK_END);
			size = (int)ftell(f);	
			fseek(f, 0, SEEK_SET);
			tmpData = MemoryManager_Alloc(size);
			fread(tmpData, size, 1, f);
			fclose(f);

			*dataSize = size;
			return tmpData;
		}else
		{
			return NULL;
		}


	}
	return NULL;
}



int  _stdcall InitDataStream(char *parent, char *streamName, int *count)
{
	//int i;
	//if (strcmp(streamName, "Phone") == 0)
	//{
	//	// you could for example do here something like: 
	//	// "Select * from phone where userId= " + currentUser["id"].ToString()
	//	for(i = 0; i < GENERATE_ITEMS_PHONES; i++)
	//	{
	//		sprintf(generatorTestValuesPhone[i], "Person %d, Phone %d", generatorTestPosPerson, i);		
	//	}
	//	generatorTestPosPhone = -1;		
	//}
	//else 
	//if (strcmp(streamName, "Data1") == 0)
	{
		// for example could go something like this
		// "Select * from person"

		//for(i = 0; i < GENERATE_ITEMS_PERSONS; i++)
		//{
			sprintf(generatorTestValuesPerson[0], "Proba.jpg", 0);		
		//}
		generatorTestPosPerson = -1;		
	}	

	return TRUE;
}

void TestPdfDynamicImage()
{

	struct PdfGenerator *generator;
	struct PdfGeneratorDataStream *stream;
	struct PdfGeneratorDataStreamRow *row;
	struct PdfGeneratorDataStreamColumn *col;

	printf("TestPdfGenerator...\n");	

	generator = PdfGenerator_Create();
	generator->useCompression = FALSE;
	PdfGenerator_SetInitializeDataStreamCallback(generator, InitDataStream);
	PdfGenerator_SetReadDataCallback(generator, ReData);
	PdfGenerator_SetRequestDataCallback(generator, ReqData);
	if (PdfGenerator_AttachTemplateFromFile(generator, "test2.xml"))
	{		
		PdfGenerator_GenerateToFile(generator, "test2.pdf");
	}
	PdfGenerator_Destroy(generator);

	printf("TestPdfGenerator test PASSED!\n");
}







void TestDLListFunctions()
{
	float tmp1=1.0f, tmp2=6.0f, tmp3=3.0f, tmp4=10.0f;
	float *tmpRes;
	struct DLList *List = DLList_Create();
	struct DLListNode *iter;

	DLList_PushBack(List, (void*)&tmp1);
	DLList_PushBack(List, (void*)&tmp2);
	DLList_PushBack(List, (void*)&tmp3);
	DLList_PushBack(List, (void*)&tmp4);

	printf("\n Index of 6: %d", DLList_IndexOf(List, (void*)&tmp2));
	printf("\n Value at index 1: %f", DLList_GetFValueAtIndex(List, 1));
	printf("\n Sorting... \n");
	List = DLList_SortByValue(List);
	printf("\n Index of 6: %d", DLList_IndexOf(List, (void*)&tmp2));
	printf("\n Value at index 1: %f", DLList_GetFValueAtIndex(List, 1));

	printf("\n\n Is contains 3.0f: %s \n", DLList_Contains(List, (void*)&tmp3) == 1 ? "true" : "false");
	printf("\n\n Is contains 0.0f: %s \n", DLList_Contains(List, (void*)&tmpRes) == 1 ? "true" : "false");

	for(iter = DLList_Begin(List); iter != DLList_End(List); iter = iter->next)
	{
		tmpRes = (float*)iter->data;
		printf("\n Val: %f", *tmpRes);
	}
	printf("\n\n");
}


void TestLoger()
{
	Logger_Initialize(LOGGER_LEVEL_NOTICE);
	Logger_EnableLogging(TRUE);
	Logger_LogNoticeMessage("Ovop %s - %d", "kako si", 345);
}

void TestBase64Decoder()
{
	char result[100];
	int i;
	for (i = 0; i < 100; i++) result[i] = 0;

	Base64Decode("b3ZkZSBzZSBrcmlqZSBzdXBlciB0YWpuYSBtYWdpamEgc3ZlZ2Egc3RvIHBvc3Rvamk=\0\0", result, 100);
	
	printf("%s", result);
	getch();
}




int main()
{
	char name[100];
	int i;
	MD5_CTX ctx;
	//TestBase64Decoder();
	//TestPage();	
	//TestTime();
	//TestRectangle();
	//TestFont();
	//TestPageResources();
	//TestSyntaxFactory();
	//TestXMLParser();
	//TestUnitConverter();
	//TestDataStream();
	//TestPdfTemplate();
    //TestPdfGenerator();
	
	
	if (CheckSerial("AxiomCoders", "AC839126801939146330248398"))
	{
		printf("Succeed");
	}

	//TestDLListFunctions();
	//TestPdfDynamicImage();
	//TestGraphicWriter();
	//TestImageWriter();
	//TestStreams();
	//TestZlib("test2.txt", "zlib.zlib", "output.txt", 1);
	//TestGetNewBalloonLocation();
	//readFile();
	//TestUnicode();
	//TestCaracterVariations();
	//TestPdfGraphicPattern();
	//TestPrecalculatedItem();
	//TestLoger();
	

	return 0;	
}
