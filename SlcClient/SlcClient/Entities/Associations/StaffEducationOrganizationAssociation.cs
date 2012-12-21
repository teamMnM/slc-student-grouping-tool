using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlcClient.Entities.Associations
{
    public class StaffEducationOrganizationAssociation
    {
        /// <summary>
        /// Month, day and year of the start or effective date of a staff member's employment, contract or relationship with the LEA.
        /// </summary>
        public DateTime beginDate { get; set; }

        /// <summary>
        /// Month, day and year of the end or termination date of a staff member's employment, contract or relationship with the LEA.
        /// </summary>
        public DateTime endDate { get; set; }

        /// <summary>
        /// The descriptive name of an individual's position.
        /// </summary>
        public string positionTitle { get; set; }

        /// <summary>
        /// The titles of employment, official status, or rank of education staff.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public StaffClassificationType staffClassification { get; set; }

        /// <summary>
        /// The educational organization to which the staff member provides services
        /// </summary>
        public string educationOrganizationReference { get; set; }

        /// <summary>
        /// The staff assigned to the education organization.
        /// </summary>
        public string staffId { get; set; }
    }
}
