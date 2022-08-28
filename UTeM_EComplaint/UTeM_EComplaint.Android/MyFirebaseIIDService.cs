using System;
using Android.App;
using Firebase.Iid;
using Android.Util;
using UTeM_EComplaint.Services;
using Xamarin.Essentials;
using UTeM_EComplaint.Model;

namespace UTeM_EComplaint.Droid
{
    [Service(Exported = true)]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    [Obsolete]
    public class MyFirebaseIIDService : FirebaseInstanceIdService
    {
        const string TAG = "MyFirebaseIIDService";

        [Obsolete]
        public override void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            Log.Debug(TAG, "Refreshed token: " + refreshedToken);
            SendRegistrationToServer(refreshedToken);
        }
        async void SendRegistrationToServer(string token)
        {
            int userID = Preferences.Get("userID", 0);

            await NotificationServices.CheckNotificationAvailabilityAddNewNotificationToken(new NotificationManagement
            {
                User = new User
                {
                    UserID = userID,
                },
                NotificationToken = token
            });
        }
    }
}