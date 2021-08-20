using FuncSFTP;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]

namespace FuncSFTP
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddOptions<SFTPOptions>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection("SFTP").Bind(settings);
                });
        }
    }
}
