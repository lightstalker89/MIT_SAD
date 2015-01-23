using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudMarketPlaceService
{
    public interface IRateVirtualMachineBehavior
    {
        DataLayer.MarketPlaceServiceResponse RateVirtualMachine(DataLayer.VirtualMachine vMachineRate, byte rate);
    }
}
