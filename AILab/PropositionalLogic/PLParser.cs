using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using AILab.Common;
using AILab.PropositionalLogic;

/*
 * Grammar for the Propositional Logic Parser 
 * 
 * Line -> LogicalSentence EndOfLine             
 * LogicalSentence -> OrAndOperands => OrAndOperands | OrAndOperands <=> OrAndOperands
 * OrAndOperands -> AndOperands OR AndOperands MoreOrOperators
 * MoreOrOperators -> OR AndOperands | ""
 * AndOperands -> Term AND Term MoreAndOperators
 * MoreAndOperators -> AND Term | ""
 * Term -> ~Term | (LogicalSentence) | Symbol        
 * Symbol -> <and string consisting of one letter and then any combination of letters and numbers>
 * 
 */

namespace AILab.PropositionalLogic
{
    // Propositional Logic Parser
    public class PLParser
    {
        // constants -- used to make code more readable
        private const Sentence.LogicOperatorTypes or = Sentence.LogicOperatorTypes.Or;
        private const Sentence.LogicOperatorTypes and = Sentence.LogicOperatorTypes.And;
        private const Sentence.LogicOperatorTypes biconditional = Sentence.LogicOperatorTypes.Biconditional;
        private const Sentence.LogicOperatorTypes implies = Sentence.LogicOperatorTypes.Implies;

        private const int tokenEof = (int)PLTokenTypes.EOF;
        private const int tokenNewline = (int)PLTokenTypes.NEWLINE;
        private const int tokenAnd = (int)PLTokenTypes.AND;
        private const int tokenBiconditional = (int)PLTokenTypes.BICONDITIONAL;
        private const int tokenImplies = (int)PLTokenTypes.IMPLIES;


        // fields
        private PLLexer lexer;

        // constructors
        public PLParser()
        {
            lexer = new PLLexer();
        }

        public PLParser(string inputString)
        {
            lexer = new PLLexer(inputString);
        }

        public PLParser(TextReader textInput)
        {
            lexer = new PLLexer(textInput);
        }


        // methods
        public void ClearInput()
        {
            lexer.Clear();
        }

        public void SetInput(string inputString)
        {
            ClearInput();
            lexer.SetInput(inputString);
        }

        public void SetInput(TextReader textInput)
        {
            ClearInput();
            lexer.SetInput(textInput);
        }


        private void Consume(PLTokenTypes tokenType)
        {
            // the passed in string must match the current token or else throw an error and stop parsing
            if ((int)tokenType == lexer.CurrentToken().TokenType)
            {
                lexer.Consume();
                return;
            }
            else
            {
                lexer.ParserError("Expected a '" + PLToken.ConvertTokenTypeToString(tokenType) + "'.");
                return;
            }
        }


        private void ConsumeEndOfLine()
        {
            if (lexer.CurrentToken().TokenType == (int)PLTokenTypes.NEWLINE ||
                    lexer.CurrentToken().TokenType == (int)PLTokenTypes.EOF)
            {
                SkipExtraNewLines();
                return;
            }
            else
            {
                lexer.ParserError("Expected a '" + PLToken.ConvertTokenTypeToString(PLTokenTypes.NEWLINE) + "' or " +
                    PLToken.ConvertTokenTypeToString(PLTokenTypes.EOF) + ".");
                return;
            }
        }


        private bool IsEndOfLine()
        {
            if (lexer.CurrentToken().TokenType == (int)PLTokenTypes.NEWLINE ||
                    lexer.CurrentToken().TokenType == (int)PLTokenTypes.EOF)
                return true;
            else
                return false;
        }

        private bool IsEndOfSubSentence()
        {
            Token token = lexer.CurrentToken();
            if (token == null)
                return false;
            // return true if this is a new line token or EOF token -- we've reached end of this sentence
            return (token.TokenType == (int)PLTokenTypes.NEWLINE || token.TokenType == (int)PLTokenTypes.EOF ||
                    token.TokenType == (int)PLTokenTypes.IMPLIES || token.TokenType == (int)PLTokenTypes.BICONDITIONAL ||
                    token.TokenType == (int)PLTokenTypes.RPAREN);
        }

        private bool IsConditional()
        {
            Token token = lexer.CurrentToken();
            if (token == null)
                return false;
            // return true if this is an implies or a biconditional
            return (token.TokenType == (int)PLTokenTypes.IMPLIES || token.TokenType == (int)PLTokenTypes.BICONDITIONAL);
        }

        private bool IsNotOperator()
        {
            Token token = lexer.CurrentToken();
            if (token == null)
                return false;
            // return true if this is a "NOT"
            return (token.TokenType == (int)PLTokenTypes.NOT);
        }

        private bool IsLeftParenthese()
        {
            Token token = lexer.CurrentToken();
            if (token == null)
                return false;
            // return true if this is a "("
            return (token.TokenType == (int)PLTokenTypes.LPAREN);
        }

        private bool IsOrOperator()
        {
            Token token = lexer.CurrentToken();
            if (token == null)
                return false;
            // return true if this is a Symbol
            return (token.TokenType == (int)PLTokenTypes.OR);
        }


        private bool IsAndOperator()
        {
            Token token = lexer.CurrentToken();
            if (token == null)
                return false;
            // return true if this is a Symbol
            return (token.TokenType == (int)PLTokenTypes.AND);
        }


        private void SkipExtraNewLines()
        {
            // take care of any new lines
            while (lexer.CurrentToken().TokenType == (int)PLTokenTypes.NEWLINE)
                lexer.Consume();
        }


        private bool IsEndOfFile()
        {
            Token token = lexer.CurrentToken();
            if (token == null)
                return false;
            // return true if this is a an EOF token
            return (token.TokenType == (int)PLTokenTypes.EOF);
        }


        public Sentence ParseSentence()
        {
            // return null if this starts out as an end of File
            if (IsEndOfFile())
                return null;

            return Line();
        }



        public Sentence[] ParseInput()
        {
            // return null if this starts out as an end of File
            if (IsEndOfFile())
                return null;

            Sentence[] sentenceList = new Sentence[1];
            Sentence[] tempSentenceList;
            int count = 1;

            while (!IsEndOfFile())
            {
                // grab a line
                sentenceList[count-1] = Line();

                // now expand array if not EOF
                if (!IsEndOfFile())
                {
                    tempSentenceList = new Sentence[count];

                    for (int i = 0; i < count; i++)
                        tempSentenceList[i] = sentenceList[i];

                    count++;
                    sentenceList = new Sentence[count];

                    for (int i = 0; i < count-1; i++)
                        sentenceList[i] = tempSentenceList[i];
                }
            }

            return sentenceList;
        }


        private Sentence Line()
        {
            Sentence line;
            line = LogicalSentence();
            ConsumeEndOfLine();
            return line;
        }


        
        private Sentence LogicalSentence() 
        {
            Sentence orAndOperand = OrAndOperands();

            if (IsConditional())
            {
                if (lexer.CurrentToken().TokenType == (int)PLTokenTypes.IMPLIES)
                {
                    Consume(PLTokenTypes.IMPLIES);
                    return new Sentence(orAndOperand, implies, OrAndOperands());
                }
                else if (lexer.CurrentToken().TokenType == (int)PLTokenTypes.BICONDITIONAL)
                {
                    Consume(PLTokenTypes.BICONDITIONAL);
                    return new Sentence(orAndOperand, biconditional, OrAndOperands());
                }
                else
                {
                    lexer.ParserError("Expected '=>' or '<=>'");
                    return null;
                }
            }
            else
                // if there is no conditional, then this has to be the end of a line
                return orAndOperand;
        }

        
        
        private Sentence OrAndOperands()
        {
            Sentence andOperands = AndOperands();

            if (!IsEndOfSubSentence())
            {
                PLTokenTypes oper = (PLTokenTypes)lexer.CurrentToken().TokenType;

                if (!(oper == PLTokenTypes.OR || oper == PLTokenTypes.AND))
                {
                    lexer.ParserError("Expected AND or OR operator");
                    return null;
                }
                else
                {
                    if (!IsEndOfSubSentence())
                        if (IsOrOperator()) // for ORs, loop through ORs
                            return MoreOrOperators(andOperands);
                }
            }

            return andOperands;
        }



        private Sentence MoreOrOperators(Sentence orOperands)
        {
            Sentence andOperands;
            while (IsOrOperator())
            {
                Consume(PLTokenTypes.OR);
                andOperands = AndOperands();
                orOperands = new Sentence(orOperands, or, andOperands);
            }
            return orOperands;
        }




        private Sentence AndOperands()
        {
            Sentence term = Term();

            if (!IsEndOfSubSentence())
            {
                PLTokenTypes oper = (PLTokenTypes)lexer.CurrentToken().TokenType;

                if (!(oper == PLTokenTypes.OR || oper == PLTokenTypes.AND))
                {
                    lexer.ParserError("Expected AND or OR operator");
                    return null;
                }
                else
                {
                    if (!IsEndOfSubSentence())
                        if (IsOrOperator()) // for ORs, kick back up a level without further processing
                            return new Sentence(term);
                        else if (IsAndOperator()) // otherwise it's AND
                            return MoreAndOperators(term);
                        else // else error
                        {
                            lexer.ParserError("Expected AND or OR operator");
                            return null;
                        }
                }
            }

            return term;
        }


        private Sentence MoreAndOperators(Sentence andOperands)
        {
            Sentence term;
            while (IsAndOperator())
            {
                Consume(PLTokenTypes.AND);
                term = Term();
                andOperands = new Sentence(andOperands, and, term);
            }
            return andOperands;
        }

        

        private Sentence Term()
        {
            if (IsNotOperator())
            {
                Sentence term;
                Consume(PLTokenTypes.NOT);
                term = Term();
                return new Sentence(term, false);
            }
            else if (IsLeftParenthese())
            {
                Sentence newSentence;
                Consume(PLTokenTypes.LPAREN);
                newSentence = LogicalSentence();
                Consume(PLTokenTypes.RPAREN);
                return newSentence;
            }
            else
            {
                // if we get here, it should be a symbol
                Token symbol = lexer.CurrentToken();
                Consume(PLTokenTypes.SYMBOL);
                return new Sentence(symbol);
            }                      
        }

    }

}
