﻿using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class OigaContext : DbContext
    {
        public OigaContext(DbContextOptions options) : base(options) { }

        public DbSet<User> User
        {
            get;
            set;
        }
    }
}