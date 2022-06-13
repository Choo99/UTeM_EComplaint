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
    internal class AdminComplaintDetailViewModel : ViewModelBase, IQueryAttributable
    {
        int complaintID;
        bool isNotAssigned;
        bool isAssigned;
        bool isInProgress;
        bool isCompleted;
        bool isRating;
        bool isNotRating;
        bool isOnlyCompleted;
        Complaint complaint;

        string pathToAddRating = $"{nameof(StaffAddRatingPage)}?complaintID=";
        string pathToEditComplaint = $"{nameof(StaffEditComplaintPage)}?complaintID=";

        public Complaint Complaint { get => complaint; set => SetProperty(ref complaint, value); }
        public bool IsAssigned { get => isAssigned; set { SetProperty(ref isAssigned, value); IsNotAssigned = !value; } }
        public bool IsNotAssigned { get => isNotAssigned; set => SetProperty(ref isNotAssigned, value); }
        public bool IsInProgress { get => isInProgress; set => SetProperty(ref isInProgress, value); }
        public bool IsCompleted { get => isCompleted; set => SetProperty(ref isCompleted, value); }
        public bool IsNotRating { get => isNotRating; set => SetProperty(ref isNotRating, value); }
        public bool IsRating { get => isRating; set { SetProperty(ref isRating, value); IsNotRating = !value; } }
        public bool IsOnlyCompleted { get => isOnlyCompleted; set { SetProperty(ref isOnlyCompleted, value); } }

        public AsyncCommand DoneCommand { get; }
        public AsyncCommand RateCommand { get; }
        public AsyncCommand EditCommand { get; }
        public AdminComplaintDetailViewModel()
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
                    if (Complaint.Rating != null)
                    {
                        IsRating = true;
                    }
                    else if (Complaint.Rating == null)
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
