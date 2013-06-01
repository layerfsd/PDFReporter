#ifndef _PdfStreamObject_
#define _PdfStreamObject_

#include "PdfFactory.h"
#include "StreamWriter.h"


/************************************************************************
writes this kind of data
stream
BYTES
endstream
/************************************************************************/
struct StreamObject
{	
	struct StreamWriter *streamWriter;
	int beginStreamOffset;  // where in file is stream starting
	int endStreamOffset; // where in file is stream ended
	int length;	
	char beginInMemory;
};

DLLEXPORT_TEST_FUNCTION void StreamObject_Init(struct StreamObject *self, struct StreamWriter *streamWriter);
/* Initializes struct. */

DLLEXPORT_TEST_FUNCTION struct StreamObject* StreamObject_Begin(struct StreamWriter *streamWriter);
/* Begin StreamObject object. */

DLLEXPORT_TEST_FUNCTION struct StreamObject* StreamObject_BeginInMemory();
/* Begin StreamObject object in memory. */

DLLEXPORT_TEST_FUNCTION void StreamObject_End(struct StreamObject *self);
/* End StreamObject writing */

DLLEXPORT_TEST_FUNCTION void StreamObject_Write(struct StreamObject *self);
/* Write stream object to document */

DLLEXPORT_TEST_FUNCTION void StreamObject_Destroy(struct StreamObject *self);
/* Destroy StreamObject struct. */

DLLEXPORT_TEST_FUNCTION void StreamObject_WriteData(struct StreamObject *self, char *data);
/* Write data to stream. */

DLLEXPORT_TEST_FUNCTION void StreamObject_WriteNewLine(struct StreamObject *self);
/* Write new line to stream. */

void StreamObject_ApplyFilter(struct StreamObject *self);
/* Compress stream written in memory */


#endif
