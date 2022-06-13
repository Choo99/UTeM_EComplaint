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
    internal class StaffAddAddressViewModel : ViewModelBase
    {
        bool isCampus;
        bool isBuilding;
        bool isLevel;
        bool isDepartment;
        bool isRoom;
        public ObservableRangeCollection<string> CampusList { get; }
        public ObservableRangeCollection<string> BuildingList { get; }
        public ObservableRangeCollection<string> LevelList { get; }
        public ObservableRangeCollection<string> DepartmentList { get; }
        public ObservableRangeCollection<string> RoomList { get; }

        public bool IsCampus { get => isCampus; set => SetProperty(ref isCampus, value); }
        public bool IsBuilding { get => isBuilding; set => SetProperty(ref isBuilding, value); }
        public bool IsLevel { get => isLevel; set => SetProperty(ref isLevel, value); }
        public bool IsDepartment { get => isDepartment; set => SetProperty(ref isDepartment, value); }
        public bool IsRoom { get => isRoom; set => SetProperty(ref isRoom, value); }

        string campus;
        string building;
        string level;
        string department;
        string room;

        public string Campus 
        { 
            get => campus; 
            set
            {
                SetProperty(ref campus, value);
                if (isBuilding || isLevel || isDepartment || isRoom)
                {
                    Building = string.Empty;
                    Level = string.Empty;
                    Department = string.Empty;
                    Room = string.Empty;

                    BuildingList.Clear();
                    LevelList.Clear();
                    DepartmentList.Clear();
                    RoomList.Clear();

                    IsBuilding = false;
                    IsLevel = false;
                    IsDepartment = false;
                    IsRoom = false;
                }
                IsCampus = true;

                List<string> mainCampusList = new List<string>()
                {
                        "FTMK",
                        "FKP",
                        "FKE",
                        "Library",
                        "HEP"
                };
                List<string> TechnologyCampus = new List<string>()
                {
                        "FPTT",
                        "FTK",
                        "FKM",
                };
                if (value == "Main Campus")
                {
                    BuildingList.ReplaceRange(mainCampusList);
                }
                else
                {
                    BuildingList.ReplaceRange(TechnologyCampus);
                }
                
            }
        }

        public string Building 
        { 
            get => building;
            set
            {
                if (isLevel || isDepartment || isRoom)
                {
                    Level = string.Empty;
                    Department = string.Empty;
                    Room = string.Empty;

                    LevelList.Clear();
                    DepartmentList.Clear();
                    RoomList.Clear();

                    IsLevel = false;
                    IsDepartment = false;
                    IsRoom = false;
                }

                List<string> levelList = new List<string>()
                {
                        "Level G",
                        "Level 1",
                        "Level 2",
                        "Level 3",
                        "Level 4"
                };
                List<string> smallLevelList = new List<string>()
                {
                        "Level G",
                        "Level 1",
                };
                SetProperty(ref building, value);
                if(value != "FPTT" && value != "FTK" && value != "HEP")
                {
                    LevelList.AddRange(smallLevelList);
                }
                else
                {
                    LevelList.AddRange(levelList);
                }

                IsBuilding = true;
            }
        }

        public string Level 
        { 
            get => level;
            set
            {
                if (isDepartment || isRoom)
                {
                    Department = string.Empty;
                    Room = string.Empty;

                    DepartmentList.Clear();
                    RoomList.Clear();

                    IsDepartment = false;
                    IsRoom = false;
                }

                List<string> departmentList = new List<string>()
                {
                    "Department 1",
                    "Department 2",
                    "Department 3",
                };
                SetProperty(ref level, value);
                DepartmentList.AddRange(departmentList);
                IsLevel = true;
            }
        }

        public string Department 
        { 
            get => department;
            set
            {
                if (isRoom)
                {
                    Room = string.Empty;

                    RoomList.Clear();

                    IsRoom = false;
                }

                List<string> roomList = new List<string>()
                {
                    "Room 1",
                    "Room 2",
                    "Room 3",
                };
                SetProperty(ref department, value);
                RoomList.AddRange(roomList);

                IsDepartment = true;
            }
        }

        public string Room {
            get => room;
            set
            {
                SetProperty(ref room, value);
                IsRoom = true;
            }
        }

        public AsyncCommand ConfirmCommand { get; }
        public AsyncCommand ClearCommand { get; }

        public StaffAddAddressViewModel()
        {
            CampusList = new ObservableRangeCollection<string>();
            BuildingList = new ObservableRangeCollection<string>();
            LevelList = new ObservableRangeCollection<string>();
            DepartmentList = new ObservableRangeCollection<string>();
            RoomList = new ObservableRangeCollection<string>();

            ConfirmCommand = new AsyncCommand(Confirm);
            ClearCommand = new AsyncCommand(Clear);

            CampusList.AddRange(new List<string>()
            {
               "Technology Campus",
               "Main Campus"
            });
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
                
                await Shell.Current.GoToAsync(string.Format("../?campus={0}&building={1}&level={2}&department={3}&room={4}",Campus,Building,Level,Department,Room));
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

            Campus = string.Empty;
            Building = string.Empty;
            Level = string.Empty;
            Department = string.Empty;
            Room = string.Empty;
        }
    }
}
