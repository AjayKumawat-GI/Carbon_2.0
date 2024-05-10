using Carbon.Service.Interfaces;
using Carbon.Service.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCore.AutoRegisterDi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Carbon.API.Infrastructure.Installers
{
    public class ServicesInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var assembliesToScan = new[]{
                Assembly.GetExecutingAssembly(),
                Assembly.GetAssembly(typeof(IBaseService))
            };

            services.RegisterAssemblyPublicNonGenericClasses(assembliesToScan)
                 .Where(c => c.Name.EndsWith("Service") || c.Name.EndsWith("Logging"))
                 .AsPublicImplementedInterfaces();

            services.AddSingleton<IExceptionLogging, ExceptionLogging>();
        }
    }
}
