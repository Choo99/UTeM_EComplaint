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
using Command = MvvmHelpers.Commands.Command;

namespace UTeM_EComplaint.ViewModels
{
    internal class StaffSearchComplaintViewModel : ViewModelBase
    {
        readonly string pathToJobDetail = $"{nameof(StaffComplaintDetailPage)}?complaintID=";
        readonly int userID;
        readonly int LOAD_SIZE = 5;

        string searchText;
        string resultText;
        string defaultSearchText;
        string noFoundSearchText;
        string placeHolderText;

        public String PlaceHolderText { get => placeHolderText; set => SetProperty(ref placeHolderText, value); }
        public String SearchText { get => searchText; set => SetProperty(ref searchText, value); }
        public String ResultText { get => resultText; set => SetProperty(ref resultText, value); }

        List<Complaint> complaints;
        public ObservableRangeCollection<Complaint> ComplaintList { get; }

        Complaint selectedComplaint;
        public Complaint SelectedComplaint { get => selectedComplaint; set => SetProperty(ref selectedComplaint, value); }

        public AsyncCommand SearchCommand { get; }
        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand LoadMoreCommand { get; }
        public Command ClearCommand { get; }
        public AsyncCommand<object> ItemSelectedCommand { get; }
        public StaffSearchComplaintViewModel()
        {
            Title = "Job Searching";
            userID = Preferences.Get("userID", 0);

            PlaceHolderText = "Search By ComplaintID and Complaint Date(YYYY-MM-DD)";
            defaultSearchText = "You can search by Complaint ID and Complaint Date(YYYY-MM-DD)";

            ResultText = defaultSearchText;

            RefreshCommand = new AsyncCommand(Refresh);
            SearchCommand = new AsyncCommand(Search);
            LoadMoreCommand = new AsyncCommand(LoadMore);
            ClearCommand = new Command(Clear);
            ItemSelectedCommand = new AsyncCommand<object>(ItemSelected);

            complaints = new List<Complaint>();
            ComplaintList = new ObservableRangeCollection<Complaint>();
        }

        private void Clear(object obj)
        {
            IsBusy = true;
            ComplaintList.Clear();
            Task.Delay(1000);
            IsBusy = false;
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

        private async Task Search()
        {
            try
            {
                int size = LOAD_SIZE;
                IsBusy = true;
                await Task.Delay(1000);
                complaints = await ComplaintServices.SearchComplaints(userID, SearchText);

                if (complaints.Count == 0)
                {
                    ResultText = "Opps! No result complaint found for key word\"" + searchText + "\""; ;
                }
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

        private async Task Refresh()
        {
            IsBusy = true;
            ComplaintList.Clear();
            ResultText = searchText;
            IsBusy = false;
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
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message + ex.ToString(), "NO");
            }
        }
    }
}
