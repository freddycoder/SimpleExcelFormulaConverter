using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleExcelFormulaConverter.Nodes
{
    public class DateTimeExpression : AST
    {
        public DateTimeExpression(AST leftChild, Token token, AST rightChild) : base(leftChild, token, rightChild)
        {
        }

        public override string ToString()
        {
            var eval = (ChildNodes[1] as IEvaluable).Eval();

            var value = eval > 0 ? $"+{eval}" : eval.ToString();

            return $"{ChildNodes[0]}{value}j";
        }
    }
}
