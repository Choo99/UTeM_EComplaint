using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmHelpers;
using MvvmHelpers.Commands;
using UTeM_EComplaint.Model;
using UTeM_EComplaint.Views;
using UTeM_EComplaint.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Command = MvvmHelpers.Commands.Command;

namespace UTeM_EComplaint.ViewModels
{
    internal class StaffHomeViewModel : ViewModelBase
    {
        string dateTimeString;
        string toggleText;
        string pieChartTitle;

        int totalComplaint;
        int pendingComplaint;
        int assignedComplaint;
        int inProgressComplaint;
        int completedComplaint;
        int staffID;

        bool isLoading;
        bool isRefresh;
        bool isPieChart;

        public ObservableRangeCollection<Statistic> Statistics { get; set; }

        string pathToPending = $"home/StaffComplaintPendingPage";
        string pathToAssigned = $"home/{nameof(StaffComplaintAssignedPage)}";
        string pathToInProgress = $"home/{nameof(StaffComplaintInProgressPage)}";
        string pathToCompleted = $"home/{nameof(StaffComplaintHistoryPage)}";

        public bool IsPieChart
        {
            get => isPieChart;
            set
            {
                SetProperty(ref isPieChart, value);
                if (value)
                    ToggleText = "Pie Chart";
                else
                    ToggleText = "Classic";
                Preferences.Set("isPieChart", value);
            }
        }
        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }
        public int TotalComplaint { get => totalComplaint; set => SetProperty(ref totalComplaint, value); }
        public int PendingComplaint { get => pendingComplaint; set => SetProperty(ref pendingComplaint, value); }
        public int AssignedComplaint { get => assignedComplaint; set => SetProperty(ref assignedComplaint, value); }
        public int InProgressComplaint { get => inProgressComplaint; set => SetProperty(ref inProgressComplaint, value); }
        public int ComplatedComplaint { get => completedComplaint; set => SetProperty(ref completedComplaint, value); }
        public string DateTimeString { get => dateTimeString; set => SetProperty(ref dateTimeString, value); }
        public string ToggleText { get => toggleText; set => SetProperty(ref toggleText, value); }
        public string PieChartTitle { get => pieChartTitle; set => SetProperty(ref pieChartTitle, value); }

        public AsyncCommand LogoutCommand { get; }
        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand ToPendingCommand { get; }
        public AsyncCommand ToAssignedCommand { get; }
        public AsyncCommand ToInProgressCommand { get; }
        public AsyncCommand ToCompletedCommand { get; }

        public StaffHomeViewModel()
        {
            DateTimeString = DateTime.Now.ToString("dd/MM/yyyy");
            LogoutCommand = new AsyncCommand(Logout);
            RefreshCommand = new AsyncCommand(Refresh);
            ToPendingCommand = new AsyncCommand(ToPending);
            ToAssignedCommand = new AsyncCommand(ToAssigned);
            ToInProgressCommand = new AsyncCommand(ToInProgress);
            ToCompletedCommand = new AsyncCommand(ToCompleted);

            IsPieChart = Preferences.Get("isPieChart",false);

            Title = "Home";
            Statistics = new ObservableRangeCollection<Statistic>();
            staffID = Preferences.Get("userID", 0);
            getStatistic();
            test();
        }

        private async void test()
        {
            try
            {
                test2();
            }
            catch (Exception ex)
            {
                var st = new System.Diagnostics.StackTrace(ex, true);
                var frame = st.GetFrame(st.FrameCount - 1);
                var linenumber = frame.GetFileLineNumber();
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
        }

        private void test2()
        {
            try
            {
                throw new Exception("haha");
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        private async Task ToPending()
        {
            await Shell.Current.GoToAsync(pathToPending);
        }

        private async Task ToAssigned()
        {
            await Shell.Current.GoToAsync(pathToAssigned);
        }

        private async Task ToInProgress()
        {
            await Shell.Current.GoToAsync(pathToInProgress);
        }

        private async Task ToCompleted()
        {
            await Shell.Current.GoToAsync(pathToCompleted);
        }

        private async Task Refresh()
        {
            isRefresh = true;
            IsBusy = true;
            getStatistic();
            IsBusy = false;
        }

        private async Task Logout()
        {
            var answer = await Application.Current.MainPage.DisplayAlert("Logout", "Are you sure you want to logout?", "YES", "NO");
            if (answer)
            {
                Preferences.Clear();
                Application.Current.MainPage = new AppShell();
            }
        }
     
        async void getStatistic()
        {
            if (!isRefresh)
            {
                IsLoading = true;
            }
            try
            {
                //TODO: Revise this API
                List<KeyValuePair<string, int>> keyValues = await ComplaintServices.GetStaffComplaintStatistic(staffID);
                foreach (KeyValuePair<string, int> item in keyValues)
                {
                    Statistic statistic = null;
                    if (item.Key == "TotalComplaint")
                    {
                        TotalComplaint = item.Value;
                        PieChartTitle = "Total " + TotalComplaint + " complaints";
                    }
                    else if (item.Key == "PendingComplaint")
                    {
                        PendingComplaint = item.Value;
                    }
                    else if (item.Key == "AssignedComplaint")
                    {
                        AssignedComplaint = item.Value;
                    }
                    else if (item.Key == "InProgressComplaint")
                    {
                        InProgressComplaint = item.Value;
                    }
                    else if (item.Key == "CompletedComplaint")
                    {
                        ComplatedComplaint = item.Value;
                    }
                    if(statistic != null)
                    {
                        Statistics.Add(statistic);
                    }
                }

                if (!isRefresh)
                {
                    foreach (KeyValuePair<string, int> item in keyValues)
                    {
                        Statistic statistic = null;
                        if (item.Key == "PendingComplaint")
                        {
                            statistic = new Statistic();
                            statistic.Name = "Pending";
                            statistic.Value = PendingComplaint;
                        }
                        else if (item.Key == "PendingComplaint")
                        {
                            statistic = new Statistic();
                            statistic.Name = "Pending";
                            statistic.Value = PendingComplaint;
                        }
                        else if (item.Key == "AssignedComplaint")
                        {
                            statistic = new Statistic();
                            statistic.Name = "Assigned";
                            statistic.Value = AssignedComplaint;
                        }
                        else if (item.Key == "InProgressComplaint")
                        {
                            statistic = new Statistic();
                            statistic.Name = "In Progress";
                            statistic.Value = InProgressComplaint;
                        }
                        else if (item.Key == "CompletedComplaint")
                        {
                            statistic = new Statistic();
                            statistic.Name = "Completed";
                            statistic.Value = ComplatedComplaint;
                        }
                        if (statistic != null)
                        {
                            Statistics.Add(statistic);
                        }
                    }
                }
                if (!isRefresh)
                {
                    IsLoading = false;
                }
                isRefresh = false;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
        }
    }
}
