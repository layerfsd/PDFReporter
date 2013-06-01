#include "Logger.h"
#include "MemoryManager.h"
#include <stdio.h>
#include <stdlib.h>
#include <time.h>
#include <stdarg.h>

struct Logger *loggerInstance = 0;

DLLEXPORT_TEST_FUNCTION void Logger_LogWarningMessage(char *msg, ...)
{
	char tmp[1024];
	va_list argList;

	if (loggerInstance != 0 && loggerInstance->logLevel >= LOGGER_LEVEL_WARNING && loggerInstance->enabled)
	{	
		va_start(argList, msg);
		vsprintf(tmp, msg, argList);
		va_end(argList);

		loggerInstance->fileHandle = fopen("pdffactory.log", "a");
		fprintf(loggerInstance->fileHandle, "[%s] %s: %s\n", "NO TIME", "WARN", tmp);	
		fclose(loggerInstance->fileHandle);
	}
}

DLLEXPORT_TEST_FUNCTION void Logger_LogNoticeMessage(char *msg, ...)
{
	char tmp[1024];
	va_list argList;
	
	if (loggerInstance != 0 && loggerInstance->logLevel >= LOGGER_LEVEL_NOTICE && loggerInstance->enabled)
	{		
		va_start(argList, msg);
		vsprintf(tmp, msg, argList);
		va_end(argList);

		loggerInstance->fileHandle = fopen("pdffactory.log", "a");
		fprintf(loggerInstance->fileHandle, "[%s] %s: %s\n", "NO TIME", "NOTICE", tmp);
		fclose(loggerInstance->fileHandle);		
	}	
}

DLLEXPORT_TEST_FUNCTION void Logger_LogErrorMessage(char *msg, ...)
{
	char tmp[1024];
	va_list argList;
	if (loggerInstance != 0 && loggerInstance->logLevel >= LOGGER_LEVEL_ERROR && loggerInstance->enabled)
	{
		va_start(argList, msg);
		vsprintf(tmp, msg, argList);
		va_end(argList);

		loggerInstance->fileHandle = fopen("pdffactory.log", "a");
		fprintf(loggerInstance->fileHandle, "[%s] %s: %s\n", "NO TIME", "ERROR", tmp);	
		fclose(loggerInstance->fileHandle);
	}
}

/// Enable/Disable logging
DLLEXPORT_TEST_FUNCTION void Logger_EnableLogging(short enable)
{
	if (loggerInstance)
	{
		loggerInstance->enabled = enable;
	}
}

DLLEXPORT_TEST_FUNCTION void Logger_SetLogLevel(short logLevel)
{
	if (loggerInstance)
	{
		loggerInstance->logLevel = logLevel;
	}
}


/// Initialize logging 
DLLEXPORT_TEST_FUNCTION void Logger_Initialize(short logLevel)
{
	loggerInstance = MemoryManager_Alloc(sizeof(struct Logger));
	loggerInstance->enabled = FALSE;
	loggerInstance->fileHandle = 0;	
	loggerInstance->initialized = TRUE;
	loggerInstance->logLevel = logLevel;
}