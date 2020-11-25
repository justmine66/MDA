using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MDA.FunctionalTest
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> PostAsync<TArg>(this HttpClient client, string path, TArg arg)
        {
            var content = JsonConvert.SerializeObject(arg);
            using var request = new HttpRequestMessage(HttpMethod.Post, path)
            {
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            };

            return await client.SendAsync(request);
        }

        public static async Task<TResult> GetAsync<TResult>(this HttpClient client, string path)
        {
            var response = await client.GetAsync(path);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TResult>(content);
        }
    }
}
