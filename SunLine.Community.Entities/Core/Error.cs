namespace SunLine.Community.Entities.Core
{
    public class Error : BaseEntity
    {
        public virtual User User { get; set; }
        public virtual string ErrorMessage { get; set; }
    }
}
