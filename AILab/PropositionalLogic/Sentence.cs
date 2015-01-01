using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AILab.Common;

namespace AILab.PropositionalLogic
{
    
    public class Sentence
    {
        public enum LogicOperatorTypes { None, And, Or, Implies, Biconditional };
        public enum SentenceType { AtomicSentence, ComplexSentence };

        // static parser -- so that Sentence can parse propositional logic text
        private static PLParser parser = new PLParser();

        // properties
        public LogicOperatorTypes LogicOperator { get; set; }
        public bool Negation { get; protected set; }
        public Token Symbol { get; protected set; }
        public SentenceType Type { get; protected set; }
        private Sentence firstSentence;
        protected Sentence ParentSentence { get; set; }
        public Sentence FirstSentence
        {
            get
            {
                return firstSentence;
            }

            protected set
            {
                firstSentence = value;
                if (value != null)
                    firstSentence.ParentSentence = this;
            }
        }
        private Sentence secondSentence;
        public Sentence SecondSentence
        {
            get
            {
                return secondSentence;
            }

            protected set
            {
                secondSentence = value;
                if (value != null)
                    secondSentence.ParentSentence = this;

            }
        }

        // Create an atomic sentence
        public Sentence(Token aToken)
        {
            if (aToken != null && aToken.TokenType == (int)PLTokenTypes.SYMBOL)
            {
                LogicOperator = LogicOperatorTypes.None;
                Negation = false;
                Symbol = aToken;
                Type = SentenceType.AtomicSentence;
                FirstSentence = null;
                SecondSentence = null;
            }
            else
                throw new Exception("You must pass a non-null Token of type Symbol to a Sentence");
        }


        // Create an Sentence that contains another Sentence -- this is to allow a Sub Sentence to contain a full sentence
        public Sentence(Sentence aSentence)
        {
            if (aSentence != null)
            {
                LogicOperator = aSentence.LogicOperator;
                Negation = aSentence.Negation;
                Symbol = aSentence.Symbol;
                Type = aSentence.Type;
                FirstSentence = aSentence.FirstSentence;
                SecondSentence = aSentence.SecondSentence;
            }
            else
                throw new Exception("Illegal parameter. Sentence cannot be null.");
        }


        // Create a Negated Symbol
        public Sentence(Token aToken, bool not): this(aToken)
        {
            if (not == false)
                Negation = true;
        }


        // Create a Negated Sentence
        public Sentence(Sentence aSentence, bool not)
        {
            if (aSentence == null)
                throw new Exception("Illegal parameter. Sentence cannot be null.");

            // if we are asking to negate this sentence...
            if (not == false)
            {                
                if (aSentence.Negation == true)
                {
                    // if this sentence we wish to negate is already a negated sentence, 
                    // then we make it a sub-sentence and thus a double (or more) negation
                    Negation = true;
                    LogicOperator = LogicOperatorTypes.None;
                    Symbol = null;
                    Type = SentenceType.ComplexSentence;
                    FirstSentence = aSentence;
                    SecondSentence = null;
                }
                else // if (aSentence.Negation == false)
                {
                    // but if this sentence is not a negated sentence, then we really just want to clone it
                    // plus add the negation
                    Negation = true;
                    LogicOperator = aSentence.LogicOperator;
                    Symbol = aSentence.Symbol;
                    Type = aSentence.Type;
                    FirstSentence = aSentence.FirstSentence;
                    SecondSentence = aSentence.SecondSentence;
                }
            }
            else
            {
                // if we are not asking to negate this sentence, just make it identical to the one passed in
                // i.e. clone it.
                LogicOperator = aSentence.LogicOperator;
                Negation = aSentence.Negation;
                Symbol = aSentence.Symbol;
                Type = aSentence.Type;
                FirstSentence = aSentence.FirstSentence;
                SecondSentence = aSentence.SecondSentence;          
            }
        }


        // Create a complex Sentence out of two Tokens
        public Sentence(Token token1, LogicOperatorTypes aOperator, Token token2)
        {
            if (token1.TokenType == (int)PLTokenTypes.SYMBOL && token2.TokenType == (int)PLTokenTypes.SYMBOL)
            {
                // None is not a legal operator
                if (!(aOperator == LogicOperatorTypes.None || token1 == null || token2 == null))
                {
                    LogicOperator = aOperator;
                    Negation = false;
                    Symbol = null;
                    Type = SentenceType.ComplexSentence;
                    FirstSentence = new Sentence(token1);
                    SecondSentence = new Sentence(token2);
                }
                else
                    throw new Exception("Illegal parameters. Operator cannot be 'None' and Tokens cannot be null.");
            }
            else
                throw new Exception("You must pass a Token of type Symbol to a Sentence");

        }


        // Create a complex Sentence out of two Symbols
       public Sentence(Sentence sentence1, LogicOperatorTypes aOperator, Sentence sentence2)
        {
            // None is not a legal operator
            if ( !(aOperator == LogicOperatorTypes.None || sentence1 == null || sentence2 == null) )
            {
                LogicOperator = aOperator;
                Negation = false;
                Symbol = null;
                Type = SentenceType.ComplexSentence;
                FirstSentence = sentence1;
                SecondSentence = sentence2;
            }
            else
                throw new Exception("Illegal parameters. Operator cannot be 'None' and Sentences cannot be null.");
        }



        // Create a sentence out of propositional logic text via the parser
        public Sentence(string inputString)
        {
            if (inputString == null || inputString == "")
                throw new Exception("Illegal parameter. The input string must not be null or empty.");

            // rule -- this must be a single propositional logic statement without new lines
            parser.SetInput(inputString);
            Sentence aSentence = parser.ParseSentence();
            if (parser.ParseSentence() != null)
                throw new Exception("Sentence(string) constructor only works with a single propositional logic statement.");
            else
            {
                LogicOperator = aSentence.LogicOperator;
                Negation = aSentence.Negation;
                Symbol = aSentence.Symbol;
                Type = aSentence.Type;
                FirstSentence = aSentence.FirstSentence;
                SecondSentence = aSentence.SecondSentence;          
            }
        }




        // Convert the Sentence to a String but (if fullParenthesis == true) with the full () to be sure
        // the sentence is using the correct order of operations
        public string ToString(bool fullParenthesis)
        {

            if (fullParenthesis == true)
            {
                string returnVal = "";

                // take care of negation first
                if (Negation == true)
                    returnVal = "~";

                // then handle the rest, i.e. atomic vs. complex
                if (Type == SentenceType.AtomicSentence)
                    returnVal = returnVal + Symbol.TokenName;
                else if (Type == SentenceType.ComplexSentence)
                {
                    if (LogicOperator == LogicOperatorTypes.None)
                        returnVal = returnVal + FirstSentence.ToString(true);
                    else
                        returnVal = returnVal + "(" + FirstSentence.ToString(true) + " " + LogicalOperatorToString(LogicOperator) + " " +
                            SecondSentence.ToString(true) + ")";
                }
                else
                    throw new Exception("Error Converting Sentence to a string. Illegal SentenceType.");

                return returnVal;
            }
            else
                return ToString();
        }


        // Convert the Sentence to a String but drop extra "()" to make it more readable
        public override string ToString()
        {
            Sentence sentence = this;
            string returnVal = "";

            if (sentence.Type == SentenceType.AtomicSentence)
                returnVal = sentence.ToString(true);
            else // if (sentence.Type == SentenceType.ComplexSentence)
            {
                // handle negated sentences
                if (sentence.Negation == true)
                {
                    if (sentence.LogicOperator == LogicOperatorTypes.None)
                        returnVal = "~(" + sentence.FirstSentence.ToString() + ")";
                    else // if we have some operator at this node, and thus two sentences
                    {

                        returnVal = "~(";

                        // do we need double ()? 
                        if (sentence.LogicOperator != sentence.FirstSentence.LogicOperator &&
                            sentence.FirstSentence.Type != SentenceType.AtomicSentence && sentence.FirstSentence.Negation == false && 
                            sentence.LogicOperator != LogicOperatorTypes.Biconditional && sentence.LogicOperator != LogicOperatorTypes.Implies)
                            returnVal = returnVal + "(" + sentence.FirstSentence.ToString() + ")";
                        else
                            returnVal = returnVal + sentence.FirstSentence.ToString();

                        returnVal = returnVal + " " + LogicalOperatorToString(sentence.LogicOperator) + " ";

                        if (sentence.LogicOperator != sentence.SecondSentence.LogicOperator &&
                            sentence.SecondSentence.Type != SentenceType.AtomicSentence && sentence.SecondSentence.Negation == false &&
                            sentence.LogicOperator != LogicOperatorTypes.Biconditional && sentence.LogicOperator != LogicOperatorTypes.Implies)
                            returnVal = returnVal + "(" + sentence.SecondSentence.ToString() + ")";
                        else
                            returnVal = returnVal + sentence.SecondSentence.ToString();

                        
                        returnVal = returnVal + ")";
                    }
                }
                else // if this is a complex sentence that is not negated
                {
                    // The key here is to determine whether or not to include () 
                    // essentially, we want to include them if we are 'changing operators' between levels
                    // but there some exceptions: we don't want to if the change of operator happens with a negation
                    // since the negation will eventually get () anyhow. This avoids double ().
                    // Also, we don't want to include () if the current node is a conditional (implies or biconditional)
                    // since this is pretty obvious when looking at it where the order of operations is
                    if (sentence.LogicOperator == LogicOperatorTypes.Implies || sentence.LogicOperator == LogicOperatorTypes.Biconditional)
                    {
                        returnVal = sentence.FirstSentence.ToString() + " " + LogicalOperatorToString(sentence.LogicOperator) + 
                            " " + sentence.SecondSentence.ToString();
                    }
                    else if (sentence.LogicOperator == LogicOperatorTypes.And || sentence.LogicOperator == LogicOperatorTypes.Or)
                    {
                        // first sentence
                        if (sentence.LogicOperator != sentence.FirstSentence.LogicOperator && 
                            sentence.FirstSentence.Type != SentenceType.AtomicSentence && sentence.FirstSentence.Negation == false)
                            returnVal = "(" + sentence.FirstSentence.ToString() + ")";                                                
                        else // if there is no change of operator (or it's atomic) or there is but it's a negation, so no ()
                            returnVal = sentence.FirstSentence.ToString();

                        // add operator
                        returnVal = returnVal + " " + LogicalOperatorToString(sentence.LogicOperator) + " ";

                        // second sentence
                        if (sentence.LogicOperator != sentence.SecondSentence.LogicOperator &&
                            sentence.SecondSentence.Type != SentenceType.AtomicSentence && sentence.SecondSentence.Negation == false)
                            returnVal = returnVal + "(" + sentence.SecondSentence.ToString() + ")";                                                
                        else // if there is no change of operator or there is but it's a negation, so no ()
                            returnVal = returnVal + sentence.SecondSentence.ToString();

                    }
                }
            }

            return returnVal;
        }





        /// <summary>
        /// GetSymbolList(): This function returns an array of symbols contained in this sentence
        /// </summary>
        /// <returns>PLSymbolList array listing all symbols contained in this sentence, all set to unassigned</returns>
        public SymbolList GetSymbolList()
        {
            // traverse the sentence tree and find each symbol
            SymbolList tempSymbolList = new SymbolList();


            CollectSymbols(tempSymbolList, this);

            return tempSymbolList;

        }





        /// <summary>
        /// LogicSymbolValue EvaluateSentence(SymbolList model): This function takes a list of symbols with assigned values
        /// and then returns if this sentence is true, false, or undefined given those values.
        /// This function does not infer anything. So it will return "undefined" unless every symbol in the sentence
        /// is assigned a value. 
        /// </summary>
        /// <param name="symbolList">The SymbolList of all symbols with known values</param>
        /// <returns></returns>
        public LogicSymbolValue EvaluateSentence(SymbolList model)
        {
            // the goal is to traverse the sentence using the symbolList and determine if the sentence is true, false, 
            // or undefined (i.e. can't be determined via this symbol list)
            
            return TraverseAndEvaluateSentence(model);
        }



        /// <summary>
        /// bool EvaluateSentence(SymbolList model): This function takes a list of symbols with assigned values
        /// and then returns if this sentence is true or false, with undefined being treated as false. 
        /// </summary>
        /// <param name="symbolList">The SymbolList of all symbols with known values</param>
        /// <returns></returns>
        public bool IsSentenceTrue(SymbolList model)
        {
            // the goal is to traverse the sentence using the symbolList and determine if the sentence is true or false
            return (TraverseAndEvaluateSentence(model) == LogicSymbolValue.True);
        }



        public bool IsSentenceFalse(SymbolList model)
        {
            // the goal is to traverse the sentence using the symbolList and determine if the sentence is true or false
            return (TraverseAndEvaluateSentence(model) == LogicSymbolValue.False);
        }



        // This private function does all the recursive work for EvaluteSentence(PLSymbolList)
        private LogicSymbolValue TraverseAndEvaluateSentence(SymbolList model)
        {
            LogicSymbolValue evaluate;

            if (Type == SentenceType.AtomicSentence)
            {
                evaluate = model.GetValue(Symbol.TokenName);
            }
            else // if (Type == SentenceType.ComplexSentence)
            {
                LogicSymbolValue subEvaluate1, subEvaluate2, subEvaluate3, subEvaluate4;

                if (LogicOperator == LogicOperatorTypes.None)
                    evaluate = FirstSentence.TraverseAndEvaluateSentence(model);
                else if (LogicOperator == LogicOperatorTypes.And)
                {
                    subEvaluate1 = FirstSentence.TraverseAndEvaluateSentence(model); 
                    subEvaluate2 = SecondSentence.TraverseAndEvaluateSentence(model);
                    if (subEvaluate1 == LogicSymbolValue.True && subEvaluate2 == LogicSymbolValue.True)
                        evaluate = LogicSymbolValue.True;
                    else if (subEvaluate1 == LogicSymbolValue.Undefined || subEvaluate2 == LogicSymbolValue.Undefined)
                        evaluate = LogicSymbolValue.Undefined;
                    else // if one or both of them are false
                        evaluate = LogicSymbolValue.False;
                }
                else if (LogicOperator == LogicOperatorTypes.Or)
                {
                    subEvaluate1 = FirstSentence.TraverseAndEvaluateSentence(model);
                    subEvaluate2 = SecondSentence.TraverseAndEvaluateSentence(model);
                    if (subEvaluate1 == LogicSymbolValue.True || subEvaluate2 == LogicSymbolValue.True)
                        evaluate = LogicSymbolValue.True;
                    else if (subEvaluate1 == LogicSymbolValue.False && subEvaluate2 == LogicSymbolValue.False)
                        evaluate = LogicSymbolValue.False;
                    else // if in an undefined state: both undefined or one undefined and one false
                        evaluate = LogicSymbolValue.Undefined;
                }
                else if (LogicOperator == LogicOperatorTypes.Implies)
                {
                    subEvaluate1 = FirstSentence.TraverseAndEvaluateSentence(model);
                    subEvaluate2 = SecondSentence.TraverseAndEvaluateSentence(model);
                    if (subEvaluate1 == LogicSymbolValue.False || subEvaluate2 == LogicSymbolValue.True) // a => b means ~a OR b
                        evaluate = LogicSymbolValue.True; 
                    else if (subEvaluate1 == LogicSymbolValue.True && subEvaluate2 == LogicSymbolValue.False) // ~(~a or b) = a and ~b
                        evaluate = LogicSymbolValue.False;
                    else // if in an undefined state
                        evaluate = LogicSymbolValue.Undefined;
                }
                else if (LogicOperator == LogicOperatorTypes.Biconditional)
                {
                    subEvaluate1 = FirstSentence.TraverseAndEvaluateSentence(model);
                    subEvaluate2 = SecondSentence.TraverseAndEvaluateSentence(model);
                    if (subEvaluate1 == LogicSymbolValue.False || subEvaluate2 == LogicSymbolValue.True) // a => b means ~a OR b
                        subEvaluate3 = LogicSymbolValue.True;
                    else if (subEvaluate1 == LogicSymbolValue.True && subEvaluate2 == LogicSymbolValue.False) // ~(~a or b) = a and ~b
                        subEvaluate3 = LogicSymbolValue.False;
                    else // if in an undefined state
                        subEvaluate3 = LogicSymbolValue.Undefined;

                    if (subEvaluate2 == LogicSymbolValue.False || subEvaluate1 == LogicSymbolValue.True) // a => b means ~a OR b
                        subEvaluate4 = LogicSymbolValue.True;
                    else if (subEvaluate2 == LogicSymbolValue.True && subEvaluate1 == LogicSymbolValue.False) // ~(~a or b) = a and ~b
                        subEvaluate4 = LogicSymbolValue.False;
                    else // if in an undefined state
                        subEvaluate4 = LogicSymbolValue.Undefined;

                    // a <=> b means (~a OR b) AND (~b OR a)
                    if (subEvaluate3 == LogicSymbolValue.True && subEvaluate4 == LogicSymbolValue.True)
                        evaluate = LogicSymbolValue.True;
                    else if (subEvaluate3 == LogicSymbolValue.False || subEvaluate4 == LogicSymbolValue.False)
                        evaluate = LogicSymbolValue.False;
                    else // undefined
                        evaluate = LogicSymbolValue.Undefined;                
                }
                else
                    throw new Exception("Error evalutating Sentence. Illegal operator type.");

            }

            if (Negation == true)
            {
                if (evaluate == LogicSymbolValue.True)
                    return LogicSymbolValue.False;
                else if (evaluate == LogicSymbolValue.False)
                    return LogicSymbolValue.True;
                else
                    return LogicSymbolValue.Undefined;
            }
            else
                return evaluate;

        }




        // TODO: Change this to use DPLL instead? 
        private bool TruthTableCheckAll(Sentence sentence, SymbolList symbols, SymbolList model)
        {
            if (symbols == null || symbols.Count() == 0)
            {
                if (EvaluateSentence(model) == sentence.EvaluateSentence(model))
                    return true;
                else
                    return false;
            }
            else
            {
                string nextSymbol = symbols.GetSymbol(0).SymbolName;
                symbols.Remove(nextSymbol);

                SymbolList copyModel1, copyModel2, copySymbols1, copySymbols2;
                copyModel1 = model.Clone();
                copyModel2 = model.Clone();
                copyModel1.SetValue(nextSymbol, true);
                copyModel2.SetValue(nextSymbol, false);

                if (symbols != null && symbols.Count() > 0)
                {
                    copySymbols1 = symbols.Clone();
                    copySymbols2 = symbols.Clone();
                }
                else
                {
                    copySymbols1 = null;
                    copySymbols2 = null;
                }

                bool check1 = TruthTableCheckAll(sentence, copySymbols1, copyModel1); 
                bool check2 = TruthTableCheckAll(sentence, copySymbols2, copyModel2);


                return check1 && check2;
            }
        }


        public bool AreEquivalent(Sentence sentence)
        {
            // this function creates a truth table for both sentences and sees if they are always equivalent

            // first confirm that they have the same symbols
            SymbolList sl1, sl2;

            sl1 = GetSymbolList();
            sl2 = sentence.GetSymbolList();
            sl1.Sort();
            sl2.Sort();
            if (sl1.Count() == sl2.Count())
                for (int i = 0, count = sl1.Count(); i < count; i++)
                    // abort immediately if we have mismatching symbols
                    if (sl1.GetSymbol(i).SymbolName != sl2.GetSymbol(i).SymbolName)
                        return false;

            // at least the symbols match each other, so we only need to use one of them now
            // create truth table
            return TruthTableCheckAll(sentence, sl1.Clone(), sl1.Clone());
        }


        public bool AreEquivalent(string input)
        {
            // this function creates a truth table for both sentences and sees if they are always equivalent
            Sentence sentence = new Sentence(input);
            return AreEquivalent(sentence);
        }



        public Sentence Clone()
        {
            // TODO -- make this a real clone function that doesn't use a string conversion so I know it will be identical and fast
            // returns a clone of the current sentence
            return new Sentence(this.ToString(true));
        }


        public Sentence ConvertToCNF()
        {
            // This function transforms the sentence into Conjunctive Normal Form
            // CNF is a form made up of Ors connected by ANDs. i.e. (A OR B OR C) AND (D OR E OR F)
            // 3-CNF is the 3-SAT problem
            Sentence sentence = Clone();
            Sentence tempSentence;
            sentence = sentence.TransformConditionals();
            sentence = sentence.TransformNots();
            do
            {
                // call TransformWithDistributiveProperty over and over until no more changes to the sentence take place
                tempSentence = sentence.Clone();
                sentence = sentence.TransformWithDistributeOrs();
            }
            while (tempSentence.ToString() != sentence.ToString());

            return sentence;
        }



        private Sentence TransformConditionals()
        {
            // start with a clone to avoid any side effects
            Sentence sentence = Clone();

            if (sentence.Type == SentenceType.AtomicSentence)
                return sentence;
            else // if (sentence.Type == SentenceType.ComplexSentence)
            {
                if (sentence.LogicOperator == LogicOperatorTypes.Biconditional)
                {
                    Sentence cloneAB, cloneBA, temp;

                    // replace biconditional (a <=> b) with a => b AND b => a

                    // a => b
                    cloneAB = sentence.Clone();
                    cloneAB.LogicOperator = LogicOperatorTypes.Implies;

                    // b => a
                    cloneBA = sentence.Clone();
                    temp = cloneBA.FirstSentence;
                    cloneBA.FirstSentence = cloneBA.SecondSentence;
                    cloneBA.SecondSentence = temp;
                    cloneBA.LogicOperator = LogicOperatorTypes.Implies;

                    cloneAB = cloneAB.TransformConditionals();
                    cloneBA = cloneBA.TransformConditionals();

                    // and
                    sentence.LogicOperator = LogicOperatorTypes.And;
                    sentence.FirstSentence = cloneAB;
                    sentence.SecondSentence = cloneBA;

                    return sentence;
                }
                else if (sentence.LogicOperator == LogicOperatorTypes.Implies)
                {
                    Sentence negFirstSentence;

                    // replace implies (a => b) with ~a OR b

                    negFirstSentence = new Sentence(sentence.FirstSentence, false);
                    negFirstSentence = negFirstSentence.TransformConditionals();
                    sentence.LogicOperator = LogicOperatorTypes.Or;
                    sentence.FirstSentence = negFirstSentence;

                    sentence.SecondSentence = sentence.SecondSentence.TransformConditionals();

                    return sentence;
                }
                else
                {
                    // we have completed the current top node and it's not a conditional
                    // we need to recursively traverse down
                    if (sentence.Type == SentenceType.AtomicSentence)
                        return sentence;
                    else
                    {
                        sentence.FirstSentence = sentence.FirstSentence.TransformConditionals();
                        if (sentence.SecondSentence != null)
                            sentence.SecondSentence = sentence.SecondSentence.TransformConditionals();
                        return sentence;
                    }
                }
            }
        }


        private Sentence TransformNots()
        {
            // start with a clone to avoid any side effects
            Sentence sentence = Clone();

            // CNF requires ~ (not) to appear only in literals
            if (sentence.Type == SentenceType.AtomicSentence)
                return sentence;
            else // if (sentence.Type == SentenceType.ComplexSentence)
            {
                if (sentence.Negation == true)
                {
                    // a Negated sentence has only one sub-sentence. If that sub-sentence is 'atomic' then it's fine as it is
                    if (sentence.Type == SentenceType.AtomicSentence)
                        return sentence;
                    else // if (sentence.FirstSentence.Type == SentenceType.ComplexSentence)
                    {

                        if (sentence.LogicOperator != LogicOperatorTypes.None)
                        {
                            // if this is a complex sentence with an operator, transfer the negation down a level and call recursively
                            sentence.Negation = false;

                            // flip ands and ors
                            if (sentence.LogicOperator == LogicOperatorTypes.And)
                                sentence.LogicOperator = LogicOperatorTypes.Or;
                            else if (sentence.LogicOperator == LogicOperatorTypes.Or)
                                sentence.LogicOperator = LogicOperatorTypes.And;

                            sentence.FirstSentence = sentence.FirstSentence.MoveNotInward().TransformNots();
                            sentence.SecondSentence = sentence.SecondSentence.MoveNotInward().TransformNots();

                            
                        }
                        else // if (sentence.LogicOperator == LogicOperatorTypes.None)
                        {
                            sentence.Negation = false;
                            sentence = sentence.FirstSentence.MoveNotInward().TransformNots();
                        }

                        return sentence;
                    }

                }
            }

            sentence.FirstSentence = sentence.FirstSentence.TransformNots();
            sentence.SecondSentence = sentence.SecondSentence.TransformNots();
            return sentence;
        }


        private Sentence MoveNotInward()
        {
            // start with a clone to avoid any side effects
            Sentence sentence = Clone();

            if (sentence.Negation == true)
                sentence.Negation = false;
            else // if (sentence.Negation == false)
                sentence.Negation = true;
            
            // if we now have a non-negated sentence without an operator, then we need to pull it all up one level
            if (sentence.LogicOperator == LogicOperatorTypes.None && sentence.Negation == false && 
                    sentence.Type == SentenceType.ComplexSentence)
                sentence = sentence.FirstSentence;

            return sentence;
        }


        private Sentence TransformWithDistributeOrs()
        {

            // start with a clone to avoid any side effects
            Sentence sentence = Clone();

            // This function assumes there are no logical operators except AND and OR plus NOT next to only literals
            // Anything else will throw an error
            Sentence OrSubsentence;

            if (sentence.Type == SentenceType.AtomicSentence)
            {
                // if this is atomic, there is nothing else to change on this thread
                return sentence;
            }
            else if (sentence.LogicOperator == LogicOperatorTypes.Or)
            {
                if (sentence.FirstSentence.LogicOperator == LogicOperatorTypes.And)
                {
                    // we have an OR above an AND on the right side
                    // I need to code an identical but reversed version of this for the left side

                    // grab the left side so that we can put it over to the right side
                    OrSubsentence = sentence.SecondSentence.Clone();
                    // drop the left side
                    sentence = sentence.FirstSentence.Clone();
                    // now traverse the AND plus any more beneath it and put the left under each AND clause
                    sentence = sentence.RedistributeOr(OrSubsentence);
                    sentence.FirstSentence = sentence.FirstSentence.TransformWithDistributeOrs();
                    sentence.SecondSentence = sentence.SecondSentence.TransformWithDistributeOrs();
                }
                else if (sentence.SecondSentence.LogicOperator == LogicOperatorTypes.And)
                {
                    // we have an OR above an AND on the right side
                    // I need to code an identical but reversed version of this for the left side

                    // grab the right side so that we can put it over to the right side
                    OrSubsentence = sentence.FirstSentence.Clone();
                    // drop the right side
                    sentence = sentence.SecondSentence.Clone();
                    // now traverse the AND plus any more beneath it and put the left under each AND clause
                    sentence = sentence.RedistributeOr(OrSubsentence);
                    sentence.FirstSentence = sentence.FirstSentence.TransformWithDistributeOrs();
                    sentence.SecondSentence = sentence.SecondSentence.TransformWithDistributeOrs();
                }
                else
                {
                    sentence.FirstSentence = sentence.FirstSentence.TransformWithDistributeOrs();
                    sentence.SecondSentence = sentence.SecondSentence.TransformWithDistributeOrs();
                }
            }
            else if (sentence.LogicOperator == LogicOperatorTypes.And)
            {
                // we are on an AND parent, so descend
                sentence.FirstSentence = sentence.FirstSentence.TransformWithDistributeOrs();
                sentence.SecondSentence = sentence.SecondSentence.TransformWithDistributeOrs();
            }
            else if (sentence.LogicOperator == LogicOperatorTypes.None)
            {
                sentence.FirstSentence = sentence.FirstSentence.TransformWithDistributeOrs();
            }
            else 
                throw new Exception("Encountered an illegal operator type in TransformWithDistributeOrs");

            return sentence;
        }


        private Sentence RedistributeOr(Sentence OrSubsentence)
        {
            // start with a clone to avoid any side effects
            Sentence sentence = Clone();

            // this sentence is guarenteed to be an AND sentence
            if (sentence.LogicOperator != LogicOperatorTypes.And)
                throw new Exception("RedistributeOr can only be called on a sentence whose top node is an AND clause");
            else
            {            
                sentence.FirstSentence = new Sentence(sentence.FirstSentence, LogicOperatorTypes.Or, OrSubsentence);
                sentence.SecondSentence = new Sentence(sentence.SecondSentence, LogicOperatorTypes.Or, OrSubsentence);
            }

            return sentence;
        }


        // static methods
        // convert logical operators to a string
        static private string LogicalOperatorToString(LogicOperatorTypes logicOperator)
        {
            if (logicOperator == LogicOperatorTypes.And)
                return "AND";
            else if (logicOperator == LogicOperatorTypes.Or)
                return "OR";
            else if (logicOperator == LogicOperatorTypes.Implies)
                return "=>";
            else if (logicOperator == LogicOperatorTypes.Biconditional)
                return "<=>";
            else
                throw new Exception("Error converting Sentence to string. Illegal operator type.");
        }

        // used by GetSymbolList() to collect all symbols in this Sentence
        private static void CollectSymbols(SymbolList tempSymbolList, Sentence subSentence)
        {
            // For each symbol, check if it's already in the list and, if not, add it
            // then return the full list. It will default to value undefined for everything.
            // then handle the rest, i.e. atomic vs. complex
            if (subSentence.Type == SentenceType.AtomicSentence)
                tempSymbolList.Add(subSentence.Symbol.TokenName);
            else // else this is a ComplexSentence
            {
                // all complex sentences have at least one sentence
                CollectSymbols(tempSymbolList, subSentence.FirstSentence);

                // if SentenceType is not None, then we have two sentences
                if (subSentence.LogicOperator != LogicOperatorTypes.None)
                    CollectSymbols(tempSymbolList, subSentence.SecondSentence);
            }
        }



    }

}
