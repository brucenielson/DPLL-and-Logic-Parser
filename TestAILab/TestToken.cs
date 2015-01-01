using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AILab.Common;
using AILab.PropositionalLogic;

namespace TestAILab
{
    [TestClass]
    public class TestToken
    {
        [TestMethod]
        public void TestTokenCreation()
        {
            // create a Token for a Symbol
            Token aToken = new PLToken("Test", PLTokenTypes.SYMBOL);
            Assert.AreEqual(aToken.TokenName, "test");
            Assert.AreEqual(aToken.TokenType, (int)PLTokenTypes.SYMBOL);
        }

        [TestMethod]
        public void TestTokenMatchName()
        {
            // create a Token and use Match Name on it
            Token aToken = new PLToken("Test", PLTokenTypes.SYMBOL);
            Assert.IsTrue(aToken.MatchName("test"));
            Assert.IsFalse(aToken.MatchName("Test2"));
        }


        [TestMethod]
        public void TestTokenEquals()
        {
            // create tokens and then test Equals function 
            Token token1 = new PLToken("Match", PLTokenTypes.SYMBOL);
            Token token2 = new PLToken("Match", PLTokenTypes.SYMBOL);
            Token token3 = new PLToken("NotMatch", PLTokenTypes.SYMBOL);
            Token token4 = new PLToken("Match", PLTokenTypes.AND);
            Assert.IsTrue(token1.Equals(token1));
            Assert.IsTrue(token1.Equals(token2));
            Assert.IsFalse(token1.Equals(token3));
            Assert.IsFalse(token1.Equals(token4));
        }


    }
}
