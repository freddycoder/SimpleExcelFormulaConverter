using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using SimpleExcelFormulaConverter;
using SimpleExcelFormulaConverter.Nodes;
using static SimpleExcelFormulaConverter.TokenBuilder;
using System.Linq;

namespace UnitTestProject1
{
    [TestClass]
    public class ParseurTest
    {
        private readonly Parseur _parseur;

        public ParseurTest()
        {
            _parseur = new Parseur();
        }

        [TestMethod]
        public void BuildParseur()
        {
            _parseur.ShouldNotBeNull();

            var tree = _parseur.Parse("10");

            tree.ShouldBeOfType<NumberNode>();

            tree.Token.ShouldBe(Token("10", TokenType.Number));
        }

        [TestMethod]
        public void ParseStringConcat()
        {
            var tree = _parseur.Parse("\"a\"&\"b\"");

            tree.ChildNodes.Count.ShouldBe(2);

            tree.ChildNodes[0].Token.ShouldBe(Token("a", TokenType.String));
            tree.ChildNodes[1].Token.ShouldBe(Token("b", TokenType.String));
            tree.Token.ShouldBe(Token("string.Concat", TokenType.StringConcat));
        }

        [TestMethod]
        public void ParseFunctionNoArguments()
        {
            var tree = _parseur.Parse("Today()");

            tree.ChildNodes.Count.ShouldBe(0);

            tree.Token.ShouldBe(Token("DateTime.Now", TokenType.Function));
        }

        [TestMethod]
        public void ParseFunctionWithOneArguments()
        {
            var tree = _parseur.Parse("Text(Today())");

            tree.ChildNodes.Count.ShouldBe(1);
            tree.Token.ShouldBe(Token("ToString", TokenType.Function));

            tree.ChildNodes[0].ChildNodes.Count.ShouldBe(0);
            tree.ChildNodes[0].Token.ShouldBe(Token("DateTime.Now", TokenType.Function));
        }

        [TestMethod]
        public void ParseFunctionWithTowArguments()
        {
            var tree = _parseur.Parse("Text(Today(), \"yyyyMMdd\")");

            tree.ChildNodes.Count.ShouldBe(2);
            tree.Token.ShouldBe(Token("ToString", TokenType.Function));

            tree.ChildNodes[0].ChildNodes.Count.ShouldBe(0);
            tree.ChildNodes[0].Token.ShouldBe(Token("DateTime.Now", TokenType.Function));

            tree.ChildNodes[1].ChildNodes.Count.ShouldBe(0);
            tree.ChildNodes[1].Token.ShouldBe(Token("yyyyMMdd", TokenType.String));
        }

        [TestMethod]
        public void ParseOneOfTargetFormula()
        {
            var tree = _parseur.Parse("Text(Today(), \"yyyyMMdd\")&\"120000+0000\"");

            tree.ChildNodes.Count.ShouldBe(2);
            tree.Token.ShouldBe(Token("string.Concat", TokenType.StringConcat));

            tree.ChildNodes[0].Token.ShouldBe(Token("ToString", TokenType.Function));
            tree.ChildNodes[1].Token.ShouldBe(Token("120000+0000", TokenType.String));

            var toString = tree.ChildNodes[0];

            toString.ChildNodes.Count.ShouldBe(2);

            toString.ChildNodes[0].Token.ShouldBe(Token("DateTime.Now", TokenType.Function));
            toString.ChildNodes[1].Token.ShouldBe(Token("yyyyMMdd", TokenType.String));
        }

        [TestMethod]
        public void ParseTodayMinusSomeDays()
        {
            var tree = _parseur.Parse("Today()-3");

            tree.ChildNodes.Count.ShouldBe(2);
            tree.ChildNodes[0].ShouldBeOfType<DateTimeFunction>();
            tree.ChildNodes[1].ShouldBeOfType<OperatorNode>();
            tree.ChildNodes[1].Token.ShouldBe(Token("-", TokenType.Operator));

            tree = tree.ChildNodes[1];

            tree.ChildNodes.Count.ShouldBe(1);
            tree.ChildNodes[0].ShouldBeOfType<NumberNode>();
        }

        [TestMethod]
        public void ParseTodayPlusSomeDays()
        {
            var tree = _parseur.Parse("Today()+3");

            tree.ChildNodes.Count.ShouldBe(2);
            tree.ChildNodes[0].ShouldBeOfType<DateTimeFunction>();
            tree.ChildNodes[1].ShouldBeOfType<OperatorNode>();
            tree.ChildNodes[1].Token.ShouldBe(Token("+", TokenType.Operator));

            tree = tree.ChildNodes[1];

            tree.ChildNodes.Count.ShouldBe(1);
            tree.ChildNodes[0].ShouldBeOfType<NumberNode>();
        }

        [TestMethod]
        public void ParseUnaryMinus()
        {
            var tree = _parseur.Parse("-3");

            tree.ChildNodes.Count.ShouldBe(1);
            tree.Token.ShouldBe(Token("-", TokenType.Operator));
            tree.ChildNodes[0].Token.ShouldBe(Token("3", TokenType.Number));
        }

        [TestMethod]
        public void ParseMoreComplexeDateFormula()
        {
            var tree = _parseur.Parse("TEXT(TODAY()-3,\"aaaammjj\")&\"120000+0000");

            tree.ShouldBeOfType<StringConcatFunction>();
            tree.ChildNodes.Count.ShouldBe(2);

            tree = tree.ChildNodes[0];

            tree.ShouldBeOfType<ToStringFunction>();
            tree.ChildNodes.Count.ShouldBe(2);

            tree = tree.ChildNodes[0];

            tree.ShouldBeOfType<DateTimeExpression>();
            tree.ChildNodes.Count.ShouldBe(2);

            tree.ChildNodes[0].ShouldBeOfType<DateTimeFunction>();
            
            tree.ChildNodes[1].ShouldBeOfType<OperatorNode>();

            tree.ChildNodes[1].ChildNodes.Count.ShouldBe(1);

            tree.ChildNodes[1].ChildNodes[0].ShouldBeOfType<NumberNode>();
        }

        [TestMethod]
        public void ParseEvenMoreComplexeDateFormula()
        {
            var tree = _parseur.Parse("TEXT(TODAY()-3+5,\"aaaammjj\")&\"120000+0000");

            var textFunction = tree.ChildNodes[0];
            textFunction.ShouldBeOfType<ToStringFunction>();

            var dateExpression = textFunction.ChildNodes[0];
            dateExpression.ShouldBeOfType<DateTimeExpression>();

            dateExpression.ToString().ShouldBe("Date+2j");

            textFunction.ToString().ShouldBe("{!Date+2j%aaaammjj%!}");

            tree.ToString().ShouldBe("{!Date+2j%aaaammjj%!}120000+0000");
        }

        [TestMethod]
        public void ParseEvenMoreComplexeDateFormula2()
        {
            var tree = _parseur.Parse("TEXT(TODAY()-30+1,\"aaaammjj\")&\"050000+0000\"");

            var textFunction = tree.ChildNodes[0];
            textFunction.ShouldBeOfType<ToStringFunction>();

            var dateExpression = textFunction.ChildNodes[0];
            dateExpression.ShouldBeOfType<DateTimeExpression>();

            dateExpression.ToString().ShouldBe("Date-29j");

            textFunction.ToString().ShouldBe("{!Date-29j%aaaammjj%!}");

            tree.ToString().ShouldBe("{!Date-29j%aaaammjj%!}050000+0000");
        }

        [TestMethod]
        public void Concatenate()
        {
            var tree = _parseur.Parse("CONCATENATE(\"/\", TEXT(TODAY() - 5, \"AAAAMMJJ\"))");

            tree.Token.ShouldBe(Token("string.Concat", TokenType.Function));

            tree.ShouldBeOfType<StringConcatFunction>();
        }

        [TestMethod]
        public void MoreComplexeCase()
        {
            var tree = _parseur.Parse("TEXT(TODAY()-30,\"aaaammjj\")&\"/\"&TEXT(TODAY(),\"aaaammjj\")");

            tree.ShouldBeOfType<StringConcatFunction>();

            tree.ChildNodes.Count.ShouldBe(2);

            tree.ChildNodes[0].ShouldBeOfType<StringConcatFunction>();
            tree.ChildNodes[1].ShouldBeOfType<ToStringFunction>();

            tree.ChildNodes[0].ChildNodes.Count.ShouldBe(2);
            tree.ChildNodes[0].ChildNodes[0].ShouldBeOfType<ToStringFunction>();
            tree.ChildNodes[0].ChildNodes[1].ShouldBeOfType<StringNode>();
        }

        [TestMethod]
        public void Eval1()
        {
            var tree = _parseur.Parse("1+1") as IEvaluable;

            tree.Eval().ShouldBe(2);
        }

        [TestMethod]
        public void Eval2()
        {
            var tree = _parseur.Parse("-30+1");

            tree.ElementAt(0).Value.ShouldBe("+");
            tree.ElementAt(1).Value.ShouldBe("-");
            tree.ElementAt(2).Value.ShouldBe("30");
            tree.ElementAt(3).Value.ShouldBe("1");

            (tree as IEvaluable).Eval().ShouldBe(-29);
        }

        [TestMethod]
        public void DateEquationToString()
        {
            var tree = _parseur.Parse("TODAY()-30+1");

            tree.ShouldBeOfType<DateTimeExpression>();

            tree.ToString().ShouldBe("Date-29j");
        }

        [TestMethod]
        public void TroisNombre()
        {
            var tree = _parseur.Parse("5 + 4 + 3");

            (tree as IEvaluable).Eval().ShouldBe(12);
        }

        [TestMethod]
        public void InvalidTokenExceptionOnInvalidCloseParentesisNumber()
        {
            Should.Throw<UnexpectedTokenTypeException>(() => _parseur.Parse("TEXT(TODAY()-5+2), \"aaaammjj\")"));
        }
    }
}
