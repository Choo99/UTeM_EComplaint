using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UTeM_EComplaint.Model;
using UTeM_EComplaint.Services;
using Xamarin.Forms;

namespace UTeM_EComplaint.ViewModels
{
    internal class StaffAddRatingViewModel : ViewModelBase, IQueryAttributable
    {
        Complaint complaint;
        string complaintID;
        int ratingValue;
        public int RatingValue { get => ratingValue; set => SetProperty(ref ratingValue,value); }
        public Complaint Complaint { get => complaint; set => SetProperty(ref complaint, value); }

        public AsyncCommand SubmitCommand { get; }
        public StaffAddRatingViewModel()
        {
            Title = "Rate technician";
            SubmitCommand = new AsyncCommand(Submit);
        }

        private async Task Submit()
        {
            int result = await RatingServices.AddRatingAndSendMessage(complaintID,ratingValue);
            if(result != 0)
            {
                await Application.Current.MainPage.DisplayAlert("Rate", "You have successfully rated this technician",null,"OK");
                await Shell.Current.Navigation.PopAsync();
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            complaintID = HttpUtility.UrlDecode(query["complaintID"]);
            getComplaint();
        }

        async void getComplaint()
        {
            try
            {
                Complaint = await ComplaintServices.GetComplaintDetail(complaintID);
            }
            catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
            
        }



    }
}
