using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using testmvc.Models;
using testmvc.Repository;

namespace testmvc.Api
{
    public class UsersController : ApiController
    {
        public IUsersRepository usersRepository { get; set; }

        public UsersController(IUsersRepository repository)
        {
            usersRepository = repository;
        }

        // GET api/users?page=1&pageSize=5
        [Authorize]
        public IEnumerable<UserModel> Get(int page, int pageSize = 1)
        {
            return usersRepository.All().OrderBy(c => c.UserId).Skip(pageSize * page).Take(pageSize);
        }

        // PUT api/users/5
        public void Put(int id, [FromBody]string value)
        {
        }
    }
}
