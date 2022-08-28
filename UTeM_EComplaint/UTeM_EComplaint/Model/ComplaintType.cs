using System;
using System.Collections.Generic;
using System.Text;

namespace UTeM_EComplaint.Model
{
    public class ComplaintType
    {
        public string ComplaintTypeCode { get; set; }
        public string ComplaintTypeName { get; set; }
        public string DisplayComplaintType
        {
            get => $"{ComplaintTypeCode} - {ComplaintTypeName}";
        }
    }
}
