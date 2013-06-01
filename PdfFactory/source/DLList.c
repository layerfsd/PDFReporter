#include <DLList.h>
#include <stdlib.h>
#include <assert.h>
#include "MemoryManager.h"

struct DLListNode *DLListNode_Create(void *data)
{
	struct DLListNode *x;
	x = (struct DLListNode *)MemoryManager_Alloc(sizeof(struct DLListNode));
	DLListNode_Init(x, data);
	return x;
}

void DLListNode_Init(struct DLListNode *self, void *data)
{
	self->data = data;
}

DLLEXPORT_TEST_FUNCTION struct DLList *DLList_Create()
{
	struct DLList *x;
	x = (struct DLList *)MemoryManager_Alloc(sizeof(struct DLList));
	DLList_Init(x);
	return x;
}

void DLList_Init(struct DLList *self)
{
	DLListNode_Init(&self->head, 0);
	self->head.prev = (struct DLListNode *)&self->head;
	self->head.next = (struct DLListNode *)&self->head;
	self->size = 0;
}

void DLList_Cleanup(struct DLList *self)
{
	DLList_Clean(self);
}

DLLEXPORT_TEST_FUNCTION void DLList_Destroy(struct DLList *self)
{
	DLList_Cleanup(self);
	MemoryManager_Free(self);
}

DLLEXPORT_TEST_FUNCTION void DLList_PushBack(struct DLList *self, void *data)
{
	struct DLListNode *node = DLListNode_Create(data);
	struct DLListNode *position = DLList_End(self);
	DLList_Insert(self, position, node);
}

void DLList_PushFront(struct DLList *self, void *data)
{
	struct DLListNode *node = DLListNode_Create(data);
	struct DLListNode *position = DLList_Begin(self);
	DLList_Insert(self, position, node);
}

void DLList_PopBack(struct DLList *self)
{
	assert(self->size > 0);
	DLList_Erase(self, self->head.prev);
}

void DLList_PopFront(struct DLList *self)
{
	assert(self->size > 0);
	DLList_Erase(self, self->head.next);
}

struct DLListNode *DLList_Begin(struct DLList *self)
{
	return self->head.next;
}

struct DLListNode *DLList_End(struct DLList *self)
{
	return &self->head;
}

void *DLList_Front(struct DLList *self)
{
	assert(self->size > 0);
	return self->head.next->data;
}

void *DLList_Back(struct DLList *self)
{
	assert(self->size > 0);
	return self->head.prev->data;
}

void DLList_Insert(struct DLList *self, struct DLListNode *position, struct DLListNode *node)
{
	struct DLListNode *prev = position->prev;
	node->prev = prev;
	node->next = position;
	position->prev = node;
	prev->next = node;
	self->size++;
}

void DLList_Erase(struct DLList *self, struct DLListNode *position)
{
	assert(self->size > 0);
	position->prev->next = position->next;
	position->next->prev = position->prev;
	MemoryManager_Free(position);
	self->size--;
}

void DLList_Clean(struct DLList *self)
{
	while (self->size > 0)
		DLList_PopBack(self);
}


DLLEXPORT_TEST_FUNCTION int DLList_Contains(struct DLList *self, void* data)
{
	struct DLListNode *iter;

	for(iter = DLList_Begin(self); iter != DLList_End(self); iter = iter->next)
	{
		if(iter->data == data)
			return 1;
	}
	return 0;
}

DLLEXPORT_TEST_FUNCTION int DLList_ContainsFloat(struct DLList *self, float value)
{
	struct DLListNode *iter;

	for(iter = DLList_Begin(self); iter != DLList_End(self); iter = iter->next)
	{
		float *f = iter->data;
		if(abs(*f - value) < EPSILON)
			return 1;
	}
	return 0;
}


DLLEXPORT_TEST_FUNCTION struct DLList* DLList_SortByValue(struct DLList *self)
{
	struct DLListNode *iter = NULL;
	struct DLListNode *iter2 = NULL;
	struct DLListNode *tempNode = NULL;
	struct DLList *tmpList = DLList_Create();
	float *tempVal = (float*)MemoryManager_Alloc(sizeof(float));
	float *tempVal2 = (float*)MemoryManager_Alloc(sizeof(float));
	int isAdded = 0;

	for(iter = DLList_Begin(self); iter != DLList_End(self); iter = iter->next)
	{
		isAdded = 0;
		tempVal = (float*)iter->data;
		if(tmpList->size == 0)
		{
			DLList_PushBack(tmpList, (void*)tempVal);
		}else{
			for(iter2 = DLList_Begin(tmpList); iter2 != DLList_End(tmpList); iter2 = iter2->next)
			{
				tempVal2 = (float*)iter2->data;
				if(tempVal != NULL && tempVal2 != NULL)
				{
					if(*tempVal < *tempVal2)
					{
						tempNode = NULL;
						tempNode = DLListNode_Create((void*)tempVal);
						DLList_Insert(tmpList, iter2, tempNode);
						isAdded = 1;
						break;
					}
				}
			}
			if(isAdded != 1)
			{
				DLList_PushBack(tmpList, iter->data);
			}
		}
	}
	return tmpList;
}




DLLEXPORT_TEST_FUNCTION float DLList_GetFValueAtIndex(struct DLList* self, int index)
{
	struct DLListNode *iter;
	int count = 0;
	float ret = 0.0f;// = (float*)MemoryManager_Alloc(sizeof(float));

	for(iter = DLList_Begin(self); iter != DLList_End(self); iter = iter->next)
	{
		if(count == index)
		{
			ret = *(float*)iter->data;
			return ret;
		}
		count++;
	}
	return 0.0f;
}




DLLEXPORT_TEST_FUNCTION int DLList_IndexOf(struct DLList *self, void* data)
{
	struct DLListNode *iter;
	int resIndexCounter = 0;
	for(iter = DLList_Begin(self); iter != DLList_End(self); iter = iter->next)
	{
		if(iter->data == data)
		{
			return resIndexCounter;
		}
		resIndexCounter++;
	}

	//if not found match for data, return -1 es indicator of error
	return -1;
}

DLLEXPORT_TEST_FUNCTION int DLList_IndexOfFloat(struct DLList *self, float value)
{
	struct DLListNode *iter;
	int resIndexCounter = 0;
	for(iter = DLList_Begin(self); iter != DLList_End(self); iter = iter->next)
	{
		float *f = iter->data;
		if(abs(*f - value) < EPSILON)
		{
			return resIndexCounter;
		}
		resIndexCounter++;
	}

	//if not found match for data, return -1 es indicator of error
	return -1;
}