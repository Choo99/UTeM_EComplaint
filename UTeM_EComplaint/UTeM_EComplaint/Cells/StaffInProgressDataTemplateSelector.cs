using System;
using System.Collections.Generic;
using System.Text;
using UTeM_EComplaint.Model;
using Xamarin.Forms;

namespace UTeM_EComplaint.Cells
{
    public class StaffInProgressDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate StaffInProgressSoftware { get; set; }
        public DataTemplate StaffInProgressHardware { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var complaint = item as Complaint;
            if (complaint.ComplaintType.ComplaintTypeCode == "S")
            {
                return StaffInProgressSoftware;
            }
            else
            {
                return StaffInProgressHardware;
            }
        }
    }
}
