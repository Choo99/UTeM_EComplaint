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
    internal class RatingServices
    {
        public static async Task<List<Complaint>> GetTechnicianRatings(int technicianID)
        {
            try
            {
                string url = string.Format("{0}/getTechnicianRatings?technicianID={1}", Global.apiUrl, technicianID);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string resultString = await response.Content.ReadAsStringAsync();
                    List<Complaint> result = JsonConvert.DeserializeObject<List<Complaint>>(resultString);
                    client.Dispose();
                    return result;
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
        public static async Task<List<KeyValuePair<string,double>>> GetTechnicianTotalRating(int technicianID)
        {
            try
            {
                string url = string.Format("{0}/getTechnicianTotalRating?technicianID={1}", Global.apiUrl, technicianID);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string resultString = await response.Content.ReadAsStringAsync();
                    List<KeyValuePair<string, double>> result = JsonConvert.DeserializeObject<List<KeyValuePair<string, double>>>(resultString);
                    client.Dispose();
                    return result;
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

        public static async Task<int> AddRating(int complaintID,int ratingValue)
        {
            try
            {
                string url = string.Format("{0}/addRating", Global.apiUrl);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(complaintID.ToString()), "complaintID");
                form.Add(new StringContent(ratingValue.ToString()), "ratingValue");

                HttpResponseMessage response = await client.PostAsync(url,form);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string resultString = await response.Content.ReadAsStringAsync();
                    int result = JsonConvert.DeserializeObject<int>(resultString);
                    client.Dispose();
                    return result;
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

        public static async Task<int> AddRatingAndSendMessage(int complaintID, int ratingValue)
        {
            try
            {
                string url = string.Format("{0}/addRatingAndSendMessage", Global.apiUrl);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(complaintID.ToString()), "complaintID");
                form.Add(new StringContent(ratingValue.ToString()), "ratingValue");

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
