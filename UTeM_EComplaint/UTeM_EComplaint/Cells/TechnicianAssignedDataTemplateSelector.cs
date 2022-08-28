using System;
using System.Collections.Generic;
using System.Text;
using UTeM_EComplaint.Model;
using Xamarin.Forms;

namespace UTeM_EComplaint.Cells
{
    internal class TechnicianAssignedDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TechnicianAssignedSoftware { get; set; }
        public DataTemplate TechnicianAssignedHardware { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var complaintDetail = item as ComplaintDetail;
            if (complaintDetail.Complaint.ComplaintType.ComplaintTypeCode == "S")
            {
                return TechnicianAssignedSoftware;
            }
            else
            {
                return TechnicianAssignedHardware;
            }
        }
    }
}
