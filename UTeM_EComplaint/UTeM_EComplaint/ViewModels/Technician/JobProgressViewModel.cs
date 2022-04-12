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
        int userID;
        Complaint selectedComplaint;
        public ObservableRangeCollection<Complaint> ComplaintList { get; set; }
        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand<object> ItemSelectedCommand { get; }
        public AsyncCommand<object> StartJobCommand { get; }

        string pathToJobDetail = $"{nameof(JobInProgressDetailPage)}?complaintID=";
        public JobProgressViewModel()
        {
            ComplaintList = new ObservableRangeCollection<Complaint>();
            userID = Preferences.Get("userID", 0);
            getData();

            RefreshCommand = new AsyncCommand(Refresh);
            ItemSelectedCommand = new AsyncCommand<object>(ItemSelected);
            Title = "In Progress task";
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
                await Application.Current.MainPage.DisplayAlert("Start", ex.Message, "NO");
            }

        }

        private async void getData()
        {
            try
            {
                List<Complaint> complaints = await ComplaintServices.GetComplaintsByStatus(userID, "In Progress");
                ComplaintList.ReplaceRange(complaints);
            }catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
        }

        async Task Refresh()
        {
            IsBusy = true;
            getData();
            await Task.Delay(1000);
            IsBusy = false;
        }

        public Complaint SelectedComplaint
        {
            get => selectedComplaint;
            set => SetProperty(ref selectedComplaint, value);
        }
    }
}

