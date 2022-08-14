using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UTeM_EComplaint.Model;
using UTeM_EComplaint.Services;
using UTeM_EComplaint.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace UTeM_EComplaint.ViewModels
{
    internal class AdminAssignDetailViewmModel : ViewModelBase, IQueryAttributable
    {
        readonly string pathToAdminDetail = $"//AdminViewAll/{nameof(AdminComplaintDetailPage)}?complaintID=";

        string complaintID;
        bool isNotAssigned = true;
        bool isAssigned;
        public bool IsAssigned { get => isAssigned; set { SetProperty(ref isAssigned, value); IsNotAssigned = !value; } }
        public bool IsNotAssigned { get => isNotAssigned; set { SetProperty(ref isNotAssigned, value); } }

        Complaint complaint;
        Technician technician;

        string pathToAssignTechnician = $"{nameof(AdminAssignTechnicianPage)}";

        public Complaint Complaint { get => complaint; set => SetProperty(ref complaint, value); }
        public Technician Technician { get => technician; set => SetProperty(ref technician, value); }

        public AsyncCommand AssignCommand { get; }
        public AsyncCommand AddTechnicianCommand { get; }
        public AsyncCommand DoneCommand { get; }
        public AsyncCommand DeleteTechnicianCommand { get; }
        public AdminAssignDetailViewmModel()
        {
            Title = "Complaint Detail";

            DoneCommand = new AsyncCommand(Done);
            AssignCommand = new AsyncCommand(Assign);
            AddTechnicianCommand = new AsyncCommand(AddTechnician);
            DeleteTechnicianCommand = new AsyncCommand(DeleteTechnician);
        }

        private async Task DeleteTechnician()
        {
            await Task.Delay(100);
            Technician = null;
            IsAssigned = false;
        }

        private async Task AddTechnician()
        {
            await Shell.Current.Navigation.PushModalAsync(new NavigationPage(new AdminAssignTechnicianPage()));
        }

        private async Task Assign()
        {
            try
            {
                var answer = await Application.Current.MainPage.DisplayAlert("Assign", "Are you want to assign technician(ID " + Technician.TechnicianID + ") to complaint(" + Complaint.ComplaintID + ")?", "YES", "No");
                if (answer)
                {
                    IsBusy = true;
                    Complaint.Technician = Technician;
                    int result = await TechnicianServices.UpdateTechnicianAndSubscribe(Complaint);
                    if (result != 0)
                    {
                        await Application.Current.MainPage.DisplayAlert("Success", "Assign job to technician(ID " + Technician.TechnicianID + ") successfully", null, "OK");
                        Technician = null;
                        IsAssigned = false;

                        await Shell.Current.GoToAsync("..");
                        await Shell.Current.GoToAsync(pathToAdminDetail + complaintID);
                    }
                    else
                    {
                        throw new Exception("Update in database fail. Please try again");
                    }
                 }
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

        private async Task Done()
        {

            await Shell.Current.GoToAsync("..");
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if (query.ContainsKey("complaintID"))
            {
                complaintID = HttpUtility.UrlDecode(query["complaintID"]);
                getComplaintDetail();
            }
            if (query.ContainsKey("technicianID"))
            {
                int technicianID = int.Parse(HttpUtility.UrlDecode(query["technicianID"]));
                getStaffDetail(technicianID);
            }
        }

        async void getStaffDetail(int technicianID)
        {
            try
            {
                IsBusy = true;
                Technician technician = await TechnicianServices.GetTechnicianWithStatistic(technicianID);
                Technician = technician;
                IsAssigned = true;
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

        async void getComplaintDetail()
        {
            try
            {
                IsBusy = true;
                Complaint = await ComplaintServices.GetComplaintDetail(complaintID);
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
    }
}
