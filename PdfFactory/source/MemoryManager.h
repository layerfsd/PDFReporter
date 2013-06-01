#ifndef _MEMORY_MANAGER_H
#define _MEMORY_MANAGER_H

#include "PdfFactory.h"

struct MemoryManager
{
	long totalUsedMemory;
	long totalFreedMemory;
};

struct MemoryManager MemoryManager_Instance;

DLLEXPORT_TEST_FUNCTION void MemoryManager_Init();
DLLEXPORT_TEST_FUNCTION void* MemoryManager_Alloc(int size);
DLLEXPORT_TEST_FUNCTION void MemoryManager_Free(void *ptr);
DLLEXPORT_TEST_FUNCTION void* MemoryManager_ReAlloc(void *ptr, int size);
DLLEXPORT_TEST_FUNCTION char *MemoryManager_StrDup(char *ptr);
DLLEXPORT_TEST_FUNCTION void MemoryManager_MemCpy(void *dst, void *src, int size);


#endif