﻿using System;
using CourseManagementSystem.Areas.Identity.Data;
using CourseManagementSystem.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(CourseManagementSystem.Areas.Identity.IdentityHostingStartup))]
namespace CourseManagementSystem.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<CourseManagementSystemContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("CourseManagementSystemContextConnection")));

                services.AddDefaultIdentity<CourseManagementSystemUser>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddEntityFrameworkStores<CourseManagementSystemContext>();
            });
        }
    }
}