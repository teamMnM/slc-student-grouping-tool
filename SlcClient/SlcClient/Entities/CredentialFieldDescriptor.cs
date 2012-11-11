using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlcClient.Entities
{
    /// <summary>
    /// This descriptor defines the fields of certification that the SEA offers to teachers.
    /// </summary>
    public class CredentialFieldDescriptor
    {
        /// <summary>
        /// A code or abbreviation that is used to refer to the certification field.
        /// </summary>
        public string codeValue { get; set; }

        /// <summary>
        /// The description of the service.
        /// </summary>
        public string description { get; set; }
    }
}