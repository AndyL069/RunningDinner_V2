using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(RunningDinner.Areas.Identity.IdentityHostingStartup))]
namespace RunningDinner.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}