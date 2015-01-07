// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMarketPlaceService.cs" company="FHWN">
//   Felber - Knopf - Popovic
// </copyright>
// <summary>
//   Defines the IMarketPlaceService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MarketPlaceService
{
    using System.Collections.Generic;
    using System.ServiceModel;

    using CommonModel;

    /// <summary>
    /// The Service1 interface.
    /// </summary>
    [ServiceContract]
    public interface IMarketPlaceService
    {
        /// <summary>
        /// The upload virtual machine.
        /// </summary>
        /// <param name="virtualMachine">
        /// The virtual Machine.
        /// </param>
        /// <param name="byteArray">
        /// The byte Array.
        /// </param>
        /// <returns>
        /// The <see cref="MarketPlaceServiceResponse"/>.
        /// </returns>
        [OperationContract]
        MarketPlaceServiceResponse UploadVirtualMachine(VirtualMachine virtualMachine, byte[] byteArray);

        /// <summary>
        /// The upload virtual appliance.
        /// </summary>
        /// <param name="appliance">
        /// The appliance.
        /// </param>
        /// <param name="byteArray">
        /// The byte Array.
        /// </param>
        /// <returns>
        /// The <see cref="MarketPlaceServiceResponse"/>.
        /// </returns>
        [OperationContract]
        MarketPlaceServiceResponse UploadVirtualAppliance(VirtualAppliance appliance, byte[] byteArray);

        /// <summary>
        /// The change description of virtual machine.
        /// </summary>
        /// <param name="updateVirtualMachine">
        /// The update Virtual Machine.
        /// </param>
        /// <returns>
        /// The <see cref="MarketPlaceServiceResponse"/>.
        /// </returns>
        [OperationContract]
        MarketPlaceServiceResponse ChangeDescriptionOfVirtualMachine(VirtualMachine updateVirtualMachine);

        /// <summary>
        /// The change description of virtual appliance.
        /// </summary>
        /// <param name="updateVirtualAppliance">
        /// The update Virtual Appliance.
        /// </param>
        /// <returns>
        /// The <see cref="MarketPlaceServiceResponse"/>.
        /// </returns>
        [OperationContract]
        MarketPlaceServiceResponse ChangeDescriptionOfVirtualAppliance(VirtualAppliance updateVirtualAppliance);

        /// <summary>
        /// The download virtual machine.
        /// </summary>
        /// <param name="virtualMachine">
        /// The virtual Machine.
        /// </param>
        /// <returns>
        /// The <see cref="MarketPlaceServiceResponse"/>.
        /// </returns>
        [OperationContract]
        DownloadVirtualMachineResponse DownloadVirtualMachine(VirtualMachine virtualMachine);

        /// <summary>
        /// The download virtual appliance.
        /// </summary>
        /// <param name="virtualAppliance">
        /// The virtual Appliance.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [OperationContract]
        DownloadVirtualApplianceResponse DownloadVirtualAppliance(VirtualAppliance virtualAppliance);

        /// <summary>
        /// The rate and comment virtual machine.
        /// </summary>
        /// <param name="virtualMachineToRate">
        /// The virtual Machine To Rate.
        /// </param>
        /// <param name="rate">
        /// The rate.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [OperationContract]
        MarketPlaceServiceResponse RateVirtualMachine(VirtualMachine virtualMachineToRate, byte rate);

        /// <summary>
        /// The rate and comment virtual appliance.
        /// </summary>
        /// <param name="virtualMachineToComment">The virtual machine to comment.</param>
        /// <param name="comment">
        /// The comment.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [OperationContract]
        MarketPlaceServiceResponse CommentVirtualMachine(VirtualMachine virtualMachineToComment, string comment);

        /// <summary>
        /// The search for specific virtual machine.
        /// </summary>
        /// <param name="specificVirtualMachine">
        /// The specific virtual machine.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>List</cref>
        ///     </see>
        ///     .
        /// </returns>
        [OperationContract]
        List<VirtualMachine> SearchForSpecificVirtualMachine(VirtualMachine specificVirtualMachine);

        /// <summary>
        /// The search for specific virtual appliance.
        /// </summary>
        /// <param name="specificVirtualAppliance">
        /// The specific virtual appliance.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>List</cref>
        ///     </see>
        ///     .
        /// </returns>
        [OperationContract]
        List<VirtualAppliance> SearchForSpecificVirtualAppliance(VirtualAppliance specificVirtualAppliance);

        /// <summary>
        /// The start instance.
        /// </summary>
        /// <param name="virtualMachineInstanceToStart">
        /// The virtual Machine Instance To Stop.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// True: Instance successfully started.
        /// </returns>
        [OperationContract]
        MarketPlaceServiceResponse StartInstance(VirtualMachineInstance virtualMachineInstanceToStart);

        /// <summary>
        /// The stop instance.
        /// </summary>
        /// <param name="virtualMachineInstanceToStop">
        /// The virtual Machine Instance To Stop.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// True: Instance successfully stopped.
        /// </returns>
        [OperationContract]
        MarketPlaceServiceResponse StopInstance(VirtualMachineInstance virtualMachineInstanceToStop);

        /// <summary>
        /// The get virtual machines.
        /// </summary>
        /// <returns>
        /// The <see>
        ///         <cref>List</cref>
        ///     </see>
        ///     .
        /// </returns>
        [OperationContract]
        List<VirtualMachineInstance> GetVirtualMachineInstances();

        /// <summary>
        /// The get virtual machines.
        /// </summary>
        /// <returns>
        /// The <see>
        ///         <cref>List</cref>
        ///     </see>
        ///     .
        /// </returns>
        [OperationContract]
        List<VirtualMachine> GetDummyVirtualMachines();

        /// <summary>
        /// The get virtual appliances.
        /// </summary>
        /// <returns>
        /// The <see>
        ///         <cref>List</cref>
        ///     </see>
        ///     .
        /// </returns>
        [OperationContract]
        List<VirtualAppliance> GetVirtualAppliances();
    }
}
