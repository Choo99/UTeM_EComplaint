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
    internal class JobDetailViewModel : ViewModelBase, IQueryAttributable
    {
        int complaintID;
        bool isNotAssigned;
        bool isAssigned;
        bool isInProgress;
        bool isCompleted;
        bool isRating;
        bool isNotRating;

        Complaint complaint;

        string pathToEditJob = $"{nameof(JobEditPage)}?complaintID=";
        string pathToDetail = $"../{nameof(JobDetailPage)}?complaintID=";

        public Complaint Complaint { get => complaint; set => SetProperty(ref complaint, value); }
        public bool IsAssigned { get => isAssigned; set { SetProperty(ref isAssigned, value); IsNotAssigned = !value; } }
        public bool IsNotAssigned { get => isNotAssigned; set => SetProperty(ref isNotAssigned, value); }
        public bool IsInProgress { get => isInProgress; set => SetProperty(ref isInProgress, value); }
        public bool IsCompleted { get => isCompleted; set => SetProperty(ref isCompleted, value); }
        public bool IsNotRating { get => isNotRating; set => SetProperty(ref isNotRating, value); }
        public bool IsRating { get => isRating; set { SetProperty(ref isRating, value); IsNotRating = !value; } }

        public AsyncCommand DoneCommand { get; }
        public AsyncCommand BackCommand { get; }
        public AsyncCommand EditCommand { get; }
        public AsyncCommand StartCommand { get; }
        public AsyncCommand FinishCommand { get; }

        public JobDetailViewModel()
        {
            Title = "Task Information";
            DoneCommand = new AsyncCommand(Done);
            BackCommand = new AsyncCommand(Back);
            EditCommand = new AsyncCommand(Edit);
            StartCommand = new AsyncCommand(Start);
            FinishCommand = new AsyncCommand(Finish);
        }

        private async Task Edit()
        {
            await Shell.Current.GoToAsync(pathToEditJob + complaintID);
        }

        private async Task Start()
        {
            try
            {
                int result = 0;
                var answer = await Application.Current.MainPage.DisplayAlert("Start", "Are you sure you want to start the job?", "YES", "NO");
                if (answer)
                {
                    try
                    {
                        result = await ActionServices.StartActionAndSendMessage(complaint.ComplaintID);
                    }
                    catch (Exception ex)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Opps! Something wrong with database[" + ex.Message + "]", "OK", "NO");
                    }

                    if (result != 0)
                    {
                        string path = pathToDetail + complaint.ComplaintID;
                        await Shell.Current.GoToAsync(path);
                        await Application.Current.MainPage.DisplayAlert("Success", "Started the job successfully", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message + ex.ToString(), "OK");
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            complaintID = int.Parse(HttpUtility.UrlDecode(query["complaintID"]));
            getComplaintDetail();
        }

        private async Task Finish()
        {
            try
            {
                var answer = await Application.Current.MainPage.DisplayAlert("Finish", "Are you sure you want to finish the job?", "Yes", "No");
                if (!await validate())
                    return;
                if (answer)
                {
                    int result = 0;
                    /*ActionTaken.ActionID = complaint.Action.ActionID;
                    Complaint newComplaint = new Complaint
                    {
                        ComplaintID = complaintID,
                        Action = actionTaken,
                    };*/

                    try
                    {
                        result = await ActionServices.EndActionAndSendNotification(Complaint);
                    }
                    catch (Exception ex)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
                    }
                    if (result == 1)
                    {
                        await Shell.Current.GoToAsync(pathToDetail + complaint.ComplaintID);
                        await Application.Current.MainPage.DisplayAlert("Success", "You complete the job ID: " + complaint.ComplaintID + " successfully", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
        }

        private async Task Back()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        private async Task Done()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        private async void getComplaintDetail()
        {
            try
            {
                IsBusy = true;
                Complaint = await ComplaintServices.GetComplaintDetail(complaintID);
                if(Complaint.ComplaintStatus == "Assigned")
                {
                    IsAssigned = true;
                    IsInProgress = false;
                    IsCompleted = false;
                }
                else if(Complaint.ComplaintStatus == "In Progress")
                {
                    IsAssigned = false;
                    IsInProgress = true;
                    IsCompleted = false;
                }
                else if(Complaint.ComplaintStatus == "Completed")
                {
                    IsAssigned = false;
                    IsCompleted = true;
                    IsInProgress = false;
                    if (complaint.Rating != null)
                    {
                        IsRating = true;
                    }
                    else
                    {
                        IsRating=false;
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

        async Task<bool> validate()
        {
            if(Complaint.Action.ActionDescription == null || Complaint.Action.SpareReplace == null || Complaint.Action.AdditionalNote == null)
            {
                if (Complaint.Action.ActionDescription == null)
                    await Application.Current.MainPage.DisplayAlert("Fill", "Please find in the action taken field","OK");
                else if(Complaint.Action.SpareReplace == null)
                    await Application.Current.MainPage.DisplayAlert("Fill", "Please find in the spare replace field", "OK");
                else if (Complaint.Action.AdditionalNote == null)
                    await Application.Current.MainPage.DisplayAlert("Fill", "Please find in the additional note field", "OK");
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
