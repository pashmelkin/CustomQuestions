using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CustomQuestions.Framework
{
    public interface IHttpClientFactory
    {
        HttpClient CreateClient();
    }

    public class HttpClientFactory : IHttpClientFactory
    {
        static string baseAddress = "http://localhost:64941/";

        public static string BaseAddress { get => baseAddress; set => baseAddress = value; }

        public HttpClient CreateClient()
        {
            var client = new HttpClient();
            SetupClientDefaults(client);
            return client;
        }

        protected virtual void SetupClientDefaults(HttpClient client)
        {
            client.Timeout = TimeSpan.FromSeconds(30);
            client.BaseAddress = new Uri(BaseAddress);
            client.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }

}
