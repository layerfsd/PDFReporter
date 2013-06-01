/*
GraphicWriter.c

Author: Marko Vranjkovic
Date: 08.09.2008.	

*/ 
#include "GraphicWriter.h"
#include "MemoryManager.h"
#include "NameObject.h"
#include "TransformationMatrix.h"
#include "Logger.h"
#include <stdlib.h>
#include <math.h>

DLLEXPORT_TEST_FUNCTION struct PdfGraphicWriter* PdfGraphicWriter_Create(struct StreamWriter *streamWriter)
{
	struct PdfGraphicWriter *writer;
	writer = (struct PdfGraphicWriter*)MemoryManager_Alloc(sizeof(struct PdfGraphicWriter));
	PdfGraphicWriter_Init(writer, streamWriter);
	writer->rgbFillColor = NULL;
	writer->rgbStrokeColor = NULL;
	writer->cmykFillColor = NULL;
	writer->cmykStrokeColor = NULL;
	return writer;
}

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_Destroy(struct PdfGraphicWriter *self)
{	
	MemoryManager_Free(self);
}

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_Init(struct PdfGraphicWriter *self, struct StreamWriter *streamWriter)
{	
	self->streamWriter = streamWriter;
}

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_DrawLine(struct PdfGraphicWriter *self, float x1, float y1, float x2, float y2)
{
	char buffer[20];
	int i;

	for(i = 0; i < 20; i++) buffer[i] = 0;
	sprintf(buffer, "%f", x1);	

	self->streamWriter->WriteData(self->streamWriter, buffer);
	self->streamWriter->WriteData(self->streamWriter, " ");

	for(i = 0; i < 20; i++) buffer[i] = 0;
	sprintf(buffer, "%f", y1);
	
	self->streamWriter->WriteData(self->streamWriter, buffer);
	self->streamWriter->WriteData(self->streamWriter, " m");
	self->streamWriter->WriteNewLine(self->streamWriter);

	for(i = 0; i < 20; i++) buffer[i] = 0;
	sprintf(buffer, "%f", x2);

	self->streamWriter->WriteData(self->streamWriter, buffer);
	self->streamWriter->WriteData(self->streamWriter, " ");
	
	for(i = 0; i < 20; i++) buffer[i] = 0;
	sprintf(buffer, "%f", y2);

	self->streamWriter->WriteData(self->streamWriter, buffer);
	self->streamWriter->WriteData(self->streamWriter, " l");
	self->streamWriter->WriteNewLine(self->streamWriter);

	self->streamWriter->WriteData(self->streamWriter, " S");
	self->streamWriter->WriteNewLine(self->streamWriter);		
}

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_SetLineWidth(struct PdfGraphicWriter *self, double lineWidth)
{
	struct NumberObject *number;

	number = NumberObject_Create(self->streamWriter, lineWidth);
	NumberObject_Write(number);
	NumberObject_Destroy(number);			
	
	self->streamWriter->WriteData(self->streamWriter, " w");
	self->streamWriter->WriteNewLine(self->streamWriter);	
}

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_DrawRectangle(struct PdfGraphicWriter *self, struct Rectangle *rect, int fill, int stroke) 
{
	struct NumberObject *number;

	if (self->rgbStrokeColor != NULL)
		PdfGraphicWriter_PaintRGBStroke(self);		
	
	if (self->rgbFillColor != NULL)
		PdfGraphicWriter_PaintRGBFill(self);

	if (self->cmykStrokeColor != NULL)
		PdfGraphicWriter_PaintCMYKStroke(self);
	
	if (self->cmykFillColor != NULL)
		PdfGraphicWriter_PaintCMYKFill(self);
	
	
	number = NumberObject_Create(self->streamWriter, rect->lowerLeftX);
	NumberObject_Write(number);
	NumberObject_Destroy(number);	
	self->streamWriter->WriteData(self->streamWriter, " ");

	number = NumberObject_Create(self->streamWriter, rect->lowerLeftY);
	NumberObject_Write(number);
	NumberObject_Destroy(number);	
	self->streamWriter->WriteData(self->streamWriter, " ");

	number = NumberObject_Create(self->streamWriter, rect->upperRightX - rect->lowerLeftX);
	NumberObject_Write(number);
	NumberObject_Destroy(number);		
	self->streamWriter->WriteData(self->streamWriter, " ");

	number = NumberObject_Create(self->streamWriter, rect->upperRightY - rect->lowerLeftY);
	NumberObject_Write(number);
	NumberObject_Destroy(number);			
	self->streamWriter->WriteData(self->streamWriter, " re");
	self->streamWriter->WriteNewLine(self->streamWriter);	

	if (fill && !stroke)
	{
		self->streamWriter->WriteData(self->streamWriter, " f");
	}
	else if (fill && stroke)
	{
		self->streamWriter->WriteData(self->streamWriter, " B");
	}
	else if (stroke && !fill)
	{
		self->streamWriter->WriteData(self->streamWriter, " S");
	}
	self->streamWriter->WriteNewLine(self->streamWriter);	
}

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_SaveGraphicState(struct PdfGraphicWriter *self)
{
	self->streamWriter->WriteData(self->streamWriter, " q");
	self->streamWriter->WriteNewLine(self->streamWriter);	
}

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_RestoreGraphicState(struct PdfGraphicWriter *self)
{
	self->streamWriter->WriteData(self->streamWriter, " Q");
	self->streamWriter->WriteNewLine(self->streamWriter);	
}


DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_PaintObject(struct PdfGraphicWriter *self)
{
	if (self->rgbStrokeColor != NULL)
		PdfGraphicWriter_PaintRGBStroke(self);		
	
	if (self->rgbFillColor != NULL)
		PdfGraphicWriter_PaintRGBFill(self);

	if (self->cmykStrokeColor != NULL)
		PdfGraphicWriter_PaintCMYKStroke(self);
	
	if (self->cmykFillColor != NULL)
		PdfGraphicWriter_PaintCMYKFill(self);
}

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_SetRGBFillColor(struct PdfGraphicWriter *self, float r,float g,float b)
{
	struct RGBColor * color;
	color = (struct RGBColor*)MemoryManager_Alloc(sizeof(struct RGBColor));
	color->blue = b;
	color->green = g;
	color->red = r;
	self->rgbFillColor = color;
	MemoryManager_Free(self->cmykFillColor);	
}

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_SetRGBStrokeColor(struct PdfGraphicWriter *self, float r, float g, float b)
{
	struct RGBColor * color;
	color = (struct RGBColor*)MemoryManager_Alloc(sizeof(struct RGBColor));
	color->blue = b;
	color->green = g;
	color->red = r;
	self->rgbStrokeColor = color;	
	MemoryManager_Free(self->cmykStrokeColor);
}

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_SetCMYKStrokeColor(struct PdfGraphicWriter *self, float cyan, float magenta, float yellow, float key)
{
	struct CMYKColor * color;
	color = (struct CMYKColor*)MemoryManager_Alloc(sizeof(struct CMYKColor));
	color->cyan = cyan;
	color->magenta = magenta;
	color->yellow = yellow;
	color->key = key;
	self->cmykStrokeColor = color;	
	MemoryManager_Free(self->rgbStrokeColor);

}

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_SetCMYKFillColor(struct PdfGraphicWriter *self, float cyan, float magenta, float yellow, float key)
{
	struct CMYKColor * color;
	color = (struct CMYKColor*)MemoryManager_Alloc(sizeof(struct CMYKColor));
	color->cyan = cyan;
	color->magenta = magenta;
	color->yellow = yellow;
	color->key = key;
	self->cmykFillColor = color;	
	MemoryManager_Free(self->rgbFillColor);
}

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_ResetColors(struct PdfGraphicWriter *self)
{
	self->rgbFillColor = NULL;
	self->rgbStrokeColor = NULL;
	self->cmykFillColor = NULL;
	self->cmykStrokeColor = NULL;
}


DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_PaintRGBStroke(struct PdfGraphicWriter *self)
{
	struct NumberObject *number = NumberObject_Create(self->streamWriter, self->rgbStrokeColor->red);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " ");

	NumberObject_Init(number, self->streamWriter, self->rgbStrokeColor->green);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " ");
	NumberObject_Init(number, self->streamWriter, self->rgbStrokeColor->blue);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " RG");
	self->streamWriter->WriteNewLine(self->streamWriter);	
	NumberObject_Destroy(number);
}

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_PaintRGBFill(struct PdfGraphicWriter *self)
{
	struct NumberObject *number = NumberObject_Create(self->streamWriter, self->rgbFillColor->red);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " ");
	NumberObject_Init(number, self->streamWriter, self->rgbFillColor->green);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " ");
	NumberObject_Init(number, self->streamWriter, self->rgbFillColor->blue);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " rg");
	self->streamWriter->WriteNewLine(self->streamWriter);	
	NumberObject_Destroy(number);
}

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_PaintCMYKStroke(struct PdfGraphicWriter *self)
{
	struct NumberObject *number = NumberObject_Create(self->streamWriter, self->cmykStrokeColor->cyan);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " ");
	NumberObject_Init(number, self->streamWriter, self->cmykStrokeColor->magenta);
	NumberObject_Write(number);

	self->streamWriter->WriteData(self->streamWriter, " ");
	NumberObject_Init(number, self->streamWriter, self->cmykStrokeColor->yellow);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " ");
	NumberObject_Init(number, self->streamWriter, self->cmykStrokeColor->key);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " K");
	self->streamWriter->WriteNewLine(self->streamWriter);	
	NumberObject_Destroy(number);
}

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_PaintCMYKFill(struct PdfGraphicWriter *self)
{
	struct NumberObject *number = NumberObject_Create(self->streamWriter, self->cmykFillColor->cyan);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " ");
	NumberObject_Init(number, self->streamWriter, self->cmykFillColor->magenta);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " ");
	NumberObject_Init(number, self->streamWriter, self->cmykFillColor->yellow);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " ");
	NumberObject_Init(number, self->streamWriter, self->cmykFillColor->key);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " k");
	self->streamWriter->WriteNewLine(self->streamWriter);	
	NumberObject_Destroy(number);
}

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_DrawCircle(struct PdfGraphicWriter *self, int x, int y, int radius, int asClippingPath)
{	
	int diameter = 2 * radius;
	double xValueInset = diameter * 0.029;
	double yValueOffset = radius * 4/3;
	double kappa = (4 * (sqrt(2) - 1) / 3) * radius;
	struct NumberObject *number = NumberObject_Create(self->streamWriter, x + radius);


	PdfGraphicWriter_PaintObject(self);	


	//Start point
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " ");
	NumberObject_Init(number, self->streamWriter, y);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " m");
	self->streamWriter->WriteNewLine(self->streamWriter);	

	//fourth quadrant
	NumberObject_Init(number, self->streamWriter, x + radius);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " ");
	NumberObject_Init(number, self->streamWriter, y - kappa);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " ");
	NumberObject_Init(number, self->streamWriter, x + kappa);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " ");
	NumberObject_Init(number, self->streamWriter, y - radius);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " ");
	NumberObject_Init(number, self->streamWriter, x);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " ");
	NumberObject_Init(number, self->streamWriter, y - radius);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " c");
	self->streamWriter->WriteNewLine(self->streamWriter);	

	//third quadrant
	NumberObject_Init(number, self->streamWriter, x - kappa);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " ");
	NumberObject_Init(number, self->streamWriter, y - radius);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " ");
	NumberObject_Init(number, self->streamWriter, x - radius);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " ");
	NumberObject_Init(number, self->streamWriter, y - kappa);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " ");
	NumberObject_Init(number, self->streamWriter, x - radius);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " ");
	NumberObject_Init(number, self->streamWriter, y);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " c");
	self->streamWriter->WriteNewLine(self->streamWriter);	

	//second quadrant
	NumberObject_Init(number, self->streamWriter, x - radius);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " ");
	NumberObject_Init(number, self->streamWriter, y + kappa);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " ");
	NumberObject_Init(number, self->streamWriter, x - kappa);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " ");
	NumberObject_Init(number, self->streamWriter, y + radius);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " ");
	NumberObject_Init(number, self->streamWriter, x);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " ");
	NumberObject_Init(number, self->streamWriter, y + radius);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " c");
	self->streamWriter->WriteNewLine(self->streamWriter);	

	//first quadrant
	NumberObject_Init(number, self->streamWriter, x + kappa);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " ");
	NumberObject_Init(number, self->streamWriter, y + radius);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " ");
	NumberObject_Init(number, self->streamWriter, x + radius);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " ");
	NumberObject_Init(number, self->streamWriter, y + kappa);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " ");
	NumberObject_Init(number, self->streamWriter, x + radius);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " ");
	NumberObject_Init(number, self->streamWriter, y);
	NumberObject_Write(number);
	self->streamWriter->WriteData(self->streamWriter, " c");
	self->streamWriter->WriteNewLine(self->streamWriter);	

	if(!asClippingPath)
	{
		self->streamWriter->WriteData(self->streamWriter, " b");
		self->streamWriter->WriteNewLine(self->streamWriter);	
	}

	NumberObject_Destroy(number);
}


DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_SetImage(struct PdfGraphicWriter *self, struct PdfImage *image, 
													   float posX, float posY, float width, float height)
{
	struct NameObject *name = NameObject_Create(self->streamWriter,"0");
	struct NumberObject *number = NumberObject_Create(self->streamWriter,0);		

	self->streamWriter->WriteData(self->streamWriter, "q");	
	self->streamWriter->WriteNewLine(self->streamWriter);	

	NumberObject_Init(number,self->streamWriter, width);
	NumberObject_Write(number);
	NumberObject_Init(number,self->streamWriter, 0);
	NumberObject_Write(number);
	NumberObject_Init(number,self->streamWriter, 0);
	NumberObject_Write(number);
	NumberObject_Init(number,self->streamWriter, -height);
	NumberObject_Write(number);
	NumberObject_Init(number,self->streamWriter, posX);
	NumberObject_Write(number);
	NumberObject_Init(number,self->streamWriter, posY);
	NumberObject_Write(number);	
	self->streamWriter->WriteData(self->streamWriter, "cm");
	self->streamWriter->WriteNewLine(self->streamWriter);	

	NameObject_Init(name, self->streamWriter, image->name);
	NameObject_Write(name);

	self->streamWriter->WriteData(self->streamWriter, "Do");	
	self->streamWriter->WriteNewLine(self->streamWriter);	

	self->streamWriter->WriteData(self->streamWriter, "Q");
	self->streamWriter->WriteNewLine(self->streamWriter);	
}


DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_SetImageTransformed(struct PdfGraphicWriter *self, struct PdfImage *image, float locX, float locY, float width, float height, float angle)
{
	struct NameObject *name = NameObject_Create(self->streamWriter,"0");
	struct NumberObject *number = NumberObject_Create(self->streamWriter,0);		
	struct TransformationMatrix *rotMat, *transMat, *scaleMat, *resMat, *resMat2;

	rotMat = TransformationMatrix_Create();
	TransformationMatrix_Rotate(rotMat, angle);
	transMat = TransformationMatrix_Create();	
	scaleMat = TransformationMatrix_Create();
	TransformationMatrix_Scale(scaleMat, width, height);
	resMat = TransformationMatrix_Create();
	resMat2 = TransformationMatrix_Create();

	// multiply scale by rotation
	TransformationMatrix_Translate(transMat, -width/2, height/2);
	TransformationMatrix_Multiply(scaleMat, transMat, resMat);	

	TransformationMatrix_Multiply(resMat, rotMat, resMat2);	

	// multiply result with translation
	//TransformationMatrix_Multiply(resMat, transMat, resMat2);

	TransformationMatrix_Translate(transMat, locX, locY);
	TransformationMatrix_Multiply(resMat2, transMat, resMat);


	self->streamWriter->WriteData(self->streamWriter, "q");	
	self->streamWriter->WriteNewLine(self->streamWriter);			
 
	NumberObject_Init(number,self->streamWriter, resMat->a);
	NumberObject_Write(number);
	NumberObject_Init(number,self->streamWriter, resMat->b);
	NumberObject_Write(number);
	NumberObject_Init(number,self->streamWriter, -resMat->c);
	NumberObject_Write(number);
	NumberObject_Init(number,self->streamWriter, -resMat->d);
	NumberObject_Write(number);
	NumberObject_Init(number,self->streamWriter, resMat->e);
	NumberObject_Write(number);
	NumberObject_Init(number,self->streamWriter, resMat->f);
	NumberObject_Write(number);	
	self->streamWriter->WriteData(self->streamWriter, "cm");
	self->streamWriter->WriteNewLine(self->streamWriter);	

	NameObject_Init(name, self->streamWriter, image->name);
	NameObject_Write(name);

	self->streamWriter->WriteData(self->streamWriter, "Do");	
	self->streamWriter->WriteNewLine(self->streamWriter);	

	self->streamWriter->WriteData(self->streamWriter, "Q");
	self->streamWriter->WriteNewLine(self->streamWriter);	

	TransformationMatrix_Destroy(rotMat);
	TransformationMatrix_Destroy(scaleMat);
	TransformationMatrix_Destroy(transMat);
	TransformationMatrix_Destroy(resMat);
	TransformationMatrix_Destroy(resMat2);
}

DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_SetTransformation(struct PdfGraphicWriter *self, float a, float b, float c, 
																float d, float e, float f)
{
	struct NumberObject *number = NumberObject_Create(self->streamWriter,0);		

	NumberObject_Init(number,self->streamWriter, a);
	NumberObject_Write(number);
	NumberObject_Init(number,self->streamWriter, b);
	NumberObject_Write(number);
	NumberObject_Init(number,self->streamWriter, c);
	NumberObject_Write(number);
	NumberObject_Init(number,self->streamWriter, d);
	NumberObject_Write(number);
	NumberObject_Init(number,self->streamWriter, e);
	NumberObject_Write(number);
	NumberObject_Init(number,self->streamWriter, f);
	NumberObject_Write(number);	
	self->streamWriter->WriteData(self->streamWriter, "cm");
	self->streamWriter->WriteNewLine(self->streamWriter);

	NumberObject_Destroy(number);
}


DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_SetImageWithTransformation(struct PdfGraphicWriter *self, struct PdfImage *image, 
																		 float a, float b, float c, float d, float e, float f)
{
	struct NameObject *name = NameObject_Create(self->streamWriter,"0");
	struct NumberObject *number = NumberObject_Create(self->streamWriter,0);		

	self->streamWriter->WriteData(self->streamWriter, "q");	
	self->streamWriter->WriteNewLine(self->streamWriter);			

	NumberObject_Init(number,self->streamWriter, a);
	NumberObject_Write(number);
	NumberObject_Init(number,self->streamWriter, b);
	NumberObject_Write(number);
	NumberObject_Init(number,self->streamWriter, -c);
	NumberObject_Write(number);
	NumberObject_Init(number,self->streamWriter, -d);
	NumberObject_Write(number);
	NumberObject_Init(number,self->streamWriter, e);
	NumberObject_Write(number);
	NumberObject_Init(number,self->streamWriter, f);
	NumberObject_Write(number);	
	self->streamWriter->WriteData(self->streamWriter, "cm");
	self->streamWriter->WriteNewLine(self->streamWriter);	

	NameObject_Init(name, self->streamWriter, image->name);
	NameObject_Write(name);

	self->streamWriter->WriteData(self->streamWriter, "Do");	
	self->streamWriter->WriteNewLine(self->streamWriter);	

	self->streamWriter->WriteData(self->streamWriter, "Q");
	self->streamWriter->WriteNewLine(self->streamWriter);	

}



DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_SetShading(struct PdfGraphicWriter *self, const char *shadingName)
{
	char shadingString[50];
	
	sprintf(shadingString, "/%s sh", shadingName);

	self->streamWriter->WriteNewLine(self->streamWriter);
	self->streamWriter->WriteData(self->streamWriter, shadingString);
	self->streamWriter->WriteNewLine(self->streamWriter);
}



DLLEXPORT_TEST_FUNCTION void PdfGraphicWriter_SetClippingPath(struct PdfGraphicWriter *self)
{
	self->streamWriter->WriteData(self->streamWriter, "W n");//setting drawn shapes to be clipping path...
	self->streamWriter->WriteNewLine(self->streamWriter);
}