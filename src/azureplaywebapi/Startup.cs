using DataLayer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace azureplaywebapi
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
            services.AddControllers();

    //        string connectionString =
    //@"mongodb://clarkezonetestcosmosforplay:QV6W2fWmedSoEGvQSVAzP3IOUZWtTyjIkGq2k4Yxna6JEm0lvGm4p7zbcPOMaKYwMzkWCxJTguDw5QYbPh4cng==@clarkezonetestcosmosforplay.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb&maxIdleTimeMS=120000&appName=@clarkezonetestcosmosforplay@&retrywrites=false";


            //TODO: fetch connection string from Azure somehow ;-0
            //var asd = new AzureServiceDescriptionService(connectionString);

            services.AddSingleton<AzureServiceDescriptionService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
            });
        }
    }
}
