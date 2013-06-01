/*
    PdfDocument.h

	Author: Nebojsa Vislavski
	Date: 30.6.2008.	
		
	This functions are used as main thing when working with pdf

*/

#ifndef _PDF_DOCUMENT_
#define _PDF_DOCUMENT_

#include "PdfFactory.h"
#include "CrossReferenceTable.h"
#include "PdfTrailer.h"
#include "DLList.h"
#include "PdfCatalog.h"
#include "FileWriter.h"
#include "PdfInformationDictionary.h"
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

struct PdfBaseObject;

#define PDFDOCUMENT_TYPE_FILE 1 // pdf document is written to file
#define PDFDOCUMENT_TYPE_SOCKET 2 // pdf document is written to socket - not implemented yet
#define PDFDOCUMENT_TYPE_MEMORY 3 // pdf document is written to memory stream

struct PdfPageProperties;

struct PdfDocument
{
	int currentObjectId;
	int nextObjectId;
	int documentType;
	struct StreamWriter *streamWriter;	 // used to write pdf info
	
	char fileName[256];

	int rootObjectId;  // where is first object
	int objectsCount;  // number of objects generated. This is increased when object is added

	int majorVersion;  //  currently only 1 is available
	int minorVersion;  // can be from 5 - 7

	struct CrossReferenceTable *crossReferenceTable; // cross reference table
	struct PdfInformationDictionary *informationDictionary;   //Holds information about document
	struct PdfTrailer *trailer; // file trailer 
	struct DLList* indirectObjectList;   // list of all indirect objects with they offsets. Required for cross reference table
	struct DLList* contentStreams; // list of all content streams. We need to keep list of those for dynamic referencing from pages. 
								   // After document closing these streams are destroyed
	struct DLList* fonts;		   // list of all fonts in document. When each font is created it is automatically added to this list
	struct DLList* embeddedFonts;  // list of all embedded fonts in the document
	struct DLList* resources;      // list of all resources used in document. Resources are auto added here when created
	struct DLList* images;         // list of all images in document. When each image is created it is automatically added to this list
	struct DLList* shadings;       // list of all shadings in document. When each shading is created it is automatically added to this list

	struct PdfCatalog *catalog; // main root catalog object
	int nextFontId;
	int nextImageId;
	int nextShadingId;
};


// Creates new pdf document of some type
DLLEXPORT_TEST_FUNCTION struct PdfDocument* PdfDocument_CreatePdfDocument(char *fileName); 
DLLEXPORT_TEST_FUNCTION struct PdfDocument* PdfDocument_CreatePdfDocumentInMemory(); 
DLLEXPORT_TEST_FUNCTION void PdfDocument_Init(struct PdfDocument *self); 

DLLEXPORT_TEST_FUNCTION int PdfDocument_Open(struct PdfDocument* document, int overwrite);  

/* open and create pdf file and write header */

DLLEXPORT_TEST_FUNCTION int PdfDocument_Close(struct PdfDocument* document); 
DLLEXPORT_TEST_FUNCTION void PdfDocument_Destroy(struct PdfDocument* document); 
//DLLEXPORT_TEST_FUNCTION void PdfDocument_WriteData(struct PdfDocument* document, char *); 
/* for writting strings */

//DLLEXPORT_TEST_FUNCTION void PdfDocument_WriteBinaryData(struct PdfDocument* document, char *data, int size); 
/* for writting binary data */

//DLLEXPORT_TEST_FUNCTION void PdfDocument_WriteNewLine(struct PdfDocument* self); 
/* Writes new line */

DLLEXPORT_TEST_FUNCTION void PdfDocument_BeginNewObject(struct PdfBaseObject *object);
/* Create and start writting new object. This will add object to list of all objects */ 

DLLEXPORT_TEST_FUNCTION void PdfDocument_EndObject(struct PdfBaseObject *object);
/* End object declaration. Will finalized object */

DLLEXPORT_TEST_FUNCTION struct PdfPage* PdfDocument_CreatePage(struct PdfDocument *self, struct PdfPageProperties *props);
/* End object declaration. Will finalized object */


DLLEXPORT_TEST_FUNCTION struct PdfFont* PdfDocument_FindFont(struct PdfDocument *self, char *fontName);
/* Find font with specified name */

DLLEXPORT_TEST_FUNCTION struct PdfFont* PdfDocument_FindFontBySaveId(struct PdfDocument *self, int saveId);
/* Find font with specified save id - saveId of templateEmbeddedFont is used */

DLLEXPORT_TEST_FUNCTION struct PdfImage* PdfDocument_FindImage(struct PdfDocument *self, char *imageName);
/* Find image with specified name */

DLLEXPORT_TEST_FUNCTION struct PdfShadingDictionary* PdfDocument_FindShading(struct PdfDocument *self, char *shadingName);
/* Find shading with specified name */


//DLLEXPORT_TEST_FUNCTION int PdfDocument_GetFilePosition(struct PdfDocument *self);
/* Get output file postion (offset from the beginning of the file).*/

//DLLEXPORT_TEST_FUNCTION void PdfDocument_SeekFile(struct PdfDocument *self, int offset);
/* Seek in file.*/

//DLLEXPORT_TEST_FUNCTION void PdfDocument_SeekFileToEnd(struct PdfDocument *self);
/* Seek to end of file.*/

DLLEXPORT_TEST_FUNCTION void PdfDocument_SetInformationDictionary(struct PdfDocument * self, struct PdfTemplateInformationDictionary* infoDict);

#endif
