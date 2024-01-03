using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities.Excel;
using NHibernate;
using IContainer = StructureMap.IContainer;

namespace MMSINC.Testing.NHibernate
{
    #region Models

    public class TestUser : IRetrievablePasswordUser, IValidatableObject
    {
        #region Constants

        public const string UNIQUE_NAME_DISPLAY_NAME = "Foo";

        #endregion

        #region Properties

        public virtual int Id { get; protected set; }

        [DisplayName(UNIQUE_NAME_DISPLAY_NAME)]
        public virtual string UniqueName
        {
            get { return Email; }
        }

        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual string PasswordAnswer { get; set; }
        public virtual string PasswordQuestion { get; set; }
        public virtual Guid PasswordSalt { get; set; }
        public virtual string SearchString { get; set; }
        public virtual int? MainGroupId { get; set; }
        public virtual int? SomeForeignId { get; set; }
        public virtual bool IsAdmin { get; set; }
        public virtual bool HasAccess { get; set; }
        public virtual decimal Age { get; set; }
        public virtual decimal? SomeNullableDecimal { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public virtual DateTime? CreatedAt { get; set; }

        public virtual DateTime? UpdatedAt { get; set; }
        public virtual DateTime? LastLoggedInAt { get; set; }

        // for testing sequences:
        public virtual int SomeOrder { get; set; }
        public virtual int OtherOrder { get; set; }

        public virtual TestGroup MainGroup { get; set; }
        public virtual TestGroup OtherGroup { get; set; }
        public virtual IList<TestGroup> Groups { get; set; }

        public virtual IList<TestUserChildItem> ChildItems { get; set; }

        #region Logical Properties

        // this is an invalid property used to test exceptions being thrown
        [DoesNotExport]
        public virtual int AdministratorId
        {
            get { return MainGroup.Administrator.Id; }
        }

        #endregion

        #endregion

        #region Constructors

        public TestUser()
        {
            Groups = new List<TestGroup>();
            ChildItems = new List<TestUserChildItem>();
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }

    public class TestUserChildItem
    {
        public virtual int Id { get; set; }
        public virtual TestUser TestUser { get; set; }
    }

    public class TestAuthenticationLog : IAuthenticationLog<TestUser>
    {
        public virtual int Id { get; set; }
        public virtual TestUser User { get; set; }
        public virtual string IpAddress { get; set; }
        public virtual DateTime LoggedInAt { get; set; }
        public virtual DateTime? LoggedOutAt { get; set; }
        public virtual Guid AuthCookieHash { get; set; }
        public virtual DateTime ExpiresAt { get; set; }
    }

    public class TestGroup
    {
        public virtual int Id { get; protected set; }
        public virtual string Name { get; set; }
        public virtual int? AdministratorId { get; set; }
        public virtual TestUser Administrator { get; set; }
        public virtual IList<TestUser> Users { get; set; }
        public virtual TestFactorylessThing FactorylessThing { get; set; }
    }

    public class TestFactorylessThing
    {
        public virtual int Id { get; protected set; }
        public virtual string Name { get; set; }
    }

    public class TestEntityLookup : EntityLookup { }

    #endregion

    #region Mappings

    public class TestUserMap : ClassMap<TestUser>
    {
        public TestUserMap()
        {
            Id(x => x.Id);
            Map(x => x.Email);
            Map(x => x.Password);
            Map(x => x.PasswordSalt);
            Map(x => x.SomeForeignId);
            Map(x => x.IsAdmin);
            Map(x => x.HasAccess);
            Map(x => x.SomeOrder);
            Map(x => x.OtherOrder);
            Map(x => x.CreatedAt);
            Map(x => x.UpdatedAt);
            Map(x => x.LastLoggedInAt);
            Map(x => x.PasswordQuestion);
            Map(x => x.PasswordAnswer);
            Map(x => x.Age);
            Map(x => x.SearchString);
            Map(x => x.SomeNullableDecimal).Nullable();
            References(x => x.MainGroup)
               .Column("MainGroupId");
            References(x => x.OtherGroup)
               .Column("OtherGroupId");
            HasManyToMany(x => x.Groups)
               .Cascade.SaveUpdate()
               .Table("TestGroupsTestUsers")
               .ParentKeyColumn("TestUserId")
               .ChildKeyColumn("TestGroupId");
            HasMany(x => x.ChildItems)
               .KeyColumn("TestUser")
               .Cascade.AllDeleteOrphan();
        }
    }

    public class TestUserChildItemMap : ClassMap<TestUserChildItem>
    {
        public TestUserChildItemMap()
        {
            Id(x => x.Id);
            References(x => x.TestUser);
        }
    }

    public class TestAuthenticationLogMap : AuthenticationLogMapBase<TestAuthenticationLog, TestUser> { }

    public class TestGroupMap : ClassMap<TestGroup>
    {
        public TestGroupMap()
        {
            Id(x => x.Id);
            Map(x => x.Name).Unique();
            References(x => x.Administrator)
               .Column("AdministratorId");
            References(x => x.FactorylessThing);
            HasManyToMany(x => x.Users)
               .Cascade.SaveUpdate()
               .Table("TestGroupsTestUsers")
               .ParentKeyColumn("TestGroupId")
               .ChildKeyColumn("TestUserId");
        }
    }

    public class TestFactorylessThingMap : ClassMap<TestFactorylessThing>
    {
        public TestFactorylessThingMap()
        {
            Id(x => x.Id);
            Map(x => x.Name).Unique();
        }
    }

    public class TestEntityLookupMap : EntityLookupMap<TestEntityLookup> { }

    #endregion

    #region Repositories

    #region TestUser

    public class TestUserRepository : RepositoryBase<TestUser>, ITestUserRepository
    {
        public TestUserRepository(ISession session, IContainer container) : base(session, container) { }

        public int Count()
        {
            return Linq.Count();
        }
    }

    public interface ITestUserRepository : MMSINC.Data.NHibernate.IRepository<TestUser>
    {
        int Count();
    }

    #endregion

    #endregion

    #region Factories

    public class TestUserFactory : TestDataFactory<TestUser>
    {
        public const string DEFAULT_PASSWORD = "the password";
        public const string DEFAULT_PASSWORD_QUESTION = "the question";
        public const string DEFAULT_PASSWORD_ANSWER = "the answer";

        static TestUserFactory()
        {
            var i = 0;
            Func<String> emailFn = () => string.Format("someuser{0}@site.com", ++i);
            Func<DateTime> createdAtFn = () => DateTime.Now;

            Defaults(new {
                Email = emailFn,
                Password = DEFAULT_PASSWORD,
                PasswordQuestion = DEFAULT_PASSWORD_QUESTION,
                PasswordAnswer = DEFAULT_PASSWORD_ANSWER,
                PasswordSalt = new Guid(),
                IsAdmin = false,
                CreatedAt = createdAtFn,
                HasAccess = true
            });
            //Sequence("SomeOrder");
            // Sequence("OtherOrder");
            OnSaving((user, session) => {
                user.Password = user.Password.Salt(user.PasswordSalt);
                user.PasswordAnswer = user.PasswordAnswer.Salt(user.PasswordSalt);
            });
        }

        public TestUserFactory(IContainer container) : base(container) { }
    }

    public class AdminUserFactory : TestUserFactory
    {
        static AdminUserFactory()
        {
            Defaults(new {
                // should override IsAdmin = false from UserFactory
                IsAdmin = true
            });
        }

        public AdminUserFactory(IContainer container) : base(container) { }
    }

    public class UserFactoryWithLambdaForEmail : TestUserFactory
    {
        public const string DEFAULT_LAMBDA_EMAIL = "lambdauser@site.com";

        static UserFactoryWithLambdaForEmail()
        {
            Func<string> getEmail = () => DEFAULT_LAMBDA_EMAIL;

            Defaults(new {
                Email = getEmail
            });
        }

        public UserFactoryWithLambdaForEmail(IContainer container) : base(container) { }
    }

    public class UserWithNoDefaultsFactory : TestDataFactory<TestUser>
    {
        public UserWithNoDefaultsFactory(IContainer container) : base(container) { }
    }

    public class UserWithDefaultGroupFactory : TestUserFactory
    {
        static UserWithDefaultGroupFactory()
        {
            Defaults(new {
                MainGroup = typeof(TestGroupFactory)
            });
            OnSaving((u, s) => { u.MainGroup.Administrator = u; });
        }

        public UserWithDefaultGroupFactory(IContainer container) : base(container) { }
    }

    public class UserWithOwnGroupFactory : UserWithDefaultGroupFactory
    {
        static UserWithOwnGroupFactory()
        {
            OnSaving((u, s) => { u.MainGroup.Name = string.Format("{0}'s Group", u.Email); });
        }

        public UserWithOwnGroupFactory(IContainer container) : base(container) { }
    }

    public class TestGroupFactory : TestDataFactory<TestGroup>
    {
        static TestGroupFactory()
        {
            Defaults(new TestGroup {
                Name = "Default Group Name"
            });
        }

        public TestGroupFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region Conventions

    public class TableNameConvention : IClassConvention
    {
        public void Apply(IClassInstance instance)
        {
            instance.Table(instance.EntityType.Name + "s");
        }
    }

    #endregion
}
