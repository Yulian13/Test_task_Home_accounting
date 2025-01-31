using BusinessLogic.Models;
using BusinessLogic.Services;
using DataBase.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Test_task_Home_accounting;
using Test_task_Home_accounting.HealthChecks;

internal class Program
{
	private static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		builder.Services
			.Configure<AppSettings>(builder.Configuration)
			.AddSingleton(sp => sp.GetRequiredService<IOptions<AppSettings>>().Value);
		builder.Services.AddTransient<UserAuthorizer>();
		builder.Services.AddTransient<CostCategoryEditor>();
		builder.Services.AddTransient<ExpenseEditor>();
		builder.Services.AddTransient<Authenticator>();
		builder.Services.AddTransient<ReportService>();
		builder.Services.AddHttpClient<FixerApiHttpService>();

		builder.Services.AddHealthChecks()
			.AddCheck<DataBaseHealthCheck>("DataBaseCheck")
			.AddCheck<CurrentServiceHealthCheck>("CurrentServiceCheck");

		builder.Services.AddDbContext<HomeAccountContext>(options =>
			options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

		Authenticator.AddAuthentication(builder.Services, builder.Configuration);
		builder.Services.AddAuthorization();

		builder.Services.AddControllers();
		builder.Services.AddEndpointsApiExplorer();

		builder.Services.AddSwaggerGen(options =>
		{
			options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
			{
				In = ParameterLocation.Header,
				Description = "Please enter 'Bearer [jwt]'",
				Name = "Authorization",
				Type = SecuritySchemeType.ApiKey
			});
			var scheme = new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = JwtBearerDefaults.AuthenticationScheme
				}
			};

			var xmlPath = Path.Combine(AppContext.BaseDirectory, "ShopAPI.xml");
			options.IncludeXmlComments(xmlPath);

			options.AddSecurityRequirement(new OpenApiSecurityRequirement { { scheme, Array.Empty<string>() } });
		});

		var app = builder.Build();
		app.MapHealthChecks("/health");

		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseHttpsRedirection();

		app.UseAuthentication();
		app.UseAuthorization();

		app.MapControllers();

		app.Run();
	}
}