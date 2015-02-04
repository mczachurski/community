using System;

namespace SunLine.Community.Entities
{
    public interface IBaseEntity
    {
        Guid Id { get; set; }
        int Version { get; set; }
        DateTime CreationDate { get; set; }
        DateTime? ModificationDate { get; set; }
    }
}
