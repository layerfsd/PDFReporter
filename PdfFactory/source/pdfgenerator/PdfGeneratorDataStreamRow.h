/* 
   PdfGeneratorDataStreamRow.h

   Autor: Nebojsa Vislavski
   Date: 18.8.2008.
*/

#ifndef _PDFGENERATORDATASTREAM_ROW_
#define _PDFGENERATORDATASTREAM_ROW_

#include "PdfFactory.h"

struct PdfGeneratorDataStreamColumn;

struct PdfGeneratorDataStreamRow
{
	struct DLList *fields; // all fields in row
};

DLLEXPORT_TEST_FUNCTION struct PdfGeneratorDataStreamRow *PdfGeneratorDataStreamRow_Create();
DLLEXPORT_TEST_FUNCTION void PdfGeneratorDataStreamRow_Init(struct PdfGeneratorDataStreamRow *self);
DLLEXPORT_TEST_FUNCTION void PdfGeneratorDataStreamRow_Destroy(struct PdfGeneratorDataStreamRow *self);
DLLEXPORT_TEST_FUNCTION void PdfGeneratorDataStreamRow_AddFieldByColumn(struct PdfGeneratorDataStreamRow *self, struct PdfGeneratorDataStreamColumn *column, void *value);
DLLEXPORT_TEST_FUNCTION void PdfGeneratorDataStreamRow_AddField(struct PdfGeneratorDataStreamRow *self, struct PdfGeneratorDataStream *stream, char *columnName, void *value);
DLLEXPORT_TEST_FUNCTION void *PdfGeneratorDataStreamRow_GetFieldValue(struct PdfGeneratorDataStreamRow *self, char *columnName);
/* Add string field */


#endif
