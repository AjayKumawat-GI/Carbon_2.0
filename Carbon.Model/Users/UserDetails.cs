using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carbon.Model.Users
{
    public class UserDetails
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime created_date { get; set; }
        public DateTime modified_date { get; set; }
        public bool is_active { get; set; }
    }
}
