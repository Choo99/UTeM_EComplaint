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
    internal class StaffComplaintPendingViewModel : ViewModelBase
    {
        readonly int LOAD_SIZE = 5;

        int staffID;
        string pathToDetail = $"{nameof(StaffComplaintDetailPage)}?complaintID=";
        string pathToEditComplaint = $"{nameof(StaffEditComplaintPage)}?complaintID=";

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
        public AsyncCommand LoadMoreCommand { get; }
        public AsyncCommand<object> EditCommand { get; }
        public AsyncCommand<object> ItemSelectedCommand { get; }


        public StaffComplaintPendingViewModel()
        {
            Title = "Pending Complaint";
            staffID = Preferences.Get("userID", 0);

            ComplaintList = new ObservableRangeCollection<Complaint>();
            complaints = new List<Complaint>();

            ItemSelectedCommand = new AsyncCommand<object>(ItemSelected);
            RefreshCommand = new AsyncCommand(Refresh);
            LoadMoreCommand = new AsyncCommand(LoadMore);
            EditCommand = new AsyncCommand<object>(Edit);
            getData();
        }

        private async Task Edit(object obj)
        {
            var complaint = obj as Complaint;
            await Shell.Current.GoToAsync(pathToEditComplaint + complaint.ComplaintID);
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
                complaints = await ComplaintServices.GetStaffComplaintByStatus(staffID,"Pending");
                if(complaints.Count < LOAD_SIZE)
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
        private async Task ItemSelected(object arg)
        {
            var complaint = arg as Complaint;
            if (complaint == null)
                return;
            SelectedComplaint = null;
            await Shell.Current.GoToAsync(pathToDetail+complaint.ComplaintID);
        }
        async Task Refresh()
        {
            IsBusy = true;
            getData();
        }
    }
}
