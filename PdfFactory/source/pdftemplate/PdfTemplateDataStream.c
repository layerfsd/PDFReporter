/*
PdfTemplateDataStream.c

Author: Marko Vranjkovic
Date: 25.07.2008.	

*/

#include "PdfTemplateDataStream.h"
#include "DLList.h"
#include "PdfTemplateElements.h"
#include "MemoryManager.h"
#include "PdfTemplate.h"

DLLEXPORT_TEST_FUNCTION void PdfTemplateDataStream_Write(struct PdfTemplateDataStream* self)
{
}

DLLEXPORT_TEST_FUNCTION struct PdfTemplateDataStream* PdfTemplateDataStream_Create(char *name)
{
	struct PdfTemplateDataStream *ret;
	ret = (struct PdfTemplateDataStream*)MemoryManager_Alloc(sizeof(struct PdfTemplateDataStream));	
	PdfTemplateDataStream_Init(ret, name);	
	return ret;
}

DLLEXPORT_TEST_FUNCTION struct PdfTemplateDataStream* PdfTemplateDataStream_CreateFromXml(xmlNode *node)
{
	struct PdfTemplateDataStream *ret;
	xmlNode *currentNode;
	xmlNode *columnsNode;
	struct PdfTemplateColumn *column;
	ret = (struct PdfTemplateDataStream*)MemoryManager_Alloc(sizeof(struct PdfTemplateDataStream));	

	PdfTemplateDataStream_Init(ret, PdfTemplate_LoadStringAttribute(node, NAME));	

	columnsNode = PdfTemplate_FindNode(node, COLUMNS);

	if (columnsNode)
	{	
		for(currentNode = columnsNode->children; currentNode; currentNode = currentNode->next)
		{
			if (currentNode->type == XML_ELEMENT_NODE)
			{
				if (strcmp(currentNode->name, COLUMN) == 0)
				{
					column = PdfTemplateColumn_CreateFromXml(currentNode);
					if (column)
					{
						DLList_PushBack(ret->columns, column);
					}
					else
					{
						return FALSE;
					}
				}
			}
		}
	}
	else 
	{
#ifdef _DEBUG
		printf("PdfTemplateDataStream: CreateFromXml: columns node missing\n");
#endif
		return FALSE;
	}

	return ret;
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateDataStream_Init(struct PdfTemplateDataStream* self, char* name)
{
	self->columns = DLList_Create();
	self->name = MemoryManager_StrDup(name);			
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateDataStream_Cleanup(struct PdfTemplateDataStream* self)
{
	if (self->name)
	{
		MemoryManager_Free(self->name);
		self->name = 0;
	}
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateDataStream_Destroy(struct PdfTemplateDataStream* self)
{
	PdfTemplateDataStream_Cleanup(self);

	// destroy all content streams	
	while(self->columns->size > 0)
	{
		struct PdfTemplateColumn *obj;
		obj = (struct PdfTemplateColumn *)DLList_Back(self->columns);
		DLList_PopBack(self->columns);
		PdfTemplateColumn_Destroy(obj);
	}
	DLList_Destroy(self->columns); // destroy list itself
	MemoryManager_Free(self);
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateDataStream_AddColumn(struct PdfTemplateDataStream* self, char* nameOfColumn, char* typeOfColumn)
{	
	struct PdfTemplateColumn* column = PdfTemplateColumn_Create(nameOfColumn, typeOfColumn);
	DLList_PushBack(self->columns, column);	
}