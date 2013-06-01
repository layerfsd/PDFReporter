/*
PdfTemplateLocation.c

Author: Marko Vranjkovic
Date: 19.08.2008.	

*/

#include "PdfTemplateLocation.h"
#include "PdfTemplateElements.h"
#include "MemoryManager.h"
#include "PdfTemplate.h"

DLLEXPORT_TEST_FUNCTION void PdfTemplateLocation_Write()
{
}

DLLEXPORT_TEST_FUNCTION struct PdfTemplateLocation* PdfTemplateLocation_Create()
{
	struct PdfTemplateLocation *ret;
	char position[100];

	ret = (struct PdfTemplateLocation*)MemoryManager_Alloc(sizeof(struct PdfTemplateLocation));		
	strcpy(position, "0");
	ret->positionX = MemoryManager_StrDup(position);
	ret->positionY = MemoryManager_StrDup(position);
	return ret;
}

DLLEXPORT_TEST_FUNCTION struct PdfTemplateLocation* PdfTemplateLocation_CreateFromXml(xmlNode *node)
{
	struct PdfTemplateLocation *ret;
	ret = (struct PdfTemplateLocation*)MemoryManager_Alloc(sizeof(struct PdfTemplateLocation));	
	ret->positionX = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(node, POSITION_X));
	ret->positionY = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(node, POSITION_Y));
	return ret;
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateLocation_Init(struct PdfTemplateLocation* self)
{
}

DLLEXPORT_TEST_FUNCTION void PdfTemplateLocation_Destroy(struct PdfTemplateLocation* self)
{
	if (self->positionX)
	{
		MemoryManager_Free(self->positionX);
		self->positionX = 0;
	}
	if (self->positionY)
	{
		MemoryManager_Free(self->positionY);
		self->positionY = 0;
	}
	MemoryManager_Free(self);
}
