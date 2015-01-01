using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AILab.PropositionalLogic;

namespace AILab.PropositionalLogic
{
    
    public class LogicSymbol
    {
        public string SymbolName;
        public LogicSymbolValue Value = LogicSymbolValue.Undefined;
    }

    public enum LogicSymbolValue : int { False=0, True=1, Undefined=-1 };

    // this class is an array of Propositional Logic Symbols that can be set to true or false
    public class SymbolList
    {
        protected LogicSymbol[] logicSymbols;

        // properties
        public bool AutoSort { get; set; } // if set to true then autosort by only 'Add'ing in a sorted order
        public bool IsSorted { get; private set;  } // will be set to true if currently sorted


        public void Add(PLToken token)
        {
            if (token.TokenType == (int)PLTokenTypes.SYMBOL)
                Add(token.TokenName);
            else
                throw new Exception("Can't Add(PLToken) a non-symbol token from a symbol list.");
            
        }
        
        public void Add(string name)
        {
            name = name.ToLower();

            int count;
            if (logicSymbols == null)
                count = 0;
            else
                count = logicSymbols.Count();

            // Only add if this symbol doesn't already exist
            if (Find(name) == null)
            {
                LogicSymbol[] tempLogicSymbol;
                tempLogicSymbol = new LogicSymbol[count + 1];
                tempLogicSymbol[count] = new LogicSymbol();
    

                if (tempLogicSymbol.Count() > 1)
                    Copy(logicSymbols, tempLogicSymbol);
                tempLogicSymbol[count].SymbolName = name;
                tempLogicSymbol[count].Value = LogicSymbolValue.Undefined; // default value to undefined
                logicSymbols = tempLogicSymbol;

                if (AutoSort == true)
                {
                    Sort();
                    IsSorted = true;
                }
                else
                    IsSorted = false;

                // TODO: I should really just add it to the correct sorted location instead of calling sort -- fix this later

            }
        }



        protected void Add(LogicSymbol[] list)
        {
            // abort of list is empty
            if (list == null || list.Count() == 0)
                return;

            for (int i = 0, count = list.Count(); i < count; i++)
            {
                Add(list[i].SymbolName, list[i].Value);
            }
        }


        public void Add(SymbolList list)
        {
            Add(list.logicSymbols);
        }



        public void Add(string name, LogicSymbolValue value)
        {
            Add(name);
            SetValue(name, value);
        }


        public void Add(string name, bool value)
        {
            Add(name);
            SetValue(name, value);
        }



        public void Remove(string name)
        {
            name = name.ToLower();

            int count;
            if (logicSymbols == null)
                count = 0;
            else
                count = logicSymbols.Count();

            // Only remove if this symbol already exist
            if (Find(name) != null)
            {
                LogicSymbol[] tempLogicSymbol;
                tempLogicSymbol = new LogicSymbol[count - 1];

                if (tempLogicSymbol.Count() > 0)
                    for (int i = 0, j = 0; i < count; i++)
                    {
                        if (logicSymbols[i].SymbolName != name)
                        {

                            tempLogicSymbol[j] = logicSymbols[i];
                            j++;
                        }
                    }

                logicSymbols = tempLogicSymbol;

                if (AutoSort == true)
                {
                    Sort();
                    IsSorted = true;
                }

                // TODO: I should really just add it to the correct sorted location instead of calling sort -- fix this later

            }

        }


        public void Remove(PLToken token)
        {
            if (token.TokenType == (int)PLTokenTypes.SYMBOL)
                Remove(token.TokenName);
            else
                throw new Exception("Can't Remove(PLToken) a non-symbol token from a symbol list.");
        }



        public LogicSymbol Find(string name)
        {

            name = name.ToLower();
            
            // TODO: Change Find to not search every element if sorted but instead break in half each search to reduce hits
            int count=0;
            if (logicSymbols != null)
                count = logicSymbols.Count();
            
            for (int i = 0; i < count; i++)
            {
                if (logicSymbols[i].SymbolName == name)
                    return logicSymbols[i];
            }
            return null;
        }


        public SymbolList Clone()
        {
            SymbolList sl = new SymbolList();
            sl.Add(logicSymbols);
            return sl;
        }


        protected static void Copy(LogicSymbol[] array1, LogicSymbol[] array2)
        {
            if (array1.Count() > array2.Count())
                throw new Exception("Invalid use of PLSymbolList copy function because array2 is smaller than array1.");

            CopySection(array1, array2, 0, array1.Count() - 1);
        }


        private static void CopySection(LogicSymbol[] array1, LogicSymbol[] array2, int start, int end)
        {
            if (array1.Count() < end || array2.Count() < end)
                throw new Exception("Invalid use of PLSymbolList copy function. Attempting to copy outside of end of array.");

            for (int i = start; i <= end; i++)
            {
                array2[i] = new LogicSymbol();
                array2[i].SymbolName = array1[i].SymbolName; 
                array2[i].Value = array1[i].Value;
            }
        }




        public bool IsTrue(string symbolName)
        {
            // this function returns true if Value == LogicSymbolValue.True. Otherwise, it returns False. (i.e. Undefined will be treated as False.)
            symbolName = symbolName.ToLower(); 
            int count = logicSymbols.Count();
            for (int i = 0; i < count; i++)
            {
                if (logicSymbols[i].SymbolName == symbolName && logicSymbols[i].Value == LogicSymbolValue.True)
                    return true;
            }
            return false;
        }



        public bool IsFalse(string symbolName)
        {
            // this function returns true if Value == LogicSymbolValue.True. Otherwise, it returns False. (i.e. Undefined will be treated as False.)
            symbolName = symbolName.ToLower();
            int count = logicSymbols.Count();
            for (int i = 0; i < count; i++)
            {
                if (logicSymbols[i].SymbolName == symbolName && logicSymbols[i].Value == LogicSymbolValue.False)
                    return true;
            }
            return false;
        }



        public LogicSymbolValue GetValue(string symbolName)
        {
            // this function returns true if Value == LogicSymbolValue.True. Otherwise, it returns False. (i.e. Undefined will be treated as False.)
            symbolName = symbolName.ToLower();
            int count = logicSymbols.Count();
            for (int i = 0; i < count; i++)
            {
                if (logicSymbols[i].SymbolName == symbolName)
                    return logicSymbols[i].Value;
            }
            return LogicSymbolValue.Undefined;
        }


        



        public void SetValue(string name, bool value)
        {
            name = name.ToLower();
            for (int i = 0; i < logicSymbols.Count(); i++)
            {
                if (logicSymbols[i].SymbolName == name)
                {
                    if (value == true)
                        logicSymbols[i].Value = LogicSymbolValue.True;
                    else //if (value == false)
                        logicSymbols[i].Value = LogicSymbolValue.False;
                }
            }
        }


        public void SetValue(string name, LogicSymbolValue value)
        {
            name = name.ToLower();
            for (int i = 0; i < logicSymbols.Count(); i++)
            {
                if (logicSymbols[i].SymbolName == name)
                {
                    logicSymbols[i].Value = value;
                }
            }
        }



        public int Count()
        {
            return logicSymbols.Count();
        }


        public void Sort()
        {
            int count = Count();
            LogicSymbol[] tempArray = new LogicSymbol[count];
            QuickSort(tempArray, 0, count-1);

            // mark it all as sorted now
            IsSorted = true;
        }

        
        private void QuickSort(LogicSymbol[] tempArray, int left, int right)
        {
            // TODO: Make Quicksort it's own class using templates (once I know how to do that)

            // quick sort of logicalSymbols
            int count = (right - left) + 1;
            // abort once we have a single symbol we are sorting
            if (count <= 1) return;

            int middle = (count / 2) + left;
            int leftCounter = left;
            int rightCounter = right;
            string pivotSymbol = logicSymbols[middle].SymbolName;

            // left side
            for (int i=left; i <= right; i++)
            {
                // skip over the pivot
                if (i == middle)
                    continue; 

                if (logicSymbols[i].SymbolName.CompareTo(pivotSymbol) <= 0)
                {
                    // keep left of pivot
                    tempArray[leftCounter] = logicSymbols[i];
                    leftCounter++;
                }
                else
                {
                    // send to right of pivot
                    tempArray[rightCounter] = logicSymbols[i];
                    rightCounter--;
                }
            }
            // insert pivot last
            tempArray[leftCounter] = logicSymbols[middle];

            // set logicalSymbols for next round
            CopySection(tempArray, logicSymbols, left, right);

            // do recursive calls
            if (left < leftCounter - 1)
                QuickSort(tempArray, left, leftCounter - 1);
            if (leftCounter + 1 < right)
                QuickSort(tempArray, leftCounter + 1, right);
        }


        public LogicSymbol GetSymbol(int position)
        {
            if (position > Count() - 1 || position < 0)
                throw new Exception("Call to GetSymbol was out of bounds.");

            return logicSymbols[position];
        }


        public LogicSymbol GetNextSymbol()
        {
            // this function returns the top symbol (0)
            // while also removing it from the symbol list
            // use with care
            LogicSymbol nextSymbol = GetSymbol(0);
            Remove(nextSymbol.SymbolName);
            return nextSymbol;
        }


        public SymbolList ExtendModel(string symbolName, bool value)
        {
            // very much like SetValue except that it clones the model first and returns the clone
            // there by leaving the original unchanged
            SymbolList copyModel = Clone();
            copyModel.SetValue(symbolName, value);

            return copyModel;
        }

    }
}
