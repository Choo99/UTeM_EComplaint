using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UTeM_EComplaint.Model
{
    public class Room
    {
        public int RoomID { get; set; }
        public Department Department { get; set; }
        public string RoomName { get; set; }
        public string CodeLevel { get; set; }
    }
}