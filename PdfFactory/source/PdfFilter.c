#include "PdfFilter.h"
#include "MemoryManager.h"
#include "StreamObject.h"
#include "MemoryWriter.h"
#include <zlib.h>


//============================= By Toma ==================================
//=                    Funkcija: PdfFilter_FlatDecode                    =
//=  Trazi: StreamObject												 =
//=  Vraca: (sve u redu): 1												 =
//=  Vraca: (greska)    : 0												 =
//========================================================================
char PdfFilter_FlateDecode(struct StreamObject *srcStream)
{
	struct MemoryWriter *memWriter;
	int ret, have, inSize;
	z_stream strm;
	char *in;
    char *out;
	char *finalOut;
	
	inSize = srcStream->endStreamOffset - srcStream->beginStreamOffset; // Eliminate start and end of s stream (comands)
	srcStream->streamWriter->Seek(srcStream->streamWriter, srcStream->beginStreamOffset);
	memWriter = (struct MemoryWriter*)srcStream->streamWriter;


	in = (void*)(memWriter->memory + memWriter->position);
	out = MemoryManager_Alloc(inSize);

	//allocate deflate state
	strm.zalloc = Z_NULL;
	strm.zfree = Z_NULL;
	strm.opaque = Z_NULL;

	ret = deflateInit(&strm, Z_DEFAULT_COMPRESSION);
	
	if (ret != Z_OK) return 0; //if bad init return 0

	strm.avail_in = inSize;
	strm.next_in = in;

	strm.avail_out = inSize;
	strm.next_out = out;
	ret = deflate(&strm, Z_FULL_FLUSH); // COMPRESS data

	if (ret == Z_STREAM_ERROR) return 0; // if compresson results in error return 0

	have = inSize - strm.avail_out;
	finalOut = MemoryManager_Alloc(have);
	MemoryManager_MemCpy(finalOut,out,have);
	//end compreson
	(void)deflateEnd(&strm);

	MemoryManager_Free(memWriter->memory); //free old memory in stream
	memWriter->memory = MemoryManager_Alloc(have); // create new memory
	MemoryManager_MemCpy(memWriter->memory,finalOut,have);// fill new memory with compressed data
	//set new length
	memWriter->size = have; 
	srcStream->length = have;

	//Free all used memory allocations
	in = NULL;
	MemoryManager_Free(out);
	MemoryManager_Free(finalOut);

	return 1;
}
