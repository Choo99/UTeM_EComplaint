using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UTeM_EComplaint.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterSubmenuPage : ContentPage
    {
        public object FocusedElement;
        public MasterSubmenuPage()
        {
            InitializeComponent();
        }
        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            if (FocusedElement != null)
            {
                var entry = FocusedElement as Entry;
                entry.Unfocus();
            }
        }

        private void FocusEvent(object sender, FocusEventArgs e)
        {
            FocusedElement = sender;
        }

        private void UnfocusEvent(object sender, FocusEventArgs e)
        {
            FocusedElement = null;
        }
    }
}