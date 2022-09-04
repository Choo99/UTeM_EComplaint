using System;
using System.Collections.Generic;
using System.Text;
using UTeM_EComplaint.Model;
using Xamarin.Forms;

namespace UTeM_EComplaint.Cells
{
    public class StaffComplaintDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Pending { get; set; }
        public DataTemplate PendingSoftware { get; set; }
        public DataTemplate Assigned { get; set; }
        public DataTemplate AssignedSoftware { get; set; }
        public DataTemplate InProgress { get; set; }
        public DataTemplate InProgressSoftware { get; set; }
        public DataTemplate Completed { get; set; }
        public DataTemplate CompletedSoftware { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var complaint = item as Complaint;
            if(complaint.ComplaintStatus == "Pending" && complaint.ComplaintType.ComplaintTypeCode == "H")
            {
                return Pending;
            }
            else if(complaint.ComplaintStatus == "Pending" && complaint.ComplaintType.ComplaintTypeCode == "S")
            {
                return PendingSoftware;
            }
            else if (complaint.ComplaintStatus == "Assigned" && complaint.ComplaintType.ComplaintTypeCode == "H")
            {
                return Assigned;
            }
            else if (complaint.ComplaintStatus == "Assigned" && complaint.ComplaintType.ComplaintTypeCode == "S")
            {
                return AssignedSoftware;
            }
            else if (complaint.ComplaintStatus == "In Progress" && complaint.ComplaintType.ComplaintTypeCode == "H")
            {
                return InProgress;
            }
            else if (complaint.ComplaintStatus == "In Progress" && complaint.ComplaintType.ComplaintTypeCode == "S")
            {
                return InProgressSoftware;
            }
            else if (complaint.ComplaintStatus == "Completed" && complaint.ComplaintType.ComplaintTypeCode == "H")
            {
                return Completed;
            }
            else
            {
                return CompletedSoftware;
            }
        }
    }
}
