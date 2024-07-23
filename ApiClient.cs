using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;

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
            var process = new Process();
            var apiClientPath = Environment.GetEnvironmentVariable("ApiClient");

            if (string.IsNullOrEmpty(apiClientPath))
            {
                return ("A variável de ambiente 'ApiClient' não está definida.");
            }

            if (!File.Exists(apiClientPath))
            {
                return ($"O arquivo especificado em 'ApiClient' não foi encontrado: {apiClientPath}");
            }

            process.StartInfo.FileName = apiClientPath;
            process.StartInfo.Arguments = $"{baseAddress} {endpoint} {token} {tempFilePath}";
            process.Start();
            process.WaitForExit(); // Aguarda até que o processo termine

            try
            {
                // Lê todo o conteúdo do arquivo e armazena em uma string
                string fileContent = File.ReadAllText(tempFilePath);
                return fileContent;
            }
            catch (Exception ex)
            {
                // Em caso de erro, retorna a mensagem de erro
                return $"Erro ao ler o arquivo: {ex.Message}";
            }
        }
    }
}
