using System;
using System.Collections.Generic;
using System.Text;

namespace UTeM_EComplaint.Model
{
    public class Module
    {
        public int ModuleID { get; set; }
        public SoftwareSystem System { get; set; }
        public string ModuleCode { get; set; }
        public string ModuleName { get; set; }
        public bool Status { get; set; }
        public string DisplayModule
        {
            get => $"{ModuleCode} - {ModuleName}";
        }
    }
}
