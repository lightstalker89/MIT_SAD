using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudMarketPlaceService
{
    public interface ISearchForSpecificVirtualApplianceBehavior
    {
        List<DataLayer.VirtualAppliance> SearchForSpecificVirtualAppliance(DataLayer.VirtualAppliance specificVAppliance);
    }
}
