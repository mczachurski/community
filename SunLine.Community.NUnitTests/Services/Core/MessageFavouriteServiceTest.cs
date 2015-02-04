using System;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using SunLine.Community.Entities.Core;
using SunLine.Community.Repositories.Core;
using SunLine.Community.Services.Core;
using SunLine.Community.Repositories.Infrastructure;

namespace SunLine.Community.NUnitTests.Services.Core
{
    [TestFixture]
    public class MessageFavouriteServiceTest : BaseTest
    {
        IMessageFavouriteService _messageFavouriteService;
        IMessageRepository _messageRepository;
        IUserMessageRepository _userMessageRepository;
        IMessageFavouriteRepository _favouriteUserMessageRepository;
        IUnitOfWork _unitOfWork;

        public override void SetUp()
        {
            base.SetUp();

            _messageFavouriteService = ServiceLocator.Current.GetInstance<IMessageFavouriteService>();
            _messageRepository = ServiceLocator.Current.GetInstance<IMessageRepository>();
            _userMessageRepository = ServiceLocator.Current.GetInstance<IUserMessageRepository>();
            _favouriteUserMessageRepository = ServiceLocator.Current.GetInstance<IMessageFavouriteRepository>();
            _unitOfWork = ServiceLocator.Current.GetInstance<IUnitOfWork>();
        }

        [Test]
        public void favourite_must_be_enabled_after_toggle()
        {
            var message = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest4);
            var userMessage = DatabaseHelper.CreateValidUserMessage(message, DatabaseHelper.UserTest4);
            _messageRepository.Create(message);
            _userMessageRepository.Create(userMessage);
            _unitOfWork.Commit();

            _messageFavouriteService.ToggleFavourite(userMessage.User.Id, userMessage.Message);
            _unitOfWork.Commit();

            var favourite = _favouriteUserMessageRepository.FindAll().FirstOrDefault(x => x.User.Id == DatabaseHelper.UserTest4.Id && x.Message.Id == message.Id);
            Assert.IsNotNull(favourite, "User favourite message must exists after toggle favorite (enable)");
        }

        [Test]
        public void favourite_must_increment_amount_of_favourites_on_main_message_after_disabled()
        {
            var message = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest4);
            var userMessage = DatabaseHelper.CreateValidUserMessage(message, DatabaseHelper.UserTest4);
            _messageRepository.Create(message);
            _userMessageRepository.Create(userMessage);
            _unitOfWork.Commit();

            _messageFavouriteService.ToggleFavourite(userMessage.User.Id, userMessage.Message);
            _unitOfWork.Commit();

            Assert.AreEqual(1, message.AmountOfFavourites, "Amount od favourites must be equal 1 after toggle favourite (enable)");
        }

        [Test]
        public void favourite_must_be_disabled_after_toggle()
        {
            var message = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest4, amountOfFavourites: 10);
            var userMessage = DatabaseHelper.CreateValidUserMessage(message, DatabaseHelper.UserTest4);
            var favourite = new MessageFavourite { User = DatabaseHelper.UserTest4, Message = message };
            _messageRepository.Create(message);
            _userMessageRepository.Create(userMessage);
            _favouriteUserMessageRepository.Create(favourite);
            _unitOfWork.Commit();

            _messageFavouriteService.ToggleFavourite(userMessage.User.Id, userMessage.Message);
            _unitOfWork.Commit();

            favourite = _favouriteUserMessageRepository.FindAll().FirstOrDefault(x => x.User.Id == DatabaseHelper.UserTest4.Id && x.Message.Id == message.Id);
            Assert.IsNull(favourite, "User favourite message must not exists after toggle favorite (disable)");
        }

        [Test]
        public void favourite_must_decrement_amount_of_favourites_on_main_message_after_disabled()
        {
            var message = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest4, amountOfFavourites: 10);
            var userMessage = DatabaseHelper.CreateValidUserMessage(message, DatabaseHelper.UserTest4);
            var favourite = new MessageFavourite { User = DatabaseHelper.UserTest4, Message = message };
            _messageRepository.Create(message);
            _userMessageRepository.Create(userMessage);
            _favouriteUserMessageRepository.Create(favourite);
            _unitOfWork.Commit();

            _messageFavouriteService.ToggleFavourite(userMessage.User.Id, userMessage.Message);
            _unitOfWork.Commit();

            Assert.AreEqual(9, message.AmountOfFavourites, "Amount of favourites must be equal 9 after toggle favourite (disable)");
        }

        [Test]
        public void service_must_return_true_when_message_is_favourite_by_user()
        {
            var message = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest4);
            var favouite = new MessageFavourite { Message = message, User = DatabaseHelper.UserTest4, Id = Guid.NewGuid() };
            _messageRepository.Create(message);
            _favouriteUserMessageRepository.Create(favouite);
            _unitOfWork.Commit();

            bool isFavourite = _messageFavouriteService.IsUserFavouriteMessage(DatabaseHelper.UserTest4.Id, message.Id);
            _unitOfWork.Commit();

            Assert.IsTrue(isFavourite, "Service must return true when message is favourited by user");
        }

        [Test]
        public void service_must_return_false_when_message_is_favourite_by_user()
        {
            var message = DatabaseHelper.CreateValidMessage(DatabaseHelper.UserTest4);
            _messageRepository.Create(message);
            _unitOfWork.Commit();

            bool isFavourite = _messageFavouriteService.IsUserFavouriteMessage(DatabaseHelper.UserTest4.Id, message.Id);
            _unitOfWork.Commit();

            Assert.IsFalse(isFavourite, "Service must return false when message is favourited by user");
        }
    }
}

