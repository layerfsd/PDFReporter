#ifndef _PDFCONTENTSTREAM_
#define _PDFCONTENTSTREAM_

#include "PdfFactory.h"
#include "PdfDocument.h"
#include "PdfBaseObject.h"
#include "PdfPage.h"
#include "StreamObject.h"


struct PdfContentStream
{
	struct PdfBaseObject base;
	
	struct StreamObject *stream; // stream object (stream ... bytes.. endstream)

	//struct StreamWriter *streamWriter; // stream writer used for writing data to file or memory. 
	int lengthOffsetPlace; // place where length should be written
	int writeStarted; // this is TRUE between begin and End
	int inMemory; // if this content stream is written from memory
	char enableCompression;
};



DLLEXPORT_TEST_FUNCTION void PdfContentStream_Init(struct PdfContentStream *self, struct PdfDocument *document);
/* Initializes PdfContentStream struct. */

DLLEXPORT_TEST_FUNCTION struct PdfContentStream* PdfContentStream_Begin(struct PdfDocument *document, char useCompression);
/* Create PdfContentStream object in memory */

DLLEXPORT_TEST_FUNCTION void PdfContentStream_End(struct PdfContentStream *self);
/* End PdfContentStream object. */

DLLEXPORT_TEST_FUNCTION void PdfContentStream_Write(struct PdfContentStream *self);
/* Write PdfContentStream object to document. */

DLLEXPORT_TEST_FUNCTION void PdfContentStream_Destroy(struct PdfContentStream *self);
/* Destroy PdfContentStream struct. */




#endif

