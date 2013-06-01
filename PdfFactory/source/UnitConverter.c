
#include <UnitConverter.h>
#include <string.h>
#include <stdlib.h>
#include "MemoryManager.h"

DLLEXPORT_TEST_FUNCTION struct UnitConverter *UnitConverter_Create()
{
	struct UnitConverter *self =
		(struct UnitConverter *)MemoryManager_Alloc(sizeof(struct UnitConverter));
	UnitConverter_Init(self);
	return self;
}

DLLEXPORT_TEST_FUNCTION void UnitConverter_Init(struct UnitConverter *self)
{
	self->units = 0;
}

DLLEXPORT_TEST_FUNCTION void UnitConverter_Cleanup(struct UnitConverter *self)
{
	struct UnitConverterUnit *x = self->units;
	while(x)
	{
		struct UnitConverterUnit *next = x->next;
		free(x->unitName);
		free(x);
		x = next;
	}
	self->units = 0;
}

DLLEXPORT_TEST_FUNCTION void UnitConverter_Destroy(struct UnitConverter *self)
{
	UnitConverter_Cleanup(self);
	free(self);
}

DLLEXPORT_TEST_FUNCTION void UnitConverter_AddUnit(struct UnitConverter *self, char *unitName, double unitScale)
{
	struct UnitConverterUnit *x =
		(struct UnitConverterUnit *)MemoryManager_Alloc(sizeof(struct UnitConverterUnit));
	x->unitName = MemoryManager_StrDup(unitName);
	x->unitScale = unitScale;
	x->next = self->units;
	self->units = x;
}

DLLEXPORT_TEST_FUNCTION void UnitConverter_AddCommonUnits(struct UnitConverter *self)
{
	UnitConverter_AddUnit(self, "pt", 1.0);   // pt is the default unit
	UnitConverter_AddUnit(self, "in", 72.0);  // 1 in = 72 pt
	UnitConverter_AddUnit(self, "cm", 72.0 * 0.3937); // 1 in = 2.41 cm
	UnitConverter_AddUnit(self, "m", 72.0 * 39.37);
	UnitConverter_AddUnit(self, "mm", 72.0 * 0.03937);	
}

DLLEXPORT_TEST_FUNCTION struct UnitConverterUnit *UnitConverter_FindUnit(struct UnitConverter *self, char *input)
{
	// skip numbers and white space
	char *p = input;
	char *p2;
	int len = 0;
	struct UnitConverterUnit *x = self->units;
	for(;;)
	{
		int c = (unsigned char)*p++;
		if (c >= '0' && c <='9')
			continue;
		if (c == '.' || c == ',' || c == '+' || c == '-')
			continue;
		if (c == ' ' || c == '\t' || c == '\n' || c == '\r')
			continue;
		break;
	}
	p--;

	// find unit mark lenght
	p2 = p;
	for(;;)
	{
		int c = (unsigned char)*p2++;
		len++;
		if (c >= 'A' && c <='Z')
			continue;
		if (c >= 'a' && c <='z')
			continue;
		len--;
		break;
	}
	p2 = p;

	// iterate trough all units and try to find match
	while(x)
	{
		int i;
		char *r = x->unitName;
		int matchFound = 1;
		p = p2;
		if (strlen(r) == len)
		{
			for(i=0; i<len; i++)
			{
				if (*p++ != *r++)
				{
					matchFound = 0;
					break;
				}
			}
			if (matchFound)
				return x;
		}
		x = x->next;
	}
	return 0;
}

DLLEXPORT_TEST_FUNCTION float UnitConverter_ConvertToPoints(struct UnitConverter *self, char *input)
{
	// replace , with . in input
	unsigned int len, i;
	float x;
	struct UnitConverterUnit *unit;

	if (!input)
	{
		return 0;
	}
	len = strlen(input);
	for(i = 0; i < len; i++)
	{
		if (input[i] == ',')
		{
			input[i] = '.';
		}
	}

	x = (float)atof(input);
	unit = UnitConverter_FindUnit(self, input);
	if (unit)
		x *= unit->unitScale;
	
	return x;
}
