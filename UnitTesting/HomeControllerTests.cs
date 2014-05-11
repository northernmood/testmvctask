using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using testmvc.WebSecurityImpl;
using testmvc.Repository;
using testmvc.Controllers;
using System.Web.Mvc;
using testmvc.Models;
using System.Linq;

namespace UnitTesting
{
    [TestClass]
    public class HomeControllerTests
    {
        private Mock<IWebSecurityWrapper> webSecurityMock;
        private UsersRepositoryMock usersRepositoryMock;

        [TestInitialize]
        public void Initialize()
        {
            webSecurityMock = new Mock<IWebSecurityWrapper>();
            usersRepositoryMock = new UsersRepositoryMock();
        }

        [TestMethod]
        public void IndexShouldReturnViewWithAllUsersExceptLogged()
        {
            int loggedUserId = 1;
            webSecurityMock.Setup(m => m.CurrentUserId).Returns(loggedUserId);

            HomeController controller = new HomeController(usersRepositoryMock.Object, webSecurityMock.Object);
            ActionResult actionResult = controller.Index();

            Assert.IsInstanceOfType(actionResult, typeof(ViewResult));

            ViewResult view = (ViewResult)actionResult;
            Assert.IsInstanceOfType(view.Model, typeof(UsersListViewModel));

            UsersListViewModel resultModel = (UsersListViewModel)view.Model;

            Assert.IsTrue(resultModel.Users.Any());
            Assert.AreEqual(resultModel.Users.Count, usersRepositoryMock.Data.Count - 1);
            Assert.IsFalse(resultModel.Users.Any(u => u.UserId == loggedUserId));
        }
    }
}
