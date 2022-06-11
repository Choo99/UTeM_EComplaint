using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UTeM_EComplaint.Views;
using Xamarin.Forms;

namespace UTeM_EComplaint.ViewModels
{
    internal class TechnicianTabbedViewModel : ViewModelBase
    {
        string pathToSearch = $"{nameof(JobSearchingPage)}";

        public AsyncCommand SearchCommand { get; }

        public TechnicianTabbedViewModel()
        {
            SearchCommand = new AsyncCommand(Search);
        }

        private async Task Search()
        {
            await Shell.Current.GoToAsync(pathToSearch);
        }
    }
}
