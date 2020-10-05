using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using TestTask.Models;
using Microsoft.AspNetCore.Mvc;

namespace TestTask
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
            services.AddControllers().AddNewtonsoftJson();
            services.AddDbContext<PersonContext>(opt =>
               opt.UseSqlite("Data Source=Perons.db"));
            services.AddControllers();
            services.AddMvc(option => option.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            /*app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "defalut",
                    template: "api/person");

                routes.MapRoute(
                    name: "FaceAPI",
                    template: "api/person/{person_id}/face/{id}",
                    defaults: new
                        {
                        controller = "Face",
                        }
                    );

                routes.MapRoute(
                    name: "PersonAPI",
                    template: "api/person/{id}",
                    defaults: new
                    {
                        controller = "Person",
                    }
                    );
            });*/

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
