using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using SimpleExcelFormulaConverter;
using SimpleExcelFormulaConverter.Nodes;
using System.Linq;
using static SimpleExcelFormulaConverter.TokenBuilder;

namespace UnitTestProject1
{
    [TestClass]
    public class ASTTest
    {
        [TestMethod]
        public void Enumerate()
        {
            var parseur = new Parseur();

            var tree = parseur.Parse("5-6");

            tree.ElementAt(0).Token.ShouldBe(Token("-", TokenType.Operator));
            tree.ElementAt(1).Token.ShouldBe(Token("5", TokenType.Number));
            tree.ElementAt(2).Token.ShouldBe(Token("6", TokenType.Number));
        }

        [TestMethod]
        public void Enumerate2()
        {
            var parseur = new Parseur();

            var tree = parseur.Parse("TEXT(TODAY()-3,\"aaaammjj\")&\"120000+0000");

            tree.Count().ShouldBe(8);
            tree.ElementAt(0).Token.ShouldBe(Token("string.Concat", TokenType.StringConcat));
            tree.ElementAt(1).Token.ShouldBe(Token("ToString", TokenType.Function));
            tree.ElementAt(2).Token.ShouldBe(Token("Expression", TokenType.Expression));
            tree.ElementAt(3).Token.ShouldBe(Token("DateTime.Now", TokenType.Function));
            tree.ElementAt(4).Token.ShouldBe(Token("-", TokenType.Operator));
            tree.ElementAt(5).Token.ShouldBe(Token("3", TokenType.Number));
            tree.ElementAt(6).Token.TokenType.ShouldBe(TokenType.String);
            tree.ElementAt(7).Token.ShouldBe(Token("120000+0000", TokenType.String));
        }

        [TestMethod]
        public void Enumerate3()
        {
            var parseur = new Parseur();

            var tree = parseur.Parse("TEXT(TODAY()-3+5,\"aaaammjj\")&\"120000+0000");

            tree.Count().ShouldBe(10);
            tree.ElementAt(0).ShouldBeOfType<StringConcatFunction>();
            tree.ElementAt(1).ShouldBeOfType<ToStringFunction>();
            tree.ElementAt(2).ShouldBeOfType<DateTimeExpression>();
            tree.ElementAt(3).ShouldBeOfType<DateTimeFunction>();
            tree.ElementAt(4).ShouldBeOfType<OperatorNode>();
            tree.ElementAt(5).ShouldBeOfType<OperatorNode>();
            tree.ElementAt(6).ShouldBeOfType<NumberNode>();
            tree.ElementAt(7).ShouldBeOfType<NumberNode>();
            tree.ElementAt(8).ShouldBeOfType<StringNode>();
            tree.ElementAt(9).ShouldBeOfType<StringNode>();
        }
    }
}
