using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortierAlgorithmen
{
    public abstract class SortAlgorithm
    {
        public abstract List<int> Sort(List<int> elements);
        public abstract void Output(List<int> sortedList);
    }
}
