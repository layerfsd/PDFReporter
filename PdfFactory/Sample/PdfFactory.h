/*
PdfGenerator.h

Author: Nebojsa Vislavski
Date: 12.08.2008.	

*/

#ifndef _PDFFACTORY_
#define _PDFFACTORY_

#define DLLIMPORT extern "C" _declspec (dllimport)

// this is called to indicate you should move to next row. return true to indicate everything is ok. if you return false generator will stop
typedef int (_stdcall *InitializeDataStreamCallback)(char *parentDataStream, char *dataStreamName, int *itemsCount);
// this is called to indicate you should move to next row. return true to indicate there are still more items
typedef int (_stdcall *ReadDataCallback)(char *dataStreamName);
// when this callback is called you need to return current value for datastream and column
typedef unsigned char* (_stdcall *RequestDataCallback)(char *dataStreamName, char *columnName, int *dataSize);
// when this callback is called you need to reset data stream
typedef void (_stdcall *ResetDataStreamCallback)(char *dataStreamName);
typedef void (_stdcall *GenerateProgressCallback)(float currentProgress);

DLLIMPORT void SetResetDataStreamCallback(ResetDataStreamCallback resetDataStreamCallback);
/* set callbacks */
DLLIMPORT void SetRequestDataCallback(RequestDataCallback requestDataCallback);
/* set callbacks */
DLLIMPORT void SetReadDataCallback(ReadDataCallback readDataCallback);
/* set callbacks */
DLLIMPORT void SetInitializeDataStreamCallback(InitializeDataStreamCallback initializeDataStreamCallback);
/* set callbacks */

DLLIMPORT short InitializeGenerator(char *companyName, char *serial);
DLLIMPORT void SetLogging(short enable, short logLevel);
DLLIMPORT void ShutdownGenerator();
DLLIMPORT int AttachTemplateFromFile(char *templateName);
DLLIMPORT int AttachTemplateFromMemory(char *templateData, int templateDataSize);
DLLIMPORT int GenerateToFile(char* outputFileName);
DLLIMPORT char* GenerateToMemory(int *outDataSize);

#endif
