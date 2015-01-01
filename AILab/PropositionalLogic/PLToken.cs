using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AILab.Common;

namespace AILab.PropositionalLogic
{
    public enum PLTokenTypes : int { SYMBOL, LPAREN, RPAREN, AND, OR, NOT, IMPLIES, BICONDITIONAL, EOF, NEWLINE };

    public class PLToken : Token
    {

        public PLToken(string name, PLTokenTypes type)
        {
            TokenName = name;
            TokenType = (int)type;
        }




        static public string ConvertTokenTypeToString(PLTokenTypes tokenType)
        {
            string tokenName;

            switch (tokenType)
            {
                case PLTokenTypes.SYMBOL:
                    tokenName = "Symbol";
                    break;

                case PLTokenTypes.AND:
                    tokenName = "AND";
                    break;

                case PLTokenTypes.BICONDITIONAL:
                    tokenName = "<=>";
                    break;

                case PLTokenTypes.EOF:
                    tokenName = "End of File";
                    break;

                case PLTokenTypes.IMPLIES:
                    tokenName = "=>";
                    break;

                case PLTokenTypes.LPAREN:
                    tokenName = "'('";
                    break;

                case PLTokenTypes.RPAREN:
                    tokenName = "')'";
                    break;

                case PLTokenTypes.NEWLINE:
                    tokenName = "New Line";
                    break;

                case PLTokenTypes.NOT:
                    tokenName = "'~'";
                    break;

                case PLTokenTypes.OR:
                    tokenName = "OR";
                    break;

                default:
                    throw new Exception("Invalid Token Types passed to ConvertTokenTypeToString()");
            }

            return tokenName;
        }


        

    }
}
