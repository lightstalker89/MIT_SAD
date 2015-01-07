// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DownloadVirtualApplianceResponse.cs" company="FHWN">
//   Felber, Knopf, Popovic
// </copyright>
// <summary>
//   The download virtual appliance response.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CommonModel
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The <see cref="DownloadVirtualApplianceResponse"/>.
    /// Holds the byteArray of virtual appliance which should be downloaded.
    /// </summary>
    [DataContract]
    public class DownloadVirtualApplianceResponse : MarketPlaceServiceResponse
    {
        /// <summary>
        /// Gets or sets the byte array.
        /// </summary>
        [DataMember]
        public byte[] ByteArray { get; set; }
    }
}
