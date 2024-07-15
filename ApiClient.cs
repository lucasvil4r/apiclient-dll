using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ApiClient
{
    [ComVisible(true)]
    [Guid("A4F02ED9-69C7-45B9-A00C-54D4A7CD842F")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IApiClient
    {
        [DispId(1)]
        string GetApiResponse(string baseAddress, string endpoint);
    }

    [ComVisible(true)]
    [Guid("8C55159E-7E42-47C6-958F-D1EDA6D446D9")]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("ApiClient")]
    public class ApiClientAsync : IApiClient
    {
        public string GetApiResponse(string baseAddress, string endpoint)
        {
            var headers = new Dictionary<string, string>
            {
                { "Authorization", "Bearer <token>" }
            };
            Task<string> task = GetApiResponseAsync(baseAddress, endpoint, headers);
            return task.Result;
        }

        public async Task<string> GetApiResponseAsync(string baseAddress, string endpoint, Dictionary<string, string> headers = null)
        {
            try
            {
                var _httpClient = new HttpClient
                {
                    BaseAddress = new Uri(baseAddress)
                };
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Adicionar headers, se fornecidos
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }

                HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode(); // Lança exceção se a resposta não for bem-sucedida

                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                // Lidar com erros de conexão, timeouts, etc.
                return $"Error: {ex.Message}";
            }
        }
    }
}
