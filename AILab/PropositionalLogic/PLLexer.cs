using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using AILab.Common;

namespace AILab.PropositionalLogic
{
    public class PLLexer : Lexer
    {
        // constructors
        public PLLexer() : base() 
        {
            symbolTable = new PLTokenTable();
        }

        public PLLexer(string inputString)
            : base(inputString)
        {
            symbolTable = new PLTokenTable();
        }

        public PLLexer(TextReader textInput)
            : base(textInput)
        {
            symbolTable = new PLTokenTable();
        }

        public override void Clear()
        {
            base.Clear();
            symbolTable = new PLTokenTable();
        }


        // Consumes the Current Token and advances it to the next one
        protected override Token GetNextToken()
        {
            string tokenName = "";
            Token aToken=null;

            // Call this function only when nextChar is on the first character of a token to be parsed
            // The types of Tokens that are valid are the PropositionalLogicTokenTypes
            if (nextChar == -1) // end of File -- return EOF token as a soft way of dealing with the problem
            {
                aToken = symbolTable.Find("End of File");
                if (aToken == null)
                {
                    // could not find EOF
                    throw new SyntaxErrorException("Lexer Syntax Error at Line: " + lineNumber.ToString() + ". No EOF Token Found.'");
                }
            }
            else if (nextChar == (int)'\n')
            {
                // we have hit the end of a line

                // increment new line
                while (nextChar == (int)'\n')
                {
                    lineNumber++;
                    // get next character
                    nextChar = input.Read();
                }
                // return a NEWLINE token
                aToken = symbolTable.Find("New Line");
                if (aToken == null)
                {
                    // could not find New Line token
                    throw new SyntaxErrorException("Lexer Syntax Error at Line: " + lineNumber.ToString() + ". No NEWLINE Token Found.'");
                }
            }
            else if (char.IsLetter((char)nextChar)) // PropositionalLogicTokenTypes.SYMBOL or operator (OR, AND)
            {
                // Now load in the whole symbol
                while (char.IsLetterOrDigit((char)nextChar))
                {
                    // create symbol name
                    tokenName = tokenName + ((char)nextChar).ToString().ToLower();
                    // get next character
                    nextChar = input.Read();
                    // check for EOF. If so, break out of loop as if white space
                    if (nextChar == -1)
                        break;
                }

                // now that we've got a symbol, move to the next one
                NextStartOfToken();

                // is this an existing symbol? If so, return it
                aToken = symbolTable.FindOrAdd(tokenName, (int)PLTokenTypes.SYMBOL);
            }
            else if (PartOfConnector((char)nextChar))
            {
                // Now load in the whole symbol
                while (PartOfConnector((char)nextChar))
                {
                    // create symbol name
                    tokenName = tokenName + ((char)nextChar).ToString();
                    // get next character
                    nextChar = input.Read();
                    // check for EOF. If so, break out of loop as if white space
                    if (nextChar == -1)
                        break;
                }

                // now that we've got a symbol, move to the next one
                NextStartOfToken();

                // is this a correct connector? If so, return it
                aToken = symbolTable.Find(tokenName);

                if (aToken == null)
                {
                    // this is an invalid token type -- stop parsing
                    throw new SyntaxErrorException("Lexer Syntax Error at Line: " + lineNumber.ToString() + ". Expected '=>' or '<=>'");
                }

            }
                // deal with funny symbols: "(", ")", "~"
            else if ((char)nextChar == '(' || (char)nextChar == ')' || (char)nextChar == '~')
            {

                // make a token for "(" or ")"
                aToken = null;
                if ((char)nextChar == '(')
                    aToken = symbolTable.Find("(");
                else if ((char)nextChar == ')')
                    aToken = symbolTable.Find(")");
                else if ((char)nextChar == '~')
                    aToken = symbolTable.Find("~");

                if (aToken == null)
                {
                    // it's not in the symbol table -- stop parsing
                    throw new SyntaxErrorException("Lexer Syntax Error at Line: " + lineNumber.ToString() + ". Symbol Table not initialized with '(' or ')'");
                }

                // now that we've got a symbol, move to the next one
                // get next character
                nextChar = input.Read();
                NextStartOfToken();

            }
            else
            {
                // did not find a valid token type
                throw new SyntaxErrorException("Lexer Syntax Error at Line: " + lineNumber.ToString() + ". Expected Alpha, =, >, or <.");
            }

            return aToken;

        }

        
        public bool IsEOF()
        {
            return (CurrentToken().TokenType == (int)PLTokenTypes.EOF);
        }


        private bool PartOfConnector(char theChar)
        {
            if (theChar == '=' || theChar == '<' || theChar == '>')
                return true;
            else
                return false;
        }

    }

}
