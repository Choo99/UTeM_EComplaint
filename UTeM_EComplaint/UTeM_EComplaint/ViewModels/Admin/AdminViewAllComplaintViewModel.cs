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
    internal class AdminViewAllComplaintViewModel : ViewModelBase
    {
        bool isLoading;
        bool isRefresh;
        string pathToDetail = $"{nameof(AdminComplaintDetailPage)}?complaintID=";
        string pathToAdminSearch = $"{nameof(StaffComplaintDetailPage)}?complaintID=";
        string searchText;
        string resultText;

        public string SearchText { get => searchText; set => SetProperty(ref searchText, value); }
        public string ResultText { get => resultText; set => SetProperty(ref resultText, value); }
        public ObservableRangeCollection<string> SelectionList { get; set; }

        readonly int LOAD_SIZE = 5;

        List<Complaint> complaints;

        int resultCount;
        public int ResultCount { get => resultCount; set => SetProperty(ref resultCount, value); }

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
        public AsyncCommand SearchCommand { get; }
        public AsyncCommand<object> ItemSelectedCommand { get; }

        public AdminViewAllComplaintViewModel()
        {
            Title = "All Complaints";

            ComplaintList = new ObservableRangeCollection<Complaint>();

            RefreshCommand = new AsyncCommand(Refresh);
            LoadMoreCommand = new AsyncCommand(LoadMore);
            SearchCommand = new AsyncCommand(Search);
            ItemSelectedCommand = new AsyncCommand<object>(ItemSelected);

            getData();
        }

        private async Task Search()
        {
            try
            {
                int size = LOAD_SIZE;
                IsLoading = true;
                await Task.Delay(1000);
                complaints = await ComplaintServices.SearchAllComplaints(SearchText);
                ResultCount = complaints.Count;

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
                IsLoading = false;
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



        private async Task Refresh()
        {
            isRefresh = true;
            IsBusy = true;
            await Task.Delay(100);
            getData();
            IsBusy = false;
            isRefresh = false;
        }

        private async Task LoadMore()
        {
            await Task.Delay(100);
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
                ResultCount = complaints.Count;

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
