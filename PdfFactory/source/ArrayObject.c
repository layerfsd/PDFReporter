/*
ArrayObject.c

Author: Nebojsa Vislavski
Date: 30.6.2008.	

ArrayObject syntax

*/

#include "ArrayObject.h"
#include "MemoryManager.h"
#include <stdlib.h>

DLLEXPORT_TEST_FUNCTION struct ArrayObject* ArrayObject_BeginArray(struct StreamWriter *streamWriter)
{
	struct ArrayObject *arrayObject;
	arrayObject = (struct ArrayObject*)MemoryManager_Alloc(sizeof(struct ArrayObject));
	// set values
	arrayObject->streamWriter = streamWriter;
	
	arrayObject->streamWriter->WriteData(arrayObject->streamWriter, "[");	
	return arrayObject;
}

DLLEXPORT_TEST_FUNCTION void ArrayObject_EndArray(struct ArrayObject *arrayObject)
{
	arrayObject->streamWriter->WriteData(arrayObject->streamWriter, "]");	
	MemoryManager_Free(arrayObject);	
}

