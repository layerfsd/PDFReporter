/*
PdfHeader.c

Author: Nebojsa Vislavski
Date: 30.6.2008.	

This functions are used as main thing when working with pdf

*/


#include "PdfHeader.h"
#include "Logger.h"
#include <stdio.h>

void PdfHeader_WriteHeader(struct PdfDocument *document)
{
	char header[10];	

	sprintf(header, "\%%PDF-%d.%d", document->majorVersion, document->minorVersion);

	document->streamWriter->WriteData(document->streamWriter, header);
	document->streamWriter->WriteNewLine(document->streamWriter);	
	Logger_LogNoticeMessage("PdfHeader_WriteHeader: Wrote header version %d.%d SUCCESS", document->majorVersion, document->minorVersion);
}
