parser grammar DoshikParser;

options { tokenVocab=DoshikLexer; }

compilationUnit: memberDeclaration* EOF;

memberDeclaration:
    methodDeclaration
    | fieldDeclaration
    ;

methodDeclaration:
    EVENT? returnType=typeTypeOrVoid methodName=IDENTIFIER formalParameters
        block
    ;

typeTypeOrVoid:
    typeType
    | VOID
    ;

fieldDeclaration
    : (PUBLIC | PRIVATE)? typeType variableDeclarator ';'
    ;

variableDeclarator
    : variableName=IDENTIFIER ('=' variableInitializer)?
    ;

variableInitializer
    : arrayInitializer
    | expression
    ;

arrayInitializer
    : '{' (variableInitializer (',' variableInitializer)* (',')? )? '}'
    ;

// Для определения не примитивных типов переменных при их объявлении (на данный момент это будут типы, которые есть в апи удона)
classOrInterfaceType
    : IDENTIFIER typeArguments? ('.' IDENTIFIER typeArguments?)*
    ;

typeArgument
    : typeType
    ;

formalParameters
    : '(' formalParameterList? ')'
    ;

formalParameterList
    : formalParameter (',' formalParameter)*
    ;

formalParameter
    : OUT? typeType parameterName=IDENTIFIER
    ;

literal
    : integerLiteral
    | FLOAT_LITERAL
    | STRING_LITERAL
    | BOOL_LITERAL
    | NULL_LITERAL
    ;

integerLiteral
    : INT_LITERAL
    | INT_HEX_LITERAL
    ;

// STATEMENTS / BLOCKS

block
    : '{' blockStatement* '}'
    ;

blockStatement
    : localVariableDeclaration ';'
    | statement
    ;

localVariableDeclaration
    : typeType variableDeclarator
    ;

statement
    : blockLabel=block
    | IF parExpression statement (ELSE statement)?
    | FOR '(' forControl ')' statement
    | WHILE parExpression statement
    | RETURN expression? ';'
    | BREAK IDENTIFIER? ';'
    | CONTINUE IDENTIFIER? ';'
    | ';'
    | statementExpression=expression ';'
    ;

forControl:
    forInit? ';' expression? ';' forUpdate=expressionList?
    ;

forInit
    : localVariableDeclaration
    | expressionList
    ;

// EXPRESSIONS

parExpression
    : '(' expression ')'
    ;

expressionList
    : expression (',' expression)*
    ;

methodCall: IDENTIFIER typeArguments? '(' methodCallParams? ')';

methodCallParams
    : methodCallParam (',' methodCallParam)*
    ;

methodCallParam:
    expression
    | OUT outVariableName=IDENTIFIER;

newCall: NEW typeType '(' methodCallParams? ')';

newConstCall: NEWCONST typeType '(' newConstCallParams? ')';

newConstCallParams
    : newConstCallParam (',' newConstCallParam)*
    ;

newConstCallParam:
    constExpression;

constExpression:
    literal
    | newConstCall
    | constArrayInitializer
    ;

constArrayInitializer
    : '{' (constExpression (',' constExpression)* (',')? )? '}'
    ;

expression
    : primary                                                                               # primaryExpression   
    | expression bop='.' ( IDENTIFIER | methodCall)                                         # dotExpression
    | expression '[' expression ']'                                                         # bracketsExpression
    | methodCall                                                                            # methodCallExpression
    | newConstCall                                                                          # newConstCallExpression
    | newCall                                                                               # newCallExpression
    | '(' typeType ')' expression                                                           # typecastExpression
    | expression postfix=('++' | '--')                                                      # unaryPostfixExpression
    | prefix=('+'|'-'|'++'|'--') expression                                                 # unaryPrefixExpression
    | '!' expression                                                                        # notExpression
    | expression bop=('*'|'/'|'%') expression                                               # multiplicationExpression
    | expression bop=('+'|'-') expression                                                   # additionExpression
    | expression bop=('<=' | '>=' | '>' | '<') expression                                   # relativeExpression
    | expression bop=('==' | '!=') expression                                               # equalsExpression
    | expression bop='&&' expression                                                        # andExpression
    | expression bop='||' expression                                                        # orExpression
    | <assoc=right> expression bop='?' expression ':' expression                            # ifElseExpression
    | <assoc=right> expression bop=('=' | '+=' | '-=' | '*=' | '/=' | '%=') expression      # assignmentExpression
    ;

primary
    : '(' expression ')' # parenthesisExpression
    | literal            # literalExpression
    | IDENTIFIER         # identifierExpression
    ;

typeType
    : (classOrInterfaceType | primitiveType) ('[' ']')*
    ;

primitiveType
    : INT
    | FLOAT
    | BOOL
    | STRING
    ;

typeArguments
    : '<' typeArgument (',' typeArgument)* '>'
    ;
