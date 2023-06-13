using GymManger.DataAccess.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Passengers.ApplicationServices.Passengers;
using Passengers.Core.Passengers;
using Passengers.DataAccess;
using Passengers.Web.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passengers.UnitTest
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
            services.AddControllers();

            services.AddDbContext<PassengersDataContext>(options =>
                options.UseInMemoryDatabase("DataTest"));

    
            services.AddTransient<IRepository<int, Passenger>, Repository<int, Passenger>>();
            services.AddTransient<IPassengersAppService, PassengersAppService>();

          
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           

            app.UseDeveloperExceptionPage();
          

            app.UseHttpsRedirection();

           

            app.UseRouting();

          
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
