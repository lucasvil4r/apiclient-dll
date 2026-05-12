using System;

namespace ApiClient.Core.Models
{
    public sealed class ApiRequestOptions
    {
        public ApiRequestOptions(string baseAddress, string endpoint, string token)
        {
            if (string.IsNullOrWhiteSpace(baseAddress))
            {
                throw new ArgumentException("Base address is required.", nameof(baseAddress));
            }

            if (string.IsNullOrWhiteSpace(endpoint))
            {
                throw new ArgumentException("Endpoint is required.", nameof(endpoint));
            }

            BaseAddress = baseAddress;
            Endpoint = endpoint;
            Token = token ?? string.Empty;
        }

        public string BaseAddress { get; }

        public string Endpoint { get; }

        public string Token { get; }
    }
}
