using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UTeM_EComplaint.Model
{
    public class Level
    {
        public int LevelID { get; set; }
        public Building Building { get; set; }
        public string LevelName { get; set; }
        public string CodeLevel { get; set; }
    }
}