/*
DictionaryObject.h

Author: Nebojsa Vislavski
Date: 30.6.2008.	

DictionaryObject syntax

*/

#ifndef _DICTIONARYOBJECT_
#define _DICTIONARYOBJECT_


#include "PdfFactory.h"
#include "StreamWriter.h"


struct DictionaryObject
{
	struct StreamWriter *streamWriter;
};

DLLEXPORT_TEST_FUNCTION struct DictionaryObject* DictionaryObject_Begin(struct StreamWriter *streamWriter);
DLLEXPORT_TEST_FUNCTION void DictionaryObject_End(struct DictionaryObject *dictionary);
DLLEXPORT_TEST_FUNCTION void DictionaryObject_WriteKey(struct DictionaryObject *dictionary, char *key);



#endif
