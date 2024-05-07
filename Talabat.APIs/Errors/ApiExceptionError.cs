namespace Talabat.APIs.Errors
{
    public class ApiExceptionError : ApiErrorResponse
    {
        public string? Details { get; set; }

        public ApiExceptionError(int statusCode, string? message = null, string? details = null):base(statusCode,message)
        {
            Details = details;
        }

    }
}
