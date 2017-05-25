using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs.Host;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace FunctionsLibraryProject
{
    public class BlobOcrTrigger
    {
        public static string Run(Stream myBlob, string name, TraceWriter log)
        {
            log.Info($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            var result = CallVisionAPI(myBlob);

            var resultObject = JsonConvert.DeserializeObject<OcrResult>(result.Result);

            var listOfBooks = new List<string>();

            foreach(var region in resultObject.regions)
            {
                foreach (var line in region.lines)
                {
                    var keywords = string.Join("+", line.words.Select(x => x.text));

                    var lookup = CallGoogleBooksAPI(keywords);

                    var lookupObject = JsonConvert.DeserializeObject<GoogleBooksResult>(lookup.Result);

                    if (lookupObject.items != null)
                    {
                        listOfBooks.Add(lookupObject.items.First().volumeInfo.title);
                    }
                }
            }

            return string.Join("/n/r ", listOfBooks);
        }

        static async Task<string> CallVisionAPI(Stream image)
        {
            using (var client = new HttpClient())
            {
                var content = new StreamContent(image);
                var url = "https://westus.api.cognitive.microsoft.com/vision/v1.0/ocr";
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Environment.GetEnvironmentVariable("Vision_API_Subscription_Key"));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                var httpResponse = await client.PostAsync(url, content);
 
                if (httpResponse.StatusCode == HttpStatusCode.OK){
                    return await httpResponse.Content.ReadAsStringAsync();
                }
            }
            return null;
        }

        static async Task<string> CallGoogleBooksAPI(string keywords)
        {
            using (var client = new HttpClient())
            {
                var url = "https://www.googleapis.com/books/v1/volumes?q=" + keywords;
                var httpResponse = await client.GetAsync(url);
 
                if (httpResponse.StatusCode == HttpStatusCode.OK){
                    return await httpResponse.Content.ReadAsStringAsync();
                }
            }
            return null;
        }
    }
}