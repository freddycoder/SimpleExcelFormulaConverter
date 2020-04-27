using SimpleExcelFormulaConverter.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleExcelFormulaConverter
{
    public interface IFormulaConverter
    {
        string Convert(string formula);
    }

    public class FormulaConverterTIPS : IFormulaConverter
    {
        public string Convert(string formula)
        {
            var parseur = new Parseur(formula);

            var tree = parseur.Parse();

            return GererEquationDate(tree.ToString());
        }

        private string GererEquationDate(string v)
        {
            while (v.Contains("{!Date+") || v.Contains("{!Date-"))
            {
                v = MoveText(v, "{!Date");
            }

            return v;
        }

        private string MoveText(string v, string cible)
        {
            var iCible = v.IndexOf(cible);

            if (iCible != -1)
            {
                var sb = new StringBuilder();
                var equationPartIsParsed = false;
                var equationPart = new StringBuilder();
                var formatPartIsParse = false;
                var formatPart = new StringBuilder();

                for(int i = 0; i < v.Length; i++)
                {
                    if (i < iCible + cible.Length)
                    {
                        sb.Append(v[i]);
                    }
                    else if (i >= iCible + cible.Length && v[i] != '!' && v[i] != '%' && equationPartIsParsed == false)
                    {
                        equationPart.Append(v[i]);
                    }
                    else if (i >= iCible + cible.Length && v[i] != '!' && formatPartIsParse == false)
                    {
                        equationPartIsParsed = true;
                        formatPart.Append(SwitchDateFormatCaractere(v[i]));

                        if (formatPart.Length > 1 && v[i] == '%')
                        {
                            formatPartIsParse = true;
                            sb.Append(formatPart.ToString());
                            sb.Append(equationPart.ToString());
                        }
                    }
                    else
                    {
                        formatPartIsParse = true;
                        equationPartIsParsed = true;

                        sb.Append(v[i]);
                    }
                }

                return sb.ToString();
            }

            return v;
        }

        private char SwitchDateFormatCaractere(char v)
        {
            switch (v)
            {
                case 'a':
                    return 'y';
                case 'm':
                    return 'M';
                case 'j':
                    return 'd';
                default:
                    return v;
            }
        }
    }
}