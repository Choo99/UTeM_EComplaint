using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UTeM_EComplaint.Model
{
    public class ComplaintDetail
    {
        public Complaint Complaint;
        public Technician Technician;
        public string SssignedDate;
        public string CompletedDate;
        public string Supervisor;
        public string ComplaintDetailStatus;
    }
}