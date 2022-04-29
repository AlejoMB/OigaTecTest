using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UserLoginRepository: IUserLoginRepository
    {
        public IEnumerable<UserLogin> GetAllMocked()
        {
            return  new List<UserLogin>()
            {
                new() { UserName = "alejo_admin", EmailAddress = "alejo.admin@email.com", Password = "MyPass_w0rd", GivenName = "Alejo", SureName = "Moncada", Role = "Administrator" },
                new() { UserName = "Jose_standard", EmailAddress = "Jose.standard@email.com", Password = "MyPass_w0rd", GivenName = "Jose", SureName = "Carvajal", Role = "Standard" },
            };
        }

        public UserLogin get(string userName, string password)
        {
            return GetAllMocked().FirstOrDefault(o => o.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase) && o.Password.Equals(password));
             
        }
    }
}
