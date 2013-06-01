/*
   Autor: Nebojsa Vislavski
   Date: 18.8.2008 

   */

#ifndef _PdfGeneratorDataStreamField_
#define _PdfGeneratorDataStreamField_

#include "PdfFactory.h"

struct PdfGeneratorDataStreamField
{
	void *value; // field value
	struct PdfGeneratorDataStreamColumn *column;  // what is column for this field
};

struct PdfGeneratorDataStreamField *PdfGeneratorDataStreamField_Create(struct PdfGeneratorDataStreamColumn *column, void *value);
/* Create new field */

void PdfGeneratorDataStreamField_Init(struct PdfGeneratorDataStreamField *self, struct PdfGeneratorDataStreamColumn *column, void *value);
void PdfGeneratorDataStreamField_Destroy(struct PdfGeneratorDataStreamField *self);



#endif
