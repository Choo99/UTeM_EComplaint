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
    internal class AdminHomeViewModel : ViewModelBase
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

        public ObservableRangeCollection<Complaint> complaintList { get; }
        public AsyncCommand LogoutCommand { get; }
        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand ToPendingCommand { get; }
        public AsyncCommand ToAssignedCommand { get; }
        public AsyncCommand ToInProgressCommand { get; }
        public AsyncCommand ToCompletedCommand { get; }

        public AdminHomeViewModel()
        {
            DateTimeString = DateTime.Now.ToString("dd/MM/yyyy");
            LogoutCommand = new AsyncCommand(Logout);
            RefreshCommand = new AsyncCommand(Refresh);
            ToPendingCommand = new AsyncCommand(ToPending);
            ToAssignedCommand = new AsyncCommand(ToAssigned);
            ToInProgressCommand = new AsyncCommand(ToInProgress);
            ToCompletedCommand = new AsyncCommand(ToCompleted);

            IsPieChart = Preferences.Get("isPieChart", false);

            Title = "Home";
            complaintList = new ObservableRangeCollection<Complaint>();
            Statistics = new ObservableRangeCollection<Statistic>();
            staffID = Preferences.Get("userID", 0);
            getStaffComplaint();
            getStatistic();
        }

        private async Task ToPending()
        {
            await Task.Delay(100);
            // await Shell.Current.GoToAsync(pathToPending);
        }

        private async Task ToAssigned()
        {
            await Task.Delay(100);
            // await Shell.Current.GoToAsync(pathToAssigned);
        }

        private async Task ToInProgress()
        {
            await Task.Delay(100);
            // await Shell.Current.GoToAsync(pathToInProgress);
        }

        private async Task ToCompleted()
        {
            await Task.Delay(100);
            // await Shell.Current.GoToAsync(pathToCompleted);
        }

        private async Task Refresh()
        {
            isRefresh = true;
            IsBusy = true;
            await Task.Delay(100);
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
        async void getStaffComplaint()
        {
            try
            {
                List<Complaint> complaints = await ComplaintServices.GetStaffComplaint(staffID);
                complaintList.AddRange(complaints);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
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
                List<KeyValuePair<string, int>> keyValues = await ComplaintServices.GetAllComplaintStatistic();
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
                    if (statistic != null)
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
