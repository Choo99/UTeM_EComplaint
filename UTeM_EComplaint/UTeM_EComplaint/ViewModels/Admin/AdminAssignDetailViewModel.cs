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
    internal class AdminAssignDetailPage : ViewModelBase, IQueryAttributable
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

        string pathToAssignTechnician = $"{nameof(AdminAssignTechnicianViewModel)}";

        public Complaint Complaint { get => complaint; set => SetProperty(ref complaint, value); }
        public bool IsAssigned { get => isAssigned; set { SetProperty(ref isAssigned, value); IsNotAssigned = !value; } }
        public bool IsNotAssigned { get => isNotAssigned; set => SetProperty(ref isNotAssigned, value); }
        public bool IsInProgress { get => isInProgress; set => SetProperty(ref isInProgress, value); }
        public bool IsCompleted { get => isCompleted; set => SetProperty(ref isCompleted, value); }
        public bool IsNotRating { get => isNotRating; set => SetProperty(ref isNotRating, value); }
        public bool IsRating { get => isRating; set { SetProperty(ref isRating, value); IsNotRating = !value; } }
        public bool IsOnlyCompleted { get => isOnlyCompleted; set { SetProperty(ref isOnlyCompleted, value); } }

        public AsyncCommand AssignCommand { get; }
        public AsyncCommand DoneCommand { get; }
        public AdminComplaintDetailViewModel()
        {
            Title = "Complaint Detail";

            DoneCommand = new AsyncCommand(Done);
            AssignCommand = new AsyncCommand(Assign);
        }

        private async Task Assign()
        {
            await Shell.Current.GoToAsync($"{nameof(AdminAssignTechnicianPage)}");
        }

        private async Task Done()
        {

            await Shell.Current.GoToAsync("..");
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if (query.ContainsKey("complaintID"))
            {
                complaintID = int.Parse(HttpUtility.UrlDecode(query["complaintID"]));
                getComplaintDetail();
            }
            if (query.ContainsKey("TechnicianID"))
            {
                complaintID = int.Parse(HttpUtility.UrlDecode(query["TechnicianID"]));
            }
        }

        void getStaffDetail()
        {

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
