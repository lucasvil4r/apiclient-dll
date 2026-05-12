using ApiClient.Core.Models;
using ApiClient.Core.Services;

public class Program
{
    public static int Main(string[] args)
    {
        Console.WriteLine("Consultando API...");

        if (args.Length < 4)
        {
            Console.WriteLine("Informe: BaseAddress Endpoint Token TempFilePath");
            return 1;
        }

        string baseAddress = args[0];
        string endpoint = args[1];
        string token = args[2];
        string tempFilePath = args[3];

        string json = RequestGetAsyncApi(baseAddress, endpoint, token);
        GetApiResponseFile(tempFilePath, json);

        return 0;
    }

    public static string RequestGetAsyncApi(string baseAddress, string endpoint, string token)
    {
        try
        {
            var request = new ApiRequestOptions(baseAddress, endpoint, token);
            var response = new HttpApiRequestService().GetAsync(request).GetAwaiter().GetResult();
            return response.ResultText;
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }

    public static string GetApiResponseFile(string tempFilePath, string json)
    {
        try
        {
            return new FileApiResponseWriter().Write(tempFilePath, json);
        }
        catch (Exception ex)
        {
            File.WriteAllText(tempFilePath, $"Error: {ex.Message}");
            return $"Error: {ex.Message}";
        }
    }
}
