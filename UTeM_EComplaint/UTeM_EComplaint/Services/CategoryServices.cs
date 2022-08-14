﻿using Newtonsoft.Json;
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
    internal class CategoryServices
    {
        public static async Task<List<Category>> GetCategories()
        {
            try
            {
                string url = string.Format("{0}/getCategories", Global.apiUrl);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string resultString = await response.Content.ReadAsStringAsync();
                    List<Category> result = JsonConvert.DeserializeObject<List<Category>>(resultString);
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

        public static async Task<int> EditCategory(Category category)
        {
            try
            {
                string url = string.Format("{0}/editCategory", Global.apiUrl);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                
                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(category.CategoryId.ToString()), "categoryID");
                form.Add(new StringContent(category.CategoryName), "categoryName");

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

        public static async Task<int> AddCategory(Category category)
        {
            try
            {
                string url = string.Format("{0}/addCategory", Global.apiUrl);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(category.CategoryName), "categoryName");

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

        public static async Task<int> DeleteCategory(Category category)
        {
            try
            {
                string url = string.Format("{0}/deleteCategory", Global.apiUrl);
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(category.CategoryId.ToString()), "categoryID");

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
