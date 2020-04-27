using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleExcelFormulaConverter.Nodes
{
    public class StringConcatFunction : AST
    {
        public StringConcatFunction(Token token) : base(token)
        {
        }

        public StringConcatFunction(AST leftNode, Token token, AST rightNode) : base(leftNode, token, rightNode)
        {
        }

        public override string ToString()
        {
            return $"{ChildNodes[0]}{ChildNodes[1]}";
        }
    }
}
