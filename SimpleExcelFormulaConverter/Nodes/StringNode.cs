using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleExcelFormulaConverter.Nodes
{
    public class StringNode : AST
    {
        public StringNode(Token token) : base(token)
        {
        }

        public StringNode(AST leftNode, Token token, AST rightNode) : base(leftNode, token, rightNode)
        {
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
