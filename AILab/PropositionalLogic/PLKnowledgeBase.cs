using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AILab.PropositionalLogic
{

    class PLKnowledgeBase
    {
        // a propositional logic knowledge base is really just an array of propositional logic sentences
        protected Sentence[] sentences;
        // these are used to determine number of trues and falses -- I need this to determine if 
        // the query being entailed is true, false, or indeterminate
        private int queryTrueCount;
        private int queryFalseCount;

        // static parser -- so that Sentence can parse propositional logic text
        private static PLParser parser = new PLParser();

        // used for finding the symbol that is a unit clause
        private LogicSymbol possibleUnitClause;
        private int countOfSymbols;

        // properties
        public bool IsCNF { get; protected set; }

        // constructor
        public PLKnowledgeBase()
        {
            IsCNF = false;
        }

        // methods
        public void Clear()
        {
            sentences = null;
            IsCNF = false;
        }

        public void Add(Sentence aSentence)
        {
            int count;
            if (sentences == null)
                count = 0;
            else
                count = sentences.Count();

            // Only add if this symbol doesn't already exist
            if (Exists(aSentence) == false)
            {
                Sentence[] tempSentences;
                tempSentences = new Sentence[count + 1];
                tempSentences[count] = aSentence;

                if (tempSentences.Count() > 1)
                    for (int i = 0; i < count; i++)
                        tempSentences[i] = sentences[i];

                count++;
                sentences = new Sentence[count];

                for (int i = 0; i < count; i++)
                    sentences[i] = tempSentences[i];
            }
            IsCNF = false;
        }


        public void Add(Sentence[] sentenceList)
        {
            for (int i = 0; i < sentenceList.Count(); i++)
                Add(sentenceList[i]);
        }


        public void Add(string input)
        {
            Sentence[] sentenceList;
            parser.SetInput(input);
            sentenceList = parser.ParseInput();
            Add(sentenceList);
        }


        public bool Exists(Sentence aSentence)
        {
            // this function searches the knowledge base and attempts to find if this sentence is already in the knowledge base
            // this function does not find logically equivalent sentences, only ones that translate to identical strings
            // when the ToString() function is called

            if (sentences != null)
                for (int i = 0; i < sentences.Count(); i++)
                {
                    if (sentences[i].ToString() == aSentence.ToString())
                        return true;
                }

            return false;
        }


        public bool Exists(string sentenceStr)
        {
            parser.SetInput(sentenceStr);
            Sentence aSentence = parser.ParseSentence();
            if (parser.ParseSentence() != null)
                throw new Exception("Sentence(string) constructor only works with a single propositional logic statement.");
            else
                return Exists(aSentence);
        }



        public int Count()
        {
            if (sentences != null)
                return sentences.Count();
            else
                return 0;
        }



        public Sentence GetSentence(int index)
        {
            if (index >= sentences.Count() || index < 0)
                throw new Exception("Attempted to use GetSentence(index) with index out of bounds.");
            
            return sentences[index];        
        }



        public PLKnowledgeBase Clone()
        {
            PLKnowledgeBase tempKB = new PLKnowledgeBase();
            

            for (int i = 0, count = Count(); i < count; i++)
            {
                tempKB.Add(sentences[i].Clone());
            }

            tempKB.IsCNF = IsCNF;

            return tempKB;
        }



        public LogicSymbolValue EvaluateKnowledgeBase(SymbolList model)
        {
            // take the "model" (a SymbolList with values) and Evaluate each 
            // Sentence in the knowledge base. If all are true, the whole is true. 
            // if any or false the whole is false
            // if there isn't enough information available, return Undefined

            LogicSymbolValue result = LogicSymbolValue.True;

            for (int i = 0, count = sentences.Count(); i < count; i++)
            {
                if (sentences[i].EvaluateSentence(model) == LogicSymbolValue.False)
                    return LogicSymbolValue.False;
                else if (sentences[i].EvaluateSentence(model) == LogicSymbolValue.Undefined)
                    result = LogicSymbolValue.Undefined;
            }

            return result;
        }


        public bool IsKnowledgeBaseTrue(SymbolList model)
        {
            // the goal is to traverse the knowledge base using the model (a symbol list) and determine if the knowledge base
            // is true
            return (EvaluateKnowledgeBase(model) == LogicSymbolValue.True);
        }



        public bool IsKnowledgeBaseFalse(SymbolList model)
        {
            // the goal is to traverse the knowledge base using the model (a symbol list) and determine if the knowledge base
            // is false
            return (EvaluateKnowledgeBase(model) == LogicSymbolValue.False);
        }





        /// <summary>
        /// GetSymbolList(): This function returns an array of symbols contained in this knowledge base
        /// </summary>
        /// <returns>PLSymbolList array listing all symbols contained in this knowledge base, all set to unassigned</returns>
        public SymbolList GetSymbolList()
        {
            // traverse the knowledge base tree and find each symbol
            SymbolList tempSymbolList = new SymbolList();

            for (int i = 0, count = sentences.Count(); i < count; i++)
            {
                tempSymbolList.Add(sentences[i].GetSymbolList());
            }

            return tempSymbolList;

        }



        public bool IsQueryTrue(Sentence query)
        {
            // IsQueryTrue is exactly the same as DPLLEntails, so just call that procedure as it's faster
            // Than TruthTableEntails
            return (DPLLEntails(query));
        }



        public bool IsQueryFalse(Sentence query)
        {
            // IsQueryFalse means that we can definitely say that the query will always be false
            // This can't be determined by DPLL, so we have to use the slower TruthTableEntails function
            Sentence inverseQuery = new Sentence(query, false);
            return (DPLLEntails(inverseQuery));
        }


        public bool IsQueryTrue(string query)
        {
            // IsQueryTrue is exactly the same as DPLLEntails, so just call that procedure as it's faster
            // Than TruthTableEntails
            return (DPLLEntails(new Sentence(query)));
        }



        public bool IsQueryFalse(string query)
        {
            // IsQueryFalse means that we can definitely say that the query will always be false
            // This can't be determined by DPLL, so we have to use the slower TruthTableEntails function
            Sentence originalQuery = new Sentence(query);
            Sentence inverseQuery = new Sentence(originalQuery, false);
            return (DPLLEntails(inverseQuery));
        }



        public LogicSymbolValue TruthTableEntails(Sentence query)
        {
            SymbolList symbols = new SymbolList();
            SymbolList model;
            symbols = GetSymbolList();
            model = symbols.Clone();
            // initialize the query counts
            queryTrueCount = 0;
            queryFalseCount = 0;

            return TruthTableCheckAll(query, symbols, model);
        }


        public LogicSymbolValue TruthTableEntails(string query)
        {
            return TruthTableEntails(new Sentence(query));
        }




        // this function does the work of creating a truth table and thus evaluating the query against the knowledge base
        private LogicSymbolValue TruthTableCheckAll(Sentence query, SymbolList symbols, SymbolList model)
        {
            if (symbols == null || symbols.Count() == 0)
            {

                if (EvaluateKnowledgeBase(model) == LogicSymbolValue.True)
                {
                    // for any model that is true for the knowledge base, evalute the sentence as well
                    LogicSymbolValue result = query.EvaluateSentence(model);
                    if (result == LogicSymbolValue.True)
                        queryTrueCount++;
                    else if (result == LogicSymbolValue.False)
                        queryFalseCount++;
                    else
                        return LogicSymbolValue.Undefined;
                    
                    return query.EvaluateSentence(model);
                }
                else if (EvaluateKnowledgeBase(model) == LogicSymbolValue.False)
                    // if the model is false for the knowledge base, we 'throw it away' - i.e. we return true
                    // we don't care about models that aren't true for the knowledge base
                    return LogicSymbolValue.True;
                else
                    return LogicSymbolValue.Undefined;
            }
            else
            {
                // Get Next Symbol
                string nextSymbol = symbols.GetNextSymbol().SymbolName;

                // extend model as both true and false
                SymbolList copyModel1, copyModel2, copySymbols1=null, copySymbols2=null;
                copyModel1 = model.ExtendModel(nextSymbol, true);
                copyModel2 = model.ExtendModel(nextSymbol, false);

                // clone a copy of the remaining symbols (to avoid side effects)
                if (symbols != null && symbols.Count() > 0)
                {
                    copySymbols1 = symbols.Clone();
                    copySymbols2 = symbols.Clone();
                }

                // try both extended models
                LogicSymbolValue check1 = TruthTableCheckAll(query, copySymbols1, copyModel1);
                LogicSymbolValue check2 = TruthTableCheckAll(query, copySymbols2, copyModel2);

                if (check1 == LogicSymbolValue.True && check2 == LogicSymbolValue.True)
                    return LogicSymbolValue.True;
                else if (check1 == LogicSymbolValue.False || check2 == LogicSymbolValue.False)
                {
                    if (queryTrueCount > 0 && queryFalseCount > 0)
                        return LogicSymbolValue.Undefined;
                    else
                        return LogicSymbolValue.False;
                }
                else
                    return LogicSymbolValue.Undefined;
            }

        }


        public bool Entails(Sentence query)
        {
            return DPLLEntails(query);
        }


        public bool DPLLEntails(Sentence query)
        {
            SymbolList symbols = new SymbolList();
            SymbolList model;

            // satisfiability is the same as entails via this formula
            // a entails b if a AND ~b are unsatisfiable
            // so we change the query to be it's negation
            query = new Sentence(query, false);

            if (!IsCNF)
            {
                PLKnowledgeBase cnfClauses = new PLKnowledgeBase();
                cnfClauses = Clone();
                cnfClauses.Add(query);
                cnfClauses = cnfClauses.CNFClone();

                symbols = cnfClauses.GetSymbolList();
                model = symbols.Clone();

                return !cnfClauses.DPLL(symbols, model);
            }
            else
            {
                symbols = GetSymbolList();
                model = symbols.Clone();

                return !DPLL(symbols, model);
            }

        }


        public bool Entails(string query)
        {
            return DPLLEntails(query);
        }


        public bool DPLLEntails(string query)
        {
            return DPLLEntails(new Sentence(query));
        }



        public bool IsSatisfiable()
        {
            return DPLLIsSatisfiable();
        }


        public bool DPLLIsSatisfiable()
        {
            // This function takes no query, but instead just checks for satisfiability for the 
            // current knowledge base
            // Satisfiability is the existence of any model that is true, so it's not the same as entails
            SymbolList symbols = new SymbolList();
            SymbolList model;

            if (!IsCNF)
            {
                PLKnowledgeBase cnfClauses = new PLKnowledgeBase();
                cnfClauses = Clone();
                cnfClauses = cnfClauses.CNFClone();

                symbols = cnfClauses.GetSymbolList();
                model = symbols.Clone();

                return cnfClauses.DPLL(symbols, model);
            }
            else
            {
                symbols = GetSymbolList();
                model = symbols.Clone();

                return DPLL(symbols, model);
            }
        
        }


        public void ConvertToCNF()
        {
            PLKnowledgeBase kb = CNFClone();
            sentences = kb.sentences;
            IsCNF = true;
        }



        public PLKnowledgeBase CNFClone()
        {
            // this function takes the whole knowledge base and converts it to a single Knowledge Base in CNF form
            // with each sentence in the knowledge base being one OR clause
            Sentence sentence;
            String cnfKnowledgeBase="";
            for (int i=0, count=Count(); i < count; i++)
            {
                // TODO - is there a more efficient way to do this then to make into a string then 
                // convert back to a Sentence?
                cnfKnowledgeBase = cnfKnowledgeBase + sentences[i].ToString(true);

                if (i < count - 1)
                    cnfKnowledgeBase = cnfKnowledgeBase + " AND ";
            }

            sentence = new Sentence(cnfKnowledgeBase);
            sentence = sentence.ConvertToCNF();
            
            // "sentence" now contains the entire knowledge base in a single logical sentence
            // now loop through and find each OR clause and build a new knowledge base out of it
            PLKnowledgeBase newKb = new PLKnowledgeBase();

            newKb.BuildCNFKnowledgeBase(sentence);
            newKb.IsCNF = true;

            return newKb;
        }




        private void BuildCNFKnowledgeBase(Sentence sentence)
        {
            // This function takes a CNF Sentence and builds a knowledge base out of it where each OR clause 
            // becomes becomes a single sentence in the knowledge base.
            // Assumption: this sentence is already in CNF form -- if it isn't, the results are unpredictable
            // every time it's called, the top node must be an AND operator or symbol

            // This function will traverse a sentence finding disjunctions and splicing it all up into sentences
            // that are added to the knowledge base passed in. 

            // Strategy: recurse through the whole sentence tree and find each AND clause and then grab the clauses
            // in between (which are either OR clauses or symbols) and stuff them separately into the knowledge base

            if (sentence.LogicOperator == Sentence.LogicOperatorTypes.And)
            {
                if (sentence.FirstSentence.LogicOperator == Sentence.LogicOperatorTypes.Or ||
                    sentence.FirstSentence.Type == Sentence.SentenceType.AtomicSentence)
                {
                    // This is the top of an OR clause or it's atomic, so add it
                    Add(sentence.FirstSentence);
                }
                else if (sentence.FirstSentence.LogicOperator == Sentence.LogicOperatorTypes.And)
                {
                    BuildCNFKnowledgeBase(sentence.FirstSentence);
                }
                else
                    throw new Exception("BuildCNFKnowledgeBase was called with 'sentence' not in CNF form");


                if (sentence.SecondSentence.LogicOperator == Sentence.LogicOperatorTypes.Or ||
                    sentence.SecondSentence.Type == Sentence.SentenceType.AtomicSentence)
                {
                    // This is the top of an OR clause or it's atomic, so add it
                    Add(sentence.SecondSentence);
                }
                else if (sentence.SecondSentence.LogicOperator == Sentence.LogicOperatorTypes.And)
                {
                    BuildCNFKnowledgeBase(sentence.SecondSentence);
                }
                else
                    throw new Exception("BuildCNFKnowledgeBase was called with 'sentence' not in CNF form");

            }
            else if (sentence.Type == Sentence.SentenceType.AtomicSentence)
            {
                // it's a symbol, so just put it into the database
                Add(sentence);
            }
            else
                throw new Exception("BuildCNFKnowledgeBase was called with 'sentence' not in CNF form");
        }



        // This function evaluates the query against the knowledge base, but does so with the DPLL algorithm 
        // instead of a full brute truth table - thus it's faster
        // The query passed must be the entire knowledge base plus the query in CNF form
        // If it isn't, it will error out
        private bool DPLL(SymbolList symbols, SymbolList model)
        {

            // Strategy 1: Early Termination
            // if every clause in clauses is true in model then return true

            if (IsKnowledgeBaseTrue(model))
                return true;

            // if some clause in clauses is false in model then return false
            if (IsKnowledgeBaseFalse(model))
                return false;

            // otherwise we are still "Undefined" and so we need to keep recursively building the model
            

            // Strategy 2: Handle pure symbols
            // TODO - this needs to be optimized -- it seems to slow things down right now
            LogicSymbol symbol = FindPureSymbol(symbols, model);


            // TODO -- this block of code repeats, so look for a way to combine it and make it more readable
            if (symbol != null)
            {
                // remove the symbol from the symbol list
                symbols = symbols.Clone();
                symbols.Remove(symbol.SymbolName);

                // extend model with this symbol and value
                model = model.Clone();
                model.SetValue(symbol.SymbolName, symbol.Value);

                return DPLL(symbols, model);
            }    

            
            // Strategy3: Handle unit clauses
            // TODO: it's lame that FindPureSymbol is 'static' and FindUnitClause is part of the object
            // But leaving it for now
            symbol = FindUnitClause(model);

            if (symbol != null)
            {
                // remove the symbol from the symbol list
                symbols = symbols.Clone(); 
                symbols.Remove(symbol.SymbolName);

                // extend model with this symbol and value
                model = model.Clone();
                model.SetValue(symbol.SymbolName, symbol.Value);

                return DPLL(symbols, model);
            }   
            


            // Done with pure symbol and unit clause short cuts for now
            // Now extend the model with both true and false (similar to truth table entails)
            string nextSymbol = symbols.GetNextSymbol().SymbolName;

            // extend model as both true and false
            SymbolList copyModel1, copyModel2, copySymbols1 = null, copySymbols2 = null;
            copyModel1 = model.ExtendModel(nextSymbol, true);
            copyModel2 = model.ExtendModel(nextSymbol, false);

            // clone a copy of the remaining symbols (to avoid side effects)
            if (symbols != null && symbols.Count() > 0)
            {
                copySymbols1 = symbols.Clone();
                copySymbols2 = symbols.Clone();
            }

            // try both extended models
            return (DPLL(copySymbols1, copyModel1) ||
                DPLL(copySymbols2, copyModel2));

        }


        private LogicSymbol FindPureSymbol(SymbolList symbols, SymbolList model)
        {
            // traverse the entire "clauses" knowledge base looking for a "pure symbol" which is a symbol that 
            // is either all not negated or all negated. These are symbols we can easily decide to set in the 
            // model to either true (if not negated) or fales (if negated.) 
            LogicSymbol symbol;
            
            for (int i=0, symbolCount=symbols.Count(); i < symbolCount; i++)
            {
                string symbolName = symbols.GetSymbol(i).SymbolName;
                // TODO: seems like I could somehow combine the two calls to IsPureSymbol into one call and optimize
                if (IsPureSymbol(model, symbols.GetSymbol(i).SymbolName, false))
                {
                    symbol = new LogicSymbol();
                    symbol.SymbolName = symbols.GetSymbol(i).SymbolName;
                    symbol.Value = LogicSymbolValue.True;
                    return symbol;
                }

                if (IsPureSymbol(model, symbols.GetSymbol(i).SymbolName, true))
                {
                    symbol = new LogicSymbol();
                    symbol.SymbolName = symbols.GetSymbol(i).SymbolName;
                    symbol.Value = LogicSymbolValue.False;
                    return symbol;
                }
            }

            return null;
        }






        private bool IsPureSymbol(SymbolList model, string symbolName, bool negation)
        {
            string searchSymbol = symbolName;

            bool isPure = true;

            for (int i = 0, count = Count(); i < count; i++)
            {
                if (GetSentence(i).EvaluateSentence(model) == LogicSymbolValue.Undefined) // TODO: this seems inefficient
                    if (!AssessSymbol(GetSentence(i), searchSymbol, negation))
                        isPure = isPure && false;
            }

            return isPure;
        }



        private static bool AssessSymbol(Sentence sentence, string searchSymbol, bool negation)
        {
            // recursively looks through this sentence for symbolName and returns true if there 
            // are no negated versions of the symbol in this sentence. 

            // Assumption: we are in CNF else throw an error
            if (sentence.LogicOperator == Sentence.LogicOperatorTypes.Or)
            {
                return AssessSymbol(sentence.FirstSentence, searchSymbol, negation) &&
                    AssessSymbol(sentence.SecondSentence, searchSymbol, negation);
            }
            else if (sentence.Type == Sentence.SentenceType.AtomicSentence)
            {
                if (sentence.Symbol.TokenName == searchSymbol && sentence.Negation == negation)
                    return true; // we found the symbol with it's correct sign
                else if (sentence.Symbol.TokenName == searchSymbol && sentence.Negation != negation)
                    return false; // we found an opposite sign, so now the whole of it is false
                else
                    return true; // not finding our symbol counts as pure
            }
            else
                throw new Exception("AssessSymbol was called for a sentence not in CNF.");
        }



        private LogicSymbol FindUnitClause(SymbolList model)
        {
            // This function searches the sentence and, given the model, determines if this
            // sentence is a unit clause (i.e. a single symbol) or not.
            // A unit clause is defined as either a sentence made up of a single symbol (negated or not)
            // or a clause with all of the other symbols evaluating to false (as per model) save one

            // To do the work, it calls the private worker function, AssessNodes, which recurses the whole sentence
            // Assumption: This funtion assumes we're in CNF or else we get an error
            
            for (int i = 0, count = sentences.Count(); i < count; i++)
            {
                possibleUnitClause = null;
                countOfSymbols = 0;
                AssessNodes(GetSentence(i), model);
                if (possibleUnitClause != null)
                    return possibleUnitClause;
            }

            return null;
        }


        private void AssessNodes(Sentence clause, SymbolList model)
        {
            // recursively looks through this sentence for one and only one symbolName that has 
            // no assignment in the model
            // TODO -- assess efficiency and optimize and make more readable

            // Assumption: we are in CNF else throw an error

            if (countOfSymbols == -1) // failure if we have a true evaluation -- can't be a unit clause
                return; 
            else if (countOfSymbols > 1) // failure if we find two unassigned symbols - not a unit clause by definition
            {
                possibleUnitClause = null;
                return;
            }
            // is this a symbol? 
            if (clause.Type == Sentence.SentenceType.AtomicSentence)
            {
                // is this already assigned in the model? 
                LogicSymbolValue value = model.GetValue(clause.Symbol.TokenName);
                if (value == LogicSymbolValue.Undefined)
                {
                    // we found a potential unit clause
                    possibleUnitClause = new LogicSymbol();
                    possibleUnitClause.SymbolName = clause.Symbol.TokenName;
                    if (clause.Negation == false)
                        possibleUnitClause.Value = LogicSymbolValue.True;
                    else
                        possibleUnitClause.Value = LogicSymbolValue.False;

                    countOfSymbols++;
                }
                else // if (value == LogicSymbolValue.False || value == LogicSymbolValue.True)
                {
                    bool modifiedValue;

                    if (value == LogicSymbolValue.True)
                        modifiedValue = true;
                    else
                        modifiedValue = false;

                    if (clause.Negation == true)
                        modifiedValue = !modifiedValue;

                    // if you find a final vlaue that is true, abort
                    if (modifiedValue == true)
                    {
                        possibleUnitClause = null;
                        countOfSymbols = -1; // signal that we are sure this is not a unit clause
                        return;
                    }
                    else
                        // final value is false, so continue searching
                        return; 
                }
            }
            else if (clause.LogicOperator == Sentence.LogicOperatorTypes.Or)
            {
                // try FirstSentence branch first
                AssessNodes(clause.FirstSentence, model);

                // abort if neccessary
                if (countOfSymbols == -1) // failure if we have a true evaluation -- can't be a unit clause
                    return;
                else if (countOfSymbols > 1) // failure if we find two unassigned symbols - not a unit clause by definition
                {
                    possibleUnitClause = null;
                    return;
                }

                AssessNodes(clause.SecondSentence, model);

                // abort if neccessary
                if (countOfSymbols == -1) // failure if we have a true evaluation -- can't be a unit clause
                    return;
                else if (countOfSymbols > 1) // failure if we find two unassigned symbols - not a unit clause by definition
                {
                    possibleUnitClause = null;
                    return;
                }


                // if we made it this far and we have a single symbol, then we are a unit clause and we're done
                return;
            }
            else
                throw new Exception("FindUnitClause was called for a sentence not in CNF.");

        }

    }
}
