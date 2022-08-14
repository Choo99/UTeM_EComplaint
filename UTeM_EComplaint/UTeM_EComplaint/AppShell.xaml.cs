using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTeM_EComplaint.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UTeM_EComplaint
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(JobHistoryPage), typeof(JobHistoryPage));
            Routing.RegisterRoute(nameof(JobProgressPage), typeof(JobProgressPage));
            Routing.RegisterRoute(nameof(JobTodoPage), typeof(JobTodoPage));
            Routing.RegisterRoute(nameof(JobDetailPage), typeof(JobDetailPage));
            Routing.RegisterRoute(nameof(TechnicianRatingPage), typeof(TechnicianRatingPage));
            Routing.RegisterRoute(nameof(StaffAddComplaintPage), typeof(StaffAddComplaintPage));
            Routing.RegisterRoute(nameof(StaffComplaintDetailPage), typeof(StaffComplaintDetailPage));
            Routing.RegisterRoute(nameof(StaffComplaintHistoryPage), typeof(StaffComplaintHistoryPage));
            Routing.RegisterRoute(nameof(JobToDoDetailPage), typeof(JobToDoDetailPage));
            Routing.RegisterRoute(nameof(JobInProgressDetailPage), typeof(JobInProgressDetailPage));
            Routing.RegisterRoute(nameof(JobCompletedDetailPage), typeof(JobCompletedDetailPage));
            Routing.RegisterRoute(nameof(StaffComplaintHistoryPage), typeof(StaffComplaintHistoryPage));
            Routing.RegisterRoute(nameof(TechnicianTabbedPage), typeof(TechnicianTabbedPage));
            Routing.RegisterRoute(nameof(StaffCompaintTabbedPage), typeof(StaffCompaintTabbedPage));
            Routing.RegisterRoute(nameof(AdminReportTabbedPage), typeof(AdminReportTabbedPage));
            Routing.RegisterRoute(nameof(StaffComplaintPendingPage), typeof(StaffComplaintPendingPage));
            Routing.RegisterRoute(nameof(StaffComplaintInProgressPage), typeof(StaffComplaintInProgressPage));
            Routing.RegisterRoute(nameof(StaffComplaintAssignedPage), typeof(StaffComplaintAssignedPage));
            Routing.RegisterRoute(nameof(StaffAddRatingPage), typeof(StaffAddRatingPage));
            Routing.RegisterRoute(nameof(StaffNotificationPage), typeof(StaffNotificationPage));
            Routing.RegisterRoute(nameof(TechnicianNotificationPage), typeof(TechnicianNotificationPage));
            Routing.RegisterRoute(nameof(StaffViewAllComplaintPage), typeof(StaffViewAllComplaintPage));
            Routing.RegisterRoute(nameof(StaffEditComplaintPage), typeof(StaffEditComplaintPage));
            Routing.RegisterRoute(nameof(JobEditPage), typeof(JobEditPage));
            Routing.RegisterRoute(nameof(JobSearchingPage), typeof(JobSearchingPage));
            Routing.RegisterRoute(nameof(StaffSearchComplaintPage), typeof(StaffSearchComplaintPage));
            Routing.RegisterRoute(nameof(AdminAssignComplaintPage), typeof(AdminAssignComplaintPage));
            Routing.RegisterRoute(nameof(AdminAssignDetailPage), typeof(AdminAssignDetailPage));
            Routing.RegisterRoute(nameof(AdminAssignTechnicianPage), typeof(AdminAssignTechnicianPage));
            Routing.RegisterRoute(nameof(AdminReportPage), typeof(AdminReportPage));
            Routing.RegisterRoute(nameof(AdminTechnicianReportPage), typeof(AdminTechnicianReportPage));
            Routing.RegisterRoute(nameof(AdminViewAllComplaintPage), typeof(AdminViewAllComplaintPage));
            Routing.RegisterRoute(nameof(AdminComplaintDetailPage), typeof(AdminComplaintDetailPage));
            Routing.RegisterRoute(nameof(StaffAddAddressPage), typeof(StaffAddAddressPage));
            Routing.RegisterRoute(nameof(StaffMapPage), typeof(StaffMapPage));
            Routing.RegisterRoute(nameof(MasterCategoryPage), typeof(MasterCategoryPage));
            Routing.RegisterRoute(nameof(MasterDivisionPage), typeof(MasterDivisionPage));
            Routing.RegisterRoute(nameof(MasterDamageTypePage), typeof(MasterDamageTypePage));
            Routing.RegisterRoute(nameof(MasterLocationPage), typeof(MasterLocationPage));
            Routing.RegisterRoute(nameof(MasterAddLocationPage), typeof(MasterAddLocationPage));


            Routing.RegisterRoute($"home/{nameof(StaffComplaintPendingPage)}", typeof(StaffComplaintPendingPage));
            Routing.RegisterRoute($"home/{nameof(StaffComplaintAssignedPage)}", typeof(StaffComplaintAssignedPage));
            Routing.RegisterRoute($"home/{nameof(StaffComplaintInProgressPage)}", typeof(StaffComplaintInProgressPage));
            Routing.RegisterRoute($"home/{nameof(StaffComplaintHistoryPage)}", typeof(StaffComplaintHistoryPage));

        }
    }
}