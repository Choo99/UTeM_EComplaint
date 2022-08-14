using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UTeM_EComplaint.Model;
using UTeM_EComplaint.Services;
using UTeM_EComplaint.Views;
using Xamarin.Forms;

namespace UTeM_EComplaint.ViewModels
{
    internal class JobToDoDetailViewModel : ViewModelBase, IQueryAttributable
    {
        public Complaint Complaint { get => complaint; set => SetProperty(ref complaint, value); }

        public AsyncCommand StartJobCommand { get; }
        public AsyncCommand BackCommand { get; }

        Complaint complaint;
        string complaintID;

        string pathToInProgress = $"../{nameof(JobProgressPage)}";
        public JobToDoDetailViewModel()
        {
            Title = "Task Information";
            StartJobCommand = new AsyncCommand(StartJob);
            BackCommand = new AsyncCommand(Back);
        }

        private async Task Back()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        private async Task StartJob()
        {
            var answer = await Application.Current.MainPage.DisplayAlert("Start", "Are you sure you want to start the job?", "Yes", "No");

            if (answer)
            {
                try
                {
                    int result = await ActionServices.StartActionAndSendMessage(complaint.ComplaintID);

                    if (result != 0)
                    {
                        await Shell.Current.GoToAsync(pathToInProgress);
                        await Application.Current.MainPage.DisplayAlert("Success", "You start the job ID: " + complaint.ComplaintID + " successfully", "OK");
                    }
                }catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Opps! Database has some error. Please try again","OK");
                }
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            complaintID = HttpUtility.UrlDecode(query["complaintID"]);
            getComplaintDetail();
        }

        private async void getComplaintDetail()
        {
            try
            {
                Complaint = await ComplaintServices.GetComplaintDetail(complaintID);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
        }
    }
}
