//-----------------------------------------------------------------------------
// Name:	PdfFunction.h
// Author:	Tomislav Kukic
// Date:	28.12.2008
//-----------------------------------------------------------------------------


#ifndef _PDFFUNCTION_
#define _PDFFUNCTION_


#define PDF_FUNCTION_TYPE_LISTOFFUNCTIONS 3
#define PDF_FUNCTION_TYPE_SIMPLEFUNCTION 2


#include "PdfBaseObject.h"

struct PdfFunction
{
	struct PdfBaseObject base;

	int functionType;
	double rIn,gIn,bIn, rOut,gOut,bOut;
	double cIn,mIn,yIn,kIn, cOut,mOut,yOut,kOut;
	double shadingFaktor;
	int numOfEncodings;

	struct DLList *functions;
};


DLLEXPORT_TEST_FUNCTION struct PdfFunction *PdfFunction_Create(struct PdfDocument *document, int type, double shadingFaktor);

DLLEXPORT_TEST_FUNCTION void PdfFunction_Init(struct PdfFunction *self, struct PdfDocument *document, int type, double shadingFaktor);

DLLEXPORT_TEST_FUNCTION void PdfFunction_Destroy(struct PdfFunction *self);

DLLEXPORT_TEST_FUNCTION void PdfFunction_Cleanup(struct PdfFunction *self);

DLLEXPORT_TEST_FUNCTION void PdfFunction_Write(struct PdfFunction *self, struct StreamWriter *streamWriter, int useCMYK);//if not using CMYK it be used RGB

DLLEXPORT_TEST_FUNCTION void PdfFunction_SetRGBStartColor(struct PdfFunction *self, double r, double g, double b);

DLLEXPORT_TEST_FUNCTION void PdfFunction_SetRGBEndColor(struct PdfFunction *self, double r, double g, double b);

DLLEXPORT_TEST_FUNCTION void PdfFunction_SetCMYKStartColor(struct PdfFunction *self, double c, double m, double y, double k);

DLLEXPORT_TEST_FUNCTION void PdfFunction_SetCMYKEndColor(struct PdfFunction *self, double c, double m, double y, double k);

DLLEXPORT_TEST_FUNCTION void PdfFunction_AddNewFunction(struct PdfFunction *self, struct PdfFunction *newFunction);


#endif
