#include "PdfGeneratorDataStreamRow.h"
#include "PdfGeneratorDataStreamField.h"
#include "PdfGeneratorDataStreamColumn.h"
#include "PdfGeneratorDataStream.h"
#include "DLList.h"
#include "MemoryManager.h"
#include <stdlib.h>
#include <string.h>

DLLEXPORT_TEST_FUNCTION struct PdfGeneratorDataStreamRow *PdfGeneratorDataStreamRow_Create()
{
	struct PdfGeneratorDataStreamRow *x;
	x = (struct PdfGeneratorDataStreamRow*)MemoryManager_Alloc(sizeof(struct PdfGeneratorDataStreamRow));
	PdfGeneratorDataStreamRow_Init(x);
	return x;
}

DLLEXPORT_TEST_FUNCTION void PdfGeneratorDataStreamRow_Init(struct PdfGeneratorDataStreamRow *self)
{
	self->fields = DLList_Create();	
}

DLLEXPORT_TEST_FUNCTION void PdfGeneratorDataStreamRow_Destroy(struct PdfGeneratorDataStreamRow *self)
{
	while(self->fields->size > 0)
	{
		struct PdfGeneratorDataStreamField *obj;
		obj = (struct PdfGeneratorDataStreamField*)DLList_Back(self->fields);
		DLList_PopBack(self->fields);
		PdfGeneratorDataStreamField_Destroy(obj);		
	}
	DLList_Destroy(self->fields); // destroy list itself


	MemoryManager_Free(self);
}


DLLEXPORT_TEST_FUNCTION void PdfGeneratorDataStreamRow_AddFieldByColumn(struct PdfGeneratorDataStreamRow *self, struct PdfGeneratorDataStreamColumn *column, void *value)
{
	struct PdfGeneratorDataStreamField *field;
	field = PdfGeneratorDataStreamField_Create(column, value);
	DLList_PushBack(self->fields, field);
}

DLLEXPORT_TEST_FUNCTION void PdfGeneratorDataStreamRow_AddField(struct PdfGeneratorDataStreamRow *self, struct PdfGeneratorDataStream *stream, char *columnName, void *value)
{
	struct PdfGeneratorDataStreamField *field;
	struct PdfGeneratorDataStreamColumn *col;
	
	col = PdfGeneratorDataStream_FindColumn(stream, columnName);	
	field = PdfGeneratorDataStreamField_Create(col, value);
	DLList_PushBack(self->fields, field);
}


DLLEXPORT_TEST_FUNCTION void *PdfGeneratorDataStreamRow_GetFieldValue(struct PdfGeneratorDataStreamRow *self, char *columnName)
{
	struct DLListNode *iter;
	struct PdfGeneratorDataStreamField *field;
	for(iter = DLList_Begin(self->fields); iter != DLList_End(self->fields); iter = iter->next)
	{
		field = (struct PdfGeneratorDataStreamField*)iter->data;
		if (strcmp(field->column->name, columnName) == 0)
		{
			return field->value;
		}
	}
	return 0; /* not found */
}
