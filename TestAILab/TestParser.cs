using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AILab.PropositionalLogic;

namespace TestAILab
{
    [TestClass]
    public class TestParser
    {
        
        [TestMethod]
        public void TestRandomParsing()
        {

            Sentence aSentence;
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
            input = input + "(T and Z) => Q or Z";
            input = input + "\n";
            input = input + "Z <=> K";
            input = input + "\n";
            input = input + "(J) <=> (K)";
            input = input + "\n";
            input = input + "A AND (B OR C) <=> Z OR (Y AND W)";
            input = input + "\n";
            input = input + "~ H AND ~(B OR ~C) => ~Y OR ~ (W AND Z)";
            input = input + "\n";
            input = input + "A AND B AND C";


            PLParser parser = new PLParser(input);
            // get first sentence "a"
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.Type, Sentence.SentenceType.AtomicSentence);
            Assert.AreEqual(aSentence.ToString(true), "a");

            // next: "b"
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.Type, Sentence.SentenceType.AtomicSentence);
            Assert.AreEqual(aSentence.ToString(true), "b");

            // Next: A AND B => L
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(aSentence.ToString(true), "((a AND b) => l)");


            // Next: A AND P => L
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(aSentence.ToString(true), "((a AND p) => l)");

            // Next: B AND L => M
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(aSentence.ToString(true), "((b AND l) => m)");

            // Next: L AND M => P
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(aSentence.ToString(true), "((l AND m) => p)");

            // Next: P => Q -- test two atomics with a conditional
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(aSentence.ToString(true), "(p => q)");

            // Next: (T AND Z) => Q OR Z -- test unneeded ()
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(aSentence.ToString(true), "((t AND z) => (q OR z))");

            // Next: Z <=> K -- test biconditional
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(aSentence.ToString(true), "(z <=> k)");

            // Next: (J) <=> (K) -- Test atomic ()
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(aSentence.ToString(true), "(j <=> k)");

            // Next: A AND (B OR C) <=> Z OR (Y AND W) -- Test ()
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(aSentence.ToString(true), "((a AND (b OR c)) <=> (z OR (y AND w)))");

            // Next: ~ H AND ~(B OR ~C) => ~Y OR ~ (W AND Z) -- test NOT
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(aSentence.ToString(true), "((~h AND ~(b OR ~c)) => (~y OR ~(w AND z)))");

            // Next: A AND B AND C -- Test three in a row without ( )
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(aSentence.ToString(true), "((a AND b) AND c)");

            // Nothing left
            aSentence = parser.ParseSentence();
            Assert.IsNull(aSentence);
        }

        [TestMethod]
        public void TestAndParsing()
        {     
            
            PLParser parser = new PLParser();
            Sentence aSentence;

            parser.SetInput("A and B");
            // get first sentence "a"
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.ToString(true), "(a AND b)");


            parser.SetInput("A and B and C");
            // get first sentence "a"
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.ToString(true), "((a AND b) AND c)");

            parser.SetInput("A and B and C and D");
            // get first sentence "a"
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.ToString(true), "(((a AND b) AND c) AND d)");


            parser.SetInput("A and B and C and D => A");
            // get first sentence "a"
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.ToString(true), "((((a AND b) AND c) AND d) => a)");

            parser.SetInput("A and B and C and D => A and B");
            // get first sentence "a"
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.ToString(true), "((((a AND b) AND c) AND d) => (a AND b))");


            parser.SetInput("A and B and C and D => A and B and C");
            // get first sentence "a"
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.ToString(true), "((((a AND b) AND c) AND d) => ((a AND b) AND c))");


            parser.SetInput("A and B and C and D => A and B and C and D");
            // get first sentence "a"
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.ToString(true), "((((a AND b) AND c) AND d) => (((a AND b) AND c) AND d))");
            
        }


        [TestMethod]
        public void TestOrParsing()
        {
            PLParser parser = new PLParser();
            Sentence aSentence;

            parser.SetInput("A OR B");
            // get first sentence "a"
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.ToString(true), "(a OR b)");
          
            parser.SetInput("A OR B OR C");
            // get first sentence "a"
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.ToString(true), "((a OR b) OR c)");

            parser.SetInput("A OR B OR C OR D");
            // get first sentence "a"
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.ToString(true), "(((a OR b) OR c) OR d)");


            parser.SetInput("A OR B OR C OR D => A");
            // get first sentence "a"
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.ToString(true), "((((a OR b) OR c) OR d) => a)");

            parser.SetInput("A OR B OR C OR D => A OR B");
            // get first sentence "a"
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.ToString(true), "((((a OR b) OR c) OR d) => (a OR b))");


            parser.SetInput("A OR B OR C OR D => A OR B OR C");
            // get first sentence "a"
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.ToString(true), "((((a OR b) OR c) OR d) => ((a OR b) OR c))");


            parser.SetInput("A OR B OR C OR D => A OR B OR C OR D");
            // get first sentence "a"
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.ToString(true), "((((a OR b) OR c) OR d) => (((a OR b) OR c) OR d))");

            parser.SetInput("A OR B OR C OR D OR E => A OR B OR C OR D OR E");
            // get first sentence "a"
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.ToString(true), "(((((a OR b) OR c) OR d) OR e) => ((((a OR b) OR c) OR d) OR e))");

        }


        [TestMethod]
        public void TestNotParsing()
        {
            PLParser parser = new PLParser();
            Sentence aSentence;

            parser.SetInput("~a or ~(~b and ~c) and ~d => (~a or ~b and ~c) and ~d and ~(~a<=>~b)");
            // get first sentence "a"
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.ToString(true), "((~a OR (~(~b AND ~c) AND ~d)) => (((~a OR (~b AND ~c)) AND ~d) AND ~(~a <=> ~b)))");

            parser.SetInput("~~~a"); 
            // get first sentence "a"
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.ToString(true), "~~~a");

        }


        [TestMethod]
        public void TestAndOrParsing()
        {
            PLParser parser = new PLParser();
            Sentence aSentence;
            
            parser.SetInput("A OR B AND C");
            // get first sentence "a"
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.ToString(true), "(a OR (b AND c))");

            
            parser.SetInput("A OR B AND C OR D");
            // get first sentence "a"
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.ToString(true), "((a OR (b AND c)) OR d)"); // instead it gets: (a OR ((b AND c) OR d))

            
            parser.SetInput("A OR B AND C OR D AND E");
            // get first sentence "a"
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.ToString(true), "((a OR (b AND c)) OR (d AND e))");             
           
            
            // now reverse everything

            parser.SetInput("A AND B OR C");
            // get first sentence "a"
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.ToString(true), "((a AND b) OR c)");


            parser.SetInput("A AND B OR C AND D");
            // get first sentence "a"
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.ToString(true), "((a AND b) OR (c AND d))"); // instead it gets: (a OR ((b AND c) OR d))

            parser.SetInput("A AND B OR C AND D OR E");
            // get first sentence "a"
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.ToString(true), "(((a AND b) OR (c AND d)) OR e)");             
            
        }


        [TestMethod]
        public void TestParenParsing()
        {
            
            PLParser parser = new PLParser();
            Sentence aSentence;
            
            parser.SetInput("a and (b and c) and d => (a and b and c) and d");
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.ToString(true), "(((a AND (b AND c)) AND d) => (((a AND b) AND c) AND d))");

            parser.SetInput("a or (b or c) or d => (a or b or c) or d");
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.ToString(true), "(((a OR (b OR c)) OR d) => (((a OR b) OR c) OR d))");

            parser.SetInput("a and (b or c) and d => ((a or b) and c) and d");
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.ToString(true), "(((a AND (b OR c)) AND d) => (((a OR b) AND c) AND d))");

            parser.SetInput("a and (b or c) and d => (a=>b) and ~(c and ~d)");
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.ToString(true), "(((a AND (b OR c)) AND d) => ((a => b) AND ~(c AND ~d)))");

            // test () around a single symbol - this should work even though it's weird
            parser.SetInput("(a) or b");
            aSentence = parser.ParseSentence();
            Assert.AreEqual(aSentence.ToString(true), "(a OR b)");

        }

        [TestMethod]
        public void TestParseInput()
        {
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
            Assert.AreEqual(sentenceList.Count(), 7);
            Assert.AreEqual(sentenceList[0].Type, Sentence.SentenceType.AtomicSentence);
            Assert.AreEqual(sentenceList[0].ToString(true), "a");
            Assert.AreEqual(sentenceList[1].Type, Sentence.SentenceType.AtomicSentence);
            Assert.AreEqual(sentenceList[1].ToString(true), "b");
            Assert.AreEqual(sentenceList[2].Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(sentenceList[2].ToString(true), "((a AND b) => l)");
            Assert.AreEqual(sentenceList[3].Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(sentenceList[3].ToString(true), "((a AND p) => l)");
            Assert.AreEqual(sentenceList[4].Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(sentenceList[4].ToString(true), "((b AND l) => m)");
            Assert.AreEqual(sentenceList[5].Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(sentenceList[5].ToString(true), "((l AND m) => p)");
            Assert.AreEqual(sentenceList[6].Type, Sentence.SentenceType.ComplexSentence);
            Assert.AreEqual(sentenceList[6].ToString(true), "(p => q)");

            // test bounds
            bool errorThrown = false;
            try
            {
                string s = sentenceList[7].ToString(true);
            }
            catch
            {
                errorThrown = true;
            }
            Assert.IsTrue(errorThrown);

            // prove that you can then do it again with reseting input but without creating a new parser
            input = "A";
            input = input + "\n";
            input = input + "B";
            parser.SetInput(input); 
            sentenceList = parser.ParseInput();
            Assert.AreEqual(sentenceList.Count(), 2);
            Assert.AreEqual(sentenceList[0].Type, Sentence.SentenceType.AtomicSentence);
            Assert.AreEqual(sentenceList[0].ToString(true), "a");
            Assert.AreEqual(sentenceList[1].Type, Sentence.SentenceType.AtomicSentence);
            Assert.AreEqual(sentenceList[1].ToString(true), "b");

            // test bounds
            errorThrown = false;
            try
            {
                string s = sentenceList[2].ToString(true);
            }
            catch
            {
                errorThrown = true;
            }
            Assert.IsTrue(errorThrown);

        }

    }
         
}
