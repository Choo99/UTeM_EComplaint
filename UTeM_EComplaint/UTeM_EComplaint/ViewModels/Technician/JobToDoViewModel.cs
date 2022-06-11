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
        readonly int LOAD_SIZE = 5;

        int userID;

        string pathToJobDetail = $"{nameof(JobDetailPage)}?complaintID=";

        Complaint selectedComplaint;

        List<Complaint> complaints;

        public Complaint SelectedComplaint
        {
            get => selectedComplaint;
            set
            {
                SetProperty(ref selectedComplaint, value);
                OnPropertyChanged();
            }
        }

        public ObservableRangeCollection<Complaint> ComplaintList { get; set; }
        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand LoadMoreCommand { get; }
        public AsyncCommand<object> ItemSelectedCommand { get; }
        public AsyncCommand<object> StartJobCommand { get; }

        public JobToDoViewModel()
        {
            Title = "To Do";

            RefreshCommand = new AsyncCommand(Refresh);
            LoadMoreCommand = new AsyncCommand(LoadMore);
            ItemSelectedCommand = new AsyncCommand<object>(ItemSelected);
            StartJobCommand = new AsyncCommand<object>(StartJob);


            ComplaintList = new ObservableRangeCollection<Complaint>();
            complaints = new List<Complaint>();

            userID = Preferences.Get("userID", 0);
            getData();
        }

        private async Task LoadMore()
        {
            if (complaints.Count == ComplaintList.Count)
                return;
            int lastItemIndexed = ComplaintList.Count;
            int nextItemIndexed = lastItemIndexed + LOAD_SIZE;

            if (nextItemIndexed > complaints.Count)
                nextItemIndexed = complaints.Count;

            for (int i = lastItemIndexed; i < nextItemIndexed; i++)
            {
                ComplaintList.Add(complaints[i]);
            }
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
            try
            {
                int size = LOAD_SIZE;
                IsBusy = true;

                complaints = await ComplaintServices.GetComplaintsByStatus(userID, "Assigned");

                if (complaints.Count < LOAD_SIZE)
                    size = complaints.Count;
                
                ComplaintList.ReplaceRange(complaints.GetRange(0, size));
            }
            catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
        async Task Refresh()
        {
            IsBusy = true;
            getData();
        }
    }
}
