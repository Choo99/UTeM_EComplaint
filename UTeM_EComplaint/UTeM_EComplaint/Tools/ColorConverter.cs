using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace UTeM_EComplaint.Tools
{
    internal class ColorConverter
    {
        public static Color StatusToColor(string status)
        {
            if (status == "Pending")
            {
                return Color.Red;
            }
            else if (status == "Assigned")
            {
                return Color.Yellow;
            }
            else if(status == "In Progress")
            {
                return Color.YellowGreen;
            }
            else if (status == "Completed")
            {
                return Color.Green;
            }
            else
            {
                throw new Exception("Status not match! Cannot find corresponding color");
            }
        }
    }
}
