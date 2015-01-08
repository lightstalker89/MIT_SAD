//-----------------------------------------------------------------------
// <copyright file="MarketPlaceService.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace CloudMarketPlaceService
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using System.Text;
    using System.Threading.Tasks;
    using DataLayer;
    using DataLayer.OpenStack.Models;
    using DataLayer.OpenStack.RequestHandling;
    using OpenStackMgmt;

    /// <summary>
    /// The implementation of the IMarketPlaceService
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class MarketPlaceService : IMarketPlaceService
    {
        /// <summary>
        /// The virtual machine list.
        /// </summary>
        private readonly List<VirtualMachine> virtualMachineList = new List<VirtualMachine>();

        /// <summary>
        /// The virtual appliance list.
        /// </summary>
        private readonly List<VirtualAppliance> virtualApplianceList = new List<VirtualAppliance>();

        /// <summary>
        /// Identity service URL - Authentication Service - KeyStone
        /// </summary>
        private string identityServiceURL = string.Empty;

        /// <summary>
        /// Compute service URL - Nova
        /// </summary>
        private string computeServiceURL = string.Empty;

        /// <summary>
        /// Image service URL - Glance
        /// </summary>
        private string imageServiceURL = string.Empty;

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vMachine"></param>
        /// <param name="byteContent"></param>
        /// <returns></returns>
        public MarketPlaceServiceResponse UploadVirtualMachine(VirtualMachine vMachine, byte[] byteContent)
        {
            var marketPlaceServiceResponse = new MarketPlaceServiceResponse() { Error = true };
            
            try
            {
                const string TmpFilePath = @"C:\MarketPlaceVirtualMachines\";

                if (!Directory.Exists(TmpFilePath))
                {
                    Directory.CreateDirectory(TmpFilePath);
                }

                int fileCount = Directory.GetFiles(TmpFilePath, "*.*", SearchOption.AllDirectories).Length + 1;

                using (var fs = new FileStream(TmpFilePath + fileCount.ToString(CultureInfo.InvariantCulture), FileMode.CreateNew, FileAccess.Write))
                {
                    fs.Write(byteContent, 0, byteContent.Length);
                }

                vMachine.ImageID = fileCount.ToString(CultureInfo.InvariantCulture);
                this.virtualMachineList.Add(vMachine);

                marketPlaceServiceResponse.Error = false;
            }
            catch (Exception ex)
            {
                marketPlaceServiceResponse.Error = true;
                marketPlaceServiceResponse.ErrorMessage = ex.Message;
            }

            return marketPlaceServiceResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vAppliance"></param>
        /// <param name="byteContent"></param>
        /// <returns></returns>
        public MarketPlaceServiceResponse UploadVirtualAppliance(VirtualAppliance vAppliance, byte[] byteContent)
        {
            var marketPlaceServiceResponse = new MarketPlaceServiceResponse() { Error = true };

            try
            {
                const string TmpFilePath = @"C:\MarketPlaceVirtualAppliances\";

                if (!Directory.Exists(TmpFilePath))
                {
                    Directory.CreateDirectory(TmpFilePath);
                }

                int fileCount = Directory.GetFiles(TmpFilePath, "*.*", SearchOption.AllDirectories).Length + 1;

                using (var fs = new FileStream(TmpFilePath + fileCount.ToString(CultureInfo.InvariantCulture), FileMode.CreateNew, FileAccess.Write))
                {
                    fs.Write(byteContent, 0, byteContent.Length);
                }

                vAppliance.ImageID = fileCount.ToString(CultureInfo.InvariantCulture);
                this.virtualApplianceList.Add(vAppliance);

                marketPlaceServiceResponse.Error = false;
            }
            catch (Exception ex)
            {
                marketPlaceServiceResponse.Error = true;
                marketPlaceServiceResponse.ErrorMessage = ex.Message;
            }

            return marketPlaceServiceResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vMachine"></param>
        /// <returns></returns>
        public MarketPlaceServiceResponse ChangeDescriptionOfVirtualMachine(VirtualMachine vMachine)
        {
            var marketPlaceServiceResponse = new MarketPlaceServiceResponse() { Error = true };

            if (this.virtualMachineList.Any())
            {
                foreach (VirtualMachine tmpVirtualMachine in this.virtualMachineList.Where(tmpVirtualMachine => tmpVirtualMachine.ImageID == vMachine.ImageID))
                {
                    tmpVirtualMachine.Comment = vMachine.Comment;
                    tmpVirtualMachine.OperatingSystemName = vMachine.OperatingSystemName;
                    tmpVirtualMachine.OperatingSystemType = vMachine.OperatingSystemType;
                    tmpVirtualMachine.OperatingSystemVersion = vMachine.OperatingSystemVersion;
                    tmpVirtualMachine.Rate = vMachine.Rate;
                    tmpVirtualMachine.RecommendedCPUCount = vMachine.RecommendedCPUCount;
                    tmpVirtualMachine.RecommendedMemory = vMachine.RecommendedMemory;
                    tmpVirtualMachine.Size = vMachine.Size;
                    tmpVirtualMachine.SupportedVirtualizationPlatforms = vMachine.SupportedVirtualizationPlatforms;

                    marketPlaceServiceResponse.Error = false;
                }
            }
            else
            {
                marketPlaceServiceResponse.Error = true;
                marketPlaceServiceResponse.ErrorMessage = "No Virtual machines stored in our marketplace.";
            }

            if (marketPlaceServiceResponse.Error)
            {
                marketPlaceServiceResponse.ErrorMessage =
                    string.Format("Virtualmachine with imageID {0} is not stored on our MarketPlace!", vMachine.ImageID);
            }

            return marketPlaceServiceResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vAppliance"></param>
        /// <returns></returns>
        public MarketPlaceServiceResponse ChangeDescriptionOfVirtualAppliance(VirtualAppliance vAppliance)
        {
            var marketPlaceServiceResponse = new MarketPlaceServiceResponse() { Error = true };

            if (this.virtualApplianceList.Any())
            {
                foreach (VirtualAppliance tmpVirtualAppliance in this.virtualApplianceList.Where(tmpVirtualAppliance => tmpVirtualAppliance.ImageID == vAppliance.ImageID))
                {
                    tmpVirtualAppliance.Comment = vAppliance.Comment;
                    tmpVirtualAppliance.OperatingSystemName = vAppliance.OperatingSystemName;
                    tmpVirtualAppliance.OperatingSystemType = vAppliance.OperatingSystemType;
                    tmpVirtualAppliance.OperatingSystemVersion = vAppliance.OperatingSystemVersion;
                    tmpVirtualAppliance.Rate = vAppliance.Rate;
                    tmpVirtualAppliance.RecommendedCPUCount = vAppliance.RecommendedCPUCount;
                    tmpVirtualAppliance.RecommendedMemory = vAppliance.RecommendedMemory;
                    tmpVirtualAppliance.Size = vAppliance.Size;
                    tmpVirtualAppliance.SupportedVirtualizationPlatforms = vAppliance.SupportedVirtualizationPlatforms;

                    marketPlaceServiceResponse.Error = false;
                }
            }
            else
            {
                marketPlaceServiceResponse.Error = true;
                marketPlaceServiceResponse.ErrorMessage = "No Virtual appliances stored in our marketplace.";
            }

            if (marketPlaceServiceResponse.Error)
            {
                marketPlaceServiceResponse.ErrorMessage =
                    string.Format(
                        "Virtual appliance with imageID {0} is not stored on our MarketPlace!", vAppliance.ImageID);
            }

            return marketPlaceServiceResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vMachine"></param>
        /// <returns></returns>
        public DownloadVirtualMachineResponse DownloadVirtualMachine(VirtualMachine vMachine)
        {
            var downloadVirtualMachineResponse = new DownloadVirtualMachineResponse() { Error = true };

            try
            {
                string tmpFilePath = @"C:\MarketPlaceVirtualMachines\";

                int fileCount = Directory.GetFiles(tmpFilePath, "*.*", SearchOption.AllDirectories).Length;

                int tmpImageId;
                if (int.TryParse(vMachine.ImageID, out tmpImageId))
                {
                    if (fileCount >= tmpImageId)
                    {
                        if (!Directory.Exists(tmpFilePath))
                        {
                            Directory.CreateDirectory(tmpFilePath);
                        }

                        tmpFilePath += vMachine.ImageID;

                        byte[] tmparray;
                        using (var fs = new FileStream(tmpFilePath, FileMode.Open))
                        {
                            var buffer = new byte[fs.Length];
                            fs.Read(buffer, 0, (int)fs.Length);
                            tmparray = buffer;
                        }

                        downloadVirtualMachineResponse.Error = false;
                        downloadVirtualMachineResponse.ByteArray = tmparray;
                    }
                    else
                    {
                        downloadVirtualMachineResponse.Error = true;
                        downloadVirtualMachineResponse.ErrorMessage = string.Format(
                            "The virtual machine image with ImageID '{0}' is not stored in our market place.", vMachine.ImageID);
                    }
                }
                else
                {
                    downloadVirtualMachineResponse.Error = true;
                    downloadVirtualMachineResponse.ErrorMessage = string.Format(
                        "Couldn't convert imageID '{0}' to int.", vMachine.ImageID);
                }
            }
            catch (Exception ex)
            {
                downloadVirtualMachineResponse.Error = true;
                downloadVirtualMachineResponse.ErrorMessage = ex.Message;
            }

            return downloadVirtualMachineResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vAppliance"></param>
        /// <returns></returns>
        public DownloadVirtualApplianceResponse DownloadVirtualAppliance(VirtualAppliance vAppliance)
        {
            var downloadVirtualApplianceResponse = new DownloadVirtualApplianceResponse() { Error = true };

            try
            {
                string tmpFilePath = @"C:\MarketPlaceVirtualAppliances\";

                int fileCount = Directory.GetFiles(tmpFilePath, "*.*", SearchOption.AllDirectories).Length;

                int tmpImageId;
                if (int.TryParse(vAppliance.ImageID, out tmpImageId))
                {
                    if (fileCount >= tmpImageId)
                    {
                        if (!Directory.Exists(tmpFilePath))
                        {
                            Directory.CreateDirectory(tmpFilePath);
                        }

                        tmpFilePath += vAppliance.ImageID;

                        byte[] tmparray;
                        using (var fs = new FileStream(tmpFilePath, FileMode.Open))
                        {
                            var buffer = new byte[fs.Length];
                            fs.Read(buffer, 0, (int)fs.Length);
                            tmparray = buffer;
                        }

                        downloadVirtualApplianceResponse.Error = false;
                        downloadVirtualApplianceResponse.ByteArray = tmparray;
                    }
                    else
                    {
                        downloadVirtualApplianceResponse.Error = true;
                        downloadVirtualApplianceResponse.ErrorMessage = string.Format(
                            "The virtual appliance image with ImageID '{0}' is not stored in our market place.", vAppliance.ImageID);
                    }
                }
                else
                {
                    downloadVirtualApplianceResponse.Error = true;
                    downloadVirtualApplianceResponse.ErrorMessage = string.Format(
                        "Couldn't convert imageID '{0}' to int.", vAppliance.ImageID);
                }
            }
            catch (Exception ex)
            {
                downloadVirtualApplianceResponse.Error = true;
                downloadVirtualApplianceResponse.ErrorMessage = ex.Message;
            }

            return downloadVirtualApplianceResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vMachineRate"></param>
        /// <param name="rate"></param>
        /// <returns></returns>
        public MarketPlaceServiceResponse RateVirtualMachine(VirtualMachine vMachineRate, byte rate)
        {
            var marketPlaceServiceResponse = new MarketPlaceServiceResponse() { Error = true };

            foreach (VirtualMachine tmpVirtualMachine in this.virtualMachineList)
            {
                if (tmpVirtualMachine.ImageID == vMachineRate.ImageID)
                {
                    tmpVirtualMachine.Rate = rate;
                    marketPlaceServiceResponse.Error = false;
                }
            }

            if (marketPlaceServiceResponse.Error)
            {
                marketPlaceServiceResponse.ErrorMessage = string.Format("Cant't find Virtual machine with ImageID '{0}'", vMachineRate.ImageID);
            }

            return marketPlaceServiceResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="virtualMachineComment"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public MarketPlaceServiceResponse CommentVirtualMachine(VirtualMachine virtualMachineComment, string comment)
        {
            var marketPlaceServiceResponse = new MarketPlaceServiceResponse() { Error = true };

            foreach (VirtualMachine tmpVirtualMachine in this.virtualMachineList)
            {
                if (tmpVirtualMachine.ImageID == virtualMachineComment.ImageID)
                {
                    tmpVirtualMachine.Comment = comment;
                    marketPlaceServiceResponse.Error = false;
                }
            }

            if (marketPlaceServiceResponse.Error)
            {
                marketPlaceServiceResponse.ErrorMessage = string.Format(
                    "Cant't find Virtual machine with ImageID '{0}'", virtualMachineComment.ImageID);
            }

            return marketPlaceServiceResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="specificVMachine"></param>
        /// <returns></returns>
        public List<VirtualMachine> SearchForSpecificVirtualMachine(VirtualMachine specificVMachine)
        {
            var virtualMachines = this.virtualMachineList;

            if (!string.IsNullOrEmpty(specificVMachine.OperatingSystemType))
            {
                virtualMachines = virtualMachines.Where(x => x.OperatingSystemType.Contains(specificVMachine.OperatingSystemType)).ToList();
            }

            if (!string.IsNullOrEmpty(specificVMachine.OperatingSystemName))
            {
                virtualMachines = virtualMachines.Where(x => x.OperatingSystemName.Contains(specificVMachine.OperatingSystemName)).ToList();
            }

            if (!string.IsNullOrEmpty(specificVMachine.OperatingSystemVersion))
            {
                virtualMachines = virtualMachines.Where(x => x.OperatingSystemVersion.Contains(specificVMachine.OperatingSystemVersion)).ToList();
            }

            return virtualMachines;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="specificVAppliance"></param>
        /// <returns></returns>
        public List<VirtualAppliance> SearchForSpecificVirtualAppliance(VirtualAppliance specificVAppliance)
        {
            var virtualAppliances = this.virtualApplianceList;

            if (!string.IsNullOrEmpty(specificVAppliance.OperatingSystemType))
            {
                virtualAppliances = virtualAppliances.Where(x => x.OperatingSystemType.Contains(specificVAppliance.OperatingSystemType)).ToList();
            }

            if (!string.IsNullOrEmpty(specificVAppliance.OperatingSystemName))
            {
                virtualAppliances = virtualAppliances.Where(x => x.OperatingSystemName.Contains(specificVAppliance.OperatingSystemName)).ToList();
            }

            if (!string.IsNullOrEmpty(specificVAppliance.OperatingSystemVersion))
            {
                virtualAppliances = virtualAppliances.Where(x => x.OperatingSystemVersion.Contains(specificVAppliance.OperatingSystemVersion)).ToList();
            }

            return virtualAppliances;
        }

        /// <summary>
        /// Starts a stopped server and changes its status to ACTIVE.
        /// </summary>
        /// <param name="vMachineInstanceToStart"></param>
        /// <returns></returns>
        public MarketPlaceServiceResponse StartInstance(VirtualMachineInstance vMachineInstanceToStart)
        {
            MarketPlaceServiceResponse startInstanceResponse = new MarketPlaceServiceResponse();

            try
            {
                // Gets an authenticated user
                IdentityObject identity = this.AuthenticateOnOpenStackCloud();
                IComputeService computeService = new ComputeService(this.computeServiceURL);
                string answer = computeService.StartServer(identity.Access.Token.Tenant.Id, vMachineInstanceToStart.InstanceID, identity);

                startInstanceResponse.Error = false;
            }
            catch(Exception ex)
            {
                startInstanceResponse.Error = true;
                startInstanceResponse.ErrorMessage = ex.Message;
            }

            return startInstanceResponse;
        }

        /// <summary>
        /// Stops a running server and changes its status to STOPPED
        /// </summary>
        /// <param name="vMachineInstanceToStop"></param>
        /// <returns></returns>
        public MarketPlaceServiceResponse StopInstance(VirtualMachineInstance vMachineInstanceToStop)
        {
            MarketPlaceServiceResponse stopInstanceResponse = new MarketPlaceServiceResponse();

            try
            {
                // Gets an authenticated user
                IdentityObject identity = this.AuthenticateOnOpenStackCloud();
                IComputeService computeService = new ComputeService(this.computeServiceURL);
                string answer = computeService.StartServer(identity.Access.Token.Tenant.Id, vMachineInstanceToStop.InstanceID, identity);

                stopInstanceResponse.Error = false;
            }
            catch(Exception ex)
            {
                stopInstanceResponse.Error = true;
                stopInstanceResponse.ErrorMessage = ex.Message;
            }

            return stopInstanceResponse;
        }

        /// <summary>
        /// Returns a list of dummy virtual machines 
        /// </summary>
        /// <returns></returns>
        public List<VirtualMachine> GetDummyVirtualMachines()
        {
            if(this.virtualMachineList == null)
            {
                return null;
            }

            return this.virtualMachineList;
        }

        /// <summary>
        /// Returns a list of dummy virtual appliances
        /// </summary>
        /// <returns></returns>
        public List<VirtualAppliance> GetVirtualAppliances()
        {
            if(this.virtualApplianceList == null)
            {
                return null;
            }

            return this.virtualApplianceList;
        }

        /// <summary>
        /// List all servers
        /// </summary>
        /// <returns></returns>
        public List<VirtualMachineInstance> GetVirtualMachineInstances()
        {
            IdentityObject identity = this.AuthenticateOnOpenStackCloud();
            IComputeService computeService = new ComputeService(this.computeServiceURL);

            ListServersObject requestParametres = new ListServersObject();
            requestParametres.Tenant_Id = identity.Access.Token.Tenant.Id;
            requestParametres.Limit = 10;

            List<VirtualMachineInstance> vmInstances = new List<VirtualMachineInstance>();

            var response = computeService.ListServers(requestParametres, identity);

            foreach (var server in response.Servers)
            {
                VirtualMachineInstance a = new VirtualMachineInstance();
                a.InstanceID = server.Id;
                vmInstances.Add(a);
            }

            return vmInstances;
        }

        /// <summary>
        /// The storage to recommended central processing unit mapper.
        /// </summary>
        /// <param name="instanceType">
        /// The size.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int StorageToRecommendedCpuMapper(string instanceType)
        {
            int tmpCpuAmountInt;

            switch (instanceType)
            {
                case "m1.small":
                    tmpCpuAmountInt = 1;
                    break;
                case "m1.medium":
                    tmpCpuAmountInt = 1;
                    break;
                case "m1.large":
                    tmpCpuAmountInt = 2;
                    break;
                case "m1.xlarge":
                    tmpCpuAmountInt = 4;
                    break;
                case "m3.xlarge":
                    tmpCpuAmountInt = 4;
                    break;
                case "m3.2xlarge":
                    tmpCpuAmountInt = 8;
                    break;
                default:
                    tmpCpuAmountInt = 1;
                    break;
            }

            return tmpCpuAmountInt;
        }

        /// <summary>
        /// The instance type to size mapper.
        /// </summary>
        /// <param name="instanceType">
        /// The instance type.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int InstanceTypeToSizeMapper(string instanceType)
        {
            int tmpSize;

            switch (instanceType)
            {
                case "m1.small":
                    tmpSize = 160;
                    break;
                case "m1.medium":
                    tmpSize = 410;
                    break;
                case "m1.large":
                    tmpSize = 840;
                    break;
                case "m1.xlarge":
                    tmpSize = 1680;
                    break;
                case "m3.xlarge":
                    tmpSize = 80;
                    break;
                case "m3.2xlarge":
                    tmpSize = 160;
                    break;
                default:
                    tmpSize = 1;
                    break;
            }

            return tmpSize;
        }

        /// <summary>
        /// The storage to recommended memory mapper.
        /// </summary>
        /// <param name="instanceType">
        /// The size.
        /// </param>
        /// <returns>
        /// The <see cref="double"/>.
        /// </returns>
        public double StorageToRecommendedMemoryMapper(string instanceType)
        {
            double tmpCpuAmountDouble;

            switch (instanceType)
            {
                case "m1.small":
                    tmpCpuAmountDouble = 1.7;
                    break;
                case "m1.medium":
                    tmpCpuAmountDouble = 3.7;
                    break;
                case "m1.large":
                    tmpCpuAmountDouble = 7.5;
                    break;
                case "m1.xlarge":
                    tmpCpuAmountDouble = 15;
                    break;
                case "m3.xlarge":
                    tmpCpuAmountDouble = 15;
                    break;
                case "m3.2xlarge":
                    tmpCpuAmountDouble = 30;
                    break;
                default:
                    tmpCpuAmountDouble = 10;
                    break;
            }

            return tmpCpuAmountDouble;
        }

        /// <summary>
        /// Connect to the OpenStack enviroment
        /// </summary>
        /// <param name="authInformation"></param>
        /// <returns></returns>
        private IdentityObject AuthenticateOnOpenStackCloud()
        {
            AuthenticationToken authInformation = this.readAuthenticationInformation();

            if(authInformation != null)
            {
                try
                {
                    IIdentityService identityService = new IdentityService(this.identityServiceURL);
                    return identityService.GetAuthentication(authInformation);
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }

            return null;
        }

        /// <summary>
        /// Read authentication informations for the OpenStackIdentity service from the web.config
        /// </summary>
        /// <returns></returns>
        private AuthenticationToken readAuthenticationInformation()
        {
            try
            {
                AuthenticationToken auth = new AuthenticationToken();
                auth.UserName = ConfigurationManager.AppSettings["Username"];
                auth.Password = ConfigurationManager.AppSettings["Password"];
                auth.TenantId = ConfigurationManager.AppSettings["TenantId"];
                auth.Region = ConfigurationManager.AppSettings["Region"];

                this.identityServiceURL = ConfigurationManager.AppSettings["OpenStackIdentityServiceURL"];
                this.computeServiceURL = ConfigurationManager.AppSettings["OpenStackComputeServiceURL"];
                this.imageServiceURL = ConfigurationManager.AppSettings["OpenStackImageServiceURL"];

                return auth;
            }
            catch(ConfigurationErrorsException cErrorExc)
            {
                // ToDo 
                throw;
            }
            catch(UriFormatException uFormatExc)
            {
                throw;
            }
            catch(ArgumentNullException aNullExc)
            {
                throw;
            }
        }
    }
}
