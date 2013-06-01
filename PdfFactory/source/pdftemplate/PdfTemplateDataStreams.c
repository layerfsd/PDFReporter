/*
PdfTemplateDataStreams.c

Author: Marko Vranjkovic
Date: 30.07.2008.	

*/

#include "PdfTemplateDataStreams.h"
#include "DLList.h"
#include <libxml/parser.h>
#include <libxml/xmlmemory.h>
#include "PdfTemplateElements.h"
#include "MemoryManager.h"
#include "PdfTemplate.h"

DLLEXPORT_TEST_FUNCTION void PdfTemplateDataStreams_Write(struct PdfTemplateDataStreams* self)
{
} 

DLLEXPORT_TEST_FUNCTION struct PdfTemplateDataStreams* PdfTemplateDataStreams_Create()
{
	struct PdfTemplateDataStreams *ret;
	ret = (struct PdfTemplateDataStreams*)MemoryManager_Alloc(sizeof(struct PdfTemplateDataStreams));
	PdfTemplateDataStreams_Init(ret, 1.0f);
	return ret;
}

int PdfTemplateDataStreams_LoadConnections1(struct PdfTemplateDataStreams *self, xmlNode *node)
{
	xmlNode *currentNode = NULL;
	struct PdfTemplateConnection *connection;
	for(currentNode = node->children; currentNode; currentNode = currentNode->next)
	{
		if (currentNode->type == XML_ELEMENT_NODE)
		{
			if (strcmp(currentNode->name, CONNECTION) == 0)
			{
				connection = PdfTemplateConnection_CreateFromXml(currentNode);
				if (connection)
				{
					DLList_PushBack(self->connections, connection);
				}
				else 
				{
					return FALSE;
				}
			}
		}
	}
	return TRUE;
}

int PdfTemplateDataStreams_LoadVersion1(struct PdfTemplateDataStreams *self, xmlNode *node)
{
	xmlNode *currentNode = NULL;
	struct PdfTemplateDataStream *dataStream;

	for(currentNode = node->children; currentNode; currentNode = currentNode->next)
	{
		if (currentNode->type == XML_ELEMENT_NODE)
		{
			if (strcmp(currentNode->name, DATA_STREAM) == 0)
			{
				dataStream = PdfTemplateDataStream_CreateFromXml(currentNode);
				if (dataStream)
				{
					DLList_PushBack(self->streams, dataStream);
				}
				else
				{
#ifdef _DEBUG
					printf("PdfTemplateDataStreams: LoadVersion1: Error creating data stream.\n");
#endif
					return FALSE;
				}
			}

			if (strcmp(currentNode->name, CONNECTIONS) == 0)
			{
				// Load connections
				if (!PdfTemplateDataStreams_LoadConnections1(self, currentNode))
				{
					return FALSE;
				}
			}
		}
	}

	return TRUE;
}

DLLEXPORT_TEST_FUNCTION struct PdfTemplateDataStreams* PdfTemplateDataStreams_CreateFromXml(xmlNode *node)
{
	struct PdfTemplateDataStreams *ret;
	ret = (struct PdfTemplateDataStreams*)MemoryManager_Alloc(sizeof(struct PdfTemplateDataStreams));
	
	PdfTemplateDataStreams_Init(ret, (float)atof(PdfTemplate_LoadStringAttribute(node, VERSION)));	
	
	// load version 1
	if (ret->version == 1.0f)
	{
		if (!PdfTemplateDataStreams_LoadVersion1(ret, node))
		{
#ifdef _DEBUG
			printf("PdfTemplateDAtaStreams: CreateFromXml: Failed loading version1\n");
#endif 
			PdfTemplateDataStreams_Destroy(ret);
			return FALSE;
		}
	}

	return ret;
}



DLLEXPORT_TEST_FUNCTION void PdfTemplateDataStreams_Init(struct PdfTemplateDataStreams* self, float versionParam)
{
	self->connections = DLList_Create();
	self->streams = DLList_Create();
	self->version = versionParam;
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateDataStreams_Destroy(struct PdfTemplateDataStreams* self)
{	
	// destroy all connections
	while(self->connections->size > 0)
	{
		struct PdfTemplateConnection *obj;
		obj = (struct PdfTemplateConnection *)DLList_Back(self->connections);
		DLList_PopBack(self->connections);
		PdfTemplateConnection_Destroy(obj);
	}
	DLList_Destroy(self->connections); // destroy list itself

	// destroy all content streams	
	while(self->streams->size > 0)
	{
		struct PdfTemplateDataStream *obj;
		obj = (struct PdfTemplateDataStream *)DLList_Back(self->streams);
		DLList_PopBack(self->streams);
		PdfTemplateDataStream_Destroy(obj);
	}
	DLList_Destroy(self->streams); // destroy list itself
	MemoryManager_Free(self);
}


// Add data stream to end of list
DLLEXPORT_TEST_FUNCTION struct PdfTemplateDataStream* PdfTemplateDataStreams_AddDataStream(struct PdfTemplateDataStreams* self, char* nameOfDataStream)
{
	struct PdfTemplateDataStream* dataStream = PdfTemplateDataStream_Create(nameOfDataStream);	
	DLList_PushBack(self->streams, dataStream);
	return dataStream;
}

// Add connection to end of list
DLLEXPORT_TEST_FUNCTION void PdfTemplateDataStreams_AddConnection(struct PdfTemplateDataStreams* self, struct PdfTemplateConnection* con)
{
	DLList_PushBack(self->connections, con);	
}