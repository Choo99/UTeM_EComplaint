using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using UTeM_EComplaint.Model;
using UTeM_EComplaint.Services;
using UTeM_EComplaint.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace UTeM_EComplaint.ViewModels
{
    internal class JobEditViewModel : ViewModelBase, IQueryAttributable
    {
        public AsyncCommand SaveCommand { get; }
        public AsyncCommand ClearCommand { get; }

        ActionTaken actionTaken;
        public ActionTaken ActionTaken { get => actionTaken; set => SetProperty(ref actionTaken, value); }

        Complaint complaint;
        string complaintID;
        string pathToCompleted = $"../{nameof(JobCompletedDetailPage)}?complaintID=";
        public JobEditViewModel()
        {
            Title = "Task Information";
            actionTaken = new ActionTaken();
            SaveCommand = new AsyncCommand(Save);
            ClearCommand = new AsyncCommand(Clear);

            complaint = new Complaint();
        }

        private async Task Clear()
        {
            var answer = await Application.Current.MainPage.DisplayAlert("Clear", "Are you sure you want to clear the dtaa?", "YES", "NO");
            if (answer)
            {

                ActionTaken newAction = new ActionTaken
                {
                    StartDate = ActionTaken.StartDate,
                    EndDate = ActionTaken.EndDate,
                };

                ActionTaken = newAction;
            }
        }

        private async Task Save()
        {
            try
            {
                var answer = await Application.Current.MainPage.DisplayAlert("Save", "Are you sure you want to save it?", "YES", "NO");
                if (answer)
                {
                    complaint.Technician = new Technician
                    {
                        TechnicianID = Preferences.Get("userID", 0),
                    };
                    int result = await ActionServices.EditAction(complaint);
                    if (result != 0)
                    {
                        await Application.Current.MainPage.DisplayAlert("Success", "Update action successfully!", null, "OK");
                        await Shell.Current.GoToAsync("..");
                    }
                    else
                    {
                        throw new Exception("Some error in database");
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            complaintID =HttpUtility.UrlDecode(query["complaintID"]);
            getComplaintDetail();
        }

        private async void getComplaintDetail()
        {
            try
            {
                IsBusy = true;
                complaint = await ComplaintServices.GetComplaintDetail(complaintID);
                ActionTaken = complaint.Action;
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
