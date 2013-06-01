/*
PdfTemplateDataStreams.h

Author: Marko Vranjkovic
Date: 30.07.2008.	

*/

#ifndef _PDFTEMPLATEDATASTREAMS_
#define _PDFTEMPLATEDATASTREAMS_

#include "PdfTemplateDataStream.h"
#include "PdfTemplateConnection.h"
#include <libxml/tree.h>

struct PdfTemplateDataStreams
{
	float version;
	struct DLList *streams;
	struct DLList* connections;
};

DLLEXPORT_TEST_FUNCTION void PdfTemplateDataStreams_Write(struct PdfTemplateDataStreams* self);

DLLEXPORT_TEST_FUNCTION struct PdfTemplateDataStreams* PdfTemplateDataStreams_Create();

DLLEXPORT_TEST_FUNCTION struct PdfTemplateDataStreams* PdfTemplateDataStreams_CreateFromXml(xmlNode *node);

DLLEXPORT_TEST_FUNCTION void PdfTemplateDataStreams_Init(struct PdfTemplateDataStreams* self, float versionParam);

DLLEXPORT_TEST_FUNCTION void PdfTemplateDataStreams_Destroy(struct PdfTemplateDataStreams* self);

DLLEXPORT_TEST_FUNCTION struct PdfTemplateDataStream* PdfTemplateDataStreams_AddDataStream(struct PdfTemplateDataStreams* self, char* nameOfDataStream);

DLLEXPORT_TEST_FUNCTION void PdfTemplateDataStreams_AddConnection(struct PdfTemplateDataStreams* self, struct PdfTemplateConnection* con);

#endif