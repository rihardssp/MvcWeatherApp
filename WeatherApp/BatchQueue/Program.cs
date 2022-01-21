using DataAccessLayer.Contexts;
using DataAccessLayer.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using System;

namespace BatchQueue
{
    /// <summary>
    /// This console is dedicated for Quartz job scheduler. 
    /// Scheduler is added as a HostBuilder through plugin to conveniently use established context & repositories, access to DI and logging,
    /// as well as configuration.
    /// 
    /// File for scheduling the job is defined below
    /// 
    /// Quartz scheduler runs on RAM DB, so this image instance only accesses DB to push data.
    /// </summary>
    class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) => 
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                    services.AddDbContext<WeatherContext>(options =>
                        options.UseSqlServer(context.Configuration.GetConnectionString("WeatherContext")))
                    .UseRepositories()
                    .UseOpenWeatherConsumer(context.Configuration)
                    .AddQuartz(quartz =>
                    {
                        quartz.UseMicrosoftDependencyInjectionJobFactory();
                        quartz.UseDefaultThreadPool(options => options.SetProperty("quartz.threadPool.threadCount", "2"));
                        quartz.UseXmlSchedulingConfiguration(options =>
                        {
                            options.FailOnFileNotFound = true;
                            options.Files = new string[] { "Schedule/Data.xml" };
                        });
                    })
                    .AddQuartzHostedService());
        
    }
}
