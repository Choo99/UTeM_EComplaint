using System;
using System.Collections.Generic;
using System.Text;

namespace UTeM_EComplaint.Model
{
    public class Submenu
    {
        public int SubmenuID { get; set; }
        public Submodule Submodule { get; set; }
        public string SubmenuCode { get; set; }
        public string SubmenuName { get; set; }
        public bool Status { get; set; }
        public string DisplaySubmenu
        {
            get => $"{SubmenuCode} - {SubmenuName}";
        }
    }
}
