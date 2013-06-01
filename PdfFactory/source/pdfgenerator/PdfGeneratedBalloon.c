#include "PdfGeneratedBalloon.h"
#include "PdfTemplateBalloon.h"
#include "Rectangle.h"
#include "RectangleNormal.h"
#include "PdfContentStream.h"
#include "GraphicWriter.h"
#include "PdfGenerator.h"
#include "PdfPage.h"
#include "DLList.h"
#include "MemoryManager.h"
#include "Logger.h"
#include <math.h>

DLLEXPORT_TEST_FUNCTION struct PdfGeneratedBalloon* PdfGeneratedBalloon_Create()
{
	struct PdfGeneratedBalloon *ret;
	ret = (struct PdfGeneratedBalloon*)MemoryManager_Alloc(sizeof(struct PdfGeneratedBalloon));
	PdfGeneratedBalloon_Init(ret);
	return ret;
}

DLLEXPORT_TEST_FUNCTION void PdfGeneratedBalloon_Init(struct PdfGeneratedBalloon *self)
{
	self->containerRect.lowerLeftX = 0;
	self->containerRect.lowerLeftY = 0;
	self->containerRect.upperRightX = 0;
	self->containerRect.upperRightY = 0;
	//self->top = 0;
	//self->left = 0;
	self->positionRect.bottom = 0;
	self->positionRect.left = 0;
	self->positionRect.top = 0;
	self->positionRect.right = 0;

	self->canGrow = FALSE;
	self->currentPage = NULL;
	self->height = 0;
	self->width = 0;
	self->rectMatrix = DLList_Create();
	self->balloons = DLList_Create();
	self->templateBalloon = NULL;
	self->parent = NULL;
	self->generatingAlgorithm = GENERATING_ALGORITHM_RIGHT_BOTTOM;
}


DLLEXPORT_TEST_FUNCTION void PdfGeneratedBalloon_Copy(struct PdfGeneratedBalloon *self, struct PdfGeneratedBalloon *original)
{
	self->containerRect = original->containerRect;	
	self->currentPage = original->currentPage;
	self->height = original->height;
	self->width = original->width;
	self->templateBalloon = original->templateBalloon;	
}

DLLEXPORT_TEST_FUNCTION void PdfGeneratedBalloon_Destroy(struct PdfGeneratedBalloon *self)
{
	PdfGeneratedBalloon_Cleanup(self);
	MemoryManager_Free(self);
}

DLLEXPORT_TEST_FUNCTION void PdfGeneratedBalloon_Cleanup(struct PdfGeneratedBalloon *self)
{	
	// destroy rect matrix
	while(self->rectMatrix->size > 0)
	{
		struct RectangleNormal *obj;
		obj = (struct RectangleNormal *)DLList_Back(self->rectMatrix);
		DLList_PopBack(self->rectMatrix);
		//RectangleNormal_Destroy(obj);
	}
	DLList_Destroy(self->rectMatrix); 

	// balloons destroy
	while(self->balloons->size > 0)
	{
		struct PdfGeneratedBalloon *obj;
		obj = (struct PdfGeneratedBalloon *)DLList_Back(self->balloons);
		DLList_PopBack(self->balloons);
		PdfGeneratedBalloon_Destroy(obj);
	}
	DLList_Destroy(self->balloons); 
}

void PdfGeneratedBalloon_AddChildToList(struct PdfGeneratedBalloon *self, struct PdfGeneratedBalloon *generatedBalloon)
{
	// add to rect matrix this rectangle
	//struct RectangleNormal *rectNormal;
	//rectNormal = RectangleNormal_Create(generatedBalloon->recleft, generatedBalloon->top, generatedBalloon->width, generatedBalloon->height);

	generatedBalloon->positionRect.right = generatedBalloon->positionRect.left + generatedBalloon->width;
	generatedBalloon->positionRect.bottom = generatedBalloon->positionRect.top + generatedBalloon->height;

	DLList_PushBack(self->rectMatrix, &generatedBalloon->positionRect);

	// add parent/child connections
	generatedBalloon->parent = self;
	DLList_PushBack(self->balloons, generatedBalloon);	
}


// TODO: Not implemented yet. Generation will go to the bottom and then to the right
DLLEXPORT_TEST_FUNCTION int PdfGeneratedBalloon_GetNewRectBottomRight(struct PdfGeneratedBalloon *self, struct PdfGeneratedBalloon *generatedBalloon, struct Rectangle *outRect)
{
	return TRUE;
}

// Generation will go first to the right and then to the bottom. outRect has 0,0 at top left coordinate system
DLLEXPORT_TEST_FUNCTION int PdfGeneratedBalloon_GetNewRectRightBottom(struct PdfGeneratedBalloon *self, struct PdfGeneratedBalloon *generatedBalloon, struct Rectangle *outRect)
{
	struct RectangleNormal resRect;
	struct RectangleNormal containRect;	
	struct DLList *horizLines = DLList_Create();
	struct DLList *vertLines = DLList_Create();
	struct DLListNode *iter;
	struct RectangleNormal* tempRect;
	int rowCount = 0;
	int colCount = 0;
	int i = 0;
	int j = 0;
	float *tempVal = (float*)MemoryManager_Alloc(sizeof(float)), *tempVal1 = (float*)MemoryManager_Alloc(sizeof(float));	
	int xind1;
	int xind2;
	int yind1;
	int yind2;
	int anyIntersection = 0;
	short allAllocated = 1;
	struct GeneratorGridCell **grid;

	if (!outRect)
	{
		return FALSE;
	}

	resRect.left = 0;
	resRect.top = 0;
	resRect.right = generatedBalloon->width;
	resRect.bottom = generatedBalloon->height;

	containRect.left = 0;
	containRect.top = 0;
	containRect.right = self->width;
	containRect.bottom = self->height;
		

	*tempVal = 0.0f;
	*tempVal1 = 0.0f;

	DLList_PushBack(vertLines, (void*) tempVal);
	DLList_PushBack(vertLines, (void*) &containRect.right);
	DLList_PushBack(horizLines, (void*) tempVal1);
	DLList_PushBack(horizLines, (void*) &containRect.bottom);		

	// Make rect lines
	//foreach(RectangleNormal rect in self->rectMatrix)
	for(iter = DLList_Begin(self->rectMatrix); iter != DLList_End(self->rectMatrix); iter = iter->next)
	{
		// add to horiz lines
		//if (!horizLines.Contains(rect.top))
		tempRect = (struct RectangleNormal*)iter->data;
		if(!DLList_ContainsFloat(horizLines, tempRect->top))
		{
			//horizLines.Add(rect.top);
			DLList_PushBack(horizLines, (void*)&tempRect->top);
		}
		//if (!horizLines.Contains(rect.bottom))
		if(!DLList_ContainsFloat(horizLines, tempRect->bottom))
		{
			//horizLines.Add(rect.bottom);
			DLList_PushBack(horizLines, (void*)&tempRect->bottom);
		}
		// add vertical lines
		//if (!vertLines.Contains(rect.left))
		if(!DLList_ContainsFloat(vertLines, tempRect->left))
		{
			//vertLines.Add(rect.left);
			DLList_PushBack(vertLines, (void*)&tempRect->left);
		}
		//if (!vertLines.Contains(rect.right))
		if(!DLList_ContainsFloat(vertLines, tempRect->right))
		{
			//vertLines.Add(rect.right);
			DLList_PushBack(vertLines, (void*)&tempRect->right);
		}
	}

	colCount = vertLines->size - 1;
	rowCount = horizLines->size - 1;

	vertLines = DLList_SortByValue(vertLines);
	horizLines = DLList_SortByValue(horizLines);
	

	if (rowCount <= 0 || colCount <= 0)
	{
		RectangleNormal_MoveTo(&resRect, 0, 0);
		outRect->lowerLeftX = resRect.left;
		outRect->upperRightX = resRect.right;
		outRect->lowerLeftY = resRect.bottom;
		outRect->upperRightY = resRect.top;		
		return 1;
	}	

	// create grid
	grid = MemoryManager_Alloc(rowCount * sizeof(struct GeneratorGridCell*));
	grid[0] = MemoryManager_Alloc(sizeof(struct GeneratorGridCell)*rowCount*colCount);
	for(i = 1; i < rowCount; i++)
	{
		grid[i] = grid[0] + i * colCount;
	}

	// clear grid
	for(i = 0; i<rowCount; i++)
	{
		for(j = 0; j<colCount; j++)
		{
			grid[i][j].allocated = 0;
			grid[i][j].sx = 0.0f;
			grid[i][j].sy = 0.0f;
		}
	}

	// fill grid			
	//foreach (RectangleNormal rect in this.rectMatrix)
	tempRect = NULL;
	iter = NULL;
	for(iter = DLList_Begin(self->rectMatrix); iter != DLList_End(self->rectMatrix); iter = iter->next)
	{
		tempRect = (struct RectangleNormal*)iter->data;

		xind1 = DLList_IndexOfFloat(vertLines, tempRect->left);
		xind2 = DLList_IndexOfFloat(vertLines, tempRect->right);
		yind1 = DLList_IndexOfFloat(horizLines, tempRect->top);
		yind2 = DLList_IndexOfFloat(horizLines, tempRect->bottom);

		for(i = xind1; i < xind2; i++)
		{
			for(j = yind1; j < yind2; j++)
			{
				grid[j][i].allocated = 1;
			}
		}				
	}	

	// make coordinates
	for (i = 0; i < rowCount; i++)
	{
		for (j = 0; j < colCount; j++)
		{
			grid[i][j].sx = DLList_GetFValueAtIndex(vertLines, j);
			grid[i][j].sy = DLList_GetFValueAtIndex(horizLines, i);
		}
	}					

	for (i = 0; i < rowCount; i++)
	{
		for (j = 0; j < colCount; j++)
		{
			if (!grid[i][j].allocated)
			{
				allAllocated = FALSE;
				break;
			}
		}
		if (!allAllocated)
		{
			break;
		}
	}

	if (allAllocated)
	{
		  // move this to bottom line in case all blocks are allocated
		  resRect.left = 0;
		  resRect.top = containRect.bottom;
		  resRect.right = generatedBalloon->width;
		  resRect.bottom = generatedBalloon->height;
	}
	else
	{
		// find first empty place for new rect
		for (i = 0; i < rowCount; i++)
		{
			for (j = 0; j < colCount; j++)
			{
				if (!grid[i][j].allocated)
				{
					// check if rect can be fitted here
					RectangleNormal_MoveTo(&resRect, grid[i][j].sx, grid[i][j].sy);
					// check for any intersection
					anyIntersection = 0;
					//foreach(RectangleNormal tmpRect in this.rectMatrix)
					tempRect = NULL;
					iter = NULL;
					for(iter = DLList_Begin(self->rectMatrix); iter != DLList_End(self->rectMatrix); iter = iter->next)
					{
						tempRect = (struct RectangleNormal*)iter->data;
						if (RectangleNormal_Intersect(tempRect, &resRect))
						{
							anyIntersection = 1;
							break;
						}
					}
					if (!anyIntersection && RectangleNormal_Contains(&containRect, &resRect))
					{
						outRect->lowerLeftX = resRect.left;
						outRect->upperRightX = resRect.right;
						outRect->lowerLeftY = resRect.bottom;
						outRect->upperRightY = resRect.top;

						DLList_Destroy(vertLines);
						DLList_Destroy(horizLines);
						MemoryManager_Free(tempVal);
						MemoryManager_Free(tempVal1);
						MemoryManager_Free(grid[0]);
						MemoryManager_Free(grid);
						return 1;
					}
				}
			}
		}
	}

	if (abs(resRect.bottom - containRect.bottom) < EPSILON || 
		resRect.bottom < containRect.bottom)
    {
		outRect->lowerLeftX = resRect.left;
		outRect->upperRightY = containRect.bottom;
		outRect->upperRightX = resRect.right;
		outRect->lowerLeftY = containRect.bottom + generatedBalloon->height;
    }
	else 
	{
		outRect->lowerLeftX = resRect.left;
		outRect->upperRightX = resRect.right;
		outRect->lowerLeftY = resRect.bottom;
		outRect->upperRightY = resRect.top;
	}

	DLList_Destroy(vertLines);
	DLList_Destroy(horizLines);
	MemoryManager_Free(tempVal);
	MemoryManager_Free(tempVal1);
	MemoryManager_Free(grid[0]);
	MemoryManager_Free(grid);
	return 0;     	
}


// find empty space for generated balloon inside self balloon
// in case there is no space return FALSE
DLLEXPORT_TEST_FUNCTION int PdfGeneratedBalloon_GetNewRect(struct PdfGeneratedBalloon *self, struct PdfGeneratedBalloon *generatedBalloon, struct Rectangle *outRect)
{
	switch (generatedBalloon->generatingAlgorithm)
	{
	case GENERATING_ALGORITHM_BOTTOM_RIGHT:
		return PdfGeneratedBalloon_GetNewRectBottomRight(self, generatedBalloon, outRect);
		break;
	case GENERATING_ALGORITHM_RIGHT_BOTTOM:
		return PdfGeneratedBalloon_GetNewRectRightBottom(self, generatedBalloon, outRect);
		break;
	default:
		return FALSE;
	}	
}

/*
 Count dynamic balloons in generated balloon
*/
int PdfGeneratedBalloon_DynamicBalloonsCount(struct PdfGeneratedBalloon *self)
{
	struct DLListNode *iter;
	struct PdfGeneratedBalloon *balloon;
	int count = 0;

	for(iter = DLList_Begin(self->balloons); iter != DLList_End(self->balloons); iter = iter->next)
	{
		balloon = (struct PdfGeneratedBalloon *)iter->data;
		if (balloon->templateBalloon && !balloon->templateBalloon->isStatic)
		{
			count++;		
		}		
	}
	return count;
}

/*
  This will check if this balloon has enough conditions to grow. Checking properties of itself. 
*/
int PdfGeneratedBalloon_CanGrow(struct PdfGeneratedBalloon *self)
{	
	if (self->canGrow)
	{
		if (self->templateBalloon)
		{
			if (self->templateBalloon->fillCapacity <= 0 || PdfGeneratedBalloon_DynamicBalloonsCount(self) <= self->templateBalloon->fillCapacity)
			{
				return TRUE;
			}
			else
			{
				return FALSE;
			}
		}
		else
		{
			return TRUE;
		}
	}
	else 
	{
		return FALSE;
	}	
}

void PdfGeneratedBalloon_DrawBackground(struct PdfGeneratedBalloon *self)
{
	struct PdfGraphicWriter *graphicWriter; 	
	struct Rectangle rect;
	float moveTop = 0.0f, moveBottom = 0.0f, moveLeft = 0.0f, moveRight = 0.0f;

    if (self->fillColorA > 0.0f)
	{		
		graphicWriter = PdfGraphicWriter_Create(localInstance->currentContentStream->stream->streamWriter);			
		{
			PdfGraphicWriter_SaveGraphicState(graphicWriter);
			PdfGraphicWriter_SetRGBFillColor(graphicWriter, self->fillColorR, self->fillColorG, self->fillColorB);		

			/*if (self->templateBalloon->topBorder.enabled)
			{
				moveTop = -(self->templateBalloon->topBorder.width / 2.0f);
			}
			if (self->templateBalloon->bottomBorder.enabled)
			{
				moveBottom = self->templateBalloon->bottomBorder.width / 2.0f;
			}
			if (self->templateBalloon->leftBorder.enabled)
			{
				moveLeft = self->templateBalloon->leftBorder.width / 2.0f;
			}
			if (self->templateBalloon->rightBorder.enabled)
			{
				moveRight = -self->templateBalloon->rightBorder.width / 2.0f;
			}*/

			rect.lowerLeftX = self->fillBackgroundRect.lowerLeftX + moveLeft;
			rect.lowerLeftY = self->fillBackgroundRect.lowerLeftY + moveBottom;
			rect.upperRightX = self->fillBackgroundRect.upperRightX + moveRight;
			rect.upperRightY = self->fillBackgroundRect.upperRightY + moveTop;
			PdfGraphicWriter_DrawRectangle(graphicWriter, &rect, TRUE, FALSE);
			PdfGraphicWriter_RestoreGraphicState(graphicWriter);
		}
		PdfGraphicWriter_Destroy(graphicWriter);
	}
}

/*
   Try to grow this balloon. 
*/
int PdfGeneratedBalloon_GrowBalloon(struct PdfGeneratedBalloon *self, struct PdfGeneratedBalloon *generatedBalloon, float growDelta)
{
	struct RectangleNormal *rect, rect1, rect2;	
	struct DLListNode *iter;
	float absX, absY;

	// check what generate algorithm is used and do logic from that
	if (generatedBalloon->generatingAlgorithm == GENERATING_ALGORITHM_RIGHT_BOTTOM)
	{
		// this means we can grow only on bottom. growDelta contains info how much we need to grow
		// 1. Grow Parent if can grow
		if (self->parent)
		{			
			// check if parent needs to grow at all
			// rect of parent
			rect1.top = 0;
			rect1.left = 0;
			rect1.right = self->parent->width;
			rect1.bottom = self->parent->height;
			
			// desired rect after growth. Growth to allow generatedBalloon to fall in
			rect2.left = self->positionRect.left;
			rect2.top = self->positionRect.top;
			rect2.bottom = rect2.top + self->height + growDelta;
			rect2.right = rect2.left + self->width;
			
			if (RectangleNormal_Contains(&rect1, &rect2))
			{
				// check if we intersect some rectangle in rectMatrix and in case we do return false						
				for(iter = DLList_Begin(self->parent->rectMatrix); iter != DLList_End(self->parent->rectMatrix); iter = iter->next)
				{				
					rect = (struct RectangleNormal *)iter->data;					

					// if rect.top != rect2.top && rect.left != rect2.left - or if this is not the same rect which already has taken place
                    // and if it does insterest something return false
					if (!(abs(rect->top - rect2.top) < EPSILON &&
						abs(rect->left - rect2.left) < EPSILON) &&
						RectangleNormal_Intersect(rect,&rect2))
					{
						Logger_LogNoticeMessage("PdfGeneratedBalloon_GrowBalloon: returned FALSE");
						return FALSE;
					}
				}

				// we don't need to grow parent just increase size of self				
				self->height += growDelta;	
				self->containerRect.lowerLeftY += growDelta;
				self->positionRect.bottom += growDelta;
			}
			else 
			{				
				// can parent grow at all
				if (PdfGeneratedBalloon_CanGrow(self->parent))
				{
					// grow parent					
					if (PdfGeneratedBalloon_GrowBalloon(self->parent, generatedBalloon, growDelta))
					{
						self->height += growDelta;		
						self->containerRect.lowerLeftY += growDelta;
						self->positionRect.bottom += growDelta;						
					}
					else 
					{
						Logger_LogNoticeMessage("PdfGeneratedBalloon_GrowBalloon: returned FALSE");
						return FALSE;
					}
				}
				else 
				{
					// cannot increase size return FALSE
					Logger_LogNoticeMessage("PdfGeneratedBalloon_GrowBalloon: returned FALSE");
					return FALSE;
				}							
			}
		}
		else 
		{
			// no parent ... grow in height by required size
			self->height += growDelta;		
			self->containerRect.lowerLeftY += growDelta;
			self->positionRect.bottom += growDelta;
		}

		// make fill background rect
		absX = PdfGeneratedBalloon_GetAbsoluteLocationX(self);
		absY = PdfGeneratedBalloon_GetAbsoluteLocationY(self);
		self->fillBackgroundRect.lowerLeftX = absX;
		self->fillBackgroundRect.lowerLeftY = absY - self->height;
		self->fillBackgroundRect.upperRightX = absX + self->width;
		self->fillBackgroundRect.upperRightY = absY - (self->height - growDelta);
		PdfGeneratedBalloon_DrawBackground(self);
	}
	if (generatedBalloon->generatingAlgorithm == GENERATING_ALGORITHM_BOTTOM_RIGHT)
	{
		// this means we can grow only on left
		// TODO: Not implemented yet
	}

	Logger_LogNoticeMessage("PdfGeneratedBalloon_GrowBalloon: returned TRUE");
	return TRUE;
}

DLLEXPORT_TEST_FUNCTION int PdfGeneratedBalloon_AddChild(struct PdfGeneratedBalloon *self, struct PdfGeneratedBalloon *generatedBalloon, 
														 int isFromStatic, short staticBottomDockedFlag)
{
	struct Rectangle newRect;
	float absX, absY;

	if (isFromStatic)	
	{
		// add it to the list
		PdfGeneratedBalloon_AddChildToList(self, generatedBalloon);
		// make fill background rect
		absX = PdfGeneratedBalloon_GetAbsoluteLocationX(generatedBalloon);
		absY = PdfGeneratedBalloon_GetAbsoluteLocationY(generatedBalloon);
		generatedBalloon->fillBackgroundRect.lowerLeftX = absX;
		generatedBalloon->fillBackgroundRect.lowerLeftY = absY-generatedBalloon->height;
		generatedBalloon->fillBackgroundRect.upperRightX = absX + generatedBalloon->width;
		generatedBalloon->fillBackgroundRect.upperRightY = absY;
		//PdfGeneratedBalloon_DrawBackground(self);
	}
	else
	{	
		// if there is no room for another child here
		if (self->templateBalloon && 
			(self->templateBalloon->fillCapacity > 0 && PdfGeneratedBalloon_DynamicBalloonsCount(self) + 1 > self->templateBalloon->fillCapacity) 
			&& !staticBottomDockedFlag)
		{
			return FALSE;
		}

		if (!PdfGeneratedBalloon_GetNewRect(self, generatedBalloon, &newRect))
		{
			if (PdfGeneratedBalloon_CanGrow(self))
			{
				// we established that balloon need to grow to put this new one inside and determined ammount of grow
                // we need to check if grow is allowed on parents side (maybe it is already taken on parents size and allocated in 
                // rect matrix. this is checked before grow                        
				if (PdfGeneratedBalloon_GrowBalloon(self, generatedBalloon, generatedBalloon->height - (self->height - newRect.upperRightY)))
				{					
					return PdfGeneratedBalloon_AddChild(self, generatedBalloon, isFromStatic, staticBottomDockedFlag);
				}
				else 
				{
					return FALSE;
				}
			}
			else
			{				
				return FALSE;
			}
		}
		else
		{
			if (staticBottomDockedFlag && self->templateBalloon && !self->templateBalloon->fitToContent)
            {
                // in case this one is static docked bottom and we wont to put him on the bottom of this balloon
				newRect.upperRightY = self->containerRect.lowerLeftY - (newRect.lowerLeftY - newRect.upperRightY);
				newRect.lowerLeftY = self->containerRect.lowerLeftY;
            }

			generatedBalloon->width = newRect.upperRightX - newRect.lowerLeftX;
			generatedBalloon->height = newRect.lowerLeftY - newRect.upperRightY;
			generatedBalloon->positionRect.top = newRect.upperRightY;
			generatedBalloon->positionRect.left = newRect.lowerLeftX;
			generatedBalloon->containerRect = newRect;			
			
			PdfGeneratedBalloon_AddChildToList(self, generatedBalloon);

			// make fill background rect
			absX = PdfGeneratedBalloon_GetAbsoluteLocationX(generatedBalloon);
			absY = PdfGeneratedBalloon_GetAbsoluteLocationY(generatedBalloon);			
			generatedBalloon->fillBackgroundRect.lowerLeftX = absX;
			generatedBalloon->fillBackgroundRect.lowerLeftY = absY-generatedBalloon->height;
			generatedBalloon->fillBackgroundRect.upperRightX = absX + generatedBalloon->width;
			generatedBalloon->fillBackgroundRect.upperRightY = absY;
		}

		// 1. ask for free space and get new rect for it 
		// 2. in case there is no space 
		//       check for conditions to grow
		//          try to grow this balloon (also parents). Return which one failed growing if anyone failed and return FALSE
		//          if balloon grew goto 1.
		//		if cannot grow return false
		// 3. in case there is enough space. 
		//		move new balloon to the rect position and set position and sizes to structure
		//	    add balloon to rect List and return true.		
	}

	return TRUE;
}



float PdfGeneratedBalloon_GetAbsoluteLocationX(struct PdfGeneratedBalloon *self)
{
	float res = 0.0;
	if (self->parent)
	{
		res = self->containerRect.lowerLeftX + PdfGeneratedBalloon_GetAbsoluteLocationX(self->parent);
	}
	else 
	{
		res = self->containerRect.lowerLeftX;
	}
	return res;
}

float PdfGeneratedBalloon_GetAbsoluteLocationY(struct PdfGeneratedBalloon *self)
{
	float res = 0.0;
	if (self->parent)
	{
		res = PdfGeneratedBalloon_GetAbsoluteLocationY(self->parent) - self->containerRect.upperRightY;
	}
	else 
	{
		res = self->containerRect.upperRightY;
	}
	return res;
}
