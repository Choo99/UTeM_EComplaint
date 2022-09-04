using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using MvvmHelpers;
using UTeM_EComplaint.Model;
using UTeM_EComplaint.Services;
using MvvmHelpers.Commands;
using System.Threading.Tasks;
using UTeM_EComplaint.Views;

namespace UTeM_EComplaint.ViewModels
{
    internal class JobToDoViewModel : ViewModelBase
    {
        readonly int LOAD_SIZE = 5;

        int userID;

        string pathToJobDetail = $"{nameof(JobDetailPage)}?complaintID=";

        ComplaintDetail selectedComplaintDetail;

        List<ComplaintDetail> complaintDetails;

        public ComplaintDetail SelectedComplaintDetail
        {
            get => selectedComplaintDetail;
            set
            {
                SetProperty(ref selectedComplaintDetail, value);
                OnPropertyChanged();
            }
        }

        int resultCount;
        public int ResultCount
        {
            get => resultCount;
            set
            {
                SetProperty(ref resultCount, value);
            }
        }

        public ObservableRangeCollection<ComplaintDetail> ComplaintDetailList { get; set; }
        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand LoadMoreCommand { get; }
        public AsyncCommand<object> ItemSelectedCommand { get; }

        public JobToDoViewModel()
        {
            Title = "To Do";

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
            }catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Start", ex.Message + ex.ToString(), "NO");
            }
         }

        private async void getData()
        {
            try
            {
                int size = LOAD_SIZE;
                IsBusy = true;

                ComplaintDetail complaintDetail = new ComplaintDetail
                {
                    ComplaintDetailStatus = "Assigned",
                    Technician = new Technician
                    {
                        TechnicianID = Preferences.Get("userID", 0),
                    },
                };
                complaintDetails = await ComplaintDetailServices.GetComplaintDetailByStatus(complaintDetail);
                ResultCount = complaintDetails.Count;
                if (complaintDetails.Count < LOAD_SIZE)
                    size = complaintDetails.Count;

                ComplaintDetailList.ReplaceRange(complaintDetails.GetRange(0, size));
            }
            catch(Exception ex)
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
    }
}
