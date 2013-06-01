/* 
PdfTemplateInformationDictionary.c

Author: Tomislav Kukic
Date: 20.2.2010.	

Used for reading information dictionary from template file
*/

#include "PdfTemplateInformationDictionary.h"
#include "PdfTemplateElements.h"
#include "MemoryManager.h"
#include "PdfTemplate.h"



DLLEXPORT_TEST_FUNCTION void PdfTemplateInformationDictionary_Write()
{
	//TO DO: Place code here...
}



DLLEXPORT_TEST_FUNCTION struct PdfTemplateInformationDictionary* PdfTemplateInformationDictionary_Create()
{
	struct PdfTemplateInformationDictionary *ret;
	ret = (struct PdfTemplateInformationDictionary*)MemoryManager_Alloc(sizeof(struct PdfTemplateInformationDictionary));
	return ret;
}




DLLEXPORT_TEST_FUNCTION struct PdfTemplateInformationDictionary* PdfTemplateInformationDictionary_CreateFromXml(xmlNode *node)
{
	struct PdfTemplateInformationDictionary *ret;
	ret = (struct PdfTemplateInformationDictionary*)MemoryManager_Alloc(sizeof(struct PdfTemplateInformationDictionary));

	ret->Title = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(node, "Title"));
	ret->Author = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(node, "Author"));
	ret->Subject = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(node, "Subject"));
	ret->Producer = MemoryManager_StrDup(PdfTemplate_LoadStringAttribute(node, "Producer"));

	return ret;
}





DLLEXPORT_TEST_FUNCTION void PdfTemplateInformationDictionary_Init(struct PdfTemplateInformationDictionary* self, char *Title, char *Author, char *Subject, char *Producer)
{
	self->Title = MemoryManager_StrDup(Title);
	self->Author = MemoryManager_StrDup(Author);
	self->Subject = MemoryManager_StrDup(Subject);
	self->Producer = MemoryManager_StrDup(Producer);
}




DLLEXPORT_TEST_FUNCTION void PdfTemplateInformationDictionary_Destroy(struct PdfTemplateInformationDictionary* self)
{
	PdfTemplateInformationDictionary_Cleanup(self);
	MemoryManager_Free(self);
}




DLLEXPORT_TEST_FUNCTION void PdfTemplateInformationDictionary_Cleanup(struct PdfTemplateInformationDictionary* self)
{
	if(self->Title)
	{
		MemoryManager_Free(self->Title);
		self->Title = 0;
	}

	if(self->Author)
	{
		MemoryManager_Free(self->Author);
		self->Author = 0;
	}

	if(self->Subject)
	{
		MemoryManager_Free(self->Subject);
		self->Subject = 0;
	}

	if(self->Producer)
	{
		MemoryManager_Free(self->Producer);
		self->Producer = 0;
	}
}
