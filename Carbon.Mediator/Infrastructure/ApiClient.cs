using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Referral.Mediator.Infrastructure
{
    public class ApiClient : IApiClient
    {
        
        //NEW
        public async Task<dynamic> CallApiAsync(string baseUrl, HttpMethod httpMethod, string endpoint, AuthenticationHeaderValue authHeader = null, object requestBody = null, IDictionary<string, string> parameters = null)
        {
            using (HttpClient client = new HttpClient())
            {
                // Set the base address of the API
                client.BaseAddress = new Uri(baseUrl);

                if(authHeader != null)
                {
                    // Add the authorization header if an auth token is provided
                    client.DefaultRequestHeaders.Authorization = authHeader;
                }

                // Add the query parameters to the request URL
                endpoint += GetQueryParams(parameters);

                // Create the HTTP request
                HttpRequestMessage request = new HttpRequestMessage(httpMethod, endpoint);

                // Add the request body if provided
                if (requestBody != null)
                {
                    request.Content = ConvertToJson(requestBody);
                    //request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                }

                //Setting User Agent
                request.Headers.UserAgent.Add(new ProductInfoHeaderValue("Referral", "1.0"));

                // Send the request and get the response
                HttpResponseMessage response = await client.SendAsync(request);

                // Deserialize the response body into the specified type
                string responseBody = await response.Content.ReadAsStringAsync();

                // Return the deserialized response data
                return responseBody;
            }
        }

        public async Task<string> CallApiAsync(string baseUrl, string endpoint, HttpMethod httpMethod, AuthenticationHeaderValue authHeader = null, object requestBody = null, IDictionary<string, string> parameters = null, bool returnJson = true)
        {
            using (HttpClient client = new HttpClient())
            {
                // Set the base address of the API
                client.BaseAddress = new Uri(baseUrl);

                if (authHeader != null)
                {
                    // Add the authorization header if an auth token is provided
                    client.DefaultRequestHeaders.Authorization = authHeader;
                }

                // Add the query parameters to the request URL
                endpoint += GetQueryParams(parameters);

                // Create the HTTP request
                HttpRequestMessage request = new HttpRequestMessage(httpMethod, endpoint);

                // Add the request body if provided
                if (requestBody != null)
                {
                    request.Content = ConvertToJson(requestBody);
                    //request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                }

                //Setting User Agent
                request.Headers.UserAgent.Add(new ProductInfoHeaderValue("Referral", "1.0"));

                // Send the request and get the response
                HttpResponseMessage response = await client.SendAsync(request);

                // Deserialize the response body into the specified type
                string responseBody = await response.Content.ReadAsStringAsync();

                // Return the deserialized response data
                return responseBody;
            }
        }

        public async Task<TResponse> CallApiAsync<TResponse>(string baseUrl, string endpoint, HttpMethod httpMethod, AuthenticationHeaderValue authHeader = null, object requestBody = null, IDictionary<string, string> parameters = null)
        {
            using (HttpClient client = new HttpClient())
            {
                // Set the base address of the API
                client.BaseAddress = new Uri(baseUrl);

                if (authHeader != null)
                {
                    // Add the authorization header if an auth token is provided
                    client.DefaultRequestHeaders.Authorization = authHeader;
                }

                // Add the query parameters to the request URL
                endpoint += GetQueryParams(parameters);

                // Create the HTTP request
                HttpRequestMessage request = new HttpRequestMessage(httpMethod, endpoint);

                // Add the request body if provided
                if (requestBody != null)
                {
                    request.Content = ConvertToJson(requestBody);
                    //request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                }

                //Setting User Agent
                request.Headers.UserAgent.Add(new ProductInfoHeaderValue("Referral", "1.0"));

                // Send the request and get the response
                HttpResponseMessage response = await client.SendAsync(request);

                // Deserialize the response body into the specified type
                string responseBody = await response.Content.ReadAsStringAsync();
                TResponse responseData = JsonConvert.DeserializeObject<TResponse>(responseBody);

                // Return the deserialized response data
                return responseData;
            }
        }

        private StringContent ConvertToJson(dynamic request)
        {
            return new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        }

        private string GetQueryParams(IDictionary<string, string> parameters)
        {
            var queryParams = "";
            if (parameters != null && parameters.Any())
            {
                queryParams = "?" + string.Join("&", parameters.Select(x => x.Key + "=" + HttpUtility.UrlEncode(x.Value)));
            }
            return queryParams;
        }
    }
}
