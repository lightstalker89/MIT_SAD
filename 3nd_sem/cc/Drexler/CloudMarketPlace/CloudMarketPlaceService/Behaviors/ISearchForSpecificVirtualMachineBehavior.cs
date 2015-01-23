using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudMarketPlaceService
{
    public interface ISearchForSpecificVirtualMachineBehavior
    {
        List<DataLayer.VirtualMachine> SearchForSpecificVirtualMachine(DataLayer.VirtualMachine specificVMachine);
    }
}
