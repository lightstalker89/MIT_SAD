using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolBox
{
    interface IDIContainer
    {
        T GetService<T>() where T : class;
        void Map<TIn, TOut>(TOut instance);
        void Map<TIn, TOut>();
    }
}
