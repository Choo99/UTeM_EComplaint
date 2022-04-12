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
    internal class StaffAddComplaintViewModel : ViewModelBase
    {
        string dateTime;
        bool isLoading;

        public ObservableRangeCollection<Division> divisionList {get;}
        public ObservableRangeCollection<Category> categoryList {get;}
        public ObservableRangeCollection<DamageType> damageTypeList {get;}

        Division selectedDivision;
        Category selectedCategory;
        DamageType selectedDamageType;
        Complaint complaint;
        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }
        public Complaint Complaint { get => complaint; set => SetProperty(ref complaint, value); }
        public Division SelectedDivision { get => selectedDivision; set => SetProperty(ref selectedDivision, value); }
        public Category SelectedCategory { get => selectedCategory; set => SetProperty(ref selectedCategory, value); }
        public DamageType SelectedDamageType { get => selectedDamageType; set => SetProperty(ref selectedDamageType, value); }
        public string DateTimeString { get => dateTime; set => SetProperty(ref dateTime, value); }

        public AsyncCommand SaveCommand { get; }

        public StaffAddComplaintViewModel()
        {
            Title = "New Complaint";
            DateTimeString = DateTime.Now.ToString("dd/MM/yyyy");
            selectedDivision = new Division();
            selectedCategory = new Category();
            selectedDamageType = new DamageType();

            complaint = new Complaint();
            divisionList = new ObservableRangeCollection<Division>();
            categoryList = new ObservableRangeCollection<Category>();
            damageTypeList = new ObservableRangeCollection<DamageType>();

            SaveCommand = new AsyncCommand(Save);

            getData();
        }

        private async Task Save()
        {
            try
            {
                IsLoading = true;
                var answer = await Application.Current.MainPage.DisplayAlert("Add", "Are you confirm your information?", "YES", "NO");
                if (answer)
                {
                    Complaint.Division = selectedDivision;
                    complaint.Category = selectedCategory;
                    complaint.DamageType = selectedDamageType;
                    complaint.Staff = new Staff
                    {
                        StaffID = Preferences.Get("userID", 0),
                    };
                    string notificationToken = DependencyService.Get<INotificationHelper>().GetToken();
                    int result = await ComplaintServices.AddComplaintAndSubscribe(Complaint, notificationToken);

                    if (result != 0)
                    {
                        Complaint = null;
                        SelectedCategory = null;
                        SelectedDamageType = null;
                        SelectedDivision = null;
                        await Application.Current.MainPage.DisplayAlert("Success", "Successfully added your complaint! We will process your complaint as soon as possible", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
            finally
            {
                IsLoading = false;
            }
            
        }

        async void getData()
        {
            try
            {
                IsLoading = true;
                List<Division> divisions = await DivisionServices.GetDivisions();
                List<Category> categories = await CategoryServices.GetCategories();
                List<DamageType> damageTypes = await DamageTypeServices.GetDamageTypes();

                divisionList.AddRange(divisions);
                categoryList.AddRange(categories);
                damageTypeList.AddRange(damageTypes);
            }catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
            finally { IsLoading = false; }
            
        }
    }
}
