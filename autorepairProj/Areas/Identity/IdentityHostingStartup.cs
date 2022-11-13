using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(autorepairProj.Areas.Identity.IdentityHostingStartup))]
namespace autorepairProj.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}