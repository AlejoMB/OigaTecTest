using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserLoginRepository
    {
        IEnumerable<UserLogin> GetAllMocked();
        UserLogin get(string userName, string password);
    }
}
