using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Linq;
using System.Net;
using weatherApp.Repository.Repositories;
using weatherApp.Repository.WeatherAppDbContext;
using weatherApp.Service.Interface;
using weatherApp.Service.Services;
using weatherApp.Shared.Classes;
using weatherApp.Shared.Middleware;
using weatherAppAPI.Helper;

namespace weatherApp
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

            //============Add Database setting=============================
            services.AddDbContext<WeatherDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("weatherAppContextConnection")));

            //===========Dependency injection==============
            services.AddScoped<WeatherDbContext>();

            //================exception handling - when model validation fails==============
            services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .Select(e => new APIError
                        {
                            HTTPCode = HttpStatusCode.BadRequest,
                            Message = e.Value.Errors.First().ErrorMessage
                        }).ToList();

                    return new BadRequestObjectResult(errors); ;
                };
            });

            //================ Configuration for Swagger==============         
            SwaggerHelper.ConfigureService(services);

            // ======================= Add Jwt Authentication ======================           
            var JWTCongig = Configuration.GetSection("JWTAuthentication");
            AuthenticationHelper.ConfigureService(services, JWTCongig["JwtIssuer"], JWTCongig["JwtAudience"], JWTCongig["JwtKey"]);

            //repository injection
            services.AddScoped<WeatherRepository>();

            //application Service injection
            services.AddTransient<IConsumeExternalAPIService, ConsumeExternalAPIService>();
            services.AddTransient<IWeatherService, WeatherService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IUserService, UserService>();

            services.AddControllers();

            //================ Configuration for Swagger==============
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo
            //    {
            //        Version = "v1",
            //        Title = "weatherApp API",
            //        Description = "weatherApp API Endpoints",
            //        Contact = new OpenApiContact
            //        {
            //            Name = "Vimalan Kumarakulasingam",
            //            Url = new Uri("https://www.linkedin.com/in/mkvimalan/"),
            //        }
            //    });
            //});

            //===================Added CorsPolicy=============================
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
        }

        //========= This method gets called by the runtime. Use this method to configure the HTTP request pipeline.==================
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IWeatherService weatherService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //====add middleware==============
            app.UseMiddleware<ExceptionAndAuthenticationMiddleware>();

            // ========http status such as 404 will not occur above so catch here============
            app.UseStatusCodePages(async context =>
            {
                await ExceptionAndAuthenticationMiddleware.HandleErrorsAsync(context.HttpContext, null, (HttpStatusCode)context.HttpContext.Response.StatusCode);
            });

            //==========Swagger Setting==========
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "weatherApp");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }


    }
}
