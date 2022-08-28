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
using Xamarin.Forms.Maps;
using Map = Xamarin.Forms.Maps.Map;

namespace UTeM_EComplaint.ViewModels
{
    internal class JobDetailViewModel : ViewModelBase, IQueryAttributable
    {
        string complaintID;
        bool isNotAssigned;
        bool isAssigned;
        bool isInProgress;
        bool isCompleted;
        bool isRating;
        bool isNotRating;
        bool isSoftware;
        bool isHardware;

        string duration;

        public ObservableRangeCollection<ComplaintDetail> TechnicianList { get; }

        ComplaintDetail complaintDetail;
        ImageSource image;
        Map map;

        string pathToEditJob = $"{nameof(JobEditPage)}?complaintID=";
        string pathToDetail = $"../{nameof(JobDetailPage)}?complaintID=";

        public ComplaintDetail ComplaintDetail { get => complaintDetail; set => SetProperty(ref complaintDetail, value); }
        public ImageSource Image { get => image; set => SetProperty(ref image, value); }
        public Map Map { get => map; set => SetProperty(ref map, value); }
        public bool IsAssigned { get => isAssigned; set { SetProperty(ref isAssigned, value); IsNotAssigned = !value; } }
        public bool IsNotAssigned { get => isNotAssigned; set => SetProperty(ref isNotAssigned, value); }
        public bool IsInProgress { get => isInProgress; set => SetProperty(ref isInProgress, value); }
        public bool IsCompleted { get => isCompleted; set => SetProperty(ref isCompleted, value); }
        public bool IsNotRating { get => isNotRating; set => SetProperty(ref isNotRating, value); }
        public bool IsRating { get => isRating; set { SetProperty(ref isRating, value); IsNotRating = !value; } }
        public bool IsSoftware { get => isSoftware; set => SetProperty(ref isSoftware, value); }
        public bool IsHardware { get => isHardware; set => SetProperty(ref isHardware, value); }

        public string Duration { get => duration; set => SetProperty(ref duration, value); }

        public AsyncCommand DoneCommand { get; }
        public AsyncCommand BackCommand { get; }
        public AsyncCommand EditCommand { get; }
        public AsyncCommand StartCommand { get; }
        public AsyncCommand FinishCommand { get; }
        public AsyncCommand OpenMapCommand { get; }
        public AsyncCommand UpdateReportCommand { get; }

        public JobDetailViewModel()
        {
            Title = "Task Information";

            TechnicianList = new ObservableRangeCollection<ComplaintDetail>();

            DoneCommand = new AsyncCommand(Done);
            BackCommand = new AsyncCommand(Back);
            EditCommand = new AsyncCommand(Edit);
            StartCommand = new AsyncCommand(Start);
            FinishCommand = new AsyncCommand(Finish);
            OpenMapCommand = new AsyncCommand(OpenMap);
            UpdateReportCommand = new AsyncCommand(UpdateReport);

            Map = new Map
            {
                IsEnabled = false
            };
        }

        private async Task UpdateReport()
        {
            try
            {
                var answer = await Application.Current.MainPage.DisplayAlert("Start", "Are you confirm the information?", "YES", "NO");
                if (answer)
                {
                    await ComplaintServices.UpdateComplaintReportAndCompleteComplaint(ComplaintDetail.Complaint);

                    string path = pathToDetail + ComplaintDetail.Complaint.ComplaintID;
                    await Shell.Current.GoToAsync(path);

                    await Application.Current.MainPage.DisplayAlert("Success", "Complete the job successfully", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
        }

        private async Task OpenMap()
        {
            if (complaintDetail.Complaint.Latitude != 0 && complaintDetail.Complaint.Longitude != 0)
                await Xamarin.Essentials.Map.OpenAsync(ComplaintDetail.Complaint.Latitude, ComplaintDetail.Complaint.Longitude);
        }

        private async Task Edit()
        {
            await Shell.Current.GoToAsync(pathToEditJob + complaintID);
        }

        private async Task Start()
        {
            try
            {
                var answer = await Application.Current.MainPage.DisplayAlert("Start", "Are you sure you want to start the job?", "YES", "NO");
                if (answer)
                {
                    try
                    {
                        ActionTaken action = new ActionTaken()
                        {
                            Complaint = new Complaint
                            {
                                ComplaintID = complaintID,
                            },
                            Technician = new Technician
                            {
                                TechnicianID = Preferences.Get("userID", 0)
                            }
                        };
                        await ActionServices.StartAction(action);
                        string path = pathToDetail + ComplaintDetail.Complaint.ComplaintID;
                        await Shell.Current.GoToAsync(path);
                        await Application.Current.MainPage.DisplayAlert("Success", "Started the job successfully", "OK");
                    }
                    catch (Exception ex)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK", "NO");
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message + ex.ToString(), "OK");
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            complaintID = HttpUtility.UrlDecode(query["complaintID"]);
            getComplaintDetail();
        }

        private async Task Finish()
        {
            try
            {
                if (!await validate())
                    return;
                var answer = await Application.Current.MainPage.DisplayAlert("Finish", "Are you sure you want to finish the job?", "Yes", "No");
                if (answer)
                {
                    IsBusy = true;
                    await ActionServices.EndActionAndCompleteComplaintDetail(ComplaintDetail);
                    await Shell.Current.GoToAsync(pathToDetail + ComplaintDetail.Complaint.ComplaintID);

                    await Application.Current.MainPage.DisplayAlert("Success", "You complete the job ID: " + complaintDetail.Complaint.ComplaintID + " successfully", "OK");
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

        private async Task Back()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        private async Task Done()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        private async void getComplaintDetail()
        {
            try
            {
                IsBusy = true;
                ComplaintDetail temp = new ComplaintDetail
                {
                    Complaint = new Complaint
                    {
                        ComplaintID = complaintID,
                    },
                    Technician = new Technician
                    {
                        TechnicianID = Preferences.Get("userID", 0)
                    }
                };

                temp = await ComplaintDetailServices.GetComplaintDetailTechnician(temp);

                if(temp.Complaint.ComplaintStatus != "Completed")
                {
                    int result = await ComplaintDetailServices.CheckComplaintDetailCompleteness(temp);
                    if (result == 0 && temp.Supervisor)
                    {
                        temp.IsSupervisorAndAllCompleted = true;
                    }
                }
                if(temp.Complaint.ComplaintType.ComplaintTypeCode == "S")
                {
                    IsSoftware = true;
                }
                else
                {
                    IsHardware = true;
                }

                ComplaintDetail = temp;
                List<ComplaintDetail> technicianList = await ComplaintDetailServices.GetComplaintDetails(temp.Complaint);
                TechnicianList.ReplaceRange(technicianList);

                /* if(ComplaintDetail.ComplaintDetailStatus == "Assigned")
                 {
                     IsAssigned = true;
                     IsInProgress = false;
                     IsCompleted = false;
                 }
                 else if(ComplaintDetail.ComplaintDetailStatus == "In Progress")
                 {
                     IsAssigned = false;
                     IsInProgress = true;
                     IsCompleted = false;
                 }
                 else if(ComplaintDetail.ComplaintDetailStatus == "Completed")
                 {
                     IsAssigned = false;
                     IsCompleted = true;
                     IsInProgress = false;
                     if (complaintDetail.Rating != null)
                     {
                         IsRating = true;
                     }
                     else
                     {
                         IsRating=false;
                     }

                 }*/

                if (ComplaintDetail.Complaint.ComplaintStatus != "Completed" && ComplaintDetail.IsSupervisorAndAllCompleted)
                {
                    await Application.Current.MainPage.DisplayAlert("Report", "Please fill in the report to complete the complaint","OK");
                }

                if(ComplaintDetail.Complaint.Longitude != 0 && ComplaintDetail.Complaint.Latitude != 0)
                    Map.MoveToRegion(MapHandler.moveToLocation(ComplaintDetail.Complaint.Latitude, ComplaintDetail.Complaint.Longitude));
                if(ComplaintDetail.Complaint.ImageBase64 != null)
                    Image = ImageHandler.LoadBase64(ComplaintDetail.Complaint.ImageBase64);
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

        async Task<bool> validate()
        {
            if(ComplaintDetail.Action.ActionDescription == null || ComplaintDetail.Action.SpareReplace == null || ComplaintDetail.Action.AdditionalNote == null)
            {
                if (ComplaintDetail.Action.ActionDescription == null)
                    await Application.Current.MainPage.DisplayAlert("Fill", "Please find in the action taken field","OK");
                else if(ComplaintDetail.Action.SpareReplace == null)
                    await Application.Current.MainPage.DisplayAlert("Fill", "Please find in the spare replace field", "OK");
                else if (ComplaintDetail.Action.AdditionalNote == null)
                    await Application.Current.MainPage.DisplayAlert("Fill", "Please find in the additional note field", "OK");
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
