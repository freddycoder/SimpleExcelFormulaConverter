using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleExcelFormulaConverter
{
    public abstract class AST : IEnumerable<AST>
    {
        public List<AST> ChildNodes { get; }
        public Token Token { get; }
        public TokenType TokenType => Token.TokenType;
        public string Value => Token.Value;

        protected AST(Token token)
        {
            Token = token;
            ChildNodes = new List<AST>();
        }

        protected AST(AST leftNode, Token token, AST rightNode)
        {
            Token = token;
            ChildNodes = new List<AST>();
            ChildNodes.Add(leftNode);
            ChildNodes.Add(rightNode);
        }

        protected AST(AST ast, Token token)
        {
            Token = token;
            ChildNodes = new List<AST>();
            ChildNodes.Add(ast);
        }

        public IEnumerator<AST> GetEnumerator()
        {
            yield return this;
            foreach (var childNode in ChildNodes)
            {
                foreach (var child in childNode)
                {
                    yield return child;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
