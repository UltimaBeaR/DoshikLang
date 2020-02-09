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
    : IDENTIFIER typeArguments? ('::' IDENTIFIER typeArguments?)*
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
    : '{' statementInBlock* '}'
    ;

statementInBlock
    : localVariableDeclaration ';'
    | statement
    ;

localVariableDeclaration
    : typeType variableDeclarator
    ;

statement
    : block                                                                             # subBlockStatement
    | IF condition=parExpression trueBody=statement (ELSE falseBody=statement)?         # ifStatement
    | FOR '(' forControl ')' body=statement                                             # forLoopStatement
    | WHILE condition=parExpression body=statement                                      # whileLoopStatement
    | RETURN expression? ';'                                                            # returnStatement
    | BREAK ';'                                                                         # breakStatement
    | CONTINUE ';'                                                                      # continueStatement
    | ';'                                                                               # nopStatement
    | expression ';'                                                                    # expressionStatement
    ;

forControl:
    forInit? ';' expression? ';' forUpdate=expressionList?
    ;

forInit
    : localVariableDeclaration
    | expressionList
    ;

// EXPRESSIONS

// Используется только для выражения типа if ( expression )  Просто оператор приоритета скобками называется primary -> parenthesisExpression
parExpression
    : '(' expression ')'
    ;

expressionList
    : expression (',' expression)*
    ;

defaultOfType: DEFAULT '(' typeType ')';

methodCall: methodName=IDENTIFIER typeArguments? '(' methodCallParams? ')';

methodCallParams
    : methodCallParam (',' methodCallParam)*
    ;

// В коде надо будет проверять что если стоит out, то expression может быть только референсом на переменную, а не любым выражением
methodCallParam:
    OUT? expression;

newCall: NEW typeType '(' methodCallParams? ')';

expression
    : primary                                                                                               # primaryExpression   
    | left=typeType '.' ( rightIdentifier=IDENTIFIER | rightMethodCall=methodCall)                          # typeDotExpression
    | left=expression '.' ( rightIdentifier=IDENTIFIER | rightMethodCall=methodCall)                        # dotExpression

    // если будут типы, поддерживающие синтаксис a[one, two, ...] то можно будет в right сделать expressionList
    | left=expression '[' right=expression ']'                                                              # bracketsExpression

    | defaultOfType                                                                                         # defaultOfTypeExpression
    | methodCall                                                                                            # methodCallExpression
    | newCall                                                                                               # newCallExpression
    | '(' typeType ')' expression                                                                           # typecastExpression
    | expression postfix=('++' | '--')                                                                      # unaryPostfixExpression
    | prefix=('+'|'-'|'++'|'--') expression                                                                 # unaryPrefixExpression
    | '!' expression                                                                                        # notExpression
    | left=expression operator=('*'|'/'|'%') right=expression                                               # multiplicationExpression
    | left=expression operator=('+'|'-') right=expression                                                   # additionExpression
    | left=expression operator=('<=' | '>=' | '>' | '<') right=expression                                   # relativeExpression
    | left=expression operator=('==' | '!=') right=expression                                               # equalsExpression
    | left=expression '&&' right=expression                                                                 # andExpression
    | left=expression '||' right=expression                                                                 # orExpression
    | <assoc=right> condition=expression '?' trueExpression=expression ':' falseExpression=expression        # ifElseExpression
    | <assoc=right> left=expression operator=('=' | '+=' | '-=' | '*=' | '/=' | '%=') right=expression      # assignmentExpression
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
    : BOOL
    | BYTE
    | SBYTE
    | CHAR
    | DECIMAL
    | DOUBLE
    | FLOAT
    | INT
    | UINT
    | LONG
    | ULONG
    | SHORT
    | USHORT
    | OBJECT
    | STRING
    ;
    
typeArguments
    : '<' typeArgument (',' typeArgument)* '>'
    ;
