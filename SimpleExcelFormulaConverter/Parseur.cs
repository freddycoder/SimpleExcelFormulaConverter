using SimpleExcelFormulaConverter.Nodes;

namespace SimpleExcelFormulaConverter
{
    public class Parseur
    {
        private Lexer _lexer;
        private Token CurrentToken { get; set; }

        public AST Parse(string formule)
        {
            _lexer = new Lexer(formule);
            CurrentToken = _lexer.GetNextToken();

            if (CurrentToken.TokenType != TokenType.EndOfText)
            {
                var tree = Group10();

                if (CurrentToken.TokenType != TokenType.EndOfText)
                {
                    ThrowUnexpectedTokenTypeException();
                }

                return tree;
            }

            return null;
        }

        private AST Group10()
        {
            var ast = Group6();

            if (ast == null)
            {
                ThrowUnexpectedTokenTypeException();
            }

            while (CurrentToken.TokenType == TokenType.StringConcat)
            {
                var token = CurrentToken;

                CurrentToken = _lexer.GetNextToken();

                ast = new StringConcatFunction(ast, token, Group6());
            }

            return ast;
        }

        private AST Group6()
        {
            var ast = Group5();

            if (CurrentToken.TokenType == TokenType.Operator)
            {
                switch (CurrentToken.Value)
                {
                    case "+":
                        var token = CurrentToken;
                        CurrentToken = _lexer.GetNextToken();
                        ast = new OperatorNode(ast, token, Group6());
                        break;
                    case "-":
                        token = CurrentToken;
                        CurrentToken = _lexer.GetNextToken();
                        ast = new OperatorNode(ast, token, Group6());
                        break;
                    default:
                        ThrowUnexpectedTokenTypeException();
                        break;
                }
            }

            return ast;
        }

        private AST Group5()
        {
            var ast = Group4();

            return ast;
        }

        private AST Group4()
        {
            var ast = Group3();

            return ast;
        }

        private AST Group3()
        {
            var ast = Group2();

            if (ast == null)
            {
                if (CurrentToken.TokenType == TokenType.Operator &&
                    (CurrentToken.Value == "-" || CurrentToken.Value == "+"))
                {
                    var token = CurrentToken;
                    CurrentToken = _lexer.GetNextToken();
                    ast = new OperatorNode(Group3(), token);
                }
            }
            else if (ast is DateTimeFunction && CurrentToken.TokenType == TokenType.Operator &&
                     (CurrentToken.Value == "-" || CurrentToken.Value == "+")) 
            {
                ast = new DateTimeExpression(ast, Token.Expression, Group6());
            }

            return ast;
        }

        private AST Group2()
        {
            var ast = Group1();

            if (CurrentToken.TokenType == TokenType.Function)
            {
                if (ast != null) ThrowUnexpectedTokenTypeException();

                switch (CurrentToken.Value)
                {
                    case "ToString":
                        ast = new ToStringFunction(CurrentToken);
                        break;
                    case "DateTime.Now":
                        ast = new DateTimeFunction(CurrentToken);
                        break;
                    case "string.Concat":
                        ast = new StringConcatFunction(CurrentToken);
                        break;
                    case "AddMonths":
                        ast = new AddMonths(CurrentToken);
                        break;
                    default:
                        ThrowUnexpectedTokenTypeException();
                        break;
                }

                CurrentToken = _lexer.GetNextToken();

                if (CurrentToken.TokenType != TokenType.OpenParenthesis) ThrowUnexpectedTokenTypeException();

                CurrentToken = _lexer.GetNextToken();

                while (CurrentToken.TokenType != TokenType.CloseParenthesis)
                {
                    ast.ChildNodes.Add(Group10());

                    if (CurrentToken.TokenType == TokenType.Separator)
                    {
                        CurrentToken = _lexer.GetNextToken();
                    }
                }

                CurrentToken = _lexer.GetNextToken();
            }

            return ast;
        }

        private AST Group1()
        {
            var ast = Group0();

            

            return ast;
        }

        private AST Group0()
        {
            switch (CurrentToken.TokenType)
            {
                case TokenType.Number:
                    var token = CurrentToken;

                    CurrentToken = _lexer.GetNextToken();

                    return new NumberNode(token);
                case TokenType.String:
                    token = CurrentToken;

                    CurrentToken = _lexer.GetNextToken();

                    return new StringNode(token);
                default:
                    return null;
            }
        }

        private void ThrowUnexpectedTokenTypeException()
        {
            throw new UnexpectedTokenTypeException($"Unexcepted TokenType {CurrentToken.TokenType} at position {_lexer.Position} of the formula {_lexer.Text}");
        }
    }
}
