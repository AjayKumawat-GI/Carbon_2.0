using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Carbon.Mediator.Infrastructure
{
    public interface IApiClient
    {
        Task<dynamic> CallApiAsync(string baseUrl, HttpMethod httpMethod, string endpoint, AuthenticationHeaderValue authHeader = null, IDictionary<string, string> headers = null, object requestBody = null, IDictionary<string, string> parameters = null);

        Task<string> CallApiAsync(string baseUrl, string endpoint, HttpMethod httpMethod, AuthenticationHeaderValue authHeader = null, IDictionary<string, string> headers = null, object requestBody = null, IDictionary<string, string> parameters = null, bool returnJson = true);

        Task<TResponse> CallApiAsync<TResponse>(string baseUrl, string endpoint, HttpMethod httpMethod, AuthenticationHeaderValue authHeader = null, IDictionary<string, string> headers = null, object requestBody = null, IDictionary<string, string> parameters = null);
    }
}
