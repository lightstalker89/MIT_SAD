using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CloudMarketPlaceService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IMarketPlaceService
    {
        [OperationContract]
        MarketPlaceServiceResponse UploadVirtualMachine(VirtualMachine virtualMachine, byte[] byteArray);

        [OperationContract]
        MarketPlaceServiceResponse UploadVirtualAppliance(VirtualAppliance appliance, byte[] byteArray);

        [OperationContract]
        MarketPlaceServiceResponse ChangeDescriptionOfVirtualMachine(VirtualMachine updateVirtualMachine);

        [OperationContract]
        MarketPlaceServiceResponse ChangeDescriptionOfVirtualAppliance(VirtualAppliance updateVirtualAppliance);

        [OperationContract]
        DownloadVirtualMachineResponse DownloadVirtualMachine(VirtualMachine virtualMachine);

        [OperationContract]
        DownloadVirtualApplianceResponse DownloadVirtualAppliance(VirtualAppliance virtualAppliance);

        [OperationContract]
        MarketPlaceServiceResponse RateVirtualMachine(VirtualMachine virtualMachineToRate, byte rate);


        [OperationContract]
        MarketPlaceServiceResponse CommentVirtualMachine(VirtualMachine virtualMachineToComment, string comment);

        [OperationContract]
        List<VirtualMachine> SearchForSpecificVirtualMachine(VirtualMachine specificVirtualMachine);

        [OperationContract]
        List<VirtualAppliance> SearchForSpecificVirtualAppliance(VirtualAppliance specificVirtualAppliance);

        [OperationContract]
        MarketPlaceServiceResponse StartInstance(VirtualMachineInstance virtualMachineInstanceToStart);

        [OperationContract]
        MarketPlaceServiceResponse StopInstance(VirtualMachineInstance virtualMachineInstanceToStop);

        [OperationContract]
        List<VirtualMachineInstance> GetVirtualMachineInstances();

        [OperationContract]
        List<VirtualMachine> GetDummyVirtualMachines();

        [OperationContract]
        List<VirtualAppliance> GetVirtualAppliances();

        [OperationContract]
        string GetData(int value);

    }
}
