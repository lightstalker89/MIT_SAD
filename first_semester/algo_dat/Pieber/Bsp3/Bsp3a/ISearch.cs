using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsp3a
{
    public interface ISearch
    {
        int CompareCount { get; }

        int Search(int[] array, int value);

        int GetMinValue(int[] array);

        int GetMaxValue(int[] array);
    }
}
