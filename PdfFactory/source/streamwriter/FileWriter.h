#ifndef _FILEWRITER_H_
#define _FILEWRITER_H_

#include "PdfFactory.h"
#include "StreamWriter.h"
#include <stdio.h>

struct FileWriter
{
	struct StreamWriter base;
	FILE *fileHandle;
};



DLLEXPORT_TEST_FUNCTION struct FileWriter* FileWriter_Create();
DLLEXPORT_TEST_FUNCTION struct FileWriter* FileWriter_CreateFromFile(char *fileName, char *mode);
DLLEXPORT_TEST_FUNCTION void FileWriter_Init(struct FileWriter *self, FILE *fileHandle);
DLLEXPORT_TEST_FUNCTION void FileWriter_Cleanup(struct FileWriter *self);
DLLEXPORT_TEST_FUNCTION void FileWriter_Destroy(struct StreamWriter *self);

DLLEXPORT_TEST_FUNCTION int FileWriter_Open(struct FileWriter *self, char *fileName, char *mode);
DLLEXPORT_TEST_FUNCTION void FileWriter_Close(struct FileWriter *self);

void FileWriter_WriteData(struct StreamWriter *self, char *string);
void FileWriter_WriteNewLine(struct StreamWriter *self);
void FileWriter_WriteBinaryData(struct StreamWriter *self, char *data, int size);
void FileWriter_Seek(struct StreamWriter *self, int offset);
int  FileWriter_GetPosition(struct StreamWriter *self);


#endif