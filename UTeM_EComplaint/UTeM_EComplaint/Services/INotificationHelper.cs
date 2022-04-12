using System;
using System.Collections.Generic;
using System.Text;

namespace UTeM_EComplaint.Services
{
    public interface INotificationHelper
    {
        string GetToken();

        void SendNotification(string title, string messageBody, IDictionary<string, string> data);

        void CreateNotificationChannel();

        void UnsubscribeFromTopic(string role, int complaintID);

        void DeleteInstance();

    }
}
