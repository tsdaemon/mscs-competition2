using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using DSC.Core.Readers;
using DSG.Deezer.Models;
using Newtonsoft.Json;
using Polly;

namespace DSG.Deezer
{
    public class DeezerClient
    {
        private const string API_URL = "https://api.deezer.com/";

        private Policy policy =>
                Policy.Handle<WebException>()
                    .WaitAndRetryForeverAsync(retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        public async Task<Track> GetTrack(int id)
        {
            var url = $"{API_URL}track/{id}";
            return (await policy.ExecuteAndCaptureAsync(() => GetRequest<Track>(url))).Result;
        }

        private async Task<T> GetRequest<T>(string url) where T: new()
        {
            var request = WebRequest.CreateHttp(url);
            using (var rdr = new StreamReader((await request.GetResponseAsync()).GetResponseStream()))
            {
                var str = await rdr.ReadToEndAsync();
                return JsonReaderInner.DeserializeDeezer<T>(str);
            }
        }
    }
}
