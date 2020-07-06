using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tandem.Users.Api.Services;
using Tandem.Users.Api.Settings;

namespace Tandem.Users.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddApplicationInsightsTelemetry(Configuration["ApplicationInsights:InstrumentationKey"]);
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IHealthService, HealthService>();
            services.Configure<CosmosDbSettings>(Configuration.GetSection(nameof(CosmosDbSettings)));
            services.AddHealthChecks().AddDbContextCheck<TandemUserContext>();
            services.AddDbContext<TandemUserContext>(options =>
            {
                options.UseCosmos(Configuration.GetValue<string>("CosmosDbSettings:EndpointUri"), Configuration.GetValue<string>("CosmosDbSettings:PrimaryKey"), Configuration.GetValue<string>("CosmosDbSettings:DatabaseId"));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                // TODO: this would be a constant
                endpoints.MapHealthChecks("/api/v1/health");
            });
        }
    }
}
