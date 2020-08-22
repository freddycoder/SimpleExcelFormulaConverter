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
        private readonly Parseur _parseur;

        public FormulaConverterTIPS()
        {
            _parseur = new Parseur();
        }

        public string Convert(string formula)
        {
            var tree = _parseur.Parse(formula);

            if (tree == null) return "";

            return GererEquationDate(tree.ToString());
        }

        private string GererEquationDate(string v)
        {
            while (v.Contains("{!Date+") && v.Contains("%"))
            {
                v = MoveText(v, "{!Date+");
            }
            while (v.Contains("{!Date-") && v.Contains("%"))
            {
                v = MoveText(v, "{!Date-");
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
                    if (i < iCible + cible.Length - 1) // On récupère le début de la formule
                    {
                        sb.Append(v[i]);
                    }
                    else if (i >= iCible + cible.Length - 1 && v[i] != '!' && v[i] != '%' && equationPartIsParsed == false) // On récupère l'equation
                    {
                        equationPart.Append(v[i]);
                    }
                    else if (i >= iCible + cible.Length - 1 && v[i] != '!' && formatPartIsParse == false) // On récupère le format
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
                    else // 
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
                case 'A':
                    return 'y';
                case 'a':
                    return 'y';
                case 'm':
                    return 'M';
                case 'J':
                    return 'd';
                case 'j':
                    return 'd';
                default:
                    return v;
            }
        }
    }
}