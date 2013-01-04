using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamMnMGroupingWebApp.Models
{
    public class LessonPlan
    {
        /// <summary>
        /// the base64 string lesson plan to be stored in the custom entity
        /// </summary>
        /// We have decided to use FTP instead
        //public string content { get; set; }

        /// <summary>
        /// file type. e.g. application/pdf
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// name of the file
        /// </summary>
        public string name { get; set; }
    }
}