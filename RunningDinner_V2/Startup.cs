using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RunningDinner.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RunningDinner.Extensions;
using RunningDinner.Models.DatabaseModels;
using System.Globalization;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OAuth;
using IdentityModel;

namespace RunningDinner
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public static string ConnectionString { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = "Prod";
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString(connectionString)));
            ConnectionString = Configuration.GetConnectionString(connectionString);
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>().AddErrorDescriber<IdentityErrorExtensions>();
            // Add Role claims to the User object
            // See: https://github.com/aspnet/Identity/issues/1813#issuecomment-420066501
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>>();
            // Repositories
            services.AddScoped<IDinnerEventsRepository, DinnerEventsRepository>();
            services.AddScoped<IEventParticipationsRepository, EventParticipationsRepository>();
            services.AddScoped<ITeamsRepository, TeamsRepository>();
            services.AddScoped<IRoutesRepository, RoutesRepository>();
            services.AddScoped<IInvitationsRepository, InvitationsRepository>();
            services.AddControllersWithViews();
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential 
                // cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                // requires using Microsoft.AspNetCore.Http;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddSession();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMvc();
            services.AddRazorPages();
            // Facebook + Google Authentication
            services.AddAuthentication()
            .AddFacebook(facebookOptions =>
            {
                IConfigurationSection facebookAuthSection = Configuration.GetSection("Facebook");
                facebookOptions.AppId = facebookAuthSection["appId"];
                facebookOptions.AppSecret = facebookAuthSection["appSecret"];
                facebookOptions.Fields.Add("picture");
                facebookOptions.Events = new OAuthEvents
                {
                    OnCreatingTicket = context =>
                    {
                        var identity = (ClaimsIdentity)context.Principal.Identity;
                        var profileImg = $"https://graph.facebook.com/{context.Principal.FindFirstValue(ClaimTypes.NameIdentifier)}/picture?type=large";
                        identity.AddClaim(new Claim(JwtClaimTypes.Picture, profileImg));
                        return Task.CompletedTask;
                    }
                };
            })
            .AddGoogle(googleOptions =>
            {
                IConfigurationSection googleAuthSection = Configuration.GetSection("GoogleAuth");
                googleOptions.ClientId = googleAuthSection["clientId"];
                googleOptions.ClientSecret = googleAuthSection["clientSecret"];
                googleOptions.Events = new OAuthEvents
                {
                    OnCreatingTicket = context =>
                    {
                        var identity = (ClaimsIdentity)context.Principal.Identity;
                        var profileImg = context.User.GetProperty("picture").ToString();
                        identity.AddClaim(new Claim(JwtClaimTypes.Picture, profileImg));
                        return Task.FromResult(0);
                    }
                };
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var cultureInfo = new CultureInfo("de-DE", false);
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            app.UseRouting();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession(); // use this before .UseEndpoints
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
