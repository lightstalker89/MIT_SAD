// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MarketPlaceServiceTests.cs" company="FHWN">
//   Felber, Knopf, Popoic
// </copyright>
// <summary>
//   Defines the MarketPlaceServiceTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MarketPlaceServiceTests
{
    using System.Collections.Generic;
    using System.IO;

    using CommonModel;

    using MarketPlaceService;

    using NUnit.Framework;

    /// <summary>
    /// The market place service tests.
    /// </summary>
    [TestFixture]
    public class MarketPlaceServiceTests
    {
        /// <summary>
        /// The upload virtual machine_ uploading virtual machine with valid image id_ virtual machine is uploaded.
        /// </summary>
        [Test]
        // ReSharper disable once InconsistentNaming
        public void UploadVirtualMachine_UploadingVirtualMachineWithValidImageId_VirtualMachineIsUploaded()
        {
            byte[] tmparray;
            using (var fs = new FileStream(@"C:\index.php", FileMode.Open))
            {
                var buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);
                tmparray = buffer;
            }

            MarketPlaceServiceResponse marketPlaceServiceResponse = 
                new MarketPlaceService().UploadVirtualMachine(new VirtualMachine() { ImageId = "4" }, tmparray);
            
            Assert.IsFalse(marketPlaceServiceResponse.Error);
        }

        /// <summary>
        /// The upload virtual appliance_ uploading virtual appliance with valid appliance id_ virtual appliance is uploaded.
        /// </summary>
        [Test]
        // ReSharper disable once InconsistentNaming
        public void UploadVirtualAppliance_UploadingVirtualApplianceWithValidApplianceId_VirtualApplianceIsUploaded()
        {
            byte[] tmparray;
            using (var fs = new FileStream(@"C:\index.php", FileMode.Open))
            {
                var buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);
                tmparray = buffer;
            }

            MarketPlaceServiceResponse marketPlaceServiceResponse =
                new MarketPlaceService().UploadVirtualAppliance(new VirtualAppliance() { ImageId = "1" }, tmparray);

            Assert.IsFalse(marketPlaceServiceResponse.Error);
        }

        /// <summary>
        /// The change description of virtual machine_ changing description of passed virtual machine_ virtual machine description is changed.
        /// </summary>
        [Test]
        // ReSharper disable once InconsistentNaming
        public void ChangeDescriptionOfVirtualMachine_ChangingDescriptionOfPassedVirtualMachine_VirtualMachineDescriptionIsChanged()
        {
            MarketPlaceServiceResponse narketPlaceServiceResponse =
                new MarketPlaceService().ChangeDescriptionOfVirtualMachine(new VirtualMachine { ImageId = "1" });

            Assert.IsFalse(narketPlaceServiceResponse.Error);
        }

        /// <summary>
        /// The change description of virtual appliance_ changing description of passed virtual appliance_ virtual appliance description is changed.
        /// </summary>
        [Test]
        // ReSharper disable once InconsistentNaming
        public void ChangeDescriptionOfVirtualAppliance_ChangingDescriptionOfPassedVirtualAppliance_VirtualApplianceDescriptionIsChanged()
        {
            MarketPlaceServiceResponse narketPlaceServiceResponse =
                new MarketPlaceService().ChangeDescriptionOfVirtualAppliance(new VirtualAppliance { ImageId = "1" });

            Assert.IsFalse(narketPlaceServiceResponse.Error);
        }

        /// <summary>
        /// The download virtual machine_ downloading passed in virtual machine_ virtual machine is downloaded.
        /// </summary>
        [Test]
        // ReSharper disable once InconsistentNaming
        public void DownloadVirtualMachine_DownloadingPassedInVirtualMachine_VirtualMachineIsDownloaded()
        {
            DownloadVirtualMachineResponse downloadVirtualMachineResponse =
                new MarketPlaceService().DownloadVirtualMachine(new VirtualMachine { ImageId = "1" });

            Assert.IsFalse(downloadVirtualMachineResponse.Error);
            Assert.IsNotNull(downloadVirtualMachineResponse.ByteArray);
        }

        /// <summary>
        /// The download virtual appliance_ downloading passed in virtual appliance_ virtual appliance is downloaded.
        /// </summary>
        [Test]
        // ReSharper disable once InconsistentNaming
        public void DownloadVirtualAppliance_DownloadingPassedInVirtualAppliance_VirtualApplianceIsDownloaded()
        {
            DownloadVirtualApplianceResponse downloadVirtualApplianceResponse =
                new MarketPlaceService().DownloadVirtualAppliance(new VirtualAppliance() { ImageId = "1" });

            Assert.IsFalse(downloadVirtualApplianceResponse.Error);
            Assert.IsNotNull(downloadVirtualApplianceResponse.ByteArray);
        }

        /// <summary>
        /// The get virtual machines_ retrieving virtual machines_ all virtual machines are returned.
        /// </summary>
        [Test]
        // ReSharper disable once InconsistentNaming
        public void GetVirtualMachines_RetrievingVirtualMachines_AllVirtualMachinesAreReturned()
        {
             List<VirtualMachine> tmpVirtualMachines = new MarketPlaceService().GetDummyVirtualMachines();

             Assert.IsNotNull(tmpVirtualMachines);
        }

        /// <summary>
        /// The get virtual appliances_ retrieving virtual appliances_ all virtual appliances are returned.
        /// </summary>
        [Test]
        // ReSharper disable once InconsistentNaming
        public void GetVirtualAppliances_RetrievingVirtualAppliances_AllVirtualAppliancesAreReturned()
        {
            List<VirtualAppliance> tmpIVirtualAppliances = new MarketPlaceService().GetVirtualAppliances();

            Assert.IsNotNull(tmpIVirtualAppliances);
        }

        /// <summary>
        /// The get virtual machines type_ retrieving virtual machines with the specific type_ all virtual machines with the specific type are returned.
        /// </summary>
        [Test]
        // ReSharper disable once InconsistentNaming
        public void GetVirtualMachinesOsType_RetrievingVirtualMachinesWithTheSpecificOsType_AllVirtualMachinesWithTheSpecificOsTypeAreReturned()
        {
            var tmpMarketPlaceService = new MarketPlaceService();

            MarketPlaceServiceResponse response1 = tmpMarketPlaceService.UploadVirtualMachine(new VirtualMachine() { ImageId = "662", OperatingSystemVersion = "x23" }, new byte[10]);
            MarketPlaceServiceResponse response2 = tmpMarketPlaceService.UploadVirtualMachine(new VirtualMachine() { ImageId = "366", OperatingSystemVersion = "x64" }, new byte[10]);
            MarketPlaceServiceResponse response3 = tmpMarketPlaceService.UploadVirtualMachine(new VirtualMachine() { ImageId = "23", OperatingSystemVersion = "x64" }, new byte[10]);
            MarketPlaceServiceResponse response4 = tmpMarketPlaceService.UploadVirtualMachine(new VirtualMachine() { ImageId = "32", OperatingSystemVersion = "x64" }, new byte[10]);

            List<VirtualMachine> tmpVirtualMachines = tmpMarketPlaceService.GetDummyVirtualMachines();
        }

        /// <summary>
        /// The get virtual machines version_ retrieving virtual machines with the specific version_ all virtual machines with the specific version are returned.
        /// </summary>
        [Test]
        // ReSharper disable once InconsistentNaming
        public void GetVirtualMachinesOsVersion_RetrievingVirtualMachinesWithTheSpecificOsVersion_AllVirtualMachinesWithTheSpecificOsVersionAreReturned()
        {
           
        }

        /// <summary>
        /// The start instance_ starting valid instance id_ instance is started.
        /// </summary>
        [Test]
        // ReSharper disable once InconsistentNaming
        public void StartInstance_StartingValidInstanceId_InstanceIsStarted()
        {
            MarketPlaceServiceResponse marketPlaceServiceResponse = new MarketPlaceService().StartInstance(
                new VirtualMachineInstance()
                    {
                        InstanceId = "i-29975a68"
                    });

            Assert.IsFalse(marketPlaceServiceResponse.Error);
        }

        /// <summary>
        /// The stop instance_ stopping valid instance id_ instance is stopped.
        /// </summary>
        [Test]
        // ReSharper disable once InconsistentNaming
        public void StopInstance_StoppingValidInstanceId_InstanceIsStopped()
        {
            MarketPlaceServiceResponse marketPlaceServiceResponse = new MarketPlaceService().StopInstance(
                new VirtualMachineInstance()
                    {
                        InstanceId = "i-29975a68"
                    });

            Assert.IsFalse(marketPlaceServiceResponse.Error);
        }
    }
}
