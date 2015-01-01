using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AILab.PropositionalLogic;

namespace TestAILab
{
    [TestClass]
    public class TestTokenTable
    {
        [TestMethod]
        public void TestBasicSymbolTableFunctions()
        {            
            
            // basic test
            PLTokenTable table = new PLTokenTable();
            Assert.AreEqual(table.Find("and").TokenType, (int)PLTokenTypes.AND);
            table.Add("Test", 100);
            Assert.AreEqual(table.Find("test").TokenType, 100);
            Assert.AreEqual(table.FindOrAdd("Test", 200).TokenType, 100);
            Assert.AreEqual(table.FindOrAdd("Test2", 200).TokenType, 200);  
        }

    }
}
