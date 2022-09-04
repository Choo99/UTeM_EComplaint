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

        public static async Task<Complaint> GetComplaintDetail(string complaintID)
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

        public static async Task<Complaint> GetComplaintAndComplaintDetail(string complaintID)
        {
            try
            {
                string url = string.Format("{0}/getComplaintAndComplaintDetail?complaintID={1}", Global.apiUrl, complaintID);
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

        

        public static async Task<string> AddComplaint(Complaint complaint)
        {
            try
            {
                string url = string.Format("{0}/addComplaint", Global.apiUrl);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(complaint.Staff.StaffID.ToString()), "staffID");
                form.Add(new StringContent(complaint.ComplaintType.ComplaintTypeCode), "complaintTypeCode");
                form.Add(new StringContent(complaint.SoftwareSystem != null? complaint.SoftwareSystem.SystemCode: string.Empty), "systemCode");
                form.Add(new StringContent(complaint.Module != null ? complaint.Module.ModuleCode: string.Empty), "moduleCode");
                form.Add(new StringContent(complaint.Submodule != null ? complaint.Submodule.SubmoduleCode: string.Empty), "submoduleCode");
                form.Add(new StringContent(complaint.Submenu != null ? complaint.Submenu.SubmenuCode: string.Empty), "submenuCode");
                form.Add(new StringContent(complaint.Division != null ? complaint.Division.DivisionID.ToString(): "0"), "divisionID");
                form.Add(new StringContent(complaint.DamageType != null ? complaint.DamageType.DamageTypeID.ToString(): "0"), "damageTypeID");
                form.Add(new StringContent(complaint.Category != null ?complaint.Category.CategoryId.ToString(): "0"), "categoryID");
                form.Add(new StringContent(complaint.Damage), "damage");
                form.Add(new StringContent(complaint.Location != null ? complaint.Location : string.Empty), "location");
                form.Add(new StringContent(complaint.ContactPhoneNumber), "contactPhoneNumber");
                form.Add(new StringContent(complaint.Longitude != 0 ? complaint.Longitude.ToString() : "0"), "longitude"); 
                form.Add(new StringContent(complaint.Latitude != 0 ? complaint.Latitude.ToString() : "0"), "latitude");
                form.Add(new StringContent(complaint.ImageBase64 != null ? complaint.ImageBase64 : string.Empty), "imageBase64");

                HttpResponseMessage response = await client.PostAsync(url, form);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string resultString = await response.Content.ReadAsStringAsync();
                    string result = JsonConvert.DeserializeObject<string>(resultString);
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
                form.Add(new StringContent(complaint.Longitude.ToString()), "longitude");
                form.Add(new StringContent(complaint.Latitude.ToString()), "latitude");
                form.Add(new StringContent(complaint.ImageBase64.ToString()), "imageBase64");

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

        public static async Task<int> StaffEditSoftwareComplaint(Complaint complaint)
        {
            try
            {
                string url = string.Format("{0}/staffEditSoftwareComplaint", Global.apiUrl);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(complaint.ComplaintID.ToString()), "complaintID");
                form.Add(new StringContent(complaint.Staff.StaffID.ToString()), "staffID");
                form.Add(new StringContent(complaint.SoftwareSystem.SystemCode), "systemCode");
                form.Add(new StringContent(complaint.Module.ModuleCode), "moduleCode");
                form.Add(new StringContent(complaint.Submodule.SubmoduleCode), "submoduleCode");
                if(complaint.Submenu.SubmenuCode != null)
                {
                    form.Add(new StringContent(complaint.Submenu.SubmenuCode), "submenuCode");
                }
                form.Add(new StringContent(complaint.Damage), "damage");
                form.Add(new StringContent(complaint.ContactPhoneNumber), "contactPhoneNumber");
                form.Add(new StringContent(complaint.ImageBase64.ToString()), "imageBase64");

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

        public static async Task<List<Complaint>> GetSoftwareComplaintsByDate(string startDate, string endDate)
        {
            try
            {
                string url = string.Format("{0}/getSoftwareComplaintsByDate?startDate={1}&endDate={2}", Global.apiUrl, startDate, endDate);
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

        public static async Task UpdateComplaintReportAndCompleteComplaint(Complaint complaint)
        {
            try
            {
                string url = string.Format("{0}/updateComplaintReportAndCompleteComplaint", Global.apiUrl);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(complaint.ComplaintID), "complaintID");
                form.Add(new StringContent(complaint.Report), "report");

                HttpResponseMessage response = await client.PostAsync(url, form);

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
