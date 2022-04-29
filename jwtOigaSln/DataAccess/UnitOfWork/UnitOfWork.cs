using DataAccess.Repositories;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private OigaContext context;
        public UnitOfWork(OigaContext context)
        {
            this.context = context;
            User = new UserRepository(this.context);
            UserLogin = new UserLoginRepository();
        }
        public IUserRepository User
        {
            get;
            private set;
        }

        public IUserLoginRepository UserLogin
        {
            get;
            private set;
        }

        public void Dispose()
        {
            context.Dispose();
        }
        public int Save()
        {
            return context.SaveChanges();
        }
    }
}
