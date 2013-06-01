/*  
   Copyright AxiomCoders
*/

#include "PdfTemplateItemBorder.h"
#include "PdfTemplateElements.h"
#include "DLList.h"
#include "MemoryManager.h"
#include "PdfTemplate.h"
#include <libxml/parser.h>
#include <libxml/xmlmemory.h>

DLLEXPORT_TEST_FUNCTION void PdfTemplateItemBorder_LoadFromXml(struct PdfTemplateItemBorder *self, xmlNode *node)
{	
	self->enabled = PdfTemplate_LoadBooleanAttribute(node, ENABLED);
	self->r = (float)PdfTemplate_LoadIntAttribute(node, RGBCOLOR_R_COMPONENT) / 255.0f;
	self->g = (float)PdfTemplate_LoadIntAttribute(node, RGBCOLOR_G_COMPONENT)/ 255.0f;
	self->b = (float)PdfTemplate_LoadIntAttribute(node, RGBCOLOR_B_COMPONENT)/ 255.0f;
	self->width = (float)PdfTemplate_LoadDoubleAttribute(node, WIDTH);
}
