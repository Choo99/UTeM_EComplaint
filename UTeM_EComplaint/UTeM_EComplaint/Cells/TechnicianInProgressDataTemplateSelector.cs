using System;
using System.Collections.Generic;
using System.Text;
using UTeM_EComplaint.Model;
using Xamarin.Forms;

namespace UTeM_EComplaint.Cells
{
    public class TechnicianInProgressDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TechnicianInProgressHardware { get; set; }
        public DataTemplate TechnicianInProgresSoftware { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var complaintDetail = item as ComplaintDetail;
            if (complaintDetail.Complaint.ComplaintType.ComplaintTypeCode == "S")
            {
                return TechnicianInProgresSoftware;
            }
            else 
            {
                return TechnicianInProgressHardware;
            }
        }
    }
}
