using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resist
{
    public class ViewModelProvider
    {
        private static MainWindowViewModel mainWindowViewModel = null;
        public static MainWindowViewModel MainWindowViewModel
        {
            get
            {
                return mainWindowViewModel ?? (mainWindowViewModel = new MainWindowViewModel());
            }
        }
    }
}
