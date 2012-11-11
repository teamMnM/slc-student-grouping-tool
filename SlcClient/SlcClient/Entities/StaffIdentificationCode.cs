using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SlcClient.Entities
{
    public class StaffIdentificationCode
    {
        /// <summary>
        /// The organization code or name assigning the staff Identification Code.
        /// </summary>
        [StringLength(60)]
        public string assigningOrganizationCode { get; set; }

        /// <summary>
        /// A unique number or alphanumeric code assigned to a staff member by a school, school system, a state, or other agency or entity.
        /// </summary>
        [StringLength(60)]
        public string ID { get; set; }

        /// <summary>
        /// A coding scheme that is used for identification and record-keeping purposes by schools, social services, or other agencies to refer to a staff member.
        /// </summary>
        public StaffIdentificationSystemType identificationSystem { get; set; }
    }
}