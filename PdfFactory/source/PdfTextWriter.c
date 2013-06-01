#include "PdfTextWriter.h"
#include "PdfDocument.h"
#include "PdfFont.h"
#include "PdfTemplateFont.h"
#include "NumberObject.h"
#include "StringObject.h"
#include "NameObject.h"
#include "UnitConverter.h"
#include "MemoryManager.h"


DLLEXPORT_TEST_FUNCTION struct PdfTextWriter* PdfTextWriter_Begin(struct StreamWriter *streamWriter)
{
	struct PdfTextWriter *writer; 
	writer = (struct PdfTextWriter*)MemoryManager_Alloc(sizeof (struct PdfTextWriter));
	writer->streamWriter = streamWriter;

	// write text start
	writer->streamWriter->WriteData(writer->streamWriter, "BT");
	writer->streamWriter->WriteNewLine(writer->streamWriter);	
	return writer;
}


DLLEXPORT_TEST_FUNCTION void PdfTextWriter_EndText(struct PdfTextWriter *self)
{
	// write text start	
	self->streamWriter->WriteData(self->streamWriter, "ET");
	self->streamWriter->WriteNewLine(self->streamWriter);	
	PdfTextWriter_Destroy(self);
}

DLLEXPORT_TEST_FUNCTION void PdfTextWriter_WriteText(struct PdfTextWriter *self, char *text)
{
	struct StringObject *sText; 
	char *cText;

	cText = CheckText(text);
	sText = StringObject_Create(self->streamWriter, cText);
	StringObject_Write(sText);
	StringObject_Destroy(sText);

	self->streamWriter->WriteData(self->streamWriter, "Tj");	
	self->streamWriter->WriteNewLine(self->streamWriter);	
}

DLLEXPORT_TEST_FUNCTION void PdfTextWriter_WriteUnicodeText(struct PdfTextWriter *self, short *text)
{

}


/*
  Items use this function to set fonts
*/
DLLEXPORT_TEST_FUNCTION void PdfTextWriter_SetFontTemplated(struct PdfTextWriter *self, struct PdfFont *font, struct PdfTemplateFont *templateFont)
{
	if (templateFont)
	{
		PdfTextWriter_SetFont(self, font, templateFont->size);
		// TODO: check for RGB or CMYK color and set appropriate. Currently only RGB
		PdfTextWriter_SetRGBColor(self, templateFont->colorR, templateFont->colorG, templateFont->colorB);
	}	
}

/* 
  this will write something like this
  /F1 12 Tf
*/
DLLEXPORT_TEST_FUNCTION void PdfTextWriter_SetFont(struct PdfTextWriter *self, struct PdfFont *font, char *size)
{
   // set current font
	struct NameObject *name;
	struct NumberObject *num;
	struct UnitConverter *unitConverter;
	double dSize;

	unitConverter = UnitConverter_Create();
	UnitConverter_AddCommonUnits(unitConverter);

	name = NameObject_Create(self->streamWriter, font->name);
	NameObject_Write(name);
	NameObject_Destroy(name);
	
	dSize = UnitConverter_ConvertToPoints(unitConverter, size);
	

	num = NumberObject_Create(self->streamWriter, dSize);
	NumberObject_Write(num);
	NumberObject_Destroy(num);

	
	self->streamWriter->WriteData(self->streamWriter, "Tf");
	self->streamWriter->WriteNewLine(self->streamWriter);	

	UnitConverter_Destroy(unitConverter);
}

DLLEXPORT_TEST_FUNCTION void PdfTextWriter_SetRGBColor(struct PdfTextWriter *self, unsigned char red, unsigned char green, unsigned char blue)
{
	char colourText[19];
	sprintf(colourText, "%.2f %.2f %.2f rg\n", (float)red / 255, (float)green / 255, (float)blue / 255);
	self->streamWriter->WriteData(self->streamWriter, colourText);	
}

DLLEXPORT_TEST_FUNCTION void PdfTextWriter_NewLine(struct PdfTextWriter *self)
{
	self->streamWriter->WriteData(self->streamWriter, "T*");	
	self->streamWriter->WriteNewLine(self->streamWriter);	
}

/*
Sets the following text matrix 
[ a b 0 
c d 0 
e f 1 ]
*/
DLLEXPORT_TEST_FUNCTION void PdfTextWriter_SetTextMatrix(struct PdfTextWriter *self, float a, float b, float c, float d, float e, float f)
{
	struct NumberObject *number;
	number = NumberObject_Create(self->streamWriter, a);
	NumberObject_Write(number);
	NumberObject_Destroy(number);

	number = NumberObject_Create(self->streamWriter, b);
	NumberObject_Write(number);
	NumberObject_Destroy(number);

	number = NumberObject_Create(self->streamWriter, c);
	NumberObject_Write(number);
	NumberObject_Destroy(number);

	number = NumberObject_Create(self->streamWriter, d);
	NumberObject_Write(number);
	NumberObject_Destroy(number);

	number = NumberObject_Create(self->streamWriter, e);
	NumberObject_Write(number);
	NumberObject_Destroy(number);

	number = NumberObject_Create(self->streamWriter, f);
	NumberObject_Write(number);
	NumberObject_Destroy(number);
	
	self->streamWriter->WriteData(self->streamWriter, "Tm");
	self->streamWriter->WriteNewLine(self->streamWriter);	
}


DLLEXPORT_TEST_FUNCTION void PdfTextWriter_SetPosition(struct PdfTextWriter *self, int x, int y)
{
	struct NumberObject *number;
	number = NumberObject_Create(self->streamWriter, x);
	NumberObject_Write(number);
	NumberObject_Destroy(number);

	number = NumberObject_Create(self->streamWriter, y);
	NumberObject_Write(number);
	NumberObject_Destroy(number);
	
	self->streamWriter->WriteData(self->streamWriter, "Td");
	self->streamWriter->WriteNewLine(self->streamWriter);	
}

void PdfTextWriter_Destroy(struct PdfTextWriter *self)
{
	MemoryManager_Free(self);	
}



char *CheckText(const char *text)
{
	unsigned int strSize, i, tmpCount, inCount, outCount;
	char *ret;
	char replace;
	replace = 0;
	ret = 0;
	tmpCount = 0;
	inCount = 0;
	outCount = 0;

	strSize = strlen(text);
	ret = MemoryManager_Alloc(strSize * 2);
	ret[0] = '\0';

	for(i=0; i<strSize; i++)
	{
		replace = 1;

		if(text[i] == '\\')
		{
			sprintf(ret+i+tmpCount,"%s","\\\\");
			tmpCount++;
			replace = 0;
		}
		
		if(text[i] == '(')
		{
			inCount++;
			replace = 1;
		}else if(text[i] == ')')
		{
			outCount++;
			replace = 1;
		}
		
		if(replace == 1)
		{
			MemoryManager_MemCpy(ret+i+tmpCount,text+i,1);
			ret[i+1+tmpCount]='\0';
		}
	}

	if(inCount != outCount)
	{
		sprintf(ret,"Text can't be writen because ( = %d and ) = %d",inCount, outCount); 
	}

	return ret;
}