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
    : (PUBLIC | PRIVATE)? typeType variableDeclarators ';'
    ;

variableDeclarators
    : variableDeclarator (',' variableDeclarator)*
    ;

variableDeclarator
    : variableDeclaratorId ('=' variableInitializer)?
    ;

variableDeclaratorId: IDENTIFIER;

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
    : OUT? typeType variableDeclaratorId
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
    : typeType variableDeclarators
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

forControl
    : enhancedForControl
    | forInit? ';' expression? ';' forUpdate=expressionList?
    ;

forInit
    : localVariableDeclaration
    | expressionList
    ;

enhancedForControl
    : typeType variableDeclaratorId ':' expression
    ;

// EXPRESSIONS

parExpression
    : '(' expression ')'
    ;

expressionList
    : expression (',' expression)*
    ;

methodCall: IDENTIFIER '(' methodCallParams? ')';

methodCallParams
    : methodCallParam (',' methodCallParam)*
    ;

methodCallParam:
    expression
    | OUT outVariableName=IDENTIFIER;

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
