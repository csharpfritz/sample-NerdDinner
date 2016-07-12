using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NerdDinner.Web;
using NerdDinner.Web.Models;
using NerdDinner.Web.Persistence;

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

            services.AddDbContext<NerdDinnerDbContext>(options =>
                    options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

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
            services.AddSession();

            // Add the system clock service
            services.AddSingleton<ISystemClock, SystemClock>();

            // Configure Auth
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "ManageStore",
                    authBuilder =>
                    {
                        authBuilder.RequireClaim("ManageStore", "Allowed");
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
            // Configure Session.
            app.UseSession();

            // Add static files to the request pipeline
            app.UseStaticFiles();

            // Add cookie-based authentication to the request pipeline
            app.UseIdentity();

            app.UseFacebookAuthentication(new FacebookOptions
            {
                AppId = "550624398330273",
                AppSecret = "10e56a291d6b618da61b1e0dae3a8954"
            });

            app.UseGoogleAuthentication(new GoogleOptions
            {
                ClientId = "995291875932-0rt7417v5baevqrno24kv332b7d6d30a.apps.googleusercontent.com",
                ClientSecret = "J_AT57H5KH_ItmMdu0r6PfXm"
            });

            app.UseTwitterAuthentication(new TwitterOptions
            {
                ConsumerKey = "lDSPIu480ocnXYZ9DumGCDw37",
                ConsumerSecret = "fpo0oWRNc3vsZKlZSq1PyOSoeXlJd7NnG4Rfc94xbFXsdcc3nH"
            });

            // The MicrosoftAccount service has restrictions that prevent the use of
            // http://localhost:5001/ for test applications.
            // As such, here is how to change this sample to uses http://ktesting.com:5001/ instead.

            // Edit the Project.json file and replace http://localhost:5001/ with http://ktesting.com:5001/.

            // From an admin command console first enter:
            // notepad C:\Windows\System32\drivers\etc\hosts
            // and add this to the file, save, and exit (and reboot?):
            // 127.0.0.1 ktesting.com

            // Then you can choose to run the app as admin (see below) or add the following ACL as admin:
            // netsh http add urlacl url=http://ktesting:5001/ user=[domain\user]

            // The sample app can then be run via:
            // dnx . web
            app.UseMicrosoftAccountAuthentication(new MicrosoftAccountOptions
            {
                DisplayName = "MicrosoftAccount - Requires project changes",
                ClientId = "000000004012C08A",
                ClientSecret = "GaMQ2hCnqAC6EcDLnXsAeBVIJOLmeutL"
            });

            // Add MVC to the request pipeline
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                     name: "default",
                     template: "{controller}/{action}/{id?}",
                     defaults: new { controller = "Home", action = "Index" });

            });

            //Populates the MusicStore sample data
            SampleData.InitializeNerdDinner(app.ApplicationServices).Wait();
        }
    }
}