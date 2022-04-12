using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UTeM_EComplaint.Model;
using UTeM_EComplaint.Services;
using UTeM_EComplaint.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace UTeM_EComplaint.ViewModels
{
    internal class StaffComplaintDetailViewModel : ViewModelBase, IQueryAttributable
    {
        int complaintID;
        bool isOwner;
        bool isNotAssigned;
        bool isAssigned;
        bool isInProgress;
        bool isCompleted;
        bool isRating;
        bool isNotRating;
        bool isEdit;
        Complaint complaint;

        string pathToAddRating = $"{nameof(StaffAddRatingPage)}?complaintID=";
        string pathToEditComplaint = $"{nameof(StaffEditComplaintPage)}?complaintID=";

        public Complaint Complaint { get => complaint; set => SetProperty(ref complaint,value) ; }
        public bool IsEdit { get => isEdit; set => SetProperty(ref isEdit, value) ; }
        public bool IsOwner { get => isOwner; set => SetProperty(ref isOwner, value) ; }
        public bool IsAssigned { get => isAssigned; set { SetProperty(ref isAssigned, value); IsNotAssigned = !value; } }
        public bool IsNotAssigned { get => isNotAssigned; set => SetProperty(ref isNotAssigned, value) ; }
        public bool IsInProgress { get => isInProgress; set => SetProperty(ref isInProgress, value) ; }
        public bool IsCompleted { get => isCompleted; set => SetProperty(ref isCompleted, value) ; }
        public bool IsNotRating { get => isNotRating; set => SetProperty(ref isNotRating, value);   }
        public bool IsRating { get => isRating; set { SetProperty(ref isRating, value); IsNotRating = !value; } }

        public AsyncCommand DoneCommand { get; }
        public AsyncCommand RateCommand { get; }
        public AsyncCommand EditCommand { get; }
        public StaffComplaintDetailViewModel()
        {
            Title = "Complaint Detail";

            DoneCommand = new AsyncCommand(Done);
            RateCommand = new AsyncCommand(Rate);
            EditCommand = new AsyncCommand(Edit);
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
            complaintID = int.Parse(HttpUtility.UrlDecode(query["complaintID"]));
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
                    IsInProgress = true;
                    IsCompleted = true;
                    if(Complaint.Rating != null)
                    {
                        IsRating = true;
                    }
                    else if(IsOwner && Complaint.Rating == null)
                    {
                        IsRating = false;
                    }
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
