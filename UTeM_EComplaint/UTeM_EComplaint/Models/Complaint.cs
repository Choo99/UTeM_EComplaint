using System;
using System.Collections.Generic;
using System.Text;

namespace UTeM_EComplaint.Models
{
    public class Complaint
    {
        public string IDAduan { get; set; }
        public string Problem_Aduan { get; set; }
        public string Date_Problem { get; set; }
        public string Ulasan_Aduan { get; set; }
        public string NoStaff { get; set; }
        public string NamaStaff { get; set; }
        public string Start_Date { get; set; }
        public string End_Date { get; set; }
        public string totaldays { get; set; }
        public string NamaJuruteknik { get; set; }
        public string NoJuruteknik { get; set; }
        public string Status { get; set; }
        public Boolean IsRating { get; set; }
        public string Bahagian { get; set; }
        public string Bahagian_Unit { get; set; }
        public string Category { get; set; }
        public string Location { get; set; }
        public string Contact_Number { get; set; }
        public string Action { get; set; }
        public string Replacement { get; set; }
        public string Note { get; set; }
        public Rating Rating { get; set; }
    }
}
