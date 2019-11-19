using ErrorIt.Api.Interfaces;
using ErrorIt.Api.Services.Caching;
using ErrorIt.Api.Services.DataAccess;
using ErrorIt.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Linq;

namespace ErrorIt.Api
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();

			services.AddDistributedMemoryCache().AddStackExchangeRedisCache(options =>
			{
				options.Configuration = "redis_cache";
				options.InstanceName = "master";
			});

			services.AddDbContext<AppDbContext>(options => {
				options.UseSqlServer(Configuration["ConnectionStrings:Data"]);
			}, ServiceLifetime.Scoped);

			services.AddHttpsRedirection(configureOptions => {
				configureOptions.HttpsPort = 5001;
				configureOptions.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
			});

			services.AddLogging(configure => configure.AddConsole());
			services.AddScoped<ICacher, Cacher>();
			services.AddScoped<IApplicationGroupRepository, ApplicationGroupRepository>();
			services.AddScoped<IApplicationRepository, ApplicationRepository>();
			services.AddScoped<IErrorTemplateRepository, ErrorTemplateRepository>();

			//services.AddSwaggerGen(options => {
			//	var title = "ErrorIt";
			//	var description = "A centralized error message storage and formatting service based on RFC 7807";
			//	options.SwaggerDoc("ErrorIt", new Microsoft.OpenApi.Models.OpenApiInfo { Title = title, Description = description });
			//});

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI", Version = "v1" });
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			RunMigrations(app);

			app.UseStaticFiles();

			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.RoutePrefix = "swagger/ui";
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI(v1)");
			});

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}

		private static void RunMigrations(IApplicationBuilder app)
		{
			using(var services = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
			{
				using(var context = services.ServiceProvider.GetService<AppDbContext>())
				{
					context.Database.EnsureCreated();

					try
					{
						var pendingMigrations = context.Database.GetPendingMigrations();

						if(pendingMigrations.Any())
							context.Database.Migrate();
					}
					catch(Exception e)
					{
						Console.WriteLine(e.Message ?? "");
					}
				}
			}
		}
	}
}
