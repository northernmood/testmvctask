using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace testmvc.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext() : base("DefaultConnection")
        {
        }

        public DbSet<UserModel> Users { get; set; }
    }
}