using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheConfectionRebirth.Util
{
    public class BinaryHeap<T>
    {
        public List<T> items = new List<T>();
        Func<T, T, bool> Predicate; //if left is greater than right

        public BinaryHeap(Func<T, T, bool> Predicate)
        {
            this.Predicate = Predicate;
        }
        public T Pop()
        {
            if (items.Count == 0)
                return default;
            T rv = items[0];
            items[0] = items[items.Count - 1];
            items.RemoveAt(items.Count - 1);
            MoveUp(1);
            return rv;
        }

        public void Add(T item)
        {
            items.Add(item);
            MoveDown(items.Count);
        }

        void MoveDown(int pos)
        {
            while (pos > 1)
            {
                int odd = pos % 2;
                int newPos = (pos - odd) / 2;
                if (trySwap(pos, newPos))
                    pos = newPos;
                else break;
            }
            MoveUp(pos);
        }

        void MoveUp(int pos)
        {
            while (pos <= items.Count / 2)
            {
                int newPos1 = pos * 2;
                int newPos2 = newPos1 + 1;

                if (newPos2 <= items.Count)
                {
                    if (Predicate(items[newPos2 - 1], items[newPos1 - 1]))
                    {
                        int tempPos = newPos2;
                        newPos2 = newPos1;
                        newPos1 = tempPos;
                    }
                    if (trySwap(newPos1, pos))
                        pos = newPos1;
                    else if (trySwap(newPos2, pos))
                        pos = newPos2;
                    else
                        return;
                }
                else
                {
                    if (trySwap(newPos1, pos))
                        pos = newPos1;
                    else
                        return;
                }
            }
        }

        bool trySwap(int top, int bottom)
        {

            int firstInd1 = top - 1;
            int secondInd1 = bottom - 1;
            if (Predicate(items[firstInd1], items[secondInd1]))
            {
                T item = items[secondInd1];
                items[secondInd1] = items[firstInd1];
                items[firstInd1] = item;
                return true;
            }
            return false;
        }
    }
}
