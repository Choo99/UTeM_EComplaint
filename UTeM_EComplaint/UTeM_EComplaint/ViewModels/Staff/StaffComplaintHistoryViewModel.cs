using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MvvmHelpers;
using MvvmHelpers.Commands;
using UTeM_EComplaint.Model;
using UTeM_EComplaint.Services;
using UTeM_EComplaint.Tools;
using UTeM_EComplaint.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Command = MvvmHelpers.Commands.Command;

namespace UTeM_EComplaint.ViewModels
{
    internal class StaffComplaintHistoryViewModel : ViewModelBase
    {
        readonly int LOAD_SIZE = 5;
        readonly int staffID;
        bool isRated;
        string pathToDetail = $"{nameof(StaffComplaintDetailPage)}?complaintID=";
        string pathToRate = $"{nameof(StaffAddRatingPage)}?complaintID=";

        public bool IsRated { get => isRated; set => SetProperty(ref isRated, value); }
        Complaint selectedComplaint;
        List<Complaint> complaints;
        public ObservableRangeCollection<Complaint> ComplaintHistory { get; }
        

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
        public AsyncCommand LoadMoreCommand { get; }
        public AsyncCommand<object> RateCommand { get; }

        public AsyncCommand<object> ItemSelectedCommand { get; }

        public StaffComplaintHistoryViewModel()
        {
            Title = "Complaint History";
            staffID = Preferences.Get("userID", 0);

            complaints = new List<Complaint>();

            ComplaintHistory = new ObservableRangeCollection<Complaint>();

            ItemSelectedCommand = new AsyncCommand<object>(ItemSelected);
            RefreshCommand = new AsyncCommand(Refresh);
            LoadMoreCommand = new AsyncCommand(LoadMore);
            RateCommand = new AsyncCommand<object>(Rate);
            getData();
        }

        private async Task LoadMore()
        {
            await Task.Delay(100);
            if (complaints.Count == ComplaintHistory.Count)
                return;
            int lastItemIndexed = ComplaintHistory.Count;
            int nextItemIndexed = lastItemIndexed + LOAD_SIZE;

            if (nextItemIndexed > complaints.Count)
                nextItemIndexed = complaints.Count;

            for (int i = lastItemIndexed; i < nextItemIndexed; i++)
            {
                ComplaintHistory.Add(complaints[i]);
            }
        }

        private async Task Rate(Object arg)
        {
            var complaint = arg as Complaint;
            await Shell.Current.GoToAsync(pathToRate + complaint.ComplaintID);
        }

        async void getData()
        {
            try
            {
                int size = LOAD_SIZE;
                IsBusy = true;
                complaints = await ComplaintServices.GetComplaintsByStatus(staffID, "Completed");
                if (complaints.Count < LOAD_SIZE)
                {
                    size = complaints.Count;
                }
                DurationHandler.durationList(ref complaints);
                ComplaintHistory.ReplaceRange(complaints.GetRange(0, size));
                
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
            IsBusy = true;
            getData();
        }
    }
}
