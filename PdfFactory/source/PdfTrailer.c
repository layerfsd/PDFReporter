/*
PdFTrailer.c

Author: Nebojsa Vislavski
Date: 2.7.2008.	

Used for writting trailer to pdf

*/

#include "PdfTrailer.h"
#include "DictionaryObject.h"
#include "NameObject.h"
#include "NumberObject.h"
#include "IndirectReference.h"
#include "MemoryManager.h"
#include "PdfInformationDictionary.h"
#include <string.h>

DLLEXPORT_TEST_FUNCTION struct PdfTrailer* PdfTrailer_Create(struct PdfDocument *document)
{
	struct PdfTrailer* trailer;
	trailer = (struct PdfTrailer*)MemoryManager_Alloc(sizeof(struct PdfTrailer));
	trailer->document = document;
	return trailer;
}
/*
   looks like this
   trailer
   << 
   /Size 8
   /Root 1 0 R
   >>
   startxref
   334
   %%EOF
*/
DLLEXPORT_TEST_FUNCTION void PdfTrailer_Write(struct PdfTrailer *self)
{
	struct DictionaryObject *dict;
	struct NumberObject *number;
	struct IndirectReference *ref;
	char tmp[11];

	self->document->streamWriter->WriteData(self->document->streamWriter, "trailer");	
	self->document->streamWriter->WriteNewLine(self->document->streamWriter);	
	
	 dict = DictionaryObject_Begin(self->document->streamWriter);
	 {
		DictionaryObject_WriteKey(dict, "Size");
		
		number = NumberObject_Create(self->document->streamWriter, self->document->objectsCount+1);
		NumberObject_Write(number);
		NumberObject_Destroy(number);
		
		DictionaryObject_WriteKey(dict, "Root");
		ref = IndirectReference_Create(self->document->streamWriter, self->document->catalog->base.objectId, self->document->catalog->base.generationNumber);
		IndirectReference_Write(ref);
		IndirectReference_Destroy(ref);

		DictionaryObject_WriteKey(dict, "Info");
		ref = IndirectReference_Create(self->document->streamWriter, self->document->informationDictionary->base.objectId, self->document->informationDictionary->base.generationNumber);
		IndirectReference_Write(ref);
		IndirectReference_Destroy(ref);
	 }
	 DictionaryObject_End(dict);
	 
	 self->document->streamWriter->WriteData(self->document->streamWriter, "startxref");
	 self->document->streamWriter->WriteNewLine(self->document->streamWriter);	

	 sprintf(tmp, "%d", self->document->crossReferenceTable->beginOffset);	 
	 self->document->streamWriter->WriteData(self->document->streamWriter, tmp);
	 self->document->streamWriter->WriteNewLine(self->document->streamWriter);	
	 self->document->streamWriter->WriteData(self->document->streamWriter, "\%\%EOF");	 

}

DLLEXPORT_TEST_FUNCTION void PdfTrailer_Destroy(struct PdfTrailer *self)
{	
	MemoryManager_Free(self);
}
