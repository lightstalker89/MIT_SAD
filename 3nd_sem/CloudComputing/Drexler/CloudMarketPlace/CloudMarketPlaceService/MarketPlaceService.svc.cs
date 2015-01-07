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

        public MarketPlaceServiceResponse UploadVirtualAppliance(VirtualAppliance vAppliance, byte[] byteContent)
        {
            return null;
        }

        public MarketPlaceServiceResponse ChangeDescriptionOfVirtualMachine(VirtualMachine vMachine)
        {
            return null;
        }

        public MarketPlaceServiceResponse ChangeDescriptionOfVirtualAppliance(VirtualAppliance vAppliance)
        {
            return null;
        }

        public DownloadVirtualMachineResponse DownloadVirtualMachine(VirtualMachine vMachine)
        {
            return null;
        }

        public DownloadVirtualApplianceResponse DownloadVirtualAppliance(VirtualAppliance vAppliance)
        {
            return null;
        }

        public MarketPlaceServiceResponse RateVirtualMachine(VirtualMachine vMachineRate, byte rate)
        {
            return null;
        }

        public MarketPlaceServiceResponse CommentVirtualMachine(VirtualMachine virtualMachineComment, string comment)
        {
            return null;
        }

        public List<VirtualMachine> SearchForSpecificVirtualMachine(VirtualMachine specificVMachine)
        {
            return null;
        }

        public List<VirtualAppliance> SearchForSpecificVirtualAppliance(VirtualAppliance specificVAppliance)
        {
            return null;
        }

        public MarketPlaceServiceResponse StartInstance(VirtualMachineInstance vMachineInstanceToStart)
        {
            // Gets an authenticated user
            IdentityObject identity = this.AuthenticateOnOpenStackCloud();
            IComputeService computeService = new ComputeService(this.computeServiceURL);
            string answer = computeService.StartServer(identity.Access.Token.Tenant.Id, vMachineInstanceToStart.InstanceID, identity);

            // TODO create response
            
            return null;
        }

        public MarketPlaceServiceResponse StopInstance(VirtualMachineInstance vMachineInstanceToStop)
        {
            // Gets an authenticated user
            IdentityObject identity = this.AuthenticateOnOpenStackCloud();
            IComputeService computeService = new ComputeService(this.computeServiceURL);
            string answer = computeService.StartServer(identity.Access.Token.Tenant.Id, vMachineInstanceToStop.InstanceID, identity);

            // TODO create response

            return null;
        }

        public List<VirtualMachine> GetDummyVirtualMachines()
        {
            return this.virtualMachineList;
        }

        public List<VirtualAppliance> GetVirtualAppliances()
        {
            return this.virtualApplianceList;
        }

        public List<VirtualMachineInstance> GetVirtualMachineInstances()
        {
            return null;
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
