//-----------------------------------------------------------------------------
// Name:	PdfTemplateItemShapeLine.c
// Author:	Tomislav Kukic
// Date:	18.12.2008
//-----------------------------------------------------------------------------


#include "PdfTemplateItemShapeLine.h"
#include "PdfTemplateElements.h"
#include "PdfTemplate.h"
#include "PdfGenerator.h"
#include "PdfTemplateBalloon.h"
#include "PdfTemplateBalloonItem.h"
#include "GraphicWriter.h"
#include "PdfPage.h"
#include "UnitConverter.h"
#include "MemoryManager.h"
#include "TransformationMatrix.h"
#include <libxml/parser.h>
#include <libxml/xmlmemory.h>
#include "PdfGeneratedBalloon.h"



DLLEXPORT_TEST_FUNCTION struct PdfTemplateItemShapeLine* PdfTemplateItemShapeLine_CreateFromXml(xmlNode *node)
{
	struct PdfTemplateItemShapeLine *ret;
	xmlNode *foundNode;

	ret = (struct PdfTemplateItemShapeLine*)MemoryManager_Alloc(sizeof(struct PdfTemplateItemShapeLine));
	ret->X2 = 0;
	ret->Y2 = 0;

	PdfTemplateBalloonItem_InitFromXml((struct PdfTemplateBalloonItem*)ret, node, PdfTemplateItemShapeLine_Destroy, PdfTemplateItemShapeLine_Process);
	
	foundNode = PdfTemplate_FindNode(node, LINE_X2);
	if(foundNode)
	{
		ret->X2 = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(foundNode, VALUE));
	}else{
		return NULL;
	}

	foundNode = PdfTemplate_FindNode(node, LINE_Y2);
	if(foundNode)
	{
		ret->Y2 = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(foundNode, VALUE));
	}else{
		return NULL;
	}

	ret->Width = PdfTemplate_LoadIntValue(node, WIDTH);
	
	return ret;
}






DLLEXPORT_TEST_FUNCTION void PdfTemplateItemShapeLine_Destroy(struct PdfTemplateBalloonItem* self)
{
	PdfTemplateItemShapeLine_Cleanup((struct PdfTemplateItemShapeLine*)self);

	// destroy 
	MemoryManager_Free((struct PdfTemplateItemShapeLine*)self);
}





DLLEXPORT_TEST_FUNCTION void PdfTemplateItemShapeLine_Cleanup(struct PdfTemplateItemShapeLine* self)
{
	if(self->X2)
	{
		MemoryManager_Free(self->X2);
		self->X2 = 0;
	}
	
	if(self->Y2)
	{
		MemoryManager_Free(self->Y2);
		self->Y2 = 0;
	}

	// cleanup parent
	PdfTemplateBalloonItem_Cleanup((struct PdfTemplateBalloonItem*)self);
}






int PdfTemplateItemShapeLine_Process(struct PdfTemplateBalloonItem *self, struct PdfGenerator *generator, struct PdfGeneratedBalloon *balloon, struct StreamWriter *streamWriter)
{
	struct PdfGraphicWriter* gW;
	struct PdfTemplateItemShapeLine* item;
	struct TransformationMatrix* tM;
	struct UnitConverter *uC;
	double tmpX, tmpY;

	item = (struct PdfTemplateItemShapeLine*)self;
	tM = TransformationMatrix_Create();
	uC = UnitConverter_Create();
	UnitConverter_AddCommonUnits(uC);

	PdfTemplateBalloonItem_GetFullTransformation(self, balloon, generator->currentPage, tM, FALSE);
	tmpX = UnitConverter_ConvertToPoints(uC, item->X2);
	tmpY = UnitConverter_ConvertToPoints(uC, item->Y2);
	

	gW = PdfGraphicWriter_Create(streamWriter);
	PdfGraphicWriter_SetLineWidth(gW, item->Width);
	PdfGraphicWriter_DrawLine(gW, tM->e, tM->f, tmpX, tM->f - tmpY);
	return TRUE;
}
