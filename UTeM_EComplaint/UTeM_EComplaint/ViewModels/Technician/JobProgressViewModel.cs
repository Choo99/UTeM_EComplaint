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
    internal class JobProgressViewModel : ViewModelBase
    {
        readonly int LOAD_SIZE = 5;

        int userID;

        List<ComplaintDetail> complaintDetails;

        ComplaintDetail selectedComplaintDetail;
        public ObservableRangeCollection<ComplaintDetail> ComplaintDetailList { get; set; }

        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand LoadMoreCommand { get; }
        public AsyncCommand<object> ItemSelectedCommand { get; }

        int resultCount;
        public int ResultCount
        {
            get => resultCount;
            set
            {
                SetProperty(ref resultCount, value);
            }
        }

        string pathToJobDetail = $"{nameof(JobDetailPage)}?complaintID=";
        public JobProgressViewModel()
        {
            Title = "In Progress task";

            RefreshCommand = new AsyncCommand(Refresh);
            LoadMoreCommand = new AsyncCommand(LoadMore);
            ItemSelectedCommand = new AsyncCommand<object>(ItemSelected);

            ComplaintDetailList = new ObservableRangeCollection<ComplaintDetail>();
            complaintDetails = new List<ComplaintDetail>();

            userID = Preferences.Get("userID", 0);
            getData();
        }

        private async Task LoadMore()
        {
            if (complaintDetails.Count == ComplaintDetailList.Count)
                return;
            int lastItemIndexed = complaintDetails.Count;
            int nextItemIndexed = lastItemIndexed + LOAD_SIZE;

            if (nextItemIndexed > complaintDetails.Count)
                nextItemIndexed = complaintDetails.Count;

            for (int i = lastItemIndexed; i < nextItemIndexed; i++)
            {
                ComplaintDetailList.Add(complaintDetails[i]);
            }
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
                await Application.Current.MainPage.DisplayAlert("Start", ex.Message, "NO");
            }

        }

        private async void getData()
        {
            try
            {
                int size = LOAD_SIZE;
                IsBusy = true;

                ComplaintDetail tempComplaintDetail = new ComplaintDetail
                {
                    ComplaintDetailStatus = "In Progress",
                    Technician = new Technician
                    {
                        TechnicianID = Preferences.Get("userID", 0),
                    }
                };
                List<ComplaintDetail> complaintDetailList = await ComplaintDetailServices.GetComplaintDetailByStatus(tempComplaintDetail);
                ResultCount = complaintDetailList.Count;
                if (complaintDetailList.Count < LOAD_SIZE)
                    size = complaintDetailList.Count;

                ComplaintDetailList.ReplaceRange(complaintDetailList.GetRange(0, size));
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

        public ComplaintDetail SelectedComplaintDetail
        {
            get => selectedComplaintDetail;
            set => SetProperty(ref selectedComplaintDetail, value);
        }
    }
}

