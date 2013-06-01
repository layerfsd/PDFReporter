/*
PdfTemplateConnection.h

Author: Marko Vranjkovic
Date: 18.08.2008.	

*/

#ifndef _PDFTEMPLATECONNECTION_
#define _PDFTEMPLATECONNECTION_

#include "PdfFactory.h"
#include "libxml/tree.h"

struct PdfTemplateConnection
{
	char* parentDataStream; //moze da se stavi i struct PdfTemplateDataStream* parentDataStream;
	char* parentColumn; //struct PdfTemplateColumn* parentColumn;
	char* childDataStream; //struct PdfTemplateDataStream* childDataStream;
	char* childColumn; //struct PdfTemplateColumn* childColumn;
};

DLLEXPORT_TEST_FUNCTION void PdfTemplateConnection_Write(struct PdfTemplateConnection* self);

DLLEXPORT_TEST_FUNCTION struct PdfTemplateConnection* PdfTemplateConnection_Create(char *childCol, char *childStream, char *parentCol, char *parentStream);
DLLEXPORT_TEST_FUNCTION struct PdfTemplateConnection* PdfTemplateConnection_CreateFromXml(xmlNode *node);

DLLEXPORT_TEST_FUNCTION void PdfTemplateConnection_Init(struct PdfTemplateConnection* self, char* childCol, char* childStream, char* parentCol, char* parentStream);

DLLEXPORT_TEST_FUNCTION void PdfTemplateConnection_Destroy(struct PdfTemplateConnection* self);

DLLEXPORT_TEST_FUNCTION void PdfTemplateConnection_Cleanup(struct PdfTemplateConnection* self);

#endif
