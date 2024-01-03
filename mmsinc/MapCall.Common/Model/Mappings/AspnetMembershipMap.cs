using System;
using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class AspnetMembershipMap : ClassMap<AspnetMembership>
    {
        public AspnetMembershipMap()
        {
            Table("aspnet_Membership");

            LazyLoad();
            ReadOnly();

            Id(x => x.UserId).GeneratedBy.Assigned().Column("UserId");

            References(x => x.AspnetApplications).Column("ApplicationId").Not.Nullable();
            References(x => x.AspnetUsers).Column("UserId").Not.Nullable().ReadOnly();

            Map(x => x.Password).Column("Password").Not.Nullable();
            Map(x => x.PasswordFormat).Column("PasswordFormat").Not.Nullable();
            Map(x => x.PasswordSalt).Column("PasswordSalt").Not.Nullable();
            Map(x => x.MobilePin).Column("MobilePIN").Nullable();
            Map(x => x.Email).Column("Email").Nullable();
            Map(x => x.LoweredEmail).Column("LoweredEmail").Nullable();
            Map(x => x.PasswordQuestion).Column("PasswordQuestion").Nullable();
            Map(x => x.PasswordAnswer).Column("PasswordAnswer").Nullable();
            Map(x => x.IsApproved).Column("IsApproved").Not.Nullable();
            Map(x => x.IsLockedOut).Column("IsLockedOut").Not.Nullable();
            Map(x => x.CreateDate).Column("CreateDate").Not.Nullable();
            Map(x => x.LastLoginDate).Column("LastLoginDate").Not.Nullable();
            Map(x => x.LastPasswordChangedDate).Column("LastPasswordChangedDate").Not.Nullable();
            Map(x => x.LastLockoutDate).Column("LastLockoutDate").Not.Nullable();
            Map(x => x.FailedPasswordAttemptCount).Column("FailedPasswordAttemptCount").Not.Nullable();
            Map(x => x.FailedPasswordAttemptWindowStart).Column("FailedPasswordAttemptWindowStart").Not.Nullable();
            Map(x => x.FailedPasswordAnswerAttemptCount).Column("FailedPasswordAnswerAttemptCount").Not.Nullable();
            Map(x => x.FailedPasswordAnswerAttemptWindowStart)
               .Column("FailedPasswordAnswerAttemptWindowStart").Not.Nullable();
            Map(x => x.Comment).Column("Comment").Nullable();
        }
    }
}
