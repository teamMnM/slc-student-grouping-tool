using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SlcClient.Entities
{
    public class EducationOrganization
    {
        public List<AccountabilityRating> accountabilityRatings { get; set; }
        
        public List<Address> addresses { get; set; }
        
        public string agencyHierarchyName { get; set; }
        
        public List<EducationOrgIdentificationCode> educationOrgIdentificationCode { get; set; }

        [StringLength(75)]
        public string nameOfInstituation { get; set; }

        public string oparationalStatus { get; set; }

        public List<string> organizationCategories { get; set; }

        public List<string> programReferences { get; set; }

        [StringLength(75)]
        public string shortNameOfInstituation { get; set; }

        [StringLength(60)]
        public string stateOrganizationId { get; set; }

        public InstitutionTelephone telephone { get; set; }

        [StringLength(80)] //(5-80)
        public string website { get; set; }

        public EducationOrganization parentEducationAgency { get; set; }
    }
}