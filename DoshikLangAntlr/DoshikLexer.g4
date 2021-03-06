lexer grammar DoshikLexer;

// Keywords

DEFAULT: 'default';
TYPEOF: 'typeof';
NEW: 'new';
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

// Simple type keywords

BOOL: 'bool';
BYTE: 'byte';
SBYTE: 'sbyte';
CHAR: 'char';
DECIMAL: 'decimal';
DOUBLE: 'double';
FLOAT: 'float';
INT: 'int';
UINT: 'uint';
LONG: 'long';
ULONG: 'ulong';
SHORT: 'short';
USHORT: 'ushort';

OBJECT: 'object';
STRING: 'string';

VOID: 'void';

// Literals

INT_LITERAL: Digits;
INT_HEX_LITERAL: '0' [xX] HexDigits;
FLOAT_LITERAL: Digits '.' Digits FloatLiteralPostfix?;
BOOL_LITERAL: 'true' | 'false';
STRING_LITERAL: '"' (~["\r\n])* '"';
NULL_LITERAL: 'null';

// Separators

SCOPE_RESOLUTION: '::';
DOT: '.';
SEMICOLON: ';';
COMMA: ',';
OPEN_PARENTHESIS: '(';
CLOSE_PARENTHESIS: ')';
OPEN_BRACE: '{';
CLOSE_BRACE: '}';
OPEN_BRACKET: '[';
CLOSE_BRACKET: ']';

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

fragment FloatLiteralPostfix: [fd];

fragment Digits: Digit+;
fragment HexDigits: HexDigit+;

fragment Digit: [0-9];
fragment HexDigit: [0-9a-fA-F];