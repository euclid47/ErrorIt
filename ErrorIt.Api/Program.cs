using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ErrorIt.Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			DotNetEnv.Env.Load();
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder
					.UseKestrel(options =>
					{
						options.ListenAnyIP(5000);
						options.ListenAnyIP(5001, configure => { configure.UseHttps(DotNetEnv.Env.GetString("pfx"), DotNetEnv.Env.GetString("pfxpass")); });
					})
					.UseStartup<Startup>();
				});
	}
}
