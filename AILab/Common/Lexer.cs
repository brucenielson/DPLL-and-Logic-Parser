using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;    

namespace AILab.Common
{

    public abstract class Lexer
    {
        // fields 
        protected TokenTable symbolTable = null; // will be instanciated in superclass
        protected TextReader input = null;
        protected int nextChar = ((int)' '); // initialize to white space
        protected int lineNumber = 1;
        protected const int lookAheadAmount = 4;
        protected Token[] tokenPipeline = new Token[lookAheadAmount];

        // abstract methods
        // must be implemented in the overriding class and given correct syntax for that type of lexer
        protected abstract Token GetNextToken();

        // constructors
        protected Lexer() { }

        public Lexer(string inputString)
        {
            SetInput(inputString);
        }

        public Lexer(TextReader textInput)
        {
            SetInput(textInput);
        }

        
        // methods
        
        // initialize the Pipeline
        private void InitPipeline()
        {
            // read first character so long as we have an input string setup
            if (input != null)
            {
                // only do the initialization if the pipeline is empty
                if (tokenPipeline[0] == null)
                {
                    NextStartOfToken();

                    // Read in lookAheadNumber of Tokens and fill up the pipeline
                    for (int i = 0; i < lookAheadAmount; i++)
                    {
                        tokenPipeline[i] = GetNextToken();
                    }
                }
            }
        }

        // Uses up the "Current Token" and then advances the Pipeline by one -- Call GetNextToken, which does all the work
        public void Consume()
        {
            InitPipeline();
            AddToTokenPipeline(GetNextToken());
        }

        // Run Consume 'number' number of times
        public void Consume(int number)
        {
            if (number < 0)
                number = 0;

            for (int i = 1; i < number; i++)
                Consume();
        }

        public Token ReadAndConsume()
        {
            Token token = CurrentToken();
            Consume();
            return token;
        }



        // gives us the current Token without moving forward        
        public Token CurrentToken()
        {
            return LookAheadToken(1);
        }



        // Allows a lookahead up to a preset number of tokens (by lookAheadAmount in the base Lexer class)
        // lookAhead = 1 is the same as CurrentToken()
        public Token LookAheadToken(int lookAhead)
        {
            InitPipeline();
            if (lookAhead > lookAheadAmount || lookAhead < 1)
                throw new Exception("Lexer Error: Can't look ahead more tokens than Look Ahead Amount set");

            return tokenPipeline[lookAhead-1];
        }


        protected void AddToTokenPipeline(Token aToken)
        {
            InitPipeline();
            for (int i = 0; i < lookAheadAmount-1; i++)
            {
                tokenPipeline[i] = tokenPipeline[i + 1];
            }
            tokenPipeline[lookAheadAmount - 1] = aToken;
        }


        public void SetInput(string inputString)
        {		
            input = new StringReader(inputString);
        }

        public void SetInput(TextReader textInput)
        {
            input = textInput;
        }

        public virtual void Clear()
        {
            input = null;
            nextChar = ((int)' ');
            symbolTable = null;
            for (int i = 0; i < lookAheadAmount - 1; i++)
                tokenPipeline[i] = null;
        }

        protected void NextStartOfToken()
        {
            // all this function does is move to the next token start if we are now on whitespace
            // if this is a newline, line number is incremented
            // Note: NextStartOfToken does nothing if EOF is reached
            while (nextChar == (int)' ' || nextChar == (int)'\t')
            {
                nextChar = input.Read();
            }
        }


        public void ParserError(string message)
        {
            throw new SyntaxErrorException("Syntax Error at Line: " + lineNumber.ToString() + ". Received '" + CurrentToken().TokenName + "'. " + message);
        }

        public void ParserError(string message, Token token)
        {
            throw new SyntaxErrorException("Syntax Error at Line: " + lineNumber.ToString() + ". Received '" + token.TokenName + "'. " + message);
        }


    }

}
