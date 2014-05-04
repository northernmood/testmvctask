using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using testmvc.Models;

namespace testmvc.Repository
{
    public class UsersRepository : RepositoryBase<UserModel>, IUsersRepository
    {
        public UsersRepository() : this(new UsersContext())
        {

        }

        public UsersRepository(DbContext context) : base(context)
        {
            
        }
    }
}