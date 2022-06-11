using System;
using System.Collections.Generic;
using MvvmHelpers.Commands;
using System.Threading.Tasks;
using MvvmHelpers;
using UTeM_EComplaint.Model;
using UTeM_EComplaint.Services;
using UTeM_EComplaint.Tools;
using UTeM_EComplaint.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Command = MvvmHelpers.Commands.Command;

namespace UTeM_EComplaint.ViewModels
{
    internal class AdditionalComplaint
    {
        public Complaint Complaint { get; set; }
        public Color BackgroundColor { get; set; }
        public Color TextColor { get; set; }
    }

    internal class StaffViewAllComplaintViewModel : ViewModelBase
    {
        int staffID;
        bool isLoading;
        bool isRefresh;
        string pathToDetail = $"{nameof(StaffComplaintDetailPage)}?complaintID=";
        readonly int LOAD_SIZE = 5;

        List<Complaint> complaints;

        Complaint selectedComplaint;
        public Complaint SelectedComplaint
        {
            get => selectedComplaint;
            set
            {
                SetProperty(ref selectedComplaint, value);
                OnPropertyChanged();
            }
        }
        public ObservableRangeCollection<Complaint> ComplaintList { get; }
        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }

        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand LoadMoreCommand { get; }
        public AsyncCommand<object> ItemSelectedCommand { get; }

        public StaffViewAllComplaintViewModel()
        {
            Title = "All Complaints";
            staffID = Preferences.Get("userID", 0);

            ComplaintList = new ObservableRangeCollection<Complaint>();

            RefreshCommand = new AsyncCommand(Refresh);
            LoadMoreCommand = new AsyncCommand(LoadMore);
            ItemSelectedCommand = new AsyncCommand<object>(ItemSelected);

            getData();
        }
        private async Task ItemSelected(object arg)
        {
            var complaint = arg as Complaint;
            if (complaint == null)
                return;
            SelectedComplaint = null;
            await Shell.Current.GoToAsync(pathToDetail + complaint.ComplaintID);
        }

        private async Task Refresh()
        {
            isRefresh = true;
            IsBusy = true;
            getData();
            IsBusy = false;
            isRefresh = false;
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
                if (!isRefresh)
                    IsLoading = true;
                complaints = await ComplaintServices.GetAllComplaints();

                ComplaintList.ReplaceRange(complaints.GetRange(0, LOAD_SIZE));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
            finally
            {
                if (!isRefresh)
                    IsLoading = false;
            }
        }

    }
}
