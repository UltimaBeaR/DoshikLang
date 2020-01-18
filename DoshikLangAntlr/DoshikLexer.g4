lexer grammar DoshikLexer;

// Keywords

NEWCONST: 'new' Whitespace 'const';
EVENT: 'event';
PRIVATE: 'private';
PUBLIC: 'public';
OUT: 'out';
IF: 'if';
ELSE: 'else';
WHILE: 'while';
FOR: 'for';
BREAK: 'break';
CONTINUE: 'continue';
RETURN: 'return';

// simple type keywords

INT: 'int';
FLOAT: 'float';
BOOL: 'bool';
STRING: 'string';
VOID: 'void';

// Literals

INT_LITERAL: Digits;
INT_HEX_LITERAL: '0' [xX] HexDigits;
FLOAT_LITERAL: Digits '.' Digits;
BOOL_LITERAL: 'true' | 'false';
STRING_LITERAL: '"' (~["\r\n])* '"';
NULL_LITERAL: 'null';

// Separators

OPEN_PARENTHESIS: '(';
CLOSE_PARENTHESIS: ')';
OPEN_BRACE: '{';
CLOSE_BRACE: '}';
OPEN_BRACKET: '[';
CLOSE_BRACKET: ']';
SEMICOLON: ';';
COMMA: ',';
DOT: '.';

// Operators

ASSIGN: '=';
ADD_ASSIGN: '+=';
SUB_ASSIGN: '-=';
MUL_ASSIGN: '*=';
DIV_ASSIGN: '/=';
MOD_ASSIGN: '%=';

GT: '>';
LT: '<';
BANG: '!';
QUESTION: '?';
COLON: ':';
EQUAL: '==';
LE: '<=';
GE: '>=';
NOTEQUAL: '!=';
AND: '&&';
OR:  '||';
INC: '++';
DEC: '--';
ADD: '+';
SUB: '-';
MUL: '*';
DIV: '/';
MOD: '%';

// Whitespace and comments

WHITESPACE:
    Whitespace
    -> channel(HIDDEN);

MULTILINE_COMMENT:
    '/*' .*? '*/'
    -> channel(HIDDEN);

COMMENT:
    '//' ~[\r\n]*
    -> channel(HIDDEN);

// Identifiers

IDENTIFIER: IdentifierLetter (IdentifierLetter | Digit)*;

// Fragment rules

fragment Whitespace: [ \t\r\n\u000C]+;

fragment IdentifierLetter: [a-zA-Z_];

fragment Digits: Digit+;
fragment HexDigits: HexDigit+;

fragment Digit: [0-9];
fragment HexDigit: [0-9a-fA-F];