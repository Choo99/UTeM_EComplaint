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
        readonly int LOAD_SIZE = 5;

        string pathToJobDetail = $"{nameof(JobDetailPage)}?complaintID=";

        int userID;

        Complaint selectedComplaint;
        List<Complaint> complaints;

        public ObservableRangeCollection<Complaint> ComplaintList { get; set; }

        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand<object> ItemSelectedCommand { get; }
        public AsyncCommand LoadMoreCommand { get; }

        
        public JobHistoryViewModel()
        {
            Title = "Job History";

            RefreshCommand = new AsyncCommand(Refresh);
            ItemSelectedCommand = new AsyncCommand<object>(ItemSelected);
            LoadMoreCommand = new AsyncCommand(LoadMore);

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
            try
            {
                int size = LOAD_SIZE;
                IsBusy = true;
                
                complaints = await ComplaintServices.GetComplaintsByStatus(userID, "Completed");

                if (complaints.Count < LOAD_SIZE)
                    size = complaints.Count;

                ComplaintList.ReplaceRange(complaints.GetRange(0, size));
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
        async Task Refresh()
        {
            IsBusy = true;
            getData();
        }

        public Complaint SelectedComplaint
        {
            get => selectedComplaint;
            set => SetProperty(ref selectedComplaint, value);
        }
    }
}
