/*
 ---------------------------------------
| File:		PdfPrecalculatedItem.h		|
| Author:	Tomislav Kukic				|
| Date:		14.1.2009					|
 ---------------------------------------
*/



#ifndef _PDF_PRECALCULATED_ITEM_
#define _PDF_PRECALCULATED_ITEM_


#include "PdfFactory.h"
#include "PdfTemplateBalloonItem.h"
#include "PdfPrecalculatedFunction.h"
#include <libxml/tree.h>

struct PdfPrecalculatedItem
{
	struct PdfTemplateBalloonItem base;
	struct DLList *functions;

	char* formula;
	char parsedFormula[255];

	struct PdfTemplateFont *font;
};

struct DLList *precalculatedItems;

void PdfPrecalculatedItem_Static_AddPrecalculatedItem(struct PdfPrecalculatedItem *newItem);
void PdfPrecalculatedItem_Static_RemovePrecalculatedItem(struct PdfPrecalculatedItem *newItem);
void PdfPrecalculatedItem_Static_AddValue(const char *dataStreamName, const char *columnName, float value);

DLLEXPORT_TEST_FUNCTION struct PdfPrecalculatedItem* PdfPrecalculatedItem_CreateFromXml(xmlNode *node);

DLLEXPORT_TEST_FUNCTION void PdfPrecalculatedItem_Init(struct PdfPrecalculatedItem* self, const char *formula);

DLLEXPORT_TEST_FUNCTION struct PdfPrecalculatedItem* PdfPrecalculatedItem_Create(const char *formula);

DLLEXPORT_TEST_FUNCTION void PdfPrecalculatedItem_Destroy(struct PdfTemplateBalloonItem* self);

DLLEXPORT_TEST_FUNCTION void PdfPrecalculatedItem_Cleanup(struct PdfPrecalculatedItem* self);

DLLEXPORT_TEST_FUNCTION void PdfPrecalculatedItem_AddValue(struct PdfPrecalculatedItem* self, const char *dataStreamName, const char *columnName, float value);

DLLEXPORT_TEST_FUNCTION char* PdfPrecalculatedItem_GetResult(struct PdfPrecalculatedItem *self);

int PdfPrecalculatedItem_Process(struct PdfTemplateBalloonItem *self, struct PdfGenerator *generator, struct PdfGeneratedBalloon *balloon, struct StreamWriter *streamWriter);

void PdfPrecalculatedItem_ParseItem(struct PdfPrecalculatedItem *self, const char *value);

int PdfPrecalculatedItem_FindFunction(struct PdfPrecalculatedItem *self, char *input, int startOfString);

char *GetName(char *inputStr, int offset);

struct PdfPrecalculatedFunction *PdfPrecalculatedItem_GetFunction(struct PdfPrecalculatedItem *self, int start);

void PdfPrecalculatedItem_ReplaceFormulaWithResults(struct PdfPrecalculatedItem *self);



#endif
