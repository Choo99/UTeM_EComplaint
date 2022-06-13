﻿using MvvmHelpers;
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
        readonly int LOAD_SIZE = 5;

        public ObservableRangeCollection<Complaint> ComplaintList { get; set; }

        string toggleText;

        int totalTask;
        int completeTask;
        int inProgressTask;
        int pendingTask;
        int technicianID;

        bool isRefresh;
        bool isLoading;
        bool isPieChart;

        List<Complaint> complaints;
        public ObservableRangeCollection<Statistic> Statistics { get;}

        public string ToggleText { get => toggleText; set => SetProperty(ref toggleText, value); }

        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }
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

        string dateTime;
        string pieChartTitle;
        string pathToToDo = $"{nameof(JobTodoPage)}";
        string pathToInProgress = $"{nameof(JobProgressPage)}";
        string pathToCompleted = $"{nameof(JobHistoryPage)}";

        string pathToToDoDetail = $"{nameof(JobToDoDetailPage)}?complaintID=";
        string pathToInProgressDetail = $"{nameof(JobInProgressDetailPage)}?complaintID=";
        string pathToCompletedDetail = $"{nameof(JobCompletedDetailPage)}?complaintID=";
        string pathToDetail = $"{nameof(JobDetailPage)}?complaintID=";

        public Complaint SelectedComplaint
        {
            get => selectedComplaint;
            set
            {
                SetProperty(ref selectedComplaint, value);
                OnPropertyChanged();
            }
        }

        public string PieChartTitle { get => pieChartTitle; set => SetProperty(ref pieChartTitle, value); }

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
        public AsyncCommand LoadMoreCommand { get; }
        public AsyncCommand<object> ItemSelectedCommand { get; }
         

        public AboutViewModel()
        {
            DateTimeString = DateTime.Now.ToString("dd/MM/yyyy");
            ComplaintList = new ObservableRangeCollection<Complaint>();
            technicianID = Preferences.Get("userID", 0);

            complaints = new List<Complaint>();
            Statistics = new ObservableRangeCollection<Statistic>();

            TotalTabbedCommand = new AsyncCommand(TotalTabbed);
            PendingTabbedCommand = new AsyncCommand(PendingTabbed);
            InProgressTabbedCommand = new AsyncCommand(InProgressTabbed);
            CompletedTabbedCommand = new AsyncCommand(CompletedTabbed);
            RefreshCommand = new AsyncCommand(Refresh);
            LogoutCommand = new AsyncCommand(Logout);
            LoadMoreCommand = new AsyncCommand(LoadMore);
            ItemSelectedCommand = new AsyncCommand<object>(ItemSelected);

            GetData();
        }

        private async Task ItemSelected(object arg)
        {
            var complaint = arg as Complaint;
            if (complaint == null)
                return;
            SelectedComplaint = null;
            await Shell.Current.GoToAsync(pathToDetail + complaint.ComplaintID);
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
            isRefresh = true;
            IsBusy = true;

            await Task.Delay(100);
            pendingTask = 0;
            inProgressTask = 0;
            completeTask = 0;
            totalTask = 0;

            GetData();
            IsBusy = false;
        }

        private async Task LoadMore()
        {
            await Task.Delay(100);
            if (complaints.Count == ComplaintList.Count)
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

        private async void GetData()
        {
            try
            {
                if (!isRefresh)
                    isLoading = true;

                int size = LOAD_SIZE;

                complaints = await ComplaintServices.GetTechnicianComplaint(technicianID);
                if (complaints.Count < LOAD_SIZE)
                {
                    size = complaints.Count;
                }
                ComplaintList.ReplaceRange(complaints.GetRange(0, size));

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
                        PieChartTitle = "Total " + TotalTask + " complaints";
                    }
                }

                if (!isRefresh)
                {
                    foreach (KeyValuePair<string, int> item in list)
                    {
                        Statistic statistic = null;
                        if (item.Key == "Assigned")
                        {
                            statistic = new Statistic();
                            statistic.Name = "Assigned";
                            statistic.Value = PendingTask;
                        }
                        else if (item.Key == "In Progress")
                        {
                            statistic = new Statistic();
                            statistic.Name = "In Progress";
                            statistic.Value = InProgressTask;
                        }
                        else if (item.Key == "Completed")
                        {
                            statistic = new Statistic();
                            statistic.Name = "Completed";
                            statistic.Value = CompleteTask;
                        }
                        if (statistic != null)
                        {
                            Statistics.Add(statistic);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
                
            }
            finally
            {
                IsLoading = false;
                isRefresh = false;
            }

        }
    }
}

