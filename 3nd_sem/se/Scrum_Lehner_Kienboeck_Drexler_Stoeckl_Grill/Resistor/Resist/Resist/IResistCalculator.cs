using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Resist
{
    public interface IResistCalculator
    {
        double GetResist(Brush c1, Brush c2, Brush c3, Brush c4, Brush c5);
    }
}
