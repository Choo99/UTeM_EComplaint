using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using UTeM_EComplaint.Model;

namespace UTeM_EComplaint.Services
{
    public class NotificationServices
    {
        public static async Task CheckNotificationAvailabilityAddNewNotificationToken(NotificationManagement notification)
        {
            try
            {
                string url = string.Format("{0}/checkNotificationAvailabilityAddNewNotificationToken", Global.notificationUrl);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(notification.User.UserID.ToString()), "userID");
                form.Add(new StringContent(notification.NotificationToken), "notificationToken");

                HttpResponseMessage response = await client.PostAsync(url,form);

                if (response.StatusCode == HttpStatusCode.OK)
                {

                }
                else
                {
                    string resultString = await response.Content.ReadAsStringAsync();
                    object result = JsonConvert.DeserializeObject<object>(resultString);
                    client.Dispose();
                    throw new Exception(result.ToString());
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task CheckNotificationAvailabilityAddNewNotificationTokenAndUpdateTokenTimeStamp(NotificationManagement notification)
        {
            try
            {
                string url = string.Format("{0}/checkNotificationAvailabilityAddNewNotificationTokenAndUpdateTokenTimeStamp", Global.notificationUrl);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(notification.User.UserID.ToString()), "userID");
                form.Add(new StringContent(notification.NotificationToken), "notificationToken");

                HttpResponseMessage response = await client.PostAsync(url,form);

                if (response.StatusCode == HttpStatusCode.OK)
                {

                }
                else
                {
                    string resultString = await response.Content.ReadAsStringAsync();
                    object result = JsonConvert.DeserializeObject<object>(resultString);
                    client.Dispose();
                    throw new Exception(result.ToString());
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
