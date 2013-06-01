#include <stdio.h>
#include <windows.h>
#include "PdfFactory.h"

#define NUMBER_PHONES 10
#define NUMBER_PERSONS 10

struct Phone
{
    char Number[256];
};

struct Person
{
   char Name[256];
   char Address[256];
   struct Phone phones[NUMBER_PHONES];

} Persons[NUMBER_PERSONS];
    
int PersonIndex = -1;
int PhoneIndex = -1;

int _stdcall ReadData(char *dataStreamName)
{
	if (strcmp(dataStreamName, "Person") == 0)
	{
		if (PersonIndex >= NUMBER_PERSONS)
		{
			return FALSE;
		}
		else 
		{
			PersonIndex++;
			return TRUE;
		}
	}
	else if (strcmp(dataStreamName, "Phone") == 0)
	{
		if (PhoneIndex >= NUMBER_PHONES)
		{
			return FALSE;
		}
		else 
		{
			PhoneIndex++;
			return TRUE;
		}
	}
	else 
	{
		return FALSE;
	}
}

int _stdcall InitializeDataStream(char *parentDataStream, char *dataStreamName, int *itemsCount)
{
	if (strcmp(dataStreamName, "Person") == 0)
	{
		*itemsCount = NUMBER_PERSONS;
		PersonIndex = -1;
		return TRUE;
	}
	else if (strcmp(dataStreamName, "Phone") == 0)
	{
		*itemsCount = NUMBER_PHONES;
		PhoneIndex = -1;
		return TRUE;
	}
	else 
	{
		itemsCount = 0;
		return TRUE;
	}
}

unsigned char* _stdcall RequestData(char *dataStreamName, char *columnName, int *dataSize)
{
	if (strcmp(dataStreamName, "Person") == 0)
	{
		if (strcmp(columnName, "Name") == 0)
		{
			return (unsigned char*)Persons[PersonIndex].Name;
		}
		if (strcmp(columnName, "Address") == 0)
		{
			return (unsigned char*)Persons[PersonIndex].Address;
		}
		else 
		{
			return 0;
		}
	}
	else if (strcmp(dataStreamName, "Phone") == 0)
	{
		if (strcmp(columnName, "Number") == 0)
		{
			return (unsigned char*)Persons[PersonIndex].phones[PhoneIndex].Number;
		}
		else 
		{
			return 0;
		}
	}
	else 
	{
		return 0;
	}
}

void InitializeData()
{
	for (int i = 0; i < NUMBER_PERSONS; i++)
	{
		memset(Persons[i].Name, 0, sizeof(Persons[i].Name));
		memset(Persons[i].Address, 0, sizeof(Persons[i].Address));

		sprintf(Persons[i].Address, "Addreess_%d", i);
		sprintf(Persons[i].Name, "Name_%d", i);
		for(int j = 0; j < NUMBER_PHONES; j++)
		{
			memset(Persons[i].phones[j].Number, 0, sizeof(Persons[i].phones[j].Number));
			sprintf(Persons[i].phones[j].Number, "Number_%d", j);
		}
	}	
}

int main()
{
	InitializeData();
	InitializeGenerator(0, 0);	
	SetInitializeDataStreamCallback(InitializeDataStream);
	SetReadDataCallback(ReadData);
	SetRequestDataCallback(RequestData);
	AttachTemplateFromFile("PdfReportsTest.prtp");
	GenerateToFile("PdfreportsTest.pdf");
	ShutdownGenerator();

	printf("Generated OK!");
	return 0;
}

