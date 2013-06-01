#ifndef _LOGGER_
#define _LOGGER_

#include "PdfFactory.h"
#include <stdio.h>

#define LOGGER_LEVEL_NOTICE 0
#define LOGGER_LEVEL_WARNING 1
#define LOGGER_LEVEL_ERROR 2

struct Logger
{
	short enabled;
	FILE *fileHandle;
	short logLevel;
	short initialized;
};

/// Singleton object used for logging. Created in Initialize function
struct Logger *loggerInstance;

/// This will log message in format 
/// [DateTime]: [TYPE]: Message
DLLEXPORT_TEST_FUNCTION void Logger_LogWarningMessage(char *msg, ...); 
DLLEXPORT_TEST_FUNCTION void Logger_LogNoticeMessage(char *msg, ...); 
DLLEXPORT_TEST_FUNCTION void Logger_LogErrorMessage(char *msg, ...); 

/// Enable/Disable logging
DLLEXPORT_TEST_FUNCTION void Logger_EnableLogging(short enable);

/// Initialize logging 
DLLEXPORT_TEST_FUNCTION void Logger_Initialize(short logLevel);



#endif