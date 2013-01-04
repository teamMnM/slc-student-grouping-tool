using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamMnMGroupingWebApp.Models
{
    public class ViewDataUploadFilesResult
    {
        public string CohortId { get; set; }
        public string Name { get; set; }
        public int Length { get; set; }
        public string Type { get; set; }
        public bool isSuccess { get; set; }
    }
}