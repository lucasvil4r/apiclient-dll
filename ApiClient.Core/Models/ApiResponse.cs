namespace ApiClient.Core.Models
{
    public sealed class ApiResponse
    {
        private ApiResponse(bool success, string content, string errorMessage)
        {
            Success = success;
            Content = content ?? string.Empty;
            ErrorMessage = errorMessage ?? string.Empty;
        }

        public bool Success { get; }

        public string Content { get; }

        public string ErrorMessage { get; }

        public string ResultText
        {
            get { return Success ? Content : ErrorMessage; }
        }

        public static ApiResponse Ok(string content)
        {
            return new ApiResponse(true, content, string.Empty);
        }

        public static ApiResponse Fail(string errorMessage)
        {
            return new ApiResponse(false, string.Empty, errorMessage);
        }
    }
}
