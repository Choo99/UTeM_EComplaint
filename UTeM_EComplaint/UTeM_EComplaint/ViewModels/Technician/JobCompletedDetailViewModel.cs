using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UTeM_EComplaint.Model;
using UTeM_EComplaint.Services;
using Xamarin.Forms;

namespace UTeM_EComplaint.ViewModels
{
    internal class JobCompletedDetailViewModel : ViewModelBase,IQueryAttributable
    {
        bool isRated;
        bool isNotRated;

        public bool IsRated
        { 
            get => isRated;
            set
            {
                SetProperty(ref isRated, value);
                IsNotRated = !value;
            }
        }
        public bool IsNotRated 
        { 
            get => isNotRated; 
            set => SetProperty(ref isNotRated, value); 
        }

        public Complaint Complaint{ get => complaint; set => SetProperty(ref complaint, value); }

        public AsyncCommand DoneCommand { get; }
        public AsyncCommand BackCommand { get; }

        Complaint complaint;
        int complaintID;
        public JobCompletedDetailViewModel()
        {
            Title = "Task Information";
            DoneCommand = new AsyncCommand(Done);
            BackCommand = new AsyncCommand(Back);
        }

        private async Task Back()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        private async Task Done()
        {
            await Shell.Current.GoToAsync("//AboutPage");
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            complaintID = int.Parse(HttpUtility.UrlDecode(query["complaintID"]));
            getComplaintDetail();
        }

        private async void getComplaintDetail()
        {
            try
            {
                Complaint = await ComplaintServices.GetComplaintDetail(complaintID);
                if(complaint.Rating != null)
                {
                    IsRated = true;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
        }
    }
}
