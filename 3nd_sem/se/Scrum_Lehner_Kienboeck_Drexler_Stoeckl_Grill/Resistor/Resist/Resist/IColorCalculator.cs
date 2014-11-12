using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Resist
{
    public interface IColorCalculator
    {
        Brush[] GetColorsByResist(double resist);
    }
}
