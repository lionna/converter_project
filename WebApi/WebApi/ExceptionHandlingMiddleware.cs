using Converter.Service.Exceptions;
using Converter.Service.Settings;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;
using FileNotFoundException = Converter.Service.Exceptions.FileNotFoundException;

namespace WebApi
{
    public class ExceptionHandlingMiddleware(
        RequestDelegate requestDelegate,
        ILogger<ExceptionHandlingMiddleware> logger,
        IOptions<ApplicationSettings> settings)
    {
        private readonly ApplicationSettings _settings = settings.Value;

		private static readonly Dictionary<Type, (HttpStatusCode StatusCode, Func<Exception, string> GetMessage)> _exceptionHandlers = new()
        {
            { typeof(FileNotFoundException), (HttpStatusCode.NotFound, _ => "File not found.") },
            { typeof(IncorrectInputFileException), (HttpStatusCode.BadRequest, ex => ex.Message) },
            { typeof(ConvertFileException), (HttpStatusCode.BadRequest, ex => ex.Message) }
        };

		public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				await requestDelegate(httpContext);
			}
			catch (Exception ex)
			{
				var exceptionType = ex.GetType();
				var (statusCode, getMessage) = _exceptionHandlers.TryGetValue(exceptionType, out var handler)
					? handler
					: (HttpStatusCode.InternalServerError, (Exception e) => e.Message);

				await HandleExceptionsAsync(
					httpContext,
					ex.Message,
					statusCode,
					getMessage(ex));
			}
		}

		private async Task HandleExceptionsAsync(
            HttpContext context,
            string exceptionMessage,
            HttpStatusCode httpStatusCode,
            string customMessage
            )
        {
            logger.LogError(message: exceptionMessage);
            var httpResponse = context.Response;

            httpResponse.ContentType = _settings.JsonContentType;
            httpResponse.StatusCode = (int)httpStatusCode;

            ErrorDto errorDto = new()
            {
                Message = customMessage,
                ErrorCode = (int)httpStatusCode,
                Result = Array.Empty<object>()
            };

            var jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var result = JsonSerializer.Serialize(errorDto, jsonSerializerOptions);
            await httpResponse.WriteAsync(result);
        }
    }
}