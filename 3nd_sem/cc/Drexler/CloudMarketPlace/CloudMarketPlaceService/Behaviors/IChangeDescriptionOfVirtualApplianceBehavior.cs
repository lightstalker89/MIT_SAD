using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudMarketPlaceService
{
    public interface IChangeDescriptionOfVirtualApplianceBehavior
    {
        DataLayer.MarketPlaceServiceResponse ChangeDescriptionOfVirtualAppliance(DataLayer.VirtualAppliance vAppliance);
    }
}
