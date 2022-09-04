using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MvvmHelpers;
using MvvmHelpers.Commands;
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
    internal class StaffAddComplaintViewModel : ViewModelBase, IQueryAttributable
    {
        readonly string pathToAddAddress = $"{nameof(StaffAddAddressPage)}";

        public static string imageString;

        string dateTime;
        string location;
        string longitude;
        string latitude;
        bool isLoading;
        bool isLocation;
        bool isPicture;
        bool isNotLocation;
        bool isSoftware;
        bool isHardware;
        bool isComplaintTypeSelected;

        public ObservableRangeCollection<ComplaintType> ComplaintTypeList { get;}
        public ObservableRangeCollection<SoftwareSystem> SoftwreSystemList {get;}
        public ObservableRangeCollection<Module> ModuleList {get;}
        public ObservableRangeCollection<Submodule> SubmoduleList {get;}
        public ObservableRangeCollection<Submenu> SubmenuList {get;}
        public ObservableRangeCollection<Division> divisionList {get;}
        public ObservableRangeCollection<Category> categoryList {get;}
        public ObservableRangeCollection<DamageType> damageTypeList {get;}

        public ObservableRangeCollection<string> CampusList {get;}
        public ObservableRangeCollection<string> BuildingList {get;}
        public ObservableRangeCollection<string> LevelList {get;}
        public ObservableRangeCollection<string> DepartmentList {get;}
        public ObservableRangeCollection<string> RoomList {get;}


        ComplaintType selectedComplaintType;

        SoftwareSystem selectedSoftwareSystem;
        Module selectedModule;
        Submodule selectedSubmodule;
        Submenu selectedSubmenu;

        Division selectedDivision;
        Category selectedCategory;
        DamageType selectedDamageType;
        Complaint complaint;

        ImageSource image;
        ImageSource softwareImage;
        Map map;

        public Map Map { get => map; set => SetProperty(ref map, value); }

        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }
        public bool IsLocation { get => isLocation; set { SetProperty(ref isLocation, value); IsNotLocation = !value; } }
        public bool IsNotLocation { get => isNotLocation; set => SetProperty(ref isNotLocation, value); }
        public bool IsSoftware { get => isSoftware; set => SetProperty(ref isSoftware, value); }
        public bool IsHardware { get => isHardware; set => SetProperty(ref isHardware, value); }
        public bool IsComplaintTypeSelected { get => isComplaintTypeSelected; set => SetProperty(ref isComplaintTypeSelected, value); }
        public bool IsPicture { get => isPicture; set => SetProperty(ref isPicture, value); }

        public Complaint Complaint { get => complaint; set => SetProperty(ref complaint, value); }

        public ComplaintType SelectedComplaintType 
        { 
            get => selectedComplaintType;
            set
            {
                SetProperty(ref selectedComplaintType, value);
                if(value != null)
                    getSoftwareOrHardware();
            }
        }
        public SoftwareSystem SelectedSoftwareSystem 
        {
            get => selectedSoftwareSystem;
            set
            {
                SetProperty(ref selectedSoftwareSystem, value);
                if(value != null)
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

        public string DateTimeString { get => dateTime; set => SetProperty(ref dateTime, value); }
        public string Location { get => location; set => SetProperty(ref location, value); }
        public ImageSource Image { get => image; set => SetProperty(ref image, value); }
        public ImageSource SoftwareImage { get => softwareImage; set => SetProperty(ref softwareImage, value); }

        public AsyncCommand AddLocationCommand { get; }
        public AsyncCommand EditLocationCommand { get; }
        public AsyncCommand ClearLocationCommand { get; }
        public AsyncCommand TakePictureCommand { get; }
        public AsyncCommand ClearPictureCommand { get; }
        public AsyncCommand SaveCommand { get; }

        public StaffAddComplaintViewModel()
        {
            Title = "New Complaint";
            DateTimeString = DateTime.Now.ToString("dd/MM/yyyy");
            selectedDivision = new Division();
            complaint = new Complaint();

            ComplaintTypeList = new ObservableRangeCollection<ComplaintType>();

            SoftwreSystemList = new ObservableRangeCollection<SoftwareSystem>();
            ModuleList = new ObservableRangeCollection<Module>();
            SubmoduleList = new ObservableRangeCollection<Submodule>();
            SubmenuList = new ObservableRangeCollection<Submenu>();

            divisionList = new ObservableRangeCollection<Division>();
            categoryList = new ObservableRangeCollection<Category>();
            damageTypeList = new ObservableRangeCollection<DamageType>();

            IsNotLocation = true;

            SaveCommand = new AsyncCommand(Save);
            AddLocationCommand = new AsyncCommand(AddLocation);
            EditLocationCommand = new AsyncCommand(EditLocation);
            TakePictureCommand = new AsyncCommand(TakePicture);
            ClearPictureCommand = new AsyncCommand(ClearPicture);
            ClearLocationCommand = new AsyncCommand(ClearLocation);

            map = new Map()
            {
                IsEnabled = false,
            };

            getData();
        }

        private async Task ClearPicture()
        {
            await Task.Delay(100);
            var ans = await Application.Current.MainPage.DisplayAlert("Picture", "Do you want to delete the picture?", "YES", "NO");
            if (ans)
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
                    imageString = Convert.ToBase64String(bytes);

                    stream.Seek(0, SeekOrigin.Begin);

                    SoftwareImage = ImageSource.FromStream(() => stream);
                }
                await Task.Delay(100);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
        }

        public async void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            try
            {
                if (query.ContainsKey("campus"))
                {
                    string campus = HttpUtility.UrlDecode(query["campus"]).Trim();
                    string building = HttpUtility.UrlDecode(query["building"]).Trim();
                    string level = HttpUtility.UrlDecode(query["level"]).Trim();
                    string department = HttpUtility.UrlDecode(query["department"]).Trim();
                    string room = HttpUtility.UrlDecode(query["room"]).Trim();

                    IsLocation = true;
                    Location = campus + ", " + building + ", " + level + ", " + department + ", " + room;

                    latitude = HttpUtility.UrlDecode(query["latitude"]).Trim();
                    longitude = HttpUtility.UrlDecode(query["longitude"]).Trim();

                    //move the map position
                    Map.MoveToRegion(MapHandler.moveToLocation(Double.Parse(latitude), Double.Parse(longitude)));

                    Image = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(imageString)));
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
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
            await Shell.Current.GoToAsync(pathToAddAddress);
        }

        private async Task EditLocation()
        {
            StaffAddAddressViewModel.imageString = imageString;
            await Shell.Current.GoToAsync(String.Format("{0}?location={1}&longitude={2}&latitude={3}", pathToAddAddress, location, longitude, latitude));
        }

        private async Task Save()
        {
            try
            {
                if (!await validation())
                    return;
                IsLoading = true;
                var answer = await Application.Current.MainPage.DisplayAlert("Add", "Are you confirm your information?", "YES", "NO");
                if (answer)
                {
                    complaint.ComplaintType = SelectedComplaintType;
                    if (selectedComplaintType.ComplaintTypeCode == "H")
                    {
                        complaint.Division = selectedDivision;
                        complaint.Category = selectedCategory;
                        complaint.DamageType = selectedDamageType;
                        complaint.Location = Location;
                        complaint.Longitude = Double.Parse(longitude);
                        complaint.Latitude = Double.Parse(latitude);
                        complaint.ImageBase64 = imageString;
                    }
                    else
                    {
                        complaint.SoftwareSystem = selectedSoftwareSystem;
                        complaint.Module = selectedModule;
                        complaint.Submodule = selectedSubmodule;
                        complaint.Submenu = selectedSubmenu;
                        complaint.ImageBase64 = imageString;
                    }

                    complaint.Staff = new Staff
                    {
                        StaffID = Preferences.Get("userID", 0),
                    };
                    //string notificationToken = DependencyService.Get<INotificationHelper>().GetToken();
                    string result = await ComplaintServices.AddComplaint(Complaint);

                    Complaint = new Complaint();
                    SelectedCategory = null;
                    SelectedDamageType = null;
                    SelectedDivision = null;
                    SelectedComplaintType = null;
                    SelectedSoftwareSystem = null;
                    SelectedModule = null;
                    SelectedSubmodule = null;
                    SelectedSubmenu = null;
                    IsHardware = false;
                    IsSoftware = false;
                    IsComplaintTypeSelected = false;
                    SoftwareImage = null;
                    IsPicture = false;
                    await Application.Current.MainPage.DisplayAlert("Success", "Successfully added your complaint! Your complaint ID is " + result, null, "OK");
                    await Shell.Current.GoToAsync($"//StaffTab/{nameof(StaffComplaintDetailPage)}?complaintID=" + result);

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

        private async Task<bool> validation()
        {
            try
            {
                bool isValid = false;
                if (SelectedComplaintType == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Fill", "Please choose a complaint type", "OK");
                }
                else if(Complaint.Damage == null || Complaint.ContactPhoneNumber == null)
                {
                    if (Complaint.Damage == null)
                        await Application.Current.MainPage.DisplayAlert("Fill", "Please fill in the description", "OK");
                    else if(Complaint.ContactPhoneNumber == null)
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
                else if (isSoftware && (selectedSoftwareSystem == null || selectedModule == null || selectedSubmodule == null))
                {
                    if (selectedSoftwareSystem == null)
                        await Application.Current.MainPage.DisplayAlert("Fill", "Please choose a system", "OK");
                    else if (selectedModule == null)
                        await Application.Current.MainPage.DisplayAlert("Fill", "Please choose a module", "OK");
                    else if(selectedSubmodule == null)
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

        async void getData()
        {
            try
            {
                IsLoading = true;
                List<ComplaintType> complaintTypes = await ComplaintTypeServices.GetComplaintType();

                ComplaintTypeList.AddRange(complaintTypes);
            }catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
            finally { IsLoading = false; }
        }

        async void getSoftwareOrHardware()
        {
            try
            {
                IsLoading = true;
                if(SelectedComplaintType.ComplaintTypeCode == "S")
                {
                    IsSoftware = true;
                    IsHardware = false;
                    IsComplaintTypeSelected = true;
                    List<SoftwareSystem> softwareSystems = await SoftwareSystemServices.GetSoftwareSystems();

                    SoftwreSystemList.ReplaceRange(softwareSystems);
                }
                else
                {
                    IsHardware = true;
                    IsSoftware = false;
                    IsComplaintTypeSelected = true;
                    List<Division> divisions = await DivisionServices.GetDivisions();
                    List<Category> categories = await CategoryServices.GetCategories();
                    List<DamageType> damageTypes = await DamageTypeServices.GetDamageTypes();

                    divisionList.ReplaceRange(divisions);
                    categoryList.ReplaceRange(categories);
                    damageTypeList.ReplaceRange(damageTypes);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
            finally { IsLoading = false; }
        }

        async void getModule()
        {
            try
            {
                IsLoading = true;

                List<Module> modules = await ModuleServices.GetAvailableModuleBySystem(new Module
                {
                    System = selectedSoftwareSystem
                });

                ModuleList.ReplaceRange(modules);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
            finally { IsLoading = false; }
        }

        async void getSubmodule()
        {
            try
            {
                IsLoading = true;

                List<Submodule> submodules = await SubmoduleServices.GetAvailableSubmoduleByModule(new Submodule
                {
                    Module = selectedModule
                });

                SubmoduleList.ReplaceRange(submodules);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
            finally { IsLoading = false; }
        }

        async void getSubmenu()
        {
            try
            {
                IsLoading = true;

                List<Submenu> submenus = await SubmenuServices.GetAvailableSubmenusBySubmodule(new Submenu
                {
                    Submodule = selectedSubmodule
                });

                SubmenuList.ReplaceRange(submenus);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
            finally { IsLoading = false; }
        }
    }
}
