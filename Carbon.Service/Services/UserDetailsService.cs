using Carbon.Data.Repositories.Interfaces;
using Carbon.Model.Users;
using Carbon.Service.Interfaces;
using Carbon.Utility.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carbon.Service.Services
{
    public class UserDetailsService : IUserDetailsService
    {
        private readonly IUserDetailsRepository _userDetailsRepository;
        public UserDetailsService(IUserDetailsRepository userDetailsRepository)
        {
            _userDetailsRepository = userDetailsRepository;
        }
        
        public async Task<UserDetails> GetByIdAsync(int id)
        {
            var userDetails = await _userDetailsRepository.GetByIdAsync(id);

            if(userDetails == null)
            {
                throw new UniversalException("User not found!");
            }

            return userDetails;
        }
    }
}
