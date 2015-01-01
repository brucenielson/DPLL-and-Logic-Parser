using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AILab.Common
{
    public abstract class TokenTable
    {
        protected List<Token> symbolTableList = new List<Token>();        

        // abstract class is specific to grammar
        protected abstract void InitializeSymbolTable();

        // constructor -- initializes symbol table by calling abstract function InitializeSymbolTable()
        public TokenTable()
        {
            InitializeSymbolTable();
        }

        // Methods
        public void Add(string name, int type)
        {
            Token aToken = new Token(name.ToLower(), type);
            this.Add(aToken);
        }

        protected void Add(Token aToken)
        {
            // add the token -- but make sure it's lower case first
            if (aToken.TokenName != aToken.TokenName.ToLower())
                aToken = new Token(aToken.TokenName.ToLower(), aToken.TokenType);

            // don't add if it's already there
            if (Find(aToken.TokenName) == null)
                symbolTableList.Add(aToken);          
        }

        public Token Find(string name)
        {
            // explaining (via delegate) how to match the requested symbol
            return symbolTableList.Find(delegate(Token targetToken) { return targetToken.MatchName(name); });
        }

        public Token Find(int tokenType)
        {
            // explaining (via delegate) how to match the requested symbol
            return symbolTableList.Find(delegate(Token targetToken) { return targetToken.MatchType(tokenType); });
        }

        public Token FindOrAdd(string name, int type)
        {
            Token aToken = null;

            // change to lower case
            name = name.ToLower();

            aToken = Find(name);

            // if it wasn't found in the symbol table already, add it
            if (aToken == null)
            {
                aToken = new Token(name, type);
                Add(aToken);
            }
            return aToken;
        }


        static private int CompareTo(Token token1, Token token2)
        {
            // CompareTo(StrB)
            // Less than zero: This instance precedes strB.  
            // Zero: This instance has the same position in the sort order as strB. 
            // Greater than zero: This instance follows strB or strB is null

            return token1.TokenName.ToLower().CompareTo(token2.TokenName.ToLower());
        }



        public void Sort()
        {
            symbolTableList.Sort(CompareTo);          
        }
    }

}
