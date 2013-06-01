/*
PdfGeneratorDataSream.h

Author: Nebojsa Vislavski
Date: 12.08.2008.	

*/


#ifndef _PDFGENERATORDATASTREAM_
#define _PDFGENERATORDATASTREAM_

#include "PdfFactory.h"
#include "PdfGeneratorDataStreamColumn.h"
#include "PdfGeneratorDataStreamRow.h"

struct PdfGeneratorDataStream
{
	char name[100]; // name of stream
	int initialized; // is data stream initialized
	int itemsCount; // items Count
	
	// These properties are deprecated
	// =======
	struct DLList *rows; // all rows in stream. Each row consists of fields
	struct DLList *columns; // all columns in data stream
	
	struct DLListNode* currentRow; // current node

	struct PdfGeneratorDataStream *childStream; // child data stream if available
	struct PdfGeneratorDataStream *parentStream; // parent data stream if available
	// TODO: add connection object description between child and himself
	// ========
};

DLLEXPORT_TEST_FUNCTION struct PdfGeneratorDataStream *PdfGeneratorDataStream_Create(char *name);
DLLEXPORT_TEST_FUNCTION void PdfGeneratorDataStream_Init(struct PdfGeneratorDataStream *self, char *name);

DLLEXPORT_TEST_FUNCTION void PdfGeneratorDataStream_First(struct PdfGeneratorDataStream *self);
/* Moves data pointer to first position. Returns 0 on empty */

DLLEXPORT_TEST_FUNCTION int PdfGeneratorDataStream_Empty(struct PdfGeneratorDataStream *self);
/* Determine if stream is empty */

DLLEXPORT_TEST_FUNCTION int PdfGeneratorDataStream_End(struct PdfGeneratorDataStream *self);
/* Determine if stream is at the end */


DLLEXPORT_TEST_FUNCTION void PdfGeneratorDataStream_Next(struct PdfGeneratorDataStream *self);
/* Move internal data pointer to next place */

DLLEXPORT_TEST_FUNCTION void PdfGeneratorDataStream_Prev(struct PdfGeneratorDataStream *self);
/* Move internal data pointer to prev place */

DLLEXPORT_TEST_FUNCTION int PdfGeneratorDataStream_Size(struct PdfGeneratorDataStream *self);
/* return number of rows */

DLLEXPORT_TEST_FUNCTION void PdfGeneratorDataStream_Destroy(struct PdfGeneratorDataStream *self);

DLLEXPORT_TEST_FUNCTION struct PdfGeneratorDataStreamColumn* PdfGeneratorDataStream_AddColumn(struct PdfGeneratorDataStream *self, int type, int length, char *name);
/* Add column to list of columns. */

DLLEXPORT_TEST_FUNCTION struct PdfGeneratorDataStreamColumn* PdfGeneratorDataStream_FindColumn(struct PdfGeneratorDataStream *self, char *name);


DLLEXPORT_TEST_FUNCTION struct PdfGeneratorDataStreamColumn* PdfGeneratorDataStream_GetColumn(struct PdfGeneratorDataStream *self, char *name);
/* Return column with this name or null if not found */

DLLEXPORT_TEST_FUNCTION struct PdfGeneratorDataStreamRow *PdfGeneratorDataStream_AddRow(struct PdfGeneratorDataStream *self);

DLLEXPORT_TEST_FUNCTION struct PdfGeneratorDataStreamRow *PdfGeneratorDataStream_GetCurrentRow(struct PdfGeneratorDataStream *self);

DLLEXPORT_TEST_FUNCTION struct PdfGeneratorDataStreamRow *PdfGeneratorDataStream_GetRow(struct PdfGeneratorDataStream *self, int index);
/* Get row at specified index */

DLLEXPORT_TEST_FUNCTION void PdfGeneratorDataStream_AddChild(struct PdfGeneratorDataStream *self, struct PdfGeneratorDataStream *childStream);
/* add child stream */

#endif
