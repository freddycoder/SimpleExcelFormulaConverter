using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using SimpleExcelFormulaConverter;
using static SimpleExcelFormulaConverter.TokenBuilder;

namespace UnitTestProject1
{
    [TestClass]
    public class LexerTest
    {
        [TestMethod]
        public void BuildLexer()
        {
            var lexer = new Lexer("10");

            lexer.ShouldNotBeNull();
        }

        [TestMethod]
        public void BuildAccesFirstChar()
        {
            var lexer = new Lexer("10");

            lexer.CurrentChar.ShouldBe('1');
        }

        [TestMethod]
        public void GetFirstToken()
        {
            var lexer = new Lexer("10");

            var token = lexer.GetNextToken();

            token.ShouldBeOfType<Token>();

            token.TokenType.ShouldBe(TokenType.Number);

            token.Value.ShouldBe("10");
        }

        [TestMethod]
        public void GetFirstTokenButThereIsSpace()
        {
            var lexer = new Lexer("     10");

            var token = lexer.GetNextToken();

            token.ShouldBeOfType<Token>();

            token.TokenType.ShouldBe(TokenType.Number);

            token.Value.ShouldBe("10");
        }

        [TestMethod]
        public void GetFirstTokenButThereIsSpace2()
        {
            var lexer = new Lexer("     -1987        ");

            var token = lexer.GetNextToken();

            token.ShouldBeOfType<Token>();

            token.ShouldBe(Token("-", TokenType.Operator));

            token = lexer.GetNextToken();

            token.ShouldBe(Token("1987", TokenType.Number));
        }

        [TestMethod]
        public void WeReachTheEnd()
        {
            var lexer = new Lexer("");

            var token = lexer.GetNextToken();

            token.ShouldBeOfType<Token>();

            token.TokenType.ShouldBe(TokenType.EndOfText);
            token.Value.ShouldBe("");
        }

        [TestMethod]
        public void WeReachTheEnd2()
        {
            var lexer = new Lexer("    ");

            var token = lexer.GetNextToken();

            token.ShouldBeOfType<Token>();

            token.TokenType.ShouldBe(TokenType.EndOfText);
            token.Value.ShouldBe("");
        }

        [TestMethod]
        public void WeReachTheEnd3()
        {
            var lexer = new Lexer("  10  ");

            lexer.GetNextToken();

            var token = lexer.GetNextToken();

            token.Value.ShouldBe("");
            token.TokenType.ShouldBe(TokenType.EndOfText);
        }

        [TestMethod]
        public void GetAString()
        {
            var lexer = new Lexer(" \"Hello\"");

            var token = lexer.GetNextToken();

            token.Value.ShouldBe("Hello");
            token.TokenType.ShouldBe(TokenType.String);
        }

        [TestMethod]
        public void TodayFunction()
        {
            var lexer = new Lexer("Today()");

            var token = lexer.GetNextToken();

            token.Value.ShouldBe("DateTime.Now");
            token.TokenType.ShouldBe(TokenType.Function);

            token = lexer.GetNextToken();

            token.Value.ShouldBe("(");
            token.TokenType.ShouldBe(TokenType.OpenParenthesis);

            token = lexer.GetNextToken();

            token.Value.ShouldBe(")");
            token.TokenType.ShouldBe(TokenType.CloseParenthesis);

            token = lexer.GetNextToken();

            token.Value.ShouldBe("");
            token.TokenType.ShouldBe(TokenType.EndOfText);
        }

        [TestMethod]
        public void AujourdhuiFunction()
        {
            var lexer = new Lexer("Aujourdhui()");

            var token = lexer.GetNextToken();

            token.Value.ShouldBe("DateTime.Now");
            token.TokenType.ShouldBe(TokenType.Function);

            token = lexer.GetNextToken();

            token.Value.ShouldBe("(");
            token.TokenType.ShouldBe(TokenType.OpenParenthesis);

            token = lexer.GetNextToken();

            token.Value.ShouldBe(")");
            token.TokenType.ShouldBe(TokenType.CloseParenthesis);

            token = lexer.GetNextToken();

            token.Value.ShouldBe("");
            token.TokenType.ShouldBe(TokenType.EndOfText);
        }

        [TestMethod]
        public void TodayFunction2()
        {
            var lexer = new Lexer(" TODAY   ()");

            var token = lexer.GetNextToken();

            token.Value.ShouldBe("DateTime.Now");
            token.TokenType.ShouldBe(TokenType.Function);

            token = lexer.GetNextToken();

            token.Value.ShouldBe("(");
            token.TokenType.ShouldBe(TokenType.OpenParenthesis);

            token = lexer.GetNextToken();

            token.Value.ShouldBe(")");
            token.TokenType.ShouldBe(TokenType.CloseParenthesis);

            token = lexer.GetNextToken();

            token.Value.ShouldBe("");
            token.TokenType.ShouldBe(TokenType.EndOfText);
        }

        [TestMethod]
        public void AujourdhuiFunction2()
        {
            var lexer = new Lexer(" AUJOURDHUI ()    ");

            var token = lexer.GetNextToken();

            token.Value.ShouldBe("DateTime.Now");
            token.TokenType.ShouldBe(TokenType.Function);

            token = lexer.GetNextToken();

            token.Value.ShouldBe("(");
            token.TokenType.ShouldBe(TokenType.OpenParenthesis);

            token = lexer.GetNextToken();

            token.Value.ShouldBe(")");
            token.TokenType.ShouldBe(TokenType.CloseParenthesis);

            token = lexer.GetNextToken();

            token.Value.ShouldBe("");
            token.TokenType.ShouldBe(TokenType.EndOfText);
        }

        [TestMethod]
        public void TextFunction()
        {
            var lexer = new Lexer("Text()");

            var token = lexer.GetNextToken();

            token.Value.ShouldBe("ToString");
            token.TokenType.ShouldBe(TokenType.Function);

            token = lexer.GetNextToken();

            token.Value.ShouldBe("(");
            token.TokenType.ShouldBe(TokenType.OpenParenthesis);

            token = lexer.GetNextToken();

            token.Value.ShouldBe(")");
            token.TokenType.ShouldBe(TokenType.CloseParenthesis);

            token = lexer.GetNextToken();

            token.Value.ShouldBe("");
            token.TokenType.ShouldBe(TokenType.EndOfText);
        }

        [TestMethod]
        public void TexteFunction()
        {
            var lexer = new Lexer("Texte()");

            var token = lexer.GetNextToken();

            token.Value.ShouldBe("ToString");
            token.TokenType.ShouldBe(TokenType.Function);

            token = lexer.GetNextToken();

            token.Value.ShouldBe("(");
            token.TokenType.ShouldBe(TokenType.OpenParenthesis);

            token = lexer.GetNextToken();

            token.Value.ShouldBe(")");
            token.TokenType.ShouldBe(TokenType.CloseParenthesis);

            token = lexer.GetNextToken();

            token.Value.ShouldBe("");
            token.TokenType.ShouldBe(TokenType.EndOfText);
        }

        [TestMethod]
        public void TextFunction2()
        {
            var lexer = new Lexer(" TEXT   ()");

            var token = lexer.GetNextToken();

            token.Value.ShouldBe("ToString");
            token.TokenType.ShouldBe(TokenType.Function);

            token = lexer.GetNextToken();

            token.Value.ShouldBe("(");
            token.TokenType.ShouldBe(TokenType.OpenParenthesis);

            token = lexer.GetNextToken();

            token.Value.ShouldBe(")");
            token.TokenType.ShouldBe(TokenType.CloseParenthesis);

            token = lexer.GetNextToken();

            token.Value.ShouldBe("");
            token.TokenType.ShouldBe(TokenType.EndOfText);
        }

        [TestMethod]
        public void TexteFunction2()
        {
            var lexer = new Lexer(" TEXTE ()    ");

            var token = lexer.GetNextToken();

            token.Value.ShouldBe("ToString");
            token.TokenType.ShouldBe(TokenType.Function);

            token = lexer.GetNextToken();

            token.Value.ShouldBe("(");
            token.TokenType.ShouldBe(TokenType.OpenParenthesis);

            token = lexer.GetNextToken();

            token.Value.ShouldBe(")");
            token.TokenType.ShouldBe(TokenType.CloseParenthesis);

            token = lexer.GetNextToken();

            token.Value.ShouldBe("");
            token.TokenType.ShouldBe(TokenType.EndOfText);
        }

        [TestMethod]
        public void StringConcat()
        {
            var lexer = new Lexer(" &    ");
            var token = lexer.GetNextToken();

            token.Value.ShouldBe("string.Concat");
            token.TokenType.ShouldBe(TokenType.StringConcat);

            token = lexer.GetNextToken();

            token.Value.ShouldBe("");
            token.TokenType.ShouldBe(TokenType.EndOfText);
        }

        [TestMethod]
        public void Separator1()
        {
            var lexer = new Lexer(" ,    ");
            var token = lexer.GetNextToken();

            token.Value.ShouldBe(",");
            token.TokenType.ShouldBe(TokenType.Separator);

            token = lexer.GetNextToken();

            token.Value.ShouldBe("");
            token.TokenType.ShouldBe(TokenType.EndOfText);
        }

        [TestMethod]
        public void Separator2()
        {
            var lexer = new Lexer(" ;    ");
            var token = lexer.GetNextToken();

            token.Value.ShouldBe(";");
            token.TokenType.ShouldBe(TokenType.Separator);

            token = lexer.GetNextToken();

            token.Value.ShouldBe("");
            token.TokenType.ShouldBe(TokenType.EndOfText);
        }

        [TestMethod]
        public void BasicMath()
        {
            var lexer = new Lexer("2 + 2");
            lexer.GetNextToken().ShouldBe(Token("2", TokenType.Number));
            lexer.GetNextToken().ShouldBe(Token("+", TokenType.Operator));
            lexer.GetNextToken().ShouldBe(Token("2", TokenType.Number));
        }

        [TestMethod]
        public void BasicMath2()
        {
            var lexer = new Lexer("2 - 2");
            lexer.GetNextToken().ShouldBe(Token("2", TokenType.Number));
            lexer.GetNextToken().ShouldBe(Token("-", TokenType.Operator));
            lexer.GetNextToken().ShouldBe(Token("2", TokenType.Number));
        }

        [TestMethod]
        public void Concatenate()
        {
            var lexer = new Lexer("CONCATENATE(\"/\", TEXT(TODAY() - 5, \"AAAAMMJJ\"))");

            lexer.GetNextToken().ShouldBe(Token("string.Concat", TokenType.Function));
        }
    }
}
