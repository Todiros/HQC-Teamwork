using System;

namespace Poker
{
    public interface IDataBase
    {
        void AddBools(bool boolean);

        void BoolsRemoveAt(int index);

        void BoolsInsert(int index, bool? obj);

        bool Contains(string checkedWinner);

        void AddCheckedWinners(string checkedWinner);

        void AddInts(int integer);

        void ClearInts();

        void ClearBools();

        void ClearCheckedWinners();

        void ToArray();

        int IntsLenght();

        int BoolsCount(Func<int, bool> predicate);

        int IndexOf(bool? obj);
    }
}