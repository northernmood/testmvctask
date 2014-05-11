using testmvc.Repository;
using Moq;
using System.Collections.Generic;
using testmvc.Models;
using System.Linq;

namespace UnitTesting
{
    public class UsersRepositoryMock : Mock<IUsersRepository>
    {
        private List<UserModel> repository = new List<UserModel> 
        { 
            new UserModel { UserId = 1, FirstName = "fname1", LastName = "lname1", LoginName = "login1", Email = "a@a.com", IsAdmin = true }, 
            new UserModel { UserId = 2, FirstName = "fname2", LastName = "lname2", LoginName = "login2", Email = "b@b.com", IsAdmin = false },
            new UserModel { UserId = 3, FirstName = "fname3", LastName = "lname3", LoginName = "login3", Email = "c@c.com", IsAdmin = false } 
        };

        public UsersRepositoryMock() : base(MockBehavior.Default)
        {
            this.Setup(m => m.All()).Returns(repository.AsQueryable<UserModel>());
            this.Setup(m => m.GetByID(It.IsAny<object>())).Returns<int>(id => repository.Find(u => u.UserId == id));
        }

        public List<UserModel> Data { get { return repository; } }
    }
}
