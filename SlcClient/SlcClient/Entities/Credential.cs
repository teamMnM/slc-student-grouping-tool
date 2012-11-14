using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SlcClient.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlcClient.Entities
{
    /// <summary>
    /// The legal document or authorization giving authorization to perform teaching assignment services.
    /// </summary>
    public class Credential
    {
        /// <summary>
        /// The month, day, and year on which an active credential held by an individual will expire.
        /// </summary>
        public DateTime credentialExpirationDate { get; set; }

        /// <summary>
        /// The field of certification for the certificate (e.g., Mathematics, Music)
        /// </summary>
        public CredentialFieldDescriptor credentialField { get; set; }

        /// <summary>
        /// The month, day, and year on which an active credential was issued to an individual.
        /// </summary>
        public DateTime credentialIssuanceDate { get; set; }

        /// <summary>
        /// An indication of the category of credential an individual holds.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public CredentialType credentialType { get; set; }

        /// <summary>
        /// The grade level(s) certified for teaching.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public LevelType level { get; set; }

        /// <summary>
        /// An indication of the pre-determined criteria for granting the teaching credential that an individual holds.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public TeachingCredentialBasisType teachingCredentialBasis { get; set; }

        /// <summary>
        /// An indication of the category of a legal document giving authorization to perform teaching assignment services.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public TeachingCredentialType teachingCredentialType { get; set; }
    }
}