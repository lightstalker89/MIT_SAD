using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsp4
{
    class Hashtable<T, U>
    {
        KeyValuePair<T, U>[][] table;

        public Hashtable(int count)
        {
            this.Count = count;
            this.table = new KeyValuePair<T, U>[this.Count][];
        }

        public int Count { get; private set; }

        private int CalculateHash(T key)
        {
            return (int)Math.Abs(key.GetHashCode() % this.Count);
            //double h1 = key.GetHashCode() * ((Math.Sqrt(5)) - 1) / 2;
            //double h2 = h1 % 1;
            //return (int)Math.Abs(this.Count * h2);
        }

        public void Add(T key, U value)
        {
            int hash = this.CalculateHash(key);

            if (this.table[hash] == null)
            {
                this.table[hash] = new KeyValuePair<T, U>[1];
            }
            else
            {
                Array.Resize<KeyValuePair<T, U>>(ref this.table[hash], this.table[hash].Length + 1);
            }

            this.table[hash][this.table[hash].Length - 1] = new KeyValuePair<T, U>(key, value);
        }

        public U this[T key]
        {
            get
            {
                int hash = this.CalculateHash(key);
                U tmp = default(U);

                foreach (KeyValuePair<T, U> item in this.table[hash] ?? new KeyValuePair<T, U>[0])
                {
                    if (item.Key.Equals(key))
                    {
                        tmp = item.Value;
                        break;
                    }
                }

                return tmp;
            }
        }
    }
}
