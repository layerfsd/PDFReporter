/*
CrossReferenceTable.c

Author: Nebojsa Vislavski
Date: 30.6.2008.	

Writes cross reference table to pdf
*/

#include "CrossReferenceTable.h"
#include "IndirectObject.h"
#include "MemoryManager.h"
#include "DLList.h"
#include <string.h>
#include <stdlib.h>

DLLEXPORT_TEST_FUNCTION struct CrossReferenceTable* CrossReferenceTable_Create(struct PdfDocument *document)
{
	struct CrossReferenceTable *result;
	result = (struct CrossReferenceTable *)MemoryManager_Alloc(sizeof (struct CrossReferenceTable));
	result->document = document;
	result->beginOffset = 0;
	return result;
}

DLLEXPORT_TEST_FUNCTION void CrossReferenceTable_Write(struct CrossReferenceTable *self)
{
	char tmp[100];
	struct DLListNode *currentObject;
	struct IndirectObject *indirectObject;

	self->document->streamWriter->WriteNewLine(self->document->streamWriter);		
	self->beginOffset = self->document->streamWriter->GetPosition(self->document->streamWriter);

	self->document->streamWriter->WriteData(self->document->streamWriter, "xref");
	self->document->streamWriter->WriteNewLine(self->document->streamWriter);		
	// write first section. Current implementation only has one section
	
	sprintf(tmp, "%d %d", 0, self->document->objectsCount+1);
	self->document->streamWriter->WriteData(self->document->streamWriter, tmp);
	self->document->streamWriter->WriteNewLine(self->document->streamWriter);		
	
	
	sprintf(tmp, "%010d %05d f ", 0, 65535);
	self->document->streamWriter->WriteData(self->document->streamWriter, tmp);
	self->document->streamWriter->WriteNewLine(self->document->streamWriter);		

	// write each object position and generation number.
	// go trough list of document->indirectObjectList
	currentObject = DLList_Begin(self->document->indirectObjectList);
	while (currentObject != DLList_End(self->document->indirectObjectList))
	{
		// write object offset, generation number and n
		indirectObject = (struct IndirectObject *)currentObject->data;
		sprintf(tmp, "%010d %05d n ", (int)indirectObject->beginObjectOffset,
			(int)indirectObject->generationNumber);

		self->document->streamWriter->WriteData(self->document->streamWriter, tmp);
		self->document->streamWriter->WriteNewLine(self->document->streamWriter);		
		currentObject = currentObject->next;
	}
}

DLLEXPORT_TEST_FUNCTION void CrossReferenceTable_Destroy(struct CrossReferenceTable *self)
{	
	MemoryManager_Free(self);
}

