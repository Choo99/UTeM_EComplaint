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
    public class ComplaintServices
    {
        public static async Task<List<KeyValuePair<string,int>>> GetComplaintStatistic(int technicianID)
        {
            try
            {
                string url = string.Format("{0}/getComplaintStatistic?technicianID={1}", Global.apiUrl, technicianID);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string resultString = await response.Content.ReadAsStringAsync();
                    List<KeyValuePair<string, int>> result = JsonConvert.DeserializeObject<List<KeyValuePair<string, int>>>(resultString);
                    client.Dispose();
                    return result;
                }
                else if (response.StatusCode == HttpStatusCode.NoContent)
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

        public static async Task<List<Complaint>> GetTechnicianComplaint(int technicianID)
        {
            try
            {
                string url = string.Format("{0}/getTechnicianComplaint?technicianID={1}", Global.apiUrl, technicianID);
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

        public static async Task<Complaint> GetComplaintDetail(int complaintID)
        {
            try
            {
                string url = string.Format("{0}/getComplaintDetail?complaintID={1}", Global.apiUrl, complaintID);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string resultString = await response.Content.ReadAsStringAsync();
                    Complaint result = JsonConvert.DeserializeObject<Complaint>(resultString);
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

        public static async Task<List<Complaint>> GetComplaintsByStatus(int userID, string complaintStatus)
        {
            try
            {
                if(!(complaintStatus == "Pending" || complaintStatus == "Assigned" 
                    || complaintStatus == "In Progress" || complaintStatus == "Completed" 
                    || complaintStatus == ""))
                {
                    throw new Exception("Wrong complaint status");
                }
                string url = string.Format("{0}/getComplaintsByStatus?userID={1}&complaintStatus={2}", Global.apiUrl, userID, complaintStatus);
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

        public static async Task<List<KeyValuePair<string, int>>> GetStaffComplaintStatistic(int staffID)
        {
            try
            {
                string url = string.Format("{0}/getStaffComplaintStatistic?staffID={1}", Global.apiUrl, staffID);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string resultString = await response.Content.ReadAsStringAsync();
                    List<KeyValuePair<string, int>> result = JsonConvert.DeserializeObject<List<KeyValuePair<string, int>>>(resultString);
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

        public static async Task<List<Complaint>> GetStaffComplaint(int staffID)
        {
            try
            {
                string url = string.Format("{0}/getStaffComplaint?staffID={1}", Global.apiUrl, staffID);
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

        public static async Task<List<Complaint>> GetStaffComplaintByStatus(int staffID,string complaintStatus)
        {
            try
            {
                string url = string.Format("{0}/getStaffComplaintByStatus?staffID={1}&complaintStatus={2}", Global.apiUrl, staffID,complaintStatus);
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

        public static async Task<int> AddComplaint(Complaint complaint)
        {
            try
            {
                string url = string.Format("{0}/addComplaint", Global.apiUrl);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(complaint.Staff.StaffID.ToString()), "staffID");
                form.Add(new StringContent(complaint.Division.DivisionID.ToString()), "divisionID");
                form.Add(new StringContent(complaint.DamageType.DamageTypeID.ToString()), "damageTypeID");
                form.Add(new StringContent(complaint.Category.CategoryId.ToString()), "categoryID");
                form.Add(new StringContent(complaint.Damage), "damage");
                form.Add(new StringContent(complaint.Location), "location");
                form.Add(new StringContent(complaint.ContactPhoneNumber), "contactPhoneNumber");

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

        public static async Task<int> AddComplaintAndSubscribe(Complaint complaint, string notificationToken)
        {
            try
            {
                string url = string.Format("{0}/addComplaintAndsubscribe", Global.apiUrl);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(complaint.Staff.StaffID.ToString()), "staffID");
                form.Add(new StringContent(complaint.Division.DivisionID.ToString()), "divisionID");
                form.Add(new StringContent(complaint.DamageType.DamageTypeID.ToString()), "damageTypeID");
                form.Add(new StringContent(complaint.Category.CategoryId.ToString()), "categoryID");
                form.Add(new StringContent(complaint.Damage), "damage");
                form.Add(new StringContent(complaint.Location), "location");
                form.Add(new StringContent(complaint.ContactPhoneNumber), "contactPhoneNumber");
                form.Add(new StringContent(notificationToken), "notificationToken");

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

        public static async Task<List<Complaint>> GetAllComplaints()
        {
            try
            {
                string url = string.Format("{0}/getAllComplaints", Global.apiUrl);
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

        public static async Task<int> StaffEditComplaint(Complaint complaint)
        {
            try
            {
                string url = string.Format("{0}/staffEditComplaint", Global.apiUrl);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(complaint.ComplaintID.ToString()), "complaintID");
                form.Add(new StringContent(complaint.Staff.StaffID.ToString()), "staffID");
                form.Add(new StringContent(complaint.Division.DivisionID.ToString()), "divisionID");
                form.Add(new StringContent(complaint.DamageType.DamageTypeID.ToString()), "damageTypeID");
                form.Add(new StringContent(complaint.Category.CategoryId.ToString()), "categoryID");
                form.Add(new StringContent(complaint.Damage), "damage");
                form.Add(new StringContent(complaint.Location), "location");
                form.Add(new StringContent(complaint.ContactPhoneNumber), "contactPhoneNumber");

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
                    throw new Exception(result);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task<List<Complaint>> SearchComplaints(int userID, string searchText)
        {
            try
            {
                string url = string.Format("{0}/searchComplaints?userID={1}&searchText={2}", Global.apiUrl, userID, searchText);
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

        public static async Task<List<Complaint>> SearchAllComplaints(string searchText)
        {
            try
            {
                string url = string.Format("{0}/searchAllComplaints?searchText={1}", Global.apiUrl, searchText);
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

        public static async Task<List<KeyValuePair<string, int>>> GetAllComplaintStatistic()
        {
            try
            {
                string url = string.Format("{0}/getAllComplaintStatistic", Global.apiUrl);
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string resultString = await response.Content.ReadAsStringAsync();
                    List<KeyValuePair<string, int>> result = JsonConvert.DeserializeObject<List<KeyValuePair<string, int>>>(resultString);
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

        public static async Task<List<Complaint>> GetAllPendingComplaints()
        {
            try
            {
                string url = string.Format("{0}/getAllPendingComplaints", Global.apiUrl);
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

        public static async Task<List<Complaint>> GetComplaintsByDate(string startDate, string endDate)
        {
            try
            {
                string url = string.Format("{0}/getComplaintsByDate?startDate={1}&endDate={2}", Global.apiUrl,startDate,endDate);
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
