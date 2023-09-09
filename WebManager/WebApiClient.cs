using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace MysteriousTools.WebManager
{
    public interface IWebApiClient
    {
        Task<string> GetAsync(string url);
    }

    public class ApiWebClient : IWebApiClient
    {
        private readonly WebClient client;
        public ApiWebClient()
        {
            client = new WebClient();
        }

        private static WebClient _instance;

        public static WebClient Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new WebClient();
                }
                return _instance;
            }
        }

        public async Task<string> GetAsync(string url)
        {
            try
            {
                return await client.DownloadStringTaskAsync(url);
            }
            catch (WebException ex)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        return reader.ReadToEnd();
                    }
                }
                else
                {
                    return ex.Message;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
