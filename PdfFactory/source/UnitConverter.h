/*
UnitConverter.h

Unit convertion class.
*/


#ifndef _UNITCONVERTER_
#define _UNITCONVERTER_

#include "PdfFactory.h"

struct UnitConverterUnit
{
	struct UnitConverterUnit *next;
	char *unitName;
	float unitScale;
};

struct UnitConverter
{
	struct UnitConverterUnit *units;
};

DLLEXPORT_TEST_FUNCTION struct UnitConverter *UnitConverter_Create();
DLLEXPORT_TEST_FUNCTION void UnitConverter_Init(struct UnitConverter *self);
DLLEXPORT_TEST_FUNCTION void UnitConverter_Cleanup(struct UnitConverter *self);
DLLEXPORT_TEST_FUNCTION void UnitConverter_Destroy(struct UnitConverter *self);

DLLEXPORT_TEST_FUNCTION void UnitConverter_AddUnit(struct UnitConverter *self, char *unitName, double unitScale);
/* Add new unit. */

DLLEXPORT_TEST_FUNCTION void UnitConverter_AddCommonUnits(struct UnitConverter *self);
/* Add some common units (pt, mm, cm, m, in). */

DLLEXPORT_TEST_FUNCTION struct UnitConverterUnit *UnitConverter_FindUnit(struct UnitConverter *self, char *input);
/* Find unit in input string. */

DLLEXPORT_TEST_FUNCTION float UnitConverter_ConvertToPoints(struct UnitConverter *self, char *input);
/* Parse input string, find unit and convert it to points. (1 pt = 1/72 in) */

#endif
