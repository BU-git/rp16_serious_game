﻿using System;
using BLL.Abstract;
using BLL.Concrete;
using Domain.Entities;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebUI.Services;
using DAL;
using Interfaces;
using WebUI.Infrastructure.Abstract;
using WebUI.Infrastructure.Concrete;

namespace WebUI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
            

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            //Add DAL
            services.AddScoped<IDAL, DAL.DAL>();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddTransient<RazorComposer>();
            services.AddTransient<AbstractEmailBuilder, EmailBuilder>();
            services.AddTransient<IMailSender, EmailSender>();
            services.AddTransient<IMailManager, EmailManager>();
            services.AddSingleton<IConfigurationRoot>(conf => Configuration);

            //Add Seed Method
            services.AddTransient<DataInitializer>();
    
            services.AddTransient<TranslationManager>();
            services.AddTransient<ITranslationProvider, JsonTranslationProvider> ( x => new JsonTranslationProvider(Configuration["Data:Resources:Path"]));

            IServiceProvider sp = services.BuildServiceProvider();
            ITranslationProvider service = sp.GetService<ITranslationProvider>();
            TranslationManager.Instance.TranslationProvider = service;
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, DataInitializer dataInitializer, IServiceProvider serviceProvider)
        {
            

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                // For more details on creating database during deployment see http://go.microsoft.com/fwlink/?LinkID=615859
                try
                {
                    using (IServiceScope serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                        .CreateScope())
                    {
                        serviceScope.ServiceProvider.GetService<ApplicationDbContext>()
                             .Database.Migrate();
                    }
                }
                catch { }
            }

            app.UseIISPlatformHandler(options => options.AuthenticationDescriptions.Clear());

            app.UseStaticFiles();

            app.UseIdentity();

            // To configure external authentication please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //Seed DataBase
            await dataInitializer.InitializeDataAsync();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
