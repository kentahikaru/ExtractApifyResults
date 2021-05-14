using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using ExtractApifyResults.Interfaces;

namespace ExtractApifyResults.Services
{
    public class Transport : ITransport
    {
        private readonly ILogger _logger;
        public Transport(ILogger<Transport> logger)
        {
            _logger = logger;
        }

        public async Task<MemoryStream> GetDataStream(string url)
        {
            MemoryStream memStream = new MemoryStream();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage response = await client.GetAsync(url))
                    {
                        await response.Content.CopyToAsync(memStream);
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Http request failed");
            }

            return memStream;
        }

        public async Task<string> GetDataString(string url)
        {
            string data = "";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage response = await client.GetAsync(url))
                    {
                        data = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Http request failed");
            }

            return data;
        }
    }
}