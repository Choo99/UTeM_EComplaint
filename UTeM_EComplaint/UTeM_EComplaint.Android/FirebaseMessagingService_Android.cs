using Android.App;
using Android.Content;
using Firebase.Messaging;

namespace UTeM_EComplaint.Droid
{
    [Service(Exported = true)]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    internal class FirebaseMessagingService_Android : FirebaseMessagingService
    {
        public FirebaseMessagingService_Android()
        {

        }
        public override void OnMessageReceived(RemoteMessage message)
        {
            var body = message.GetNotification().Body;
            var title = message.GetNotification().Title;
            
            new NotificationHelper_Android().CreateNotificationChannel();
            new NotificationHelper_Android().SendNotification(title, body, message.Data);
        }
    }
}