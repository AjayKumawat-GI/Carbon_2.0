using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//using Carbon.Utility.AppSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carbon.API.Infrastructure.Installers
{
    public class AppSettingsInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
        }
    }
}
