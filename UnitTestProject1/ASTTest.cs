using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using SimpleExcelFormulaConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            tree.Count().ShouldBe(7);
            tree.ElementAt(0).Token.ShouldBe(Token("string.Concat", TokenType.StringConcat));
            tree.ElementAt(1).Token.ShouldBe(Token("ToString", TokenType.Function));
            tree.ElementAt(2).Token.ShouldBe(Token("-", TokenType.Operator));
            tree.ElementAt(3).Token.ShouldBe(Token("DateTime.Now", TokenType.Function));
            tree.ElementAt(4).Token.ShouldBe(Token("3", TokenType.Number));
            tree.ElementAt(5).Token.TokenType.ShouldBe(TokenType.String);
            tree.ElementAt(6).Token.ShouldBe(Token("120000+0000", TokenType.String));
        }
    }
}
