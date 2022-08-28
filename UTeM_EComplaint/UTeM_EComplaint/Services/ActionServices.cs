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
    internal class ActionServices
    {
        public static async Task StartAction(ActionTaken action)
        {
            try
            {
                string url = string.Format("{0}/addAction", Global.apiUrl);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(action.Complaint.ComplaintID), "complaintID");
                form.Add(new StringContent(action.Technician.TechnicianID.ToString()), "technicianID");

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

        public static async Task<int> StartActionAndSendMessage(string complaintID)
        {
            try
            {
                string url = string.Format("{0}/addActionAndSendMessage", Global.apiUrl);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(complaintID.ToString()), "complaintID");

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


        public static async Task EndActionAndCompleteComplaintDetail(ComplaintDetail complaintDetail)
        {
            try
            {
                string url = string.Format("{0}/endAction", Global.apiUrl);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(complaintDetail.Complaint.ComplaintID), "complaintID");
                form.Add(new StringContent(complaintDetail.Technician.TechnicianID.ToString()), "technicianID");
                form.Add(new StringContent(complaintDetail.Action.ActionDescription), "actionDescription");
                form.Add(new StringContent(complaintDetail.Action.SpareReplace), "spareReplace");
                form.Add(new StringContent(complaintDetail.Action.AdditionalNote), "additionalNote");
                

                HttpResponseMessage response = await client.PostAsync(url, form);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    /*string resultString = await response.Content.ReadAsStringAsync();
                    int result = JsonConvert.DeserializeObject<int>(resultString);
                    client.Dispose();
                    return result;*/
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

        /*public static async Task<int> EndActionAndSendNotification(Complaint complaint)
        {
            try
            {
                string url = string.Format("{0}/endActionAndUpdateComplaintAndSendNotification", Global.apiUrl);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(complaint.ComplaintID.ToString()), "complaintID");
                form.Add(new StringContent(complaint.ComplaintDetail.ActionDescription), "actionDescription");
                form.Add(new StringContent(complaint.Action.SpareReplace), "spareReplace");
                form.Add(new StringContent(complaint.Action.AdditionalNote), "additionalNote");


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
                    string result = JsonConvert.DeserializeObject<string>(resultString);
                    client.Dispose();
                    throw new Exception(result);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }*/

        /* public static async Task<int> EditAction(Complaint complaint)
         {
             try
             {
                 string url = string.Format("{0}/editAction", Global.apiUrl);
                 var client = new HttpClient();
                 client.Timeout = TimeSpan.FromSeconds(5000);
                 client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                 MultipartFormDataContent form = new MultipartFormDataContent();
                 form.Add(new StringContent(complaint.ComplaintID.ToString()), "complaintID");
                 form.Add(new StringContent(complaint.Technician.TechnicianID.ToString()), "technicianID");
                 form.Add(new StringContent(complaint.Action.ActionDescription), "actionDescription");
                 form.Add(new StringContent(complaint.Action.SpareReplace), "spareReplace");
                 form.Add(new StringContent(complaint.Action.AdditionalNote), "additionalNote");


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
         }*/
    }
}
