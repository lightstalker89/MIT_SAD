using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientGUI
{
    class ServiceFactory
    {
        public static IService GetService(Boolean soa)
        {
            if (soa) return new ServiceSOA();
            else return new ServiceREST();
        }
    }
}
