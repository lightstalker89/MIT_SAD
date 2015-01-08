// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DownloadVirtualMachineResponse.cs" company="FHWN">
//   Felber, Knopf, Popovic
// </copyright>
// <summary>
//   The download virtual machine response.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CommonModel
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The <see cref="DownloadVirtualMachineResponse"/>.
    /// Holds the byteArray of virtual machine which should be downloaded.
    /// </summary>
    [DataContract]
    public class DownloadVirtualMachineResponse : MarketPlaceServiceResponse
    {
        /// <summary>
        /// Gets or sets the byte array.
        /// </summary>
        [DataMember]
        public byte[] ByteArray { get; set; }
    }
}
