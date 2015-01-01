using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AILab.PropositionalLogic;
using AILab.Common;

namespace TestPLLexer
{
    [TestClass]
    public class TestPLLexer
    {
        [TestMethod]
        public void TestPLLexerFunctions()
        {
            PLLexer PLLexer = new PLLexer("token1 AND token2 => token3");
            Token aToken;

            // get "token1"
            aToken= PLLexer.ReadAndConsume();
            Assert.AreEqual("token1", aToken.TokenName);
            Assert.AreEqual((int)PLTokenTypes.SYMBOL, aToken.TokenType);

            // get "and"
            aToken = PLLexer.ReadAndConsume();
            Assert.AreEqual("and", aToken.TokenName);
            Assert.AreEqual((int)PLTokenTypes.AND, aToken.TokenType);

            // get "token2"
            aToken = PLLexer.ReadAndConsume();
            Assert.AreEqual("token2", aToken.TokenName);
            Assert.AreEqual((int)PLTokenTypes.SYMBOL, aToken.TokenType);

            // get '=>'
            aToken = PLLexer.ReadAndConsume();
            Assert.AreEqual("=>", aToken.TokenName);
            Assert.AreEqual((int)PLTokenTypes.IMPLIES, aToken.TokenType);

            // we are not yet eof
            bool isEOF = PLLexer.IsEOF();
            Assert.IsFalse(isEOF);


            // get "token3"
            aToken = PLLexer.ReadAndConsume();
            Assert.AreEqual("token3", aToken.TokenName);
            Assert.AreEqual((int)PLTokenTypes.SYMBOL, aToken.TokenType);

            // we should now be at eof
            isEOF = PLLexer.IsEOF();
            Assert.IsTrue(isEOF);
        }


        [TestMethod]
        public void TestPLLexerFunctionsNoSpaces()
        {
            PLLexer PLLexer = new PLLexer("token1 AND token2=>token3");
            Token aToken;

            // get "token1"
            aToken = PLLexer.ReadAndConsume();
            Assert.AreEqual("token1", aToken.TokenName);
            Assert.AreEqual((int)PLTokenTypes.SYMBOL, aToken.TokenType);

            // get "and"
            aToken = PLLexer.ReadAndConsume();
            Assert.AreEqual("and", aToken.TokenName);
            Assert.AreEqual((int)PLTokenTypes.AND, aToken.TokenType);

            // get "token2"
            aToken = PLLexer.ReadAndConsume();
            Assert.AreEqual("token2", aToken.TokenName);
            Assert.AreEqual((int)PLTokenTypes.SYMBOL, aToken.TokenType);

            // get '=>'
            aToken = PLLexer.ReadAndConsume();
            Assert.AreEqual("=>", aToken.TokenName);
            Assert.AreEqual((int)PLTokenTypes.IMPLIES, aToken.TokenType);

            // get "token3"
            aToken = PLLexer.ReadAndConsume();
            Assert.AreEqual("token3", aToken.TokenName);
            Assert.AreEqual((int)PLTokenTypes.SYMBOL, aToken.TokenType);

            // we should now be at eof
            bool isEOF = PLLexer.IsEOF();
            Assert.IsTrue(isEOF);

            // trying to get another token should return a 'null'
            aToken = PLLexer.ReadAndConsume();
            Assert.AreEqual((int)PLTokenTypes.EOF, aToken.TokenType);
        }

        [TestMethod]
        public void TestPLLexerFunctionsWithWhitespace()
        {
            PLLexer PLLexer = new PLLexer("  token1 AND    token2   =>token3   ");
            Token aToken;

            // skip white space and get "token1"
            aToken = PLLexer.ReadAndConsume();
            Assert.AreEqual("token1", aToken.TokenName);
            Assert.AreEqual((int)PLTokenTypes.SYMBOL, aToken.TokenType);

            // get "and"
            aToken = PLLexer.ReadAndConsume();
            Assert.AreEqual("and", aToken.TokenName);
            Assert.AreEqual((int)PLTokenTypes.AND, aToken.TokenType);

            // skip whitespace and get "token2"
            aToken = PLLexer.ReadAndConsume();
            Assert.AreEqual("token2", aToken.TokenName);
            Assert.AreEqual((int)PLTokenTypes.SYMBOL, aToken.TokenType);

            // skip whitespace and get '=>'
            aToken = PLLexer.ReadAndConsume();
            Assert.AreEqual("=>", aToken.TokenName);
            Assert.AreEqual((int)PLTokenTypes.IMPLIES, aToken.TokenType);

            // get "token3" without spaces in between
            aToken = PLLexer.ReadAndConsume();
            Assert.AreEqual("token3", aToken.TokenName);
            Assert.AreEqual((int)PLTokenTypes.SYMBOL, aToken.TokenType);

            // we should now be at eof even though there is whitespace there
            bool isEOF = PLLexer.IsEOF();
            Assert.IsTrue(isEOF);

            // trying to get another token should return a 'null'
            aToken = PLLexer.ReadAndConsume();
            Assert.AreEqual((int)PLTokenTypes.EOF, aToken.TokenType);

        }

        [TestMethod]
        public void TestPLLexerSyntaxError()
        {
            PLLexer PLLexer = new PLLexer("token1 AND token2 >= 1token3");
            Token aToken;
            bool errorThrown = false;


            try
            {
                // get "token1" -- this will fill up pipeline and cause an error
                aToken = PLLexer.CurrentToken();
            }
            catch
            {
                errorThrown = true;
            }
            Assert.IsTrue(errorThrown);

            aToken = PLLexer.CurrentToken();
            Assert.AreEqual("token1", aToken.TokenName);
            Assert.AreEqual((int)PLTokenTypes.SYMBOL, aToken.TokenType);

            // get "and"
            errorThrown = false;
            try
            {
                PLLexer.Consume();
            }
            catch
            {
                errorThrown = true;
            }
            Assert.IsTrue(errorThrown);

            // we should not be at eof because we have an invalid token
            bool isEOF = PLLexer.IsEOF();
            Assert.IsFalse(isEOF);
        }

        
        [TestMethod]
        public void TestPLLexerParentheses()
        {
            PLLexer PLLexer = new PLLexer("(token1 AND token2)<=> (token3)");
            Token aToken;

            // get "("
            aToken = PLLexer.ReadAndConsume();
            Assert.AreEqual("(", aToken.TokenName);
            Assert.AreEqual((int)PLTokenTypes.LPAREN, aToken.TokenType);

            // get "token1"
            aToken = PLLexer.ReadAndConsume();
            Assert.AreEqual("token1", aToken.TokenName);
            Assert.AreEqual((int)PLTokenTypes.SYMBOL, aToken.TokenType);

            // get "and"
            aToken = PLLexer.ReadAndConsume();
            Assert.AreEqual("and", aToken.TokenName);
            Assert.AreEqual((int)PLTokenTypes.AND, aToken.TokenType);

            // get "token2"
            aToken = PLLexer.ReadAndConsume();
            Assert.AreEqual("token2", aToken.TokenName);
            Assert.AreEqual((int)PLTokenTypes.SYMBOL, aToken.TokenType);
            
            // get ")"
            aToken = PLLexer.ReadAndConsume();
            Assert.AreEqual(")", aToken.TokenName);
            Assert.AreEqual((int)PLTokenTypes.RPAREN, aToken.TokenType);

            // get "<=>"
            aToken = PLLexer.ReadAndConsume();
            Assert.AreEqual("<=>", aToken.TokenName);
            Assert.AreEqual((int)PLTokenTypes.BICONDITIONAL, aToken.TokenType);

            // get "("
            aToken = PLLexer.ReadAndConsume();
            Assert.AreEqual("(", aToken.TokenName);
            Assert.AreEqual((int)PLTokenTypes.LPAREN, aToken.TokenType);

            // get "token3"
            aToken = PLLexer.ReadAndConsume();
            Assert.AreEqual("token3", aToken.TokenName);
            Assert.AreEqual((int)PLTokenTypes.SYMBOL, aToken.TokenType);

            // get ")"
            aToken = PLLexer.ReadAndConsume();
            Assert.AreEqual(")", aToken.TokenName);
            Assert.AreEqual((int)PLTokenTypes.RPAREN, aToken.TokenType);
            
            // we should now be at eof
            bool isEOF = PLLexer.IsEOF();
            Assert.IsTrue(isEOF);

        }

        [TestMethod]
        public void TestThrowParsingError()
        {
            PLLexer lexer = new PLLexer();
            Token testToken = new Token("Test Token", (int)PLTokenTypes.SYMBOL);

            bool errorThrown = false;
            try
            {
                lexer.ParserError("Test Error", testToken);
            }
            catch
            {
                errorThrown = true;
            }
            Assert.IsTrue(errorThrown);
        }

    }
}
