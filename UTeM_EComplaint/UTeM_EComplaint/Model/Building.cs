using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UTeM_EComplaint.Model
{
    public class Building
    {
        public int BuildingID { get; set; }
        public Campus Campus { get; set; }
        public string BuildingName { get; set; }
        public string CodeLevel { get; set; }
    }
}