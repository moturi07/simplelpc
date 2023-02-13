using lpcDAL.Data;
using lpcDAL.Services;
using lpcDAL.Services.IServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

namespace lpcApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public bool IsDev { get; set; }
        public Startup(IConfiguration configuration, IHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            IsDev = hostingEnvironment.IsDevelopment();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkSqlite().AddDbContext<ApplicationDbContext>();

            services.AddMvcCore()
                 .AddApiExplorer()
            .AddAuthorization()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
                options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            var _urls = !string.IsNullOrEmpty(Configuration["CORS"]) ? Configuration["CORS"].ToString().Split(';') : null;

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                                  builder =>
                                  builder.WithOrigins(_urls)
                                         .AllowAnyMethod()
                                         .AllowAnyHeader()
                                         .AllowCredentials());
            });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(xmlFilename);
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Loap Payment System API",
                    Description = "API Definations for Loap Payment System"
                });
            });
            services.AddScoped<ILoanCalculatorService, LoanCalculatorService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
        {
            if (env.IsDevelopment())
            {
                app.UseCors("CorsPolicy");
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "lpcApp v1"));
            }
            var _appSettings = configuration["CORS"];
            var origins = _appSettings.Split(";");
            app.UseCors(x => x
                      .WithOrigins(origins)
                      .AllowAnyMethod()
                      .AllowCredentials()
                      .AllowAnyHeader());
            //app.UseHttpsRedirection();
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