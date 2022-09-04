using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UTeM_EComplaint.Model;
using UTeM_EComplaint.Services;
using UTeM_EComplaint.Tools;
using UTeM_EComplaint.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Map = Xamarin.Forms.Maps.Map;

namespace UTeM_EComplaint.ViewModels
{
    internal class AdminAssignDetailViewmModel : ViewModelBase, IQueryAttributable
    {
        readonly string pathToAdminDetail = $"//AdminViewAll/{nameof(AdminComplaintDetailPage)}?complaintID=";

        string complaintID;
        bool isNotAssigned = true;
        bool isAssigned;
        bool isSoftware;
        bool isHardware;
        List<ComplaintDetail> complaintDetailList;
        ImageSource image;
        public Map Map { get; private set; }
        public ImageSource Image { get => image; set => SetProperty(ref image, value); }

        public bool IsAssigned { get => isAssigned; set { SetProperty(ref isAssigned, value); IsNotAssigned = !value; } }
        public bool IsNotAssigned { get => isNotAssigned; set { SetProperty(ref isNotAssigned, value); } }
        public bool IsSoftware { get => isSoftware; set { SetProperty(ref isSoftware, value); } }
        public bool IsHardware { get => isHardware; set { SetProperty(ref isHardware, value); } }

        Complaint complaint;
        Technician technician;
        public ObservableRangeCollection<ComplaintDetail> SelectedComplaintDetails { get; }

        string pathToAssignTechnician = $"{nameof(AdminAssignTechnicianPage)}";

        public Complaint Complaint { get => complaint; set => SetProperty(ref complaint, value); }
        public Technician Technician { get => technician; set => SetProperty(ref technician, value); }

        public AsyncCommand AssignCommand { get; }
        public AsyncCommand AddTechnicianCommand { get; }
        public AsyncCommand DoneCommand { get; }
        public AsyncCommand<object> DeleteTechnicianCommand { get; }
        public AdminAssignDetailViewmModel()
        {

            SelectedComplaintDetails = new ObservableRangeCollection<ComplaintDetail>();

            DoneCommand = new AsyncCommand(Done);
            AssignCommand = new AsyncCommand(Assign);
            AddTechnicianCommand = new AsyncCommand(AddTechnician);
            DeleteTechnicianCommand = new AsyncCommand<object>(DeleteTechnician);

            Map = new Map
            {
                IsEnabled = false
            };
        }

        private async Task DeleteTechnician(object obj)
        {
            await Task.Delay(100);
            ComplaintDetail complaintDetail = obj as ComplaintDetail;

            if(SelectedComplaintDetails.Count != 1 && complaintDetail.Supervisor)
            {
                await Application.Current.MainPage.DisplayAlert("Delete", "Cannot delete supervisor from the selected technician!", "OK");
                return;
            }
            complaintDetailList.Remove(complaintDetail);
            SelectedComplaintDetails.Remove(complaintDetail);
            if (SelectedComplaintDetails.Count == 0)
            {
                IsAssigned = false;
            }
        }

        private async Task AddTechnician()
        {
            await Shell.Current.Navigation.PushModalAsync(new NavigationPage(new AdminAssignTechnicianPage()));
        }

        private async Task Assign()
        {
            try
            {
                if ( !await validation())
                    return;
                var answer = await Application.Current.MainPage.DisplayAlert("Assign", "Are you confirm your selection?", "YES", "No");
                if (answer)
                {
                    IsBusy = true;
                    int result = await ComplaintDetailServices.AddComplaintDetails(complaintDetailList);
                    if (result != 0)
                    {
                        await Application.Current.MainPage.DisplayAlert("Success", "Assign job to technicians successfully", null, "OK");
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

        private async Task<bool> validation()
        {
            foreach(var complaintDetail in complaintDetailList)
            {
                if(complaintDetail.JobDescription == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Please fill the in job description for every technician", null, "OK");
                    return false;
                }
                   
            }
            return true;
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
            if (query.ContainsKey("technicianID1"))
            {
                try
                {
                    int counter = 1;
                    List<Technician> technicians = new List<Technician>();
                    List<bool> supervisors = new List<bool>();
                    while (query.ContainsKey("technicianID" + counter))
                    {
                        Technician technician = new Technician
                        {
                            TechnicianID = int.Parse(HttpUtility.UrlDecode(query["technicianID" + counter]))
                        };
                        technicians.Add(technician);
                        supervisors.Add(HttpUtility.UrlDecode(query["supervisor" + counter]) == "True" ? true : false);
                        counter++;
                    }
                    getStaffDetail(technicians,supervisors);
                }
                catch (Exception ex)
                {
                    Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
                }
            }
        }

        async void getStaffDetail(List<Technician> technicians, List<bool> supervisors)
        {
            try
            {
                IsBusy = true;
                technicians = await TechnicianServices.GetTechnicianWithStatistic(technicians);

                List<ComplaintDetail> complaintDetails = new List<ComplaintDetail>();
                for(int i = 0; i < technicians.Count && i <supervisors.Count; i++)
                {
                    ComplaintDetail complaintDetail = new ComplaintDetail
                    {
                        Complaint = new Complaint
                        {
                            ComplaintID = complaintID
                        },
                        Technician = technicians[i],
                        Supervisor = supervisors[i],
                    };
                    complaintDetails.Add(complaintDetail);
                }
                complaintDetailList = complaintDetails;
                SelectedComplaintDetails.AddRange(complaintDetails);
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
                if(Complaint.ComplaintType.ComplaintTypeCode == "S")
                {
                    IsSoftware = true;
                }
                else
                {
                    IsHardware = true;
                }
                if (Complaint.ImageBase64 != null)
                    Image = ImageHandler.LoadBase64(Complaint.ImageBase64);
                if (Complaint.Longitude != 0 && Complaint.Latitude != 0)
                    Map.MoveToRegion(MapHandler.moveToLocation(Complaint.Latitude, Complaint.Longitude));
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
