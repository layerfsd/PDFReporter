/*
GraphicWriter.h

Author: Marko Vranjkovic
Date: 08.09.2008.	

*/


#ifndef _PDFGRAPHICWRITER_
#define _PDFGRAPHICWRITER_


#include "PdfFactory.h"
#include "StreamWriter.h"
#include "PdfContentStream.h"
#include "Rectangle.h"
#include "NumberObject.h"
#include "StringObject.h"
#include "PdfImage.h"

struct RGBColor
{
	float red;
	float green;
	float blue;
};

struct CMYKColor
{
	float cyan;
	float magenta;
	float yellow;
	float key;
};

struct PdfGraphicWriter
{
	struct StreamWriter *streamWriter;
	struct RGBColor *rgbFillColor; 
	struct RGBColor *rgbStrokeColor;
	struct CMYKColor *cmykFillColor; 
	struct CMYKColor *cmykStrokeColor;
};


DLLEXPORT_TEST_FUNCTION struct PdfGraphicWriter* PdfGraphicWriter_Create(struct StreamWriter *streamWriter);

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_Destroy(struct PdfGraphicWriter *self);

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_Init(struct PdfGraphicWriter *self, struct StreamWriter *streamWriter);

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_SetLineWidth(struct PdfGraphicWriter *self, double lineWidth);

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_DrawLine(struct PdfGraphicWriter *self, float x1, float y1, float x2, float y2);

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_DrawRectangle(struct PdfGraphicWriter *self, struct Rectangle *rect, int fill, int stroke);

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_SaveGraphicState(struct PdfGraphicWriter *self);

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_RestoreGraphicState(struct PdfGraphicWriter *self);

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_SetRGBFillColor(struct PdfGraphicWriter *self, float r, float g, float b);

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_SetRGBStrokeColor(struct PdfGraphicWriter *self, float r, float g, float b);

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_SetCMYKStrokeColor(struct PdfGraphicWriter *self, float cyan, float magenta, float yellow, float key);

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_SetCMYKFillColor(struct PdfGraphicWriter *self, float cyan, float magenta, float yellow, float key);

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_PaintRGBStroke(struct PdfGraphicWriter *self);

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_PaintRGBFill(struct PdfGraphicWriter *self);

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_PaintCMYKStroke(struct PdfGraphicWriter *self);

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_PaintCMYKFill(struct PdfGraphicWriter *self);

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_DrawCircle(struct PdfGraphicWriter *self, int x, int y, int radius, int asClippingPath);

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_PaintObject(struct PdfGraphicWriter *self);

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_ResetColors(struct PdfGraphicWriter *self);

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_SetImage(struct PdfGraphicWriter *self, struct PdfImage *image, float posX, float posY, float width, float height);

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_SetImageTransformed(struct PdfGraphicWriter *self, struct PdfImage *image, float locX, float locY, float width, float height, float angle);
/* Sets image fully transformed. Angle is in degreses */

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_SetImageWithTransformation(struct PdfGraphicWriter *self, struct PdfImage *image, float a, float b, float c, float d, float e, float f);

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_SetTransformation(struct PdfGraphicWriter *self, float a, float b, float c, float d, float e, float f);

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_SetShading(struct PdfGraphicWriter *self, const char *shadingName);

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_SetClippingPath(struct PdfGraphicWriter *self);


#endif