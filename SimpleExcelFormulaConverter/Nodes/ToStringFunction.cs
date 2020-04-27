using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleExcelFormulaConverter.Nodes
{
    public class ToStringFunction : AST
    {
        public ToStringFunction(Token token) : base(token)
        {
        }

        public ToStringFunction(AST leftNode, Token token, AST rightNode) : base(leftNode, token, rightNode)
        {
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("{!");

            for (int i = 0; i < ChildNodes.Count; i++)
            {
                if (i == 1 && ChildNodes[0] is DateTimeFunction)
                {
                    sb.Append(SwitchCharDateTimeFormat(ChildNodes[i].ToString()));
                }
                else
                {
                    sb.Append(ChildNodes[i].ToString());
                }

                if (ChildNodes.Count > 1)
                {
                    sb.Append("%");
                }
            }

            sb.Append("!}");

            return sb.ToString();
        }

        private string SwitchCharDateTimeFormat(string v)
        {
            var sb = new StringBuilder();

            foreach (var c in v)
            {
                switch (c)
                {
                    case 'a':
                        sb.Append('y');
                        break;
                    case 'j':
                        sb.Append('d');
                        break;
                    case 'm':
                        sb.Append('M');
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }

            return sb.ToString();
        }
    }
}
