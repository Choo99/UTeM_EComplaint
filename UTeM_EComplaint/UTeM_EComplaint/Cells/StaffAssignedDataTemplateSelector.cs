using System;
using System.Collections.Generic;
using System.Text;
using UTeM_EComplaint.Model;
using Xamarin.Forms;

namespace UTeM_EComplaint.Cells
{
    public class StaffAssignedDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate StaffAssignedSoftware { get; set; }
        public DataTemplate StaffAssignedHardware { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var complaint = item as Complaint;
            if (complaint.ComplaintType.ComplaintTypeCode == "S")
            {
                return StaffAssignedSoftware;
            }
            else
            {
                return StaffAssignedHardware;
            }
        }
    }
}
