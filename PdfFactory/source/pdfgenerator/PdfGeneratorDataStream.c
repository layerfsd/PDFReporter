/*
PdfGeneratorDataSream.c

Author: Nebojsa Vislavski
Date: 12.08.2008.	

*/

#include "PdfGeneratorDataStream.h"
#include "PdfGeneratorDataStreamColumn.h"
#include "PdfGeneratorDataStreamRow.h"
#include "DLList.h"
#include "MemoryManager.h"
#include <stdlib.h>
#include <string.h>


DLLEXPORT_TEST_FUNCTION struct PdfGeneratorDataStream *PdfGeneratorDataStream_Create(char *name)
{
	struct PdfGeneratorDataStream *x;
	x = (struct PdfGeneratorDataStream*)MemoryManager_Alloc(sizeof(struct PdfGeneratorDataStream));
	PdfGeneratorDataStream_Init(x, name);
	return x;
}

DLLEXPORT_TEST_FUNCTION void PdfGeneratorDataStream_Init(struct PdfGeneratorDataStream *self, char *name)
{
	if (name)
	{
		strcpy(self->name, name);
	}
	else
	{
		strcpy(self->name, "\0");
	}
	
	self->rows = DLList_Create();
	self->columns = DLList_Create();
	self->currentRow = 0;
	self->childStream = 0;
	self->parentStream = 0;
}

DLLEXPORT_TEST_FUNCTION void PdfGeneratorDataStream_First(struct PdfGeneratorDataStream *self)
{
	self->currentRow = DLList_Begin(self->rows);
}


DLLEXPORT_TEST_FUNCTION int PdfGeneratorDataStream_Empty(struct PdfGeneratorDataStream *self)
{
	return self->rows->size == 0;
}

DLLEXPORT_TEST_FUNCTION int PdfGeneratorDataStream_End(struct PdfGeneratorDataStream *self)
{
	return self->currentRow == DLList_End(self->rows);
}

DLLEXPORT_TEST_FUNCTION void PdfGeneratorDataStream_Next(struct PdfGeneratorDataStream *self)
{
	if (self->currentRow != 0)
	{
		self->currentRow = self->currentRow->next;
	}
}


DLLEXPORT_TEST_FUNCTION void PdfGeneratorDataStream_Prev(struct PdfGeneratorDataStream *self)
{
	if (self->currentRow != 0)
	{
		self->currentRow = self->currentRow->prev;
	}
}


DLLEXPORT_TEST_FUNCTION int PdfGeneratorDataStream_Size(struct PdfGeneratorDataStream *self)
{
	return self->rows->size;
}



DLLEXPORT_TEST_FUNCTION void PdfGeneratorDataStream_Destroy(struct PdfGeneratorDataStream *self)
{
	while(self->rows->size > 0)
	{
		struct PdfGeneratorDataStreamRow *obj;
		obj = (struct PdfGeneratorDataStreamRow*)DLList_Back(self->rows);
		DLList_PopBack(self->rows);
		PdfGeneratorDataStreamRow_Destroy(obj);		
	}
	DLList_Destroy(self->rows); // destroy list itself

	while(self->columns->size > 0)
	{
		struct PdfGeneratorDataStreamColumn *obj;
		obj = (struct PdfGeneratorDataStreamColumn*)DLList_Back(self->columns);
		DLList_PopBack(self->columns);
		PdfGeneratorDataStreamColumn_Destroy(obj);		
	}
	DLList_Destroy(self->columns); // destroy list itself

	MemoryManager_Free(self);
}


int CheckColumnExists(struct PdfGeneratorDataStream *self, char *name)
{
	struct DLListNode *iter;
	struct PdfGeneratorDataStreamColumn *col;
	for(iter = DLList_Begin(self->columns); iter != DLList_End(self->columns); iter = iter->next)
	{
		col = (struct PdfGeneratorDataStreamColumn*)iter->data;
		if (strcmp(col->name, name) == 0)
		{
			return TRUE;
		}
	}
	return FALSE;
}

DLLEXPORT_TEST_FUNCTION struct PdfGeneratorDataStreamColumn* PdfGeneratorDataStream_AddColumn(struct PdfGeneratorDataStream *self, int type, int length, char *name)
{
	struct PdfGeneratorDataStreamColumn *col; 
	if (!CheckColumnExists(self, name))
	{
		col = PdfGeneratorDataStreamColumn_Create(type, length, name);
		DLList_PushBack(self->columns, col);
		return col;
	}
	else 
	{
		return 0;
	}	
}

DLLEXPORT_TEST_FUNCTION struct PdfGeneratorDataStreamColumn* PdfGeneratorDataStream_FindColumn(struct PdfGeneratorDataStream *self, char *name)
{
	struct DLListNode *iter;
	struct PdfGeneratorDataStreamColumn *col;
	for(iter = DLList_Begin(self->columns); iter != DLList_End(self->columns); iter = iter->next)
	{
		col = (struct PdfGeneratorDataStreamColumn*)iter->data;
		if (strcmp(col->name, name) == 0)
		{
			return col;
		}
	}
	return 0;
}

DLLEXPORT_TEST_FUNCTION struct PdfGeneratorDataStreamColumn* PdfGeneratorDataStream_GetColumn(struct PdfGeneratorDataStream *self, char *name)
{
	struct PdfGeneratorDataStreamColumn *col; 
	struct DLListNode *iter;
	col = 0;
	for(iter = DLList_Begin(self->columns); iter != DLList_End(self->columns); iter = iter->next)
	{
		col = (struct PdfGeneratorDataStreamColumn*)iter->data;
		if (strcmp(col->name, name) == 0)
		{
			return col;
		}
	}
	return 0;
}

DLLEXPORT_TEST_FUNCTION struct PdfGeneratorDataStreamRow *PdfGeneratorDataStream_AddRow(struct PdfGeneratorDataStream *self)
{
	struct PdfGeneratorDataStreamRow *row;
	row = PdfGeneratorDataStreamRow_Create();
	DLList_PushBack(self->rows, row);
	return row;
}

DLLEXPORT_TEST_FUNCTION struct PdfGeneratorDataStreamRow *PdfGeneratorDataStream_GetCurrentRow(struct PdfGeneratorDataStream *self)
{
	if (self->currentRow != 0)
	{
		return (struct PdfGeneratorDataStreamRow*) self->currentRow->data;
	}
	else 
	{
		return 0;
	}
}


DLLEXPORT_TEST_FUNCTION void PdfGeneratorDataStream_AddChild(struct PdfGeneratorDataStream *self, struct PdfGeneratorDataStream *childStream)
{
	// TODO: Missing connection description
	self->childStream = childStream;
	childStream->parentStream = self;
}

DLLEXPORT_TEST_FUNCTION struct PdfGeneratorDataStreamRow *PdfGeneratorDataStream_GetRow(struct PdfGeneratorDataStream *self, int index)
{
	struct DLListNode *iter;
	int i = 0;
	for(iter = DLList_Begin(self->rows); iter != DLList_End(self->rows); iter = iter->next)
	{
		if (i == index)
		{
			return (struct PdfGeneratorDataStreamRow *)iter->data;
		}
		i++;
	}
	return 0;
}
