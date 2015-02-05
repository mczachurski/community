using NUnit.Framework;
using Microsoft.Practices.ServiceLocation;
using SunLine.Community.Repositories.Core;
using SunLine.Community.Repositories.Infrastructure;
using SunLine.Community.Services.Core;

namespace SunLine.Community.NUnitTests.Services.Core
{
    [TestFixture]
    public class UserServiceTest : BaseTest
    {
        IUserRepository _userReposiotry;
        IUserService _userService;
        private IUnitOfWork _unitOfWork;

        public override void SetUp()
        {
            base.SetUp();

            _userReposiotry = ServiceLocator.Current.GetInstance<IUserRepository>();
            _userService = ServiceLocator.Current.GetInstance<IUserService>();
            _unitOfWork = ServiceLocator.Current.GetInstance<IUnitOfWork>();
        }

        [Test]
        public void service_must_return_expected_user_by_id()
        {
            var user1 = DatabaseHelper.CreateValidUser("user101");
            _userReposiotry.Create(user1);
            _unitOfWork.Commit();

            var userFromDb = _userService.FindById(user1.Id);

            Assert.IsNotNull(userFromDb, "Service must return user when we try get user by id");
            Assert.AreEqual(user1.Id, userFromDb.Id, "Service must return expected user when we try get user by id");
        }

        [Test]
        public void service_must_return_expected_user_by_user_name()
        {
            var user2 = DatabaseHelper.CreateValidUser("user102");
            _userReposiotry.Create(user2);
            _unitOfWork.Commit();

            var userFromDb = _userService.FindByUserName("user102");

            Assert.IsNotNull(userFromDb, "Service must return user when we try get user by user name");
            Assert.AreEqual(user2.Id, userFromDb.Id, "Service must return expected user when we try get user by user name");
        }

        [Test]
        public void service_must_allow_update_user_data()
        {
            var user2 = DatabaseHelper.CreateValidUser("user103", "First name", "Last name");
            _userReposiotry.Create(user2);
            _unitOfWork.Commit();

            user2.FirstName = "Diffrent name";
            user2.LastName = "Diffrent surname";
            _userService.Update(user2);
            _unitOfWork.Commit();

            var userFromDb = _userReposiotry.FindById(user2.Id);
            Assert.IsNotNull(userFromDb, "Service must return user when we try get user by user name");
            Assert.AreEqual("Diffrent name", userFromDb.FirstName, "Service must update user data");   
            Assert.AreEqual("Diffrent surname", userFromDb.LastName, "Service must update user data");   
        }
    }
}

