using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleExcelFormulaConverter.Nodes
{
    public class OperatorNode : AST
    {
        public OperatorNode(Token token) : base(token)
        {
        }

        public OperatorNode(AST ast, Token token) : base(ast, token)
        {
        }

        public OperatorNode(AST leftNode, Token token, AST rightNode) : base(leftNode, token, rightNode)
        {
        }

        public override string ToString()
        {
            if (ChildNodes.Count == 1)
            {
                return $"{Value}{ChildNodes[0]}";
            }

            var result = $"{ChildNodes[0]}{Value}{ChildNodes[1]}";

            if (ChildNodes[0] is DateTimeFunction)
            {
                result += "j";
            }

            return result;
        }
    }
}
