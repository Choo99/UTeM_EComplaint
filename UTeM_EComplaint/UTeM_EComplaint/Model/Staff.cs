using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UTeM_EComplaint.Model
{
    public class Staff
    {
        public int StaffID { get; set; }
        public string Name { get; set; }
        public string ResponsibilityCentre { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsOwner { get; set; }
        public string NotificationToken { get; set; }
    }
}