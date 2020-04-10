using System;
using SelecTunes.Backend.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.HttpOverrides;
using System.Net;
using Microsoft.OpenApi.Models;
using SelecTunes.Backend.Models;
using Microsoft.AspNetCore.Http;
using SelecTunes.Backend.Helper;
using System.Net.Http;
using Microsoft.Extensions.Options;
using SelecTunes.Backend.Helper.Hubs;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace SelecTunes.Backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("Default"));
            });

            services.AddSignalR();

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = "localhost:6379";
                //options.Configuration = Configuration.GetConnectionString("Redis");
            });

            services.AddHttpClient("spotify", c =>
            {
                c.BaseAddress = new Uri("https://api.spotify.com/v1/");
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Add("User-Agent", "SelecTunes-Api");
            });

            services.AddHttpClient("spotify-accounts", c =>
            {
                c.BaseAddress = new Uri("https://accounts.spotify.com/api/");
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Add("User-Agent", "SelecTunes-Api");
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SelecTunesApi", Version = "v1" });
            });

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;

                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;

                options.Lockout.AllowedForNewUsers = false;
                options.Lockout.MaxFailedAccessAttempts = 3;

                options.User.RequireUniqueEmail = true;
            }).AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationContext>();

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.KnownProxies.Add(IPAddress.Parse("127.0.0.1"));
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.SlidingExpiration = true;

                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.None;
                options.Cookie.IsEssential = true;
                options.Cookie.Name = "Holtzmann";
                options.Cookie.SameSite = SameSiteMode.None;

                options.LoginPath = "/api/Auth/Login";
                options.AccessDeniedPath = "/Api/Auth/AccessDenied";
            });

            services.AddScoped<AuthHelper>(options => {
                IHttpClientFactory _cf = options.GetRequiredService<IHttpClientFactory>();
                IOptions<AppSettings> _options = options.GetRequiredService<IOptions<AppSettings>>();

                return new AuthHelper
                {
                    ClientFactory = _cf,
                    ClientSecret = _options.Value.ClientSecret,
                    ClientId = _options.Value.ClientId,
                    RedirectUrl = _options.Value.RedirectUri,
                };
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().Build();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();

                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                });
            }

            if (context != null)
            {
                context.Database.Migrate();
            }

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SelecTunes API v1");
            });

            app.UseCors();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseUserDestroyer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<QueueHub>("/queue");
                endpoints.MapHub<ChatHub>("/chat");
            });
        }
    }
}
