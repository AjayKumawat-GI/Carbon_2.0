using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Carbon.Database.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Carbon.Data;

namespace Carbon.API.Infrastructure.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CarbonDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Con")));
            //Configure DB service and after that uncomment the UnitOfWork Part
            //services.AddSingleton<IUnitOfWork, UnitOfWork>();
        }
    }
}
