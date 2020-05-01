using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleExcelFormulaConverter.Nodes
{
    public class AddMonths : AST
    {
        public AddMonths(Token token) : base(token)
        {
        }

        public override string ToString()
        {
            var monthsAdded = ChildNodes[1];

            if (int.TryParse(monthsAdded.ToString(), out var result))
            {
                if (result >= 0)
                {
                    return $"{ChildNodes[0]}+{result}mois";
                }
            }

            return $"{ChildNodes[0]}{ChildNodes[1]}mois";
        }
    }
}
