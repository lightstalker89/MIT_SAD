using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudMarketPlaceService
{
    public interface IDownloadVirtualApplianceBehavior
    {
        DataLayer.DownloadVirtualApplianceResponse DownloadVirtualAppliance(DataLayer.VirtualAppliance vAppliance);
    }
}
