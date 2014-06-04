using MSC.Phone.Shared.Contracts.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;


namespace MSC.Phone.Shared
{
    public class HttpClientService : IHttpClientService
    {
        public async Task<string> GetRawAsync(string url, CancellationToken cancellationToken)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.AutomaticDecompression = System.Net.DecompressionMethods.GZip;
            var client = new HttpClient(httpClientHandler);
            var response = await client.GetAsync(url, cancellationToken).ConfigureAwait(false);
            if (response != null && (
                response.StatusCode == System.Net.HttpStatusCode.OK))
                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            else
                return null;
        }

        public async Task<T> GetJsonAsync<T>(string url, CancellationToken cancellationToken)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.AutomaticDecompression = System.Net.DecompressionMethods.GZip;
            var client = new HttpClient(httpClientHandler);
            var response = await client.GetAsync(url, cancellationToken).ConfigureAwait(false);
            if (response != null && (
                response.StatusCode == System.Net.HttpStatusCode.OK))
            {
                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                try
                {
                    return JsonConvert.DeserializeObject<T>(responseContent);
                }
                catch(Exception xcp)
                {

                }
            }
            return default(T);
        }

        public async Task<T> GetXmlAsync<T>(string url, CancellationToken cancellationToken)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.AutomaticDecompression = System.Net.DecompressionMethods.GZip;
            var client = new HttpClient(httpClientHandler);
            var response = await client.GetAsync(url, cancellationToken).ConfigureAwait(false);
            if (response != null && (
                response.StatusCode == System.Net.HttpStatusCode.OK))
            {
                var serializer = new XmlSerializer(typeof(T));

                using (var stringReader = new StringReader(await response.Content.ReadAsStringAsync().ConfigureAwait(false)))
                {
                    var xmlReader = XmlReader.Create(stringReader);
                    return (T)serializer.Deserialize(xmlReader);
                }
            }
            else
            {
                return default(T);
            }
        }
    }
}
