using System.Threading.Tasks;
using ApiClient.Core.Models;

namespace ApiClient.Core.Services
{
    public interface IApiRequestService
    {
        Task<ApiResponse> GetAsync(ApiRequestOptions request);
    }
}
