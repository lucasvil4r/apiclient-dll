using System.Net.Http.Headers;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Consultando API...");
        if (args.Length >= 4)
        {
            string baseAddress = args[0];
            string endpoint = args[1];
            string token = args[2];
            string tempFilePath = args[3];

            string json = RequestGetAsyncApi(baseAddress, endpoint, token);
            GetApiResponseFile(tempFilePath, json);
        }
    }

    public static string RequestGetAsyncApi(string baseAddress, string endpoint, string token)
    {
        try
        {
            var _httpClient = new HttpClient { BaseAddress = new Uri(baseAddress) };
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
    public static string GetApiResponseFile(string tempFilePath, string json)
    {
        try
        {
            // Write the JSON data to the temporary file
            File.WriteAllText(tempFilePath, json);

            // Return the path to the temporary file
            return tempFilePath;
        }
        catch (Exception ex)
        {
            // Handle any exceptions (e.g., permission issues, disk full, etc.)
            File.WriteAllText(tempFilePath, $"Error: {ex.Message}");
            return ($"Error: {ex.Message}");
        }
    }
}
