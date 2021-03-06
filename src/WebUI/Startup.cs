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
using DAL;
using Interfaces;
using RestSharp;
using WebUI.Infrastructure.Abstract;
using WebUI.Infrastructure.Concrete;
using WebUI.Services.Abstract;
using WebUI.Services.Concrete;
using React.AspNet;

namespace WebUI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("config.json");


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

            services.AddCaching();
            services.AddSession();
            services.AddReact();
            services.AddMvc();

            //Add DAL
            services.AddScoped<IDAL, DAL.DAL>();

            // Add application services.
            services.AddTransient<TranslationManager>();
            services.AddTransient<ITranslationProvider, JsonTranslationProvider>(x => new JsonTranslationProvider(Configuration["Data:Resources:Path"]));
            services.AddTransient<IMailSender, EmailSender>();
            services.AddTransient<IMailManager, EmailManager>();
            services.AddTransient<IRestClient, RestClient>();
            services.AddTransient<IZipWorker, ZipWorker>();
            services.AddTransient<ICountryListProvider, RESTCountryListProvider>();

            // Infrastructure
            services.AddTransient<IViewComposer, RazorViewComposer>();
            services.AddTransient<AbstractEmailBuilder, EmailBuilder>();
            services.AddSingleton(conf => Configuration);
            services.AddTransient<ICryptoServices, CryptoServices>();
            services.AddTransient<IPropertyConfigurator, JsonPropertyConfigurator>();

            //Add Seed Method
            services.AddTransient<DataInitializer>();

            var sp = services.BuildServiceProvider();
            var service = sp.GetService<ITranslationProvider>();
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
                    using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                        .CreateScope())
                    {
                        serviceScope.ServiceProvider.GetService<ApplicationDbContext>()
                             .Database.Migrate();
                    }
                }
                catch
                {
                    // ignored
                }
            }

            app.UseIISPlatformHandler(options => options.AuthenticationDescriptions.Clear());
            // Initialise ReactJS.NET. Must be before static files.
            app.UseReact(config =>
            {
                // If you want to use server-side rendering of React components,
                // add all the necessary JavaScript files here. This includes
                // your components as well as all of their dependencies.
                // See http://reactjs.net/ for more information. Example:
                config
                    .AddScript("~/assets/scripts/Comments.jsx");

                // If you use an external build too (for example, Babel, Webpack,
                // Browserify or Gulp), you can improve performance by disabling
                // ReactJS.NET's version of Babel and loading the pre-transpiled
                // scripts. Example:
                //config
                //    .SetLoadBabel(false)
                //    .AddScriptWithoutTransform("~/Scripts/bundle.server.js");
            });

            app.UseStaticFiles();

            app.UseIdentity();

            app.UseSession();

            // To configure external authentication please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "ResetPassword",
                    template: "Account/ResetPassword/{code?}",
                    defaults: new { controller = "Account", action = "ResetPassword"});
                routes.MapRoute(
                    name: "ManageMessage",
                    template: "Manage/Index/{message?}",
                    defaults: new { controller = "Manage", action = "Index" });
                routes.MapRoute(
                    name: "ManageInfo",
                    template: "Manage/Edit/Info",
                    defaults: new { controller = "Manage", action = "EditPersonalInformation" });
                routes.MapRoute(
                    name: "ManagePassword",
                    template: "Manage/Edit/Password",
                    defaults: new { controller = "Manage", action = "ChangePassword" });
                routes.MapRoute(
                    name: "SetPassword",
                    template: "Manage/Edit/SetPass",
                    defaults: new { controller = "Manage", action = "SetPassword" });
                routes.MapRoute(
                    name: "ManageAvatar",
                    template: "Manage/Edit/Avatar",
                    defaults: new { controller = "Manage", action = "ChangeAvatar" });
                routes.MapRoute(
                    name: "AvatarList",
                    template: "Marketplace/{avatarId?}",
                    defaults: new { controller = "Marketplace", action = "AvatarList" });
                routes.MapRoute(
                    name: "RegStep1",
                    template: "Registration/Step/1",
                    defaults: new { controller = "Registration", action = "StepOne" });
                routes.MapRoute(
                    name: "RegStep2",
                    template: "Registration/Step/2/{familyId}",
                    defaults: new { controller = "Registration", action = "StepTwo" });
                routes.MapRoute(
                    name: "RegStep3",
                    template: "Registration/Step/3/{avatarId?}",
                    defaults: new { controller = "Registration", action = "StepThree" });
                routes.MapRoute(
                    name: "TasksByRegion",
                    template: "Task/Region/{region}",
                    defaults: new { controller = "Task", action = "ViewTasksByRegion" });
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
