using EronNew.Models;
using EronNew.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.AspNetCore.Identity.UI.Services;
using EronNew.Helpers;
using Elastic.Apm.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Localization;
using EronNew.Resources;
using System.Reflection;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace EronNew
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
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<IronKeyContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();
                });
            });

            services.AddDefaultIdentity<ExtendedIdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.Cookie.Name = "Eron_gr_App";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(365);
                options.LoginPath = "/Identity/Account/Login";
                // ReturnUrlParameter requires 
                //using Microsoft.AspNetCore.Authentication.Cookies;
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });
            services.Configure<IdentityOptions>(options =>
            {
                // User settings.
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";

                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;

                // Lockout settings.
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            });

            services.AddAuthentication()
                //.AddMicrosoftAccount(microsoftOptions =>
                //{
                //    //730b6e0c-28e9-4f11-8f7d-26c630a1d10b eoMw.me3vc25whPKjf0HT-r.8D.d436J_T
                //    microsoftOptions.ClientId = "434a6e91-2f59-48f5-9123-e3694c537334";
                //    microsoftOptions.ClientSecret = "Qqik_s5HbUJFKjsw6wKs-_.OBi9322Z5eO";
                //    microsoftOptions.CallbackPath = "/signing-microsoft";
                //})
                .AddGoogle(googleOptions =>
                {
                    //IConfigurationSection googleAuthNSection = Configuration.GetSection("Authentication:Google");
                    googleOptions.ClientId = "781045614395-9s41le21bpanjqfm3ftucq57o8r6fn6h.apps.googleusercontent.com";
                    googleOptions.ClientSecret = "zEt45zDy2XoqtDDmJRZwMiit";
                    googleOptions.CallbackPath = "/signing-google";
                })
                //.AddTwitter(twitterOptions => {})
                .AddFacebook(facebookOptions =>
                {
                    //715417039083770 3087c8efac6e5a0d4da3d2d3b0294bbc
                    facebookOptions.AppId = "407388710507530";
                    facebookOptions.AppSecret = "c6d07d05c9a4246e0e84b78a3a4240d8";
                    facebookOptions.CallbackPath = "/signing-facebook";
                    facebookOptions.AccessDeniedPath = "/Identity/Account/AccessDenied";
                });

            services.AddControllersWithViews().AddNewtonsoftJson(options =>
               options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddSingleton<GlobalCultureService>();
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddViewLocalization(
                    LanguageViewLocationExpanderFormat.Suffix,
                    opts => { opts.ResourcesPath = "Resources"; }
                    )
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                    {
                        var assemblyName = new AssemblyName(typeof(SharedResources).GetTypeInfo().Assembly.FullName);
                        return factory.Create("SharedResources", assemblyName.Name);
                    };
                })
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                    {
                        var assemblyName = new AssemblyName(typeof(SharedResources).GetTypeInfo().Assembly.FullName);
                        return factory.Create("SharedResources", assemblyName.Name);
                    };
                });

            CultureInfo ci = new CultureInfo("el-GR");
            ci.NumberFormat.CurrencySymbol = "€";
            CultureInfo ciEN = new CultureInfo("en-US");
            ciEN.NumberFormat.CurrencySymbol = "€";
            CultureInfo ciFR = new CultureInfo("fr");
            ciFR.NumberFormat.CurrencySymbol = "€";
            CultureInfo ciES = new CultureInfo("es");
            ciES.NumberFormat.CurrencySymbol = "€";
            CultureInfo ciTR = new CultureInfo("tr");
            ciTR.NumberFormat.CurrencySymbol = "€";
            CultureInfo ciDE = new CultureInfo("de");
            ciDE.NumberFormat.CurrencySymbol = "€";

            services.Configure<RequestLocalizationOptions>(
                options =>
                {
                    var supportedCultures = new List<CultureInfo> { ci, ciEN, ciFR, ciES, ciTR, ciDE };
                    options.DefaultRequestCulture = new RequestCulture(culture: "el-GR", uiCulture: "el-GR");
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;

                    options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
                });
            CultureInfo.DefaultThreadCurrentCulture = ci;

            services.AddRazorPages();
            //services.AddTransient<IIronKeyContext, IronKeyContext>();
            services.AddTransient<ISearchEngineContext, SearchEngineContext>();
            services.AddTransient<IDomainModel, DomainModel>();
            services.AddTransient<IPostViewModel, PostViewModel>();
            services.AddTransient<IAdministrationViewModel, AdministrationViewModel>();
            
            services.AddScoped<IIronKeyContext, IronKeyContext>();
            services.AddScoped<IMyIndexEngine, MyIndexEngine>();
            services.AddScoped<IEmailSender, EmailService>();
            services.AddScoped<ExtendedUserManager<ExtendedIdentityUser>>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseExceptionHandler("/Home/Error");
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseElasticApm(Configuration);
            app.UseHttpsRedirection();

            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });

        }
    }
}
