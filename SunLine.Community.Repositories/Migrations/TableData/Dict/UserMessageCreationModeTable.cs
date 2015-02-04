using System;
using System.Linq;
using SunLine.Community.Entities.Dict;

namespace SunLine.Community.Repositories.Migrations.TableData.Dict
{
    public static class UserMessageCreationModeTable
    {
        public static void Initialize(IDatabaseContext context)
        {
            AddUserMessageCreationMode(context, UserMessageCreationModeEnum.ByHimselfNew, "By himself");
            AddUserMessageCreationMode(context, UserMessageCreationModeEnum.ByObservedNew, "By observed");
            AddUserMessageCreationMode(context, UserMessageCreationModeEnum.ByObservedTransmit, "Transmit by observerd");

            AddUserMessageCreationMode(context, UserMessageCreationModeEnum.ByNotObserved, "By not observed");
            AddUserMessageCreationMode(context, UserMessageCreationModeEnum.ByFavourites, "By favourites");
            AddUserMessageCreationMode(context, UserMessageCreationModeEnum.ByEditors, "By editors");
            AddUserMessageCreationMode(context, UserMessageCreationModeEnum.ByHimselfTransmitedFromUnknownFeed, "By himself transmitted from unknown feed");
        }

        private static void AddUserMessageCreationMode(IDatabaseContext context, UserMessageCreationModeEnum userMessageCreationModeEnum, string name)
        {
            if (!context.UserMessageCreationModes.Any(x => x.UserMessageCreationModeEnum == userMessageCreationModeEnum))
            {
                context.UserMessageCreationModes.Add(new UserMessageCreationMode { Name = name, UserMessageCreationModeEnum = userMessageCreationModeEnum, CreationDate = DateTime.UtcNow, Version = 1 });
            }
        }
    }
}
