using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ApiClient.Core.Models;

namespace ApiClient.Core.Services
{
    public sealed class HttpApiRequestService : IApiRequestService
    {
        private readonly HttpMessageHandler _handler;

        public HttpApiRequestService()
        {
        }

        public HttpApiRequestService(HttpMessageHandler handler)
        {
            _handler = handler;
        }

        public async Task<ApiResponse> GetAsync(ApiRequestOptions request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            try
            {
                using (var httpClient = CreateHttpClient(request.BaseAddress))
                {
                    ConfigureHeaders(httpClient, request.Token);

                    var response = await httpClient.GetAsync(request.Endpoint).ConfigureAwait(false);
                    var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    if (!response.IsSuccessStatusCode)
                    {
                        return ApiResponse.Fail(
                            string.Format(
                                "HttpRequest Error: StatusCode={0}; ReasonPhrase={1}; Content={2}",
                                (int)response.StatusCode,
                                response.ReasonPhrase,
                                content));
                    }

                    return ApiResponse.Ok(content);
                }
            }
            catch (HttpRequestException ex)
            {
                var innerException = ex.InnerException == null ? "No inner exception." : ex.InnerException.ToString();
                return ApiResponse.Fail(
                    string.Format(
                        "HttpRequest Error: {0}{1}Inner Exception: {2}",
                        ex.Message,
                        Environment.NewLine,
                        innerException));
            }
            catch (Exception ex)
            {
                return ApiResponse.Fail(string.Format("General Error: {0}", ex.Message));
            }
        }

        private HttpClient CreateHttpClient(string baseAddress)
        {
            return _handler == null
                ? new HttpClient { BaseAddress = new Uri(baseAddress) }
                : new HttpClient(_handler, false) { BaseAddress = new Uri(baseAddress) };
        }

        private static void ConfigureHeaders(HttpClient httpClient, string token)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrWhiteSpace(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}
