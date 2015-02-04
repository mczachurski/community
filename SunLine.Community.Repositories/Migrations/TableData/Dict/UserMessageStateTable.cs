using System;
using System.Linq;
using SunLine.Community.Entities.Dict;

namespace SunLine.Community.Repositories.Migrations.TableData.Dict
{
    public static class UserMessageStateTable
    {
        public static void Initialize(IDatabaseContext context)
        {
            AddUserMessageState(context, UserMessageStateEnum.Unreaded, "Unreaded");
            AddUserMessageState(context, UserMessageStateEnum.Readed, "Readed");
            AddUserMessageState(context, UserMessageStateEnum.Deleted, "Deleted");
        }

        private static void AddUserMessageState(IDatabaseContext context, UserMessageStateEnum userMessageStateEnum, string name)
        {
            if (!context.UserMessageStates.Any(x => x.UserMessageStateEnum == userMessageStateEnum))
            {
                context.UserMessageStates.Add(new UserMessageState { Name = name, UserMessageStateEnum = userMessageStateEnum, CreationDate = DateTime.UtcNow, Version = 1 });
            }
        }
    }
}
