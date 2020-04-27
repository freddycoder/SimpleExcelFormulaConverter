using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleExcelFormulaConverter
{
    public struct Token
    {
        public TokenType TokenType;
        public string Value;

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
        Operator
    }

    public class FunctionDictionary : Dictionary<string, string>
    {
        public FunctionDictionary() : base(StringComparer.InvariantCultureIgnoreCase)
        {
            Add("Aujourdhui",  "DateTime.Now");
            Add("Today",       "DateTime.Now");
            Add("Texte",       "ToString");
            Add("Text",        "ToString");
            Add("CONCATENATE", "string.Concat");
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
