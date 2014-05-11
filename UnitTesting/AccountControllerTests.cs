using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using testmvc.Controllers;
using testmvc.Repository;
using testmvc.Models;
using System.Linq;
using AutoMapper;
using System.Web.Mvc;
using testmvc.WebSecurityImpl;
using Moq;

namespace UnitTesting
{
    [TestClass]
    public class AccountControllerTests
    {
        private Mock<IWebSecurityWrapper> webSecurityMock;
        private Mock<IUsersRepository> usersRepositoryMock;

        [TestInitialize]
        public void Initialize()
        {
            webSecurityMock = new Mock<IWebSecurityWrapper>();
            usersRepositoryMock = new UsersRepositoryMock();
        }

        [TestMethod]
        public void LoginOfRegisteredUserShouldRedirectToHomeIndex()
        {
            webSecurityMock.Setup(t => t.Login("user", "pass", true)).Returns(true);
            LoginUserViewModel user = new LoginUserViewModel { LoginName = "user", Password = "pass", RememberMe = true };

            AccountController controller = new AccountController(usersRepositoryMock.Object, webSecurityMock.Object);
            ActionResult actionResult = controller.Login(user);
            
            Assert.IsInstanceOfType(actionResult, typeof(RedirectToRouteResult));

            RedirectToRouteResult redirect = (RedirectToRouteResult)actionResult;

            Assert.AreEqual(redirect.RouteValues["action"], "index");
            Assert.AreEqual(redirect.RouteValues["controller"], "home");
        }

        [TestMethod]
        public void LoginOfInvalidUserShouldReturnViewWithUserData()
        {
            LoginUserViewModel user = new LoginUserViewModel { LoginName = "user", Password = "", RememberMe = true };

            AccountController controller = new AccountController(usersRepositoryMock.Object, webSecurityMock.Object);
            ActionResult actionResult = controller.Login(user);

            Assert.IsInstanceOfType(actionResult, typeof(ViewResult));

            ViewResult viewResult = (ViewResult)actionResult;

            Assert.AreEqual(viewResult.Model, user);
        }
    }
}
