namespace CloudMarketPlaceService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// 
    /// </summary>
    public interface IUploadVirtualApplianceBehavior
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vAppliance"></param>
        /// <param name="byteContent"></param>
        /// <returns></returns>
        DataLayer.MarketPlaceServiceResponse UploadVirtualAppliance(DataLayer.VirtualAppliance vAppliance, byte[] byteContent);
    }
}
