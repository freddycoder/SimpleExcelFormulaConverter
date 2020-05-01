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

    public static class Functions
    {
        public static Dictionary<string, string> Dictionnary;

        static Functions()
        {
            Dictionnary = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            Dictionnary.Add("Aujourdhui",  "DateTime.Now");
            Dictionnary.Add("Today",       "DateTime.Now");
            Dictionnary.Add("Texte",       "ToString");
            Dictionnary.Add("Text",        "ToString");
            Dictionnary.Add("CONCATENATE", "string.Concat");
            Dictionnary.Add("EDATE",       "AddMonths");
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
