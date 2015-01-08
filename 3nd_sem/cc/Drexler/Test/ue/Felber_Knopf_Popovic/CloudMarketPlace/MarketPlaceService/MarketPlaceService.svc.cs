// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MarketPlaceService.svc.cs" company="FHWN">
//   Felber Knopf Popovic
// </copyright>
// <summary>
//   Defines the MarketPlaceService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MarketPlaceService
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.ServiceModel;

    using Amazon;
    using Amazon.EC2;
    using Amazon.EC2.Model;
    using CommonModel;

    /// <summary>
    /// The MarketPlaceService
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
        /// The upload virtual machine.
        /// </summary>
        /// <param name="virtualMachine">
        /// The virtual Machine.
        /// </param>
        /// <param name="byteArray">
        /// The byte Array.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public MarketPlaceServiceResponse UploadVirtualMachine(VirtualMachine virtualMachine, byte[] byteArray)
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
                    fs.Write(byteArray, 0, byteArray.Length);
                }
                
                virtualMachine.ImageId = fileCount.ToString(CultureInfo.InvariantCulture);
                this.virtualMachineList.Add(virtualMachine);

                marketPlaceServiceResponse.Error = false;
            }
            catch (Exception ex)
            {
                marketPlaceServiceResponse.Error = true;
                marketPlaceServiceResponse.ErrorMessage = ex.Message;
            }

            return marketPlaceServiceResponse;

            /* try
            {
                // Run new instance with the passed image id
                RunInstancesResponse runResponse = 
                    AWSClientFactory.CreateAmazonEC2Client().RunInstances(
                    new RunInstancesRequest()
                {
                    ImageId = imageId,
                    MinCount = 1,
                    MaxCount = 1,
                    InstanceType = InstanceType.M1Small
                });

                return runResponse.HttpStatusCode == HttpStatusCode.OK;
            }
            catch (Exception)
            {
                return false;
            }*/
        }

        /// <summary>
        /// The upload virtual appliance.
        /// </summary>
        /// <param name="virtualAppliance">
        /// The virtual Appliance.
        /// </param>
        /// <param name="byteArray">
        /// The dummy file
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public MarketPlaceServiceResponse UploadVirtualAppliance(VirtualAppliance virtualAppliance, byte[] byteArray)
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
                    fs.Write(byteArray, 0, byteArray.Length);
                }

                virtualAppliance.ImageId = fileCount.ToString(CultureInfo.InvariantCulture);
                this.virtualApplianceList.Add(virtualAppliance);

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
        /// The change description of virtual machine.
        /// </summary>
        /// <param name="newVirtualMachineDescription">
        /// The update Virtual Machine.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public MarketPlaceServiceResponse ChangeDescriptionOfVirtualMachine(VirtualMachine newVirtualMachineDescription)
        {
            var marketPlaceServiceResponse = new MarketPlaceServiceResponse() { Error = true };

            if (this.virtualMachineList.Any())
            {
                foreach (VirtualMachine tmpVirtualMachine in this.virtualMachineList.Where(tmpVirtualMachine => tmpVirtualMachine.ImageId == newVirtualMachineDescription.ImageId))
                {
                    tmpVirtualMachine.Comment = newVirtualMachineDescription.Comment;
                    tmpVirtualMachine.OperatingSystemName = newVirtualMachineDescription.OperatingSystemName;
                    tmpVirtualMachine.OperatingSystemType = newVirtualMachineDescription.OperatingSystemType;
                    tmpVirtualMachine.OperatingSystemVersion = newVirtualMachineDescription.OperatingSystemVersion;
                    tmpVirtualMachine.Rate = newVirtualMachineDescription.Rate;
                    tmpVirtualMachine.RecommendedCpu = newVirtualMachineDescription.RecommendedCpu;
                    tmpVirtualMachine.RecommendedMemory = newVirtualMachineDescription.RecommendedMemory;
                    tmpVirtualMachine.Size = newVirtualMachineDescription.Size;
                    tmpVirtualMachine.SupportedVirtualizationPlatforms = newVirtualMachineDescription.SupportedVirtualizationPlatforms;

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
                    string.Format(
                        "Virtualmachine with imageID {0} is not stored on our MarketPlace!",
                        newVirtualMachineDescription.ImageId);
            }

            return marketPlaceServiceResponse;
        }

        /// <summary>
        /// The change description of virtual appliance.
        /// </summary>
        /// <param name="newVirtualApplianceDescription">
        /// The new Virtual Appliance Description.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public MarketPlaceServiceResponse ChangeDescriptionOfVirtualAppliance(VirtualAppliance newVirtualApplianceDescription)
        {
            var marketPlaceServiceResponse = new MarketPlaceServiceResponse() { Error = true };

            if (this.virtualApplianceList.Any())
            {
                foreach (VirtualAppliance tmpVirtualAppliance in this.virtualApplianceList.Where(tmpVirtualAppliance => tmpVirtualAppliance.ImageId == newVirtualApplianceDescription.ImageId))
                {
                    tmpVirtualAppliance.Comment = newVirtualApplianceDescription.Comment;
                    tmpVirtualAppliance.OperatingSystemName = newVirtualApplianceDescription.OperatingSystemName;
                    tmpVirtualAppliance.OperatingSystemType = newVirtualApplianceDescription.OperatingSystemType;
                    tmpVirtualAppliance.OperatingSystemVersion = newVirtualApplianceDescription.OperatingSystemVersion;
                    tmpVirtualAppliance.Rate = newVirtualApplianceDescription.Rate;
                    tmpVirtualAppliance.RecommendedCpu = newVirtualApplianceDescription.RecommendedCpu;
                    tmpVirtualAppliance.RecommendedMemory = newVirtualApplianceDescription.RecommendedMemory;
                    tmpVirtualAppliance.Size = newVirtualApplianceDescription.Size;
                    tmpVirtualAppliance.SupportedVirtualizationPlatforms = newVirtualApplianceDescription.SupportedVirtualizationPlatforms;

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
                        "Virtual appliance with imageID {0} is not stored on our MarketPlace!",
                        newVirtualApplianceDescription.ImageId);
            }

            return marketPlaceServiceResponse;
        }

        /// <summary>
        /// The download virtual machine.
        /// </summary>
        /// <param name="virtualMachine">
        /// The virtual Machine.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public DownloadVirtualMachineResponse DownloadVirtualMachine(VirtualMachine virtualMachine)
        {
            var downloadVirtualMachineResponse = new DownloadVirtualMachineResponse() { Error = true };

            try
            {
                string tmpFilePath = @"C:\MarketPlaceVirtualMachines\";

                int fileCount = Directory.GetFiles(tmpFilePath, "*.*", SearchOption.AllDirectories).Length;

                int tmpImageId;
                if (int.TryParse(virtualMachine.ImageId, out tmpImageId))
                {
                    if (fileCount >= tmpImageId)
                    {
                        if (!Directory.Exists(tmpFilePath))
                        {
                            Directory.CreateDirectory(tmpFilePath);
                        }

                        tmpFilePath += virtualMachine.ImageId;

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
                            "The virtual machine image with ImageID '{0}' is not stored in our market place.",
                            virtualMachine.ImageId);
                    }
                }
                else
                {
                    downloadVirtualMachineResponse.Error = true;
                    downloadVirtualMachineResponse.ErrorMessage = string.Format(
                        "Couldn't convert imageID '{0}' to int.",
                        virtualMachine.ImageId);
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
        /// The download virtual appliance.
        /// </summary>
        /// <param name="virtualAppliance">
        /// The virtual Appliance.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// Implementation soon.
        /// </exception>
        public DownloadVirtualApplianceResponse DownloadVirtualAppliance(VirtualAppliance virtualAppliance)
        {
            var downloadVirtualApplianceResponse = new DownloadVirtualApplianceResponse() { Error = true };

            try
            {
                string tmpFilePath = @"C:\MarketPlaceVirtualAppliances\";

                int fileCount = Directory.GetFiles(tmpFilePath, "*.*", SearchOption.AllDirectories).Length;

                int tmpImageId;
                if (int.TryParse(virtualAppliance.ImageId, out tmpImageId))
                {
                    if (fileCount >= tmpImageId)
                    {
                        if (!Directory.Exists(tmpFilePath))
                        {
                            Directory.CreateDirectory(tmpFilePath);
                        }

                        tmpFilePath += virtualAppliance.ImageId;

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
                            "The virtual appliance image with ImageID '{0}' is not stored in our market place.",
                            virtualAppliance.ImageId);
                    }
                }
                else
                {
                    downloadVirtualApplianceResponse.Error = true;
                    downloadVirtualApplianceResponse.ErrorMessage = string.Format(
                        "Couldn't convert imageID '{0}' to int.",
                        virtualAppliance.ImageId);
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
        /// The rate virtual machine.
        /// </summary>
        /// <param name="virtualMachineToRate">
        /// The virtual machine to rate.
        /// </param>
        /// <param name="rate">
        /// The rate.
        /// </param>
        /// <returns>
        /// The <see cref="MarketPlaceServiceResponse"/>.
        /// </returns>
        public MarketPlaceServiceResponse RateVirtualMachine(VirtualMachine virtualMachineToRate, byte rate)
        {
            var marketPlaceServiceResponse = new MarketPlaceServiceResponse() { Error = true };

            foreach (VirtualMachine tmpVirtualMachine in this.virtualMachineList)
            {
                if (tmpVirtualMachine.ImageId == virtualMachineToRate.ImageId)
                {
                    tmpVirtualMachine.Rate = rate;
                    marketPlaceServiceResponse.Error = false;
                }
            }

            if (marketPlaceServiceResponse.Error)
            {
                marketPlaceServiceResponse.ErrorMessage = string.Format(
                    "Cant't find Virtual machine with ImageID '{0}'",
                    virtualMachineToRate.ImageId);
            }

            return marketPlaceServiceResponse;
        }

        /// <summary>
        /// The comment virtual machine.
        /// </summary>
        /// <param name="virtualMachineToComment">
        /// The virtual machine to comment.
        /// </param>
        /// <param name="comment">
        /// The comment.
        /// </param>
        /// <returns>
        /// The <see cref="MarketPlaceServiceResponse"/>.
        /// </returns>
        public MarketPlaceServiceResponse CommentVirtualMachine(VirtualMachine virtualMachineToComment, string comment)
        {
            var marketPlaceServiceResponse = new MarketPlaceServiceResponse() { Error = true };

            foreach (VirtualMachine tmpVirtualMachine in this.virtualMachineList)
            {
                if (tmpVirtualMachine.ImageId == virtualMachineToComment.ImageId)
                {
                    tmpVirtualMachine.Comment = comment;
                    marketPlaceServiceResponse.Error = false;
                }
            }

            if (marketPlaceServiceResponse.Error)
            {
                marketPlaceServiceResponse.ErrorMessage = string.Format(
                    "Cant't find Virtual machine with ImageID '{0}'",
                    virtualMachineToComment.ImageId);
            }

            return marketPlaceServiceResponse;
        }

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
        public List<VirtualMachine> SearchForSpecificVirtualMachine(VirtualMachine specificVirtualMachine)
        {
            var virtualMachines = this.virtualMachineList;

            if (!string.IsNullOrEmpty(specificVirtualMachine.OperatingSystemType))
            {
                virtualMachines = virtualMachines.Where(x => x.OperatingSystemType.Contains(specificVirtualMachine.OperatingSystemType)).ToList();
            }

            if (!string.IsNullOrEmpty(specificVirtualMachine.OperatingSystemName))
            {
                virtualMachines = virtualMachines.Where(x => x.OperatingSystemName.Contains(specificVirtualMachine.OperatingSystemName)).ToList();
            }

            if (!string.IsNullOrEmpty(specificVirtualMachine.OperatingSystemVersion))
            {
                virtualMachines = virtualMachines.Where(x => x.OperatingSystemVersion.Contains(specificVirtualMachine.OperatingSystemVersion)).ToList();
            }

            return virtualMachines;
        }

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
        public List<VirtualAppliance> SearchForSpecificVirtualAppliance(VirtualAppliance specificVirtualAppliance)
        {
            var virtualAppliances = this.virtualApplianceList;

            if (!string.IsNullOrEmpty(specificVirtualAppliance.OperatingSystemType))
            {
                virtualAppliances = virtualAppliances.Where(x => x.OperatingSystemType.Contains(specificVirtualAppliance.OperatingSystemType)).ToList();
            }

            if (!string.IsNullOrEmpty(specificVirtualAppliance.OperatingSystemName))
            {
                virtualAppliances = virtualAppliances.Where(x => x.OperatingSystemName.Contains(specificVirtualAppliance.OperatingSystemName)).ToList();
            }

            if (!string.IsNullOrEmpty(specificVirtualAppliance.OperatingSystemVersion))
            {
                virtualAppliances = virtualAppliances.Where(x => x.OperatingSystemVersion.Contains(specificVirtualAppliance.OperatingSystemVersion)).ToList();
            }

            return virtualAppliances;
        }

        /// <summary>
        /// The start instance.
        /// </summary>
        /// <param name="virtualMachineInstanceToStart">The <see cref="VirtualMachineInstance"/> which should be started.
        /// </param>
        /// <returns>
        /// True: If instance successfully started.
        /// </returns>
        public MarketPlaceServiceResponse StartInstance(VirtualMachineInstance virtualMachineInstanceToStart)
        {
            var marketPlaceServiceResponse = new MarketPlaceServiceResponse() { Error = true };

            try
            {
                // Print the number of Amazon EC2 instances.
                IAmazonEC2 ec2 = AWSClientFactory.CreateAmazonEC2Client();
                DescribeInstancesResponse ec2Response = ec2.DescribeInstances(new DescribeInstancesRequest());

                if (ec2Response.Reservations.Select(
                        reservation => reservation.Instances.SingleOrDefault(tempInstance => tempInstance.InstanceId == virtualMachineInstanceToStart.InstanceId)).Any(
                        tmpInstance => tmpInstance != null))
                {
                    StartInstancesResponse response = ec2.StartInstances(
                   new StartInstancesRequest()
                   {
                       InstanceIds = new List<string>() { virtualMachineInstanceToStart.InstanceId }
                   });

                    if (response.HttpStatusCode == HttpStatusCode.OK)
                    {
                        marketPlaceServiceResponse.Error = false;
                    }
                    else
                    {
                        marketPlaceServiceResponse.Error = true;
                        marketPlaceServiceResponse.ErrorMessage = string.Format("HTTPSTATUSCODE: '{0}'", response.HttpStatusCode);
                    }
                }
                else
                {
                    marketPlaceServiceResponse.Error = true;
                    marketPlaceServiceResponse.ErrorMessage =
                        string.Format("Instance with instanceID '{0}' couldn't be found.", virtualMachineInstanceToStart.InstanceId);
                }
            }
            catch (Exception ex)
            {
                marketPlaceServiceResponse.Error = true;
                marketPlaceServiceResponse.ErrorMessage = ex.Message;
            }

            return marketPlaceServiceResponse;
        }

        /// <summary>
        /// The stop instance.
        /// </summary>
        /// <param name="virtualMachineInstanceToStop">The <see cref="VirtualMachineInstance"/> which should be stopped.
        /// The instance id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        /// <exception cref="NotImplementedException">the implementation
        /// </exception>
        public MarketPlaceServiceResponse StopInstance(VirtualMachineInstance virtualMachineInstanceToStop)
        {
            var marketPlaceServiceResponse = new MarketPlaceServiceResponse() { Error = true };

            try
            {
                // Print the number of Amazon EC2 instances.
                IAmazonEC2 ec2 = AWSClientFactory.CreateAmazonEC2Client();
                var ec2Request = new DescribeInstancesRequest();

                DescribeInstancesResponse ec2Response = ec2.DescribeInstances(ec2Request);

                if (ec2Response.Reservations.Select(
                    reservation => reservation.Instances.SingleOrDefault(tempInstance => tempInstance.InstanceId == virtualMachineInstanceToStop.InstanceId)).Any(
                    tmpInstance => tmpInstance != null))
                {
                    StopInstancesResponse response = ec2.StopInstances(
                        new StopInstancesRequest()
                            {
                                InstanceIds = new List<string>() { virtualMachineInstanceToStop.InstanceId }
                            });

                    if (response.HttpStatusCode == HttpStatusCode.OK)
                    {
                        marketPlaceServiceResponse.Error = false;
                    }
                    else
                    {
                        marketPlaceServiceResponse.Error = true;
                        marketPlaceServiceResponse.ErrorMessage = string.Format("HTTPSTATUSCODE: '{0}'", response.HttpStatusCode);
                    }
                }
                else
                {
                    marketPlaceServiceResponse.Error = true;
                    marketPlaceServiceResponse.ErrorMessage =
                        string.Format("Instance with instanceID '{0}' couldn't be found.", virtualMachineInstanceToStop.InstanceId);
                }
            }
            catch (Exception ex)
            {
                marketPlaceServiceResponse.Error = true;
                marketPlaceServiceResponse.ErrorMessage = ex.Message;
            }

            return marketPlaceServiceResponse;
        }

        /// <summary>
        /// The get possible virtual machines to upload.
        /// </summary>
        /// <returns>
        /// List of <see cref="VirtualMachine"/>
        /// </returns>
        public List<VirtualMachine> GetDummyVirtualMachines()
        {
            return this.virtualMachineList;

            /*DescribeImagesResponse ec2Response =
                AWSClientFactory.CreateAmazonEC2Client()
                    .DescribeImages(
                        new DescribeImagesRequest
                            {
                                ImageIds =
                                    new List<string>()
                                        {
                                            "ami-5256b825",
                                            "ami-6a56b81d",
                                            "ami-8e987ef9",
                                            "ami-80987ef7",
                                            "ami-873ad7f0",
                                            "ami-db36dbac",
                                            "ami-1937da6e",
                                            "ami-833bd6f4",
                                            "ami-cd34d9ba",
                                            "ami-cf3ad7b8"
                                        }
                            });

            return
                ec2Response.Images.Select(
                    image =>
                    new VirtualMachine
                        {
                            ImageId = image.ImageId,
                            OperatingSystemType = string.Empty,
                            OperatingSystemName = image.Name,
                            OperatingSystemVersion = image.Architecture.Value,
                            Size = 0, // This is defined later. 
                            SupportedVirtualizationPlatforms = image.VirtualizationType.Value,
                            RecommendedCpu = 0, // This is defined later. 
                            RecommendedMemory = 0 // This is defined later. 
                        }).ToList();*/
        }

        /// <summary>
        /// The get virtual appliances.
        /// </summary>
        /// <returns>
        /// The <see>
        ///         <cref>List</cref>
        ///     </see>
        ///     .
        /// </returns>
        public List<VirtualAppliance> GetVirtualAppliances()
        {
            return this.virtualApplianceList;
        }

        /// <summary>
        /// The get virtual machines.
        /// </summary>
        /// <returns>
        /// The <see>
        ///         <cref>List</cref>
        ///     </see>
        ///     .
        /// </returns>
        public List<VirtualMachineInstance> GetVirtualMachineInstances()
        {
            // Print the number of Amazon EC2 instances.
            IAmazonEC2 ec2 = AWSClientFactory.CreateAmazonEC2Client();
            var ec2Request = new DescribeInstancesRequest();

            DescribeInstancesResponse ec2Response = ec2.DescribeInstances(ec2Request);

            return (from reservation in ec2Response.Reservations
                    from instance in reservation.Instances
                    select new VirtualMachineInstance
                    {
                        OperatingSystemType = string.Empty, // TODO: system type ??
                        OperatingSystemName = instance.Platform == null ? instance.VirtualizationType.Value : instance.Platform.Value,
                        OperatingSystemVersion = instance.Architecture.Value,
                        Size = this.InstanceTypeToSizeMapper(instance.InstanceType),

                        // HelpLink: http://nagarun.wordpress.com/2012/10/30/aws-virtualization-hvm-vs-paravirtualization/
                        // hvm (Hardware assisted virtual machine
                        // pv (Para-virtualization)
                        SupportedVirtualizationPlatforms = instance.VirtualizationType.Value, 
                        RecommendedCpu = this.StorageToRecommendedCpuMapper(instance.InstanceType),
                        RecommendedMemory = this.StorageToRecommendedMemoryMapper(instance.InstanceType),
                        ImageId = instance.ImageId,
                        InstanceId = instance.InstanceId
                    }).ToList();
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
    }
}
