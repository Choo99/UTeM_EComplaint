using System;
using System.Collections.Generic;
using System.Text;
using UTeM_EComplaint.Model;

namespace UTeM_EComplaint.Services
{
    public interface INotificationHelper
    {
        void GetToken();

        void SendNotification(string title, string messageBody, IDictionary<string, string> data);

        void CreateNotificationChannel();

        void UnsubscribeFromTopic(string role, string complaintID);

        void DeleteInstance();

        void UpdateInstanceID(User user);

    }
}
