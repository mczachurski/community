using System;
using SunLine.Community.Entities.Core;

namespace SunLine.Community.Services.Core
{
    public interface IErrorService
    {
        Error Create(string errorMessage);
        Error Create(Guid userId, string errorMessage);
    }
}
