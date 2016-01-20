namespace Poker
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public class DataBase
    {
        private readonly IList<bool?> bools = new List<bool?>();
        private readonly List<string> checkedWinners = new List<string>();
        private readonly List<int> ints = new List<int>();

        public DataBase()
        {
            this.bools = new List<bool?>();
            this.checkedWinners = new List<string>();
            this.ints = new List<int>();
        }


        public void AddBools(bool boolean)
        {
            this.bools.Add(boolean);
        }

        public void BoolsRemoveAt(int index)
        {
            this.bools.RemoveAt(index);
        }

        public void BoolsInsert(int index, bool? obj)
        {
            this.bools.Insert(index,obj);
        }

        public bool Contains(string checkedWinner)
        {
            return this.checkedWinners.Contains(checkedWinner);
        }

        public void AddCheckedWinners(string checkedWinner)
        {
            this.checkedWinners.Add(checkedWinner);
        }

        public void AddInts(int integer)
        {
            this.ints.Add(integer);
        }

        public void ClearInts()
        {
            this.ints.Clear();
        }

        public void ClearBools()
        {
            this.bools.Clear();
        }

        public void ClearCheckedWinners()
        {
            this.checkedWinners.Clear();
        }

        public void ToArray()
        {
            this.ints.ToArray();
        }

        public int IntsLenght()
        {
            return this.ints.ToArray().Length;
        }

        public int BoolsCount (Func<int, bool> predicate)
        {
            bool count = predicate(bools.ToArray().Length);
            if (count)
            {
                return bools.ToArray().Length;
            }
            return default(int);
        }

        public int IndexOf(bool? obj)
        {
            return bools.IndexOf(obj);
        }
    }
}