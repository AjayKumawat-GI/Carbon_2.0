using Carbon.Data.Infrastructure;
using Carbon.Data.Repositories.Interfaces;
using Carbon.Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carbon.Data.Repositories
{
    public class UserDetailsRepository : Repository<UserDetails>, IUserDetailsRepository
    {
        public UserDetailsRepository(CarbonDbContext context) : base(context)
        {
        }
    }
}
