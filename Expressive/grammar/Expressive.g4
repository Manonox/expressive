grammar Expressive;


chunk
    : statement_list EOF
    ;


block
    : LEFT_BRACE statement_list RIGHT_BRACE
    ;


statement_list
    : statement*
    ;


statement
    : let_statement
    | assign_statement
    | compound_statement
    | expression
    | block
    | if_else_statement
    | for_statement
    | while_statement
    | loop_statement
    | return_statement
    | function_def
    ;

let_statement
    : 'let' 'global'? binding ('=' expression)?
    ;

assign_statement
    : variable '=' expression
    ;

compound_statement
    : variable compound_op expression
    ;

if_else_statement
    : 'if' expression block ('else' 'if' expression block)* ('else' block)?
    ;

for_statement
    : 'for' binding 'in' expression block
    ;

while_statement
    : 'while' expression block
    ;

loop_statement
    : 'loop' block
    ;

return_statement
    : 'return' expression
    ;


simple_expression
    : NIL
    | FALSE
    | TRUE
    | INT
    | FLOAT
    | STRING
    | IDENTIFIER
    | lambda_def
    | LEFT_PARENTHESIS expression RIGHT_PARENTHESIS
    | terrorist_if
    ;

expression
    : postfix
    | (SUB | NOT | BITWISE_NOT) expression
    | <assoc = right> expression POW expression
    | expression (MUL | DIV | MOD) expression
    | expression (ADD | SUB) expression
    | expression (BITWISE_LEFT_SHIFT | BITWISE_RIGHT_SHIFT) expression
    | expression (RANGE | INCLUSIVE_RANGE) expression
    | expression (LESS | LESS_OR_EQUALS | GREATER | GREATER_OR_EQUALS) expression
    | expression (EQUALS | NOT_EQUALS) expression
    | expression BITWISE_AND expression
    | expression BITWISE_XOR expression
    | expression BITWISE_OR expression
    | expression AND expression
    | expression OR expression
    | simple_expression
    ;

postfix
    : simple_expression (call | index)+
    ;

call
    : LEFT_PARENTHESIS (expression (',' expression)*)? RIGHT_PARENTHESIS
    ;

index
    : LEFT_BRACKET (expression (',' expression)*) RIGHT_BRACKET
    ;


terrorist_if
    : 'if' expression block 'else' block
    ;

binding
    : IDENTIFIER (':' IDENTIFIER)?
    ;

binding_list
    : (binding (',' binding)*)?
    ;

variable
    : IDENTIFIER
    ;

function_def
    : 'fn' 'global'? IDENTIFIER LEFT_PARENTHESIS binding_list RIGHT_PARENTHESIS ('->' IDENTIFIER)? block
    ;

lambda_def
    : 'fn' LEFT_PARENTHESIS binding_list RIGHT_PARENTHESIS ('->' IDENTIFIER)? block
    | BITWISE_OR binding_list BITWISE_OR ('->' IDENTIFIER)? block
    ;


IDENTIFIER
    : [a-zA-Z_][a-zA-Z0-9_]*
    ;

INT
    : [0-9]+
    ;

FLOAT
    : [0-9]* '.' [0-9]+
    ;

STRING
    : '"' ( EscapeSequence | ~('\\' | '"'))* '"'
    ;

COMMENT
    : ('//' ~('\r' | '\n')* '\r'? '\n') -> skip
    ;

MULTI_COMMENT
    : ('/*' .*? '*/') -> skip
    ;

compound_op
    : OR_ASSIGNMENT
    | AND_ASSIGNMENT
    | ADD_ASSIGNMENT
    | SUB_ASSIGNMENT
    | MUL_ASSIGNMENT
    | DIV_ASSIGNMENT
    | MOD_ASSIGNMENT
    | POW_ASSIGNMENT
    | BITWISE_OR_ASSIGNMENT
    | BITWISE_AND_ASSIGNMENT
    | BITWISE_XOR_ASSIGNMENT
    | BITWISE_LEFT_SHIFT_ASSIGNMENT
    | BITWISE_RIGHT_SHIFT_ASSIGNMENT
    ;


WS
    : (' ' | '\t' | '\u000C' | '\r' | '\n')+ -> skip
    ;


fragment EscapeSequence:
    '\\' [abfnrtvz"'\\]
    | '\\' '\r'? '\n'
    | DecimalEscape
    | HexEscape
    | UtfEscape
;

fragment DecimalEscape: '\\' Digit | '\\' Digit Digit | '\\' [0-2] Digit Digit;

fragment HexEscape: '\\' 'x' HexDigit HexDigit;

fragment UtfEscape: '\\' 'u{' HexDigit+ '}';

fragment Digit: [0-9];

fragment HexDigit: [0-9a-fA-F];



LEFT_PARENTHESIS:
    '(';
RIGHT_PARENTHESIS:
    ')';

LEFT_BRACKET:
    '[';
RIGHT_BRACKET:
    ']';

LEFT_BRACE:
    '{';
RIGHT_BRACE:
    '}';


NIL:
    'nil';

FALSE:
    'false';

TRUE:
    'true';


ADD:
    '+';
SUB:
    '-';
MUL:
    '*';
DIV:
    '/';
MOD:
    '%';

POW:
    '**';

NOT:
    '!';
BITWISE_NOT:
    '~';

BITWISE_LEFT_SHIFT:
    '<<';
BITWISE_RIGHT_SHIFT:
    '>>';

RANGE:
    '..';
INCLUSIVE_RANGE:
    '..=';

LESS:
    '<';
LESS_OR_EQUALS:
    '<=';
GREATER:
    '>';
GREATER_OR_EQUALS:
    '>=';

EQUALS:
    '==';
NOT_EQUALS:
    '!=';

BITWISE_AND:
    '&';
BITWISE_XOR:
    '^';
BITWISE_OR:
    '|';

AND:
    '&&';
OR:
    '||';


OR_ASSIGNMENT:
    '||=';

AND_ASSIGNMENT:
    '&&=';

ADD_ASSIGNMENT:
    '+=';

SUB_ASSIGNMENT:
    '-=';

MUL_ASSIGNMENT:
    '*=';

DIV_ASSIGNMENT:
    '/=';

MOD_ASSIGNMENT:
    '%=';

POW_ASSIGNMENT:
    '**=';

BITWISE_OR_ASSIGNMENT:
    '|=';

BITWISE_AND_ASSIGNMENT:
    '&=';

BITWISE_XOR_ASSIGNMENT:
    '^=';

BITWISE_LEFT_SHIFT_ASSIGNMENT:
    '<<=';

BITWISE_RIGHT_SHIFT_ASSIGNMENT:
    '>>=';