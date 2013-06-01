/*
 ---------------------------------------
| File:		PdfPrecalculatedFunction.h	|
| Author:	Tomislav Kukic				|
| Date:		14.1.2009					|
 ---------------------------------------
*/


#ifndef _PDF_PRECALCULATED_FUNCTION_
#define _PDF_PRECALCULATED_FUNCTION_


#define PDF_PRECALCULATED_FUNCTION_TYPE_SUM			1
#define PDF_PRECALCULATED_FUNCTION_TYPE_AVG			2
#define PDF_PRECALCULATED_FUNCTION_TYPE_MAX			3
#define PDF_PRECALCULATED_FUNCTION_TYPE_MIN			4
#define PDF_PRECALCULATED_FUNCTION_TYPE_COUNT		5
#define PDF_PRECALCULATED_FUNCTION_TYPE_DECTOHEX	6
#define PDF_PRECALCULATED_FUNCTION_TYPE_DECTOBIN	7

#include "PdfFactory.h"

struct PdfPrecalculatedFunction
{
	char *dataStream;
	char *columnName;
	
	int type;
	int count;

	int start;
	int end;

	double result;
	double avgResult;
	char *data;
};


DLLEXPORT_TEST_FUNCTION struct PdfPrecalculatedFunction *PdfPrecalculatedFunction_Create(int type, const char *datastream, const char *column);

DLLEXPORT_TEST_FUNCTION void PdfPrecalculatedFunction_Destroy(struct PdfPrecalculatedFunction *self);

DLLEXPORT_TEST_FUNCTION void PdfPrecalculatedFunction_Cleanup(struct PdfPrecalculatedFunction *self);

DLLEXPORT_TEST_FUNCTION double PdfPrecalculatedFunction_GetValue(struct PdfPrecalculatedFunction *self, char *retValue);

DLLEXPORT_TEST_FUNCTION void PdfPrecalculatedFunction_AddValue(struct PdfPrecalculatedFunction *self, float inValue);
DLLEXPORT_TEST_FUNCTION void PdfPrecalculatedFunction_ResetValue(struct PdfPrecalculatedFunction *self);


DLLEXPORT_TEST_FUNCTION void PdfPrecalculatedFunction_SetStringPosition(struct PdfPrecalculatedFunction *self, int start, int end);




#endif

