using System;
using System.Collections.Generic;

namespace SimpleExcelFormulaConverter
{
    public struct Token
    {
        public TokenType TokenType;
        public string Value;

        public static Token Expression { get; } = new Token { TokenType = TokenType.Expression, Value = "Expression" };

        public override string ToString()
        {
            return $"{TokenType}:{Value}";
        }
    }

    public enum TokenType
    {
        Number,
        EndOfText,
        String,
        Function,
        OpenParenthesis,
        CloseParenthesis,
        StringConcat,
        Separator,
        Operator,
        Expression
    }

    public static class Functions
    {
        public static Dictionary<string, string> Dictionary;

        static Functions()
        {
            Dictionary = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "Aujourdhui", "DateTime.Now" },
                { "Today", "DateTime.Now" },
                { "Texte", "ToString" },
                { "Text", "ToString" },
                { "CONCATENATE", "string.Concat" },
                { "EDATE", "AddMonths" }
            };
        }
    }

    public static class TokenBuilder
    {
        public static Token Token(string value, TokenType tokenType)
        {
            return new Token { Value = value, TokenType = tokenType };
        }
    }
}
