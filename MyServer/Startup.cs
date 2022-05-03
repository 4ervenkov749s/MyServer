using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MyRabbitMqService.DL;
using RabbitMqService.BL.DataFlow;
using RabbitMqService.BL.Interfaces;
using RabbitMqService.BL.Services;
using RabbitMqService.Models;

namespace MyServer
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
            services.Configure<MongoDbConfiguration>(Configuration.GetSection(nameof(MongoDbConfiguration)));

            //services.AddHostedService<KafkaConsumer<Person>>();
            services.AddHostedService<KafkaPersonConsumer>();
            services.AddSingleton<IPersonDataFlow, PersonDataFlow>();
            services.AddSingleton<IRabbitMqService, RabbitMqService5>();
            services.AddSingleton<IPersonRepository, PersonRepository>();     
            services.AddHostedService<CachePublisher>();
            services.AddSingleton<IKafkaService, KafkaService>();
            services.AddSingleton<IKafkaAdminService, KafkaAdminService>();
            services.AddHostedService<CachePublisherKafka>();


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyServer", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyServer v1"));
            }

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
