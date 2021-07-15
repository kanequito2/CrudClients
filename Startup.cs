using CrudClients.Database;
using CrudClients.UseCase;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.AspNetCore.Http;
using System.Net;
using CrudClients.Database.MongoDb;

namespace CrudClients
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
            services.AddControllers(options =>
            {
                options.AllowEmptyInputInBodyModelBinding = true;
            }).AddNewtonsoftJson();
            services.AddScoped<IClientUseCase,ClientUseCase>();
            services.AddScoped<IClientRepository,ClientRepository>();
            services.AddSingleton<IContext>(new MongoContext(@"mongodb://localhost:27017","CrudClients"));
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStatusCodePagesWithRedirects("/Error/{0}");


            //app.UseStatusCodePages(async context =>
            //{
            //    var response = context.HttpContext.Response;
            //    await response.WriteAsync($"Unhandled error captured with code:\n{response.StatusCode}");
            //});

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
