using MvvmHelpers;
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
    internal class StaffEditComplaintViewModel : ViewModelBase, IQueryAttributable
    {
        int complaintID;
        Complaint complaint;
        public Complaint Complaint { get => complaint; set => SetProperty(ref complaint, value); }

        public ObservableRangeCollection<Division> divisionList { get; }
        public ObservableRangeCollection<Category> categoryList { get; }
        public ObservableRangeCollection<DamageType> damageTypeList { get; }

        int selectedDivisionIndex = -1;
        int selectedCategoryIndex = -1;
        int selectedDamageTypeIndex = -1;

        public int SelectedDivisionIndex { get => selectedDivisionIndex; set => SetProperty(ref selectedDivisionIndex, value); }
        public int SelectedCategoryIndex { get => selectedCategoryIndex; set => SetProperty(ref selectedCategoryIndex, value); }
        public int SelectedDamageTypeIndex { get => selectedDamageTypeIndex; set => SetProperty(ref selectedDamageTypeIndex, value); }

        Division selectedDivision;
        Category selectedCategory;
        DamageType selectedDamageType;

        public Division SelectedDivision { get => selectedDivision; set => SetProperty(ref selectedDivision, value); }
        public Category SelectedCategory { get => selectedCategory; set => SetProperty(ref selectedCategory, value); }
        public DamageType SelectedDamageType { get => selectedDamageType; set => SetProperty(ref selectedDamageType, value); }

        public AsyncCommand SaveCommand { get; }
        public AsyncCommand ClearCommand { get; }

        public StaffEditComplaintViewModel()
        {
            Title = "Edit complaint";
            SaveCommand = new AsyncCommand(Save);
            ClearCommand = new AsyncCommand(Clear);

            divisionList = new ObservableRangeCollection<Division>();
            categoryList = new ObservableRangeCollection<Category>();
            damageTypeList = new ObservableRangeCollection<DamageType>();

            selectedDivision = new Division();
            selectedCategory = new Category();
            selectedDamageType = new DamageType();
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

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            complaintID = int.Parse(HttpUtility.UrlDecode(query["complaintID"]));

            getComplaintDetail();
        }

        async void getComplaintDetail()
        {
            try
            {
                IsBusy = true;
                Complaint = await ComplaintServices.GetComplaintDetail(complaintID);

                List<Division> divisions = await DivisionServices.GetDivisions();
                List<Category> categories = await CategoryServices.GetCategories();
                List<DamageType> damageTypes = await DamageTypeServices.GetDamageTypes();

                divisionList.AddRange(divisions);
                categoryList.AddRange(categories);
                damageTypeList.AddRange(damageTypes);

                SelectedDivision = Complaint.Division;
                SelectedCategory = Complaint.Category;
                SelectedDamageType = Complaint.DamageType;

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
