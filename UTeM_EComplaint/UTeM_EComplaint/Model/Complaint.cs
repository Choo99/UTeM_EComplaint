using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UTeM_EComplaint.Model
{
    public class Complaint
    {
        public int ComplaintID { get; set; }
        public Division Division { get; set; }
        public Category Category { get; set; }
        public DamageType DamageType { get; set; }
        public Staff Staff { get; set; }
        public Technician Technician { get; set; }
        public Rating Rating { get; set; }
        public ActionTaken Action { get; set; }
        public string ComplaintDate { get; set; }
        public string Damage { get; set; }
        public string Location { get; set; }
        public string ContactPhoneNumber { get; set; }
        public string ComplaintStatus { get; set; }
        public int TotalDays { get; set; }
        public bool IsRated { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}