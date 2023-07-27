namespace MI.Platform.Api.Extensions
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class HttpRequestExtensions
    {
        /// <summary>
        /// Returns the deserialized request body.
        /// </summary>
        /// <typeparam name="T">Type used for deserialization of the request body.</typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public static async Task<T> GetJsonBodyAsync<T>(this HttpRequest request) =>
            JsonConvert.DeserializeObject<T>(await request.ReadAsStringAsync());

        /// <summary>
        /// Returns query string params as Dictionary<string,string>
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Dictionary<string, string> QueryToDictionary(this HttpRequest request) =>
            request.Query.Keys.Cast<string>().ToDictionary(k => k, v => (string)request.Query[v]);
    }
}