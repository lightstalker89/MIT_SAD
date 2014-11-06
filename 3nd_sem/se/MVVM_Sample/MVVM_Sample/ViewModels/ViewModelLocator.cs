using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_Sample.ViewModels
{
    public class ViewModelLocator
    {

        public MainWindow ViewModelMainWindow
        {
            get { return new MainWindow(); }
        }
    }
}
