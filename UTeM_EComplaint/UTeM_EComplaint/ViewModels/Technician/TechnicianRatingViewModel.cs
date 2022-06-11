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

namespace UTeM_EComplaint.ViewModels
{
    internal class TechnicianRatingViewModel : ViewModelBase
    {
        string totalReviews;
        public string TotalReviews { get => totalReviews; set => SetProperty(ref totalReviews, value); }

        public ObservableRangeCollection<Complaint> RatingList { get; set; }
        public double SumRating { get => sumRating; set => SetProperty(ref sumRating, value); }
        public double TotalRating { get => totalRating; set => SetProperty(ref totalRating, value); }
        public string AverageRating { get => averageRating; set => SetProperty(ref averageRating, value); }
        public AsyncCommand<object> ItemSelectedCommand { get; }
        public AsyncCommand RefreshCommand { get; }
        public Complaint SelectedComplaint { get => selectedComplaint; set => SetProperty(ref selectedComplaint, value); }
        Complaint selectedComplaint;


        double sumRating;
        double totalRating;
        string averageRating;
        int userID;
        string pathToDetail = $"{nameof(JobDetailPage)}?complaintID=";
        public TechnicianRatingViewModel()
        {
            Title = "Rating";
            userID = Preferences.Get("userID", 0);
            RatingList = new ObservableRangeCollection<Complaint>();
            getData();
            ItemSelectedCommand = new AsyncCommand<object>(ItemSelected);
            RefreshCommand = new AsyncCommand(Refresh);
        }

        async Task Refresh()
        {
            IsBusy = true;
            getData();
            IsBusy = false;
        }

        private async Task ItemSelected(object arg)
        {
            var complaint = arg as Complaint;
            if (complaint == null)
                return;
            SelectedComplaint = null;
            string path = pathToDetail + complaint.ComplaintID;
            await Shell.Current.GoToAsync(path);
        }

        async void getData()
        {
            try
            {
                List<KeyValuePair<string, double>> results = await RatingServices.GetTechnicianTotalRating(userID);
                foreach (KeyValuePair<string, double> item in results)
                {
                    if (item.Key == "SumRating")
                    {
                        SumRating = item.Value;
                    }
                    else if (item.Key == "TotalRating")
                    {
                        TotalRating = item.Value;
                    }
                }
                double averageRating = SumRating / totalRating;
                AverageRating = string.Format("{0:0.0}", averageRating);

                TotalReviews = "(" + TotalRating + " Reviews)";
                List<Complaint> ratings = await RatingServices.GetTechnicianRatings(userID);
                RatingList.ReplaceRange(ratings);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Jobs", ex.Message, "OK");
            }
        }
    }
}
