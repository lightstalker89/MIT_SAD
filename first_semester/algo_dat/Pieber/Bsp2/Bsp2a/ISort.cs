using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsp2a
{
    public interface ISort
    {
        string Name();
        void Sort(ref int[] array);
    }
}
