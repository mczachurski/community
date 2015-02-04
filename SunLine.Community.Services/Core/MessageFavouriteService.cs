using System;
using SunLine.Community.Entities.Core;
using SunLine.Community.Repositories.Core;

namespace SunLine.Community.Services.Core
{
    [BusinessLogic]
    public class MessageFavouriteService : IMessageFavouriteService
    {
        private readonly IMessageFavouriteRepository _messageFavouriteRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;

        public MessageFavouriteService(
            IMessageFavouriteRepository messageFavouriteRepository, 
            IUserRepository userRepository, 
            IMessageRepository messageRepository)
        {
            _messageFavouriteRepository = messageFavouriteRepository;
            _userRepository = userRepository;
            _messageRepository = messageRepository;
        }

        public bool IsUserFavouriteMessage(Guid userId, Guid messageId)
        {
            return _messageFavouriteRepository.IsUserFavouriteMessage(userId, messageId);
        }

        public void ToggleFavourite(Guid userId, Message message)
        {
            var userFavouriteMessage = _messageFavouriteRepository.GetUserFavouriteMessage(userId, message.Id);
            if (userFavouriteMessage != null)
            {
                _messageFavouriteRepository.Delete(userFavouriteMessage);
                message.AmountOfFavourites--;
            }
            else
            {
                User user = _userRepository.FindById(userId);
                if (user != null)
                {
                    userFavouriteMessage = _messageFavouriteRepository.Create(new MessageFavourite
                        {
                            User = user,
                            Message = message
                        });

                    _messageFavouriteRepository.Create(userFavouriteMessage);
                    message.AmountOfFavourites++;
                }
            }

            _messageRepository.Update(message);
        }

        private  MessageFavourite GetUserFavouriteMessage(Guid userId, Guid messageId)
        {
            return _messageFavouriteRepository.GetUserFavouriteMessage(userId, messageId);
        }
    }
}

