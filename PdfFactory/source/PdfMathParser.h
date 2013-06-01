/*
File: PdfMathParser.h
Date: 19.01.2009
Author: Tomislav Kukic (Prepravka originalnog koda pisanog za c++)
*/



#ifndef PARSER_H
#define PARSER_H

// declarations
//#include <stdio.h>
//#include <stdlib.h>
//#include <string.h>
//#include <ctype.h>
//#include <math.h>
//#include "MemoryManager.h"
//#include "PdfMathParserVariableList.h"


enum TOKENTYPE {NOTHING = -1, DELIMETER, NUMBER, VARIABLE, FUNCTION, PDF_FUNCTION, UNKNOWN};

enum OPERATOR_ID {AND, OR, BITSHIFTLEFT, BITSHIFTRIGHT,                 // level 2
EQUAL, UNEQUAL, SMALLER, LARGER, SMALLEREQ, LARGEREQ,    // level 3
PLUS, MINUS,                     // level 4
MULTIPLY, DIVIDE, MODULUS, XOR,  // level 5
POW,                             // level 6
FACTORIAL};                      // level 7


#define NAME_LEN_MAX 30
#define EXPR_LEN_MAX 255
#define ERR_LEN_MAX 255


struct Parser
{
	// data
	char expr[EXPR_LEN_MAX+1];    // holds the expression
	char* e;                      // points to a character in expr

	char token[NAME_LEN_MAX+1];   // holds the token
	enum TOKENTYPE token_type;         // type of the token

	double ans;                   // holds the result of the expression
	char ans_str[255];            // holds a string containing the result 
								  // of the expression

	//struct Variablelist user_var;        // list with variables defined by user
};


struct Parser *Parser_Parser();
char* Parser_parse(struct Parser *self, const char expr[]);



void Parser_getToken(struct Parser *self);

double Parser_parse_level1(struct Parser *self);
double Parser_parse_level2(struct Parser *self);
double Parser_parse_level3(struct Parser *self);
double Parser_parse_level4(struct Parser *self);
double Parser_parse_level5(struct Parser *self);
double Parser_parse_level6(struct Parser *self);
double Parser_parse_level7(struct Parser *self);
double Parser_parse_level8(struct Parser *self);
double Parser_parse_level9(struct Parser *self);
double Parser_parse_level10(struct Parser *self);
double Parser_parse_number(struct Parser *self);

int Parser_get_operator_id(struct Parser *self, const char op_name[]);
double Parser_eval_operator(const int op_id, const double *lhs, const double rhs);
double Parser_eval_function(const char fn_name[], const double *value);
double Parser_eval_variable(struct Parser *self, const char var_name[]);

int Parser_row();
int Parser_col(struct Parser *self);


//Functions
double factorial(double value);
double sign(double value);


#endif
