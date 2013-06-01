/*
ArrayObject.h

Author: Nebojsa Vislavski
Date: 30.6.2008.	

ArrayObject syntax

*/

#ifndef _ARRAYOBJECT_
#define _ARRAYOBJECT_

#include "PdfDocument.h"
#include "PdfFactory.h"
#include "NameObject.h"
#include "StreamWriter.h"

struct ArrayObject
{
	struct StreamWriter *streamWriter;
};

DLLEXPORT_TEST_FUNCTION struct ArrayObject* ArrayObject_BeginArray(struct StreamWriter *writer);
DLLEXPORT_TEST_FUNCTION void ArrayObject_EndArray(struct ArrayObject *arrayObject);


#endif
