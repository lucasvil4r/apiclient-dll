using System;
using System.Runtime.InteropServices;
using ApiClient.Core.Models;
using ApiClient.Core.Services;

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
        private readonly IApiRequestService _requestService;

        public ApiClientAsync()
            : this(new HttpApiRequestService())
        {
        }

        internal ApiClientAsync(IApiRequestService requestService)
        {
            _requestService = requestService;
        }

        public string RequestGetAsyncApi(string baseAddress, string endpoint, string token)
        {
            try
            {
                var request = new ApiRequestOptions(baseAddress, endpoint, token);
                var response = _requestService.GetAsync(request).GetAwaiter().GetResult();
                return response.ResultText;
            }
            catch (Exception ex)
            {
                return $"General Error: {ex.Message}\nStackTrace: {ex.StackTrace}";
            }
        }
    }
}
