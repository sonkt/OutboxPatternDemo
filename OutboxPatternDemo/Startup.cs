using OutboxPattern.Data;
using OutboxPattern.Services;
using OutboxPattern.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace OutboxPattern
{


    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Sử dụng InMemory for testing
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("OutboxPatternDb");
                options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            services.AddScoped<OrderService>();
            services.AddScoped<OrderProcessingService>();
            services.AddScoped<PaymentService>();
            services.AddHostedService<OutboxProcessor>();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}