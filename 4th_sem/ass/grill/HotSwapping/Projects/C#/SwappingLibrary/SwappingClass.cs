using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;

namespace SwappingLibrary
{
    [Serializable()]
    public class SwappingClass
    {
        public void swappingMethod()
        {
            MessageBox.Show(@"Origin code, i will be replaced");
        }
    }
}
