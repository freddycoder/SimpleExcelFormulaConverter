using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleExcelFormulaConverter
{
    public class Lexer
    {
        private readonly IReadOnlyDictionary<string, string> _functionsDictionary;
        private readonly string _text;
        private int _pos;

        public Lexer(string text)
        {
            _functionsDictionary = Functions.Dictionnary;
            _text = text;
            _pos = 0;
        }

        public char CurrentChar => _pos < _text.Length ? _text[_pos] : '\0';

        public int Position => _pos;
        public string Text => _text;

        public Token GetNextToken()
        {
            SkipSpace();

            if (CurrentChar == '\0') return new Token { TokenType = TokenType.EndOfText, Value = "" };

            var sb = new StringBuilder();
            TokenType tokenType = new TokenType();

            if (char.IsDigit(CurrentChar))
            {
                tokenType = TokenType.Number;
                while (char.IsDigit(CurrentChar))
                {
                    sb.Append(CurrentChar);
                    _pos++;
                }
            }
            else if (CurrentChar == '"')
            {
                tokenType = TokenType.String;
                _pos++;

                while (CurrentChar != '"' && CurrentChar != '\0')
                {
                    sb.Append(CurrentChar);
                    _pos++;
                }

                if (CurrentChar == '"') _pos++;
            }
            else if (char.IsLetter(CurrentChar) || CurrentChar == '_')
            {
                tokenType = TokenType.Function;
                while (char.IsLetter(CurrentChar) || CurrentChar == '_')
                {
                    sb.Append(CurrentChar);
                    _pos++;
                }
                var functionName = sb.ToString();
                if (_functionsDictionary.TryGetValue(functionName, out var newName))
                {
                    sb.Clear();
                    sb.Append(newName);
                }
            }
            else if (CurrentChar == '(')
            {
                tokenType = TokenType.OpenParenthesis;
                sb.Append(CurrentChar);
                _pos++;
            }
            else if (CurrentChar == ')')
            {
                tokenType = TokenType.CloseParenthesis;
                sb.Append(CurrentChar);
                _pos++;
            }
            else if (CurrentChar == '&')
            {
                tokenType = TokenType.StringConcat;
                sb.Append("string.Concat");
                _pos++;
            }
            else if (CurrentChar == ',' || CurrentChar == ';')
            {
                tokenType = TokenType.Separator;
                sb.Append(CurrentChar);
                _pos++;
            }
            else if (CurrentChar == '+' || CurrentChar == '-')
            {
                tokenType = TokenType.Operator;
                sb.Append(CurrentChar);
                _pos++;
            }

            return new Token { TokenType = tokenType, Value = sb.ToString() };
        }

        private void SkipSpace()
        {
            while (char.IsWhiteSpace(CurrentChar))
            {
                _pos++;
            }
        }
    }
}
