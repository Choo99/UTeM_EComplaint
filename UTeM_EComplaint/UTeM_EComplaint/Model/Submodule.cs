using System;
using System.Collections.Generic;
using System.Text;

namespace UTeM_EComplaint.Model
{
    public class Submodule
    {
        public int SubmoduleID { get; set; }
        public Module Module { get; set; }
        public string SubmoduleCode { get; set; }
        public string SubmoduleName { get; set; }
        public bool Status { get; set; }
        public string DisplaySubmodule
        {
            get => $"{SubmoduleCode} - {SubmoduleName}";
        }
    }
}
