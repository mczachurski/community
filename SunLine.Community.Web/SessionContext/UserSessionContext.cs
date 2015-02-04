using System;

namespace SunLine.Community.Web.SessionContext
{
    public class UserSessionContext
    {
        private readonly Guid _userId;
        private readonly string _userName;
        private readonly string _email;
        private readonly string _firstName;
        private readonly string _lastName;
        private readonly Guid _languageId;
        private readonly string _gravatarHash;

        public UserSessionContext(Guid userId, string userName, string email, string firstName, string lastName, string gravatarHash, Guid languageId)
        {
            _languageId = languageId;
            _userName = userName;
            _email = email;
            _userId = userId;
            _firstName = firstName;
            _lastName = lastName;    
            _gravatarHash = gravatarHash;
        }

        public string UserName
        {
            get { return _userName; }
        }

        public Guid LanguageId
        {
            get { return _languageId; }
        }

        public string Email
        {
            get { return _email; }
        }

        public Guid UserId
        {
            get { return _userId; }
        }

        public string FirstName
        {
            get { return _firstName; }
        }

        public string LastName
        {
            get { return _lastName; }
        }

        public string FullName
        {
            get { return string.Format("{0} {1}", _firstName, _lastName); }
        }

        public string GravatarHash
        {
            get { return _gravatarHash; }
        }
    }
}