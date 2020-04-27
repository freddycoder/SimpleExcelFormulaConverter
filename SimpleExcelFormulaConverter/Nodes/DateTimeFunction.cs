using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleExcelFormulaConverter.Nodes
{
    public class DateTimeFunction : AST
    {
        public DateTimeFunction(Token token) : base(token)
        {
        }

        public DateTimeFunction(AST leftNode, Token token, AST rightNode) : base(leftNode, token, rightNode)
        {
        }

        public override string ToString()
        {
            return "Date";
        }
    }
}
