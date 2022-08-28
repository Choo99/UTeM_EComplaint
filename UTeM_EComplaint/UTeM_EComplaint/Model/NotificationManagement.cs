using System;
using System.Collections.Generic;
using System.Text;

namespace UTeM_EComplaint.Model
{
    public class NotificationManagement
    {
        public int NotificationID { get; set; }
        public string NotificationToken { get; set; }
        public User User { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}
