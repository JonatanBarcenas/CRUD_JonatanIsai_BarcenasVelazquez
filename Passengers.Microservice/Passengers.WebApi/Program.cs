using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace Passengers.WebApi
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateBootstrapLogger();

            Log.Information("Starting up!");

            try
            {
                CreateHostBuilder(args).Build().Run();

                Log.Information("Stopped cleanly");
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "An unhandled exception ocurred during bootsraping");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }



        public static IHostBuilder CreateHostBuilder(string[] args) =>
         Host.CreateDefaultBuilder(args)
             .UseSerilog((context, services, configuration) => configuration
             .ReadFrom.Configuration(context.Configuration)
             .ReadFrom.Services(services)
             .Enrich.FromLogContext()
              )
             .ConfigureWebHostDefaults(webBuilder => {
                 webBuilder.UseStartup<Startup>();

             });
    }
}
