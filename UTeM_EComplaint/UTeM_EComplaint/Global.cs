using System;
using System.Collections.Generic;
using System.Text;

namespace UTeM_EComplaint
{
    class Global
    {
        public static string AppName = "UTeMTechnician";
        public static int HttpTimeout = 2000;
        public static int local_curr_version = 12;
        public static int GDesiredAccuracy = 5;
        public static int G_StatusLogon = 0;
        public static string G_Msg = "x";
        public static string apiUrl = "https://devmobile.utem.edu.my/student1/api/Complaint";
        public static string notificationUrl = "https://devmobile.utem.edu.my/student1/api/Notification";

        // public event PropertyChangedEventHandler PropertyChanged;

        public static class UserLoginDetails
        {
            public static string NoTechnician { set; get; }
            public static string Nama_Technician { set; get; }
            public static string Username_Technician { set; get; }
            public static string Password { set; get; }

        }

        public static class AduanDetails
        {
            public static string NoAduan { set; get; }
            public static string NoStaff { set; get; }
            public static string NamaStaff { set; get; }
            public static DateTime TarikhAduan { set; get; }
            public static string Bahagian { set; get; }
            public static int JDKID { set; get; }
            public static string PTJID { set; get; }
            public static string PTJName { set; get; }
            public static string Kerosakan { set; get; }
            public static string Ulasan { set; get; }
            public static string Lokasi { set; get; }
            public static string Hubungi { set; get; }
            public static DateTime Tarikh_mula { set; get; }
            public static DateTime Tarikh_Selesai { set; get; }
            public static int Bilangan_Hari { set; get; }
            public static string Tindakan { set; get; }
            public static string StatusTask { set; get; }
            public static string StatusTaskID { set; get; }
            public static string KomponenID { set; get; }
            public static string JenisJDK { get; set; }
            public static string KategoriJDK { get; set; }
            public static string idselected { get; set; }
            public static string idselectedinprogress { get; set; }

        }
    }
}
