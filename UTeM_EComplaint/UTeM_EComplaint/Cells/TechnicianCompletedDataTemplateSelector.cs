using System;
using System.Collections.Generic;
using System.Text;
using UTeM_EComplaint.Model;
using Xamarin.Forms;

namespace UTeM_EComplaint.Cells
{
    public class TechnicianCompletedDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TechnicianCompletedHardware { get; set; }
        public DataTemplate TechnicianCompletedSoftware { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var complaintDetail = item as ComplaintDetail;
            if (complaintDetail.Complaint.ComplaintType.ComplaintTypeCode == "S")
            {
                return TechnicianCompletedSoftware;
            }
            else
            {
                return TechnicianCompletedHardware;
            }
        }
    }
}
