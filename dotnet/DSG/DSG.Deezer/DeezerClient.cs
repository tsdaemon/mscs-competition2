using System;
using System.IO;
using System.Net;
using DSC.Core.Readers;
using DSG.Deezer.Exceptions;
using DSG.Deezer.Models;
using Polly;

namespace DSG.Deezer
{
    public class DeezerClient
    {
        private const string API_URL = "https://api.deezer.com/";

        private Policy policy => 
                Policy.Handle<WebException>()
                    .Or<DeezerWaitException>()
                    .WaitAndRetry(new[]{
                        new TimeSpan(0,0,10),
                        new TimeSpan(0,1,0),
                        new TimeSpan(0,5,0),
                        new TimeSpan(0,10,0),
                        new TimeSpan(0,10,0)
                    });

        public Track GetTrack(int id)
        {
            var url = $"{API_URL}track/{id}";
            return policy.ExecuteAndCapture(() => GetRequest<Track>(url)).Result;
        }

        private T GetRequest<T>(string url) where T: class,new()
        {
            var request = WebRequest.CreateHttp(url);
            using (var rdr = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                var str = rdr.ReadToEnd();
                if (str.Contains("error"))
                {
                    var error = JsonReaderInner.DeserializeDeezer<ErrorResponse>(str);
                    if (error.Error != null)
                    {
                        switch (error.Error.Code)
                        {
                            //http://developers.deezer.com/api/errors
                            case 4:
                            case 100:
                            case 700:
                                throw new DeezerWaitException();
                            case 800:
                            case 600:
                            case 501:
                            case 500:
                            case 200:
                            case 300:
                                return null;
                        }

                    }
                }
                return JsonReaderInner.DeserializeDeezer<T>(str);
            }
        }
    }
}
