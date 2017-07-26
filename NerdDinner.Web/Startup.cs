using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NerdDinner.Web;
using NerdDinner.Web.Models;
using NerdDinner.Web.Persistence;
using System;
using System.IO.Compression;

namespace NerdDinner.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
    
            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
               .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets();
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
                
            }
            Configuration = builder.Build();

            HostingEnvironment = env;

        }

        public IConfiguration Configuration { get; private set; }

        public IHostingEnvironment HostingEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddEntityFrameworkSqlite().AddDbContext<NerdDinnerDbContext>(options => {
                    var connStringBuilder = new SqliteConnectionStringBuilder() {           DataSource = "./dinners.db"
                    };
                    options.UseSqlite(connStringBuilder.ToString());
            });

            services.AddTransient<INerdDinnerRepository, NerdDinnerRepository>();

            // Add Identity services to the services container
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Cookies.ApplicationCookie.AccessDeniedPath = "/Home/AccessDenied";
                
            })
                    .AddEntityFrameworkStores<NerdDinnerDbContext>()
                    .AddDefaultTokenProviders();


            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithOrigins("http://example.com");
                });
            });

            services.AddLogging();

            // Add MVC services to the services container
            services.AddMvc();

            // Add memory cache services
            if (HostingEnvironment.IsProduction())
            {
              services.AddMemoryCache();
              services.AddDistributedMemoryCache();
            }

            // Add session related services.
            // TODO: Test Session timeout
            services.AddSession(options =>
            {
               // options.CookieName = ".AdventureWorks.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(10);
            });
            // Add Compression Middleware 
           //services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

           //services.AddResponseCompression(options =>
           // {
           //    options.Providers.Add<GzipCompressionProvider>();
           // });


            // Add the system clock service
            services.AddSingleton<ISystemClock, SystemClock>();

            // Configure Auth
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "ManageDinner",
                    authBuilder =>
                    {
                        authBuilder.RequireClaim("ManageDinner", "Allowed");
                    });
            });
        }      

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // Add to app Compression
            //app.UseResponseCompression();
            // Configure Session.
            app.UseSession();
            // Add static files to the request pipeline
            app.UseStaticFiles();
            // Add cookie-based authentication to the request pipeline
            app.UseIdentity();
            //The following lines starting with app.UseThirdPartyAuthenicaiton enable logging in 
            //with login providers https://docs.asp.net/en/latest/security/authentication/sociallogins.html

            app.UseGoogleAuthentication(new GoogleOptions
            {
                ClientId = Configuration["Authentication:Google:ClientId"],
                ClientSecret = Configuration["Authentication:Google:ClientSecret"]
            });

            app.UseTwitterAuthentication(new TwitterOptions
            {
                ConsumerKey = Configuration["Authentication:Twitter:ConsumerKey"],
                ConsumerSecret = Configuration["Authentication:Twitter:ConsumerSecret"]
            });

            app.UseMicrosoftAccountAuthentication(new MicrosoftAccountOptions
            {
                DisplayName = "MicrosoftAccount - Requires project changes",
                ClientId = Configuration["Authentication:Microsoft:ClientID"],
                ClientSecret =Configuration ["Authentication: Microsoft:ClientSecret"]
            });

            // Add MVC to the request pipeline
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name:"redirectjsdinner",
                    template:"dinners/{*pathInfo}",
                    defaults: new { controller = "Home", action = "Index" }
                    );
                routes.MapRoute(
                     name: "default",
                     template: "{controller}/{action}/{id?}",
                     defaults: new { controller = "Home", action = "Index" });

            });

            //Populate dinner sample data
            SampleData.InitializeNerdDinner(app.ApplicationServices).Wait();
        }
    }
}