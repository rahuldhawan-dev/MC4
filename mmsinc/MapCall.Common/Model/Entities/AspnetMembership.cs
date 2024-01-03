using System;
using System.Text;
using System.Collections.Generic;

namespace MapCall.Common.Model.Entities
{
    public class AspnetMembership
    {
        public virtual System.Guid UserId { get; set; }
        public virtual AspnetApplications AspnetApplications { get; set; }
        public virtual AspnetUsers AspnetUsers { get; set; }
        public virtual string Password { get; set; }
        public virtual int PasswordFormat { get; set; }
        public virtual string PasswordSalt { get; set; }
        public virtual string MobilePin { get; set; }
        public virtual string Email { get; set; }
        public virtual string LoweredEmail { get; set; }
        public virtual string PasswordQuestion { get; set; }
        public virtual string PasswordAnswer { get; set; }
        public virtual bool IsApproved { get; set; }
        public virtual bool IsLockedOut { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual DateTime LastLoginDate { get; set; }
        public virtual DateTime LastPasswordChangedDate { get; set; }
        public virtual DateTime LastLockoutDate { get; set; }
        public virtual int FailedPasswordAttemptCount { get; set; }
        public virtual DateTime FailedPasswordAttemptWindowStart { get; set; }
        public virtual int FailedPasswordAnswerAttemptCount { get; set; }
        public virtual DateTime FailedPasswordAnswerAttemptWindowStart { get; set; }
        public virtual string Comment { get; set; }
    }
}
