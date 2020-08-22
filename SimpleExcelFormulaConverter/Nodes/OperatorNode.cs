using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleExcelFormulaConverter.Nodes
{
    public class OperatorNode : AST, IEvaluable
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

        public int Eval()
        {
            if (ChildNodes.Count == 1) return Value == "-" ? - (ChildNodes[0] as IEvaluable).Eval() : (ChildNodes[0] as IEvaluable).Eval();

            switch (Value)
            {
                case "+":
                    return (ChildNodes[0] as IEvaluable).Eval() + (ChildNodes[1] as IEvaluable).Eval();
                case "-":
                    return (ChildNodes[0] as IEvaluable).Eval() - (ChildNodes[1] as IEvaluable).Eval();
                default:
                    throw new InvalidOperationException($"L'operateur '{Value}' n'est pas supporté");
            }
        }

        public override string ToString()
        {
            return Eval().ToString();
        }
    }
}
