using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MvvmHelpers;
using MvvmHelpers.Commands;
using UTeM_EComplaint.Model;
using UTeM_EComplaint.Services;
using UTeM_EComplaint.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Command = MvvmHelpers.Commands.Command;

namespace UTeM_EComplaint.ViewModels
{
    internal class AdminAssignTechnicianViewModel : ViewModelBase
    {
        readonly int LOAD_SIZE = 5;

        int previousCheckedComplaintDetailIndex = -1;
        bool isConfirmed;

        List<object> selectedTechnicians;
        List<ComplaintDetail> complaintDetails;

        ComplaintDetail selectedSupervisor;

        public ObservableRangeCollection<ComplaintDetail> ComplaintDetailList { get; }
        public ObservableRangeCollection<ComplaintDetail> SelectedComplaintDetails { get; }

        public ComplaintDetail SelectedSupervisor { get => selectedSupervisor; set => SetProperty(ref selectedSupervisor, value); }
        public bool IsConfirmed { get => isConfirmed; set => SetProperty(ref isConfirmed, value); }
        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand<IList<object>> ItemSelectedCommand { get; }
        public AsyncCommand<object> CheckedCommand { get; }
        public AsyncCommand LoadMoreCommand { get; }
        public AsyncCommand SubmitCommand { get; }
        public AsyncCommand ClearCommand { get; }
        public AsyncCommand<object> SelectSupervisorCommand { get; }

        public AdminAssignTechnicianViewModel()
        {
            ComplaintDetailList = new ObservableRangeCollection<ComplaintDetail>();
            SelectedComplaintDetails = new ObservableRangeCollection<ComplaintDetail>();
            complaintDetails = new List<ComplaintDetail>();
            selectedTechnicians = new List<object>();

            ItemSelectedCommand = new AsyncCommand<IList<object>>(ItemSelected);
            CheckedCommand = new AsyncCommand<object>(Checked);
            RefreshCommand = new AsyncCommand(Refresh);
            LoadMoreCommand = new AsyncCommand(LoadMore);
            SubmitCommand = new AsyncCommand(Submit);
            ClearCommand = new AsyncCommand(Clear);
            SelectSupervisorCommand = new AsyncCommand<object>(SelectSupervisor);
            getData();
        }

        private async Task SelectSupervisor(object obj)
        {
            ComplaintDetail complaintDetail = obj as ComplaintDetail;
            var ans = await Application.Current.MainPage.DisplayAlert("Supervisor", $"Are you sure you want to choose {complaintDetail.Technician.TechnicianName} as supervisor?","YES","NO");

            if (ans)
            {
                SelectedComplaintDetails[SelectedComplaintDetails.IndexOf(complaintDetail)].Supervisor = true;
                string parameters = "";
                for (int i = 0; i < SelectedComplaintDetails.Count; i++)
                {
                    ComplaintDetail complaintDetailItem = SelectedComplaintDetails[i] as ComplaintDetail;
                    parameters += $"technicianID{i + 1}={complaintDetailItem.Technician.TechnicianID}&supervisor{i + 1}={complaintDetailItem.Supervisor}&";
                }
                IsConfirmed = false;
                await Shell.Current.GoToAsync("..?" + parameters);
            }
        }

        private async Task Clear()
        {
            await Task.Delay(100);
        }

        private async Task Checked(object obj)
        {
            /*ComplaintDetail currentComplaintDetail = obj as ComplaintDetail;

            int currentCheckedComplaintDetailIndex = ComplaintDetailList.IndexOf(currentComplaintDetail);

            if (previousCheckedComplaintDetailIndex != -1 && currentComplaintDetail.Supervisor)
            {
                ComplaintDetailList[previousCheckedComplaintDetailIndex].Supervisor = false;
            }

            previousCheckedComplaintDetailIndex = currentCheckedComplaintDetailIndex;
            await Task.Delay(100);*/
            //ComplaintDetailList[ComplaintDetailList.IndexOf(obj as ComplaintDetail)].Supervisor = false ;
        }

        private async Task Submit()
        {
            if (SelectedComplaintDetails.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Select", "Please select at least one technician", "OK");
                return;
            }
            IsConfirmed = true;
        }

        private async Task LoadMore()
        {
            await Task.Delay(100);
            if (ComplaintDetailList.Count == complaintDetails.Count)
                return;

            int lastItemIndexed = ComplaintDetailList.Count;
            int nextItemIndexed = lastItemIndexed + LOAD_SIZE;

            if (nextItemIndexed > complaintDetails.Count)
                nextItemIndexed = complaintDetails.Count;

            for (int i = lastItemIndexed; i < nextItemIndexed; i++)
            {
                ComplaintDetailList.Add(complaintDetails[i]);
            }
        }

        async void getData()
        {
            try
            {
                int size = LOAD_SIZE;
                IsBusy = true;
                List<Technician> technicians = await TechnicianServices.GetAllTechnicianWithStatistic();
                foreach (Technician technician in technicians)
                {
                    ComplaintDetail complaintDetail = new ComplaintDetail
                    {
                        Technician = technician,
                    };
                    complaintDetails.Add(complaintDetail);
                }
                if (complaintDetails.Count < LOAD_SIZE)
                    size = complaintDetails.Count;
                ComplaintDetailList.ReplaceRange(complaintDetails.GetRange(0, size));
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
        private async Task ItemSelected(IList<object> arg)
        {
            try
            {
                //if(SelectedComplaintDetails != 0 && arg.Count != SelectedComplaintDetails)
                List<ComplaintDetail> complaintDetailList = new List<ComplaintDetail>();
                foreach (var complaintDetail in arg)
                {
                    var complaintComplaintDetail = complaintDetail as ComplaintDetail;
                    complaintDetailList.Add(complaintComplaintDetail);
                }
                SelectedComplaintDetails.ReplaceRange(complaintDetailList);
            }catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
        }
        async Task Refresh()
        {
            IsBusy = true;
            getData();
            IsBusy = false;
        }
    }
}
