//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.7.2
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from D:\Programs\DoshikLang\DoshikLangAntlr\DoshikLexer.g4 by ANTLR 4.7.2

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using System;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.7.2")]
[System.CLSCompliant(false)]
public partial class DoshikLexer : Lexer {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		DEFAULT=1, TYPEOF=2, NEW=3, EVENT=4, PRIVATE=5, PUBLIC=6, OUT=7, IF=8, 
		ELSE=9, WHILE=10, FOR=11, BREAK=12, CONTINUE=13, RETURN=14, BOOL=15, BYTE=16, 
		SBYTE=17, CHAR=18, DECIMAL=19, DOUBLE=20, FLOAT=21, INT=22, UINT=23, LONG=24, 
		ULONG=25, SHORT=26, USHORT=27, OBJECT=28, STRING=29, VOID=30, INT_LITERAL=31, 
		INT_HEX_LITERAL=32, FLOAT_LITERAL=33, BOOL_LITERAL=34, STRING_LITERAL=35, 
		NULL_LITERAL=36, SCOPE_RESOLUTION=37, DOT=38, SEMICOLON=39, COMMA=40, 
		OPEN_PARENTHESIS=41, CLOSE_PARENTHESIS=42, OPEN_BRACE=43, CLOSE_BRACE=44, 
		OPEN_BRACKET=45, CLOSE_BRACKET=46, ASSIGN=47, ADD_ASSIGN=48, SUB_ASSIGN=49, 
		MUL_ASSIGN=50, DIV_ASSIGN=51, MOD_ASSIGN=52, GT=53, LT=54, BANG=55, QUESTION=56, 
		COLON=57, EQUAL=58, LE=59, GE=60, NOTEQUAL=61, AND=62, OR=63, INC=64, 
		DEC=65, ADD=66, SUB=67, MUL=68, DIV=69, MOD=70, WHITESPACE=71, MULTILINE_COMMENT=72, 
		COMMENT=73, IDENTIFIER=74;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"DEFAULT", "TYPEOF", "NEW", "EVENT", "PRIVATE", "PUBLIC", "OUT", "IF", 
		"ELSE", "WHILE", "FOR", "BREAK", "CONTINUE", "RETURN", "BOOL", "BYTE", 
		"SBYTE", "CHAR", "DECIMAL", "DOUBLE", "FLOAT", "INT", "UINT", "LONG", 
		"ULONG", "SHORT", "USHORT", "OBJECT", "STRING", "VOID", "INT_LITERAL", 
		"INT_HEX_LITERAL", "FLOAT_LITERAL", "BOOL_LITERAL", "STRING_LITERAL", 
		"NULL_LITERAL", "SCOPE_RESOLUTION", "DOT", "SEMICOLON", "COMMA", "OPEN_PARENTHESIS", 
		"CLOSE_PARENTHESIS", "OPEN_BRACE", "CLOSE_BRACE", "OPEN_BRACKET", "CLOSE_BRACKET", 
		"ASSIGN", "ADD_ASSIGN", "SUB_ASSIGN", "MUL_ASSIGN", "DIV_ASSIGN", "MOD_ASSIGN", 
		"GT", "LT", "BANG", "QUESTION", "COLON", "EQUAL", "LE", "GE", "NOTEQUAL", 
		"AND", "OR", "INC", "DEC", "ADD", "SUB", "MUL", "DIV", "MOD", "WHITESPACE", 
		"MULTILINE_COMMENT", "COMMENT", "IDENTIFIER", "Whitespace", "IdentifierLetter", 
		"FloatLiteralPostfix", "Digits", "HexDigits", "Digit", "HexDigit"
	};


	public DoshikLexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public DoshikLexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, "'default'", "'typeof'", "'new'", "'event'", "'private'", "'public'", 
		"'out'", "'if'", "'else'", "'while'", "'for'", "'break'", "'continue'", 
		"'return'", "'bool'", "'byte'", "'sbyte'", "'char'", "'decimal'", "'double'", 
		"'float'", "'int'", "'uint'", "'long'", "'ulong'", "'short'", "'ushort'", 
		"'object'", "'string'", "'void'", null, null, null, null, null, "'null'", 
		"'::'", "'.'", "';'", "','", "'('", "')'", "'{'", "'}'", "'['", "']'", 
		"'='", "'+='", "'-='", "'*='", "'/='", "'%='", "'>'", "'<'", "'!'", "'?'", 
		"':'", "'=='", "'<='", "'>='", "'!='", "'&&'", "'||'", "'++'", "'--'", 
		"'+'", "'-'", "'*'", "'/'", "'%'"
	};
	private static readonly string[] _SymbolicNames = {
		null, "DEFAULT", "TYPEOF", "NEW", "EVENT", "PRIVATE", "PUBLIC", "OUT", 
		"IF", "ELSE", "WHILE", "FOR", "BREAK", "CONTINUE", "RETURN", "BOOL", "BYTE", 
		"SBYTE", "CHAR", "DECIMAL", "DOUBLE", "FLOAT", "INT", "UINT", "LONG", 
		"ULONG", "SHORT", "USHORT", "OBJECT", "STRING", "VOID", "INT_LITERAL", 
		"INT_HEX_LITERAL", "FLOAT_LITERAL", "BOOL_LITERAL", "STRING_LITERAL", 
		"NULL_LITERAL", "SCOPE_RESOLUTION", "DOT", "SEMICOLON", "COMMA", "OPEN_PARENTHESIS", 
		"CLOSE_PARENTHESIS", "OPEN_BRACE", "CLOSE_BRACE", "OPEN_BRACKET", "CLOSE_BRACKET", 
		"ASSIGN", "ADD_ASSIGN", "SUB_ASSIGN", "MUL_ASSIGN", "DIV_ASSIGN", "MOD_ASSIGN", 
		"GT", "LT", "BANG", "QUESTION", "COLON", "EQUAL", "LE", "GE", "NOTEQUAL", 
		"AND", "OR", "INC", "DEC", "ADD", "SUB", "MUL", "DIV", "MOD", "WHITESPACE", 
		"MULTILINE_COMMENT", "COMMENT", "IDENTIFIER"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "DoshikLexer.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ChannelNames { get { return channelNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override string SerializedAtn { get { return new string(_serializedATN); } }

	static DoshikLexer() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}
	private static char[] _serializedATN = {
		'\x3', '\x608B', '\xA72A', '\x8133', '\xB9ED', '\x417C', '\x3BE7', '\x7786', 
		'\x5964', '\x2', 'L', '\x20A', '\b', '\x1', '\x4', '\x2', '\t', '\x2', 
		'\x4', '\x3', '\t', '\x3', '\x4', '\x4', '\t', '\x4', '\x4', '\x5', '\t', 
		'\x5', '\x4', '\x6', '\t', '\x6', '\x4', '\a', '\t', '\a', '\x4', '\b', 
		'\t', '\b', '\x4', '\t', '\t', '\t', '\x4', '\n', '\t', '\n', '\x4', '\v', 
		'\t', '\v', '\x4', '\f', '\t', '\f', '\x4', '\r', '\t', '\r', '\x4', '\xE', 
		'\t', '\xE', '\x4', '\xF', '\t', '\xF', '\x4', '\x10', '\t', '\x10', '\x4', 
		'\x11', '\t', '\x11', '\x4', '\x12', '\t', '\x12', '\x4', '\x13', '\t', 
		'\x13', '\x4', '\x14', '\t', '\x14', '\x4', '\x15', '\t', '\x15', '\x4', 
		'\x16', '\t', '\x16', '\x4', '\x17', '\t', '\x17', '\x4', '\x18', '\t', 
		'\x18', '\x4', '\x19', '\t', '\x19', '\x4', '\x1A', '\t', '\x1A', '\x4', 
		'\x1B', '\t', '\x1B', '\x4', '\x1C', '\t', '\x1C', '\x4', '\x1D', '\t', 
		'\x1D', '\x4', '\x1E', '\t', '\x1E', '\x4', '\x1F', '\t', '\x1F', '\x4', 
		' ', '\t', ' ', '\x4', '!', '\t', '!', '\x4', '\"', '\t', '\"', '\x4', 
		'#', '\t', '#', '\x4', '$', '\t', '$', '\x4', '%', '\t', '%', '\x4', '&', 
		'\t', '&', '\x4', '\'', '\t', '\'', '\x4', '(', '\t', '(', '\x4', ')', 
		'\t', ')', '\x4', '*', '\t', '*', '\x4', '+', '\t', '+', '\x4', ',', '\t', 
		',', '\x4', '-', '\t', '-', '\x4', '.', '\t', '.', '\x4', '/', '\t', '/', 
		'\x4', '\x30', '\t', '\x30', '\x4', '\x31', '\t', '\x31', '\x4', '\x32', 
		'\t', '\x32', '\x4', '\x33', '\t', '\x33', '\x4', '\x34', '\t', '\x34', 
		'\x4', '\x35', '\t', '\x35', '\x4', '\x36', '\t', '\x36', '\x4', '\x37', 
		'\t', '\x37', '\x4', '\x38', '\t', '\x38', '\x4', '\x39', '\t', '\x39', 
		'\x4', ':', '\t', ':', '\x4', ';', '\t', ';', '\x4', '<', '\t', '<', '\x4', 
		'=', '\t', '=', '\x4', '>', '\t', '>', '\x4', '?', '\t', '?', '\x4', '@', 
		'\t', '@', '\x4', '\x41', '\t', '\x41', '\x4', '\x42', '\t', '\x42', '\x4', 
		'\x43', '\t', '\x43', '\x4', '\x44', '\t', '\x44', '\x4', '\x45', '\t', 
		'\x45', '\x4', '\x46', '\t', '\x46', '\x4', 'G', '\t', 'G', '\x4', 'H', 
		'\t', 'H', '\x4', 'I', '\t', 'I', '\x4', 'J', '\t', 'J', '\x4', 'K', '\t', 
		'K', '\x4', 'L', '\t', 'L', '\x4', 'M', '\t', 'M', '\x4', 'N', '\t', 'N', 
		'\x4', 'O', '\t', 'O', '\x4', 'P', '\t', 'P', '\x4', 'Q', '\t', 'Q', '\x4', 
		'R', '\t', 'R', '\x3', '\x2', '\x3', '\x2', '\x3', '\x2', '\x3', '\x2', 
		'\x3', '\x2', '\x3', '\x2', '\x3', '\x2', '\x3', '\x2', '\x3', '\x3', 
		'\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', 
		'\x3', '\x3', '\x3', '\x4', '\x3', '\x4', '\x3', '\x4', '\x3', '\x4', 
		'\x3', '\x5', '\x3', '\x5', '\x3', '\x5', '\x3', '\x5', '\x3', '\x5', 
		'\x3', '\x5', '\x3', '\x6', '\x3', '\x6', '\x3', '\x6', '\x3', '\x6', 
		'\x3', '\x6', '\x3', '\x6', '\x3', '\x6', '\x3', '\x6', '\x3', '\a', '\x3', 
		'\a', '\x3', '\a', '\x3', '\a', '\x3', '\a', '\x3', '\a', '\x3', '\a', 
		'\x3', '\b', '\x3', '\b', '\x3', '\b', '\x3', '\b', '\x3', '\t', '\x3', 
		'\t', '\x3', '\t', '\x3', '\n', '\x3', '\n', '\x3', '\n', '\x3', '\n', 
		'\x3', '\n', '\x3', '\v', '\x3', '\v', '\x3', '\v', '\x3', '\v', '\x3', 
		'\v', '\x3', '\v', '\x3', '\f', '\x3', '\f', '\x3', '\f', '\x3', '\f', 
		'\x3', '\r', '\x3', '\r', '\x3', '\r', '\x3', '\r', '\x3', '\r', '\x3', 
		'\r', '\x3', '\xE', '\x3', '\xE', '\x3', '\xE', '\x3', '\xE', '\x3', '\xE', 
		'\x3', '\xE', '\x3', '\xE', '\x3', '\xE', '\x3', '\xE', '\x3', '\xF', 
		'\x3', '\xF', '\x3', '\xF', '\x3', '\xF', '\x3', '\xF', '\x3', '\xF', 
		'\x3', '\xF', '\x3', '\x10', '\x3', '\x10', '\x3', '\x10', '\x3', '\x10', 
		'\x3', '\x10', '\x3', '\x11', '\x3', '\x11', '\x3', '\x11', '\x3', '\x11', 
		'\x3', '\x11', '\x3', '\x12', '\x3', '\x12', '\x3', '\x12', '\x3', '\x12', 
		'\x3', '\x12', '\x3', '\x12', '\x3', '\x13', '\x3', '\x13', '\x3', '\x13', 
		'\x3', '\x13', '\x3', '\x13', '\x3', '\x14', '\x3', '\x14', '\x3', '\x14', 
		'\x3', '\x14', '\x3', '\x14', '\x3', '\x14', '\x3', '\x14', '\x3', '\x14', 
		'\x3', '\x15', '\x3', '\x15', '\x3', '\x15', '\x3', '\x15', '\x3', '\x15', 
		'\x3', '\x15', '\x3', '\x15', '\x3', '\x16', '\x3', '\x16', '\x3', '\x16', 
		'\x3', '\x16', '\x3', '\x16', '\x3', '\x16', '\x3', '\x17', '\x3', '\x17', 
		'\x3', '\x17', '\x3', '\x17', '\x3', '\x18', '\x3', '\x18', '\x3', '\x18', 
		'\x3', '\x18', '\x3', '\x18', '\x3', '\x19', '\x3', '\x19', '\x3', '\x19', 
		'\x3', '\x19', '\x3', '\x19', '\x3', '\x1A', '\x3', '\x1A', '\x3', '\x1A', 
		'\x3', '\x1A', '\x3', '\x1A', '\x3', '\x1A', '\x3', '\x1B', '\x3', '\x1B', 
		'\x3', '\x1B', '\x3', '\x1B', '\x3', '\x1B', '\x3', '\x1B', '\x3', '\x1C', 
		'\x3', '\x1C', '\x3', '\x1C', '\x3', '\x1C', '\x3', '\x1C', '\x3', '\x1C', 
		'\x3', '\x1C', '\x3', '\x1D', '\x3', '\x1D', '\x3', '\x1D', '\x3', '\x1D', 
		'\x3', '\x1D', '\x3', '\x1D', '\x3', '\x1D', '\x3', '\x1E', '\x3', '\x1E', 
		'\x3', '\x1E', '\x3', '\x1E', '\x3', '\x1E', '\x3', '\x1E', '\x3', '\x1E', 
		'\x3', '\x1F', '\x3', '\x1F', '\x3', '\x1F', '\x3', '\x1F', '\x3', '\x1F', 
		'\x3', ' ', '\x3', ' ', '\x3', '!', '\x3', '!', '\x3', '!', '\x3', '!', 
		'\x3', '\"', '\x3', '\"', '\x3', '\"', '\x3', '\"', '\x5', '\"', '\x162', 
		'\n', '\"', '\x3', '#', '\x3', '#', '\x3', '#', '\x3', '#', '\x3', '#', 
		'\x3', '#', '\x3', '#', '\x3', '#', '\x3', '#', '\x5', '#', '\x16D', '\n', 
		'#', '\x3', '$', '\x3', '$', '\a', '$', '\x171', '\n', '$', '\f', '$', 
		'\xE', '$', '\x174', '\v', '$', '\x3', '$', '\x3', '$', '\x3', '%', '\x3', 
		'%', '\x3', '%', '\x3', '%', '\x3', '%', '\x3', '&', '\x3', '&', '\x3', 
		'&', '\x3', '\'', '\x3', '\'', '\x3', '(', '\x3', '(', '\x3', ')', '\x3', 
		')', '\x3', '*', '\x3', '*', '\x3', '+', '\x3', '+', '\x3', ',', '\x3', 
		',', '\x3', '-', '\x3', '-', '\x3', '.', '\x3', '.', '\x3', '/', '\x3', 
		'/', '\x3', '\x30', '\x3', '\x30', '\x3', '\x31', '\x3', '\x31', '\x3', 
		'\x31', '\x3', '\x32', '\x3', '\x32', '\x3', '\x32', '\x3', '\x33', '\x3', 
		'\x33', '\x3', '\x33', '\x3', '\x34', '\x3', '\x34', '\x3', '\x34', '\x3', 
		'\x35', '\x3', '\x35', '\x3', '\x35', '\x3', '\x36', '\x3', '\x36', '\x3', 
		'\x37', '\x3', '\x37', '\x3', '\x38', '\x3', '\x38', '\x3', '\x39', '\x3', 
		'\x39', '\x3', ':', '\x3', ':', '\x3', ';', '\x3', ';', '\x3', ';', '\x3', 
		'<', '\x3', '<', '\x3', '<', '\x3', '=', '\x3', '=', '\x3', '=', '\x3', 
		'>', '\x3', '>', '\x3', '>', '\x3', '?', '\x3', '?', '\x3', '?', '\x3', 
		'@', '\x3', '@', '\x3', '@', '\x3', '\x41', '\x3', '\x41', '\x3', '\x41', 
		'\x3', '\x42', '\x3', '\x42', '\x3', '\x42', '\x3', '\x43', '\x3', '\x43', 
		'\x3', '\x44', '\x3', '\x44', '\x3', '\x45', '\x3', '\x45', '\x3', '\x46', 
		'\x3', '\x46', '\x3', 'G', '\x3', 'G', '\x3', 'H', '\x3', 'H', '\x3', 
		'H', '\x3', 'H', '\x3', 'I', '\x3', 'I', '\x3', 'I', '\x3', 'I', '\a', 
		'I', '\x1D7', '\n', 'I', '\f', 'I', '\xE', 'I', '\x1DA', '\v', 'I', '\x3', 
		'I', '\x3', 'I', '\x3', 'I', '\x3', 'I', '\x3', 'I', '\x3', 'J', '\x3', 
		'J', '\x3', 'J', '\x3', 'J', '\a', 'J', '\x1E5', '\n', 'J', '\f', 'J', 
		'\xE', 'J', '\x1E8', '\v', 'J', '\x3', 'J', '\x3', 'J', '\x3', 'K', '\x3', 
		'K', '\x3', 'K', '\a', 'K', '\x1EF', '\n', 'K', '\f', 'K', '\xE', 'K', 
		'\x1F2', '\v', 'K', '\x3', 'L', '\x6', 'L', '\x1F5', '\n', 'L', '\r', 
		'L', '\xE', 'L', '\x1F6', '\x3', 'M', '\x3', 'M', '\x3', 'N', '\x3', 'N', 
		'\x3', 'O', '\x6', 'O', '\x1FE', '\n', 'O', '\r', 'O', '\xE', 'O', '\x1FF', 
		'\x3', 'P', '\x6', 'P', '\x203', '\n', 'P', '\r', 'P', '\xE', 'P', '\x204', 
		'\x3', 'Q', '\x3', 'Q', '\x3', 'R', '\x3', 'R', '\x3', '\x1D8', '\x2', 
		'S', '\x3', '\x3', '\x5', '\x4', '\a', '\x5', '\t', '\x6', '\v', '\a', 
		'\r', '\b', '\xF', '\t', '\x11', '\n', '\x13', '\v', '\x15', '\f', '\x17', 
		'\r', '\x19', '\xE', '\x1B', '\xF', '\x1D', '\x10', '\x1F', '\x11', '!', 
		'\x12', '#', '\x13', '%', '\x14', '\'', '\x15', ')', '\x16', '+', '\x17', 
		'-', '\x18', '/', '\x19', '\x31', '\x1A', '\x33', '\x1B', '\x35', '\x1C', 
		'\x37', '\x1D', '\x39', '\x1E', ';', '\x1F', '=', ' ', '?', '!', '\x41', 
		'\"', '\x43', '#', '\x45', '$', 'G', '%', 'I', '&', 'K', '\'', 'M', '(', 
		'O', ')', 'Q', '*', 'S', '+', 'U', ',', 'W', '-', 'Y', '.', '[', '/', 
		']', '\x30', '_', '\x31', '\x61', '\x32', '\x63', '\x33', '\x65', '\x34', 
		'g', '\x35', 'i', '\x36', 'k', '\x37', 'm', '\x38', 'o', '\x39', 'q', 
		':', 's', ';', 'u', '<', 'w', '=', 'y', '>', '{', '?', '}', '@', '\x7F', 
		'\x41', '\x81', '\x42', '\x83', '\x43', '\x85', '\x44', '\x87', '\x45', 
		'\x89', '\x46', '\x8B', 'G', '\x8D', 'H', '\x8F', 'I', '\x91', 'J', '\x93', 
		'K', '\x95', 'L', '\x97', '\x2', '\x99', '\x2', '\x9B', '\x2', '\x9D', 
		'\x2', '\x9F', '\x2', '\xA1', '\x2', '\xA3', '\x2', '\x3', '\x2', '\n', 
		'\x4', '\x2', 'Z', 'Z', 'z', 'z', '\x5', '\x2', '\f', '\f', '\xF', '\xF', 
		'$', '$', '\x4', '\x2', '\f', '\f', '\xF', '\xF', '\x5', '\x2', '\v', 
		'\f', '\xE', '\xF', '\"', '\"', '\x5', '\x2', '\x43', '\\', '\x61', '\x61', 
		'\x63', '|', '\x4', '\x2', '\x66', '\x66', 'h', 'h', '\x3', '\x2', '\x32', 
		';', '\x5', '\x2', '\x32', ';', '\x43', 'H', '\x63', 'h', '\x2', '\x20C', 
		'\x2', '\x3', '\x3', '\x2', '\x2', '\x2', '\x2', '\x5', '\x3', '\x2', 
		'\x2', '\x2', '\x2', '\a', '\x3', '\x2', '\x2', '\x2', '\x2', '\t', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '\v', '\x3', '\x2', '\x2', '\x2', '\x2', '\r', 
		'\x3', '\x2', '\x2', '\x2', '\x2', '\xF', '\x3', '\x2', '\x2', '\x2', 
		'\x2', '\x11', '\x3', '\x2', '\x2', '\x2', '\x2', '\x13', '\x3', '\x2', 
		'\x2', '\x2', '\x2', '\x15', '\x3', '\x2', '\x2', '\x2', '\x2', '\x17', 
		'\x3', '\x2', '\x2', '\x2', '\x2', '\x19', '\x3', '\x2', '\x2', '\x2', 
		'\x2', '\x1B', '\x3', '\x2', '\x2', '\x2', '\x2', '\x1D', '\x3', '\x2', 
		'\x2', '\x2', '\x2', '\x1F', '\x3', '\x2', '\x2', '\x2', '\x2', '!', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '#', '\x3', '\x2', '\x2', '\x2', '\x2', '%', 
		'\x3', '\x2', '\x2', '\x2', '\x2', '\'', '\x3', '\x2', '\x2', '\x2', '\x2', 
		')', '\x3', '\x2', '\x2', '\x2', '\x2', '+', '\x3', '\x2', '\x2', '\x2', 
		'\x2', '-', '\x3', '\x2', '\x2', '\x2', '\x2', '/', '\x3', '\x2', '\x2', 
		'\x2', '\x2', '\x31', '\x3', '\x2', '\x2', '\x2', '\x2', '\x33', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '\x35', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'\x37', '\x3', '\x2', '\x2', '\x2', '\x2', '\x39', '\x3', '\x2', '\x2', 
		'\x2', '\x2', ';', '\x3', '\x2', '\x2', '\x2', '\x2', '=', '\x3', '\x2', 
		'\x2', '\x2', '\x2', '?', '\x3', '\x2', '\x2', '\x2', '\x2', '\x41', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '\x43', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'\x45', '\x3', '\x2', '\x2', '\x2', '\x2', 'G', '\x3', '\x2', '\x2', '\x2', 
		'\x2', 'I', '\x3', '\x2', '\x2', '\x2', '\x2', 'K', '\x3', '\x2', '\x2', 
		'\x2', '\x2', 'M', '\x3', '\x2', '\x2', '\x2', '\x2', 'O', '\x3', '\x2', 
		'\x2', '\x2', '\x2', 'Q', '\x3', '\x2', '\x2', '\x2', '\x2', 'S', '\x3', 
		'\x2', '\x2', '\x2', '\x2', 'U', '\x3', '\x2', '\x2', '\x2', '\x2', 'W', 
		'\x3', '\x2', '\x2', '\x2', '\x2', 'Y', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'[', '\x3', '\x2', '\x2', '\x2', '\x2', ']', '\x3', '\x2', '\x2', '\x2', 
		'\x2', '_', '\x3', '\x2', '\x2', '\x2', '\x2', '\x61', '\x3', '\x2', '\x2', 
		'\x2', '\x2', '\x63', '\x3', '\x2', '\x2', '\x2', '\x2', '\x65', '\x3', 
		'\x2', '\x2', '\x2', '\x2', 'g', '\x3', '\x2', '\x2', '\x2', '\x2', 'i', 
		'\x3', '\x2', '\x2', '\x2', '\x2', 'k', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'm', '\x3', '\x2', '\x2', '\x2', '\x2', 'o', '\x3', '\x2', '\x2', '\x2', 
		'\x2', 'q', '\x3', '\x2', '\x2', '\x2', '\x2', 's', '\x3', '\x2', '\x2', 
		'\x2', '\x2', 'u', '\x3', '\x2', '\x2', '\x2', '\x2', 'w', '\x3', '\x2', 
		'\x2', '\x2', '\x2', 'y', '\x3', '\x2', '\x2', '\x2', '\x2', '{', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '}', '\x3', '\x2', '\x2', '\x2', '\x2', '\x7F', 
		'\x3', '\x2', '\x2', '\x2', '\x2', '\x81', '\x3', '\x2', '\x2', '\x2', 
		'\x2', '\x83', '\x3', '\x2', '\x2', '\x2', '\x2', '\x85', '\x3', '\x2', 
		'\x2', '\x2', '\x2', '\x87', '\x3', '\x2', '\x2', '\x2', '\x2', '\x89', 
		'\x3', '\x2', '\x2', '\x2', '\x2', '\x8B', '\x3', '\x2', '\x2', '\x2', 
		'\x2', '\x8D', '\x3', '\x2', '\x2', '\x2', '\x2', '\x8F', '\x3', '\x2', 
		'\x2', '\x2', '\x2', '\x91', '\x3', '\x2', '\x2', '\x2', '\x2', '\x93', 
		'\x3', '\x2', '\x2', '\x2', '\x2', '\x95', '\x3', '\x2', '\x2', '\x2', 
		'\x3', '\xA5', '\x3', '\x2', '\x2', '\x2', '\x5', '\xAD', '\x3', '\x2', 
		'\x2', '\x2', '\a', '\xB4', '\x3', '\x2', '\x2', '\x2', '\t', '\xB8', 
		'\x3', '\x2', '\x2', '\x2', '\v', '\xBE', '\x3', '\x2', '\x2', '\x2', 
		'\r', '\xC6', '\x3', '\x2', '\x2', '\x2', '\xF', '\xCD', '\x3', '\x2', 
		'\x2', '\x2', '\x11', '\xD1', '\x3', '\x2', '\x2', '\x2', '\x13', '\xD4', 
		'\x3', '\x2', '\x2', '\x2', '\x15', '\xD9', '\x3', '\x2', '\x2', '\x2', 
		'\x17', '\xDF', '\x3', '\x2', '\x2', '\x2', '\x19', '\xE3', '\x3', '\x2', 
		'\x2', '\x2', '\x1B', '\xE9', '\x3', '\x2', '\x2', '\x2', '\x1D', '\xF2', 
		'\x3', '\x2', '\x2', '\x2', '\x1F', '\xF9', '\x3', '\x2', '\x2', '\x2', 
		'!', '\xFE', '\x3', '\x2', '\x2', '\x2', '#', '\x103', '\x3', '\x2', '\x2', 
		'\x2', '%', '\x109', '\x3', '\x2', '\x2', '\x2', '\'', '\x10E', '\x3', 
		'\x2', '\x2', '\x2', ')', '\x116', '\x3', '\x2', '\x2', '\x2', '+', '\x11D', 
		'\x3', '\x2', '\x2', '\x2', '-', '\x123', '\x3', '\x2', '\x2', '\x2', 
		'/', '\x127', '\x3', '\x2', '\x2', '\x2', '\x31', '\x12C', '\x3', '\x2', 
		'\x2', '\x2', '\x33', '\x131', '\x3', '\x2', '\x2', '\x2', '\x35', '\x137', 
		'\x3', '\x2', '\x2', '\x2', '\x37', '\x13D', '\x3', '\x2', '\x2', '\x2', 
		'\x39', '\x144', '\x3', '\x2', '\x2', '\x2', ';', '\x14B', '\x3', '\x2', 
		'\x2', '\x2', '=', '\x152', '\x3', '\x2', '\x2', '\x2', '?', '\x157', 
		'\x3', '\x2', '\x2', '\x2', '\x41', '\x159', '\x3', '\x2', '\x2', '\x2', 
		'\x43', '\x15D', '\x3', '\x2', '\x2', '\x2', '\x45', '\x16C', '\x3', '\x2', 
		'\x2', '\x2', 'G', '\x16E', '\x3', '\x2', '\x2', '\x2', 'I', '\x177', 
		'\x3', '\x2', '\x2', '\x2', 'K', '\x17C', '\x3', '\x2', '\x2', '\x2', 
		'M', '\x17F', '\x3', '\x2', '\x2', '\x2', 'O', '\x181', '\x3', '\x2', 
		'\x2', '\x2', 'Q', '\x183', '\x3', '\x2', '\x2', '\x2', 'S', '\x185', 
		'\x3', '\x2', '\x2', '\x2', 'U', '\x187', '\x3', '\x2', '\x2', '\x2', 
		'W', '\x189', '\x3', '\x2', '\x2', '\x2', 'Y', '\x18B', '\x3', '\x2', 
		'\x2', '\x2', '[', '\x18D', '\x3', '\x2', '\x2', '\x2', ']', '\x18F', 
		'\x3', '\x2', '\x2', '\x2', '_', '\x191', '\x3', '\x2', '\x2', '\x2', 
		'\x61', '\x193', '\x3', '\x2', '\x2', '\x2', '\x63', '\x196', '\x3', '\x2', 
		'\x2', '\x2', '\x65', '\x199', '\x3', '\x2', '\x2', '\x2', 'g', '\x19C', 
		'\x3', '\x2', '\x2', '\x2', 'i', '\x19F', '\x3', '\x2', '\x2', '\x2', 
		'k', '\x1A2', '\x3', '\x2', '\x2', '\x2', 'm', '\x1A4', '\x3', '\x2', 
		'\x2', '\x2', 'o', '\x1A6', '\x3', '\x2', '\x2', '\x2', 'q', '\x1A8', 
		'\x3', '\x2', '\x2', '\x2', 's', '\x1AA', '\x3', '\x2', '\x2', '\x2', 
		'u', '\x1AC', '\x3', '\x2', '\x2', '\x2', 'w', '\x1AF', '\x3', '\x2', 
		'\x2', '\x2', 'y', '\x1B2', '\x3', '\x2', '\x2', '\x2', '{', '\x1B5', 
		'\x3', '\x2', '\x2', '\x2', '}', '\x1B8', '\x3', '\x2', '\x2', '\x2', 
		'\x7F', '\x1BB', '\x3', '\x2', '\x2', '\x2', '\x81', '\x1BE', '\x3', '\x2', 
		'\x2', '\x2', '\x83', '\x1C1', '\x3', '\x2', '\x2', '\x2', '\x85', '\x1C4', 
		'\x3', '\x2', '\x2', '\x2', '\x87', '\x1C6', '\x3', '\x2', '\x2', '\x2', 
		'\x89', '\x1C8', '\x3', '\x2', '\x2', '\x2', '\x8B', '\x1CA', '\x3', '\x2', 
		'\x2', '\x2', '\x8D', '\x1CC', '\x3', '\x2', '\x2', '\x2', '\x8F', '\x1CE', 
		'\x3', '\x2', '\x2', '\x2', '\x91', '\x1D2', '\x3', '\x2', '\x2', '\x2', 
		'\x93', '\x1E0', '\x3', '\x2', '\x2', '\x2', '\x95', '\x1EB', '\x3', '\x2', 
		'\x2', '\x2', '\x97', '\x1F4', '\x3', '\x2', '\x2', '\x2', '\x99', '\x1F8', 
		'\x3', '\x2', '\x2', '\x2', '\x9B', '\x1FA', '\x3', '\x2', '\x2', '\x2', 
		'\x9D', '\x1FD', '\x3', '\x2', '\x2', '\x2', '\x9F', '\x202', '\x3', '\x2', 
		'\x2', '\x2', '\xA1', '\x206', '\x3', '\x2', '\x2', '\x2', '\xA3', '\x208', 
		'\x3', '\x2', '\x2', '\x2', '\xA5', '\xA6', '\a', '\x66', '\x2', '\x2', 
		'\xA6', '\xA7', '\a', 'g', '\x2', '\x2', '\xA7', '\xA8', '\a', 'h', '\x2', 
		'\x2', '\xA8', '\xA9', '\a', '\x63', '\x2', '\x2', '\xA9', '\xAA', '\a', 
		'w', '\x2', '\x2', '\xAA', '\xAB', '\a', 'n', '\x2', '\x2', '\xAB', '\xAC', 
		'\a', 'v', '\x2', '\x2', '\xAC', '\x4', '\x3', '\x2', '\x2', '\x2', '\xAD', 
		'\xAE', '\a', 'v', '\x2', '\x2', '\xAE', '\xAF', '\a', '{', '\x2', '\x2', 
		'\xAF', '\xB0', '\a', 'r', '\x2', '\x2', '\xB0', '\xB1', '\a', 'g', '\x2', 
		'\x2', '\xB1', '\xB2', '\a', 'q', '\x2', '\x2', '\xB2', '\xB3', '\a', 
		'h', '\x2', '\x2', '\xB3', '\x6', '\x3', '\x2', '\x2', '\x2', '\xB4', 
		'\xB5', '\a', 'p', '\x2', '\x2', '\xB5', '\xB6', '\a', 'g', '\x2', '\x2', 
		'\xB6', '\xB7', '\a', 'y', '\x2', '\x2', '\xB7', '\b', '\x3', '\x2', '\x2', 
		'\x2', '\xB8', '\xB9', '\a', 'g', '\x2', '\x2', '\xB9', '\xBA', '\a', 
		'x', '\x2', '\x2', '\xBA', '\xBB', '\a', 'g', '\x2', '\x2', '\xBB', '\xBC', 
		'\a', 'p', '\x2', '\x2', '\xBC', '\xBD', '\a', 'v', '\x2', '\x2', '\xBD', 
		'\n', '\x3', '\x2', '\x2', '\x2', '\xBE', '\xBF', '\a', 'r', '\x2', '\x2', 
		'\xBF', '\xC0', '\a', 't', '\x2', '\x2', '\xC0', '\xC1', '\a', 'k', '\x2', 
		'\x2', '\xC1', '\xC2', '\a', 'x', '\x2', '\x2', '\xC2', '\xC3', '\a', 
		'\x63', '\x2', '\x2', '\xC3', '\xC4', '\a', 'v', '\x2', '\x2', '\xC4', 
		'\xC5', '\a', 'g', '\x2', '\x2', '\xC5', '\f', '\x3', '\x2', '\x2', '\x2', 
		'\xC6', '\xC7', '\a', 'r', '\x2', '\x2', '\xC7', '\xC8', '\a', 'w', '\x2', 
		'\x2', '\xC8', '\xC9', '\a', '\x64', '\x2', '\x2', '\xC9', '\xCA', '\a', 
		'n', '\x2', '\x2', '\xCA', '\xCB', '\a', 'k', '\x2', '\x2', '\xCB', '\xCC', 
		'\a', '\x65', '\x2', '\x2', '\xCC', '\xE', '\x3', '\x2', '\x2', '\x2', 
		'\xCD', '\xCE', '\a', 'q', '\x2', '\x2', '\xCE', '\xCF', '\a', 'w', '\x2', 
		'\x2', '\xCF', '\xD0', '\a', 'v', '\x2', '\x2', '\xD0', '\x10', '\x3', 
		'\x2', '\x2', '\x2', '\xD1', '\xD2', '\a', 'k', '\x2', '\x2', '\xD2', 
		'\xD3', '\a', 'h', '\x2', '\x2', '\xD3', '\x12', '\x3', '\x2', '\x2', 
		'\x2', '\xD4', '\xD5', '\a', 'g', '\x2', '\x2', '\xD5', '\xD6', '\a', 
		'n', '\x2', '\x2', '\xD6', '\xD7', '\a', 'u', '\x2', '\x2', '\xD7', '\xD8', 
		'\a', 'g', '\x2', '\x2', '\xD8', '\x14', '\x3', '\x2', '\x2', '\x2', '\xD9', 
		'\xDA', '\a', 'y', '\x2', '\x2', '\xDA', '\xDB', '\a', 'j', '\x2', '\x2', 
		'\xDB', '\xDC', '\a', 'k', '\x2', '\x2', '\xDC', '\xDD', '\a', 'n', '\x2', 
		'\x2', '\xDD', '\xDE', '\a', 'g', '\x2', '\x2', '\xDE', '\x16', '\x3', 
		'\x2', '\x2', '\x2', '\xDF', '\xE0', '\a', 'h', '\x2', '\x2', '\xE0', 
		'\xE1', '\a', 'q', '\x2', '\x2', '\xE1', '\xE2', '\a', 't', '\x2', '\x2', 
		'\xE2', '\x18', '\x3', '\x2', '\x2', '\x2', '\xE3', '\xE4', '\a', '\x64', 
		'\x2', '\x2', '\xE4', '\xE5', '\a', 't', '\x2', '\x2', '\xE5', '\xE6', 
		'\a', 'g', '\x2', '\x2', '\xE6', '\xE7', '\a', '\x63', '\x2', '\x2', '\xE7', 
		'\xE8', '\a', 'm', '\x2', '\x2', '\xE8', '\x1A', '\x3', '\x2', '\x2', 
		'\x2', '\xE9', '\xEA', '\a', '\x65', '\x2', '\x2', '\xEA', '\xEB', '\a', 
		'q', '\x2', '\x2', '\xEB', '\xEC', '\a', 'p', '\x2', '\x2', '\xEC', '\xED', 
		'\a', 'v', '\x2', '\x2', '\xED', '\xEE', '\a', 'k', '\x2', '\x2', '\xEE', 
		'\xEF', '\a', 'p', '\x2', '\x2', '\xEF', '\xF0', '\a', 'w', '\x2', '\x2', 
		'\xF0', '\xF1', '\a', 'g', '\x2', '\x2', '\xF1', '\x1C', '\x3', '\x2', 
		'\x2', '\x2', '\xF2', '\xF3', '\a', 't', '\x2', '\x2', '\xF3', '\xF4', 
		'\a', 'g', '\x2', '\x2', '\xF4', '\xF5', '\a', 'v', '\x2', '\x2', '\xF5', 
		'\xF6', '\a', 'w', '\x2', '\x2', '\xF6', '\xF7', '\a', 't', '\x2', '\x2', 
		'\xF7', '\xF8', '\a', 'p', '\x2', '\x2', '\xF8', '\x1E', '\x3', '\x2', 
		'\x2', '\x2', '\xF9', '\xFA', '\a', '\x64', '\x2', '\x2', '\xFA', '\xFB', 
		'\a', 'q', '\x2', '\x2', '\xFB', '\xFC', '\a', 'q', '\x2', '\x2', '\xFC', 
		'\xFD', '\a', 'n', '\x2', '\x2', '\xFD', ' ', '\x3', '\x2', '\x2', '\x2', 
		'\xFE', '\xFF', '\a', '\x64', '\x2', '\x2', '\xFF', '\x100', '\a', '{', 
		'\x2', '\x2', '\x100', '\x101', '\a', 'v', '\x2', '\x2', '\x101', '\x102', 
		'\a', 'g', '\x2', '\x2', '\x102', '\"', '\x3', '\x2', '\x2', '\x2', '\x103', 
		'\x104', '\a', 'u', '\x2', '\x2', '\x104', '\x105', '\a', '\x64', '\x2', 
		'\x2', '\x105', '\x106', '\a', '{', '\x2', '\x2', '\x106', '\x107', '\a', 
		'v', '\x2', '\x2', '\x107', '\x108', '\a', 'g', '\x2', '\x2', '\x108', 
		'$', '\x3', '\x2', '\x2', '\x2', '\x109', '\x10A', '\a', '\x65', '\x2', 
		'\x2', '\x10A', '\x10B', '\a', 'j', '\x2', '\x2', '\x10B', '\x10C', '\a', 
		'\x63', '\x2', '\x2', '\x10C', '\x10D', '\a', 't', '\x2', '\x2', '\x10D', 
		'&', '\x3', '\x2', '\x2', '\x2', '\x10E', '\x10F', '\a', '\x66', '\x2', 
		'\x2', '\x10F', '\x110', '\a', 'g', '\x2', '\x2', '\x110', '\x111', '\a', 
		'\x65', '\x2', '\x2', '\x111', '\x112', '\a', 'k', '\x2', '\x2', '\x112', 
		'\x113', '\a', 'o', '\x2', '\x2', '\x113', '\x114', '\a', '\x63', '\x2', 
		'\x2', '\x114', '\x115', '\a', 'n', '\x2', '\x2', '\x115', '(', '\x3', 
		'\x2', '\x2', '\x2', '\x116', '\x117', '\a', '\x66', '\x2', '\x2', '\x117', 
		'\x118', '\a', 'q', '\x2', '\x2', '\x118', '\x119', '\a', 'w', '\x2', 
		'\x2', '\x119', '\x11A', '\a', '\x64', '\x2', '\x2', '\x11A', '\x11B', 
		'\a', 'n', '\x2', '\x2', '\x11B', '\x11C', '\a', 'g', '\x2', '\x2', '\x11C', 
		'*', '\x3', '\x2', '\x2', '\x2', '\x11D', '\x11E', '\a', 'h', '\x2', '\x2', 
		'\x11E', '\x11F', '\a', 'n', '\x2', '\x2', '\x11F', '\x120', '\a', 'q', 
		'\x2', '\x2', '\x120', '\x121', '\a', '\x63', '\x2', '\x2', '\x121', '\x122', 
		'\a', 'v', '\x2', '\x2', '\x122', ',', '\x3', '\x2', '\x2', '\x2', '\x123', 
		'\x124', '\a', 'k', '\x2', '\x2', '\x124', '\x125', '\a', 'p', '\x2', 
		'\x2', '\x125', '\x126', '\a', 'v', '\x2', '\x2', '\x126', '.', '\x3', 
		'\x2', '\x2', '\x2', '\x127', '\x128', '\a', 'w', '\x2', '\x2', '\x128', 
		'\x129', '\a', 'k', '\x2', '\x2', '\x129', '\x12A', '\a', 'p', '\x2', 
		'\x2', '\x12A', '\x12B', '\a', 'v', '\x2', '\x2', '\x12B', '\x30', '\x3', 
		'\x2', '\x2', '\x2', '\x12C', '\x12D', '\a', 'n', '\x2', '\x2', '\x12D', 
		'\x12E', '\a', 'q', '\x2', '\x2', '\x12E', '\x12F', '\a', 'p', '\x2', 
		'\x2', '\x12F', '\x130', '\a', 'i', '\x2', '\x2', '\x130', '\x32', '\x3', 
		'\x2', '\x2', '\x2', '\x131', '\x132', '\a', 'w', '\x2', '\x2', '\x132', 
		'\x133', '\a', 'n', '\x2', '\x2', '\x133', '\x134', '\a', 'q', '\x2', 
		'\x2', '\x134', '\x135', '\a', 'p', '\x2', '\x2', '\x135', '\x136', '\a', 
		'i', '\x2', '\x2', '\x136', '\x34', '\x3', '\x2', '\x2', '\x2', '\x137', 
		'\x138', '\a', 'u', '\x2', '\x2', '\x138', '\x139', '\a', 'j', '\x2', 
		'\x2', '\x139', '\x13A', '\a', 'q', '\x2', '\x2', '\x13A', '\x13B', '\a', 
		't', '\x2', '\x2', '\x13B', '\x13C', '\a', 'v', '\x2', '\x2', '\x13C', 
		'\x36', '\x3', '\x2', '\x2', '\x2', '\x13D', '\x13E', '\a', 'w', '\x2', 
		'\x2', '\x13E', '\x13F', '\a', 'u', '\x2', '\x2', '\x13F', '\x140', '\a', 
		'j', '\x2', '\x2', '\x140', '\x141', '\a', 'q', '\x2', '\x2', '\x141', 
		'\x142', '\a', 't', '\x2', '\x2', '\x142', '\x143', '\a', 'v', '\x2', 
		'\x2', '\x143', '\x38', '\x3', '\x2', '\x2', '\x2', '\x144', '\x145', 
		'\a', 'q', '\x2', '\x2', '\x145', '\x146', '\a', '\x64', '\x2', '\x2', 
		'\x146', '\x147', '\a', 'l', '\x2', '\x2', '\x147', '\x148', '\a', 'g', 
		'\x2', '\x2', '\x148', '\x149', '\a', '\x65', '\x2', '\x2', '\x149', '\x14A', 
		'\a', 'v', '\x2', '\x2', '\x14A', ':', '\x3', '\x2', '\x2', '\x2', '\x14B', 
		'\x14C', '\a', 'u', '\x2', '\x2', '\x14C', '\x14D', '\a', 'v', '\x2', 
		'\x2', '\x14D', '\x14E', '\a', 't', '\x2', '\x2', '\x14E', '\x14F', '\a', 
		'k', '\x2', '\x2', '\x14F', '\x150', '\a', 'p', '\x2', '\x2', '\x150', 
		'\x151', '\a', 'i', '\x2', '\x2', '\x151', '<', '\x3', '\x2', '\x2', '\x2', 
		'\x152', '\x153', '\a', 'x', '\x2', '\x2', '\x153', '\x154', '\a', 'q', 
		'\x2', '\x2', '\x154', '\x155', '\a', 'k', '\x2', '\x2', '\x155', '\x156', 
		'\a', '\x66', '\x2', '\x2', '\x156', '>', '\x3', '\x2', '\x2', '\x2', 
		'\x157', '\x158', '\x5', '\x9D', 'O', '\x2', '\x158', '@', '\x3', '\x2', 
		'\x2', '\x2', '\x159', '\x15A', '\a', '\x32', '\x2', '\x2', '\x15A', '\x15B', 
		'\t', '\x2', '\x2', '\x2', '\x15B', '\x15C', '\x5', '\x9F', 'P', '\x2', 
		'\x15C', '\x42', '\x3', '\x2', '\x2', '\x2', '\x15D', '\x15E', '\x5', 
		'\x9D', 'O', '\x2', '\x15E', '\x15F', '\a', '\x30', '\x2', '\x2', '\x15F', 
		'\x161', '\x5', '\x9D', 'O', '\x2', '\x160', '\x162', '\x5', '\x9B', 'N', 
		'\x2', '\x161', '\x160', '\x3', '\x2', '\x2', '\x2', '\x161', '\x162', 
		'\x3', '\x2', '\x2', '\x2', '\x162', '\x44', '\x3', '\x2', '\x2', '\x2', 
		'\x163', '\x164', '\a', 'v', '\x2', '\x2', '\x164', '\x165', '\a', 't', 
		'\x2', '\x2', '\x165', '\x166', '\a', 'w', '\x2', '\x2', '\x166', '\x16D', 
		'\a', 'g', '\x2', '\x2', '\x167', '\x168', '\a', 'h', '\x2', '\x2', '\x168', 
		'\x169', '\a', '\x63', '\x2', '\x2', '\x169', '\x16A', '\a', 'n', '\x2', 
		'\x2', '\x16A', '\x16B', '\a', 'u', '\x2', '\x2', '\x16B', '\x16D', '\a', 
		'g', '\x2', '\x2', '\x16C', '\x163', '\x3', '\x2', '\x2', '\x2', '\x16C', 
		'\x167', '\x3', '\x2', '\x2', '\x2', '\x16D', '\x46', '\x3', '\x2', '\x2', 
		'\x2', '\x16E', '\x172', '\a', '$', '\x2', '\x2', '\x16F', '\x171', '\n', 
		'\x3', '\x2', '\x2', '\x170', '\x16F', '\x3', '\x2', '\x2', '\x2', '\x171', 
		'\x174', '\x3', '\x2', '\x2', '\x2', '\x172', '\x170', '\x3', '\x2', '\x2', 
		'\x2', '\x172', '\x173', '\x3', '\x2', '\x2', '\x2', '\x173', '\x175', 
		'\x3', '\x2', '\x2', '\x2', '\x174', '\x172', '\x3', '\x2', '\x2', '\x2', 
		'\x175', '\x176', '\a', '$', '\x2', '\x2', '\x176', 'H', '\x3', '\x2', 
		'\x2', '\x2', '\x177', '\x178', '\a', 'p', '\x2', '\x2', '\x178', '\x179', 
		'\a', 'w', '\x2', '\x2', '\x179', '\x17A', '\a', 'n', '\x2', '\x2', '\x17A', 
		'\x17B', '\a', 'n', '\x2', '\x2', '\x17B', 'J', '\x3', '\x2', '\x2', '\x2', 
		'\x17C', '\x17D', '\a', '<', '\x2', '\x2', '\x17D', '\x17E', '\a', '<', 
		'\x2', '\x2', '\x17E', 'L', '\x3', '\x2', '\x2', '\x2', '\x17F', '\x180', 
		'\a', '\x30', '\x2', '\x2', '\x180', 'N', '\x3', '\x2', '\x2', '\x2', 
		'\x181', '\x182', '\a', '=', '\x2', '\x2', '\x182', 'P', '\x3', '\x2', 
		'\x2', '\x2', '\x183', '\x184', '\a', '.', '\x2', '\x2', '\x184', 'R', 
		'\x3', '\x2', '\x2', '\x2', '\x185', '\x186', '\a', '*', '\x2', '\x2', 
		'\x186', 'T', '\x3', '\x2', '\x2', '\x2', '\x187', '\x188', '\a', '+', 
		'\x2', '\x2', '\x188', 'V', '\x3', '\x2', '\x2', '\x2', '\x189', '\x18A', 
		'\a', '}', '\x2', '\x2', '\x18A', 'X', '\x3', '\x2', '\x2', '\x2', '\x18B', 
		'\x18C', '\a', '\x7F', '\x2', '\x2', '\x18C', 'Z', '\x3', '\x2', '\x2', 
		'\x2', '\x18D', '\x18E', '\a', ']', '\x2', '\x2', '\x18E', '\\', '\x3', 
		'\x2', '\x2', '\x2', '\x18F', '\x190', '\a', '_', '\x2', '\x2', '\x190', 
		'^', '\x3', '\x2', '\x2', '\x2', '\x191', '\x192', '\a', '?', '\x2', '\x2', 
		'\x192', '`', '\x3', '\x2', '\x2', '\x2', '\x193', '\x194', '\a', '-', 
		'\x2', '\x2', '\x194', '\x195', '\a', '?', '\x2', '\x2', '\x195', '\x62', 
		'\x3', '\x2', '\x2', '\x2', '\x196', '\x197', '\a', '/', '\x2', '\x2', 
		'\x197', '\x198', '\a', '?', '\x2', '\x2', '\x198', '\x64', '\x3', '\x2', 
		'\x2', '\x2', '\x199', '\x19A', '\a', ',', '\x2', '\x2', '\x19A', '\x19B', 
		'\a', '?', '\x2', '\x2', '\x19B', '\x66', '\x3', '\x2', '\x2', '\x2', 
		'\x19C', '\x19D', '\a', '\x31', '\x2', '\x2', '\x19D', '\x19E', '\a', 
		'?', '\x2', '\x2', '\x19E', 'h', '\x3', '\x2', '\x2', '\x2', '\x19F', 
		'\x1A0', '\a', '\'', '\x2', '\x2', '\x1A0', '\x1A1', '\a', '?', '\x2', 
		'\x2', '\x1A1', 'j', '\x3', '\x2', '\x2', '\x2', '\x1A2', '\x1A3', '\a', 
		'@', '\x2', '\x2', '\x1A3', 'l', '\x3', '\x2', '\x2', '\x2', '\x1A4', 
		'\x1A5', '\a', '>', '\x2', '\x2', '\x1A5', 'n', '\x3', '\x2', '\x2', '\x2', 
		'\x1A6', '\x1A7', '\a', '#', '\x2', '\x2', '\x1A7', 'p', '\x3', '\x2', 
		'\x2', '\x2', '\x1A8', '\x1A9', '\a', '\x41', '\x2', '\x2', '\x1A9', 'r', 
		'\x3', '\x2', '\x2', '\x2', '\x1AA', '\x1AB', '\a', '<', '\x2', '\x2', 
		'\x1AB', 't', '\x3', '\x2', '\x2', '\x2', '\x1AC', '\x1AD', '\a', '?', 
		'\x2', '\x2', '\x1AD', '\x1AE', '\a', '?', '\x2', '\x2', '\x1AE', 'v', 
		'\x3', '\x2', '\x2', '\x2', '\x1AF', '\x1B0', '\a', '>', '\x2', '\x2', 
		'\x1B0', '\x1B1', '\a', '?', '\x2', '\x2', '\x1B1', 'x', '\x3', '\x2', 
		'\x2', '\x2', '\x1B2', '\x1B3', '\a', '@', '\x2', '\x2', '\x1B3', '\x1B4', 
		'\a', '?', '\x2', '\x2', '\x1B4', 'z', '\x3', '\x2', '\x2', '\x2', '\x1B5', 
		'\x1B6', '\a', '#', '\x2', '\x2', '\x1B6', '\x1B7', '\a', '?', '\x2', 
		'\x2', '\x1B7', '|', '\x3', '\x2', '\x2', '\x2', '\x1B8', '\x1B9', '\a', 
		'(', '\x2', '\x2', '\x1B9', '\x1BA', '\a', '(', '\x2', '\x2', '\x1BA', 
		'~', '\x3', '\x2', '\x2', '\x2', '\x1BB', '\x1BC', '\a', '~', '\x2', '\x2', 
		'\x1BC', '\x1BD', '\a', '~', '\x2', '\x2', '\x1BD', '\x80', '\x3', '\x2', 
		'\x2', '\x2', '\x1BE', '\x1BF', '\a', '-', '\x2', '\x2', '\x1BF', '\x1C0', 
		'\a', '-', '\x2', '\x2', '\x1C0', '\x82', '\x3', '\x2', '\x2', '\x2', 
		'\x1C1', '\x1C2', '\a', '/', '\x2', '\x2', '\x1C2', '\x1C3', '\a', '/', 
		'\x2', '\x2', '\x1C3', '\x84', '\x3', '\x2', '\x2', '\x2', '\x1C4', '\x1C5', 
		'\a', '-', '\x2', '\x2', '\x1C5', '\x86', '\x3', '\x2', '\x2', '\x2', 
		'\x1C6', '\x1C7', '\a', '/', '\x2', '\x2', '\x1C7', '\x88', '\x3', '\x2', 
		'\x2', '\x2', '\x1C8', '\x1C9', '\a', ',', '\x2', '\x2', '\x1C9', '\x8A', 
		'\x3', '\x2', '\x2', '\x2', '\x1CA', '\x1CB', '\a', '\x31', '\x2', '\x2', 
		'\x1CB', '\x8C', '\x3', '\x2', '\x2', '\x2', '\x1CC', '\x1CD', '\a', '\'', 
		'\x2', '\x2', '\x1CD', '\x8E', '\x3', '\x2', '\x2', '\x2', '\x1CE', '\x1CF', 
		'\x5', '\x97', 'L', '\x2', '\x1CF', '\x1D0', '\x3', '\x2', '\x2', '\x2', 
		'\x1D0', '\x1D1', '\b', 'H', '\x2', '\x2', '\x1D1', '\x90', '\x3', '\x2', 
		'\x2', '\x2', '\x1D2', '\x1D3', '\a', '\x31', '\x2', '\x2', '\x1D3', '\x1D4', 
		'\a', ',', '\x2', '\x2', '\x1D4', '\x1D8', '\x3', '\x2', '\x2', '\x2', 
		'\x1D5', '\x1D7', '\v', '\x2', '\x2', '\x2', '\x1D6', '\x1D5', '\x3', 
		'\x2', '\x2', '\x2', '\x1D7', '\x1DA', '\x3', '\x2', '\x2', '\x2', '\x1D8', 
		'\x1D9', '\x3', '\x2', '\x2', '\x2', '\x1D8', '\x1D6', '\x3', '\x2', '\x2', 
		'\x2', '\x1D9', '\x1DB', '\x3', '\x2', '\x2', '\x2', '\x1DA', '\x1D8', 
		'\x3', '\x2', '\x2', '\x2', '\x1DB', '\x1DC', '\a', ',', '\x2', '\x2', 
		'\x1DC', '\x1DD', '\a', '\x31', '\x2', '\x2', '\x1DD', '\x1DE', '\x3', 
		'\x2', '\x2', '\x2', '\x1DE', '\x1DF', '\b', 'I', '\x2', '\x2', '\x1DF', 
		'\x92', '\x3', '\x2', '\x2', '\x2', '\x1E0', '\x1E1', '\a', '\x31', '\x2', 
		'\x2', '\x1E1', '\x1E2', '\a', '\x31', '\x2', '\x2', '\x1E2', '\x1E6', 
		'\x3', '\x2', '\x2', '\x2', '\x1E3', '\x1E5', '\n', '\x4', '\x2', '\x2', 
		'\x1E4', '\x1E3', '\x3', '\x2', '\x2', '\x2', '\x1E5', '\x1E8', '\x3', 
		'\x2', '\x2', '\x2', '\x1E6', '\x1E4', '\x3', '\x2', '\x2', '\x2', '\x1E6', 
		'\x1E7', '\x3', '\x2', '\x2', '\x2', '\x1E7', '\x1E9', '\x3', '\x2', '\x2', 
		'\x2', '\x1E8', '\x1E6', '\x3', '\x2', '\x2', '\x2', '\x1E9', '\x1EA', 
		'\b', 'J', '\x2', '\x2', '\x1EA', '\x94', '\x3', '\x2', '\x2', '\x2', 
		'\x1EB', '\x1F0', '\x5', '\x99', 'M', '\x2', '\x1EC', '\x1EF', '\x5', 
		'\x99', 'M', '\x2', '\x1ED', '\x1EF', '\x5', '\xA1', 'Q', '\x2', '\x1EE', 
		'\x1EC', '\x3', '\x2', '\x2', '\x2', '\x1EE', '\x1ED', '\x3', '\x2', '\x2', 
		'\x2', '\x1EF', '\x1F2', '\x3', '\x2', '\x2', '\x2', '\x1F0', '\x1EE', 
		'\x3', '\x2', '\x2', '\x2', '\x1F0', '\x1F1', '\x3', '\x2', '\x2', '\x2', 
		'\x1F1', '\x96', '\x3', '\x2', '\x2', '\x2', '\x1F2', '\x1F0', '\x3', 
		'\x2', '\x2', '\x2', '\x1F3', '\x1F5', '\t', '\x5', '\x2', '\x2', '\x1F4', 
		'\x1F3', '\x3', '\x2', '\x2', '\x2', '\x1F5', '\x1F6', '\x3', '\x2', '\x2', 
		'\x2', '\x1F6', '\x1F4', '\x3', '\x2', '\x2', '\x2', '\x1F6', '\x1F7', 
		'\x3', '\x2', '\x2', '\x2', '\x1F7', '\x98', '\x3', '\x2', '\x2', '\x2', 
		'\x1F8', '\x1F9', '\t', '\x6', '\x2', '\x2', '\x1F9', '\x9A', '\x3', '\x2', 
		'\x2', '\x2', '\x1FA', '\x1FB', '\t', '\a', '\x2', '\x2', '\x1FB', '\x9C', 
		'\x3', '\x2', '\x2', '\x2', '\x1FC', '\x1FE', '\x5', '\xA1', 'Q', '\x2', 
		'\x1FD', '\x1FC', '\x3', '\x2', '\x2', '\x2', '\x1FE', '\x1FF', '\x3', 
		'\x2', '\x2', '\x2', '\x1FF', '\x1FD', '\x3', '\x2', '\x2', '\x2', '\x1FF', 
		'\x200', '\x3', '\x2', '\x2', '\x2', '\x200', '\x9E', '\x3', '\x2', '\x2', 
		'\x2', '\x201', '\x203', '\x5', '\xA3', 'R', '\x2', '\x202', '\x201', 
		'\x3', '\x2', '\x2', '\x2', '\x203', '\x204', '\x3', '\x2', '\x2', '\x2', 
		'\x204', '\x202', '\x3', '\x2', '\x2', '\x2', '\x204', '\x205', '\x3', 
		'\x2', '\x2', '\x2', '\x205', '\xA0', '\x3', '\x2', '\x2', '\x2', '\x206', 
		'\x207', '\t', '\b', '\x2', '\x2', '\x207', '\xA2', '\x3', '\x2', '\x2', 
		'\x2', '\x208', '\x209', '\t', '\t', '\x2', '\x2', '\x209', '\xA4', '\x3', 
		'\x2', '\x2', '\x2', '\r', '\x2', '\x161', '\x16C', '\x172', '\x1D8', 
		'\x1E6', '\x1EE', '\x1F0', '\x1F6', '\x1FF', '\x204', '\x3', '\x2', '\x3', 
		'\x2',
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
