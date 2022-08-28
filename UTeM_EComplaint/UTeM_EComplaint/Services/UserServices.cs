using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UTeM_EComplaint.Model;
using Newtonsoft.Json;
using System.Net;

namespace UTeM_EComplaint.Services
{
    public class UserServices
    {

        public static async Task<User> Login(User user)
        {
            try
            {
                string url = string.Format("{0}/login" , Global.apiUrl);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(Global.HttpTimeout);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(user.Username), "username");
                form.Add(new StringContent(user.Password), "password");

                HttpResponseMessage response = await client.PostAsync(url,form);

                if(response.StatusCode == HttpStatusCode.OK)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    User values = JsonConvert.DeserializeObject<User>(result);                
                    client.Dispose();
                    return values;
                }
                else if(response.StatusCode == HttpStatusCode.NoContent)
                {
                    throw new Exception("Wrong username and password! Please try again");
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

        public static async Task<int> UpdateNotificationToken(User user)
        {
            try
            {
                string url = string.Format("{0}/updateNotificationToken", Global.apiUrl);
                var client = new HttpClient();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(user.UserID.ToString()), "userID");
                form.Add(new StringContent(user.NotificationToken), "notificationToken");
                HttpResponseMessage response = await client.PostAsync(url, form);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string resultString = await response.Content.ReadAsStringAsync();
                    int result = JsonConvert.DeserializeObject<int>(resultString);
                    client.Dispose();
                    return result;
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
