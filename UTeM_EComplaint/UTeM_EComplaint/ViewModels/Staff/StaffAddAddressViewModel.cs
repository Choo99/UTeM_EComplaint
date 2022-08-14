using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
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
    internal class StaffAddAddressViewModel : ViewModelBase, IQueryAttributable
    {
        bool isCampus;
        bool isBuilding;
        bool isLevel;
        bool isDepartment;
        bool isRoom;

        bool isEdit;
        bool isPicture;
        bool isNotPicture = true;

        int selectedCampusIndex;
        int selectedBuildingIndex;
        int selectedLevelIndex;
        int selectedDepartmentIndex;
        int selectedRoomIndex;

        string longitude;
        string latitude;
        string position;
        string imageBase64;

        string campusName;
        string buildingName;
        string levelName;
        string departmentName;
        string roomName;

        public static string imageString;

        string pathToMap = $"{nameof(StaffMapPage)}";

        Campus selectedCampus;
        Building selectedBuilding;
        Level selectedLevel;
        Department selectedDepartment;
        Room selectedRoom;

        ImageSource image;
        Map map;

        public ObservableRangeCollection<Campus> CampusList { get; }
        public ObservableRangeCollection<Building> BuildingList { get; }
        public ObservableRangeCollection<Level> LevelList { get; }
        public ObservableRangeCollection<Department> DepartmentList { get; }
        public ObservableRangeCollection<Room> RoomList { get; }
        public ObservableRangeCollection<Position> PinList { get; }

        public Campus SelectedCampus { get => selectedCampus; set => SetProperty(ref selectedCampus, value); }
        public Building SelectedBuilding { get => selectedBuilding; set => SetProperty(ref selectedBuilding, value); }
        public Level SelectedLevel { get => selectedLevel; set => SetProperty(ref selectedLevel, value); }
        public Department SelectedDepartment { get => selectedDepartment; set => SetProperty(ref selectedDepartment, value); }
        public Room SelectedRoom { get => selectedRoom; set => SetProperty(ref selectedRoom, value); }

        public bool IsCampus { get => isCampus; set => SetProperty(ref isCampus, value); }
        public bool IsBuilding { get => isBuilding; set => SetProperty(ref isBuilding, value); }
        public bool IsLevel { get => isLevel; set => SetProperty(ref isLevel, value); }
        public bool IsDepartment { get => isDepartment; set => SetProperty(ref isDepartment, value); }
        public bool IsRoom { get => isRoom; set => SetProperty(ref isRoom, value); }
        public bool IsEdit { get => isEdit; set => SetProperty(ref isEdit, value); }
        public bool IsPicture { get => isPicture; set { IsNotPicture = !value; SetProperty(ref isPicture, value); } }
        public bool IsNotPicture { get => isNotPicture; set => SetProperty(ref isNotPicture, value); }

        public int SelectedCampusIndex { get => selectedCampusIndex; set => SetProperty(ref selectedCampusIndex, value); }
        public int SelectedBuildingIndex { get => selectedBuildingIndex; set => SetProperty(ref selectedBuildingIndex, value); }
        public int SelectedLevelIndex { get => selectedLevelIndex; set => SetProperty(ref selectedLevelIndex, value); }
        public int SelectedDepartmentIndex { get => selectedDepartmentIndex; set => SetProperty(ref selectedDepartmentIndex, value); }
        public int SelectedRoomIndex { get => selectedRoomIndex; set => SetProperty(ref selectedRoomIndex, value); }

        public string Longitude { get => longitude; set => SetProperty(ref longitude, value); }
        public string Latitude { get => latitude; set => SetProperty(ref latitude, value); }
        public string Position { get => position; set => SetProperty(ref position, value); }
        public string ImageBase64 { get => imageBase64; set => SetProperty(ref imageBase64, value); }
        public ImageSource Image { get => image; set => SetProperty(ref image, value); }
        public Map Map { get => map; set => SetProperty(ref map, value); }

        Campus campus;
        Building building;
        Level level;
        Department department;
        Room room;

        public Campus Campus 
        { 
            get => campus; 
            set
            {
                SetProperty(ref campus, value);
                if (isBuilding || isLevel || isDepartment || isRoom)
                {
                    Building = null;
                    Level = null;
                    Department = null;
                    Room = null;

                    IsBuilding = false;
                    IsLevel = false;
                    IsDepartment = false;
                    IsRoom = false;
                }
                IsCampus = true;

                if(value != null)
                    getBuilding(value);
                SetProperty(ref campus, value);
            }
        }

        public Building Building 
        { 
            get => building;
            set
            {
                if (isLevel || isDepartment || isRoom)
                {
                    Level = null;
                    Department = null;
                    Room = null;

                    IsLevel = false;
                    IsDepartment = false;
                    IsRoom = false;
                }
                IsBuilding = true;

                if (value != null)
                    getLevel(value);
                SetProperty(ref building, value);
            }
        }

        public Level Level 
        { 
            get => level;
            set
            {
                if (isDepartment || isRoom)
                {
                    Department = null;
                    Room = null;

                    IsDepartment = false;
                    IsRoom = false;
                }
                IsLevel = true;

                if (value != null)
                    getDepartment(value);
                SetProperty(ref level, value);
            }
        }

        public Department Department 
        { 
            get => department;
            set
            {
                if (isRoom)
                {
                    Room = null;
                    IsRoom = false;
                }
                IsDepartment = true;

                if (value != null)
                    getRoom(value);
                SetProperty(ref department, value);
            }
        }

        public Room Room {
            get => room;
            set
            {
                IsRoom = true;
                SetProperty(ref room, value);
            }
        }

        public AsyncCommand ConfirmCommand { get; }
        public AsyncCommand ClearCommand { get; }
        public AsyncCommand OpenMapCommand { get; }
        public AsyncCommand TakePictureCommand { get; }
        public AsyncCommand ClearPictureCommand { get; }

        public StaffAddAddressViewModel()
        {
            CampusList = new ObservableRangeCollection<Campus>();
            BuildingList = new ObservableRangeCollection<Building>();
            LevelList = new ObservableRangeCollection<Level>();
            DepartmentList = new ObservableRangeCollection<Department>();
            RoomList = new ObservableRangeCollection<Room>();
            PinList = new ObservableRangeCollection<Position>();

            ConfirmCommand = new AsyncCommand(Confirm);
            ClearCommand = new AsyncCommand(Clear);
            OpenMapCommand = new AsyncCommand(OpenMap);
            TakePictureCommand = new AsyncCommand(TakePicture);
            ClearPictureCommand = new AsyncCommand(ClearPicture);
            
            Map = new Map
            {
                IsEnabled = false,
            };
        }

        async void getCampus()
        {
            try
            {
                List<Campus> campuses = await LocationServices.GetCampus();
                CampusList.ReplaceRange(campuses);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
            

        }

        async void getBuilding(Campus campus)
        {
            try
            {
                List<Building> buildings = await LocationServices.GetBuilding(campus);
                BuildingList.ReplaceRange(buildings);

                //Auto fill in the field
                if(buildingName != null)
                    Building = BuildingList.FirstOrDefault(item => item.BuildingName.Contains(buildingName));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
        }

        async void getLevel(Building building)
        {
            try
            {
                List<Level> levels = await LocationServices.GetLevel(building);
                LevelList.ReplaceRange(levels);

                //Auto fill in the field
                if (levelName != null)
                    Level = LevelList.FirstOrDefault(item => item.LevelName.Contains(levelName));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
        }

        async void getDepartment(Level level)
        {
            try
            {
                List<Department> departments = await LocationServices.GetDepartment(level);
                DepartmentList.ReplaceRange(departments);

                //Auto fill in the field
                if (departmentName != null)
                    Department = DepartmentList.FirstOrDefault(item => item.DepartmentName.Contains(departmentName));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
        }

        async void getRoom(Department department)
        {
            try
            {
                List<Room> rooms = await LocationServices.GetRoom(department);
                RoomList.ReplaceRange(rooms);

                //Auto fill in the field
                if (roomName != null)
                    Room = RoomList.FirstOrDefault(item => item.RoomName.Contains(roomName));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
        }

        private async Task TakePicture()
        {
            var result = await MediaPicker.CapturePhotoAsync();
            
            if (result != null)
            {
                IsPicture = true;

                var stream = await result.OpenReadAsync();
               
                var bytes = new byte[stream.Length];
                await stream.ReadAsync(bytes, 0, (int)stream.Length);
                imageBase64 = Convert.ToBase64String(bytes);

                stream.Seek(0, SeekOrigin.Begin);

                Image = ImageSource.FromStream(() => stream);
            }
            await Task.Delay(100);
        }

        private async Task ClearPicture()
        {
            await Task.Delay(100);
            var ans = await Application.Current.MainPage.DisplayAlert("Picture", "Do you want to delete the picture?", "YES","NO");
            if(ans)
                IsPicture = false;
        }

        public async void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            try
            {
                if(Campus == null)
                {
                    List<Campus> campuses = await LocationServices.GetCampus();
                    CampusList.ReplaceRange(campuses);
                }
                
                if (query.ContainsKey("edit"))
                {
                    IsEdit = true;
                }
                else if (!IsEdit && !query.ContainsKey("longitude"))
                {
                    getLocation();
                }


                if (query.ContainsKey("longitude"))
                {
                    Longitude = HttpUtility.UrlDecode(query["longitude"]).Trim();
                    Latitude = HttpUtility.UrlDecode(query["latitude"]).Trim();
                    Position = Latitude + " , " + Longitude;

                    //move the map position
                    Map.MoveToRegion(MapHandler.moveToLocation(double.Parse(Latitude), double.Parse(Longitude)));

                    IsPicture = true;
                }

                if (query.ContainsKey("location"))
                {
                    //Split location into 5 parts
                    List<string> parts = HttpUtility.UrlDecode(query["location"]).Trim().Split(',').Select(p => p.Trim()).ToList();

                    campusName = parts[0];
                    buildingName = parts[1];
                    levelName = parts[2];
                    departmentName= parts[3];
                    roomName = parts[4];

                    Campus = CampusList.FirstOrDefault(item => item.CampusName.Contains(parts[0]));

                    if(imageString != null)
                        Image = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(imageString)));
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
        }

        private async Task OpenMap()
        {
            if(latitude == null && longitude == null)
                await Shell.Current.GoToAsync(pathToMap);
            else
                await Shell.Current.GoToAsync(string.Format("{0}?longitude={1}&latitude={2}", pathToMap,longitude,latitude));
        }

        public async Task Confirm()
        {
            if (!isCampus || !isBuilding || !isLevel || !isDepartment || !isRoom)
            {
                await Application.Current.MainPage.DisplayAlert("Address", "Please finish up the address!", "OK");
                return;
            }
                
            var answer = await Application.Current.MainPage.DisplayAlert("Address", "Are you confirm the address?", "YES", "NO");
            if (answer)
            {
                if(IsEdit)
                    StaffEditComplaintViewModel.base64String = imageBase64;
                else
                    StaffAddComplaintViewModel.imageString = imageBase64;

                await Shell.Current.GoToAsync(string.Format("../?campus={0}&building={1}&level={2}&department={3}&room={4}&longitude={5}&latitude={6}",
                    Campus.CampusName,Building.BuildingName,Level.LevelName,Department.DepartmentName,Room.RoomName,longitude,latitude));
            }
        }

        public async Task Clear()
        {
            await Task.Delay(100);
            IsCampus = false;
            IsBuilding = false;
            IsLevel = false;
            IsDepartment = false;
            IsRoom = false;

            BuildingList.Clear();
            LevelList.Clear();
            DepartmentList.Clear();
            RoomList.Clear();

            Campus = null;
            Building = null;
            Level = null;
            Department = null;
            Room = null;
        }

        public async void getLocation()
        {
            try
            {
                var location = await Geolocation.GetLocationAsync();

                if (location != null)
                {
                    Map.MoveToRegion(MapHandler.moveToLocation(location.Latitude, location.Longitude));
                    Longitude =location.Longitude.ToString();
                    Latitude = location.Latitude.ToString();

                    Position = Latitude + " , " + Longitude;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }
    }
}
