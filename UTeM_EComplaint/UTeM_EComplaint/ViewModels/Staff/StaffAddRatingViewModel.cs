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
        ComplaintDetail complaintDetail;

        int ratingValue;

        public int RatingValue { get => ratingValue; set => SetProperty(ref ratingValue,value); }
        public ComplaintDetail ComplaintDetail { get => complaintDetail; set => SetProperty(ref complaintDetail, value); }

        public AsyncCommand SubmitCommand { get; }
        public StaffAddRatingViewModel()
        {
            Title = "Rate technician";
            SubmitCommand = new AsyncCommand(Submit);
        }

        private async Task Submit()
        {
            try
            {
                await RatingServices.AddRating(new Rating
                {
                    Complaint = ComplaintDetail.Complaint,
                    Technician = ComplaintDetail.Technician,
                    RatingValue = RatingValue,
                });
                await Application.Current.MainPage.DisplayAlert("Rate", "You have successfully rated this technician", null, "OK");
                await Shell.Current.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), null, "OK");
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            string complaintID = HttpUtility.UrlDecode(query["complaintID"]);
            int technicianID = Int32.Parse(HttpUtility.UrlDecode(query["technicianID"]));
            getComplaint(complaintID,technicianID);
        }

        async void getComplaint(string complaintID, int technicianID)
        {
            try
            {
                ComplaintDetail = await ComplaintDetailServices.GetComplaintDetail(new ComplaintDetail
                {
                    Complaint = new Complaint
                    {
                        ComplaintID = complaintID,
                    },
                    Technician = new Technician
                    {
                        TechnicianID = technicianID
                    },
                });
                Console.WriteLine("");
            }
            catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
            
        }



    }
}
