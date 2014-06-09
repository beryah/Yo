using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace TelerikTest
{
    public class ApiClient
    {
        private string uriString = @"http://localhost:19672/";

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string uri, TRequest request)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(this.uriString);

                try
                {
                    var response = await httpClient.PostAsJsonAsync<TRequest>(uri, request);

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsAsync<TResponse>();
                    }
                    else
                    {
                        var message = string.Format(@"{0}/{1}", (int)response.StatusCode, response.ReasonPhrase);
                        throw new Exception(string.Format("Call API Failed:{0}", message));
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
