﻿using System;
using Android.App;
using Firebase.Iid;
using Android.Util;
using UTeM_EComplaint.Services;
using Android.OS;
using Android.Content;
using Xamarin.Forms;
using UTeM_EComplaint.Droid;
using System.Collections.Generic;
using AndroidX.Core.App;
using Firebase.Messaging;
using Android.Media;
using UTeM_EComplaint.Model;
using Xamarin.Essentials;

[assembly: Dependency(typeof(NotificationHelper_Android))]
namespace UTeM_EComplaint.Droid
{
    
    public class NotificationHelper_Android : INotificationHelper
    {
        private Context mContext;


        public NotificationHelper_Android()
        {
            mContext = global::Android.App.Application.Context;
        }
        public void SendNotification(string title, string messageBody, IDictionary<string, string> data)
        {
            var intent = new Intent(mContext, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            foreach (var key in data.Keys)
            {
                intent.PutExtra(key, data[key]);
            }

            var pendingIntent = PendingIntent.GetActivity(mContext, MainActivity.NOTIFICATION_ID, intent, PendingIntentFlags.OneShot);

            var notificationBuilder = new NotificationCompat.Builder(mContext, MainActivity.CHANNEL_ID)
                                        .SetContentTitle(title)
                                        .SetSmallIcon(Resource.Drawable.pppk)
                                        .SetContentText(messageBody)
                                        .SetAutoCancel(true)
                                        .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Ringtone))
                                        .SetContentIntent(pendingIntent);
            var notificationManager = NotificationManagerCompat.From(mContext);
            notificationManager.Notify(new Random().Next(), notificationBuilder.Build());

        }

        public void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channel = new NotificationChannel(MainActivity.CHANNEL_ID,
                                                    "FCM Notifications",
                                                    NotificationImportance.Default)
            {

                Description = "Firebase Cloud Messages appear in this channel"
            };

            var notificationManager = (NotificationManager)mContext.GetSystemService(Android.Content.Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        [Obsolete]
        public async void GetToken()
        {
            try
            {
                var token = FirebaseInstanceId.Instance.Token;

                await NotificationServices.CheckNotificationAvailabilityAddNewNotificationTokenAndUpdateTokenTimeStamp(new NotificationManagement
                {
                    User = new User
                    {
                        UserID = Preferences.Get("userID", 0),
                    },
                    NotificationToken = token,
                });
            }
            catch(Exception)
            {
                throw;
            }

        }
        public void UnsubscribeFromTopic(string role, string complaintID)
        {
            string topic = "";
            if(role == "staff")
            {
                topic = "complaintID" + complaintID;
            }
            else if(role == "technician")
            {
                topic = "ComplaintID" + complaintID + "Technician";
            }
            FirebaseMessaging.Instance.UnsubscribeFromTopic(topic);
        }

        [Obsolete]
        public void DeleteInstance()
        {
            FirebaseInstanceId.Instance.DeleteInstanceId();
        }

        [Obsolete]
        public async void UpdateInstanceID(User user)
        {
            user.NotificationToken = FirebaseInstanceId.Instance.Token;
            int result = await UserServices.UpdateNotificationToken(user);
        }
    }
}