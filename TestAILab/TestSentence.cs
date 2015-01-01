using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AILab.PropositionalLogic;

namespace TestSentences
{
    [TestClass]
    public class TestSentence
    {

        [TestMethod]
        public void TestAtomicSentence()
        {
            // test atomic sentence using a Symbol Token
            PLToken aSymbol = new PLToken("Test", (int)PLTokenTypes.SYMBOL);
            Sentence atomicSentence = new Sentence(aSymbol);
            Assert.AreEqual(atomicSentence.Type, Sentence.SentenceType.AtomicSentence);
            Assert.AreEqual(atomicSentence.Symbol.TokenName, "test");
            Assert.AreEqual(atomicSentence.Negation, false);
            Assert.AreEqual(atomicSentence.ToString(true), "test");

            // test an attempt to create an atomic sentence with a non-symbol token
            bool errorThrown = false;
            try
            {
                aSymbol = new PLToken("and", PLTokenTypes.AND);
                atomicSentence = new Sentence(aSymbol);
            }
            catch
            {
                errorThrown = true;
            }
            Assert.IsTrue(errorThrown);
        }




        [TestMethod]
        public void TestNegatedSentence()
        {
            // test an "atomic" negated sentence using a Symbol Token
            PLToken aSymbol = new PLToken("Test", PLTokenTypes.SYMBOL);
            Sentence negatedAtomicSentence = new Sentence(aSymbol, false);
            Assert.AreEqual(negatedAtomicSentence.Type, Sentence.SentenceType.AtomicSentence);
            Assert.AreEqual(negatedAtomicSentence.Negation, true);
            Assert.AreEqual(negatedAtomicSentence.Symbol.TokenName, "test");
            Assert.AreEqual(negatedAtomicSentence.Symbol.TokenType, (int)PLTokenTypes.SYMBOL);
            Assert.AreEqual(negatedAtomicSentence.ToString(true), "~test");

            // test an "atomic" negated sentence using a non-Symbol Token -- should throw error            
            bool errorThrown = false;
            try
            {
                aSymbol = new PLToken("and", PLTokenTypes.AND);
                negatedAtomicSentence = new Sentence(aSymbol, false);
            }
            catch
            {
                errorThrown = true;
            }
            Assert.IsTrue(errorThrown);
            

            // test regular negated sentence
            aSymbol = new PLToken("Test", PLTokenTypes.SYMBOL);
            Sentence atomicSentence = new Sentence(aSymbol);
            Sentence negatedSentence = new Sentence(atomicSentence, false);
            Assert.AreEqual(negatedAtomicSentence.Negation, true);
            Assert.AreEqual(negatedAtomicSentence.Symbol.TokenName, "test");
            Assert.AreEqual(negatedAtomicSentence.Symbol.TokenType, (int)PLTokenTypes.SYMBOL);
            Assert.AreEqual(negatedSentence.ToString(true), "~test");

            // test complex negated sentence
            PLToken aSymbol1 = new PLToken("test1", PLTokenTypes.SYMBOL);
            PLToken aSymbol2 = new PLToken("test2", PLTokenTypes.SYMBOL);

            negatedSentence = new Sentence(aSymbol1, Sentence.LogicOperatorTypes.And, aSymbol2);
            negatedSentence = new Sentence(negatedSentence, false);

            Assert.AreEqual(negatedSentence.Negation, true);
            Assert.AreEqual(negatedSentence.FirstSentence.Symbol.TokenName, "test1");
            Assert.AreEqual(negatedSentence.SecondSentence.Symbol.TokenName, "test2");
            Assert.AreEqual(negatedSentence.LogicOperator, Sentence.LogicOperatorTypes.And);
            Assert.AreEqual(negatedSentence.ToString(true), "~(test1 AND test2)");

        }


        [TestMethod]
        public void TestNotNegatedSentences()
        {
            // I did not test out using the "negated sentence" calls but putting "true" in the parameter. That should 
            // behave just like not having the parameter
            Sentence sentence1, sentence2;
            PLToken token;

            token = new PLToken("a", PLTokenTypes.SYMBOL);
            sentence1 = new Sentence(token);
            sentence2 = new Sentence(token, true);
            Assert.IsTrue(sentence1.ToString(true) == sentence2.ToString(true));
            Assert.AreEqual(sentence1.ToString(true), "a");

            sentence1 = new Sentence("a and ~b");
            sentence2 = new Sentence(sentence1, true);
            Assert.IsTrue(sentence1.ToString(true) == sentence2.ToString(true));
            Assert.AreEqual(sentence1.ToString(true), "(a AND ~b)");

            sentence1 = new Sentence("~~~(a and b)");
            sentence2 = new Sentence(sentence1, true);
            Assert.IsTrue(sentence1.ToString(true) == sentence2.ToString(true));
            Assert.AreEqual(sentence1.ToString(true), "~~~(a AND b)");

        }


        [TestMethod]
        public void TestComplexSentence()
        {
            PLToken symbol1;
            PLToken symbol2;
            Sentence complexSentence;
            
            // test a complex sentence using a Symbol Tokens
            symbol1 = new PLToken("Test1", PLTokenTypes.SYMBOL);
            symbol2 = new PLToken("Test2", PLTokenTypes.SYMBOL);
            complexSentence = new Sentence(symbol1, Sentence.LogicOperatorTypes.And, symbol2);
            Assert.AreEqual(complexSentence.Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(complexSentence.FirstSentence.Symbol.TokenName, "test1");
            Assert.AreEqual(complexSentence.SecondSentence.Symbol.TokenName, "test2");
            Assert.AreEqual(complexSentence.LogicOperator, Sentence.LogicOperatorTypes.And);

            // test a complex sentence using a non-Symbol Token -- should throw error            
            bool errorThrown = false;
            try
            {
                symbol1 = new PLToken("and", PLTokenTypes.AND);
                symbol2 = new PLToken("Test2", PLTokenTypes.SYMBOL);
                complexSentence = new Sentence(symbol1, Sentence.LogicOperatorTypes.And, symbol2);
            }
            catch
            {
                errorThrown = true;
            }
            Assert.IsTrue(errorThrown);

            // try again with the other parameter
            errorThrown = false;
            try
            {
                symbol1 = new PLToken("Test1", PLTokenTypes.SYMBOL);
                symbol2 = new PLToken("and", PLTokenTypes.AND);
                complexSentence = new Sentence(symbol1, Sentence.LogicOperatorTypes.And, symbol2);
            }
            catch
            {
                errorThrown = true;
            }
            Assert.IsTrue(errorThrown);
           

            // test regular complex sentence
            symbol1 = new PLToken("Test1", PLTokenTypes.SYMBOL);
            symbol2 = new PLToken("Test2", PLTokenTypes.SYMBOL);
            Sentence atomicSentence1 = new Sentence(symbol1);
            Sentence atomicSentence2 = new Sentence(symbol2);
            complexSentence = new Sentence(atomicSentence1, Sentence.LogicOperatorTypes.And, atomicSentence2);
            Assert.AreEqual(complexSentence.Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(complexSentence.FirstSentence.Symbol.TokenName, "test1");
            Assert.AreEqual(complexSentence.SecondSentence.Symbol.TokenName, "test2");
            Assert.AreEqual(complexSentence.LogicOperator, Sentence.LogicOperatorTypes.And);

            // test that None is not a legal type of operator
            errorThrown = false;
            try
            {
                complexSentence = new Sentence(symbol1, Sentence.LogicOperatorTypes.None, symbol2);
            }
            catch
            {
                errorThrown = true;
            }
            Assert.IsTrue(errorThrown);

            errorThrown = false;
            try
            {
                complexSentence = new Sentence(atomicSentence1, Sentence.LogicOperatorTypes.None, atomicSentence2);
            }
            catch
            {
                errorThrown = true;
            }
            Assert.IsTrue(errorThrown);


        }


        [TestMethod]
        public void TestGetSymbolListSentence()
        {

            PLParser parser = new PLParser();
            Sentence aSentence;
            SymbolList list;
            LogicSymbol symbol;

            parser.SetInput("J OR ~B AND (G OR D) OR F => E OR C AND I OR ~(H => A)");
            // get first sentence "a"
            aSentence = parser.ParseSentence();
            list = aSentence.GetSymbolList();
            list.Sort();

            Assert.AreEqual(list.Count(), 10);
            symbol = list.GetSymbol(0);
            Assert.AreEqual(symbol.SymbolName, "a");
            symbol = list.GetSymbol(1);
            Assert.AreEqual(symbol.SymbolName, "b");
            symbol = list.GetSymbol(2);
            Assert.AreEqual(symbol.SymbolName, "c");
            symbol = list.GetSymbol(3);
            Assert.AreEqual(symbol.SymbolName, "d");
            symbol = list.GetSymbol(4);
            Assert.AreEqual(symbol.SymbolName, "e");
            symbol = list.GetSymbol(5);
            Assert.AreEqual(symbol.SymbolName, "f");
            symbol = list.GetSymbol(6);
            Assert.AreEqual(symbol.SymbolName, "g");
            symbol = list.GetSymbol(7);
            Assert.AreEqual(symbol.SymbolName, "h");
            symbol = list.GetSymbol(8);
            Assert.AreEqual(symbol.SymbolName, "i");
            symbol = list.GetSymbol(9);
            Assert.AreEqual(symbol.SymbolName, "j");

            // check for error
            bool errorThrown = false;
            try
            {
                symbol = list.GetSymbol(10);
            }
            catch
            {
                errorThrown = true;
            }
            Assert.IsTrue(errorThrown);

        }



        [TestMethod]
        public void TestSentenceStringCreation()
        {
            // this test will test creation of a Sentence using a string describing propositional logic
            // this will ensure the static parser building into the Sentence class really works
            Sentence complexSentence = new Sentence("Test1 and Test2");
            Assert.AreEqual(complexSentence.Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(complexSentence.FirstSentence.Symbol.TokenName, "test1");
            Assert.AreEqual(complexSentence.SecondSentence.Symbol.TokenName, "test2");
            Assert.AreEqual(complexSentence.LogicOperator, Sentence.LogicOperatorTypes.And);


            // test attempting to create an invalid Sentence via the parser -- i.e. one with a new line 
            bool errorThrown = false;
            try
            {
                string input;
                input = "A";
                input = input + "\n";
                input = input + "B";
                input = input + "\n";
                input = input + "A AND B => L"; 
                PLToken symbol1 = new PLToken("and", PLTokenTypes.AND);
                PLToken symbol2 = new PLToken("Test2", PLTokenTypes.SYMBOL);
                complexSentence = new Sentence(input);
            }
            catch
            {
                errorThrown = true;
            }
            Assert.IsTrue(errorThrown);
        }




        [TestMethod]
        public void TestBasicEvaluateSentence()
        {

            PLParser parser = new PLParser();
            Sentence sentence;
            SymbolList list;

            // (a and ~b) and (c or d) and e
            parser.SetInput("A AND ~B AND (C OR D) AND E");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.Sort();
            
            // test true
            list.SetValue("a", true);
            list.SetValue("b", false);
            list.SetValue("c", true);
            list.SetValue("d", false);
            list.SetValue("e", true);

            Assert.IsTrue(sentence.IsSentenceTrue(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.True);

            // test false
            list.SetValue("a", true);
            list.SetValue("b", true);
            list.SetValue("c", false);
            list.SetValue("d", true);
            list.SetValue("e", true);

            Assert.IsFalse(sentence.IsSentenceTrue(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.False);

            // test atomic
            // one symbol
            parser.SetInput("a");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.SetValue("A", true);
            Assert.IsTrue(sentence.IsSentenceTrue(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.True);

            // negated symbol
            parser.SetInput("~A");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.SetValue("a", true);
            Assert.IsTrue(sentence.IsSentenceFalse(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.False);

            // double negated symbol
            parser.SetInput("~~A");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.SetValue("a", true);
            Assert.IsTrue(sentence.IsSentenceTrue(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.True);

        }


        [TestMethod]
        public void TestAndEvaluateSentence()
        {
            // now do detailed tests on evalute sentence, trying each combination of AND

            PLParser parser = new PLParser();
            Sentence sentence;
            SymbolList list;            

            // a and b - both true
            parser.SetInput("a and b");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.SetValue("a", true);
            list.SetValue("b", true);
            Assert.IsTrue(sentence.IsSentenceTrue(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.True);

            // a and b - one true
            parser.SetInput("a and b");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.SetValue("a", true);
            list.SetValue("b", false);
            Assert.IsTrue(sentence.IsSentenceFalse(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.False);
            list.SetValue("a", false);
            list.SetValue("b", true);
            Assert.IsTrue(sentence.IsSentenceFalse(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.False);

            // a and b - both false
            parser.SetInput("a and b");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.SetValue("a", false);
            list.SetValue("b", false);
            Assert.IsTrue(sentence.IsSentenceFalse(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.False);

            // ~(a and b) - both true
            parser.SetInput("~(a and b)");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.SetValue("a", true);
            list.SetValue("b", true);
            Assert.IsTrue(sentence.IsSentenceFalse(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.False);

            // ~a and ~b - both true
            parser.SetInput("~a and ~b");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.SetValue("a", true);
            list.SetValue("b", true);
            Assert.IsTrue(sentence.IsSentenceFalse(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.False);

            // ~(a and b) - both false
            parser.SetInput("~(a and b)");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.SetValue("a", false);
            list.SetValue("b", false);
            Assert.IsTrue(sentence.IsSentenceTrue(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.True);

            // ~a and ~b - both false
            parser.SetInput("~a and ~b");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.SetValue("a", false);
            list.SetValue("b", false);
            Assert.IsTrue(sentence.IsSentenceTrue(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.True);

            // ~(a and b) - one true
            parser.SetInput("~(a and b)");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.SetValue("a", true);
            list.SetValue("b", false);
            Assert.IsTrue(sentence.IsSentenceTrue(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.True);
            list.SetValue("a", false);
            list.SetValue("b", true);
            Assert.IsTrue(sentence.IsSentenceTrue(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.True);

            // ~a and ~b - one true
            parser.SetInput("~a and ~b");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.SetValue("a", true);
            list.SetValue("b", false);
            Assert.IsTrue(sentence.IsSentenceFalse(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.False);
            list.SetValue("a", false);
            list.SetValue("b", true);
            Assert.IsTrue(sentence.IsSentenceFalse(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.False);

        }


        [TestMethod]
        public void TestOrEvaluateSentence()
        {
            // now do detailed tests on evalute sentence, trying each combination of AND

            PLParser parser = new PLParser();
            Sentence sentence;
            SymbolList list;

            // a or b - both true
            parser.SetInput("a or b");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.SetValue("a", true);
            list.SetValue("b", true);
            Assert.IsTrue(sentence.IsSentenceTrue(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.True);

            // a or b - one true
            parser.SetInput("a or b");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.SetValue("a", true);
            list.SetValue("b", false);
            Assert.IsTrue(sentence.IsSentenceTrue(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.True);
            list.SetValue("a", false);
            list.SetValue("b", true);
            Assert.IsTrue(sentence.IsSentenceTrue(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.True);

            // a or b - both false
            parser.SetInput("a or b");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.SetValue("a", false);
            list.SetValue("b", false);
            Assert.IsTrue(sentence.IsSentenceFalse(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.False);

            // ~(a or b) - both true
            parser.SetInput("~(a or b)");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.SetValue("a", true);
            list.SetValue("b", true);
            Assert.IsTrue(sentence.IsSentenceFalse(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.False);

            // ~a or ~b - both true
            parser.SetInput("~a or ~b");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.SetValue("a", true);
            list.SetValue("b", true);
            Assert.IsTrue(sentence.IsSentenceFalse(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.False);

            // ~(a or b) - both false
            parser.SetInput("~(a or b)");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.SetValue("a", false);
            list.SetValue("b", false);
            Assert.IsTrue(sentence.IsSentenceTrue(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.True);

            // ~a or ~b - both false
            parser.SetInput("~a or ~b");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.SetValue("a", false);
            list.SetValue("b", false);
            Assert.IsTrue(sentence.IsSentenceTrue(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.True);

            // ~(a or b) - one true
            parser.SetInput("~(a or b)");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.SetValue("a", true);
            list.SetValue("b", false);
            Assert.IsTrue(sentence.IsSentenceFalse(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.False);
            list.SetValue("a", false);
            list.SetValue("b", true);
            Assert.IsTrue(sentence.IsSentenceFalse(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.False);

            // ~a or ~b - one true
            parser.SetInput("~a or ~b");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.SetValue("a", true);
            list.SetValue("b", false);
            Assert.IsTrue(sentence.IsSentenceTrue(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.True);
            list.SetValue("a", false);
            list.SetValue("b", true);
            Assert.IsTrue(sentence.IsSentenceTrue(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.True);

        }


        [TestMethod]
        public void TestImpliesEvaluateSentence()
        {
            // now do detailed tests on evalute sentence, trying each combination of AND

            PLParser parser = new PLParser();
            Sentence sentence;
            SymbolList list;

            // a => b - both true
            parser.SetInput("a => b");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.SetValue("a", true);
            list.SetValue("b", true);
            Assert.IsTrue(sentence.IsSentenceTrue(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.True);

            // a => b - both false
            parser.SetInput("a => b");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.SetValue("a", false);
            list.SetValue("b", false);
            Assert.IsTrue(sentence.IsSentenceTrue(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.True);

            // a => b - one true one false
            parser.SetInput("a => b");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.SetValue("a", true);
            list.SetValue("b", false);
            Assert.IsTrue(sentence.IsSentenceFalse(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.False);
            list.SetValue("a", false);
            list.SetValue("b", true);
            Assert.IsTrue(sentence.IsSentenceTrue(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.True);

            // ~(a => b) - both true, but negated
            parser.SetInput("~(a => b)");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.SetValue("a", true);
            list.SetValue("b", true);
            Assert.IsTrue(sentence.IsSentenceFalse(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.False);
        }



        [TestMethod]
        public void TestBiconditionalEvaluateSentence()
        {
            // now do detailed tests on evalute sentence, trying each combination of AND

            PLParser parser = new PLParser();
            Sentence sentence;
            SymbolList list;

            // a <=> b - both true
            parser.SetInput("a <=> b");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.SetValue("a", true);
            list.SetValue("b", true);
            Assert.IsTrue(sentence.IsSentenceTrue(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.True);

            // a <=> b - both false
            parser.SetInput("a <=> b");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.SetValue("a", false);
            list.SetValue("b", false);
            Assert.IsTrue(sentence.IsSentenceTrue(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.True);

            // a <=> b - one true one false
            parser.SetInput("a <=> b");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.SetValue("a", true);
            list.SetValue("b", false);
            Assert.IsTrue(sentence.IsSentenceFalse(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.False);
            list.SetValue("a", false);
            list.SetValue("b", true);
            Assert.IsTrue(sentence.IsSentenceFalse(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.False);

            // ~(a <=> b) - both true, but negated
            parser.SetInput("~(a <=> b)");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.SetValue("a", true);
            list.SetValue("b", true);
            Assert.IsTrue(sentence.IsSentenceFalse(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.False);
        }



        [TestMethod]
        public void TestAreEquivalent()
        {
            // create different but logically equivalent sentences and
            // verify that the AreEquivalent() functions really detect that they are the same

            // static forms first
            Sentence sentence1, sentence2;

            sentence1 = new Sentence("a => b");
            sentence2 = new Sentence("A => B");

            Assert.IsTrue(sentence1.AreEquivalent(sentence2));

            sentence1 = new Sentence("a => b");
            sentence2 = new Sentence("a or b");

            Assert.IsFalse(sentence1.AreEquivalent(sentence2));

            sentence1 = new Sentence("a => b");
            sentence2 = new Sentence("~a or b");

            Assert.IsTrue(sentence1.AreEquivalent(sentence2));


            sentence1 = new Sentence("a <=> b");
            sentence2 = new Sentence("a => b");

            Assert.IsFalse(sentence1.AreEquivalent(sentence2));

            sentence1 = new Sentence("a <=> b");
            sentence2 = new Sentence("(a => b) AND (b => a)");

            Assert.IsTrue(sentence1.AreEquivalent(sentence2));

            sentence1 = new Sentence("a");
            sentence2 = new Sentence("~~a");

            Assert.IsTrue(sentence1.AreEquivalent(sentence2));

            sentence1 = new Sentence("~(~a)");
            sentence2 = new Sentence("a");

            Assert.IsTrue(sentence1.AreEquivalent(sentence2));

            sentence1 = new Sentence("~(a AND b)");
            sentence2 = new Sentence("~a OR ~b");

            Assert.IsTrue(sentence1.AreEquivalent(sentence2));

            sentence1 = new Sentence("~(a OR b)");
            sentence2 = new Sentence("~a and ~b");

            Assert.IsTrue(sentence1.AreEquivalent(sentence2));

            sentence1 = new Sentence("a and b and c");
            sentence2 = new Sentence("c and a and b");

            Assert.IsTrue(sentence1.AreEquivalent(sentence2));

            // now test the object version
            sentence1 = new Sentence("a => b");
            sentence2 = new Sentence("A => B");

            Assert.IsTrue(sentence1.AreEquivalent(sentence2));
            Assert.IsTrue(sentence1.AreEquivalent("a=>b"));

            sentence1 = new Sentence("a <=> b");
            sentence2 = new Sentence("a => b");

            Assert.IsFalse(sentence1.AreEquivalent(sentence2));
            Assert.IsFalse(sentence1.AreEquivalent("a=>b"));

        }

        [TestMethod]
        public void TestEvaluateSentenceUndefined()
        {
            PLParser parser = new PLParser();
            Sentence sentence;
            SymbolList list;

            // baseline: all set to true
            parser.SetInput("a and b and c");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.SetValue("a", LogicSymbolValue.True);
            list.SetValue("b", LogicSymbolValue.True);
            list.SetValue("c", LogicSymbolValue.True);
            Assert.IsTrue(sentence.IsSentenceTrue(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.True);

            // test undefined on AND
            parser.SetInput("a and b and c");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.SetValue("a", LogicSymbolValue.True);
            list.SetValue("b", LogicSymbolValue.Undefined);
            list.SetValue("c", LogicSymbolValue.True);
            Assert.IsFalse(sentence.IsSentenceTrue(list));
            Assert.IsFalse(sentence.IsSentenceFalse(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.Undefined);


            // test undefined on OR: should still return true
            parser.SetInput("a or b or c");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.SetValue("a", LogicSymbolValue.True);
            list.SetValue("b", LogicSymbolValue.Undefined);
            list.SetValue("c", LogicSymbolValue.True);
            Assert.IsTrue(sentence.IsSentenceTrue(list));
            Assert.IsFalse(sentence.IsSentenceFalse(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.True);

            // test not specifying all symbols: should default to undefined
            parser.SetInput("a and b or c");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.SetValue("a", LogicSymbolValue.True);
            list.SetValue("b", LogicSymbolValue.False);
            Assert.IsFalse(sentence.IsSentenceTrue(list));
            Assert.IsFalse(sentence.IsSentenceFalse(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.Undefined);


            // test =>: if first part is false, the statement is considered true
            parser.SetInput("a and b => c");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.SetValue("a", LogicSymbolValue.True);
            list.SetValue("b", LogicSymbolValue.False);
            list.SetValue("c", LogicSymbolValue.Undefined);
            Assert.IsTrue(sentence.IsSentenceTrue(list));
            Assert.IsFalse(sentence.IsSentenceFalse(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.True);

            // test =>: if first part is undefined, it's undefined, unless c is true
            parser.SetInput("a and b => c");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.SetValue("a", LogicSymbolValue.True);
            list.SetValue("b", LogicSymbolValue.Undefined);
            list.SetValue("c", LogicSymbolValue.True);
            Assert.IsTrue(sentence.IsSentenceTrue(list));
            Assert.IsFalse(sentence.IsSentenceFalse(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.True);

            // test =>: if first part is undefined, it's undefined, unless c is true
            parser.SetInput("a and b => c");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.SetValue("a", LogicSymbolValue.True);
            list.SetValue("c", LogicSymbolValue.False);
            Assert.IsFalse(sentence.IsSentenceTrue(list));
            Assert.IsFalse(sentence.IsSentenceFalse(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.Undefined);
            
            // test =>: if first part is false and undefined, it's false (first part) so statement is true
            parser.SetInput("a and b => c");
            sentence = parser.ParseSentence();
            list = sentence.GetSymbolList();
            list.SetValue("a", LogicSymbolValue.False);
            list.SetValue("b", LogicSymbolValue.Undefined);
            list.SetValue("c", LogicSymbolValue.True);
            Assert.IsTrue(sentence.IsSentenceTrue(list));
            Assert.IsFalse(sentence.IsSentenceFalse(list));
            Assert.AreEqual(sentence.EvaluateSentence(list), LogicSymbolValue.True);


        }



        [TestMethod]
        public void TestClone()
        {
            // creates a clone of the current sentence
            Sentence sentence1, clone;
            sentence1 = new Sentence("a or b and c or ~k and ~~x or ~(g=>~h) <=> h and j or k or ~(u or k and ~a)");
            clone = sentence1.Clone();
            Assert.AreEqual(sentence1.ToString(true), clone.ToString(true));
            Assert.AreNotEqual(sentence1, clone);

        }



        [TestMethod]
        public void TestConvertToCNF()
        {
            // This tests the ConvertToCNF() function which transforms a sentence into Conjunctive Normal Form
            Sentence test;


            // atomic sentence test
            test = new Sentence("a");
            test = test.ConvertToCNF();
            Assert.IsTrue(test.AreEquivalent("a"));
            Assert.AreEqual(test.ToString(), "a");


            // simple negation tests
            test = new Sentence("a and b");
            test = test.ConvertToCNF();
            test = new Sentence(test, false);
            test = test.ConvertToCNF(); 
            Assert.IsTrue(test.AreEquivalent("~(a and b)"));
            Assert.AreEqual(test.ToString(), "~a OR ~b");

            test = new Sentence("a or b");
            test = test.ConvertToCNF();
            test = new Sentence(test, false);
            test = test.ConvertToCNF(); 
            Assert.IsTrue(test.AreEquivalent("~(a OR b)"));
            Assert.AreEqual(test.ToString(), "~a AND ~b");

            test = new Sentence("a and b or c");
            test = test.ConvertToCNF();
            test = new Sentence(test, false);
            test = test.ConvertToCNF();
            Assert.IsTrue(test.AreEquivalent("~(a AND b OR C)"));
            Assert.IsTrue(test.AreEquivalent("~((a OR c) AND (b OR c))"));
            Assert.IsTrue(test.AreEquivalent("~(a OR c) OR ~(b OR c)"));
            Assert.IsTrue(test.AreEquivalent("(~a AND ~c) OR (~b AND ~c)"));
            Assert.AreEqual(test.ToString(), "(~b OR ~a) AND (~c OR ~a) AND (~b OR ~c) AND (~c OR ~c)"); // gives an equivalent response, but not the one I'm expecting           
            


            
            // implies test
            test = new Sentence("a and b => c and d");
            test = test.ConvertToCNF();
            Assert.IsTrue(test.AreEquivalent("a and b => c and d"));
            Assert.AreEqual(test.ToString(), "(c OR ~a OR ~b) AND (d OR ~a OR ~b)");            


            // partial biconditional
            test = new Sentence("(~a OR ~b) OR (c AND d)");
            test = test.ConvertToCNF();
            Assert.IsTrue(test.AreEquivalent("a and b => c and d"));
            Assert.IsTrue(test.AreEquivalent("(~a OR ~b) OR (c AND d)"));
            Assert.IsFalse(test.AreEquivalent("c and d => a and b"));
            Assert.AreEqual(test.ToString(), "(c OR ~a OR ~b) AND (d OR ~a OR ~b)");
            

            // partial biconditional
            test = new Sentence("(~c OR ~d) OR (a AND b)");
            test = test.ConvertToCNF();
            Assert.IsTrue(test.AreEquivalent("c and d => a and b"));
            Assert.IsTrue(test.AreEquivalent("(~c OR ~d) OR (a AND b)"));
            Assert.AreEqual(test.ToString(), "(a OR ~c OR ~d) AND (b OR ~c OR ~d)");



            // partial biconditional
            test = new Sentence("((c OR ~a OR ~b) AND (d OR ~a OR ~b)) AND ((a OR ~c OR ~d) AND (b OR ~c OR ~d))");
            test = test.ConvertToCNF();
            Assert.IsTrue(test.AreEquivalent("a and b <=> c and d"));
            Assert.IsTrue(test.AreEquivalent("((c OR ~a OR ~b) AND (d OR ~a OR ~b)) AND ((a OR ~c OR ~d) AND (b OR ~c OR ~d))"));
            Assert.AreEqual(test.ToString(), "(c OR ~a OR ~b) AND (d OR ~a OR ~b) AND (a OR ~c OR ~d) AND (b OR ~c OR ~d)");



            // partial biconditional
            test = new Sentence("(~a OR ~b OR (c AND d))");
            test = test.ConvertToCNF();
            Assert.IsTrue(test.AreEquivalent("a and b => c and d"));
            Assert.IsTrue(test.AreEquivalent("(~a OR ~b) OR (c AND d)"));
            Assert.IsFalse(test.AreEquivalent("c and d => a and b"));
            Assert.AreEqual(test.ToString(), "(c OR ~a OR ~b) AND (d OR ~a OR ~b)");
            

            // partial biconditional
            test = new Sentence("(~c OR ~d OR (a AND b))");
            test = test.ConvertToCNF();
            Assert.IsTrue(test.AreEquivalent("c and d => a and b"));
            Assert.IsTrue(test.AreEquivalent("(~c OR ~d) OR (a AND b)"));
            Assert.AreEqual(test.ToString(), "(a OR ~c OR ~d) AND (b OR ~c OR ~d)");



            // partial biconditional
            test = new Sentence("(~a OR ~b OR (c AND d)) AND ((a OR ~c OR ~d) AND (b OR ~c OR ~d))");
            test = test.ConvertToCNF();
            Assert.IsTrue(test.AreEquivalent("a and b <=> c and d"));
            Assert.IsTrue(test.AreEquivalent("((c OR ~a OR ~b) AND (d OR ~a OR ~b)) AND ((a OR ~c OR ~d) AND (b OR ~c OR ~d))"));
            Assert.AreEqual(test.ToString(), "(c OR ~a OR ~b) AND (d OR ~a OR ~b) AND (a OR ~c OR ~d) AND (b OR ~c OR ~d)");


            // biconditional test
            test = new Sentence("a and b <=> c and d");
            test = test.ConvertToCNF();
            Assert.IsTrue(test.AreEquivalent("a and b <=> c and d"));
            Assert.AreEqual(test.ToString(), "(c OR ~a OR ~b) AND (d OR ~a OR ~b) AND (a OR ~c OR ~d) AND (b OR ~c OR ~d)");


            // test nested conditionals
            test = new Sentence("a and (b => c) and d");
            test = test.ConvertToCNF();
            Assert.IsTrue(test.AreEquivalent("a and (b => c) and d"));            
            Assert.IsTrue(test.AreEquivalent("a and (~b OR c) and d"));
            Assert.AreEqual(test.ToString(), "a AND (~b OR c) AND d");



            // negated literals only test
            test = new Sentence("~(~a and b) or ~(~~(c and ~d and e))");
            test = test.ConvertToCNF();
            Assert.IsTrue(test.AreEquivalent("((a OR ~b) OR ((~c OR d) OR ~e))"));
            Assert.IsTrue(test.AreEquivalent("~(~a and b) or ~(~~(c and ~d and e))"));
            Assert.AreEqual(test.ToString(), "a OR ~b OR ~c OR d OR ~e");


            test = new Sentence("c and (d or e)"); // (a AND b) OR (c AND (d OR e))
            test = test.ConvertToCNF();
            Assert.IsTrue(test.AreEquivalent("(c AND (d OR e))"));
            Assert.IsTrue(test.AreEquivalent("c and (d or e)"));
            Assert.IsTrue(test.AreEquivalent("(c AND (d OR e))"));
            Assert.AreEqual(test.ToString(), "c AND (d OR e)");


            test = new Sentence("a or c and (d or e)"); // (a AND b) OR (c AND (d OR e))
            test = test.ConvertToCNF();
            Assert.IsTrue(test.AreEquivalent("a or (c AND (d OR e))"));
            Assert.IsTrue(test.AreEquivalent("a or c and (d or e)"));
            Assert.IsTrue(test.AreEquivalent("a or (c AND (d OR e))"));
            Assert.AreEqual(test.ToString(), "(c OR a) AND (d OR e OR a)");


            test = new Sentence("(b or c and (d or e)) and ((c OR a) AND (d OR e OR a))");
            Assert.IsTrue(test.AreEquivalent("(a and b) or (c AND (d OR e))"));
            Assert.IsTrue(test.AreEquivalent("a and b or c and (d or e)"));


            test = new Sentence("(b or c and (d or e)) and ((c OR a) AND (d OR e OR a))"); // (a AND b) OR (c AND (d OR e))
            test = test.ConvertToCNF();
            Assert.IsTrue(test.AreEquivalent("(b or c and (d or e)) and ((c OR a) AND (d OR e OR a))"));
            Assert.IsTrue(test.AreEquivalent("a and b or c and (d or e)"));
            Assert.IsTrue(test.AreEquivalent("(a and b) or (c AND (d OR e))"));
            Assert.AreEqual(test.ToString(), "(c OR b) AND (d OR e OR b) AND (c OR a) AND (d OR e OR a)");


            test = new Sentence("a and b or c"); // (a AND b) OR (c AND (d OR e))
            test = test.ConvertToCNF();
            Assert.IsTrue(test.AreEquivalent("(a AND b) OR c"));
            Assert.IsTrue(test.AreEquivalent("a and b or c"));
            Assert.IsTrue(test.AreEquivalent("(a AND b) OR (c)"));
            Assert.AreEqual(test.ToString(), "(a OR c) AND (b OR c)");
            // test negated
            test = new Sentence(test, false);
            test = test.ConvertToCNF();
            Assert.IsTrue(test.AreEquivalent("~((a AND b) OR c)"));
            Assert.IsTrue(test.AreEquivalent("~(a and b or c)"));
            Assert.IsTrue(test.AreEquivalent("~((a AND b) OR (c))"));
            Assert.AreEqual(test.ToString(), "(~b OR ~a) AND (~c OR ~a) AND (~b OR ~c) AND (~c OR ~c)");


            test = new Sentence("a and b or (d or e)"); // (a AND b) OR (c AND (d OR e))
            test = test.ConvertToCNF();
            Assert.IsTrue(test.AreEquivalent("(a AND b) OR (d OR e)"));
            Assert.IsTrue(test.AreEquivalent("a and b or (d OR e)"));
            Assert.AreEqual(test.ToString(), "(a OR d OR e) AND (b OR d OR e)"); 


            test = new Sentence("(a and b) or (c and d)"); // (a AND b) OR (c AND (d OR e))
            test = test.ConvertToCNF();
            Assert.IsTrue(test.AreEquivalent("(a AND b) OR (c AND d)"));
            Assert.IsTrue(test.AreEquivalent("a and b or c and d"));
            Assert.AreEqual(test.ToString(), "(c OR a) AND (d OR a) AND (c OR b) AND (d OR b)"); 
            
            // test negated
            test = new Sentence(test, false);
            test = test.ConvertToCNF();
            Assert.IsTrue(test.AreEquivalent("~((a AND b) OR (c AND d))"));
            Assert.IsTrue(test.AreEquivalent("~(a and b or c and d)"));
            Assert.IsTrue(test.AreEquivalent("(~c AND ~a) OR (~d AND ~a) OR (~c AND ~b) OR (~d AND ~b)"));
            Assert.AreEqual(test.ToString(), "(~d OR ~c OR ~d OR ~c) AND (~b OR ~c OR ~d OR ~c) AND (~d OR ~b OR ~d OR ~c) AND (~b OR ~b OR ~d OR ~c) AND (~d OR ~c OR ~a OR ~c) AND (~b OR ~c OR ~a OR ~c) AND (~d OR ~b OR ~a OR ~c) AND (~b OR ~b OR ~a OR ~c) AND (~d OR ~c OR ~d OR ~a) AND (~b OR ~c OR ~d OR ~a) AND (~d OR ~b OR ~d OR ~a) AND (~b OR ~b OR ~d OR ~a) AND (~d OR ~c OR ~a OR ~a) AND (~b OR ~c OR ~a OR ~a) AND (~d OR ~b OR ~a OR ~a) AND (~b OR ~b OR ~a OR ~a)"); 



            test = new Sentence("a and b or c and d"); // (a AND b) OR (c AND (d OR e))
            test = test.ConvertToCNF();
            Assert.IsTrue(test.AreEquivalent("(a AND b) OR (c AND d)"));
            Assert.IsTrue(test.AreEquivalent("a and b or c and d"));
            Assert.AreEqual(test.ToString(), "(c OR a) AND (d OR a) AND (c OR b) AND (d OR b)"); 


            
            test = new Sentence("a and b or c and (d or e)"); // (a AND b) OR (c AND (d OR e))
            test = test.ConvertToCNF();
            Assert.IsTrue(test.AreEquivalent("(a AND b) OR (c AND (d OR e))"));
            Assert.IsTrue(test.AreEquivalent("a and b or c and (d or e)"));
            Assert.IsTrue(test.AreEquivalent("((a AND b) OR (c AND (d OR e)))"));
            Assert.AreEqual(test.ToString(), "(c OR a) AND (d OR e OR a) AND (c OR b) AND (d OR e OR b)");


            test = new Sentence("(a and b or c and (d or e) or f) or g or h");
            test = test.ConvertToCNF();
            Assert.IsTrue(test.AreEquivalent("((((a AND b) OR (c AND (d OR e)) OR f) OR g) or h)"));
            Assert.IsTrue(test.AreEquivalent("(a and b or c and (d or e) or f) or g or h"));
            Assert.AreEqual(test.ToString(), "(c OR a OR f OR g OR h) AND (d OR e OR a OR f OR g OR h) AND (c OR b OR f OR g OR h) AND (d OR e OR b OR f OR g OR h)");


            // partial negation test
            test = new Sentence("a and b or c and (d or e)");
            test = test.ConvertToCNF();
            Assert.IsTrue(test.AreEquivalent("(a AND b) OR (c AND (d OR e))"));
            Assert.IsTrue(test.AreEquivalent("a and b or c and (d or e)"));
            Assert.IsTrue(test.AreEquivalent("(((a AND b) OR (c AND (d OR e))))"));
            Assert.AreEqual(test.ToString(), "(c OR a) AND (d OR e OR a) AND (c OR b) AND (d OR e OR b)");

            test = new Sentence(test, false);
            test = test.ConvertToCNF();
            Assert.IsTrue(test.AreEquivalent("~(a and b or c and (d or e))"));
            Assert.IsTrue(test.AreEquivalent("~(((a AND b) OR (c AND (d OR e))))"));
            Assert.IsTrue(test.AreEquivalent("(~c AND ~a) OR (~d AND ~e AND ~a) OR (~c AND ~b) OR (~d AND ~e AND ~b)"));
            Assert.IsTrue(test.AreEquivalent("(~d OR ~c OR ~d OR ~c) AND (~e OR ~c OR ~d OR ~c) AND (~b OR ~c OR ~d OR ~c) AND (~d OR ~b OR ~d OR ~c) AND (~e OR ~b OR ~d OR ~c) AND (~b OR ~b OR ~d OR ~c) AND (~d OR ~c OR ~e OR ~c) AND (~e OR ~c OR ~e OR ~c) AND (~b OR ~c OR ~e OR ~c) AND (~d OR ~b OR ~e OR ~c) AND (~e OR ~b OR ~e OR ~c) AND (~b OR ~b OR ~e OR ~c) AND (~d OR ~c OR ~a OR ~c) AND (~e OR ~c OR ~a OR ~c) AND (~b OR ~c OR ~a OR ~c) AND (~d OR ~b OR ~a OR ~c) AND (~e OR ~b OR ~a OR ~c) AND (~b OR ~b OR ~a OR ~c) AND (~d OR ~c OR ~d OR ~a) AND (~e OR ~c OR ~d OR ~a) AND (~b OR ~c OR ~d OR ~a) AND (~d OR ~b OR ~d OR ~a) AND (~e OR ~b OR ~d OR ~a) AND (~b OR ~b OR ~d OR ~a) AND (~d OR ~c OR ~e OR ~a) AND (~e OR ~c OR ~e OR ~a) AND (~b OR ~c OR ~e OR ~a) AND (~d OR ~b OR ~e OR ~a) AND (~e OR ~b OR ~e OR ~a) AND (~b OR ~b OR ~e OR ~a) AND (~d OR ~c OR ~a OR ~a) AND (~e OR ~c OR ~a OR ~a) AND (~b OR ~c OR ~a OR ~a) AND (~d OR ~b OR ~a OR ~a) AND (~e OR ~b OR ~a OR ~a) AND (~b OR ~b OR ~a OR ~a)"));
            Assert.AreEqual(test.ToString(), "(~d OR ~c OR ~d OR ~c) AND (~e OR ~c OR ~d OR ~c) AND (~b OR ~c OR ~d OR ~c) AND (~d OR ~b OR ~d OR ~c) AND (~e OR ~b OR ~d OR ~c) AND (~b OR ~b OR ~d OR ~c) AND (~d OR ~c OR ~e OR ~c) AND (~e OR ~c OR ~e OR ~c) AND (~b OR ~c OR ~e OR ~c) AND (~d OR ~b OR ~e OR ~c) AND (~e OR ~b OR ~e OR ~c) AND (~b OR ~b OR ~e OR ~c) AND (~d OR ~c OR ~a OR ~c) AND (~e OR ~c OR ~a OR ~c) AND (~b OR ~c OR ~a OR ~c) AND (~d OR ~b OR ~a OR ~c) AND (~e OR ~b OR ~a OR ~c) AND (~b OR ~b OR ~a OR ~c) AND (~d OR ~c OR ~d OR ~a) AND (~e OR ~c OR ~d OR ~a) AND (~b OR ~c OR ~d OR ~a) AND (~d OR ~b OR ~d OR ~a) AND (~e OR ~b OR ~d OR ~a) AND (~b OR ~b OR ~d OR ~a) AND (~d OR ~c OR ~e OR ~a) AND (~e OR ~c OR ~e OR ~a) AND (~b OR ~c OR ~e OR ~a) AND (~d OR ~b OR ~e OR ~a) AND (~e OR ~b OR ~e OR ~a) AND (~b OR ~b OR ~e OR ~a) AND (~d OR ~c OR ~a OR ~a) AND (~e OR ~c OR ~a OR ~a) AND (~b OR ~c OR ~a OR ~a) AND (~d OR ~b OR ~a OR ~a) AND (~e OR ~b OR ~a OR ~a) AND (~b OR ~b OR ~a OR ~a)");



            // partial negation test
            test = new Sentence("a and b or c and (d or e)");
            test = test.ConvertToCNF();
            Assert.IsTrue(test.AreEquivalent("(a AND b) OR (c AND (d OR e))"));
            Assert.IsTrue(test.AreEquivalent("a and b or c and (d or e)"));
            Assert.IsTrue(test.AreEquivalent("(((a AND b) OR (c AND (d OR e))))"));
            Assert.AreEqual(test.ToString(), "(c OR a) AND (d OR e OR a) AND (c OR b) AND (d OR e OR b)");

            test = new Sentence(test, false);
            test = test.ConvertToCNF();
            Assert.IsTrue(test.AreEquivalent("~(a and b or c and (d or e))"));
            Assert.IsTrue(test.AreEquivalent("~(((a AND b) OR (c AND (d OR e))))"));
            Assert.AreEqual(test.ToString(), "(~d OR ~c OR ~d OR ~c) AND (~e OR ~c OR ~d OR ~c) AND (~b OR ~c OR ~d OR ~c) AND (~d OR ~b OR ~d OR ~c) AND (~e OR ~b OR ~d OR ~c) AND (~b OR ~b OR ~d OR ~c) AND (~d OR ~c OR ~e OR ~c) AND (~e OR ~c OR ~e OR ~c) AND (~b OR ~c OR ~e OR ~c) AND (~d OR ~b OR ~e OR ~c) AND (~e OR ~b OR ~e OR ~c) AND (~b OR ~b OR ~e OR ~c) AND (~d OR ~c OR ~a OR ~c) AND (~e OR ~c OR ~a OR ~c) AND (~b OR ~c OR ~a OR ~c) AND (~d OR ~b OR ~a OR ~c) AND (~e OR ~b OR ~a OR ~c) AND (~b OR ~b OR ~a OR ~c) AND (~d OR ~c OR ~d OR ~a) AND (~e OR ~c OR ~d OR ~a) AND (~b OR ~c OR ~d OR ~a) AND (~d OR ~b OR ~d OR ~a) AND (~e OR ~b OR ~d OR ~a) AND (~b OR ~b OR ~d OR ~a) AND (~d OR ~c OR ~e OR ~a) AND (~e OR ~c OR ~e OR ~a) AND (~b OR ~c OR ~e OR ~a) AND (~d OR ~b OR ~e OR ~a) AND (~e OR ~b OR ~e OR ~a) AND (~b OR ~b OR ~e OR ~a) AND (~d OR ~c OR ~a OR ~a) AND (~e OR ~c OR ~a OR ~a) AND (~b OR ~c OR ~a OR ~a) AND (~d OR ~b OR ~a OR ~a) AND (~e OR ~b OR ~a OR ~a) AND (~b OR ~b OR ~a OR ~a)");


            test = new Sentence("(a and b or c and (d or e) or f)");
            test = test.ConvertToCNF();
            Assert.IsTrue(test.AreEquivalent("(a AND b) OR (c AND (d OR e)) OR f"));
            Assert.IsTrue(test.AreEquivalent("(a and b or c and (d or e) or f)"));
            Assert.IsTrue(test.AreEquivalent("(((a AND b) OR (c AND (d OR e))) OR f)"));
            Assert.AreEqual(test.ToString(), "(c OR a OR f) AND (d OR e OR a OR f) AND (c OR b OR f) AND (d OR e OR b OR f)");

            // test negated sentence
            test = new Sentence(test, false);
            test = test.ConvertToCNF();
            Assert.IsTrue(test.AreEquivalent("~(a and b or c and (d or e) or f)"));
            Assert.IsTrue(test.AreEquivalent("~(((a AND b) OR (c AND (d OR e))) OR f)"));
            Assert.IsTrue(test.AreEquivalent("(~c AND ~a AND ~f) OR (~d AND ~e AND ~a AND ~f) OR (~c AND ~b AND ~f) OR (~d AND ~e AND ~b AND ~f)"));
            Assert.AreEqual(test.ToString(), "(~d OR ~c OR ~d OR ~c) AND (~e OR ~c OR ~d OR ~c) AND (~b OR ~c OR ~d OR ~c) AND (~f OR ~c OR ~d OR ~c) AND (~d OR ~b OR ~d OR ~c) AND (~e OR ~b OR ~d OR ~c) AND (~b OR ~b OR ~d OR ~c) AND (~f OR ~b OR ~d OR ~c) AND (~d OR ~f OR ~d OR ~c) AND (~e OR ~f OR ~d OR ~c) AND (~b OR ~f OR ~d OR ~c) AND (~f OR ~f OR ~d OR ~c) AND (~d OR ~c OR ~e OR ~c) AND (~e OR ~c OR ~e OR ~c) AND (~b OR ~c OR ~e OR ~c) AND (~f OR ~c OR ~e OR ~c) AND (~d OR ~b OR ~e OR ~c) AND (~e OR ~b OR ~e OR ~c) AND (~b OR ~b OR ~e OR ~c) AND (~f OR ~b OR ~e OR ~c) AND (~d OR ~f OR ~e OR ~c) AND (~e OR ~f OR ~e OR ~c) AND (~b OR ~f OR ~e OR ~c) AND (~f OR ~f OR ~e OR ~c) AND (~d OR ~c OR ~a OR ~c) AND (~e OR ~c OR ~a OR ~c) AND (~b OR ~c OR ~a OR ~c) AND (~f OR ~c OR ~a OR ~c) AND (~d OR ~b OR ~a OR ~c) AND (~e OR ~b OR ~a OR ~c) AND (~b OR ~b OR ~a OR ~c) AND (~f OR ~b OR ~a OR ~c) AND (~d OR ~f OR ~a OR ~c) AND (~e OR ~f OR ~a OR ~c) AND (~b OR ~f OR ~a OR ~c) AND (~f OR ~f OR ~a OR ~c) AND (~d OR ~c OR ~f OR ~c) AND (~e OR ~c OR ~f OR ~c) AND (~b OR ~c OR ~f OR ~c) AND (~f OR ~c OR ~f OR ~c) AND (~d OR ~b OR ~f OR ~c) AND (~e OR ~b OR ~f OR ~c) AND (~b OR ~b OR ~f OR ~c) AND (~f OR ~b OR ~f OR ~c) AND (~d OR ~f OR ~f OR ~c) AND (~e OR ~f OR ~f OR ~c) AND (~b OR ~f OR ~f OR ~c) AND (~f OR ~f OR ~f OR ~c) AND (~d OR ~c OR ~d OR ~a) AND (~e OR ~c OR ~d OR ~a) AND (~b OR ~c OR ~d OR ~a) AND (~f OR ~c OR ~d OR ~a) AND (~d OR ~b OR ~d OR ~a) AND (~e OR ~b OR ~d OR ~a) AND (~b OR ~b OR ~d OR ~a) AND (~f OR ~b OR ~d OR ~a) AND (~d OR ~f OR ~d OR ~a) AND (~e OR ~f OR ~d OR ~a) AND (~b OR ~f OR ~d OR ~a) AND (~f OR ~f OR ~d OR ~a) AND (~d OR ~c OR ~e OR ~a) AND (~e OR ~c OR ~e OR ~a) AND (~b OR ~c OR ~e OR ~a) AND (~f OR ~c OR ~e OR ~a) AND (~d OR ~b OR ~e OR ~a) AND (~e OR ~b OR ~e OR ~a) AND (~b OR ~b OR ~e OR ~a) AND (~f OR ~b OR ~e OR ~a) AND (~d OR ~f OR ~e OR ~a) AND (~e OR ~f OR ~e OR ~a) AND (~b OR ~f OR ~e OR ~a) AND (~f OR ~f OR ~e OR ~a) AND (~d OR ~c OR ~a OR ~a) AND (~e OR ~c OR ~a OR ~a) AND (~b OR ~c OR ~a OR ~a) AND (~f OR ~c OR ~a OR ~a) AND (~d OR ~b OR ~a OR ~a) AND (~e OR ~b OR ~a OR ~a) AND (~b OR ~b OR ~a OR ~a) AND (~f OR ~b OR ~a OR ~a) AND (~d OR ~f OR ~a OR ~a) AND (~e OR ~f OR ~a OR ~a) AND (~b OR ~f OR ~a OR ~a) AND (~f OR ~f OR ~a OR ~a) AND (~d OR ~c OR ~f OR ~a) AND (~e OR ~c OR ~f OR ~a) AND (~b OR ~c OR ~f OR ~a) AND (~f OR ~c OR ~f OR ~a) AND (~d OR ~b OR ~f OR ~a) AND (~e OR ~b OR ~f OR ~a) AND (~b OR ~b OR ~f OR ~a) AND (~f OR ~b OR ~f OR ~a) AND (~d OR ~f OR ~f OR ~a) AND (~e OR ~f OR ~f OR ~a) AND (~b OR ~f OR ~f OR ~a) AND (~f OR ~f OR ~f OR ~a) AND (~d OR ~c OR ~d OR ~f) AND (~e OR ~c OR ~d OR ~f) AND (~b OR ~c OR ~d OR ~f) AND (~f OR ~c OR ~d OR ~f) AND (~d OR ~b OR ~d OR ~f) AND (~e OR ~b OR ~d OR ~f) AND (~b OR ~b OR ~d OR ~f) AND (~f OR ~b OR ~d OR ~f) AND (~d OR ~f OR ~d OR ~f) AND (~e OR ~f OR ~d OR ~f) AND (~b OR ~f OR ~d OR ~f) AND (~f OR ~f OR ~d OR ~f) AND (~d OR ~c OR ~e OR ~f) AND (~e OR ~c OR ~e OR ~f) AND (~b OR ~c OR ~e OR ~f) AND (~f OR ~c OR ~e OR ~f) AND (~d OR ~b OR ~e OR ~f) AND (~e OR ~b OR ~e OR ~f) AND (~b OR ~b OR ~e OR ~f) AND (~f OR ~b OR ~e OR ~f) AND (~d OR ~f OR ~e OR ~f) AND (~e OR ~f OR ~e OR ~f) AND (~b OR ~f OR ~e OR ~f) AND (~f OR ~f OR ~e OR ~f) AND (~d OR ~c OR ~a OR ~f) AND (~e OR ~c OR ~a OR ~f) AND (~b OR ~c OR ~a OR ~f) AND (~f OR ~c OR ~a OR ~f) AND (~d OR ~b OR ~a OR ~f) AND (~e OR ~b OR ~a OR ~f) AND (~b OR ~b OR ~a OR ~f) AND (~f OR ~b OR ~a OR ~f) AND (~d OR ~f OR ~a OR ~f) AND (~e OR ~f OR ~a OR ~f) AND (~b OR ~f OR ~a OR ~f) AND (~f OR ~f OR ~a OR ~f) AND (~d OR ~c OR ~f OR ~f) AND (~e OR ~c OR ~f OR ~f) AND (~b OR ~c OR ~f OR ~f) AND (~f OR ~c OR ~f OR ~f) AND (~d OR ~b OR ~f OR ~f) AND (~e OR ~b OR ~f OR ~f) AND (~b OR ~b OR ~f OR ~f) AND (~f OR ~b OR ~f OR ~f) AND (~d OR ~f OR ~f OR ~f) AND (~e OR ~f OR ~f OR ~f) AND (~b OR ~f OR ~f OR ~f) AND (~f OR ~f OR ~f OR ~f)");


        }



        [TestMethod]
        public void TestToReadableString()
        {
            
            Sentence test;

            test = new Sentence("a and b and c and d and e");
            Assert.AreEqual(test.ToString(), "a AND b AND c AND d AND e");

            test = new Sentence("a and b and c or d and e");
            Assert.AreEqual(test.ToString(), "(a AND b AND c) OR (d AND e)");

            test = new Sentence("a or b or c or d or e");
            Assert.AreEqual(test.ToString(), "a OR b OR c OR d OR e");

            test = new Sentence("a and b or c and d or e");
            Assert.AreEqual(test.ToString(), "(a AND b) OR (c AND d) OR e");

            test = new Sentence("~a or ~b or c or ~d or ~e");
            Assert.AreEqual(test.ToString(), "~a OR ~b OR c OR ~d OR ~e");

            test = new Sentence("~a or ~(b or c) or ~(d or ~e)");
            Assert.AreEqual(test.ToString(), "~a OR ~(b OR c) OR ~(d OR ~e)");

            test = new Sentence("~a and ~(b or c) OR ~(d AND ~e)");
            Assert.AreEqual(test.ToString(), "(~a AND ~(b OR c)) OR ~(d AND ~e)");

            test = new Sentence("~a or ~(b or c) or ~(d or ~e) => a and c");
            Assert.AreEqual(test.ToString(), "~a OR ~(b OR c) OR ~(d OR ~e) => a AND c");

            test = new Sentence("~a or ~(a or b and c => c and d or e) or ~(d or ~e) => a and c");
            Assert.AreEqual(test.ToString(), "~a OR ~(a OR (b AND c) => (c AND d) OR e) OR ~(d OR ~e) => a AND c");

            test = new Sentence("a and b or c and d");
            Assert.AreEqual(test.ToString(true), "((a AND b) OR (c AND d))");
            Assert.AreEqual(test.ToString(), "(a AND b) OR (c AND d)");
            test = new Sentence(test, false);
            Assert.AreEqual(test.ToString(true), "~((a AND b) OR (c AND d))");
            Assert.AreEqual(test.ToString(), "~((a AND b) OR (c AND d))");

            test = new Sentence("a and b or c");
            test = test.ConvertToCNF();
            test = new Sentence(test, false);
            test = test.ConvertToCNF();
            Assert.IsTrue(test.AreEquivalent("~(a AND b OR C)"));
            Assert.IsTrue(test.AreEquivalent("~((a OR c) AND (b OR c))"));
            Assert.IsTrue(test.AreEquivalent("~(a OR c) OR ~(b OR c)"));
            Assert.IsTrue(test.AreEquivalent("(~a AND ~c) OR (~b AND ~c)"));
            Assert.AreEqual(test.ToString(), "(~b OR ~a) AND (~c OR ~a) AND (~b OR ~c) AND (~c OR ~c)");
            Assert.AreEqual(test.ToString(true), "(((~b OR ~a) AND (~c OR ~a)) AND ((~b OR ~c) AND (~c OR ~c)))");


        }

    }
        
}
