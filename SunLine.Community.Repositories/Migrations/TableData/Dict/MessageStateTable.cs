using System;
using System.Linq;
using SunLine.Community.Entities.Dict;

namespace SunLine.Community.Repositories.Migrations.TableData.Dict
{
    public static class MessageStateTable
    {
        public static void Initialize(IDatabaseContext context)
        {
            AddMessageState(context, MessageStateEnum.Draft, "Draft");
            AddMessageState(context, MessageStateEnum.Published, "Published");
            AddMessageState(context, MessageStateEnum.Deleted, "Deleted");
        }

        private static void AddMessageState(IDatabaseContext context, MessageStateEnum messageStateEnum, string name)
        {
            if (!context.MessageStates.Any(x => x.MessageStateEnum == messageStateEnum))
            {
                context.MessageStates.Add(new MessageState { Name = name, MessageStateEnum = messageStateEnum, CreationDate = DateTime.UtcNow, Version = 1 });
            }
        }
    }
}
