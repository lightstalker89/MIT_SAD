using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudMarketPlaceService
{
    public interface IChangeDescriptionOfVirtualMachineBehavior
    {
        DataLayer.MarketPlaceServiceResponse ChangeDescriptionOfVirtualMachine(DataLayer.VirtualMachine vMachine);
    }
}
