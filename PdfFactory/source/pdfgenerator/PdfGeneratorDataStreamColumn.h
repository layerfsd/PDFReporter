#ifndef _PDFGENERATORDATASTREAM_COLUMN_
#define _PDFGENERATORDATASTREAM_COLUMN_

#include "PdfFactory.h"


#define COLUMN_TYPE_NONE 0
#define COLUMN_TYPE_STRING 1
#define COLUMN_TYPE_INT 2
#define COLUMN_TYPE_DOUBLE 3
#define COLUMN_TYPE_CHAR 4



struct PdfGeneratorDataStreamColumn 
{
	char name[50];
	int type;
	int length;
};


struct PdfGeneratorDataStreamColumn* PdfGeneratorDataStreamColumn_Create(int type, int length, char *name);
void PdfGeneratorDataStreamColumn_Init(struct PdfGeneratorDataStreamColumn *self, int type, int length, char *name);
void PdfGeneratorDataStreamColumn_Destroy(struct PdfGeneratorDataStreamColumn *self);


#endif
