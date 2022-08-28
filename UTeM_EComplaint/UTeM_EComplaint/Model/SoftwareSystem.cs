using System;
using System.Collections.Generic;
using System.Text;

namespace UTeM_EComplaint.Model
{
    public class SoftwareSystem
    {
        public int SoftwareSystemID { get; set; }
        public string SystemCode { get; set; }
        public string Abbreviation { get; set; }
        public string SoftwareSystemName { get; set; }
        public bool Status { get; set; }
        public string DisplaySoftwareSystem
        {
            get => $"{Abbreviation} - {SoftwareSystemName}";
        }
    }
}
