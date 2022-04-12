using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UTeM_EComplaint.Model;
using UTeM_EComplaint.Services;
using UTeM_EComplaint.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace UTeM_EComplaint.ViewModels
{
    internal class JobHistoryViewModel : ViewModelBase
    {
        int userID;
        Complaint selectedComplaint;
        public ObservableRangeCollection<Complaint> ComplaintList { get; set; }
        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand<object> ItemSelectedCommand { get; }
        public AsyncCommand<object> StartJobCommand { get; }

        string pathToJobDetail = $"{nameof(JobCompletedDetailPage)}?complaintID=";
        public JobHistoryViewModel()
        {
            ComplaintList = new ObservableRangeCollection<Complaint>();
            userID = Preferences.Get("userID", 0);
            getData();

            RefreshCommand = new AsyncCommand(Refresh);
            ItemSelectedCommand = new AsyncCommand<object>(ItemSelected);
            StartJobCommand = new AsyncCommand<object>(StartJob);

            Title = "Job History";

        }

        private async Task StartJob(object arg)
        {
            try
            {
                var complaint = arg as Complaint;
                var answer = await Application.Current.MainPage.DisplayAlert("Start", "Are you sure you want to start the job?", "YES", "NO");
                if (answer)
                {
                    int result = await ActionServices.StartActionAndSendMessage(complaint.ComplaintID);
                    if (result != 0)
                    {
                        string path = pathToJobDetail + complaint.ComplaintID;
                        await Shell.Current.GoToAsync(path);
                        await Application.Current.MainPage.DisplayAlert("Success", "Started the job successfully", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Success", ex.Message, "OK", "NO");
            }
        }

        private async Task ItemSelected(object arg)
        {
            try
            {
                var complaint = arg as Complaint;
                if (complaint == null)
                    return;
                SelectedComplaint = null;

                await Shell.Current.GoToAsync(pathToJobDetail + complaint.ComplaintID);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Start", ex.Message, "NO");
            }

        }

        private async void getData()
        {
            List<Complaint> complaints = await ComplaintServices.GetComplaintsByStatus(userID, "Completed");
            ComplaintList.ReplaceRange(complaints);
        }
        async Task Refresh()
        {
            IsBusy = true;
            getData();
            await Task.Delay(1000);
            IsBusy = false;
        }

        public Complaint SelectedComplaint
        {
            get => selectedComplaint;
            set => SetProperty(ref selectedComplaint, value);
        }
    }
}
