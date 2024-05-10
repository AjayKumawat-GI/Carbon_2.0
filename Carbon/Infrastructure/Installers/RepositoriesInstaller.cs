using Carbon.Data.Repositories;
using Carbon.Data.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carbon.API.Infrastructure.Installers
{
    public class RepositoriesInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            #region User_Details
            services.AddTransient(typeof(IUserDetailsRepository), typeof(UserDetailsRepository));
            #endregion
        }
    }
}
