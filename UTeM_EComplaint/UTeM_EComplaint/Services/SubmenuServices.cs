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
    internal class SubmenuServices
    {
        public static async Task<List<Submenu>> GetAllSubmenus()
        {
            try
            {
                string url = string.Format("{0}/getAllSubmenus", Global.apiUrl);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string resultString = await response.Content.ReadAsStringAsync();
                    List<Submenu> result = JsonConvert.DeserializeObject<List<Submenu>>(resultString);
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

        public static async Task<List<Submenu>> GetAvailableSubmenusBySubmodule(Submenu submenu)
        {
            try
            {
                string url = string.Format("{0}/getAvailableSubmenusBySubmodule?submoduleID={1}", Global.apiUrl,submenu.Submodule.SubmoduleID.ToString());
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string resultString = await response.Content.ReadAsStringAsync();
                    List<Submenu> result = JsonConvert.DeserializeObject<List<Submenu>>(resultString);
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

        public static async Task<List<Submenu>> GetSubmenusBySubmodule(Submenu submenu)
        {
            try
            {
                string url = string.Format("{0}/getSubmenusBySubmodule?submoduleID={1}", Global.apiUrl,submenu.Submodule.SubmoduleID.ToString());
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string resultString = await response.Content.ReadAsStringAsync();
                    List<Submenu> result = JsonConvert.DeserializeObject<List<Submenu>>(resultString);
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

        public static async Task EditSubmenu(Submenu submenu)
        {
            try
            {
                string url = string.Format("{0}/editSubmenu", Global.apiUrl);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(submenu.SubmenuID.ToString()), "submenuID");
                form.Add(new StringContent(submenu.SubmenuCode), "submenuCode");
                form.Add(new StringContent(submenu.SubmenuName), "submenuName");
                form.Add(new StringContent(submenu.Status.ToString()), "status");

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

        public static async Task AddSubmenu(Submenu submenu)
        {
            try
            {
                string url = string.Format("{0}/addSubmenu", Global.apiUrl);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(submenu.SubmenuCode), "submenuCode");
                form.Add(new StringContent(submenu.SubmenuName), "submenuName");
                form.Add(new StringContent(submenu.Status.ToString()), "status");
                form.Add(new StringContent(submenu.Submodule.SubmoduleID.ToString()), "submoduleID");


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

        public static async Task DeleteSubmenu(Submenu submenu)
        {
            try
            {
                string url = string.Format("{0}/deleteSubmenu", Global.apiUrl);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(submenu.SubmenuID.ToString()), "submenuID");

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
