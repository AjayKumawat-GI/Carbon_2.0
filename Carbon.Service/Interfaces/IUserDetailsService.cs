using Carbon.Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carbon.Service.Interfaces
{
    public interface IUserDetailsService
    {
        Task<UserDetails> GetByIdAsync(int id);

    }
}
