using System;
using System.Collections.Generic;
using System.Text;
using UTeM_EComplaint.Model;

namespace UTeM_EComplaint.Tools
{
    internal class DurationHandler
    {
        public static void durationList(ref List<Complaint> complaints)
        {
            for (var i = 0; i < complaints.Count; i++)
            {
                var days = Math.Truncate(complaints[i].TotalDays);
                var hoursInDay = complaints[i].TotalDays - days;

                var hours = Math.Truncate(hoursInDay * 24);
                var minutes = hours * 60;
                
                if (days > 0)
                    complaints[i].Duration = String.Format("{0} days {1} hours", days, hours);
                else if (days == 0 && hours > 0)
                    complaints[i].Duration = String.Format("{0} hours", hours);
                else 
                    complaints[i].Duration = String.Format("{0} minutes", minutes);
            }
        }

        public static void durationList(ref List<ComplaintDetail> complaintDetails)
        {
            for (var i = 0; i < complaintDetails.Count; i++)
            {
                var days = Math.Truncate(complaintDetails[i].Action.TotalDays);
                var hoursInDay = complaintDetails[i].Action.TotalDays - days;

                var hours = Math.Truncate(hoursInDay * 24);
                var minutes = hours * 60;

                if (days > 0)
                    complaintDetails[i].Action.Duration = String.Format("{0} days {1} hours", days, hours);
                else if (days == 0 && hours > 0)
                    complaintDetails[i].Action.Duration = String.Format("{0} hours", hours);
                else
                    complaintDetails[i].Action.Duration = String.Format("{0} minutes", minutes);
            }
        }

        public static string calculateDuration(double totalDays)
        {
            string duration = null;

            var days = Math.Truncate(totalDays);
            var hoursInDay = totalDays - days;

            var hours = Math.Truncate(hoursInDay * 24);
            var minutes = hours * 60;

            if (days > 0)
                duration = String.Format("{0} days {1} hours", days, hours);
            else if (days == 0 && hours > 0)
                duration = String.Format("{0} hours", hours);
            else 
                duration = String.Format("{0} minutes", minutes);

            return duration;
        }
    }
}
