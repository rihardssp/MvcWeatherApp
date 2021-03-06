using DataAccessLayer.Contexts;
using DataAccessLayer.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.WebEncoders;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using WebMvc.Configuration;

namespace WebMvc
{
    /// <summary>
    /// This MVC is the front-end for the Weather app. Due to convenience, it'll also seed the database 
    /// with the use of EF (which wouldn't be realistic on production environment)
    /// </summary>
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
            services.AddControllersWithViews();

            services.AddDbContext<WeatherContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("WeatherContext")));

            services.UseRepositories();

            // Removed encoding of unicode chars, while retaining the encoding to avoid xss
            services.Configure<WebEncoderOptions>(options =>
            {
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
            });

            services.Configure<ViewConfiguration>(Configuration.GetSection(ViewConfiguration.Section));
            services.Configure<DateTimeConfiguration>(Configuration.GetSection(DateTimeConfiguration.Section));
            services.Configure<WeatherConfiguration>(Configuration.GetSection(WeatherConfiguration.Section));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, WeatherContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            context.Database.Migrate();

            // Disabled http 
            //app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
