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
    internal class DamageTypeServices
    {
        public static async Task<List<DamageType>> GetDamageTypes()
        {
            try
            {
                string url = string.Format("{0}/getDamageTypes", Global.apiUrl);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string resultString = await response.Content.ReadAsStringAsync();
                    List<DamageType> result = JsonConvert.DeserializeObject<List<DamageType>>(resultString);
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

        public static async Task<int> EditDamageType(DamageType damageType)
        {
            try
            {
                string url = string.Format("{0}/editDamageType", Global.apiUrl);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(damageType.DamageTypeID.ToString()), "damageTypeID");
                form.Add(new StringContent(damageType.DamageTypeName), "damageTypeName");

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
        }

        public static async Task<int> AddDamageType(DamageType damageType)
        {
            try
            {
                string url = string.Format("{0}/addDamageType", Global.apiUrl);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(damageType.DamageTypeName), "damageTypeName");

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
        }

        public static async Task<int> DeleteDamageType(DamageType damageType)
        {
            try
            {
                string url = string.Format("{0}/deleteDamageType", Global.apiUrl);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(damageType.DamageTypeID.ToString()), "damageTypeID");

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
        }
    }
}
