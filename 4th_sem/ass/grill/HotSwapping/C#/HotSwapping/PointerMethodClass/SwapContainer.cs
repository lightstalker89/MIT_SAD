using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HotSwapping.PointerMethodClass
{
    public class SwapContainer
    {
        public void swapMe()
        {
            MessageBox.Show(@"Origin code, I will be replaced");
        }
    }
}
