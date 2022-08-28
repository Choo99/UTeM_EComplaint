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
    internal class ModuleServices
    {
        public static async Task<List<Module>> GetAllModules()
        {
            try
            {
                string url = string.Format("{0}/getAllModules", Global.apiUrl);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string resultString = await response.Content.ReadAsStringAsync();
                    List<Module> result = JsonConvert.DeserializeObject<List<Module>>(resultString);
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

        public static async Task<List<Module>> GetAvailableModuleBySystem(Module module)
        {
            try
            {
                string url = string.Format("{0}/GetAvailableModulesBySystem?softwareSystemID={1}", Global.apiUrl,module.System.SoftwareSystemID.ToString());
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string resultString = await response.Content.ReadAsStringAsync();
                    List<Module> result = JsonConvert.DeserializeObject<List<Module>>(resultString);
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

        public static async Task<List<Module>> GetModulesBySystem(Module module)
        {
            try
            {
                string url = string.Format("{0}/getModulesBySystem?softwareSystemID={1}", Global.apiUrl,module.System.SoftwareSystemID.ToString());
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string resultString = await response.Content.ReadAsStringAsync();
                    List<Module> result = JsonConvert.DeserializeObject<List<Module>>(resultString);
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

        public static async Task EditModule(Module module)
        {
            try
            {
                string url = string.Format("{0}/editModule", Global.apiUrl);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(module.ModuleID.ToString()), "moduleID");
                form.Add(new StringContent(module.ModuleCode), "moduleCode");
                form.Add(new StringContent(module.ModuleName), "moduleName");
                form.Add(new StringContent(module.Status.ToString()), "status");

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

        public static async Task AddModule(Module module)
        {
            try
            {
                string url = string.Format("{0}/addModule", Global.apiUrl);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(module.ModuleCode), "moduleCode");
                form.Add(new StringContent(module.ModuleName), "moduleName");
                form.Add(new StringContent(module.Status.ToString()), "status");
                form.Add(new StringContent(module.System.SoftwareSystemID.ToString()), "softwareSystemID");

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

        public static async Task DeleteModule(Module module)
        {
            try
            {
                string url = string.Format("{0}/deleteModule", Global.apiUrl);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(module.ModuleID.ToString()), "moduleID");

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
