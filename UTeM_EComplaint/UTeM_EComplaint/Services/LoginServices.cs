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
    public class LoginServices
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
                    throw new Exception(response.ReasonPhrase);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
