using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AILab.Common;

namespace AILab.PropositionalLogic
{
    public class PLTokenTable : TokenTable
    {

        protected override void InitializeSymbolTable()
        {
            // add the reserved keywords for propositional logic
            Add("~", (int)PLTokenTypes.NOT);
            Add("and", (int)PLTokenTypes.AND);
            Add("or", (int)PLTokenTypes.OR);
            Add("=>", (int)PLTokenTypes.IMPLIES);
            Add("<=>", (int)PLTokenTypes.BICONDITIONAL);
            Add("(", (int)PLTokenTypes.LPAREN);
            Add(")", (int)PLTokenTypes.RPAREN);
            Add("End of File", (int)PLTokenTypes.EOF);
            Add("New Line", (int)PLTokenTypes.NEWLINE);
        }
    }
}
