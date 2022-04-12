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
        List<AdditionalComplaint> additionalComplaints;


        AdditionalComplaint selectedComplaint;
        public AdditionalComplaint SelectedComplaint
        {
            get => selectedComplaint;
            set
            {
                SetProperty(ref selectedComplaint, value);
                OnPropertyChanged();
            }
        }
        public ObservableRangeCollection<AdditionalComplaint> ComplaintList { get; }
        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }

        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand LoadMoreCommand { get; }
        public AsyncCommand<object> ItemSelectedCommand { get; }

        public StaffViewAllComplaintViewModel()
        {
            Title = "All Complaints";
            staffID = Preferences.Get("userID", 0);

            ComplaintList = new ObservableRangeCollection<AdditionalComplaint>();

            RefreshCommand = new AsyncCommand(Refresh);
            LoadMoreCommand = new AsyncCommand(LoadMore);
            ItemSelectedCommand = new AsyncCommand<object>(ItemSelected);

            getData();
        }
        private async Task ItemSelected(object arg)
        {
            var complaint = arg as AdditionalComplaint;
            if (complaint == null)
                return;
            SelectedComplaint = null;
            await Shell.Current.GoToAsync(pathToDetail + complaint.Complaint.ComplaintID);
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
            if (ComplaintList.Count == additionalComplaints.Count)
                return;

            int lastItemIndexed = ComplaintList.Count;
            int nextItemIndexed = lastItemIndexed + LOAD_SIZE;

            if (nextItemIndexed > additionalComplaints.Count)
                nextItemIndexed = additionalComplaints.Count;

            for (int i = lastItemIndexed; i < nextItemIndexed; i++)
            {
                ComplaintList.Add(additionalComplaints[i]);
            }
        }

        async void getData()
        {
            try
            {
                if (!isRefresh)
                    IsLoading = true;
                List<Complaint> complaints = await ComplaintServices.GetAllComplaints();
                additionalComplaints = new List<AdditionalComplaint>();
                for (int i = 0; i < complaints.Count; i++)
                {
                    Color backgroundColor = Color.White;

                    if (complaints[i].Staff.StaffID == staffID)
                    {
                        backgroundColor = Color.Gray;
                    }
                    AdditionalComplaint additionalComplaint = new AdditionalComplaint
                    {
                        Complaint = complaints[i],
                        BackgroundColor = backgroundColor,
                        TextColor = ColorConverter.StatusToColor(complaints[i].ComplaintStatus)
                    };
                    additionalComplaints.Add(additionalComplaint);
                }
                ComplaintList.ReplaceRange(additionalComplaints.GetRange(0, LOAD_SIZE));
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
