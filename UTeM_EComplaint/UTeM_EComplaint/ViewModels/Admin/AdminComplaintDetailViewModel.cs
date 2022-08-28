using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UTeM_EComplaint.Model;
using UTeM_EComplaint.Services;
using UTeM_EComplaint.Tools;
using UTeM_EComplaint.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Map = Xamarin.Forms.Maps.Map;

namespace UTeM_EComplaint.ViewModels
{
    internal class AdminComplaintDetailViewModel : ViewModelBase, IQueryAttributable
    {
        string complaintID;
        bool isNotAssigned;
        bool isAssigned;
        bool isInProgress;
        bool isCompleted;
        bool isRating;
        bool isNotRating;
        bool isOnlyCompleted;
        bool isSoftware;
        bool isHardware;
        Complaint complaint;
        ImageSource image;
        Map map;

        string pathToAddRating = $"{nameof(StaffAddRatingPage)}?complaintID=";
        string pathToEditComplaint = $"{nameof(StaffEditComplaintPage)}?complaintID=";

        public Complaint Complaint { get => complaint; set => SetProperty(ref complaint, value); }
        public ImageSource Image { get => image; set => SetProperty(ref image, value); }
        public Map Map { get => map; set => SetProperty(ref map, value); }
        public bool IsAssigned { get => isAssigned; set { SetProperty(ref isAssigned, value); IsNotAssigned = !value; } }
        public bool IsNotAssigned { get => isNotAssigned; set => SetProperty(ref isNotAssigned, value); }
        public bool IsInProgress { get => isInProgress; set => SetProperty(ref isInProgress, value); }
        public bool IsCompleted { get => isCompleted; set => SetProperty(ref isCompleted, value); }
        public bool IsNotRating { get => isNotRating; set => SetProperty(ref isNotRating, value); }
        public bool IsRating { get => isRating; set { SetProperty(ref isRating, value); IsNotRating = !value; } }
        public bool IsOnlyCompleted { get => isOnlyCompleted; set { SetProperty(ref isOnlyCompleted, value); } }
        public bool IsSoftware { get => isSoftware; set { SetProperty(ref isSoftware, value); } }
        public bool IsHardware { get => isHardware; set { SetProperty(ref isHardware, value); } }

        public AsyncCommand DoneCommand { get; }
        public AsyncCommand RateCommand { get; }
        public AsyncCommand EditCommand { get; }
        public AdminComplaintDetailViewModel()
        {
            Title = "Complaint Detail";

            DoneCommand = new AsyncCommand(Done);
            RateCommand = new AsyncCommand(Rate);
            EditCommand = new AsyncCommand(Edit);

            Map = new Map()
            {
                IsEnabled = false
            };
        }

        private async Task Edit()
        {
            await Shell.Current.GoToAsync(pathToEditComplaint + complaintID);
        }

        private async Task Rate()
        {
            try
            {
                await Shell.Current.GoToAsync(pathToAddRating + complaintID);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }

        }

        private async Task Done()
        {
            await Shell.Current.GoToAsync("..");
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            complaintID = HttpUtility.UrlDecode(query["complaintID"]);
            getComplaintDetail();
        }

        async void getComplaintDetail()
        {
            try
            {
                IsBusy = true;
                Complaint = await ComplaintServices.GetComplaintAndComplaintDetail(complaintID);
                
                if (Complaint.ComplaintStatus == "Pending")
                {
                    IsNotAssigned = true;
                }
                else if (Complaint.ComplaintStatus == "Assigned")
                {
                    IsAssigned = true;
                }
                else if (Complaint.ComplaintStatus == "In Progress")
                {
                    IsAssigned = true;
                    IsInProgress = true;
                }
                else if (Complaint.ComplaintStatus == "Completed")
                {
                    IsAssigned = true;
                    IsInProgress = true;
                    IsCompleted = true;
                    IsOnlyCompleted = true;
                }
                if(Complaint.ComplaintType.ComplaintTypeCode == "S")
                {
                    IsSoftware = true;
                }
                else
                {
                    IsHardware = true;
                }
                if (Complaint.Longitude != 0 && Complaint.Latitude != 0)
                    Map.MoveToRegion(MapHandler.moveToLocation(Complaint.Latitude, Complaint.Longitude));
                if (Complaint.ImageBase64 != null)
                    Image = ImageHandler.LoadBase64(Complaint.ImageBase64);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
