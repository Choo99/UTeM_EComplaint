using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UTeM_EComplaint.Model;

namespace UTeM_EComplaint.Services
{
    internal class ComplaintTypeServices
    {
        public static async Task<List<ComplaintType>> GetComplaintType()
        {
            try
            {
                string url = string.Format("{0}/getComplaintTypes", Global.apiUrl);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string resultString = await response.Content.ReadAsStringAsync();
                    List<ComplaintType> result = JsonConvert.DeserializeObject<List<ComplaintType>>(resultString);
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
