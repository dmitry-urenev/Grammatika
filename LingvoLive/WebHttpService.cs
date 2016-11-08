using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.IO;
using Newtonsoft.Json;

namespace LingvoLive
{
    public abstract class WebHttpService
    {
        public WebHttpService(string baseUrl)
        {
            BaseAddress = baseUrl;
        }

        public enum RequestType { GET, POST }

        public string BaseAddress { get; protected set; }

        public async Task<T> MakeRequest<T>(string endpointUrl, IEnumerable<KeyValuePair<string, string>> queryParams,
            RequestType requestType = RequestType.GET)
        {
            using (HttpClient hc = new HttpClient())
            {
                HttpResponseMessage response;

                UriBuilder urlBuilder = new UriBuilder(BaseAddress);
                urlBuilder.Fragment = endpointUrl;

                switch (requestType)
                {
                    case RequestType.POST:
                        var content = new FormUrlEncodedContent(queryParams);
                        response = await hc.PostAsync(urlBuilder.ToString(), content);
                        break;

                    default:
                        urlBuilder.Query = string.Join("&",
                                queryParams.Where(p => !string.IsNullOrWhiteSpace(p.Key))
                                    .Select(p => string.Format("{0}={1}", WebUtility.UrlEncode(p.Key), WebUtility.UrlEncode(p.Value))));

                        response = await hc.GetAsync(urlBuilder.ToString());
                        break;
                }

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return await Task.Factory.StartNew<T>(() =>
                    {
                        return JsonConvert.DeserializeObject<T>(content);
                    });
                }
                return default(T);
            }
        }
    }
}
