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
        public DataTemplate Assigned { get; set; }
        public DataTemplate InProgress { get; set; }
        public DataTemplate Completed { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var complaint = item as Complaint;
            if(complaint.ComplaintStatus == "Pending")
            {
                return Pending;
            }
            else if (complaint.ComplaintStatus == "Assigned")
            {
                return Assigned;
            }
            else if (complaint.ComplaintStatus == "In Progress")
            {
                return InProgress;
            }
            else
            {
                return Completed;
            }
        }
    }
}
