﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
// You need to add one more using statement:
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MvcMovie.Controllers;
using MvcMovie.DataAccess;

namespace MvcMovie.FeatureTests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices(services =>
                    {
                        //THIS IS THE CODE TO ADD:
                        services.AddDbContext<MvcMovieContext>(options =>
                            options.UseInMemoryDatabase("TestDatabase"));

                        services.AddControllersWithViews()
                            .AddApplicationPart(typeof(HomeController).Assembly);
                    });

                    webBuilder.Configure(app =>
                    {
                        app.UseRouting();
                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllerRoute(
                                name: "default",
                                pattern: "{controller=Home}/{action=Index}/{id?}");
                        });
                    });
                });
    }
}