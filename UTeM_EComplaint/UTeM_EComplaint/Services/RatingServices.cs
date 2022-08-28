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
        public static async Task<List<ComplaintDetail>> GetTechnicianRatings(int technicianID)
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
                    List<ComplaintDetail> result = JsonConvert.DeserializeObject<List<ComplaintDetail>>(resultString);
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

        public static async Task AddRating(Rating rating)
        {
            try
            {
                string url = string.Format("{0}/addRating", Global.apiUrl);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(rating.Complaint.ComplaintID), "complaintID");
                form.Add(new StringContent(rating.Technician.TechnicianID.ToString()), "technicianID");
                form.Add(new StringContent(rating.RatingValue.ToString()), "ratingValue");

                HttpResponseMessage response = await client.PostAsync(url,form);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return;
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

        public static async Task<int> AddRatingAndSendMessage(string complaintID, int ratingValue)
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
