namespace UserManagementAPI.Middleware
{
	public class AuthenticationMiddleware
	{
		private readonly RequestDelegate _next;

		public AuthenticationMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			if (!context.Request.Headers.ContainsKey("Authorization"))
			{
				context.Response.StatusCode = StatusCodes.Status401Unauthorized;
				await context.Response.WriteAsync("Unauthorized");
				return;
			}

			var token = context.Request.Headers["Authorization"].ToString().Split(" ").Last();
			if (token != "valid-token") // Replace with your token validation logic
			{
				context.Response.StatusCode = StatusCodes.Status401Unauthorized;
				await context.Response.WriteAsync("Unauthorized");
				return;
			}

			await _next(context);
		}
	}
}
