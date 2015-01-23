namespace CloudMarketPlaceService
{
    /// <summary>
    /// Defines the interface of a component that is able to upload a virtual machine.
    /// </summary>
    public interface IUploadVirtualMachineBehavior
    {
        /// <summary>
        /// Uploads a virtual machine.
        /// </summary>
        /// <param name="vMachine">Specifies the virtual machine that shall be uploaded.</param>
        /// <param name="byteContent">Specifies the virtual machine image that shall be uploaded.</param>
        /// <returns></returns>
        DataLayer.MarketPlaceServiceResponse UploadVirtualMachine(DataLayer.VirtualMachine vMachine, byte[] byteContent);
    }
}
