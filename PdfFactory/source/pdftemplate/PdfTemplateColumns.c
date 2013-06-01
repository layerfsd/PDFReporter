/*
PdfTemplateColumns.c

Author: Marko Vranjkovic
Date: 30.07.2008.	

*/

#include "PdfTemplateColumns.h"

DLLEXPORT void PdfTemplateColumns_Write()
{
}

DLLEXPORT struct PdfTemplateColumns* PdfTemplateColumns_Create()
{
	struct PdfTemplateColumn *ret;
	ret = (struct PdfTemplateColumns*)malloc(sizeof(struct PdfTemplateColumns));
	PdfTemplateColumns_Init(ret);
	return ret;
}

DLLEXPORT void PdfTemplateColumns_Init(struct PdfTemplateColumns* self)
{
	self->elements = NULL;
	self->name = " ";
}

DLLEXPORT void PdfTemplateColumns_Destroy(struct PdfTemplateColumns* self)
{
}

