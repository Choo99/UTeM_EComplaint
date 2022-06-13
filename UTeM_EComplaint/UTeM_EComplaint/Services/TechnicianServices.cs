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
    internal class TechnicianServices
    {
        public static async Task<List<Technician>> GetAllTechnicians()
        {
            try
            {
                string url = string.Format("{0}/getAllTechnicians", Global.apiUrl);
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string resultString = await response.Content.ReadAsStringAsync();
                    List<Technician> result = JsonConvert.DeserializeObject<List<Technician>>(resultString);
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

        public static async Task<Technician> GetTechnician(int technicianID)
        {
            try
            {
                string url = string.Format("{0}/getTechnicianWithStatistic?technicianID={1}", Global.apiUrl, technicianID);
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string resultString = await response.Content.ReadAsStringAsync();
                    Technician result = JsonConvert.DeserializeObject<Technician>(resultString);
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

        public static async Task<int> UpdateTechnicianAndSubscribe(Complaint complaint)
        {
            try
            {
                string url = string.Format("{0}/updateTechnicianAndSubscribe", Global.apiUrl);
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(complaint.ComplaintID.ToString()), "complaintID");
                form.Add(new StringContent(complaint.Technician.TechnicianID.ToString()), "technicianID");

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
                    string resultString = await response.Content.ReadAsStringAsync();
                    string result = JsonConvert.DeserializeObject<string>(resultString);
                    client.Dispose();
                    throw new Exception(result);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task<List<Technician>> GetAllTechnicianWithStatistic()
        {
            try
            {
                string url = string.Format("{0}/getAllTechnicianWithStatistic", Global.apiUrl);
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string resultString = await response.Content.ReadAsStringAsync();
                    List<Technician> result = JsonConvert.DeserializeObject<List<Technician>>(resultString);
                    client.Dispose();
                    return result;
                }
                else
                {
                    string resultString = await response.Content.ReadAsStringAsync();
                    string result = JsonConvert.DeserializeObject<string>(resultString);
                    client.Dispose();
                    throw new Exception(result);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task<Technician> GetTechnicianWithStatistic(int technicianID)
        {
            try
            {
                string url = string.Format("{0}/getTechnicianWithStatistic?technicianID={1}", Global.apiUrl,technicianID);
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string resultString = await response.Content.ReadAsStringAsync();
                    Technician result = JsonConvert.DeserializeObject<Technician>(resultString);
                    client.Dispose();
                    return result;
                }
                else
                {
                    string resultString = await response.Content.ReadAsStringAsync();
                    string result = JsonConvert.DeserializeObject<string>(resultString);
                    client.Dispose();
                    throw new Exception(result);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
