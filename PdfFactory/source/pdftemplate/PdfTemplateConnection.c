/*0
PdfTemplateConnection.c

Author: Marko Vranjkovic
Date: 18.08.2008.	

*/

#include "PdfTemplateConnection.h"
#include <libxml/parser.h>
#include <libxml/xmlmemory.h>
#include "PdfTemplateElements.h"
#include "MemoryManager.h"
#include "PdfTemplate.h"

DLLEXPORT_TEST_FUNCTION void PdfTemplateConnection_Write(struct PdfTemplateConnection* self)
{
}

DLLEXPORT_TEST_FUNCTION struct PdfTemplateConnection* PdfTemplateConnection_Create(char* childCol, char* childStream, char* parentCol, char* parentStream)
{
	struct PdfTemplateConnection *ret;
	ret = (struct PdfTemplateConnection*)MemoryManager_Alloc(sizeof(struct PdfTemplateConnection));
	PdfTemplateConnection_Init(ret, childCol, childStream, parentCol, parentStream);
	return ret;
}

DLLEXPORT_TEST_FUNCTION struct PdfTemplateConnection* PdfTemplateConnection_CreateFromXml(xmlNode *node)
{
	struct PdfTemplateConnection *ret;
	ret = (struct PdfTemplateConnection*)MemoryManager_Alloc(sizeof(struct PdfTemplateConnection));

	ret->parentDataStream = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(node, PARENT_DATA_STREAM));
	ret->childDataStream = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(node, CHILD_DATA_STREAM));
	ret->parentColumn = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(node, PARENT_COLUMN));
	ret->childColumn = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(node, CHILD_COLUMN));
	
	return ret;
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateConnection_Init(struct PdfTemplateConnection* self, char* childCol, char* childStream, char* parentCol, char* parentStream)
{
	self->childColumn = MemoryManager_StrDup(childCol);
	self->childDataStream = MemoryManager_StrDup(childStream);
	self->parentColumn = MemoryManager_StrDup(parentCol);
	self->parentDataStream = MemoryManager_StrDup(parentStream);
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateConnection_Destroy(struct PdfTemplateConnection* self)
{
	PdfTemplateConnection_Cleanup(self);
	MemoryManager_Free(self); 
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateConnection_Cleanup(struct PdfTemplateConnection* self)
{
	if (self->childColumn)
	{
		MemoryManager_Free(self->childColumn);
		self->childColumn = 0;
	}
	if (self->childDataStream)
	{
		MemoryManager_Free(self->childDataStream);
		self->childDataStream = 0;
	}
	if (self->parentColumn)
	{
		MemoryManager_Free(self->parentColumn);
		self->parentColumn = 0;
	}
	if (self->parentDataStream)
	{
		MemoryManager_Free(self->parentDataStream);
		self->parentDataStream = 0;
	}
}