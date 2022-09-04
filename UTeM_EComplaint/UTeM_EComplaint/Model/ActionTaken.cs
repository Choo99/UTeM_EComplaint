using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UTeM_EComplaint.Model
{
    public class ActionTaken
    {
        private double totaldays;
        public Complaint Complaint { get; set; }
        public Technician Technician { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ActionDescription { get; set; }
        public string SpareReplace { get; set; }
        public string AdditionalNote { get; set; }
        public double TotalDays {
            get => totaldays;
            set
            {
                var tempdays = value;
                var days = Math.Truncate(tempdays);
                var hoursInDay = value - days;

                var hours = Math.Truncate(hoursInDay * 24);
                var minutes = hours * 60;

                if (days > 0)
                    Duration = String.Format("{0} days {1} hours", days, hours);
                else if (days == 0 && hours > 0)
                    Duration = String.Format("{0} hours", hours);
                else
                    Duration = String.Format("{0} minutes", minutes);

                totaldays = value;
            }
        }
        public string Duration { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}