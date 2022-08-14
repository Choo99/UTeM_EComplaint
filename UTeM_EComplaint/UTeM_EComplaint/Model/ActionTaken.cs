using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UTeM_EComplaint.Model
{
    public class ActionTaken
    {
        public Complaint Complaint { get; set; }
        public Technician Technician { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ActionDescription { get; set; }
        public string SpareReplace { get; set; }
        public string AdditionalNote { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}