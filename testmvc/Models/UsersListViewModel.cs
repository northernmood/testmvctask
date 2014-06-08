using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testmvc.Models
{
    public class UsersListViewModel
    {
        public UsersListViewModel()
        {
            Users = new List<UserModel>();
        }

        public UsersListViewModel(IEnumerable<UserModel> list)
        {
            Users = new List<UserModel>(list);
        }

        public List<UserModel> Users { get; set; }
    }
}