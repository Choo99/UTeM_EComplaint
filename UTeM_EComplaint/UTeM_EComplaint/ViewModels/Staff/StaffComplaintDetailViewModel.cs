using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.IO;
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
    internal class StaffComplaintDetailViewModel : ViewModelBase, IQueryAttributable
    {
        string complaintID;
        bool isOwner;
        bool isNotAssigned;
        bool isAssigned;
        bool isInProgress;
        bool isCompleted;
        bool isRating;
        bool isNotRating;
        bool isEdit;
        bool isOnlyCompleted;
        Complaint complaint;
        ImageSource image;

        string pathToAddRating = $"{nameof(StaffAddRatingPage)}?complaintID=";
        string pathToEditComplaint = $"{nameof(StaffEditComplaintPage)}?complaintID=";

        string duration;

        public Complaint Complaint { get => complaint; set => SetProperty(ref complaint,value) ; }
        public ImageSource Image { get => image; set => SetProperty(ref image, value) ; }
        public Map Map { get; private set; }
        public bool IsEdit { get => isEdit; set => SetProperty(ref isEdit, value) ; }
        public bool IsOwner { get => isOwner; set => SetProperty(ref isOwner, value) ; }
        public bool IsAssigned { get => isAssigned; set { SetProperty(ref isAssigned, value); IsNotAssigned = !value; } }
        public bool IsNotAssigned { get => isNotAssigned; set => SetProperty(ref isNotAssigned, value) ; }
        public bool IsInProgress { get => isInProgress; set => SetProperty(ref isInProgress, value) ; }
        public bool IsCompleted { get => isCompleted; set => SetProperty(ref isCompleted, value) ; }
        public bool IsNotRating { get => isNotRating; set => SetProperty(ref isNotRating, value);   }
        public bool IsRating { get => isRating; set { SetProperty(ref isRating, value); IsNotRating = !value; } }
        public bool IsOnlyCompleted { get => isOnlyCompleted; set { SetProperty(ref isOnlyCompleted, value); } }
        public string Duration { get => duration; set { SetProperty(ref duration, value); } }

        public AsyncCommand DoneCommand { get; }
        public AsyncCommand RateCommand { get; }
        public AsyncCommand EditCommand { get; }
        public StaffComplaintDetailViewModel()
        {
            Title = "Complaint Detail";

            DoneCommand = new AsyncCommand(Done);
            RateCommand = new AsyncCommand(Rate);
            EditCommand = new AsyncCommand(Edit);

            Map = new Map
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
            if(IsOwner && IsCompleted)
            {
                DependencyService.Get<INotificationHelper>().UnsubscribeFromTopic("staff", complaintID);
            }
            await Shell.Current.GoToAsync("..");
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            complaintID =HttpUtility.UrlDecode(query["complaintID"]);
            getComplaintDetail();
        }

        async void getComplaintDetail()
        {
            try
            {
                IsBusy = true;
                Complaint = await ComplaintServices.GetComplaintDetail(complaintID);
                if(Complaint.Staff.StaffID == Preferences.Get("userID", 0))
                {
                    IsOwner = true;
                }
                if (Complaint.ComplaintStatus == "Pending")
                {
                    IsNotAssigned = true;
                    if (isOwner)
                    {
                        IsEdit = true;
                    }
                }
                else if(Complaint.ComplaintStatus == "Assigned")
                {
                    IsAssigned = true;
                }
                else if(Complaint.ComplaintStatus == "In Progress")
                {
                    IsAssigned = true;
                    IsInProgress = true;
                }
                else if(Complaint.ComplaintStatus == "Completed")
                {
                    IsAssigned = true;
                    IsCompleted = true;
                    IsOnlyCompleted = true;
                    if(Complaint.Rating != null)
                    {
                        IsRating = true;
                    }
                    else if(IsOwner && Complaint.Rating == null)
                    {
                        IsRating = false;
                    }
                    Duration = DurationHandler.calculateDuration(Complaint.TotalDays);
                }

                if(Complaint.ImageBase64 != null)
                    Image = ImageHandler.LoadBase64(Complaint.ImageBase64);

                if(Complaint.Longitude != 0 && Complaint.Latitude != 0)
                {
                    Map.MoveToRegion(MapHandler.moveToLocation(Complaint.Latitude, Complaint.Longitude));
                }
                
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
