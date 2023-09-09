using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MysteriousTools.WebManager
{
    public interface IHttpService
    {
        Task<string> GetAsync(string uri, IEnumerable<KeyValuePair<string, string>> headers = null, int timeout = -1);
        Task<string> PostAsync(string uri, Dictionary<string, string> data, IEnumerable<KeyValuePair<string, string>> headers = null, int timeout = -1);
    }
    public class Api : IHttpService, IDisposable
    {
        public Api()
        {
        }

        private static Api _instance;

        public static Api Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Api();
                }
                return _instance;
            }
        }
        //Örnek Kullanım
        /*
         * Get Method
         * string url = "https://example.com/api/data"; // İstek yapılacak URL
           string response = await MyWebRequest.Instance.GetAsync(url);
           Console.WriteLine(response);
        //Post Methot

        string url = "https://example.com/api/data"; // İstek yapılacak URL
        Dictionary<string, string> postData = new Dictionary<string, string>()
        {
            { "param1", "value1" },
            { "param2", "value2" }
        };

        string response = await MyWebRequest.Instance.PostAsync(url, postData);
        Console.WriteLine(response);
        */
        public string UserAgent { get; set; }

        private enum HttpMethod
        {
            Get,
            Post
        }
        private async Task<string> SendAsync(HttpMethod method, string uri, Dictionary<string, string> data = null, IEnumerable<KeyValuePair<string, string>> headers = null, int timeout = -1)
        {
            headers = headers ?? Enumerable.Empty<KeyValuePair<string, string>>(); // headers, eğer null ise boş bir IEnumerable olarak atanır.

            Uri baseUri = new Uri(uri);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUri.ToString()); // HTTP isteği oluşturulur ve URL belirlenir.
            request.Method = method.ToString(); // HTTP isteği tipi belirlenir (GET, POST, vb.).
            foreach (var header in headers) // headers üzerinde döngü oluşturulur ve istek başlıklarına eklenir.
            {
                request.Headers.Add(header.Key, header.Value);
            }

            request.UserAgent = UserAgent; // İstek için kullanılacak user-agent belirlenir.
            if (timeout > 0)
            {
                request.Timeout = timeout; // Timeout değeri belirlenir.
            }
            try
            {
                if (method == HttpMethod.Get) // Eğer istek tipi GET ise,
                {
                    using (var response = await request.GetResponseAsync()) // HTTP isteği gönderilir ve cevap alınır.
                    using (var reader = new StreamReader(response.GetResponseStream())) // Cevap stream'i okunabilir hale getirilir.
                    {
                        return await reader.ReadToEndAsync(); // Stream sonuna kadar okuma yapılır ve string olarak döndürülür.
                    }
                }
                else if (method == HttpMethod.Post) // Eğer istek tipi POST ise,
                {
                    request.ContentType = "application/x-www-form-urlencoded"; // İstek tipi belirlenir.

                    var postData = data != null
                        ? string.Join("&", data.Select(kv => $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}")) // POST verileri URL kodlanır ve & ile birleştirilir.
                        : "";

                    var encoding = Encoding.UTF8; // Karakter kodlaması UTF8 olarak belirlenir.
                    var bytes = encoding.GetBytes(postData); // POST verileri byte dizisine dönüştürülür.
                    request.ContentLength = bytes.Length; // POST verilerinin uzunluğu belirlenir.

                    using (var stream = await request.GetRequestStreamAsync()) // HTTP isteği için bir stream oluşturulur.
                    {
                        await stream.WriteAsync(bytes, 0, bytes.Length); // POST verileri stream'e yazılır.
                    }

                    using (var response = await request.GetResponseAsync()) // HTTP isteği gönderilir ve cevap alınır.
                    using (var reader = new StreamReader(response.GetResponseStream())) // Cevap stream'i okunabilir hale getirilir.
                    {
                        return await reader.ReadToEndAsync(); // Stream sonuna kadar okuma yapılır ve string olarak döndürülür.
                    }
                }
                else // Eğer istek tipi ne GET ne de POST ise,
                {
                    throw new NotSupportedException($"The HTTP method '{method}' is not supported."); // İstek tipi desteklenmiyorsa hata fırlatılır.
                }
            }
            catch (WebException ex)
            {
                throw new WebException($"An error occurred while sending a {method} request: {ex.Message}");
            }
            finally
            {
                Dispose();
            }
        }
        public async Task<string> GetAsync(string uri, IEnumerable<KeyValuePair<string, string>> headers = null, int timeout = -1)
        {
            return await SendAsync(HttpMethod.Get, uri, headers: headers, timeout: timeout);
        }

        public async Task<string> PostAsync(string uri, Dictionary<string, string> data, IEnumerable<KeyValuePair<string, string>> headers = null, int timeout = -1)
        {
            return await SendAsync(HttpMethod.Post, uri, data, headers, timeout: timeout);
        }
        public void Dispose()
        {
        }
    }
}
