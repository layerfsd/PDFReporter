
#ifndef _PDFGENERATED_BALLOON_
#define _PDFGENERATED_BALLOON_

#include "PdfFactory.h"
#include "Rectangle.h"
#include "RectangleNormal.h"

#define GENERATING_ALGORITHM_RIGHT_BOTTOM 1 // generation will go to the right and then to bottom
#define GENERATING_ALGORITHM_BOTTOM_RIGHT 2 // generation will go to the bottom and then to the right



struct GeneratorGridCell
{
	float sx;
	float sy;
	int allocated;
};



// Generated ballooon is item created when generator is working. Balloons are created from templates
struct PdfGeneratedBalloon
{
	struct PdfTemplateBalloon *templateBalloon; // template balloon generated this one
	struct DLList *rectMatrix; // list of rects already used. This is used to check for next balloon space. Rect is positioned from 0,0. It is RectNormal object here and not Rect

	int	  canGrow; // if this balloon can grow. Default is NO

	float width;  // in main drawing units
	float height; // in main drawing units
	//float top; // this is top location relative to parent
	//float left; // this is left location relative to parent

	int    generatingAlgorithm; 

	struct RectangleNormal positionRect; // this is container in normal dimensions
	struct Rectangle containerRect; 
	struct Rectangle fillBackgroundRect; // rect used for filling background color. It is changed based on grow
	struct PdfPage *currentPage; // current page that is generated

	struct DLList *balloons; // generated balloons inside this one
	struct PdfGeneratedBalloon *parent; // parent balloon or null for page

	float fillColorR;
	float fillColorG;
	float fillColorB;
	float fillColorA;
};



DLLEXPORT_TEST_FUNCTION struct PdfGeneratedBalloon* PdfGeneratedBalloon_Create();

DLLEXPORT_TEST_FUNCTION void PdfGeneratedBalloon_Init(struct PdfGeneratedBalloon *self);

DLLEXPORT_TEST_FUNCTION void PdfGeneratedBalloon_Copy(struct PdfGeneratedBalloon *self, struct PdfGeneratedBalloon *original);
/* This will make copy of generated balloon. NOTE! this is not yet completed method */

void PdfGeneratedBalloon_AddChildToList(struct PdfGeneratedBalloon *self, struct PdfGeneratedBalloon *generatedBalloon);


DLLEXPORT_TEST_FUNCTION int PdfGeneratedBalloon_AddChild(struct PdfGeneratedBalloon *self, struct PdfGeneratedBalloon *generatedBalloon, 
														 int isFromStatic, short staticBottomDockedFlag);
/* indicates if this balloon can add child considering size and other options. It will auto-grow if there is not enough space and if such options exists
	container rect and width, height should be as valid as possible.
*/

float PdfGeneratedBalloon_GetAbsoluteLocationX(struct PdfGeneratedBalloon *self);
float PdfGeneratedBalloon_GetAbsoluteLocationY(struct PdfGeneratedBalloon *self);
void  PdfGeneratedBalloon_DrawBackground(struct PdfGeneratedBalloon *self);

DLLEXPORT_TEST_FUNCTION int PdfGeneratedBalloon_GetNewRectBottomRight(struct PdfGeneratedBalloon *self, struct PdfGeneratedBalloon *generatedBalloon, struct Rectangle *outRect);
DLLEXPORT_TEST_FUNCTION int PdfGeneratedBalloon_GetNewRectRightBottom(struct PdfGeneratedBalloon *self, struct PdfGeneratedBalloon *generatedBalloon, struct Rectangle *outRect);
DLLEXPORT_TEST_FUNCTION int PdfGeneratedBalloon_GetNewRect(struct PdfGeneratedBalloon *self, struct PdfGeneratedBalloon *generatedBalloon, struct Rectangle *outRect);
DLLEXPORT_TEST_FUNCTION void PdfGeneratedBalloon_Destroy(struct PdfGeneratedBalloon *self);
DLLEXPORT_TEST_FUNCTION void PdfGeneratedBalloon_Cleanup(struct PdfGeneratedBalloon *self);

#endif