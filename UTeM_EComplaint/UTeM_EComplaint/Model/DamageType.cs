using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UTeM_EComplaint.Model
{
    public class DamageType
    {
        public int DamageTypeID { get; set; }
        public string DamageTypeName { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}