//-----------------------------------------------------------------------------
// Name:	PdfShadingDictionary.c
// Author:	Tomislav Kukic
// Date:	27.12.2008
//-----------------------------------------------------------------------------


/*
-------------------- Shading-Type HELP --------------
-  2 Axial shading									-
-  3 Radial shading									-
-----------------------------------------------------
*/



#ifndef _PDFSHADINGDICTIONARY_
#define _PDFSHADINGDICTIONARY_

#define PDF_SHADING_TYPE_AXIAL 2
#define PDF_SHADING_TYPE_RADIAL 3


#include "PdfBaseObject.h"
#include "ArrayObject.h"

struct PdfShadingDictionary
{
	struct PdfBaseObject base;
	int shadingType;   // Consult help above
	char *colorSpace;   // Example DeviceRGB or DeviceCMYK
	char *name;
	char axialCoords[100];
	char radialCoords[100];
	int antiAliasing;  // Boolean
	struct PdfFunction *function;
	struct PdfFunction *lastAddedFunction;
	int useCMYK;
};



DLLEXPORT_TEST_FUNCTION struct PdfShadingDictionary *PdfShadingDictionary_Create(struct PdfDocument *document, int shadingType, int functionType, int antiAliasing, int useCMYK, double shadingFactor);

DLLEXPORT_TEST_FUNCTION void PdfShadingDictionary_Init(struct PdfShadingDictionary *self, struct PdfDocument *document, int shadingType, int functionType, int antiAliasing, int useCMYK, double shadingFactor);

DLLEXPORT_TEST_FUNCTION void PdfShadingDictionary_Destroy(struct PdfShadingDictionary *self);

DLLEXPORT_TEST_FUNCTION void PdfShadingDictionary_Cleanup(struct PdfShadingDictionary *self);

DLLEXPORT_TEST_FUNCTION void PdfShadingDictionary_Write(struct PdfShadingDictionary *self, struct StreamWriter *streamWriter);

DLLEXPORT_TEST_FUNCTION void PdfShadingDictionary_AddNextFunction(struct PdfShadingDictionary *self, struct PdfFunction *nextFunction);

DLLEXPORT_TEST_FUNCTION void PdfShadingDictionary_SetAxialCoords(struct PdfShadingDictionary *self, double startX, double startY, double endX, double endY);

DLLEXPORT_TEST_FUNCTION void PdfShadingDictionary_SetRadialCoords(struct PdfShadingDictionary *self, double startX, double startY, double startRadius, double endX, double endY, double endRadius);

DLLEXPORT_TEST_FUNCTION void PdfShadingDictionary_SetRGBStartColor(struct PdfShadingDictionary *self, double r, double g, double b);

DLLEXPORT_TEST_FUNCTION void PdfShadingDictionary_SetRGBEndColor(struct PdfShadingDictionary *self, double r, double g, double b);

DLLEXPORT_TEST_FUNCTION void PdfShadingDictionary_SetCMYKStartColor(struct PdfShadingDictionary *self, double c, double m, double y, double k);

DLLEXPORT_TEST_FUNCTION void PdfShadingDictionary_SetCMYKEndColor(struct PdfShadingDictionary *self, double c, double m, double y, double k);





#endif
