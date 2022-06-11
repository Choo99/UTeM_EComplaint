using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using UTeM_EComplaint.Views;

namespace UTeM_EComplaint.ViewModels
{
    internal class StaffComplaintTabbedViewModel : ViewModelBase
    {
        string pathToSearch = $"{nameof(StaffSearchComplaintPage)}";

        public AsyncCommand SearchCommand { get; }

        public StaffComplaintTabbedViewModel()
        {
            Title = "Complaints";
            SearchCommand = new AsyncCommand(Search);
        }

        private async Task Search()
        {
            await Shell.Current.GoToAsync(pathToSearch);
        }
    }
}
