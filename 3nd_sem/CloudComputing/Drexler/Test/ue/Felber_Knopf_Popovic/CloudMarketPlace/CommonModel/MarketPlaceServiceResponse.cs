// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MarketPlaceServiceResponse.cs" company="FHWN">
//   Felber, Knopf, Popovic
// </copyright>
// <summary>
//   The market place service response.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CommonModel
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The market place service response.
    /// If Error is false - ErrorMessage is null
    /// If Error is true - The specific error is stored in ErrorMessage
    /// </summary>
    [DataContract]
    public class MarketPlaceServiceResponse
    {
        /// <summary>
        /// Gets or sets a value indicating whether error.
        /// </summary>
        [DataMember]
        public bool Error { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        [DataMember]
        public string ErrorMessage { get; set; }
    }
}