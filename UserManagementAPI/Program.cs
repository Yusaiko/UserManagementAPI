
namespace UserManagementAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			// Middleware Pipeline
			app.UseMiddleware<ErrorHandlingMiddleware>();
			app.UseMiddleware<AuthenticationMiddleware>();
			app.UseMiddleware<LoggingMiddleware>();

			app.UseAuthorization();

			app.MapControllers();

			app.Run();

		}
	}
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

	public class ErrorHandlingMiddleware
	{
		private readonly RequestDelegate _next;

		public ErrorHandlingMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				context.Response.ContentType = "application/json";
				context.Response.StatusCode = StatusCodes.Status500InternalServerError;
				await context.Response.WriteAsync(new { error = "Internal server error" }.ToString());
				Console.WriteLine($"Exception: {ex.Message}");
			}
		}
	}

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
