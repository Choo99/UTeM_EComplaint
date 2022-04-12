using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using MvvmHelpers;
using UTeM_EComplaint.Model;
using UTeM_EComplaint.Services;
using MvvmHelpers.Commands;
using System.Threading.Tasks;
using UTeM_EComplaint.Views;

namespace UTeM_EComplaint.ViewModels
{
    internal class JobToDoViewModel : ViewModelBase
    {
        int userID;
        Complaint selectedComplaint;
        public ObservableRangeCollection<Complaint> ComplaintList { get; set; }
        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand<object> ItemSelectedCommand { get; }
        public AsyncCommand<object> StartJobCommand { get; }

        string pathToJobDetail = $"{nameof(JobToDoDetailPage)}?complaintID=";
        public JobToDoViewModel()
        {
            ComplaintList = new ObservableRangeCollection<Complaint>();
            userID = Preferences.Get("userID", 0);
            getData();

            RefreshCommand = new AsyncCommand(Refresh);
            ItemSelectedCommand = new AsyncCommand<object>(ItemSelected);
            StartJobCommand = new AsyncCommand<object>(StartJob);

            Title = "To Do";
            
        }

        private async Task StartJob(object arg)
        {
            try
            {
                int result = 0;
                var complaint = arg as Complaint;
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
                         string path = pathToJobDetail + complaint.ComplaintID;
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

        private async Task ItemSelected(object arg)
        {
            try
            {
                var complaint = arg as Complaint;
                if (complaint == null)
                    return;
                SelectedComplaint = null;

                await Shell.Current.GoToAsync(pathToJobDetail + complaint.ComplaintID);
            }catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Start", ex.Message + ex.ToString(), "NO");
            }
            
         }

        private async void getData()
        {
            List<Complaint> complaints = await ComplaintServices.GetComplaintsByStatus(userID, "Assigned");
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
