using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UTeM_EComplaint.Model
{
    public class Department
    {
        public int DepartmentID { get; set; }
        public Level Level { get; set; }
        public string DepartmentName { get; set; }
        public string CodeLevel { get; set; }
    }
}