using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SlcClient.Entities
{
    /// <summary>
    /// This entity represents any program designed to work in conjunction with or to supplement the main academic program. 
    /// Programs may provide instruction, training, services or benefits through federal, state, or local agencies. 
    /// Programs may also include organized extracurricular activities for students.
    /// </summary>
    public class Program
    {
        [StringLength(20)]
        public string programId { get; set; }

        public string programSponsor { get; set; }

        public string programType { get; set; }

        public List<ServiceDescriptorType> services { get; set; }
    }
}