using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenWeatherWindows.Core
{
    public static class NetClient
    {
        public static string GetResponseJson(string request)
        {
            var t = Task.Run(() => GetAsyncResponse(new Uri(request)));
            t.Wait();

            return t.Result;
        }

        private static async Task<string> GetAsyncResponse(Uri request)
        {
            var response = string.Empty;

            using (var client = new HttpClient())
            {
                HttpResponseMessage result = await client.GetAsync(request);

                if (result.IsSuccessStatusCode)
                {
                    return await result.Content.ReadAsStringAsync();
                }
            }

            return response;
        }
    }
}
