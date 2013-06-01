#include "FileWriter.h"
#include "MemoryManager.h"

DLLEXPORT_TEST_FUNCTION struct FileWriter* FileWriter_Create()
{
	struct FileWriter *x;
	x = (struct FileWriter*)MemoryManager_Alloc(sizeof(struct FileWriter));
	// set values
	FileWriter_Init(x, 0);
	return x;
}


DLLEXPORT_TEST_FUNCTION struct FileWriter* FileWriter_CreateFromFile(char *fileName, char *mode)
{
	struct FileWriter *x = FileWriter_Create();
	FileWriter_Open(x, fileName, mode);
	return x;
}

DLLEXPORT_TEST_FUNCTION void FileWriter_Init(struct FileWriter *self, FILE *fileHandle)
{
	StreamWriter_Init((struct StreamWriter*)self);
	self->fileHandle = fileHandle;
	self->base.Destroy = FileWriter_Destroy;
	self->base.WriteBinaryData = FileWriter_WriteBinaryData;
	self->base.WriteData = FileWriter_WriteData;
	self->base.WriteNewLine = FileWriter_WriteNewLine;
	self->base.Seek = FileWriter_Seek;
	self->base.GetPosition = FileWriter_GetPosition;
}

DLLEXPORT_TEST_FUNCTION int FileWriter_Open(struct FileWriter *self, char *fileName, char *mode)
{
	self->fileHandle = fopen(fileName, mode);
	if (self->fileHandle == 0)
	{
		return FALSE;
	}
	else 
	{
		return TRUE;
	}
}

DLLEXPORT_TEST_FUNCTION void FileWriter_Close(struct FileWriter *self)
{
	if (self->fileHandle)
	{
		fflush(self->fileHandle);
		fclose(self->fileHandle);
		self->fileHandle = 0;
	}
}



DLLEXPORT_TEST_FUNCTION void FileWriter_Cleanup(struct FileWriter *self)
{
	FileWriter_Close(self);
	self->fileHandle = NULL;
}

DLLEXPORT_TEST_FUNCTION void FileWriter_Destroy(struct StreamWriter *self)
{
	struct FileWriter *fileWriter = (struct FileWriter *)self;
	FileWriter_Cleanup(fileWriter);

	MemoryManager_Free(fileWriter);
}


void FileWriter_WriteData(struct StreamWriter *self, char *string)
{
	struct FileWriter *fileWriter = (struct FileWriter*)self;

	if (fileWriter->fileHandle)
	{
		fwrite(string, 1, strlen(string), fileWriter->fileHandle);
	}	
}

void FileWriter_WriteNewLine(struct StreamWriter *self)
{
	char eofln;
	struct FileWriter *fileWriter = (struct FileWriter*)self;

	if (fileWriter->fileHandle)
	{
		eofln = '\n';
		fputc(eofln, fileWriter->fileHandle);		
	}
}

void FileWriter_WriteBinaryData(struct StreamWriter *self, char *data, int size)
{
	struct FileWriter *fileWriter = (struct FileWriter*)self;

	if (fileWriter->fileHandle)
	{
		fwrite(data, size, 1, fileWriter->fileHandle);
	}
}

void FileWriter_Seek(struct StreamWriter *self, int offset)
{
	struct FileWriter *fileWriter = (struct FileWriter*)self;

	fseek(fileWriter->fileHandle, offset, SEEK_SET);
}

int FileWriter_GetPosition(struct StreamWriter *self)
{
	struct FileWriter *fileWriter = (struct FileWriter*)self;
	
	if (fileWriter->fileHandle)
	{		
		return (int)ftell(fileWriter->fileHandle);		
	}
	else 
	{
		return 0;
	}
}
