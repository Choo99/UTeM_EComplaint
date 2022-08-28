using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UTeM_EComplaint.Model
{
    public class ComplaintDetail 
    { 
        private string complaintDetailStatus;

        public Complaint Complaint { get; set; }
        public Technician Technician { get; set; }
        public string JobDescription { get; set; }
        public string AssignedDate { get; set; }
        public string CompletedDate { get; set; }
        public bool Supervisor { get; set; }
        public string ComplaintDetailStatus
        {
            get => complaintDetailStatus;
            set
            {
                complaintDetailStatus = value;
                if(value == "Assigned")
                {
                    IsAssigned = true;
                }
                else if (value == "In Progress")
                {
                    IsInProgress = true;
                }
                else if (value == "Completed")
                {
                   IsCompleted = true;
                }
                else if (value == "Rated")
                {
                    IsCompleted = true;
                    IsRated = true;
                }
            }
        }
        public Rating Rating { get; set; }
        public ActionTaken Action { get; set; }
        public bool IsAssigned { get; set; }
        public bool IsInProgress { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsSupervisorAndAllCompleted { get; set; }
        public bool IsRated { get; set; }
        public bool IsStart { get; set; }
    }
}