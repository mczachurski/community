using System;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.Practices.ServiceLocation;
using SunLine.Community.Common;
using SunLine.Community.Entities.Core;
using SunLine.Community.Entities.Dict;
using SunLine.Community.Repositories.Core;
using SunLine.Community.Repositories.Dict;
using SunLine.Community.Repositories.Infrastructure;
using File = System.IO.File;

namespace SunLine.Community.NUnitTests
{
    static class DatabaseHelper
    {
        public static void CreateTemplateData()
        {
            try
            {
                CreateUsers();
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        public static void ClearTemplateDatabase()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string script = File.ReadAllText("ClearDatabase.sql");
                SqlCommand command = new SqlCommand(script, connection);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
        }

        public static User UserTest1
        {
            get { return GetUserByUserName("usertest1"); }
        }

        public static User UserTest2A
        {
            get { return GetUserByUserName("usertest2A"); }
        }

        public static User UserTest2B
        {
            get { return GetUserByUserName("usertest2B"); }
        }

        public static User UserTest3
        {
            get { return GetUserByUserName("usertest3"); }
        }

        public static User UserTest4
        {
            get { return GetUserByUserName("usertest4"); }
        }

        public static User UserTest5
        {
            get { return GetUserByUserName("usertest5"); }
        }

        public static User GetUserByUserName(string userName)
        {
            IUserRepository userRepository = ServiceLocator.Current.GetInstance<IUserRepository>();
            var user = userRepository.FindByUserName(userName);
            if (user == null)
            {
                throw new ArgumentException(string.Format("User doesn't exists - {0}", userName));
            }

            return user;
        }

        public static Message CreateValidMessage(
            User user, 
            MessageStateEnum stateEnum = MessageStateEnum.Published, 
            string mind = "Fake message.",
            string speech = "",
            int amountOfFavourites = 0,
            DateTime? creationDate = null,
            Message quotedMessage = null,
            Message commentedMessage = null)
        {
            IMessageStateRepository messageStateRepository = ServiceLocator.Current.GetInstance<IMessageStateRepository>();
            ILanguageRepository laguageRepository = ServiceLocator.Current.GetInstance<ILanguageRepository>();
            Language language = laguageRepository.FindByCode("PL");

            return new Message 
            { 
                Id = Guid.NewGuid(), 
                User = user,
                Mind = mind,
                MessageState = messageStateRepository.FindByEnum(stateEnum),
                AmountOfFavourites = amountOfFavourites,
                CreationDate = creationDate ?? DateTime.UtcNow,
                QuotedMessage = quotedMessage,
                CommentedMessage = commentedMessage,
                Speech = speech,
                Language = language
            };
        }

        public static UserMessage CreateValidUserMessage(
            Message message, 
            User user, 
            bool wasTransmitted = false, 
            bool haveMention = false, 
            UserMessageCreationModeEnum modeEnum = UserMessageCreationModeEnum.ByObservedNew,
            UserMessageStateEnum stateEnum = UserMessageStateEnum.Unreaded)
        {
            IUserMessageStateRepository userMessageStateRepository = ServiceLocator.Current.GetInstance<IUserMessageStateRepository>();
            IUserMessageCreationModeRepository userMessageCreationModeRepository = ServiceLocator.Current.GetInstance<IUserMessageCreationModeRepository>();

            return new UserMessage
            {
                Id = Guid.NewGuid(),
                Message = message,
                User = user,
                UserMessageState = userMessageStateRepository.FindByEnum(stateEnum),
                WasTransmitted = wasTransmitted,
                HaveMention = haveMention,
                UserMessageCreationMode = userMessageCreationModeRepository.FindByEnum(modeEnum),
                SortingDate = DateTime.UtcNow
            };
        }

        public static User CreateValidUser(string userName, string firstName = "FirstName", string lastName = "LastName")
        {
            ILanguageRepository laguageRepository = ServiceLocator.Current.GetInstance<ILanguageRepository>();
            Language language = laguageRepository.FindByCode("PL");

            return new User
            {
                Id = Guid.NewGuid(),
                UserName = userName,
                FirstName = firstName,
                LastName = lastName,
                Language = language,
                CreationDate = DateTime.UtcNow,
                Email = userName + "@unknown.to",
                GravatarHash = userName.CalculateMd5Hash()
            };
        }

        private static void CreateUsers()
        {
            IUserRepository userRepository = ServiceLocator.Current.GetInstance<IUserRepository>();
            IUserConnectionRepository userConnectionRepository = ServiceLocator.Current.GetInstance<IUserConnectionRepository>();
            IUnitOfWork unitOfWork = ServiceLocator.Current.GetInstance<IUnitOfWork>();

            var usertest1 = CreateValidUser("usertest1", "Martin", "Test1");
            var usertest2A = CreateValidUser("usertest2A", "Arnold", "Test2A");
            var usertest2B = CreateValidUser("usertest2B", "Victor", "Test2B");
            var usertest3 = CreateValidUser("usertest3", "Victor", "Test3");
            var usertest4 = CreateValidUser("usertest4", "Victor", "Test4");
            var usertest5 = CreateValidUser("usertest5", "Victor", "Test5");
            var usertest6 = CreateValidUser("usertest6", "Victor", "Test6");
            var usertest7 = CreateValidUser("usertest7", "Victor", "Test7");
            var usertest8 = CreateValidUser("usertest8", "Victor", "Test8");
            var usertest9 = CreateValidUser("usertest9", "Victor", "Test9");
            var unknown1 = CreateValidUser("unknown1", "Victor", "Unknown1");
            var unknown2 = CreateValidUser("unknown2", "Victor", "Unknown2");

            userRepository.Create(usertest1);
            userRepository.Create(usertest2A);
            userRepository.Create(usertest2B);
            userRepository.Create(usertest3);
            userRepository.Create(usertest4);
            userRepository.Create(usertest5);
            userRepository.Create(usertest6);
            userRepository.Create(usertest7);
            userRepository.Create(usertest8);
            userRepository.Create(usertest9);
            userRepository.Create(unknown1);
            userRepository.Create(unknown2);

            userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = usertest1, ToUser = usertest2A });
            userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = usertest2A, ToUser = usertest2B });
            userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = usertest2A, ToUser = usertest3 });
            userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = usertest2B, ToUser = usertest2A });
            userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = usertest3, ToUser = usertest4 });
            userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = usertest4, ToUser = usertest5 });
            userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = usertest5, ToUser = usertest6 });
            userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = usertest6, ToUser = usertest7 });
            userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = usertest7, ToUser = usertest8 });
            userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = usertest8, ToUser = usertest9 });
            userConnectionRepository.Create(new UserConnection { Id = Guid.NewGuid(), FromUser = unknown1, ToUser = unknown2 });

            unitOfWork.Commit();
        }
    }
}
