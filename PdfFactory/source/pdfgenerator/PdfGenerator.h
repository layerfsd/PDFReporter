/*
PdfGenerator.h

Author: Nebojsa Vislavski
Date: 12.08.2008.	

*/

#ifndef _PDFGENERATOR_
#define _PDFGENERATOR_

#include "PdfFactory.h"

// this is called to indicate you should move to next row. return true to indicate everything is ok. if you return false generator will stop
typedef int (_stdcall *InitializeDataStreamCallback)(char *parentDataStream, char *dataStreamName, int *itemsCount);
// this is called to indicate you should move to next row. return true to indicate there are still more items
typedef int (_stdcall *ReadDataCallback)(char *dataStreamName);
// when this callback is called you need to return current value for datastream and column
typedef unsigned char* (_stdcall *RequestDataCallback)(char *dataStreamName, char *columnName, int *dataSize);
// when this callback is called you need to reset data stream
typedef void (_stdcall *ResetDataStreamCallback)(char *dataStreamName);
typedef void (_stdcall *GenerateProgressCallback)(float currentProgress);

struct PdfGenerator
{
	int templateLoaded;  // is template loaded
	int stopGenerating;  // this is FALSE. it can be set to true by user or when generator has finished
	struct PdfTemplate *pdfTemplate; // template used for generating	
	struct DLList *dataStreams;	

	struct PdfGeneratorDataStream *topDataStreamPosition; // 
	struct PdfDocument *document;
	struct PdfPage *currentPage; // current page that is generated
	struct PdfContentStream *currentContentStream;
	struct DLList *rectMatrix; // matrix used for checking of free space for balloons on page

	struct DLList *pageBalloons; // this is top level main page balloons. We use page balloons so algorithm only works with balloons
	struct PdfGeneratedBalloon *currentPageBalloon; // this is current page balloon used. Assigned when new page is created
	int pageSkipDataReadMarker; // as page does not have its template balloon item we use this

	int useCompression; // if flate decode should be used for content streams. Default is FALSE
	
	int pageNumber; // current page number		
	int validSerial;

	GenerateProgressCallback progressCallback;
	ResetDataStreamCallback resetDataStreamCallback;
	RequestDataCallback requestDataCallback;
	ReadDataCallback readDataCallback;
	InitializeDataStreamCallback initializeDataStreamCallback;
};

/*
   Here are main functions to be exported. They all use local instance of generator
*/

struct PdfGenerator *localInstance;

DLLEXPORT void SetResetDataStreamCallback(ResetDataStreamCallback resetDataStreamCallback);
/* set callbacks */
DLLEXPORT void SetRequestDataCallback(RequestDataCallback requestDataCallback);
/* set callbacks */
DLLEXPORT void SetReadDataCallback(ReadDataCallback readDataCallback);
/* set callbacks */
DLLEXPORT void SetInitializeDataStreamCallback(InitializeDataStreamCallback initializeDataStreamCallback);
/* set callbacks */

DLLEXPORT short InitializeGenerator(char *companyName, char *serial);
DLLEXPORT void SetLogging(short enable, short logLevel);
DLLEXPORT void ShutdownGenerator();
DLLEXPORT int AttachTemplateFromFile(char *templateName);
DLLEXPORT int AttachTemplateFromMemory(char *templateData, int templateDataSize);
DLLEXPORT int GenerateToFile(char* outputFileName);
DLLEXPORT char* GenerateToMemory(int *outDataSize);
DLLEXPORT_TEST_FUNCTION int CheckSerial(char *companyName, char *serial);

/*
   Other functions used for testing

*/


DLLEXPORT_TEST_FUNCTION struct PdfGenerator* PdfGenerator_Create();

DLLEXPORT_TEST_FUNCTION void PdfGenerator_Init(struct PdfGenerator *self);

DLLEXPORT_TEST_FUNCTION void PdfGenerator_Destroy(struct PdfGenerator *self);

void PdfGenerator_ClearDataStreams(struct PdfGenerator* self);
/* Clear all data streams */

DLLEXPORT_TEST_FUNCTION int PdfGenerator_AttachTemplateFromFile(struct PdfGenerator* self, char *templateName);
/* attach template file name */

DLLEXPORT_TEST_FUNCTION int PdfGenerator_AttachTemplateFromMemory(struct PdfGenerator* self, char *templateData, int templateSize);
/* attach template from memory data*/

DLLEXPORT_TEST_FUNCTION void PdfGenerator_SetProgressCallback(struct PdfGenerator* self, GenerateProgressCallback callback);
/* set callbacks */

DLLEXPORT_TEST_FUNCTION void PdfGenerator_SetResetDataStreamCallback(struct PdfGenerator* self, ResetDataStreamCallback resetDataStreamCallback);
/* set callbacks */
DLLEXPORT_TEST_FUNCTION void PdfGenerator_SetRequestDataCallback(struct PdfGenerator* self, RequestDataCallback requestDataCallback);
/* set callbacks */
DLLEXPORT_TEST_FUNCTION void PdfGenerator_SetReadDataCallback(struct PdfGenerator* self, ReadDataCallback readDataCallback);
/* set callbacks */
DLLEXPORT_TEST_FUNCTION void PdfGenerator_SetInitializeDataStreamCallback(struct PdfGenerator* self, InitializeDataStreamCallback initializeDataStreamCallback);
/* set callbacks */


struct PdfGeneratorDataStream* PdfGenerator_FindDataStream(struct PdfGenerator *self, char *dataStreamName);

DLLEXPORT_TEST_FUNCTION int PdfGenerator_GenerateToFile(struct PdfGenerator* self, char* outputFileName);
/* generate pdf file, return false on failure. DataStreams must be attached and template must be loaded*/

DLLEXPORT_TEST_FUNCTION char* PdfGenerator_GenerateToMemory(struct PdfGenerator* self, int* outDataSizeFilled);
/* generate pdf file to memory and return it. It will return null and dataSizeFilled = 0 if some error appear */

DLLEXPORT_TEST_FUNCTION char PdfGenerator_GetErrors(struct PdfGenerator* self);
/* return errors from last call */

DLLEXPORT_TEST_FUNCTION int PdfGenerator_GetNextBalloonLocation(struct PdfGenerator *self, struct Rectangle *container, struct Rectangle *rect, struct DLList *rectMatrix, double *outLocX, double *outLocY);

DLLEXPORT_TEST_FUNCTION void PdfGenerator_DrawRemainingBalloons(struct PdfGenerator *self, struct DLList *remainingBalloons);

#endif
