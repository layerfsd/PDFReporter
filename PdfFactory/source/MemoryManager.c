#include "MemoryManager.h"
#include <stdlib.h>

DLLEXPORT_TEST_FUNCTION void MemoryManager_Init()
{
	MemoryManager_Instance.totalFreedMemory = 0;
	MemoryManager_Instance.totalUsedMemory = 0;

}

DLLEXPORT_TEST_FUNCTION void* MemoryManager_Alloc(int size)
{
	void *ptr;
	ptr = malloc(size);
	MemoryManager_Instance.totalUsedMemory += size;
	return ptr;
}

DLLEXPORT_TEST_FUNCTION void MemoryManager_Free(void *ptr)
{
	free(ptr);
}

DLLEXPORT_TEST_FUNCTION void* MemoryManager_ReAlloc(void *ptr, int size)
{
	return realloc(ptr, size);
}

DLLEXPORT_TEST_FUNCTION char *MemoryManager_StrDup(const char *ptr)
{
	unsigned int l = strlen(ptr);
	char *p = MemoryManager_Alloc(l+1);
	memcpy(p, ptr, l);
	p[l] = 0;
	return p;

	//return strdup(ptr);
}

DLLEXPORT_TEST_FUNCTION void MemoryManager_MemCpy(void *dst, void *src, int size)
{
	memcpy(dst, src, size);
}

