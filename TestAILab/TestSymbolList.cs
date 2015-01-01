using AILab.PropositionalLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestAILab
{
    
    
    /// <summary>
    ///This is a test class for PLSymbolListTest and is intended
    ///to contain all PLSymbolListTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TestSymbolList
    {

        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod()]
        public void AddSymbolTest()
        {
            SymbolList symbolList = new SymbolList();
            symbolList.Add("a");
            symbolList.Add("c");
            symbolList.Add("b");

            Assert.AreEqual(symbolList.Count(), 3);
            Assert.IsNotNull(symbolList.Find("a"));
            Assert.IsNotNull(symbolList.Find("b"));
            Assert.IsNotNull(symbolList.Find("c"));
            Assert.IsNull(symbolList.Find("d"));
        }



        /// <summary>
        ///A test for Remove
        ///</summary>
        [TestMethod()]
        public void RemoveSymbolTest()
        {
            SymbolList symbolList = new SymbolList();
            symbolList.Add("a");
            symbolList.Add("c");
            symbolList.Add("b");

            Assert.AreEqual(symbolList.Count(), 3);
            Assert.IsNotNull(symbolList.Find("a"));
            Assert.IsNotNull(symbolList.Find("b"));
            Assert.IsNotNull(symbolList.Find("c"));
            Assert.IsNull(symbolList.Find("d"));

            symbolList.Remove("b");

            Assert.AreEqual(symbolList.Count(), 2);
            Assert.IsNotNull(symbolList.Find("a"));
            Assert.IsNull(symbolList.Find("b"));
            Assert.IsNotNull(symbolList.Find("c"));
            Assert.IsNull(symbolList.Find("d"));

            symbolList.Remove(new PLToken("a", PLTokenTypes.SYMBOL));

            Assert.AreEqual(symbolList.Count(), 1);
            Assert.IsNull(symbolList.Find("a"));
            Assert.IsNull(symbolList.Find("b"));
            Assert.IsNotNull(symbolList.Find("c"));
            Assert.IsNull(symbolList.Find("d"));


        }


        /// <summary>
        ///A test for IsTrue, IsFalse and SetValue (boolean) functions
        ///</summary>
        [TestMethod()]
        public void IsTrueAndSetValueTest()
        {
            SymbolList symbolList = new SymbolList();
            symbolList.Add("a");
            symbolList.Add("c");
            symbolList.Add("b");

            symbolList.SetValue("a", true);
            symbolList.SetValue("b", false);
            symbolList.SetValue("c", true);
            
            Assert.AreEqual(symbolList.Count(), 3);
            Assert.AreEqual(symbolList.IsTrue("a"), true);
            Assert.AreEqual(symbolList.IsFalse("b"), true);
            Assert.AreEqual(symbolList.IsTrue("c"), true);
            // I have it returning false for a symbol that doesn't exist
            Assert.AreEqual(symbolList.IsTrue("d"), false);
        }



        /// <summary>
        ///A test for GetValue and SetValue (non-boolean) functions
        ///</summary>
        [TestMethod()]
        public void GetValueTest()
        {
            SymbolList symbolList = new SymbolList();
            symbolList.Add("a", true);
            symbolList.Add("c", LogicSymbolValue.True);
            symbolList.Add("d", LogicSymbolValue.Undefined);
            symbolList.Add("b", LogicSymbolValue.False);
            symbolList.Add("e");

            Assert.AreEqual(symbolList.Count(), 5);
            Assert.AreEqual(symbolList.GetValue("a"), LogicSymbolValue.True);
            Assert.AreEqual(symbolList.GetValue("b"), LogicSymbolValue.False);
            Assert.AreEqual(symbolList.GetValue("c"), LogicSymbolValue.True);
            Assert.AreEqual(symbolList.GetValue("d"), LogicSymbolValue.Undefined);
            Assert.AreEqual(symbolList.GetValue("e"), LogicSymbolValue.Undefined);
            // I have it returning false for a symbol that doesn't exist
            Assert.AreEqual(symbolList.GetValue("f"), LogicSymbolValue.Undefined);
        }


        /// <summary>
        ///A test for Symbol Table QuickSort
        ///</summary>
        [TestMethod()]
        public void QuickSortTest()
        {
            SymbolList symbolList = new SymbolList();
            // don't sort so we can test it
            symbolList.AutoSort = false;
            symbolList.Add("l");
            symbolList.Add("c");
            symbolList.Add("g");
            symbolList.Add("h");
            symbolList.Add("i");
            symbolList.Add("a");
            symbolList.Add("d");
            symbolList.Add("k");
            symbolList.Add("p");
            symbolList.Add("f");
            symbolList.Add("n");
            symbolList.Add("b");
            symbolList.Add("j");
            symbolList.Add("m");
            symbolList.Add("o");
            symbolList.Add("e");

            Assert.IsFalse(symbolList.IsSorted);
            Assert.AreEqual(symbolList.Count(), 16);
            Assert.AreEqual(symbolList.GetSymbol(0).SymbolName, "l");
            Assert.AreEqual(symbolList.GetSymbol(1).SymbolName, "c");
            Assert.AreEqual(symbolList.GetSymbol(2).SymbolName, "g");
            Assert.AreEqual(symbolList.GetSymbol(3).SymbolName, "h");
            Assert.AreEqual(symbolList.GetSymbol(4).SymbolName, "i");
            Assert.AreEqual(symbolList.GetSymbol(5).SymbolName, "a");
            Assert.AreEqual(symbolList.GetSymbol(6).SymbolName, "d");
            Assert.AreEqual(symbolList.GetSymbol(7).SymbolName, "k");
            Assert.AreEqual(symbolList.GetSymbol(8).SymbolName, "p");
            Assert.AreEqual(symbolList.GetSymbol(9).SymbolName, "f");
            Assert.AreEqual(symbolList.GetSymbol(10).SymbolName, "n");
            Assert.AreEqual(symbolList.GetSymbol(11).SymbolName, "b");
            Assert.AreEqual(symbolList.GetSymbol(12).SymbolName, "j");
            Assert.AreEqual(symbolList.GetSymbol(13).SymbolName, "m");
            Assert.AreEqual(symbolList.GetSymbol(14).SymbolName, "o");
            Assert.AreEqual(symbolList.GetSymbol(15).SymbolName, "e");

            symbolList.Sort();

            Assert.IsTrue(symbolList.IsSorted);
            Assert.AreEqual(symbolList.Count(), 16);
            Assert.AreEqual(symbolList.GetSymbol(0).SymbolName, "a");
            Assert.AreEqual(symbolList.GetSymbol(1).SymbolName, "b");
            Assert.AreEqual(symbolList.GetSymbol(2).SymbolName, "c");
            Assert.AreEqual(symbolList.GetSymbol(3).SymbolName, "d");
            Assert.AreEqual(symbolList.GetSymbol(4).SymbolName, "e");
            Assert.AreEqual(symbolList.GetSymbol(5).SymbolName, "f");
            Assert.AreEqual(symbolList.GetSymbol(6).SymbolName, "g");
            Assert.AreEqual(symbolList.GetSymbol(7).SymbolName, "h");
            Assert.AreEqual(symbolList.GetSymbol(8).SymbolName, "i");
            Assert.AreEqual(symbolList.GetSymbol(9).SymbolName, "j");
            Assert.AreEqual(symbolList.GetSymbol(10).SymbolName, "k");
            Assert.AreEqual(symbolList.GetSymbol(11).SymbolName, "l");
            Assert.AreEqual(symbolList.GetSymbol(12).SymbolName, "m");
            Assert.AreEqual(symbolList.GetSymbol(13).SymbolName, "n");
            Assert.AreEqual(symbolList.GetSymbol(14).SymbolName, "o");
            Assert.AreEqual(symbolList.GetSymbol(15).SymbolName, "p");

        }




        /// <summary>
        ///A test for Add(SymbolList list)
        ///</summary>
        [TestMethod()]
        public void AddSymbolListTest()
        {

            Sentence sentence1 = new Sentence("a and c and b => d");
            Sentence sentence2 = new Sentence("e and c and f => b and g");
            SymbolList sList = new SymbolList();
            sList.Add(sentence1.GetSymbolList());
            sList.Sort();
            Assert.AreEqual(sList.GetSymbol(0).SymbolName, "a");
            Assert.AreEqual(sList.GetSymbol(1).SymbolName, "b");
            Assert.AreEqual(sList.GetSymbol(2).SymbolName, "c");
            Assert.AreEqual(sList.GetSymbol(3).SymbolName, "d");

            sList.Add(sentence2.GetSymbolList());
            sList.Sort();
            Assert.AreEqual(sList.GetSymbol(0).SymbolName, "a");
            Assert.AreEqual(sList.GetSymbol(1).SymbolName, "b");
            Assert.AreEqual(sList.GetSymbol(2).SymbolName, "c");
            Assert.AreEqual(sList.GetSymbol(3).SymbolName, "d");
            Assert.AreEqual(sList.GetSymbol(4).SymbolName, "e");
            Assert.AreEqual(sList.GetSymbol(5).SymbolName, "f");
            Assert.AreEqual(sList.GetSymbol(6).SymbolName, "g");

        }


        /// <summary>
        ///A test for the Copy function
        ///</summary>
        [TestMethod()]
        public void CloneTest()
        {
            SymbolList s1 = new Sentence("a and b and c").GetSymbolList();
            SymbolList s2, s3;

            s1.Sort();
            Assert.AreEqual(s1.GetSymbol(0).SymbolName, "a");
            Assert.AreEqual(s1.GetSymbol(1).SymbolName, "b");
            Assert.AreEqual(s1.GetSymbol(2).SymbolName, "c");

            s2 = s1.Clone();
            Assert.AreEqual(s2.GetSymbol(0).SymbolName, "a");
            Assert.AreEqual(s2.GetSymbol(1).SymbolName, "b");
            Assert.AreEqual(s2.GetSymbol(2).SymbolName, "c");

            for (int i = 0, count = s1.Count(); i < count; i++)
            {
                bool value;
                if (i % 2 == 0)
                    value = true;
                else
                    value = false;
                s1.SetValue(s1.GetSymbol(i).SymbolName, value);
            }

            s3 = s1.Clone();
            Assert.AreEqual(s3.GetSymbol(0).SymbolName, "a");
            Assert.AreEqual(s3.GetSymbol(0).Value, LogicSymbolValue.True);
            Assert.AreEqual(s3.GetSymbol(1).SymbolName, "b");
            Assert.AreEqual(s3.GetSymbol(1).Value, LogicSymbolValue.False);
            Assert.AreEqual(s3.GetSymbol(2).SymbolName, "c");
            Assert.AreEqual(s3.GetSymbol(2).Value, LogicSymbolValue.True);
        }

    }
}
