using Filed.Payments.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Filed.Payment.Infrastructure.Config;
using Filed.Payments.Infrastructure.Response;
using Filed.Payment.Interfaces;
using Filed.Payment.Repository.Command;
using Filed.Payment.Services;
using Filed.Payments.Interfaces;
using Filed.Payments.Services;

namespace Filed.Payments
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
            // Configure appsettings to use IOptions pattern
            services.Configure<AppConfig>(Configuration.GetSection("AppConfig"));

            string connectionString = Configuration.GetConnectionString("DefaultConnection"); 
            // Use DB context pooling to improve performance
            services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(connectionString));
            services.AddTransient<ResponseFactory>();
            services.AddScoped(typeof(ICommandRepository<>), typeof(CommandRepository<>));
            services.AddTransient<ICheapPaymentGateway, CheapPaymentGateway>();
            services.AddTransient<IExpensivePaymentGateway, ExpensivePaymentGateway>();
            services.AddTransient<IPaymentService, PaymentService>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Filed.Payment", Version = "v1" });
            });

            // Set CORS policy
            services.AddCors(Options =>
            {
                Options.AddPolicy("CorsPolicy",
                    builder =>
                    builder
                    .WithOrigins("https://filed.payment.com")
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    );
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Filed.Payment v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
