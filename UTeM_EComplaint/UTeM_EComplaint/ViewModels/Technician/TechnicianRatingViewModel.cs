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
        public ObservableRangeCollection<Complaint> RatingList { get; set; }
        public double SumRating { get => sumRating; set => SetProperty(ref sumRating, value); }
        public double TotalRating { get => totalRating; set => SetProperty(ref totalRating, value); }
        public double AverageRating { get => averageRating; set => SetProperty(ref averageRating, value); }
        public AsyncCommand<object> ItemSelectedCommand { get; }
        public AsyncCommand RefreshCommand { get; }
        public Complaint SelectedComplaint { get => selectedComplaint; set => SetProperty(ref selectedComplaint, value); }
        Complaint selectedComplaint;


        double sumRating;
        double totalRating;
        double averageRating;
        int userID;
        string pathToDetail = $"{nameof(JobDetailPage)}?complaintID=";
        public TechnicianRatingViewModel()
        {
            Title = "Rating";
            userID = Preferences.Get("userID", 0);
            RatingList = new ObservableRangeCollection<Complaint>();
            getTotalRating();
            getRating();
            ItemSelectedCommand = new AsyncCommand<object>(ItemSelected);
            RefreshCommand = new AsyncCommand(Refresh);
        }

        async Task Refresh()
        {
            IsBusy = true;
            getRating();
            getTotalRating();
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

        async void getRating()
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
                    else if (item.Key == "AverageRating")
                    {
                        AverageRating = item.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Jobs", ex.Message, "OK");
            }
        }

        async void getTotalRating()
        {
            try
            {
                List<Complaint> ratings = await RatingServices.GetTechnicianRatings(userID);
                RatingList.AddRange(ratings);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
