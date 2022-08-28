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
    internal class JobSearchingViewModel : ViewModelBase
    {
        readonly string pathToJobDetail = $"{nameof(JobDetailPage)}?complaintID=";
        readonly int userID;
        readonly int LOAD_SIZE = 5;

        string searchText;
        string resultText;

        public String SearchText { get => searchText; set => SetProperty(ref searchText,value); }
        public String ResultText { get => resultText; set => SetProperty(ref resultText, value); }

        List<ComplaintDetail> complaintDetails;
        public ObservableRangeCollection<ComplaintDetail> ComplaintDetailList { get; }

        ComplaintDetail selectedComplaintDetail;
        public ComplaintDetail SelectedComplaintDetail { get => selectedComplaintDetail; set => SetProperty(ref selectedComplaintDetail, value); }

        public AsyncCommand SearchCommand { get; }
        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand LoadMoreCommand { get; }
        public Command ClearCommand { get; }
        public AsyncCommand<object> ItemSelectedCommand { get; }
        public JobSearchingViewModel()
        {
            Title = "Job Searching";
            userID = Preferences.Get("userID", 0);

            ResultText = "Search by Complaint ID and Complaint Date(YYYY-MM-DD)";

            RefreshCommand = new AsyncCommand(Refresh);
            SearchCommand = new AsyncCommand(Search);
            LoadMoreCommand = new AsyncCommand(LoadMore);
            ClearCommand = new Command(Clear);
            ItemSelectedCommand = new AsyncCommand<object>(ItemSelected);

            complaintDetails = new List<ComplaintDetail>();
            ComplaintDetailList = new ObservableRangeCollection<ComplaintDetail>();

        }

        private void Clear(object obj)
        {
            IsBusy = true;
            ComplaintDetailList.Clear();
            Task.Delay(1000);
            IsBusy = false;
        }

        private async Task LoadMore()
        {
            await Task.Delay(100);
            if (complaintDetails.Count == ComplaintDetailList.Count)
                return;
            int lastItemIndexed = ComplaintDetailList.Count;
            int nextItemIndexed = lastItemIndexed + LOAD_SIZE;

            if (nextItemIndexed > complaintDetails.Count)
                nextItemIndexed = complaintDetails.Count;

            for (int i = lastItemIndexed; i < nextItemIndexed; i++)
            {
                ComplaintDetailList.Add(complaintDetails[i]);
            }
        }

        private async Task Search()
        {
            try
            {
                int size = LOAD_SIZE;
                IsBusy = true;
                await Task.Delay(1000);
                complaintDetails = await ComplaintDetailServices.TechnicianSearchComplaitDetail(new Technician
                {
                    TechnicianID = Preferences.Get("userID", 0)
                }, SearchText) ;

                if(complaintDetails.Count == 0)
                {
                    ResultText = "Opps! No job found for key word\"" + searchText + "\"";
                }
                if (complaintDetails.Count < LOAD_SIZE)
                    size = complaintDetails.Count;

                ComplaintDetailList.ReplaceRange(complaintDetails.GetRange(0, size));
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
            ComplaintDetailList.Clear();
            ResultText = "You can search by Location,Staff Name, Job Category, Damage Type or Division";
            IsBusy =false;
        }

        private async Task ItemSelected(object arg)
        {
            try
            {
                var complaintDetail = arg as ComplaintDetail;
                if (complaintDetail == null)
                    return;
                SelectedComplaintDetail = null;

                await Shell.Current.GoToAsync(pathToJobDetail + complaintDetail.Complaint.ComplaintID);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message + ex.ToString(), "NO");
            }
        }
    }
}
