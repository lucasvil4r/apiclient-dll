using System;
using System.Runtime.InteropServices;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiClient
{
    [ComVisible(true)]
    [Guid("A4F02ED9-69C7-45B9-A00C-54D4A7CD842F")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IApiClient
    {
        [DispId(1)]
        string RequestGetAsyncApi(string baseAddress, string endpoint, string token);
    }

    [ComVisible(true)]
    [Guid("8C55159E-7E42-47C6-958F-D1EDA6D446D9")]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("ApiClient")]
    public class ApiClientAsync : IApiClient
    {
        public string RequestGetAsyncApi(string baseAddress, string endpoint, string token)
        {
            try
            {
                // Chama o método assíncrono e espera o resultado.
                return RequestGetAsync(baseAddress, endpoint, token).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                // Retorna a mensagem de erro detalhada.
                return $"General Error: {ex.Message}\nStackTrace: {ex.StackTrace}";
            }
        }

        private async Task<string> RequestGetAsync(string baseAddress, string endpoint, string token)
        {
            try
            {
                using (var _httpClient = new HttpClient { BaseAddress = new Uri(baseAddress) })
                {
                    _httpClient.DefaultRequestHeaders.Accept.Clear();
                    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                    var response = await _httpClient.GetAsync(endpoint);
                    response.EnsureSuccessStatusCode(); // Lança exceção se a resposta não for bem-sucedida
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (HttpRequestException ex)
            {
                var innerEx = ex.InnerException != null ? ex.InnerException.ToString() : "No inner exception.";
                return $"HttpRequest Error: {ex.Message}\nInner Exception: {innerEx}\nStackTrace: {ex.StackTrace}";
            }
            catch (Exception ex)
            {
                // Captura erros gerais.
                return $"General Error: {ex.Message}";
            }
        }
    }
}
