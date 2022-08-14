using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    internal class MasterLocationViewModel : ViewModelBase
    {
        readonly int LOAD_SIZE = 8;
        readonly string PATH_TO_ADD_LOCATION = $"{nameof(MasterAddLocationPage)}";

        bool isSelected;
        bool isAdd;
        bool isCampus;
        bool isBuilding;
        bool isLevel;
        bool isDepartment;
        bool isRoom;

        
        Room previousSelectedRoom;
        Room newRoom;

        Campus selectedCampus;
        Building selectedBuilding;
        Level selectedLevel;
        Department selectedDepartment;
        Room selectedRoom;

        public bool IsSelected { get => isSelected; set => SetProperty(ref isSelected, value); }
        public bool IsAdd { get => isAdd; set => SetProperty(ref isAdd, value); }
        public bool IsCampus { get => isCampus; set => SetProperty(ref isCampus, value); }
        public bool IsBuilding { get => isBuilding; set => SetProperty(ref isBuilding, value); }
        public bool IsLevel { get => isLevel; set => SetProperty(ref isLevel, value); }
        public bool IsDepartment { get => isDepartment; set => SetProperty(ref isDepartment, value); }
        public bool IsRoom { get => isRoom; set => SetProperty(ref isRoom, value); }

        public bool IsExpanded { get; set; }
        public bool IsCampusNew { get; set; }
        public bool IsBuildingNew { get; set; }
        public bool IsLevelNew { get; set; }
        public bool IsDepartmentNew { get; set; }
        public bool IsRoomNew { get; set; }

        List<Room> rooms;


        public ObservableRangeCollection<Room> RoomList { get; }
        public Room PreviousSelectedRoom
        {
            get => previousSelectedRoom;
            set
            {
                SetProperty(ref previousSelectedRoom, value);
                OnPropertyChanged();
            }
        }
        public Room NewRoom
        {
            get => newRoom;
            set
            {
                SetProperty(ref newRoom, value);
                OnPropertyChanged();
            }
        }

        public Campus SelectedCampus
        {
            get => selectedCampus;
            set
            {
                SetProperty(ref selectedCampus, value);
                OnPropertyChanged();
            }
        }
        public Building SelectedBuilding
        {
            get => selectedBuilding;
            set
            {
                SetProperty(ref selectedBuilding, value);
                OnPropertyChanged();
            }
        }
        public Level SelectedLevel
        {
            get => selectedLevel;
            set
            {
                SetProperty(ref selectedLevel, value);
                OnPropertyChanged();
            }
        }
        public Department SelectedDepartment
        {
            get => selectedDepartment;
            set
            {
                SetProperty(ref selectedDepartment, value);
                OnPropertyChanged();
            }
        }
        public Room SelectedRoom
        {
            get => selectedRoom;
            set
            {
                SetProperty(ref selectedRoom, value);
                OnPropertyChanged();
            }
        }

        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand<object> ItemSelectedCommand { get; }
        public AsyncCommand LoadMoreCommand { get; }
        public AsyncCommand SaveCommand { get; }
        public AsyncCommand AddButtonCommand { get; }
        public AsyncCommand AddCommand { get; }
        public AsyncCommand DeleteCommand { get; }

        public AsyncCommand SaveCampusCommand { get; }
        public AsyncCommand SaveBuildingCommand { get; }
        public AsyncCommand SaveLevelCommand { get; }
        public AsyncCommand SaveDepartmentCommand { get; }
        public AsyncCommand SaveRoomCommand { get; }

        public AsyncCommand<object> CampusCommand { get; }
        public AsyncCommand<object> BuildingCommand { get; }
        public AsyncCommand<object> LevelCommand { get; }
        public AsyncCommand<object> DepartmentCommand { get; }
        public AsyncCommand<object> RoomCommand { get; }

        public MasterLocationViewModel()
        {
            Title = "Category complaint";

            RoomList = new ObservableRangeCollection<Room>();
            rooms = new List<Room>();

            ItemSelectedCommand = new AsyncCommand<object>(ItemSelected);
            RefreshCommand = new AsyncCommand(Refresh);
            LoadMoreCommand = new AsyncCommand(LoadMore);
            AddButtonCommand = new AsyncCommand(AddButton);

            CampusCommand = new AsyncCommand<object>(CampusPopup);
            BuildingCommand = new AsyncCommand<object>(BuildingPopup);
            LevelCommand = new AsyncCommand<object>(LevelPopup);
            DepartmentCommand = new AsyncCommand<object>(DepartmentPopup);
            RoomCommand = new AsyncCommand<object>(RoomPopup);

            SaveCampusCommand = new AsyncCommand(SaveCampus);
            SaveBuildingCommand = new AsyncCommand(SaveBuilding);
            SaveLevelCommand = new AsyncCommand(SaveLevel);
            SaveDepartmentCommand = new AsyncCommand(SaveDepartment);
            SaveRoomCommand = new AsyncCommand(SaveRoom);

            getData();
        }

        private async Task SaveCampus()
        {
            try
            {
                var answer = await Application.Current.MainPage.DisplayAlert("Save", "Are you sure you want to save the information?", "YES", "NO");
                if (answer)
                {
                    IsBusy = true;
                    if (answer)
                    {
                        IsBusy = true;
                        int result = await LocationServices.EditCampus(selectedCampus);

                        if (result != 0)
                        {
                            await Application.Current.MainPage.DisplayAlert("Success", "Your information has been saved", "OK");
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Error", "Fail to edit", "OK");
                        }
                        IsCampus = false;
                        getData();
                    }
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

        private async Task SaveBuilding()
        {
            try
            {
                var answer = await Application.Current.MainPage.DisplayAlert("Save", "Are you sure you want to save the information?", "YES", "NO");
                if (answer)
                {
                    IsBusy = true;
                    if (answer)
                    {
                        IsBusy = true;
                        int result = await LocationServices.EditBuilding(selectedBuilding);

                        if (result != 0)
                        {
                            await Application.Current.MainPage.DisplayAlert("Success", "Your information has been saved", "OK");
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Error", "Fail to edit", "OK");
                        }
                        IsBuilding = false;
                        getData();
                    }
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

        private async Task SaveLevel()
        {
            try
            {
                var answer = await Application.Current.MainPage.DisplayAlert("Save", "Are you sure you want to save the information?", "YES", "NO");
                if (answer)
                {
                    IsBusy = true;
                    if (answer)
                    {
                        IsBusy = true;
                        int result = await LocationServices.EditLevel(selectedLevel);

                        if (result != 0)
                        {
                            await Application.Current.MainPage.DisplayAlert("Success", "Your information has been saved", "OK");
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Error", "Fail to edit", "OK");
                        }
                        IsLevel = false;
                        getData();
                    }
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

        private async Task SaveDepartment()
        {
            try
            {
                var answer = await Application.Current.MainPage.DisplayAlert("Save", "Are you sure you want to save the information?", "YES", "NO");
                if (answer)
                {
                    IsBusy = true;
                    if (answer)
                    {
                        IsBusy = true;
                        int result = await LocationServices.EditDepartment(selectedDepartment);

                        if (result != 0)
                        {
                            await Application.Current.MainPage.DisplayAlert("Success", "Your information has been saved", "OK");
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Error", "Fail to edit", "OK");
                        }
                        IsDepartment = false;
                        getData();
                    }
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

        private async Task SaveRoom()
        {
            try
            {
                var answer = await Application.Current.MainPage.DisplayAlert("Save", "Are you sure you want to save the information?", "YES", "NO");
                if (answer)
                {
                    IsBusy = true;
                    if (answer)
                    {
                        IsBusy = true;
                        int result = await LocationServices.EditRoom(selectedRoom);

                        if (result != 0)
                        {
                            await Application.Current.MainPage.DisplayAlert("Success", "Your information has been saved", "OK");
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Error", "Fail to edit", "OK");
                        }
                        IsRoom = false;
                        getData();
                    }
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

        private async Task CampusPopup(object arg)
        {
            Campus campus = arg as Campus;
            selectedCampus = campus;   

            IsCampus = true;
        }

        private async Task BuildingPopup(object arg)
        {
            Building building = arg as Building;
            selectedBuilding = building;

            IsBuilding = true;
        }

        private async Task LevelPopup(object arg)
        {
            Level level = arg as Level;
            selectedLevel = level;

            IsLevel = true;
        }

        private async Task DepartmentPopup(object arg)
        {
            Department department = arg as Department;
            selectedDepartment = department;

            IsDepartment = true;
        }

        private async Task RoomPopup(object arg)
        {
            Room room = arg as Room;
            selectedRoom = room;

            IsRoom = true;
        }

        private async Task AddButton()
        {
            await Shell.Current.GoToAsync(PATH_TO_ADD_LOCATION);
        }

        private async Task LoadMore()
        {
            if (RoomList.Count == rooms.Count)
                return;

            int lastItemIndexed = RoomList.Count;
            int nextItemIndexed = lastItemIndexed + LOAD_SIZE;

            if (nextItemIndexed > rooms.Count)
                nextItemIndexed = rooms.Count;

            for (int i = lastItemIndexed; i < nextItemIndexed; i++)
            {
                RoomList.Add(rooms[i]);
            }
        }

        async void getData()
        {
            try
            {
                int size = LOAD_SIZE;
                IsBusy = true;
                rooms = await LocationServices.GetLocations();
                if (rooms.Count < LOAD_SIZE)
                    size = rooms.Count;
                RoomList.ReplaceRange(rooms.GetRange(0, size));
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
        private async Task ItemSelected(object arg)
        {
            var room = arg as Room;
            if (room == null)
                return;

            PreviousSelectedRoom = room;

            IsSelected = true;

            SelectedRoom = null;
        }

        async Task Refresh()
        {
            IsBusy = true;
            getData();
            IsBusy = false;
        }
    }
}
