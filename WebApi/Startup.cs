using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Data;
using WebApi.Auth;
using WebApi.ViewModels;
using WebApi.Helpers;
using Model;
using Data.Infrastructure.UnitOfWork;
using Data.Repositories;
using Service.Services;
using WebApi.Mappings;
using Newtonsoft.Json.Serialization;
using Common;
using WebApi.Extensions;

namespace WebApi
{
    public class Startup
    {
        private const string SecretKey = "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH"; // todo: get this from somewhere secure
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            if (env.IsEnvironment("Development"))
            {
            }
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add context
            services.AddDbContext<HauLeDbContext>(opstions => opstions
            .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Data")));

            services.AddSingleton<IJwtFactory, JwtFactory>();
            // jwt wire up
            // Get options from app settings
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            // Configure JwtIssuerOptions  Add JWT generation endpoint:
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

            // api user claim policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.Rol, Constants.Strings.JwtClaims.ApiAccess));
            });

            services.AddIdentity<AppUser, IdentityRole>
                (o =>
                {
                    // configure identity options
                    o.Password.RequireDigit = false;
                    o.Password.RequireLowercase = false;
                    o.Password.RequireUppercase = false;
                    o.Password.RequireNonAlphanumeric = false;
                    o.Password.RequiredLength = 6;
                })
                .AddEntityFrameworkStores<HauLeDbContext>()
                .AddDefaultTokenProviders();

            // ----===========---- Dependency Injection ----===========----
            //AddUnitOfWork
            services.AddUnitOfWork<HauLeDbContext>();

            // Add Repository
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();

            services.AddScoped<IStudentService, StudentService>();
            //services.AddScoped<IEnrollmentService, EnrollmentService>();
            //services.AddScoped<ICourseSevice, CourseSevice>();

            //Add Mapping
            Mapper.Initialize(x =>
                x.AddProfile<AutoMapperConfiguration>()
            );

            
            services.AddCors(options => options.AddPolicy("AllowCors",
                            builder => { builder.AllowAnyOrigin().WithMethods("GET", "PUT", "POST", "DELETE").AllowAnyHeader(); }));

            // Add framework services.
            services.AddMvc().AddJsonOptions(opts =>
            {
                opts.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            //Config my app
            // Add functionality to inject IOptions<T>
            services.AddOptions();
            // Add our Config object so it can be injected
            services.Configure<MySettings>(Configuration.GetSection("MySettings"));

            // *If* you need access to generic IConfiguration this is **required**
            services.AddSingleton<IConfiguration>(Configuration);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, HauLeDbContext contextDb)
        {
            loggerFactory.AddConsole();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();


            app.UseExceptionHandler(
            builder =>
            {
                builder.Run(
                  async context =>
                  {
                      context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                      context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                      var error = context.Features.Get<IExceptionHandlerFeature>();
                      if (error != null)
                      {
                          context.Response.AddApplicationError(error.Error.Message);
                          await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                      }
                  });
            });

            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters
            });




            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
            app.UseCors(builder => builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
            //Add Data To DB
            HauLeDbInitializer.Initialize(contextDb);
        }
    }
}
