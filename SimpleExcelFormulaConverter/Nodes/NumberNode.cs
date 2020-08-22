using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleExcelFormulaConverter.Nodes
{
    public class NumberNode : AST, IEvaluable
    {
        public NumberNode(Token token) : base(token)
        {
        }

        public NumberNode(AST leftNode, Token token, AST rightNode) : base(leftNode, token, rightNode)
        {
        }

        public int Eval()
        {
            return int.Parse(Value);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
