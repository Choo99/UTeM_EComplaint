using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UTeM_EComplaint.Model;
using UTeM_EComplaint.Services;
using UTeM_EComplaint.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace UTeM_EComplaint.ViewModels
{
    internal class AboutViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Complaint> ComplaintList { get; set; }
        int totalTask;
        int completeTask;
        int inProgressTask;
        int pendingTask;
        int technicianID;

        string dateTime;
        string pathToToDo = $"{nameof(JobTodoPage)}";
        string pathToInProgress = $"{nameof(JobProgressPage)}";
        string pathToCompleted = $"{nameof(JobHistoryPage)}";

        string pathToToDoDetail = $"{nameof(JobToDoDetailPage)}?complaintID=";
        string pathToInProgressDetail = $"{nameof(JobInProgressDetailPage)}?complaintID=";
        string pathToCompletedDetail = $"{nameof(JobCompletedDetailPage)}?complaintID=";

        public Complaint SelectedComplaint
        {
            get => selectedComplaint;
            set
            {
                SetProperty(ref selectedComplaint, value);
                OnPropertyChanged();
            }
        }

        public int TotalTask
        {
            get => totalTask;
            set => SetProperty(ref totalTask, value);
        }
        public int CompleteTask
        {
            get => completeTask;
            set => SetProperty(ref completeTask, value);
        }
        public int InProgressTask
        {
            get => inProgressTask;
            set => SetProperty(ref inProgressTask, value);
        }
        public int PendingTask
        {
            get => pendingTask;
            set => SetProperty(ref pendingTask, value);
        }
        public string DateTimeString
        {
            get => dateTime;
            set => SetProperty(ref dateTime, value);
        }

        Complaint selectedComplaint;
        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand TotalTabbedCommand { get; }
        public AsyncCommand PendingTabbedCommand { get; }
        public AsyncCommand InProgressTabbedCommand { get; }
        public AsyncCommand CompletedTabbedCommand { get; }
        public AsyncCommand LogoutCommand { get; }
        public AsyncCommand<object> ItemSelectedCommand { get; }
         

        public AboutViewModel()
        {
            DateTimeString = DateTime.Now.ToString("dd/MM/yyyy");
            ComplaintList = new ObservableRangeCollection<Complaint>();
            technicianID = Preferences.Get("userID", 0);
            GetStatistic();
            GetComplaintList();
            TotalTabbedCommand = new AsyncCommand(TotalTabbed);
            PendingTabbedCommand = new AsyncCommand(PendingTabbed);
            InProgressTabbedCommand = new AsyncCommand(InProgressTabbed);
            CompletedTabbedCommand = new AsyncCommand(CompletedTabbed);
            RefreshCommand = new AsyncCommand(Refresh);
            LogoutCommand = new AsyncCommand(Logout);
            ItemSelectedCommand = new AsyncCommand<object>(ItemSelected);
            
        }

        private async Task ItemSelected(object arg)
        {
            var complaint = arg as Complaint;
            if (complaint == null)
                return;
            SelectedComplaint = null;
            if(complaint.ComplaintStatus == "Assigned")
            {
                await Shell.Current.GoToAsync(pathToToDoDetail + complaint.ComplaintID);
            }
            else if (complaint.ComplaintStatus == "In Progress")
            {
                await Shell.Current.GoToAsync(pathToInProgressDetail + complaint.ComplaintID);
            }
            else if((complaint.ComplaintStatus == "Completed"))
            {
                await Shell.Current.GoToAsync(pathToCompletedDetail + complaint.ComplaintID);
            }

        }

        async Task TotalTabbed()
        {

            string text = "Total tasks: " + totalTask +"\n";
            text += "To Do tasks: " + pendingTask + "\n";
            text += "In progress tasks: " + inProgressTask + "\n";
            await Application.Current.MainPage.DisplayAlert("Jobs", text, "OK");
        }        
        async Task Logout()
        {

            var isLogout = await Application.Current.MainPage.DisplayAlert("logout", "Are you sure you want to logout", "Yes", "No");
            if (isLogout)
            {
                Preferences.Remove("userID");
                Preferences.Remove("role");
                await Shell.Current.GoToAsync("//LoginPages");
            }
        }
        async Task PendingTabbed()
        {
            await Shell.Current.GoToAsync(pathToToDo);
        }
        async Task InProgressTabbed()
        {
            await Shell.Current.GoToAsync(pathToInProgress);
        }
        async Task CompletedTabbed()
        {
            await Shell.Current.GoToAsync(pathToCompleted);
        }

        async Task Refresh()
        {
            IsBusy = true;
            pendingTask = 0;
            inProgressTask = 0;
            completeTask = 0;
            totalTask = 0;
            GetStatistic();
            GetComplaintList();
            IsBusy = false;
        }

        private async void GetComplaintList()
        {
            try
            {
                List<Complaint> complaints = await ComplaintServices.GetTechnicianComplaint(technicianID);
                ComplaintList.Clear();
                ComplaintList.AddRange(complaints);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message + ex.StackTrace, "OK");
            }
        }
        private async void GetStatistic()
        {
            try
            {
                List<KeyValuePair<string, int>> list = await ComplaintServices.GetComplaintStatistic(technicianID);

                foreach (KeyValuePair<string, int> item in list)
                {
                    if (item.Key == "Assigned")
                    {
                        PendingTask = item.Value;
                    }
                    else if (item.Key == "In Progress")
                    {
                        InProgressTask = item.Value;
                    }
                    else if (item.Key == "Completed")
                    {
                        CompleteTask = item.Value;
                    }
                    else if (item.Key == "Total Task")
                    {
                        TotalTask = item.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
        }
    }
}

