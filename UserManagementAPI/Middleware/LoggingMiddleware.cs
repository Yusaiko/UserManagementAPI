namespace UserManagementAPI.Middleware
{
	public class LoggingMiddleware
	{
		private readonly RequestDelegate _next;

		public LoggingMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			Console.WriteLine($"HTTP Request: {context.Request.Method} {context.Request.Path}");

			// Call the next middleware in the pipeline
			await _next(context);

			Console.WriteLine($"HTTP Response: {context.Response.StatusCode}");
		}
	}
}
