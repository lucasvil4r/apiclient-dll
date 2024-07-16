using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.IO;

namespace ApiClient
{
    [ComVisible(true)]
    [Guid("A4F02ED9-69C7-45B9-A00C-54D4A7CD842F")]
    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
    public interface IApiClient
    {
        [DispId(1)]
        string RequestRunExecutableApi(string baseAddress, string endpoint, string token);
    }

    [ComVisible(true)]
    [Guid("8C55159E-7E42-47C6-958F-D1EDA6D446D9")]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("ApiClient")]
    public class ApiClientAsync : IApiClient
    {
        public string RequestRunExecutableApi(string baseAddress, string endpoint, string token)
        {
            // Get the temporary directory path
            string tempDir = Path.GetTempPath();

            // Create a unique file name for the temporary file
            string tempFileName = $"api_response_{Guid.NewGuid()}.json";

            // Combine the directory and file name to get the full path
            string tempFilePath = Path.Combine(tempDir, tempFileName);

            // Execute a aplicação de console
            System.Diagnostics.Process.Start(Environment.GetEnvironmentVariable("ApiClient"), $"{baseAddress} {endpoint} {token} {tempFilePath}");

            return tempFilePath;
        }
    }
}
