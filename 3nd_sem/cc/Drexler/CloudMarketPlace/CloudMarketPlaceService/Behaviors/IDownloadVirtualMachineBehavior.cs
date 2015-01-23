using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudMarketPlaceService
{
    public interface IDownloadVirtualMachineBehavior
    {
        DataLayer.DownloadVirtualMachineResponse DownloadVirtualMachine(DataLayer.VirtualMachine vMachine);
    }
}
