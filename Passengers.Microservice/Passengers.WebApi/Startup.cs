using GymManger.DataAccess.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Passengers.ApplicationServices;
using Passengers.ApplicationServices.Passengers;
using Passengers.DataAccess;
using Passengers.WebApi.Shared.Config;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using AutoMapper;
using Passengers.Core.Passengers;
using Passengers.Web.Auth;
using System.ComponentModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Passengers.WebApi
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

            string connectionString = Configuration.GetConnectionString("Default");


            Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithExceptionDetails()
    .WriteTo.MySQL(connectionString, tableName: "logs")
    .CreateLogger();



            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog();
            });

            services.AddDbContext<PassengersDataContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddIdentity<IdentityUser, IdentityRole>(
                opts =>
                {
                    opts.Password.RequireDigit = true;
                    opts.Password.RequireLowercase = true;
                    opts.Password.RequireUppercase = true;
                    opts.Password.RequireNonAlphanumeric = true;
                    opts.Password.RequiredLength = 7;
                    opts.Password.RequiredUniqueChars = 4;
                })
                .AddEntityFrameworkStores<PassengersDataContext>()
                .AddDefaultTokenProviders();

           

            services.AddControllers();

            services.Configure<JwtTokenValidationSettings>(Configuration.GetSection("JwtTokenValidationSettings"));

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                builder => builder.AllowAnyOrigin()
                .AllowAnyMethod().WithHeaders("Authorization"));
            });

           

            services.AddTransient<IRepository<int, Passenger>, Repository<int, Passenger>>();
            services.AddTransient<IPassengersAppService, PassengersAppService>();
            services.AddTransient<IJwtIssuerOptions, JwtIssuerFactory>();
            services.AddAutoMapper(typeof(MapperProfile));
            services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter your token in the text input below. \r\n",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                }
                );

                option.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {

                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }

                        },

                        new String[] {}
                    }
                });
            });

            var tokenValidationSettings = services.BuildServiceProvider().GetService<IOptions<JwtTokenValidationSettings>>().Value;

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidIssuer = tokenValidationSettings.ValidIssuer,
                        ValidAudience = tokenValidationSettings.ValidAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenValidationSettings.SecretKey)),
                        ClockSkew = TimeSpan.Zero
                    };
                });
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, PassengersDataContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/error-development");
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseSerilogRequestLogging();

            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Passengers.Web v1"));
            context.Database.Migrate();

            app.InitDb();

            app.UseHttpsRedirection();

            app.UseCors("CorsPolicy");

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


    }
}