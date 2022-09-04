using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.IO;
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

        bool isHardware;
        bool isSoftware;
        public bool IsHardware { get => isHardware; set { SetProperty(ref isHardware, value); } }
        public bool IsSoftware { get => isSoftware; set { SetProperty(ref isSoftware, value);} }

        public bool IsLocation { get => isLocation; set { SetProperty(ref isLocation, value); IsNotLocation = !value; } }
        public bool IsNotLocation { get => isNotLocation; set => SetProperty(ref isNotLocation, value); }
        public Complaint Complaint { get => complaint; set => SetProperty(ref complaint, value); }
        public ImageSource Image { get => image; set => SetProperty(ref image, value); }
        public Map Map { get; private set; }

        public ObservableRangeCollection<Division> divisionList { get; }
        public ObservableRangeCollection<Category> categoryList { get; }
        public ObservableRangeCollection<DamageType> damageTypeList { get; }
        public ObservableRangeCollection<SoftwareSystem> SoftwareList { get; }
        public ObservableRangeCollection<Module> ModuleList { get; }
        public ObservableRangeCollection<Submodule> SubmoduleList { get; }
        public ObservableRangeCollection<Submenu> SubmenuList { get; }
        int selectedDivisionIndex = -1;
        int selectedCategoryIndex = -1;
        int selectedDamageTypeIndex = -1;

        int selectedSystemIndex = -1;
        int selectedModuleIndex = -1;
        int selectedSubmoduleIndex = -1;
        int selectedSubmenuIndex = -1;

        public int SelectedSystemIndex { get => selectedSystemIndex; set => SetProperty(ref selectedSystemIndex, value); }
        public int SelectedModuleIndex { get => selectedModuleIndex; set => SetProperty(ref selectedModuleIndex, value); }
        public int SelectedSubmoduleIndex { get => selectedSubmoduleIndex; set => SetProperty(ref selectedSubmoduleIndex, value); }
        public int SelectedsubmenuIndex { get => selectedSubmenuIndex; set => SetProperty(ref selectedSubmenuIndex, value); }

        public int SelectedDivisionIndex { get => selectedDivisionIndex; set => SetProperty(ref selectedDivisionIndex, value); }
        public int SelectedCategoryIndex { get => selectedCategoryIndex; set => SetProperty(ref selectedCategoryIndex, value); }
        public int SelectedDamageTypeIndex { get => selectedDamageTypeIndex; set => SetProperty(ref selectedDamageTypeIndex, value); }

        public string Location { get => location; set => SetProperty(ref location, value); }

        bool isPicture;
        public bool IsPicture { get => isPicture; set => SetProperty(ref isPicture, value); }

        Division selectedDivision;
        Category selectedCategory;
        DamageType selectedDamageType;

        SoftwareSystem selectedSystem;
        Module selectedModule;
        Submodule selectedSubmodule;
        Submenu selectedSubmenu;

        public SoftwareSystem SelectedSystem 
        {
            get => selectedSystem;
            set
            {
                SetProperty(ref selectedSystem, value);
                if (value != null)
                    getModule();
            }
        }
        public Module SelectedModule 
        { 
            get => selectedModule;
            set
            {
                SetProperty(ref selectedModule, value);
                if (value != null)
                    getSubmodule();
            }
        }
        public Submodule SelectedSubmodule
        { 
            get => selectedSubmodule;
            set
            {
                SetProperty(ref selectedSubmodule, value);
                if (value != null)
                    getSubmenu();
            }
        }
        public Submenu SelectedSubmenu { get => selectedSubmenu; set => SetProperty(ref selectedSubmenu, value); }

        public Division SelectedDivision { get => selectedDivision; set => SetProperty(ref selectedDivision, value); }
        public Category SelectedCategory { get => selectedCategory; set => SetProperty(ref selectedCategory, value); }
        public DamageType SelectedDamageType { get => selectedDamageType; set => SetProperty(ref selectedDamageType, value); }

        public AsyncCommand SaveCommand { get; }
        public AsyncCommand ClearCommand { get; }

        public AsyncCommand AddLocationCommand { get; }
        public AsyncCommand ClearLocationCommand { get; }
        public AsyncCommand ClearPictureCommand { get; }
        public AsyncCommand TakePictureCommand { get; }

        public StaffEditComplaintViewModel()
        {
            Title = "Edit complaint";
            SaveCommand = new AsyncCommand(Save);
            ClearCommand = new AsyncCommand(Clear);
            AddLocationCommand = new AsyncCommand(AddLocation);
            ClearLocationCommand = new AsyncCommand(ClearLocation);
            ClearPictureCommand = new AsyncCommand(ClearPicture);
            TakePictureCommand = new AsyncCommand(TakePicture);

            IsLocation = true;

            divisionList = new ObservableRangeCollection<Division>();
            categoryList = new ObservableRangeCollection<Category>();
            damageTypeList = new ObservableRangeCollection<DamageType>();

            SoftwareList = new ObservableRangeCollection<SoftwareSystem>();
            ModuleList = new ObservableRangeCollection<Module>();
            SubmoduleList = new ObservableRangeCollection<Submodule>();
            SubmenuList = new ObservableRangeCollection<Submenu>();

            selectedDivision = new Division();
            selectedCategory = new Category();
            selectedDamageType = new DamageType();

            Map = new Map();
            Map.IsEnabled = false;
        }

        private async Task ClearLocation()
        {
            await Task.Delay(100);
            Image = null;
            IsPicture = false;
        }
        private async Task ClearPicture()
        {
            await Task.Delay(100);
            Image = null;
            IsPicture = false;
        }

        private async Task TakePicture()
        {
            try
            {
                var result = await MediaPicker.CapturePhotoAsync();

                if (result != null)
                {
                    IsPicture = true;

                    var stream = await result.OpenReadAsync();

                    var bytes = new byte[stream.Length];
                    await stream.ReadAsync(bytes, 0, (int)stream.Length);
                    base64String = Convert.ToBase64String(bytes);

                    stream.Seek(0, SeekOrigin.Begin);

                    Image = ImageHandler.LoadBase64(base64String);
                }
                await Task.Delay(100);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
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
                    int result = 0;
                    if (isHardware)
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
                        result = await ComplaintServices.StaffEditComplaint(Complaint);
                    }
                    else
                    {
                        Complaint.SoftwareSystem = selectedSystem;
                        Complaint.Module = selectedModule;
                        Complaint.Submodule = selectedSubmodule;
                        Complaint.ImageBase64 = base64String;
                        if(selectedSubmenu == null)
                        {
                            selectedSubmenu = new Submenu();
                        }
                        Complaint.Submenu = selectedSubmenu;
                        complaint.Staff = new Staff
                        {
                            StaffID = Preferences.Get("userID", 0),
                        };
                        result = await ComplaintServices.StaffEditSoftwareComplaint(Complaint);
                    }
                    
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

        private async Task<bool> validation()
        {
            try
            {
                bool isValid = false;
                if (Complaint.Damage == null || Complaint.ContactPhoneNumber == null)
                {
                    if (Complaint.Damage == null)
                        await Application.Current.MainPage.DisplayAlert("Fill", "Please fill in the description", "OK");
                    else if (Complaint.ContactPhoneNumber == null)
                        await Application.Current.MainPage.DisplayAlert("Fill", "Please fill in the contact phone number", "OK");
                }
                else if (isHardware && (selectedCategory == null || selectedDamageType == null || selectedDivision == null || Location == null))
                {
                    if (selectedDivision == null)
                        await Application.Current.MainPage.DisplayAlert("Fill", "Please choose a division", "OK");
                    else if (selectedDamageType == null)
                        await Application.Current.MainPage.DisplayAlert("Fill", "Please choose a damage type", "OK");
                    else if (selectedCategory == null)
                        await Application.Current.MainPage.DisplayAlert("Fill", "Please choose a category", "OK");
                    else if (Location == null)
                        await Application.Current.MainPage.DisplayAlert("Fill", "Please fill in the location", "OK");
                }
                else if (isSoftware && (selectedSystem == null || selectedModule == null || selectedSubmodule == null))
                {
                    if (selectedSystem == null)
                        await Application.Current.MainPage.DisplayAlert("Fill", "Please choose a system", "OK");
                    else if (selectedModule == null)
                        await Application.Current.MainPage.DisplayAlert("Fill", "Please choose a module", "OK");
                    else if (selectedSubmodule == null)
                        await Application.Current.MainPage.DisplayAlert("Fill", "Please choose a submodule", "OK");
                }
                else
                    isValid = true;
                return isValid;
            }
            catch (Exception)
            {
                throw;
            }
        }
        async void getComplaintDetail()
        {
            try
            {
                IsBusy = true;
                Complaint = await ComplaintServices.GetComplaintAndComplaintDetail(complaintID);
                if(Complaint.ComplaintType.ComplaintTypeCode == "S")
                {
                    IsSoftware = true;
                }
                else
                {
                    IsHardware = true;
                }
                //Initiate value of location
                Image = ImageHandler.LoadBase64(Complaint.ImageBase64);
                IsPicture = true;

                Map.MoveToRegion(MapHandler.moveToLocation(Complaint.Latitude, Complaint.Longitude));

                if (isHardware)
                {
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

                    SelectedDivisionIndex = divisions.FindIndex(item => item.DivisionID == SelectedDivision.DivisionID);
                    SelectedDamageTypeIndex = damageTypes.FindIndex(item => item.DamageTypeID == SelectedDamageType.DamageTypeID);
                    SelectedCategoryIndex = categories.FindIndex(item => item.CategoryId == SelectedCategory.CategoryId);
                }
                else
                {
                    List<SoftwareSystem> systems = await SoftwareSystemServices.GetSoftwareSystems();
                    SoftwareList.ReplaceRange(systems);

                    SelectedSystemIndex = systems.FindIndex(item => item.SystemCode == Complaint.SoftwareSystem.SystemCode);
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
        async void getModule()
        {
            try
            {
                IsBusy = true;

                List<Module> modules = await ModuleServices.GetAvailableModuleBySystem(new Module
                {
                    System = selectedSystem
                });

                ModuleList.ReplaceRange(modules);
                SelectedModuleIndex = modules.FindIndex(item => item.ModuleCode == Complaint.Module.ModuleCode);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
            finally { IsBusy = false; }
        }

        async void getSubmodule()
        {
            try
            {
                IsBusy = true;

                List<Submodule> submodules = await SubmoduleServices.GetAvailableSubmoduleByModule(new Submodule
                {
                    Module = selectedModule
                });

                SubmoduleList.ReplaceRange(submodules);
                SelectedSubmoduleIndex = submodules.FindIndex(item => item.SubmoduleCode == Complaint.Submodule.SubmoduleCode);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
            finally { IsBusy = false; }
        }

        async void getSubmenu()
        {
            try
            {
                IsBusy = true;

                List<Submenu> submenus = await SubmenuServices.GetAvailableSubmenusBySubmodule(new Submenu
                {
                    Submodule = selectedSubmodule
                });

                SubmenuList.ReplaceRange(submenus);
                if(SelectedSubmenu != null)
                    SelectedsubmenuIndex = submenus.FindIndex(item => item.SubmenuCode == Complaint.Submenu.SubmenuCode);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
            finally { IsBusy = false; }
        }
    }
}
