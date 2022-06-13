using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UTeM_EComplaint.Model
{
    public class Technician : User
    {
        public int TechnicianID { get; set; }
        public string TechnicianName { get; set; }

        //Aggregate field
        public double OverallRating { get; set; }
        public int CountRating { get; set; }
        public int CompletedTask { get; set; }
    }
}