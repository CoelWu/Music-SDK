using Music.SDK.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Music.SDK.Provider
{
    public class Music
    {
        internal HttpClient httpClient;
        internal HeaderHacker headerHacker;
        internal LrcTools lrcTools;

        public Music()
        {
            httpClient = new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            });
            headerHacker = new HeaderHacker();
            lrcTools = new LrcTools();
        }

        public long ToLong(string data)
        {
            long result;
            if (!long.TryParse(data, out result))
            {
                result = 0;
            }
            return result;
        }

        internal HttpContent ConvertToHttpContent(object postData = null)
        {
            string json = JsonConvert.SerializeObject(postData);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            return httpContent;
        }

        internal HttpContent ConvertToHttpContent(Dictionary<string, string> postData = null)
        {
            string json = JsonConvert.SerializeObject(postData);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            return httpContent;
        }
    }
}
