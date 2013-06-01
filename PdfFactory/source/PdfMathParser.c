
// declarations
//#include <stdio.h>
#include "PdfMathParser.h"
#include "MemoryManager.h"
#include <stdio.h>
#include <stdlib.h>



/*
* constructor. 
* Initializes all data with zeros and empty strings
*/
struct Parser *Parser_Parser()
{
	struct Parser* ret;
	ret = (struct Parser *)MemoryManager_Alloc(sizeof(struct Parser));

	ret->expr[0] = '\0';
	ret->e = NULL;

	ret->token[0] = '\0';
	ret->token_type = NOTHING;

	ret->ans = 0.0;
	ret->ans_str[0] = '\0';

	return ret;
}


/**
* parses and evaluates the given expression
* On error, an error of type Error is thrown
*/
char* Parser_parse(struct Parser *self, const char new_expr[])
{
	// check the length of expr
	if (strlen(new_expr) > EXPR_LEN_MAX)
	{
		return NULL;
	}

	// initialize all variables
	strcpy(self->expr, new_expr);     // copy the given expression to expr
	self->e = MemoryManager_StrDup(self->expr);                   // let e point to the start of the expression
	self->ans = 0;

	Parser_getToken(self);
	if (self->token_type == DELIMETER && *self->token == '\0')
	{
		return NULL;
	}

	self->ans = Parser_parse_level1(self);

	// check for garbage at the end of the expression 
	// an expression ends with a character '\0' and token_type = delimiter
	if (self->token_type != DELIMETER || *self->token != '\0')
	{
		if (self->token_type == DELIMETER)
		{
			return NULL;
		}
		else
		{
			return NULL;
		}
	}  

	// add the answer to memory as variable "Ans"
	//self->user_var.add("Ans", self->ans);

	_snprintf(self->ans_str, sizeof(self->ans_str), "%g", self->ans);

	return self->ans_str;
}


/*
* checks if the given char c is a minus
*/
int isMinus(const char c)
{
	if (c == 0) return 0;
	return c == '-';
}



/*
* checks if the given char c is whitespace
* whitespace when space chr(32) or tab chr(9)
*/
int isWhiteSpace(const char c)
{
	if (c == 0) return 0;
	return c == 32 || c == 9;  // space or tab
}

/*
* checks if the given char c is a delimeter
* minus is checked apart, can be unary minus
*/
int isDelimeter(const char c)
{
	if (c == 0) return 0;
	return strchr("&|<>=+/*%^!", c) != 0;
}

/*
* checks if the given char c is NO delimeter
*/
int isNotDelimeter(const char c)
{
	if (c == 0) return 0;
	return strchr("&|<>=+-/*%^!()", c) != 0;
}

/*
* checks if the given char c is a letter or undersquare
*/
int isAlpha(const char c)
{
	if (c == 0) return 0;
	return strchr("ABCDEFGHIJKLMNOPQRSTUVWXYZ_", toupper(c)) != 0;
}

/*
* checks if the given char c is a digit or dot
*/
int isDigitDot(const char c)
{
	if (c == 0) return 0;
	return strchr("0123456789.", c) != 0;
}

/*
* checks if the given char c is a digit
*/
int isDigit(const char c)
{
	if (c == 0) return 0;
	return strchr("0123456789", c) != 0;
}

void StrToUpper(const char *source, char *dest)
{
	unsigned int i;
	unsigned int len;

	len = strlen(source);
	for(i = 0; i < len; i++)
	{
		dest[i] = toupper(source[i]);
	}
}


/**
* Get next token in the current string expr.
* Uses the Parser data expr, e, token, t, token_type and err
*/
void Parser_getToken(struct Parser *self)
{
	char* t = NULL;           // points to a character in token
	char* e2 = NULL;
	self->token_type = NOTHING;
	
	t = self->token;         // let t point to the first character in token
	*t = '\0';         // set token empty

	//printf("\tgetToken e:{%c}, ascii=%i, col=%i\n", *e, *e, e-expr);

	// skip over whitespaces
	while (*self->e == ' ' || *self->e == '\t')     // space or tab
	{
		self->e++;
	}

	// check for end of expression
	if (*self->e == '\0')
	{
		// token is still empty
		self->token_type = DELIMETER;
		return;
	}

	// check for minus
	if (*self->e == '-')
	{
		self->token_type = DELIMETER;
		*t = *self->e;
		self->e++;
		t++;
		*t = '\0';  // add a null character at the end of token
		return;
	}

	// check for parentheses
	if (*self->e == '(' || *self->e == ')')
	{
		self->token_type = DELIMETER;
		*t = *self->e;
		self->e++;
		t++;
		*t = '\0';
		return;
	}

	// check for operators (delimeters)
	if (isDelimeter(*self->e))
	{
		self->token_type = DELIMETER;
		while (isDelimeter(*self->e))
		{
			*t = *self->e;
			self->e++;
			t++;
		}
		*t = '\0';  // add a null character at the end of token
		return;
	}

	// check for a value
	if (isDigitDot(*self->e))
	{
		self->token_type = NUMBER;
		while (isDigitDot(*self->e))
		{
			*t = *self->e;
			self->e++;
			t++;
		}

		// check for scientific notation like "2.3e-4" or "1.23e50"
		if (toupper(*self->e) == 'E')
		{
			*t = *self->e;
			self->e++;
			t++;

			if (*self->e == '+' || *self->e == '-')
			{
				*t = *self->e;
				self->e++;
				t++;
			}

			while (isDigit(*self->e))
			{
				*t = *self->e;
				self->e++;
				t++;
			}
		}

		*t = '\0';
		return;
	}

	// check for variables or functions
	if (isAlpha(*self->e))
	{
		while (isAlpha(*self->e) || isDigit(*self->e))
			//while (isNotDelimeter(*e))
		{
			*t = *self->e;
			self->e++;
			t++;
		}
		*t = '\0';  // add a null character at the end of token

		// check if this is a variable or a function.
		// a function has a parentesis '(' open after the name 
		e2 = self->e;

		// skip whitespaces
		while (*e2 == ' ' || *e2 == '\t')     // space or tab
		{
			e2++;
		}

		if (*e2 == '(') 
		{
			self->token_type = FUNCTION;
		}
		else
		{
			self->token_type = VARIABLE;
		}
		return;
	}

	// something unknown is found, wrong characters -> a syntax error
	self->token_type = UNKNOWN;
	while (*self->e != '\0')
	{
		*t = *self->e;
		self->e++;
		t++;
	}
	*t = '\0';

	return;
}


/*
* assignment of variable or function
*/
double Parser_parse_level1(struct Parser *self)
{
	//double ans;
	//char token_now[NAME_LEN_MAX+1];
	char* e_now = self->e;
	enum TOKENTYPE token_type_now = self->token_type;

	if (self->token_type == VARIABLE)
	{
		// copy current token
		//strcpy(token_now, self->token);

		//Parser_getToken(self);
		//if (strcmp(self->token, "=") == 0)
		//{
		//	// assignment
		//	Parser_getToken(self);
		//	ans = Parser_parse_level2(self);
		//	if (self->user_var.add(token_now, ans) == FALSE)
		//	{
		//		return 0.0;
		//	}
		//	return ans;
		//}
		//else
		//{
		//	// go back to previous token
		//	self->e = MemoryManager_StrDup(e_now);
		//	self->token_type = token_type_now;
		//	strcpy(self->token, token_now);
		//}
	}

	return Parser_parse_level2(self);
}


/*
* conditional operators and bitshift
*/
double Parser_parse_level2(struct Parser *self)
{
	int op_id;
	double ans;
	ans = Parser_parse_level3(self);

	op_id = Parser_get_operator_id(self, self->token);
	while (op_id == AND || op_id == OR || op_id == BITSHIFTLEFT || op_id == BITSHIFTRIGHT)
	{
		Parser_getToken(self);
		ans = Parser_eval_operator(op_id, &ans, Parser_parse_level3(self));
		op_id = Parser_get_operator_id(self, self->token);
	}

	return ans;
}

/*
* conditional operators
*/
double Parser_parse_level3(struct Parser *self)
{
	int op_id;
	double ans;
	ans = Parser_parse_level4(self);

	op_id = Parser_get_operator_id(self, self->token);
	while (op_id == EQUAL || op_id == UNEQUAL || op_id == SMALLER || op_id == LARGER || op_id == SMALLEREQ || op_id == LARGEREQ)
	{
		Parser_getToken(self);
		ans = Parser_eval_operator(op_id, &ans, Parser_parse_level4(self));
		op_id = Parser_get_operator_id(self, self->token);
	}

	return ans;
}

/*
* add or subtract
*/
double Parser_parse_level4(struct Parser *self)
{
	int op_id;
	double ans;
	ans = Parser_parse_level5(self);

	op_id = Parser_get_operator_id(self, self->token);
	while (op_id == PLUS || op_id == MINUS)
	{
		Parser_getToken(self);
		ans = Parser_eval_operator(op_id, &ans, Parser_parse_level5(self));
		op_id = Parser_get_operator_id(self, self->token);
	}

	return ans;
}


/*
* multiply, divide, modulus, xor
*/
double Parser_parse_level5(struct Parser *self)
{
	int op_id;
	double ans;
	ans = Parser_parse_level6(self);

	op_id = Parser_get_operator_id(self, self->token);
	while (op_id == MULTIPLY || op_id == DIVIDE || op_id == MODULUS || op_id == XOR)
	{
		Parser_getToken(self);
		ans = Parser_eval_operator(op_id, &ans, Parser_parse_level6(self));
		op_id = Parser_get_operator_id(self, self->token);
	}

	return ans;
}


/*
* power
*/
double Parser_parse_level6(struct Parser *self)
{
	int op_id;
	double ans;
	ans = Parser_parse_level7(self);

	op_id = Parser_get_operator_id(self, self->token);
	while (op_id == POW)
	{
		Parser_getToken(self);
		ans = Parser_eval_operator(op_id, &ans, Parser_parse_level7(self));
		op_id = Parser_get_operator_id(self, self->token);
	}

	return ans;
}

/*
* Factorial
*/
double Parser_parse_level7(struct Parser *self)
{
	int op_id;
	double ans;
	ans = Parser_parse_level8(self);

	op_id = Parser_get_operator_id(self, self->token);
	while (op_id == FACTORIAL)
	{
		Parser_getToken(self);
		// factorial does not need a value right from the 
		// operator, so zero is filled in.
		ans = Parser_eval_operator(op_id, &ans, 0.0);
		op_id = Parser_get_operator_id(self, self->token);
	}

	return ans;
}

/*
* Unary minus
*/
double Parser_parse_level8(struct Parser *self)
{
	double ans;

	int op_id = Parser_get_operator_id(self, self->token);    
	if (op_id == MINUS)
	{
		Parser_getToken(self);
		ans = Parser_parse_level9(self);
		ans = -ans;
	}
	else
	{
		ans = Parser_parse_level9(self);
	}

	return ans;
}


/*
* functions
*/
double Parser_parse_level9(struct Parser *self)
{
	char fn_name[NAME_LEN_MAX+1];
	double ans, parseRes;

	if (self->token_type == FUNCTION)
	{
		strcpy(fn_name, self->token);
		Parser_getToken(self);
		parseRes = Parser_parse_level10(self);
		ans = Parser_eval_function(fn_name, &parseRes);
	}
	else
	{
		ans = Parser_parse_level10(self);
	}

	return ans;
}


/*
* parenthesized expression or value
*/
double Parser_parse_level10(struct Parser *self)
{
	double ans = 0.0;

	// check if it is a parenthesized expression
	if (self->token_type == DELIMETER)
	{
		if ((self->token[0] == '(') && (self->token[1] == '\0'))
		{
			Parser_getToken(self);
			ans = Parser_parse_level2(self);
			if (self->token_type != DELIMETER || self->token[0] != ')' || self->token[1] || '\0')
			{
				return 0.0;
			}
			Parser_getToken(self);
			return ans;
		}
	}

	// if not parenthesized then the expression is a value
	return Parser_parse_number(self);
}


double Parser_parse_number(struct Parser *self)
{
	double ans = 0.0;

	switch (self->token_type)
	{
	case NUMBER:
		// this is a number
		ans = atof(self->token);
		Parser_getToken(self);
		break;

	case VARIABLE:
		// this is a variable
		ans = Parser_eval_variable(self, self->token);
		Parser_getToken(self);  
		break;

	default:
		// syntax error or unexpected end of expression
		if (self->token[0] == '\0')
		{
			return 0.0;
		}
		else
		{
			return 0.0;
		}
		break;
	}

	return ans;
}


/*
* returns the id of the given operator
* treturns -1 if the operator is not recognized
*/
int Parser_get_operator_id(struct Parser *self, const char op_name[])
{
	// level 2
	if (!strcmp(op_name, "&")) {return AND;}
	if (!strcmp(op_name, "|")) {return OR;}
	if (!strcmp(op_name, "<<")) {return BITSHIFTLEFT;}
	if (!strcmp(op_name, ">>")) {return BITSHIFTRIGHT;}

	// level 3
	if (!strcmp(op_name, "=")) {return EQUAL;}
	if (!strcmp(op_name, "<>")) {return UNEQUAL;}
	if (!strcmp(op_name, "<")) {return SMALLER;}
	if (!strcmp(op_name, ">")) {return LARGER;}
	if (!strcmp(op_name, "<=")) {return SMALLEREQ;}
	if (!strcmp(op_name, ">=")) {return LARGEREQ;}

	// level 4
	if (!strcmp(op_name, "+")) {return PLUS;}
	if (!strcmp(op_name, "-")) {return MINUS;}

	// level 5
	if (!strcmp(op_name, "*")) {return MULTIPLY;}
	if (!strcmp(op_name, "/")) {
		return DIVIDE;}
	if (!strcmp(op_name, "%")) {return MODULUS;}
	if (!strcmp(op_name, "||")) {return XOR;}

	// level 6
	if (!strcmp(op_name, "^")) {return POW;}

	// level 7
	if (!strcmp(op_name, "!")) {return FACTORIAL;}

	return -1;
}


/*
* evaluate an operator for given valuess
*/
double Parser_eval_operator(const int op_id, const double *lhs, const double rhs)
{
	switch (op_id)
	{
		// level 2
	case AND:           return (int)(*lhs) & (int)(rhs);
	case OR:            return (int)(*lhs) | (int)(rhs);
	case BITSHIFTLEFT:  return (int)(*lhs) << (int)(rhs);
	case BITSHIFTRIGHT: return (int)(*lhs) >> (int)(rhs);

		// level 3
	case EQUAL:     return *lhs == rhs;
	case UNEQUAL:   return *lhs != rhs;
	case SMALLER:   return *lhs < rhs;
	case LARGER:    return *lhs > rhs;
	case SMALLEREQ: return *lhs <= rhs;
	case LARGEREQ:  return *lhs >= rhs;

		// level 4
	case PLUS:      return *lhs + rhs;
	case MINUS:     return *lhs - rhs;

		// level 5
	case MULTIPLY:  return *lhs * rhs;
	case DIVIDE:    return *lhs / rhs;
	case MODULUS:   return (int)(*lhs) % (int)(rhs); // todo: give a warning if the values are not integer?
	case XOR:       return (int)(*lhs) ^ (int)(rhs);

		// level 6
	case POW:       return pow(*lhs, rhs);

		// level 7
	case FACTORIAL: return factorial(*lhs);
	}

	return 0;
}


/*
* evaluate a function
*/
double Parser_eval_function(const char fn_name[], const double *value)
{
	// first make the function name upper case
	char fnU[NAME_LEN_MAX+1];
	StrToUpper(fn_name, fnU);

	// arithmetic 
	if (!strcmp(fnU, "ABS")) {return abs(*value);}
	if (!strcmp(fnU, "EXP")) {return exp(*value);}
	if (!strcmp(fnU, "SIGN")) {return sign(*value);}
	if (!strcmp(fnU, "SQRT")) {return sqrt(*value);}
	if (!strcmp(fnU, "LOG")) {return log(*value);}
	if (!strcmp(fnU, "LOG10")) {return log10(*value);}

	// trigonometric
	if (!strcmp(fnU, "SIN")) {return sin(*value);}
	if (!strcmp(fnU, "COS")) {return cos(*value);}
	if (!strcmp(fnU, "TAN")) {return tan(*value);}
	if (!strcmp(fnU, "ASIN")) {return asin(*value);}
	if (!strcmp(fnU, "ACOS")) {return acos(*value);}
	if (!strcmp(fnU, "ATAN")) {return atan(*value);}

	// probability
	if (!strcmp(fnU, "FACTORIAL")) {return factorial(*value);}
	
	return 0;
}


/*
* evaluate a variable
*/
double Parser_eval_variable(struct Parser *self, const char var_name[])
{
	// first make the variable name uppercase
	char varU[NAME_LEN_MAX+1];	
	StrToUpper(var_name, varU);

	// check for built-in variables
	if (!strcmp(varU, "E")) {return 2.7182818284590452353602874713527;}
	if (!strcmp(varU, "PI")) {return 3.1415926535897932384626433832795;}

	// check for user defined variables

	/*if (self->user_var.get_value(var_name, &ans))
	{
		return ans;
	}*/

	return 0;
}



/*
* Shortcut for getting the current row value (one based)
* Returns the line of the currently handled expression
*/
int Parser_row()
{
	return -1;
}

/*
* Shortcut for getting the current col value (one based)
* Returns the column (position) where the last token starts
*/
int Parser_col(struct Parser *self)
{
	return self->e-self->expr-strlen(self->token)+1;
}




// FUNCTIONS
double factorial(double value)
{
	double res;
	int v = 0;
	v = (int)(value);

	if (value != (double)(v))
	{
		return 0.0;
	}

	res = v;
	v--;
	while (v > 1)
	{
		res *= v;
		v--;
	}

	if (res == 0) res = 1;        // 0! is per definition 1
	return res;
}



double sign(double value)
{
	if (value > 0) return 1;
	if (value < 0) return -1;
	return 0;
}
