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
    internal class StaffEditComplaintViewModel : ViewModelBase, IQueryAttributable
    {
        string complaintID;
        Complaint complaint;
        bool isLocation;
        bool isNotLocation;
        string location;
        ImageSource image;

        public static string base64String;

        readonly string pathToAddAddress = $"{nameof(StaffAddAddressPage)}";

        public bool IsLocation { get => isLocation; set { SetProperty(ref isLocation, value); IsNotLocation = !value; } }
        public bool IsNotLocation { get => isNotLocation; set => SetProperty(ref isNotLocation, value); }
        public Complaint Complaint { get => complaint; set => SetProperty(ref complaint, value); }
        public ImageSource Image { get => image; set => SetProperty(ref image, value); }
        public Map Map { get; private set; }

        public ObservableRangeCollection<Division> divisionList { get; }
        public ObservableRangeCollection<Category> categoryList { get; }
        public ObservableRangeCollection<DamageType> damageTypeList { get; }

        int selectedDivisionIndex = -1;
        int selectedCategoryIndex = -1;
        int selectedDamageTypeIndex = -1;

        public int SelectedDivisionIndex { get => selectedDivisionIndex; set => SetProperty(ref selectedDivisionIndex, value); }
        public int SelectedCategoryIndex { get => selectedCategoryIndex; set => SetProperty(ref selectedCategoryIndex, value); }
        public int SelectedDamageTypeIndex { get => selectedDamageTypeIndex; set => SetProperty(ref selectedDamageTypeIndex, value); }

        public string Location { get => location; set => SetProperty(ref location, value); }

        Division selectedDivision;
        Category selectedCategory;
        DamageType selectedDamageType;

        public Division SelectedDivision { get => selectedDivision; set => SetProperty(ref selectedDivision, value); }
        public Category SelectedCategory { get => selectedCategory; set => SetProperty(ref selectedCategory, value); }
        public DamageType SelectedDamageType { get => selectedDamageType; set => SetProperty(ref selectedDamageType, value); }

        public AsyncCommand SaveCommand { get; }
        public AsyncCommand ClearCommand { get; }

        public AsyncCommand AddLocationCommand { get; }
        public AsyncCommand ClearLocationCommand { get; }

        public StaffEditComplaintViewModel()
        {
            Title = "Edit complaint";
            SaveCommand = new AsyncCommand(Save);
            ClearCommand = new AsyncCommand(Clear);
            AddLocationCommand = new AsyncCommand(AddLocation);
            ClearLocationCommand = new AsyncCommand(ClearLocation);

            IsLocation = true;

            divisionList = new ObservableRangeCollection<Division>();
            categoryList = new ObservableRangeCollection<Category>();
            damageTypeList = new ObservableRangeCollection<DamageType>();

            selectedDivision = new Division();
            selectedCategory = new Category();
            selectedDamageType = new DamageType();

            Map = new Map();
            Map.IsEnabled = false;
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if (query.ContainsKey("campus"))
            {
                string campus = HttpUtility.UrlDecode(query["campus"]).Trim();
                string building = HttpUtility.UrlDecode(query["building"]).Trim();
                string level = HttpUtility.UrlDecode(query["level"]).Trim();
                string department = HttpUtility.UrlDecode(query["department"]).Trim();
                string room = HttpUtility.UrlDecode(query["room"]).Trim();
                Complaint.Longitude = Convert.ToDouble(HttpUtility.UrlDecode(query["longitude"]).Trim());
                Complaint.Latitude = Convert.ToDouble(HttpUtility.UrlDecode(query["latitude"]).Trim());

                Map.MoveToRegion(MapHandler.moveToLocation(Complaint.Latitude, Complaint.Longitude));

                if (base64String != null)
                {
                    Image = ImageHandler.LoadBase64(base64String);
                    Complaint.ImageBase64 = base64String;
                    base64String = null;
                }
                    
                IsLocation = true;
                Location = campus + ", " + building + ", " + level + ", " + department + ", " + room;
            }
            if (query.ContainsKey("complaintID"))
            {
                complaintID = HttpUtility.UrlDecode(query["complaintID"]);
                getComplaintDetail();
            }
        }

        private async Task ClearLocation()
        {
            await Task.Delay(100);
            Location = null;
            IsLocation = false;
        }

        private async Task AddLocation()
        {
            StaffAddAddressViewModel.imageString = Complaint.ImageBase64;
            await Shell.Current.GoToAsync(string.Format("{0}?longitude={1}&latitude={2}&location={3}&edit=&image=",pathToAddAddress,Complaint.Longitude,Complaint.Latitude,Complaint.Location));
        }

        private async Task Clear()
        {
            var answer = await Application.Current.MainPage.DisplayAlert("Clear", "Are you sure you want to clear the dtaa?", "YES", "NO");
            if (answer)
            {

                Complaint newComplaint = new Complaint
                {
                    ComplaintID = Complaint.ComplaintID,
                    ComplaintDate = Complaint.ComplaintDate
                };

                SelectedCategoryIndex = -1;
                SelectedDamageTypeIndex = -1;
                SelectedDivisionIndex = -1;

                SelectedCategory = null;
                SelectedDamageType = null;
                selectedDivision = null;

                Location = null;
                IsLocation=false;

                Complaint = newComplaint;
            }
        }

        private async Task Save()
        {
            try
            {
                var answer = await Application.Current.MainPage.DisplayAlert("Save", "Are you sure you want to save it?", "YES", "NO");
                if (answer)
                {
                    Complaint.Division = selectedDivision;
                    complaint.Category = selectedCategory;
                    complaint.DamageType = selectedDamageType;
                    Complaint.Location = location;

                    //longitude, latitude & image solved in query

                    complaint.Staff = new Staff
                    {
                        StaffID = Preferences.Get("userID", 0),
                    };
                    int result = await ComplaintServices.StaffEditComplaint(Complaint);
                    if(result != 0)
                    {
                        await Application.Current.MainPage.DisplayAlert("Success", "Update Complaint: " + complaintID + " successfully!", null,"OK");
                        await Shell.Current.GoToAsync("..");
                    }
                    else
                    {
                        throw new Exception("Some error in database");
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
        }

        async void getComplaintDetail()
        {
            try
            {
                IsBusy = true;
                Complaint = await ComplaintServices.GetComplaintDetail(complaintID);

                //Initiate value of location
                Image = ImageHandler.LoadBase64(Complaint.ImageBase64);

                Map.MoveToRegion(MapHandler.moveToLocation(Complaint.Latitude, Complaint.Longitude));

                List<Division> divisions = await DivisionServices.GetDivisions();
                List<Category> categories = await CategoryServices.GetCategories();
                List<DamageType> damageTypes = await DamageTypeServices.GetDamageTypes();

                divisionList.ReplaceRange(divisions);
                categoryList.ReplaceRange(categories);
                damageTypeList.ReplaceRange(damageTypes);

                SelectedDivision = Complaint.Division;
                SelectedCategory = Complaint.Category;
                SelectedDamageType = Complaint.DamageType;
                Location = Complaint.Location;

                SelectedDivisionIndex = divisions.FindIndex( item=> item.DivisionID == SelectedDivision.DivisionID);
                SelectedDamageTypeIndex = damageTypes.FindIndex( item=> item.DamageTypeID == SelectedDamageType.DamageTypeID);
                SelectedCategoryIndex = categories.FindIndex( item=> item.CategoryId == SelectedCategory.CategoryId);
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
