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
using UTeM_EComplaint.Views;
using Xamarin.Forms;

namespace UTeM_EComplaint.ViewModels
{
    internal class JobInProgressDetailViewModel : ViewModelBase, IQueryAttributable
    {
        public Complaint Complaint { get => complaint; set => SetProperty(ref complaint, value); }
        public AsyncCommand BackCommand { get; }

        ActionTaken actionTaken;
        public ActionTaken ActionTaken { get => actionTaken; set => SetProperty(ref actionTaken, value); }

        Complaint complaint;
        string complaintID;
        string pathToCompleted = $"../{nameof(JobCompletedDetailPage)}?complaintID=";
        public JobInProgressDetailViewModel()
        {
            Title = "Task Information";
            actionTaken = new ActionTaken();
            BackCommand = new AsyncCommand(Back);
        }

        private async Task Back()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            complaintID = HttpUtility.UrlDecode(query["complaintID"]);
            getComplaintDetail();
        }

        private async void getComplaintDetail()
        {
            try
            {
                Complaint = await ComplaintServices.GetComplaintDetail(complaintID);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
        }
    }
}
