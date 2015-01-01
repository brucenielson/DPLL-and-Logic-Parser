using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AILab.Common
{
    public class Token
    {
        // properties
        private string tokenName;
        public string TokenName 
        { 
            get { return tokenName; }
            set { tokenName = value.ToLower(); } 
        }
        
        public int TokenType { get; set; }

        // constructors
        protected Token() { }

        public Token(string name, int type)
        {
            TokenName = name.ToLower();
            TokenType = type;
        }

        // methods
        public bool MatchName(string name)
        {
            if (TokenName.ToLower() == name.ToLower())
                return true;
            else
                return false;
        }


        public bool MatchType(int tokenType)
        {
            if (TokenType == tokenType)
                return true;
            else
                return false;
        }


        public bool Equals(Token aToken)
        {
            if (this == aToken)
            {
                return true;
            }
            if ((aToken == null) || (TokenType != aToken.TokenType))
            {
                return false;
            }
            return ((aToken.TokenType == TokenType) && (aToken.TokenName.Equals(TokenName)));
        }

        public override string ToString()
        {
            return "[ " + TokenType + " " + TokenName + " ]";
        }

    }

}
