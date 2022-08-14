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
    internal class MasterAddLocationViewModel : ViewModelBase
    {
        Campus selectedCampus;
        Building selectedBuilding;
        Level selectedLevel;
        Department selectedDepartment;
        Room selectedRoom;

        Campus campus;
        Building building;
        Level level;
        Department department;
        Room room;

        bool isCampus;
        bool isBuilding;
        bool isLevel;
        bool isDepartment;
        bool isRoom;

        public bool isCampusNew;
        public bool isBuildingNew;
        public bool isLevelNew;
        public bool isDepartmentNew;
        public bool isRoomNew;

        string newCampusName;
        string newBuildingName;
        string newLevelName;
        string newDepartmentName;
        string newRoomName;

        public bool IsCampus { get => isCampus; set => SetProperty(ref isCampus, value); }
        public bool IsBuilding { get => isBuilding; set => SetProperty(ref isBuilding, value); }
        public bool IsLevel { get => isLevel; set => SetProperty(ref isLevel, value); }
        public bool IsDepartment { get => isDepartment; set => SetProperty(ref isDepartment, value); }
        public bool IsRoom { get => isRoom; set => SetProperty(ref isRoom, value); }

        public bool IsCampusNew 
        {
            get => isCampusNew;
            set
            {
                SetProperty(ref isCampusNew, value);
                IsBuildingNew = value;
                IsLevelNew = value;
                IsDepartmentNew = value;

                if (isCampusNew)
                    Campus = null;
                else
                    NewCampusName = null;
            }
        }
        public bool IsBuildingNew
        {
            get => isBuildingNew;
            set
            {
                SetProperty(ref isBuildingNew, value);
                IsLevelNew = value;
                IsDepartmentNew = value;

                if (isBuildingNew)
                    Building = null;
                else
                    NewBuildingName = null;
            }
        }
        public bool IsLevelNew
        {
            get => isLevelNew;
            set
            {
                SetProperty(ref isLevelNew, value);
                IsDepartmentNew = value;

                if (isLevelNew)
                    Level = null;
                else
                    NewLevelName = null;
            }
        }
        public bool IsDepartmentNew
        {
            get => isDepartmentNew;
            set
            {
                SetProperty(ref isDepartmentNew, value);

                if (isDepartmentNew)
                    Department = null;
                else
                    NewDepartmentName = null;
            }
        }

        public string NewCampusName { get => newCampusName; set => SetProperty(ref newCampusName, value); }
        public string NewBuildingName { get => newBuildingName; set => SetProperty(ref newBuildingName, value); }
        public string NewLevelName { get => newLevelName; set => SetProperty(ref newLevelName, value); }
        public string NewDepartmentName { get => newDepartmentName; set => SetProperty(ref newDepartmentName, value); }
        public string NewRoomName { get => newRoomName; set => SetProperty(ref newRoomName, value); }

        public ObservableRangeCollection<Campus> CampusList { get; }
        public ObservableRangeCollection<Building> BuildingList { get; }
        public ObservableRangeCollection<Level> LevelList { get; }
        public ObservableRangeCollection<Department> DepartmentList { get; }
        public ObservableRangeCollection<Room> RoomList { get; }

        public Campus SelectedCampus { get => selectedCampus; set => SetProperty(ref selectedCampus, value); }
        public Building SelectedBuilding { get => selectedBuilding; set => SetProperty(ref selectedBuilding, value); }
        public Level SelectedLevel { get => selectedLevel; set => SetProperty(ref selectedLevel, value); }
        public Department SelectedDepartment { get => selectedDepartment; set => SetProperty(ref selectedDepartment, value); }
        public Room SelectedRoom { get => selectedRoom; set => SetProperty(ref selectedRoom, value); }

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

                if (value != null)
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

        public Room Room
        {
            get => room;
            set
            {
                IsRoom = true;
                SetProperty(ref room, value);
            }
        }

        public AsyncCommand NewCampusCommand { get; }
        public AsyncCommand NewBuildingCommand { get; }
        public AsyncCommand NewLevelCommand { get; }
        public AsyncCommand NewDepartmentCommand { get; }
        public AsyncCommand SaveCommand { get; }

        public MasterAddLocationViewModel()
        {
            CampusList = new ObservableRangeCollection<Campus>();
            BuildingList = new ObservableRangeCollection<Building>();
            LevelList = new ObservableRangeCollection<Level>();
            DepartmentList = new ObservableRangeCollection<Department>();
            RoomList = new ObservableRangeCollection<Room>();

            NewCampusCommand = new AsyncCommand(NewCampus);
            NewBuildingCommand = new AsyncCommand(NewBuilding);
            NewLevelCommand = new AsyncCommand(NewLevel);
            NewDepartmentCommand = new AsyncCommand(NewDepartment);
            SaveCommand = new AsyncCommand(Save);

            IsCampusNew = false;
            IsBuildingNew = false;
            IsLevelNew = false;
            IsDepartmentNew = false;
            getCampus();
        }

        private async Task Save()
        {
            try
            {
                IsBusy = true;

                var ans = await Application.Current.MainPage.DisplayAlert("Save", "Do you confirm your information?", "Yes", "No");
                if (ans)
                {
                    Campus campus = null;
                    Building building = null;
                    Level level = null;
                    Department department = null;
                    Room room = null;

                    int result = 0;

                    if (IsCampusNew)
                    {
                        campus = new Campus
                        {
                            CampusName = newCampusName
                        };

                        building = new Building
                        {
                            BuildingName = newBuildingName
                        };

                        level = new Level
                        {
                            LevelName = newLevelName
                        };

                        department = new Department
                        {
                            DepartmentName = newDepartmentName
                        };

                        room = new Room
                        {
                            RoomName = newRoomName
                        };

                        result = await LocationServices.AddCampusAndDepencies(campus, building, level, department, room);
                    }
                    else if(IsBuildingNew)
                    {
                        campus = Campus;

                        building = new Building
                        {
                            BuildingName = newBuildingName
                        };

                        level = new Level
                        {
                            LevelName = newLevelName
                        };

                        department = new Department
                        {
                            DepartmentName = newDepartmentName
                        };

                        room = new Room
                        {
                            RoomName = newRoomName
                        };

                        result = await LocationServices.AddBuildingAndDepencies(campus, building, level, department, room);
                    }
                    else if (IsLevelNew)
                    {
                        building = Building;

                        level = new Level
                        {
                            LevelName = newLevelName
                        };

                        department = new Department
                        {
                            DepartmentName = newDepartmentName
                        };

                        room = new Room
                        {
                            RoomName = newRoomName
                        };

                        result = await LocationServices.AddLevelAndDepencies(building, level, department, room);
                    }
                    else if (IsDepartmentNew)
                    {
                        level = Level;

                        department = new Department
                        {
                            DepartmentName = newDepartmentName
                        };

                        room = new Room
                        {
                            RoomName = newRoomName
                        };

                        result = await LocationServices.AddDepartmentAndDependencies(level, department, room);
                    }
                    else 
                    {
                        department = Department;

                        room = new Room
                        {
                            RoomName = newRoomName
                        };

                        result = await LocationServices.AddRoom(department, room);
                    }
                    if (result != 0)
                    {
                        await Application.Current.MainPage.DisplayAlert("Success", "New location added successfully", "Yes");
                        await Shell.Current.GoToAsync("..");
                    }
                    else
                    {
                        throw new Exception("Database error");
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

        private async Task NewCampus()
        {
            IsCampusNew = !IsCampusNew;
        }

        private async Task NewBuilding()
        {
            IsBuildingNew = !IsBuildingNew;
        }

        private async Task NewLevel()
        {
            IsLevelNew = !IsLevelNew;
        }

        private async Task NewDepartment()
        {
            IsDepartmentNew = !IsDepartmentNew;
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
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
        }
    }
}
