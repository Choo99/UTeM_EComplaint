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
    internal class JobInProgressDetailViewModel : ViewModelBase, IQueryAttributable
    {
        public Complaint Complaint { get => complaint; set => SetProperty(ref complaint, value); }
        public AsyncCommand FinishJobCommand { get; }
        public AsyncCommand BackCommand { get; }

        ActionTaken actionTaken;
        public ActionTaken ActionTaken { get => actionTaken; set => SetProperty(ref actionTaken, value); }

        Complaint complaint;
        string complaintID;
        string pathToCompleted = $"../{nameof(JobCompletedDetailPage)}?complaintID=";
        public JobInProgressDetailViewModel()
        {
            Title = "Task Information";
            actionTaken = new ActionTaken();
            FinishJobCommand = new AsyncCommand(FinishJob);
            BackCommand = new AsyncCommand(Back);
        }

        private async Task Back()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        private async Task FinishJob()
        {
            try
            {
                var answer = await Application.Current.MainPage.DisplayAlert("Finish", "Are you sure you want to finish the job?", "Yes", "No");
                if (answer)
                {
                    int result = 0;
                    //TODO:
                    //ActionTaken.ActionID = complaint.Action.ActionID;
                    Complaint newComplaint = new Complaint
                    {
                        ComplaintID = complaintID,
                        Action = actionTaken,
                    };

                    try
                    {
                        result = await ActionServices.EndActionAndSendNotification(newComplaint);
                    }
                    catch (Exception ex)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Opps! Something wrong with database. Please try again", "OK");
                    }
                    if (result == 1)
                    {
                        await Shell.Current.GoToAsync(pathToCompleted + complaint.ComplaintID);
                        await Application.Current.MainPage.DisplayAlert("Success", "You complete the job ID: " + complaint.ComplaintID + " successfully", "OK");
                    }
                }
            }catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
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
