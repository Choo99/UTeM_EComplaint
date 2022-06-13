using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MvvmHelpers;
using MvvmHelpers.Commands;
using UTeM_EComplaint.Model;
using UTeM_EComplaint.Services;
using UTeM_EComplaint.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Command = MvvmHelpers.Commands.Command;

namespace UTeM_EComplaint.ViewModels
{
    internal class StaffComplaintInProgressViewModel : ViewModelBase
    {
        readonly int LOAD_SIZE = 5;

        int staffID;
        string pathToDetail = $"{nameof(StaffComplaintDetailPage)}?complaintID=";

        Complaint selectedComplaint;
        List<Complaint> complaints;
        public ObservableRangeCollection<Complaint> ComplaintList { get; }
        public Complaint SelectedComplaint
        {
            get => selectedComplaint;
            set
            {
                SetProperty(ref selectedComplaint, value);
                OnPropertyChanged();
            }
        }

        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand<object> ItemSelectedCommand { get; }
        public AsyncCommand LoadMoreCommand { get; }

        public StaffComplaintInProgressViewModel()
        {
            Title = "In Progress Complaint";
            staffID = Preferences.Get("userID", 0);

            ComplaintList = new ObservableRangeCollection<Complaint>();
            complaints = new List<Complaint>();

            ItemSelectedCommand = new AsyncCommand<object>(ItemSelected);
            RefreshCommand = new AsyncCommand(Refresh);
            LoadMoreCommand = new AsyncCommand(LoadMore);
            getData();
        }

        private async Task LoadMore()
        {
            if (ComplaintList.Count == complaints.Count)
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

        async void getData()
        {
            try
            {
                int size = LOAD_SIZE;
                IsBusy = true;
                complaints = await ComplaintServices.GetComplaintsByStatus(staffID, "In Progress");
                if (complaints.Count < LOAD_SIZE)
                {
                    size = complaints.Count;
                }
                ComplaintList.ReplaceRange(complaints.GetRange(0, size));
                IsBusy = false;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
            
        }
        private async Task ItemSelected(object arg)
        {
            var complaint = arg as Complaint;
            if (complaint == null)
                return;
            SelectedComplaint = null;
            await Shell.Current.GoToAsync(pathToDetail + complaint.ComplaintID);
        }
        async Task Refresh()
        {
            await Task.Delay(100);
            IsBusy = true;
            getData();
            IsBusy = false;
        }
    }
}
