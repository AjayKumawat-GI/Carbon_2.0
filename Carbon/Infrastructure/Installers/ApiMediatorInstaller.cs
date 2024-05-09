using Microsoft.AspNetCore.Authentication;
using Carbon.API.Infrastructure.Installers;
using Carbon.Mediator.Infrastructure;

namespace Carbon.API.Infrastructure.Installers
{
    public class ApiMediatorInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services
                //ApiClients
                .AddTransient(typeof(IApiClient), typeof(ApiClient));
        }
    }
}
