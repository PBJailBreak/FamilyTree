using Core.Contracts.DataAcess;
using Core.Contracts.Services;
using Hangfire;
using Infrastructure.DataAcess;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Data.SqlClient;
using UI.Attributes;
using UI.Middleware;

namespace FamilyTree
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IConfiguration configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<FamilyTreeDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("FamilyTreeDbContext"))
                )
                .AddMemoryCache()
                .ConfigureDataAcess()
                .ConfigureDomainServices()
                .ConfigureHangFire(configuration)
                .AddSwaggerGen(options =>
                    {
                        options.SwaggerDoc("v1", new Info { Title = "Family tree API", Version = "v1" });
                    })
                .AddMvc(options =>
                    {
                        options.Filters.Add(typeof(ModelStateValidationAttribute));
                    })
                .AddJsonOptions(options =>
                    {
                        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app
                .UseHangfireDashboard()
                .ConfigureCustomExceptionHandler()
                .UseHttpsRedirection()
                .UseMvc()
                .UseSwagger()
                .UseSwaggerUI(sa =>
                    {
                        sa.SwaggerEndpoint("/swagger/v1/swagger.json", "Family tree API V1");
                    });
        }
    }

    public static class ConfigurationExtensions
    {
        private static void EnsureHangFireDatabaseIsPresent(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("HangFire");

            if (connectionString == null)
            {
                throw new Exception("Please, correctly configure HangFire connection string.");
            }

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var commandText = string.Format(@"IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'{0}') create database [{0}];", "HangFire");

                using (var command = new SqlCommand(commandText, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static IServiceCollection ConfigureHangFire(this IServiceCollection services, IConfiguration configuration)
        {
            EnsureHangFireDatabaseIsPresent(configuration);

            services.AddHangfire(x =>
                x.UseSqlServerStorage(configuration.GetConnectionString("HangFire"))
            );
            services.AddHangfireServer();

            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 });

            return services;
        }

        public static IApplicationBuilder ConfigureCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();

            return app;
        }

        public static IServiceCollection ConfigureDomainServices(this IServiceCollection services)
        {
            services.AddHttpClient<IAgeService, AgeService>();
            services.AddScoped<IPeopleService, PeopleService>();
            services.AddScoped<IFamilyTreeService, FamilyTreeService>();

            return services;
        }

        public static IServiceCollection ConfigureDataAcess(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
