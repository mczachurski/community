using System;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using SunLine.Community.Entities.Core;
using SunLine.Community.Repositories.Core;
using SunLine.Community.Services.Core;
using System.Collections.Generic;
using System.Linq;
using SunLine.Community.Repositories.Infrastructure;

namespace SunLine.Community.NUnitTests.Services.Core
{
    [TestFixture]
    public class UserConnectionServiceTest : BaseTest
    {
        IUserRepository _userReposiotry;
        IUserConnectionService _userConnectionService;
        IUserConnectionRepository _userConnectionRepository;
        private IUnitOfWork _unitOfWork;

        public override void SetUp()
        {
            base.SetUp();

            _userReposiotry = ServiceLocator.Current.GetInstance<IUserRepository>();
            _userConnectionService = ServiceLocator.Current.GetInstance<IUserConnectionService>();
            _userConnectionRepository = ServiceLocator.Current.GetInstance<IUserConnectionRepository>();
            _unitOfWork = ServiceLocator.Current.GetInstance<IUnitOfWork>();
        }

        [Test]
        public void connection_must_be_created_after_toggle_when_connection_not_exists()
        {
            var user1 = DatabaseHelper.CreateValidUser("user1");
            var user2 = DatabaseHelper.CreateValidUser("user2");
            _userReposiotry.Create(user1);
            _userReposiotry.Create(user2);
            _unitOfWork.Commit();

            bool userHaveConnection = _userConnectionService.ToggleConnection(user1.Id, user2.Id);
            _unitOfWork.Commit();

            Assert.IsTrue(userHaveConnection, "User must have connection after toggle connection");
            Assert.IsTrue(_userConnectionService.IsConnectionBetweenUsers(user1.Id, user2.Id), "User must have connection after toggle connection");
        }

        [Test]
        public void connection_must_be_removed_after_toggle_when_connection_exists()
        {
            var user1 = DatabaseHelper.CreateValidUser("user3");
            var user2 = DatabaseHelper.CreateValidUser("user4");
            _userReposiotry.Create(user1);
            _userReposiotry.Create(user2);
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user1, ToUser = user2 });
            _unitOfWork.Commit();

            bool userHaveConnection = _userConnectionService.ToggleConnection(user1.Id, user2.Id);
            _unitOfWork.Commit();

            Assert.IsFalse(userHaveConnection, "User must not have connection after toggle connection");
            Assert.IsFalse(_userConnectionService.IsConnectionBetweenUsers(user1.Id, user2.Id), "User must not have connection after toggle connection");
        }

        [Test]
        public void service_must_return_correct_amount_of_users_observed_by_user()
        {
            var user1 = DatabaseHelper.CreateValidUser("user5");
            var user2 = DatabaseHelper.CreateValidUser("user6");
            var user3 = DatabaseHelper.CreateValidUser("user7");
            var user4 = DatabaseHelper.CreateValidUser("user8");
            _userReposiotry.Create(user1);
            _userReposiotry.Create(user2);
            _userReposiotry.Create(user3);
            _userReposiotry.Create(user4);
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user1, ToUser = user2 });
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user1, ToUser = user3 });
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user1, ToUser = user4 });
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user2, ToUser = user1 });
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user3, ToUser = user4 });
            _unitOfWork.Commit();

            int amount = _userConnectionService.AmountOfAllObservedByUser(user1.Id);
            _unitOfWork.Commit();

            Assert.AreEqual(3, amount, "Service must return correct amount of users observed by user");
        }

        [Test]
        public void service_must_return_correct_amount_of_users_that_observe_user()
        {
            var user1 = DatabaseHelper.CreateValidUser("user9");
            var user2 = DatabaseHelper.CreateValidUser("user10");
            var user3 = DatabaseHelper.CreateValidUser("user11");
            var user4 = DatabaseHelper.CreateValidUser("user12");
            _userReposiotry.Create(user1);
            _userReposiotry.Create(user2);
            _userReposiotry.Create(user3);
            _userReposiotry.Create(user4);
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user1, ToUser = user2 });
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user3, ToUser = user1 });
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user1, ToUser = user4 });
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user2, ToUser = user1 });
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user3, ToUser = user4 });
            _unitOfWork.Commit();

            int amount = _userConnectionService.AmountOfAllUserObservers(user1.Id);
            _unitOfWork.Commit();

            Assert.AreEqual(2, amount, "Service must return correct amount of users observing user");
        }

        [Test]
        public void service_must_return_expected_users_that_observe_user()
        {
            var user1 = DatabaseHelper.CreateValidUser("user13");
            var user2 = DatabaseHelper.CreateValidUser("user14");
            var user3 = DatabaseHelper.CreateValidUser("user15");
            var user4 = DatabaseHelper.CreateValidUser("user16");
            _userReposiotry.Create(user1);
            _userReposiotry.Create(user2);
            _userReposiotry.Create(user3);
            _userReposiotry.Create(user4);
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user1, ToUser = user2, CreationDate = DateTime.UtcNow.AddDays(-5) });
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user3, ToUser = user1, CreationDate = DateTime.UtcNow.AddDays(-4) });
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user1, ToUser = user4, CreationDate = DateTime.UtcNow.AddDays(-3) });
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user2, ToUser = user1, CreationDate = DateTime.UtcNow.AddDays(-2) });
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user3, ToUser = user4, CreationDate = DateTime.UtcNow.AddDays(-1) });
            _unitOfWork.Commit();

            IList<User> users = _userConnectionService.FindUserObservers(user1.Id, 0, 10);
            _unitOfWork.Commit();

            Assert.AreEqual(2, users.Count, "Service must return correct amount of users observing user");
            Assert.IsNotNull(users.FirstOrDefault(x => x.Id == user2.Id), "Service must return expected user that observe user (1)");
            Assert.IsNotNull(users.FirstOrDefault(x => x.Id == user3.Id), "Service must return expected user that observe user (2)");
        }

        [Test]
        public void service_must_return_expected_users_that_observe_user_in_correct_order()
        {
            var user1 = DatabaseHelper.CreateValidUser("user17");
            var user2 = DatabaseHelper.CreateValidUser("user18");
            var user3 = DatabaseHelper.CreateValidUser("user19");
            var user4 = DatabaseHelper.CreateValidUser("user20");
            _userReposiotry.Create(user1);
            _userReposiotry.Create(user2);
            _userReposiotry.Create(user3);
            _userReposiotry.Create(user4);
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user1, ToUser = user2 }).CreationDate = DateTime.UtcNow.AddDays(-5);
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user3, ToUser = user1 }).CreationDate = DateTime.UtcNow.AddDays(-4);
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user1, ToUser = user4 }).CreationDate = DateTime.UtcNow.AddDays(-3);
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user2, ToUser = user1 }).CreationDate = DateTime.UtcNow.AddDays(-2);
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user3, ToUser = user4 }).CreationDate = DateTime.UtcNow.AddDays(-1);
            _unitOfWork.Commit();

            IList<User> users = _userConnectionService.FindUserObservers(user1.Id, 0, 10);
            _unitOfWork.Commit();

            Assert.AreEqual(2, users.Count, "Service must return correct amount of users observing user");
            Assert.AreEqual(user2.Id, users[0].Id, "Service must return expected user that observe user in correct order (1)");
            Assert.AreEqual(user3.Id, users[1].Id, "Service must return expected user that observe user in correct order (2)");
        }

        [Test]
        public void service_must_return_expected_users_that_observe_user_when_we_specify_page()
        {
            var user1 = DatabaseHelper.CreateValidUser("user21");
            var user2 = DatabaseHelper.CreateValidUser("user22");
            var user3 = DatabaseHelper.CreateValidUser("user23");
            var user4 = DatabaseHelper.CreateValidUser("user24");
            var user5 = DatabaseHelper.CreateValidUser("user25");
            var user6 = DatabaseHelper.CreateValidUser("user26");
            var user7 = DatabaseHelper.CreateValidUser("user27");
            var user8 = DatabaseHelper.CreateValidUser("user28");
            _userReposiotry.Create(user1);
            _userReposiotry.Create(user2);
            _userReposiotry.Create(user3);
            _userReposiotry.Create(user4);
            _userReposiotry.Create(user5);
            _userReposiotry.Create(user6);
            _userReposiotry.Create(user7);
            _userReposiotry.Create(user8);
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user1, ToUser = user2 }).CreationDate = DateTime.UtcNow.AddDays(-9);
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user3, ToUser = user1 }).CreationDate = DateTime.UtcNow.AddDays(-8);
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user1, ToUser = user4 }).CreationDate = DateTime.UtcNow.AddDays(-7);
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user2, ToUser = user1 }).CreationDate = DateTime.UtcNow.AddDays(-6);
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user3, ToUser = user4 }).CreationDate = DateTime.UtcNow.AddDays(-5);
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user5, ToUser = user1 }).CreationDate = DateTime.UtcNow.AddDays(-4);
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user6, ToUser = user1 }).CreationDate = DateTime.UtcNow.AddDays(-3);
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user7, ToUser = user1 }).CreationDate = DateTime.UtcNow.AddDays(-2);
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user8, ToUser = user1 }).CreationDate = DateTime.UtcNow.AddDays(-1);
            _unitOfWork.Commit();

            IList<User> users = _userConnectionService.FindUserObservers(user1.Id, 2, 2);
            _unitOfWork.Commit();

            Assert.AreEqual(2, users.Count, "Service must return correct amount of users observing user when we get users from specific page");
            Assert.IsNotNull(users.FirstOrDefault(x => x.Id == user2.Id), "Service must return expected user that observe user when we get users from specific page (1)");
            Assert.IsNotNull(users.FirstOrDefault(x => x.Id == user3.Id), "Service must return expected user that observe user when we get users from specific pate (2)");
        }

        [Test]
        public void service_must_return_expected_users_that_are_observed_by_user()
        {
            var user1 = DatabaseHelper.CreateValidUser("user29");
            var user2 = DatabaseHelper.CreateValidUser("user30");
            var user3 = DatabaseHelper.CreateValidUser("user31");
            var user4 = DatabaseHelper.CreateValidUser("user32");
            _userReposiotry.Create(user1);
            _userReposiotry.Create(user2);
            _userReposiotry.Create(user3);
            _userReposiotry.Create(user4);
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user1, ToUser = user2 });
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user3, ToUser = user1 });
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user1, ToUser = user4 });
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user2, ToUser = user1 });
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user3, ToUser = user4 });
            _unitOfWork.Commit();

            IList<User> users = _userConnectionService.FindObservedByUser(user1.Id, 0, 10);
            _unitOfWork.Commit();

            Assert.AreEqual(2, users.Count, "Service must return correct amount of users that are observing by user");
            Assert.IsNotNull(users.FirstOrDefault(x => x.Id == user2.Id), "Service must return expected user that are observing by user (1)");
            Assert.IsNotNull(users.FirstOrDefault(x => x.Id == user4.Id), "Service must return expected user that are observing by user (2)");
        }

        [Test]
        public void service_must_return_expected_users_that_are_observed_by_user_in_correct_order()
        {
            var user1 = DatabaseHelper.CreateValidUser("user33");
            var user2 = DatabaseHelper.CreateValidUser("user34");
            var user3 = DatabaseHelper.CreateValidUser("user35");
            var user4 = DatabaseHelper.CreateValidUser("user36");
            _userReposiotry.Create(user1);
            _userReposiotry.Create(user2);
            _userReposiotry.Create(user3);
            _userReposiotry.Create(user4);
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user1, ToUser = user2 }).CreationDate = DateTime.UtcNow.AddDays(-5);
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user3, ToUser = user1 }).CreationDate = DateTime.UtcNow.AddDays(-4);
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user1, ToUser = user4 }).CreationDate = DateTime.UtcNow.AddDays(-3);
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user2, ToUser = user1 }).CreationDate = DateTime.UtcNow.AddDays(-2);
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user3, ToUser = user4 }).CreationDate = DateTime.UtcNow.AddDays(-1);
            _unitOfWork.Commit();

            IList<User> users = _userConnectionService.FindObservedByUser(user1.Id, 0, 10);
            _unitOfWork.Commit();

            Assert.AreEqual(2, users.Count, "Service must return correct amount of users that are observing by user");
            Assert.AreEqual(user4.Id, users[0].Id, "Service must return expected user that are observing by user in correct order (1)");
            Assert.AreEqual(user2.Id, users[1].Id, "Service must return expected user that are observing by user in correct order (2)");
        }

        [Test]
        public void service_must_return_expected_users_that_are_observing_by_user_when_we_specify_page()
        {
            var user1 = DatabaseHelper.CreateValidUser("user37");
            var user2 = DatabaseHelper.CreateValidUser("user38");
            var user3 = DatabaseHelper.CreateValidUser("user39");
            var user4 = DatabaseHelper.CreateValidUser("user40");
            var user5 = DatabaseHelper.CreateValidUser("user41");
            var user6 = DatabaseHelper.CreateValidUser("user42");
            var user7 = DatabaseHelper.CreateValidUser("user43");
            var user8 = DatabaseHelper.CreateValidUser("user44");
            _userReposiotry.Create(user1);
            _userReposiotry.Create(user2);
            _userReposiotry.Create(user3);
            _userReposiotry.Create(user4);
            _userReposiotry.Create(user5);
            _userReposiotry.Create(user6);
            _userReposiotry.Create(user7);
            _userReposiotry.Create(user8);
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user1, ToUser = user2 }).CreationDate = DateTime.UtcNow.AddDays(-9);
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user3, ToUser = user1 }).CreationDate = DateTime.UtcNow.AddDays(-8);
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user1, ToUser = user4 }).CreationDate = DateTime.UtcNow.AddDays(-7);
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user2, ToUser = user1 }).CreationDate = DateTime.UtcNow.AddDays(-6);
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user3, ToUser = user4 }).CreationDate = DateTime.UtcNow.AddDays(-5);
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user1, ToUser = user5 }).CreationDate = DateTime.UtcNow.AddDays(-4);
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user1, ToUser = user6 }).CreationDate = DateTime.UtcNow.AddDays(-3);
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user1, ToUser = user7 }).CreationDate = DateTime.UtcNow.AddDays(-2);
            _userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = user1, ToUser = user8 }).CreationDate = DateTime.UtcNow.AddDays(-1);
            _unitOfWork.Commit();

            IList<User> users = _userConnectionService.FindObservedByUser(user1.Id, 2, 2);
            _unitOfWork.Commit();

            Assert.AreEqual(2, users.Count, "Service must return correct amount of users that are observing by user when we get users from specific page");
            Assert.IsNotNull(users.FirstOrDefault(x => x.Id == user4.Id), "Service must return expected user that are observing by user when we get users from specific page (1)");
            Assert.IsNotNull(users.FirstOrDefault(x => x.Id == user2.Id), "Service must return expected user that are observing by user when we get users from specific pate (2)");
        }
    }
}
