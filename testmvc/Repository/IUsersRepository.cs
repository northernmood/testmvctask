using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using testmvc.Models;

namespace testmvc.Repository
{
    public interface IUsersRepository : IRepository<UserModel>
    {
    }
}