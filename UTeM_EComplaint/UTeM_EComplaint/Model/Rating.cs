using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UTeM_EComplaint.Model
{
    public class Rating
    {
        public int RatingID { get; set; }
        public int RatingValue { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}