﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using testmvc.Models;

namespace testmvc.Repository
{
    public class UsersRepository : RepositoryBase<UserModel>, IUsersRepository
    {
        private UsersContext usersContext;

        public UsersRepository() : base(new UsersContext())
        {

        }

        public UsersRepository(DbContext context) : base(context)
        {
            
        }
    }
}