using GymManger.DataAccess.Repositories;
using Journeys.ApplicationServices.Journeys;
using Journeys.Core;
using Journeys.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journeys.UnitTest
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

            services.AddDbContext<JourneyDataContext>(options =>
                options.UseInMemoryDatabase("DataTest"));


            services.AddTransient<IRepository<int, Journey>, Repository<int, Journey>>();
            services.AddTransient<IJourneyAppService, JourneyAppService>();


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
