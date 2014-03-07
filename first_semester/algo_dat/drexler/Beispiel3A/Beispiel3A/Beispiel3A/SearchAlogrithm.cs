using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beispiel3A
{
    public abstract class SearchAlogrithm
    {
        public abstract int Search(int[] numbers, int value, out int compareCount);
        public abstract int Min(int[] numbers, out int compareCount);
        public abstract int Max(int[] numbers, out int compareCount);
    }
}
