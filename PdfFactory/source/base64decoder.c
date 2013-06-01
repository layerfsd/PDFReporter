
#include "base64decoder.h"
#include <stdio.h>

/**
 * characters used for Base64 encoding
 */  
const char *BASE64_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

/**
 * determine the value of a base64 encoding character
 *
 * @param base64char the character of which the value is searched
 * @return the value in case of success (0-63), -1 on failure
 */  
int _base64_char_value(char base64char)
 {
    if (base64char >= 'A' && base64char <= 'Z')	return base64char-'A';
    if (base64char >= 'a' && base64char <= 'z') return base64char-'a'+26;
    if (base64char >= '0' && base64char <= '9') return base64char-'0'+2*26;
    if (base64char == '+') return 2*26+10;
    if (base64char == '/') return 2*26+11;
    return -1;
} 

/**
 * decode a 4 char base64 encoded byte triple
 *
 * @param quadruple the 4 characters that should be decoded
 * @param result the decoded data
 * @return lenth of the result (1, 2 or 3), 0 on failure
 */  
int _base64_decode_triple(char quadruple[4], unsigned char *result)
 {
    int i, triple_value, bytes_to_decode = 3, only_equals_yet = 1;
    int char_value[4];

    for (i=0; i<4; i++)
	{
		char_value[i] = _base64_char_value(quadruple[i]);
	}

    /* check if the characters are valid */
    for (i=3; i>=0; i--)
    {
		if (char_value[i]<0)
		{
			if (only_equals_yet && quadruple[i]=='=')
			{
				/* we will ignore this character anyway, make it something
				* that does not break our calculations */
				char_value[i]=0;
				bytes_to_decode--;
			continue;
			}	
			return 0;
		}
		/* after we got a real character, no other '=' are allowed anymore */
		only_equals_yet = 0;
	}

    /* if we got "====" as input, bytes_to_decode is -1 */
    if (bytes_to_decode < 0)
	{
		bytes_to_decode = 0;
	}

    /* make one big value out of the partial values */
    triple_value = char_value[0];
    triple_value *= 64;
    triple_value += char_value[1];
    triple_value *= 64;
    triple_value += char_value[2];
    triple_value *= 64;
    triple_value += char_value[3];

    /* break the big value into bytes */
    for (i=bytes_to_decode; i<3; i++)
	{
		triple_value /= 256;
	}
    for (i=bytes_to_decode-1; i>=0; i--)
    {
		result[i] = triple_value%256;
		triple_value /= 256;
    }

    return bytes_to_decode;
} 



DLLEXPORT_TEST_FUNCTION short Base64Decode(char *source, char *target, int targetLen)
{
	char *src, *tmpptr;
    char quadruple[4], tmpresult[3];
    int i, tmplen = 3;
    size_t converted = 0;

    /* concatinate '===' to the source to handle unpadded base64 data */
	src = (char *)MemoryManager_Alloc(strlen(source)+5);
    if (src == NULL)
	{
		return FALSE;	
	}	
    strcpy(src, source);
    strcat(src, "====");
    tmpptr = src;

    /* convert as long as we get a full result */
    while (tmplen == 3)
    {
		/* get 4 characters to convert */
		for (i=0; i<4; i++)
		{
			/* skip invalid characters - we won't reach the end */
			while (*tmpptr != '=' && _base64_char_value(*tmpptr)<0)
			{
				tmpptr++;
			}

			quadruple[i] = *(tmpptr++);
		}

		/* convert the characters */
		tmplen = _base64_decode_triple(quadruple, tmpresult);

		/* check if the fit in the result buffer */
		if (targetLen < tmplen)
		{
			MemoryManager_Free(src);
			return FALSE;
		}

		/* put the partial result in the result buffer */		
		MemoryManager_MemCpy(target, tmpresult, tmplen);
		target += tmplen;
		targetLen -= tmplen;
		converted += tmplen;
    }

	MemoryManager_Free(src);
    return TRUE;
	
}