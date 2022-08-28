using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UTeM_EComplaint.Model;
using UTeM_EComplaint.Services;
using UTeM_EComplaint.Tools;
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

        ComplaintDetail selectedComplaintDetail;
        List<ComplaintDetail> complaintDetails;

        public ObservableRangeCollection<ComplaintDetail> ComplaintDetailList { get; set; }

        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand<object> ItemSelectedCommand { get; }
        public AsyncCommand LoadMoreCommand { get; }

        
        public JobHistoryViewModel()
        {
            Title = "Job History";

            RefreshCommand = new AsyncCommand(Refresh);
            ItemSelectedCommand = new AsyncCommand<object>(ItemSelected);
            LoadMoreCommand = new AsyncCommand(LoadMore);

            ComplaintDetailList = new ObservableRangeCollection<ComplaintDetail>();
            complaintDetails = new List<ComplaintDetail>();

            userID = Preferences.Get("userID", 0);
            getData();
        }

        private async Task LoadMore()
        {
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

                complaintDetails = await ComplaintDetailServices.GetComplaintDetailByStatus(new ComplaintDetail
                {
                    Technician = new Technician 
                    { 
                        TechnicianID = Preferences.Get("userID",0),
                    },
                    ComplaintDetailStatus = "Completed"
                });

                DurationHandler.durationList(ref complaintDetails);

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
