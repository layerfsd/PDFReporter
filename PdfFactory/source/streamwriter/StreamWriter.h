#ifndef _STREAMWRITER_
#define _STREAMWRITER_

#include "PdfFactory.h"


typedef void (*WriteData_Method)(struct StreamWriter *, char *);
typedef void (*WriteNewLine_Method)(struct StreamWriter *);
typedef void (*WriteBinaryData_Method)(struct StreamWriter *, char *, int size);
typedef void (*Destroy_Method)(struct StreamWriter *);
typedef void (*Seek_Method)(struct StreamWriter *, int offset);
typedef int (*GetPosition_Method)(struct StreamWriter *);


struct StreamWriter
{	
	WriteData_Method WriteData;
	WriteNewLine_Method WriteNewLine;
	WriteBinaryData_Method WriteBinaryData;
	Seek_Method Seek;
	GetPosition_Method GetPosition;
	Destroy_Method Destroy;		
};


DLLEXPORT_TEST_FUNCTION struct StreamWriter* StreamWriter_Create();
DLLEXPORT_TEST_FUNCTION void StreamWriter_Init(struct StreamWriter *self);
DLLEXPORT_TEST_FUNCTION void StreamWriter_Cleanup(struct StreamWriter *self);
DLLEXPORT_TEST_FUNCTION void StreamWriter_Destroy(struct StreamWriter *self);


void StreamWriter_WriteData(struct StreamWriter *self, char *string);
void StreamWriter_WriteNewLine(struct StreamWriter *self);
void StreamWriter_WriteBinaryData(struct StreamWriter *self, char *data, int size);
void StreamWriter_Seek(struct StreamWriter *self, int offset);
int StreamWriter_GetPosition(struct StreamWriter *self);




#endif