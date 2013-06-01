#ifndef __DL_LIST_H
#define __DL_LIST_H

#include "PdfFactory.h"

struct DLListNode
{
	struct DLListNode *prev;
	struct DLListNode *next;
	void *data;
};

struct DLList
{
	struct DLListNode head;
	int size;
};

struct DLListNode *DLListNode_Create(void *data);
void DLListNode_Init(struct DLListNode *self, void *data);

DLLEXPORT_TEST_FUNCTION struct DLList *DLList_Create();
void DLList_Init(struct DLList *self);
void DLList_Cleanup(struct DLList *self);
DLLEXPORT_TEST_FUNCTION void DLList_Destroy(struct DLList *self);

DLLEXPORT_TEST_FUNCTION void DLList_PushBack(struct DLList *self, void *data);
void DLList_PushFront(struct DLList *self, void *data);
void DLList_PopBack(struct DLList *self);
void DLList_PopFront(struct DLList *self);
DLLEXPORT_TEST_FUNCTION struct DLListNode *DLList_Begin(struct DLList *self);
DLLEXPORT_TEST_FUNCTION struct DLListNode *DLList_End(struct DLList *self);
void *DLList_Front(struct DLList *self);
void *DLList_Back(struct DLList *self);
void DLList_Insert(struct DLList *self, struct DLListNode *position, struct DLListNode *node);
void DLList_Erase(struct DLList *self, struct DLListNode *position);
void DLList_Clean(struct DLList *self);
DLLEXPORT_TEST_FUNCTION int DLList_Contains(struct DLList *self, void* data);
DLLEXPORT_TEST_FUNCTION int DLList_ContainsFloat(struct DLList *self, float value);
DLLEXPORT_TEST_FUNCTION struct DLList* DLList_SortByValue(struct DLList *self);
DLLEXPORT_TEST_FUNCTION int DLList_IndexOf(struct DLList *self, void* data);
DLLEXPORT_TEST_FUNCTION int DLList_IndexOfFloat(struct DLList *self, float value);
DLLEXPORT_TEST_FUNCTION float DLList_GetFValueAtIndex(struct DLList* self, int index);

#endif
