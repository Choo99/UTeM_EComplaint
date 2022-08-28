using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xamarin.Forms;

namespace UTeM_EComplaint.Model
{
    public class Complaint : ObservableObject
    {
        private ComplaintType complaintType;
        private bool isHardware;
        private bool isSoftware;
        public string ComplaintID { get; set; }
        public ComplaintType ComplaintType {
            get => complaintType;
            set
            {
                complaintType = value;
                if(complaintType != null && complaintType.ComplaintTypeCode == "S")
                {
                    IsSoftware = true;
                }
                if (complaintType != null && complaintType.ComplaintTypeCode == "H")
                {
                    IsHardware = true;
                }
            }
        }
        public SoftwareSystem SoftwareSystem { get; set; }
        public Module Module { get; set; }
        public Submodule Submodule { get; set; }
        public Submenu Submenu { get; set; }
        public Division Division { get; set; }
        public Category Category { get; set; }
        public DamageType DamageType { get; set; }
        public Staff Staff { get; set; }
        public string ComplaintDate { get; set; }
        public string Damage { get; set; }
        public string Location { get; set; }
        public string ContactPhoneNumber { get; set; }
        public string ComplaintStatus { get; set; }
        public double TotalDays { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string ImageBase64 { get; set; }
        public string Duration { get; set; }
        public string Report { get; set; }
        public bool IsCompleted
        {
            get => ComplaintStatus == "Completed";
        }
        public bool IsHardware { get => isHardware; set { SetProperty(ref isHardware, value); } }
        public bool IsSoftware { get => isSoftware; set { SetProperty(ref isSoftware, value); } }
        public List<ComplaintDetail> ComplaintDetails { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}