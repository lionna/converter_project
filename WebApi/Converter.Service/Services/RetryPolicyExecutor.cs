using Microsoft.Extensions.Logging;
using Polly;

namespace Converter.Service.Services
{
	public class RetryPolicyExecutor
	{
		private static readonly ILogger<RetryPolicyExecutor> _logger;

		static RetryPolicyExecutor()
		{
			_logger = LoggerFactory.Create(builder =>
			{
				builder.AddConsole();
			}).CreateLogger<RetryPolicyExecutor>();
		}

		public static async Task<T> ExecuteAsync<T>(Func<Task<T>> action, int maxRetries)
		{
			var retryPolicy = Policy
				.Handle<Exception>(ex => ex is not FileNotFoundException)
				.WaitAndRetryAsync(
					maxRetries,
					retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
					(exception, retryCount) =>
					{
						_logger.LogWarning($"Retry #{retryCount} due to exception: {exception}");
					}
				);

			try
			{
				return await retryPolicy.ExecuteAsync(async () => await action());
			}
			catch (FileNotFoundException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new Exception("An error occurred while executing the action after multiple retries.", ex);
			}
		}

		public static async Task ExecuteAsync(Func<Task> action, int maxRetries)
		{
			var retryPolicy = Policy
				.Handle<Exception>(ex => ex is not FileNotFoundException)
				.WaitAndRetryAsync(
					maxRetries,
					retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
					(exception, retryCount) =>
					{
						_logger.LogWarning($"Retry #{retryCount} due to exception: {exception}");
					}
				);

			try
			{
				await retryPolicy.ExecuteAsync(async () => await action());
			}
			catch (FileNotFoundException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new Exception("An error occurred while executing the action after multiple retries.", ex);
			}
		}
	}
}
