using AILab.PropositionalLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestAILab
{
    
    
    /// <summary>
    ///This is a test class for PLKnowledgeBaseTest and is intended
    ///to contain all PLKnowledgeBaseTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TestPLKnowledgeBase
    {


        /// <summary>
        ///A test for Add(Sentence) and Add(string)
        ///</summary>
        [TestMethod()]
        public void AddTest()
        {
            PLKnowledgeBase kb = new PLKnowledgeBase();
            Sentence sentence;

            // test adding a single sentence using a string or a Sentence
            kb.Add("a or b and c => d");
            sentence = new Sentence("c and a or d => b");
            kb.Add(sentence);

            Assert.AreEqual(kb.GetSentence(0).ToString(true), "((a OR (b AND c)) => d)");
            Assert.AreEqual(kb.GetSentence(1).ToString(true), "(((c AND a) OR d) => b)");

            bool errorThrown = false;
            try
            {
                string s = kb.GetSentence(3).ToString(true);
            }
            catch
            {
                errorThrown = true;
            }
            Assert.IsTrue(errorThrown);

            errorThrown = false;
            try
            {
                string s = kb.GetSentence(-1).ToString(true);
            }
            catch
            {
                errorThrown = true;
            }
            Assert.IsTrue(errorThrown);


        }


        /// <summary>
        ///A test for Add(Sentence[])
        ///</summary>
        [TestMethod()]
        public void AddArrayTest()
        {
            // this tests both ways to add multiple sentences to a knowledge base (i.e. Add(Sentence[]) and Add(string)
            // it incidentally tests Clear() and Count() and GetSentence()
            PLKnowledgeBase kb1 = new PLKnowledgeBase();
            PLKnowledgeBase kb2 = new PLKnowledgeBase(); 
            Sentence[] sentenceList;

            string input;
            input = "A";
            input = input + "\n";
            input = input + "B";
            input = input + "\n";
            input = input + "A AND B => L";
            input = input + "\n";
            input = input + "A AND P => L";
            input = input + "\n";
            input = input + "B AND L => M";
            input = input + "\n";
            input = input + "L AND M => P";
            input = input + "\n";
            input = input + "P => Q";

            PLParser parser = new PLParser(input);
            sentenceList = parser.ParseInput();
            kb1.Add(sentenceList);
            Assert.AreEqual(kb1.Count(), 7);
            Assert.AreEqual(kb1.GetSentence(0).Type, Sentence.SentenceType.AtomicSentence);
            Assert.AreEqual(kb1.GetSentence(0).ToString(true), "a");
            Assert.AreEqual(kb1.GetSentence(1).Type, Sentence.SentenceType.AtomicSentence);
            Assert.AreEqual(kb1.GetSentence(1).ToString(true), "b");
            Assert.AreEqual(kb1.GetSentence(2).Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(kb1.GetSentence(2).ToString(true), "((a AND b) => l)");
            Assert.AreEqual(kb1.GetSentence(3).Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(kb1.GetSentence(3).ToString(true), "((a AND p) => l)");
            Assert.AreEqual(kb1.GetSentence(4).Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(kb1.GetSentence(4).ToString(true), "((b AND l) => m)");
            Assert.AreEqual(kb1.GetSentence(5).Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(kb1.GetSentence(5).ToString(true), "((l AND m) => p)");
            Assert.AreEqual(kb1.GetSentence(6).Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(kb1.GetSentence(6).ToString(true), "(p => q)");


            kb2.Add(input);
            Assert.AreEqual(kb2.Count(), 7);
            Assert.AreEqual(kb2.GetSentence(0).Type, Sentence.SentenceType.AtomicSentence);
            Assert.AreEqual(kb2.GetSentence(0).ToString(true), "a");
            Assert.AreEqual(kb2.GetSentence(1).Type, Sentence.SentenceType.AtomicSentence);
            Assert.AreEqual(kb2.GetSentence(1).ToString(true), "b");
            Assert.AreEqual(kb2.GetSentence(2).Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(kb2.GetSentence(2).ToString(true), "((a AND b) => l)");
            Assert.AreEqual(kb2.GetSentence(3).Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(kb2.GetSentence(3).ToString(true), "((a AND p) => l)");
            Assert.AreEqual(kb2.GetSentence(4).Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(kb2.GetSentence(4).ToString(true), "((b AND l) => m)");
            Assert.AreEqual(kb2.GetSentence(5).Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(kb2.GetSentence(5).ToString(true), "((l AND m) => p)");
            Assert.AreEqual(kb2.GetSentence(6).Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(kb2.GetSentence(6).ToString(true), "(p => q)");

            // and test Clear
            kb1.Clear();
            Assert.AreEqual(kb1.Count(), 0);

        }


        /// <summary>
        ///A test for Exists
        ///</summary>
        [TestMethod()]
        public void ExistsTest()
        {
            PLKnowledgeBase kb = new PLKnowledgeBase();

            string input;
            input = "A";
            input = input + "\n";
            input = input + "B";
            input = input + "\n";
            input = input + "A AND B => L";
            input = input + "\n";
            input = input + "A AND P => L";
            input = input + "\n";
            input = input + "B AND L => M";
            input = input + "\n";
            input = input + "L AND M => P";
            input = input + "\n";
            input = input + "P => Q";

            kb.Add(input);

            Assert.IsTrue(kb.Exists("B AND L => M"));
            Assert.IsTrue(kb.Exists("P => Q"));
            Assert.IsTrue(kb.Exists("A AND B => L"));
            Assert.IsFalse(kb.Exists("B AND Z => M"));

        }



        [TestMethod()]
        public void EvaluateKnowledgeBaseTest()
        {
            // Tests for EvaluateKnowledgeBase(SymbolList model), IsKnowledgeBaseTrue(SymbolList model), IsKnowledgeBaseFalse(SymbolList model)
            // incidently tests GetSymbolList
            PLKnowledgeBase kb = new PLKnowledgeBase();

            string input;
            input = "A";
            input = input + "\n";
            input = input + "B";
            input = input + "\n";
            input = input + "A AND B => L";
            input = input + "\n";
            input = input + "A AND P => L";
            input = input + "\n";
            input = input + "B AND L => M";
            input = input + "\n";
            input = input + "L AND M => P";
            input = input + "\n";
            input = input + "P => Q";
            input = input + "\n";
            input = input + "~A => Z";
            input = input + "\n";
            input = input + "A and Z => W";
            input = input + "\n";
            input = input + "A or Z => ~X";

            kb.Add(input);

            SymbolList sl = kb.GetSymbolList();
            sl.Sort();

            Assert.AreEqual(sl.GetSymbol(0).SymbolName, "a");
            Assert.AreEqual(sl.GetSymbol(1).SymbolName, "b");
            Assert.AreEqual(sl.GetSymbol(2).SymbolName, "l");
            Assert.AreEqual(sl.GetSymbol(3).SymbolName, "m");
            Assert.AreEqual(sl.GetSymbol(4).SymbolName, "p");
            Assert.AreEqual(sl.GetSymbol(5).SymbolName, "q");
            Assert.AreEqual(sl.GetSymbol(6).SymbolName, "w");
            Assert.AreEqual(sl.GetSymbol(7).SymbolName, "x");
            Assert.AreEqual(sl.GetSymbol(8).SymbolName, "z");

            bool errorThrown = false;

            try
            {
                sl.GetSymbol(9);
            }
            catch
            {
                errorThrown = true;
            }

            Assert.IsTrue(errorThrown);

            // now test evaluate knowledge base

            // test an undefined set
            sl.SetValue("a", true);
            sl.SetValue("b", true);

            Assert.IsTrue(kb.EvaluateKnowledgeBase(sl) == LogicSymbolValue.Undefined);
            Assert.IsFalse(kb.IsKnowledgeBaseTrue(sl));
            Assert.IsFalse(kb.IsKnowledgeBaseFalse(sl));

            // test a valid set
            sl.SetValue("l", true);
            sl.SetValue("m", true);
            sl.SetValue("p", true);
            sl.SetValue("q", true);
            sl.SetValue("w", true);
            sl.SetValue("x", false);
            sl.SetValue("z", false);

            Assert.IsTrue(kb.EvaluateKnowledgeBase(sl) == LogicSymbolValue.True);
            Assert.IsTrue(kb.IsKnowledgeBaseTrue(sl));
            Assert.IsFalse(kb.IsKnowledgeBaseFalse(sl));

            // now test an invalid set of symbols
            sl.SetValue("b", false);

            Assert.IsTrue(kb.EvaluateKnowledgeBase(sl) == LogicSymbolValue.False);
            Assert.IsFalse(kb.IsKnowledgeBaseTrue(sl));
            Assert.IsTrue(kb.IsKnowledgeBaseFalse(sl));

            // test having only partial info, but still at least one clause is known to be false
            sl = kb.GetSymbolList();
            sl.SetValue("b", false);

            Assert.IsTrue(kb.EvaluateKnowledgeBase(sl) == LogicSymbolValue.False);
            Assert.IsFalse(kb.IsKnowledgeBaseTrue(sl));
            Assert.IsTrue(kb.IsKnowledgeBaseFalse(sl));


        }


        
        [TestMethod()]
        public void TruthTableEntailsTest()
        {
            
            // This tests out "TruthTableEntails()" plus it's "IsQueryTrue()" and "IsQueryFalse()" counterparts.
            // I  need to keep the tests to a minimum because this is an expensive action time wise 
            // since it's an NP-Complete Exponential problem. So most of the tests will stay commented out and
            // the tests I run over and over will stay to a minimum to make sure things haven't broken.
            PLKnowledgeBase kb = new PLKnowledgeBase();

            string input;
            input = "A";
            input = input + "\n";
            input = input + "B";
            input = input + "\n";
            input = input + "A AND B => L";
            //input = input + "\n";
            //input = input + "A AND P => L";
            input = input + "\n";
            input = input + "B AND L => M";
            input = input + "\n";
            input = input + "L AND M => P";
            input = input + "\n";
            input = input + "P => Q";
            input = input + "\n";
            input = input + "~A => Z";
            //input = input + "\n";
            //input = input + "A and Z => W";
            //input = input + "\n";
            //input = input + "A or Z => ~X";
            
            kb.Add(input);
            
            bool result1 = kb.IsQueryTrue("q");            
            Assert.IsTrue(result1);
            result1 = kb.IsQueryFalse("q");
            Assert.IsFalse(result1);
            LogicSymbolValue result2 = kb.TruthTableEntails("q");
            Assert.AreEqual(result2, LogicSymbolValue.True);

            // y is undefined because its not in the model at all
            result1 = kb.IsQueryTrue("y");
            Assert.IsFalse(result1);
            result2 = kb.TruthTableEntails("y");
            Assert.AreEqual(result2, LogicSymbolValue.Undefined);

            // test Undefined situations
            result1 = kb.IsQueryTrue("z");
            Assert.IsFalse(result1);
            result1 = kb.IsQueryFalse("z");
            Assert.IsFalse(result1);
            result2 = kb.TruthTableEntails("z");
            Assert.AreEqual(result2, LogicSymbolValue.Undefined);

            //result1 = kb.IsQueryTrue("~z");
            //Assert.IsFalse(result1);
            //result1 = kb.IsQueryFalse("~z");
            //Assert.IsFalse(result1);
            //result2 = kb.TruthTableEntails("~z");
            //Assert.AreEqual(result2, LogicSymbolValue.Undefined);

            result2 = kb.TruthTableEntails("~z or z");
            Assert.AreEqual(result2, LogicSymbolValue.True);
            //result1 = kb.IsQueryTrue("~z or z");
            //Assert.IsTrue(result1);
            //result1 = kb.IsQueryFalse("~z or z");
            //Assert.IsFalse(result1);
            
            
            /* //Removed to speed up tests -- keep if needing a more through retest
            result1 = kb.IsQueryTrue("a");
            Assert.IsTrue(result1);
            result2 = kb.TruthTableEntails("a");
            Assert.AreEqual(result2, LogicSymbolValue.True);
            

            result2 = kb.TruthTableEntails("a and ~a");
            Assert.AreEqual(result2, LogicSymbolValue.False);
            //result1 = kb.IsQueryTrue("a and ~a");
            //Assert.IsFalse(result1);
            result1 = kb.IsQueryFalse("a and ~a");
            Assert.IsTrue(result1);

            result1 = kb.IsQueryTrue("a or ~a");
            Assert.IsTrue(result1);
            result2 = kb.TruthTableEntails("a or ~a");
            Assert.AreEqual(result2, LogicSymbolValue.True);

            
            result1 = kb.IsQueryTrue("l");
            Assert.IsTrue(result1);
            result2 = kb.TruthTableEntails("l");
            Assert.AreEqual(result2, LogicSymbolValue.True);

            result1 = kb.IsQueryTrue("p");
            Assert.IsTrue(result1);
            result2 = kb.TruthTableEntails("p");
            Assert.AreEqual(result2, LogicSymbolValue.True);

            result1 = kb.IsQueryTrue("a and b and l and m and p and q");
            Assert.IsTrue(result1);
            result2 = kb.TruthTableEntails("a and b and l and m and p and q");
            Assert.AreEqual(result2, LogicSymbolValue.True);

            result1 = kb.IsQueryTrue("a and b and l and m and p and q and ~a");
            Assert.IsFalse(result1);
            result2 = kb.TruthTableEntails("a and b and l and m and p and q and ~a");
            Assert.AreEqual(result2, LogicSymbolValue.False);


            result1 = kb.IsQueryTrue("a and b and l and m and p and q and z");
            Assert.IsFalse(result1);
            result2 = kb.TruthTableEntails("a and b and l and m and p and q and z");
            Assert.AreEqual(result2, LogicSymbolValue.Undefined);

            result1 = kb.IsQueryTrue("x");
            Assert.IsFalse(result1);
            result2 = kb.TruthTableEntails("x");
            Assert.AreEqual(result2, LogicSymbolValue.False);

            result1 = kb.IsQueryTrue("~x");
            Assert.IsTrue(result1);
            result2 = kb.TruthTableEntails("~x");
            Assert.AreEqual(result2, LogicSymbolValue.True);

            result1 = kb.IsQueryTrue("w");
            Assert.IsFalse(result1);
            result1 = kb.IsQueryFalse("w");
            Assert.IsFalse(result1);
            result2 = kb.TruthTableEntails("w");
            Assert.AreEqual(result2, LogicSymbolValue.Undefined);

            result1 = kb.IsQueryTrue("~w");
            Assert.IsFalse(result1);
            result1 = kb.IsQueryFalse("~w");
            Assert.IsFalse(result1);
            result2 = kb.TruthTableEntails("~w");
            Assert.AreEqual(result2, LogicSymbolValue.Undefined);

            result1 = kb.IsQueryTrue("~w or w");
            Assert.IsTrue(result1);
            result1 = kb.IsQueryFalse("~w or w");
            Assert.IsFalse(result1);
            result2 = kb.TruthTableEntails("~w or w");
            Assert.AreEqual(result2, LogicSymbolValue.True);
            */


        }



        [TestMethod()]
        public void ConvertToCNFTest()
        {
            PLKnowledgeBase kb = new PLKnowledgeBase();
            PLKnowledgeBase kbCNF;

            string input;
            input = "A";
            input = input + "\n";
            input = input + "B";
            input = input + "\n";
            input = input + "A AND B => L";
            input = input + "\n";
            input = input + "A AND P => L";
            input = input + "\n";
            input = input + "B AND L => M";
            input = input + "\n";
            input = input + "L AND M => P";
            input = input + "\n";
            input = input + "P => Q";

            kb.Add(input);
            Assert.AreEqual(kb.Count(), 7);
            Assert.AreEqual(kb.GetSentence(0).Type, Sentence.SentenceType.AtomicSentence);
            Assert.AreEqual(kb.GetSentence(0).ToString(true), "a");
            Assert.AreEqual(kb.GetSentence(1).Type, Sentence.SentenceType.AtomicSentence);
            Assert.AreEqual(kb.GetSentence(1).ToString(true), "b");
            Assert.AreEqual(kb.GetSentence(2).Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(kb.GetSentence(2).ToString(true), "((a AND b) => l)");
            Assert.AreEqual(kb.GetSentence(3).Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(kb.GetSentence(3).ToString(true), "((a AND p) => l)");
            Assert.AreEqual(kb.GetSentence(4).Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(kb.GetSentence(4).ToString(true), "((b AND l) => m)");
            Assert.AreEqual(kb.GetSentence(5).Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(kb.GetSentence(5).ToString(true), "((l AND m) => p)");
            Assert.AreEqual(kb.GetSentence(6).Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(kb.GetSentence(6).ToString(true), "(p => q)");

            kbCNF = kb.CNFClone();
            Assert.AreEqual(kb.Count(), 7);
            
            Assert.AreEqual(7, kbCNF.Count());
            Assert.AreEqual(Sentence.SentenceType.AtomicSentence, kbCNF.GetSentence(0).Type);
            Assert.AreEqual( "a", kbCNF.GetSentence(0).ToString());
            Assert.AreEqual(Sentence.SentenceType.AtomicSentence, kbCNF.GetSentence(1).Type);
            Assert.AreEqual("b", kbCNF.GetSentence(1).ToString());
            Assert.AreEqual(Sentence.SentenceType.ComplexSentence, kbCNF.GetSentence(2).Type);
            Assert.AreEqual("~a OR ~b OR l", kbCNF.GetSentence(2).ToString());
            Assert.AreEqual(Sentence.SentenceType.ComplexSentence, kbCNF.GetSentence(3).Type);
            Assert.AreEqual("~a OR ~p OR l", kbCNF.GetSentence(3).ToString());
            Assert.AreEqual(Sentence.SentenceType.ComplexSentence, kbCNF.GetSentence(4).Type);
            Assert.AreEqual("~b OR ~l OR m", kbCNF.GetSentence(4).ToString());
            Assert.AreEqual(Sentence.SentenceType.ComplexSentence, kbCNF.GetSentence(5).Type);
            Assert.AreEqual("~l OR ~m OR p", kbCNF.GetSentence(5).ToString());
            Assert.AreEqual(Sentence.SentenceType.ComplexSentence, kbCNF.GetSentence(6).Type);
            Assert.AreEqual("~p OR q", kbCNF.GetSentence(6).ToString());


            kb.Clear();
            input = "A";
            kb.Add(input);
            Assert.AreEqual(1, kb.Count());
            Assert.AreEqual(Sentence.SentenceType.AtomicSentence, kb.GetSentence(0).Type);
            Assert.AreEqual("a", kb.GetSentence(0).ToString(true));
            kbCNF = kb.CNFClone();
            Assert.AreEqual(1, kb.Count());
            Assert.AreEqual(1, kbCNF.Count());
            Assert.AreEqual(Sentence.SentenceType.AtomicSentence, kbCNF.GetSentence(0).Type);
            Assert.AreEqual("a", kbCNF.GetSentence(0).ToString());

        }

        [TestMethod()]
        public void DPLLEntailsTest()
        {           
            // This tests out "DPLLEntails()" plus it's "IsQueryTrue()" and "IsQueryFalse()" counterparts.
            // Hopefully this is faster than TruthTable entails and I can do a full suite of tests.
            // DPLLEntails returns the same results as TruthTableEntails with the exception that it is boolean only
            // with "undefined" being returned as "false"
            PLKnowledgeBase kb = new PLKnowledgeBase();

            string input;
            input = "A";
            input = input + "\n";
            input = input + "B";
            input = input + "\n";
            input = input + "A AND B => L";
            input = input + "\n";
            input = input + "A AND P => L";
            input = input + "\n";
            input = input + "B AND L => M";
            input = input + "\n";
            input = input + "L AND M => P";
            input = input + "\n";
            input = input + "P => Q";
            input = input + "\n";
            input = input + "~A => Z";
            input = input + "\n";
            input = input + "A and Z => W";
            input = input + "\n";
            input = input + "A or Z => ~X";

            kb.Add(input);

            bool result1, result2;
            
            result1 = kb.IsQueryTrue("q");
            Assert.IsTrue(result1);
            result1 = kb.IsQueryFalse("q");
            Assert.IsFalse(result1);
            result2 = kb.DPLLEntails("q");
            Assert.IsTrue(result2);
           
            
            // y is undefined because its not in the model at all
            result1 = kb.IsQueryTrue("y");
            Assert.IsFalse(result1);
            result2 = kb.DPLLEntails("y");
            Assert.IsFalse(result2); 

            // test Undefined situations
            result1 = kb.IsQueryTrue("z");
            Assert.IsFalse(result1);
            result1 = kb.IsQueryFalse("z");
            Assert.IsFalse(result1);
            result2 = kb.DPLLEntails("z");
            Assert.IsFalse(result2);

            result1 = kb.IsQueryTrue("~z");
            Assert.IsFalse(result1);
            result1 = kb.IsQueryFalse("~z");
            Assert.IsFalse(result1);
            result2 = kb.DPLLEntails("~z");
            Assert.IsFalse(result2);

            result2 = kb.DPLLEntails("~z or z");
            Assert.IsTrue(result2);
            result1 = kb.IsQueryTrue("~z or z");
            Assert.IsTrue(result1);
            result1 = kb.IsQueryFalse("~z or z");
            Assert.IsFalse(result1);

    
            result1 = kb.IsQueryTrue("a");
            Assert.IsTrue(result1);
            result2 = kb.DPLLEntails("a");
            Assert.IsTrue(result2);


            result2 = kb.DPLLEntails("a and ~a");
            Assert.IsFalse(result2);
            result1 = kb.IsQueryTrue("a and ~a");
            Assert.IsFalse(result1);
            result1 = kb.IsQueryFalse("a and ~a");
            Assert.IsTrue(result1);

            result1 = kb.IsQueryTrue("a or ~a");
            Assert.IsTrue(result1);
            result2 = kb.DPLLEntails("a or ~a");
            Assert.IsTrue(result2);

            
            result1 = kb.IsQueryTrue("l");
            Assert.IsTrue(result1);
            result2 = kb.DPLLEntails("l");
            Assert.IsTrue(result2);

            result1 = kb.IsQueryTrue("p");
            Assert.IsTrue(result1);
            result2 = kb.DPLLEntails("p");
            Assert.IsTrue(result2);

            result1 = kb.IsQueryTrue("a and b and l and m and p and q");
            Assert.IsTrue(result1);
            result2 = kb.DPLLEntails("a and b and l and m and p and q");
            Assert.IsTrue(result2);

            result1 = kb.IsQueryTrue("a and b and l and m and p and q and ~a");
            Assert.IsFalse(result1);
            result2 = kb.DPLLEntails("a and b and l and m and p and q and ~a");
            Assert.IsFalse(result2);


            result1 = kb.IsQueryTrue("a and b and l and m and p and q and z");
            Assert.IsFalse(result1);
            result2 = kb.DPLLEntails("a and b and l and m and p and q and z");
            Assert.IsFalse(result2);

            result1 = kb.IsQueryTrue("x");
            Assert.IsFalse(result1);
            result2 = kb.DPLLEntails("x");
            Assert.IsFalse(result2);

            result1 = kb.IsQueryTrue("~x");
            Assert.IsTrue(result1);
            result2 = kb.DPLLEntails("~x");
            Assert.IsTrue(result2);

            result1 = kb.IsQueryTrue("w");
            Assert.IsFalse(result1);
            result1 = kb.IsQueryFalse("w");
            Assert.IsFalse(result1);
            result2 = kb.DPLLEntails("w");
            Assert.IsFalse(result2);

            result1 = kb.IsQueryTrue("~w");
            Assert.IsFalse(result1);
            result1 = kb.IsQueryFalse("~w");
            Assert.IsFalse(result1);
            result2 = kb.DPLLEntails("~w");
            Assert.IsFalse(result2);

            result1 = kb.IsQueryTrue("~w or w");
            Assert.IsTrue(result1);
            result1 = kb.IsQueryFalse("~w or w");
            Assert.IsFalse(result1);
            result2 = kb.DPLLEntails("~w or w");
            Assert.IsTrue(result2);           
        }


        [TestMethod]
        public void TestDPLLSatisfiability()
        {
            // TODO - need more test cases
            PLKnowledgeBase kb = new PLKnowledgeBase();

            string input;
            bool result;

            input = "A and ~A";
            kb.Add(input);
            Assert.IsFalse(kb.IsCNF);
            result = kb.DPLLIsSatisfiable();
            Assert.IsFalse(result);
            kb.Clear();

            input = "A";
            kb.Add(input);
            Assert.IsFalse(kb.IsCNF);
            result = kb.DPLLIsSatisfiable();
            Assert.IsTrue(result);
            kb.Clear();

            // repeat with pre CNF knowledge bases to test other route

            input = "A and ~A";
            kb.Add(input);
            Assert.IsFalse(kb.IsCNF);
            kb.ConvertToCNF();
            Assert.IsTrue(kb.IsCNF);
            result = kb.DPLLIsSatisfiable();
            Assert.IsFalse(result);
            kb.Clear();
            Assert.IsFalse(kb.IsCNF);

            input = "A";
            kb.Add(input);
            Assert.IsFalse(kb.IsCNF);
            kb.ConvertToCNF(); 
            Assert.IsTrue(kb.IsCNF);
            result = kb.IsSatisfiable();
            Assert.IsTrue(result);
            kb.Clear();
            Assert.IsFalse(kb.IsCNF);

        }


        [TestMethod]
        public void TestClone()
        {
            PLKnowledgeBase kb = new PLKnowledgeBase();
            PLKnowledgeBase clone;

            string input;
            input = "A";
            input = input + "\n";
            input = input + "B";
            input = input + "\n";
            input = input + "A AND B => L";
            input = input + "\n";
            input = input + "A AND P => L";
            input = input + "\n";
            input = input + "B AND L => M";
            input = input + "\n";
            input = input + "L AND M => P";
            input = input + "\n";
            input = input + "P => Q";

            kb.Add(input);

            clone = kb.Clone();
            Assert.AreEqual(clone.Count(), 7);
            Assert.AreEqual(clone.GetSentence(0).Type, Sentence.SentenceType.AtomicSentence);
            Assert.AreEqual(clone.GetSentence(0).ToString(true), "a");
            Assert.AreEqual(clone.GetSentence(1).Type, Sentence.SentenceType.AtomicSentence);
            Assert.AreEqual(clone.GetSentence(1).ToString(true), "b");
            Assert.AreEqual(clone.GetSentence(2).Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(clone.GetSentence(2).ToString(true), "((a AND b) => l)");
            Assert.AreEqual(clone.GetSentence(3).Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(clone.GetSentence(3).ToString(true), "((a AND p) => l)");
            Assert.AreEqual(clone.GetSentence(4).Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(clone.GetSentence(4).ToString(true), "((b AND l) => m)");
            Assert.AreEqual(clone.GetSentence(5).Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(clone.GetSentence(5).ToString(true), "((l AND m) => p)");
            Assert.AreEqual(clone.GetSentence(6).Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(clone.GetSentence(6).ToString(true), "(p => q)");

        }

    }
         
}
