using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Runtime.InteropServices;

namespace ApiClient
{
    [ComVisible(true)]
    [Guid("A4F02ED9-69C7-45B9-A00C-54D4A7CD842F")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IApiClient
    {
        [DispId(1)]
        string GetApiResponse(string baseAddress, string endpoint, string token);
    }

    [ComVisible(true)]
    [Guid("8C55159E-7E42-47C6-958F-D1EDA6D446D9")]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("ApiClient")]
    public class ApiClientAsync : IApiClient
    {
        public string GetApiResponse(string baseAddress, string endpoint, string token)
        {
            try
            {
                var _httpClient = new HttpClient{BaseAddress = new Uri(baseAddress)};
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                HttpResponseMessage response = _httpClient.GetAsync(endpoint).Result;
                response.EnsureSuccessStatusCode(); // Lança exceção se a resposta não for bem-sucedida
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (HttpRequestException ex)
            {
                // Lidar com erros de conexão, timeouts, etc.
                return $"Error: {ex.Message}";
            }
        }
    }
}
