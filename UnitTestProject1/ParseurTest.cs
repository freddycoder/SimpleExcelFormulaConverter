using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using SimpleExcelFormulaConverter;
using SimpleExcelFormulaConverter.Nodes;
using static SimpleExcelFormulaConverter.TokenBuilder;

namespace UnitTestProject1
{
    [TestClass]
    public class ParseurTest
    {
        [TestMethod]
        public void BuildParseur()
        {
            var parseur = new Parseur("10");

            parseur.ShouldNotBeNull();

            var tree = parseur.Parse();

            tree.ShouldBeOfType<NumberNode>();

            tree.Token.ShouldBe(Token("10", TokenType.Number));
        }

        [TestMethod]
        public void ParseStringConcat()
        {
            var parseur = new Parseur("\"a\"&\"b\"");

            var tree = parseur.Parse();

            tree.ChildNodes.Count.ShouldBe(2);

            tree.ChildNodes[0].Token.ShouldBe(Token("a", TokenType.String));
            tree.ChildNodes[1].Token.ShouldBe(Token("b", TokenType.String));
            tree.Token.ShouldBe(Token("string.Concat", TokenType.StringConcat));
        }

        [TestMethod]
        public void ParseFunctionNoArguments()
        {
            var parseur = new Parseur("Today()");

            var tree = parseur.Parse();

            tree.ChildNodes.Count.ShouldBe(0);

            tree.Token.ShouldBe(Token("DateTime.Now", TokenType.Function));
        }

        [TestMethod]
        public void ParseFunctionWithOneArguments()
        {
            var parseur = new Parseur("Text(Today())");

            var tree = parseur.Parse();

            tree.ChildNodes.Count.ShouldBe(1);
            tree.Token.ShouldBe(Token("ToString", TokenType.Function));

            tree.ChildNodes[0].ChildNodes.Count.ShouldBe(0);
            tree.ChildNodes[0].Token.ShouldBe(Token("DateTime.Now", TokenType.Function));
        }

        [TestMethod]
        public void ParseFunctionWithTowArguments()
        {
            var parseur = new Parseur("Text(Today(), \"yyyyMMdd\")");

            var tree = parseur.Parse();

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
            var parseur = new Parseur("Text(Today(), \"yyyyMMdd\")&\"120000+0000\"");

            var tree = parseur.Parse();

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
            var parseur = new Parseur("Today()-3");

            var tree = parseur.Parse();

            tree.ChildNodes.Count.ShouldBe(2);
            tree.Token.ShouldBe(Token("-", TokenType.Operator));

            tree.ChildNodes[0].Token.ShouldBe(Token("DateTime.Now", TokenType.Function));
            tree.ChildNodes[1].Token.ShouldBe(Token("3", TokenType.Number));
        }

        [TestMethod]
        public void ParseTodayPlusSomeDays()
        {
            var parseur = new Parseur("Today()+3");

            var tree = parseur.Parse();

            tree.ChildNodes.Count.ShouldBe(2);
            tree.Token.ShouldBe(Token("+", TokenType.Operator));

            tree.ChildNodes[0].Token.ShouldBe(Token("DateTime.Now", TokenType.Function));
            tree.ChildNodes[1].Token.ShouldBe(Token("3", TokenType.Number));
        }

        [TestMethod]
        public void ParseUnaryMinus()
        {
            var parseur = new Parseur("-3");

            var tree = parseur.Parse();

            tree.ChildNodes.Count.ShouldBe(1);
            tree.Token.ShouldBe(Token("-", TokenType.Operator));
            tree.ChildNodes[0].Token.ShouldBe(Token("3", TokenType.Number));
        }

        [TestMethod]
        public void ParseMoreComplexeDateFormula()
        {
            var parseur = new Parseur("TEXT(TODAY()-3,\"aaaammjj\")&\"120000+0000");

            var tree = parseur.Parse();

            tree.ShouldBeOfType<StringConcatFunction>();
            tree.ChildNodes.Count.ShouldBe(2);

            tree = tree.ChildNodes[0];

            tree.ShouldBeOfType<ToStringFunction>();
            tree.ChildNodes.Count.ShouldBe(2);

            tree = tree.ChildNodes[0];

            tree.ShouldBeOfType<OperatorNode>();
            tree.ChildNodes.Count.ShouldBe(2);

            tree.ChildNodes[0].ShouldBeOfType<DateTimeFunction>();
            tree.ChildNodes[1].ShouldBeOfType<NumberNode>();
        }
    }
}
