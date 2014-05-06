using testmvc.Repository;
using Moq;

namespace UnitTesting
{
    internal class UsersRepositoryMock : Mock<IUsersRepository>
    {
        public UsersRepositoryMock() : base(MockBehavior.Strict)
        {
        }
    }
}
