using AILab.PropositionalLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestAILab
{
    
    
    /// <summary>
    ///This is a test class for PLTokenTest and is intended
    ///to contain all PLTokenTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TestPLToken
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for ConvertTokenTypeToString
        ///</summary>
        [TestMethod()]
        public void ConvertTokenTypeToStringTest()
        {
            Assert.AreEqual(PLToken.ConvertTokenTypeToString(PLTokenTypes.AND), "AND");
            Assert.AreEqual(PLToken.ConvertTokenTypeToString(PLTokenTypes.BICONDITIONAL), "<=>");
            Assert.AreEqual(PLToken.ConvertTokenTypeToString(PLTokenTypes.EOF), "End of File");
            Assert.AreEqual(PLToken.ConvertTokenTypeToString(PLTokenTypes.IMPLIES), "=>");
            Assert.AreEqual(PLToken.ConvertTokenTypeToString(PLTokenTypes.LPAREN), "'('");
            Assert.AreEqual(PLToken.ConvertTokenTypeToString(PLTokenTypes.NEWLINE), "New Line");
            Assert.AreEqual(PLToken.ConvertTokenTypeToString(PLTokenTypes.NOT), "'~'");
            Assert.AreEqual(PLToken.ConvertTokenTypeToString(PLTokenTypes.OR), "OR");
            Assert.AreEqual(PLToken.ConvertTokenTypeToString(PLTokenTypes.RPAREN), "')'");
            Assert.AreEqual(PLToken.ConvertTokenTypeToString(PLTokenTypes.SYMBOL), "Symbol");
        }
    }
}
