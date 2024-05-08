using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//using Referral.Utility.AppSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Referral.API.Infrastructure.Installers
{
    public class AppSettingsInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
        }
    }
}
