using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Mappings.Users;
using MapCall.Common.Model.ViewModels;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;

namespace MapCall.Common.Model.Entities.Users
{
    [Serializable]
    public class User : IAdministratedUser,
        IRetrievablePasswordUser, 
        IEntityLookup, 
        IUserWithProfile,
        IThingWithNotes,
        IThingWithDocuments
    {
        #region Consts

        public struct StringLengths
        {
            public const int EMAIL = 50, 
                             MAX_FULL_NAME = 25,
                             MAX_LAST_NAME = 25,
                             MAX_EMPLOYEE_ID = 15,
                             ADDRESS = 50,
                             ALL_PHONE_NUMBERS = 12,
                             CITY = 50,
                             STATE = 50,
                             USERNAME = 20,
                             ZIP_CODE = 50;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        [Obsolete("Use Id instead")]
        public virtual int RecId => Id;

        public virtual string UniqueName => UserName;

        public virtual string UserName { get; set; }

        public virtual string FullName { get; set; }

        // TODO: I think we should consider ditching this property. As far as I know
        // we only ever use it for searching. Realistically, we could just search against
        // the FullName property instead.
        public virtual string LastName { get; set; }

        /// <summary>
        /// Returns true only if the user is a SITE ADMINISTRATOR.
        /// This is NOT a field editable on the site. 
        /// </summary>
        public virtual bool IsAdmin { get; set; }

        /// <summary>
        /// Returns true if this user is allowed to administrate/modify User records.
        /// </summary>
        [View(Description = "Check to allow this user to manage users. Specific role access is still controlled by the User Administrator role for the specific role.")]
        public virtual bool IsUserAdmin { get; set; }

        /// <summary>
        /// Set to true if the user account has access to the site.
        /// If this is false, they will not be able to access any part of MapCall.
        /// </summary>
        public virtual bool HasAccess { get; set; }
        public virtual string Email { get; set; }

        // NOTE: The address fields and phone number fields
        // are not used very often, but when they are used they're
        // used for contractors who don't have Employee data linked.
        // In particular, the Address field is (annoyingly) used to
        // mention the name of the company the user works for.
        public virtual string Address { get; set; }
        public virtual string City { get; set; }
        public virtual string State { get; set; }
        public virtual string ZipCode { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string CellPhoneNumber { get; set; }
        public virtual string FaxNumber { get; set; }

        /// <summary>
        /// The type of user. This has no functional value that I know of but
        /// the business does set these values.
        /// </summary>
        public virtual UserType UserType { get; set; }

        public virtual IList<AuthenticationLog> AuthenticationLogs { get; set; }

        public virtual DateTime? LastLoggedInAt { get; set; }

        /// <summary>
        /// Authorize.Net customer profile identifier.
        /// </summary>
        public virtual int? CustomerProfileId { get; set; }

        /// <summary>
        /// Indicates the last time the user's payment profile information,
        /// which should happen once per day.  If null, the user's profile info
        /// has never been verified, or was never found to be valid.
        /// </summary>
        public virtual DateTime? ProfileLastVerified { get; set; }

        /// <summary>
        /// The Employee record linked to this user. Not all users are employees!
        /// </summary>
        public virtual Employee Employee { get; set; }

        public virtual OperatingCenter DefaultOperatingCenter { get; set; }

        #region Unimplemented IUser properties

        // tblPermissions has a Password column but it's entirely unused.
        public virtual string Password =>
            throw new NotSupportedException("Not supported with MapCall's current implementation.");

        public virtual Guid PasswordSalt =>
            throw new NotSupportedException("Not supported with MapCall's current implementation.");

        public virtual string PasswordQuestion =>
            throw new NotSupportedException("Not supported with MapCall's current implementation.");

        public virtual string PasswordAnswer =>
            throw new NotSupportedException("Not supported with MapCall's current implementation.");

        #endregion

        #region Notes/Docs

        public virtual string TableName => UserMap.TABLE_NAME;
        public virtual IList<Document<User>> Documents { get; set; } = new List<Document<User>>();
        public virtual IList<Note<User>> Notes { get; set; } = new List<Note<User>>();
        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();
        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        #endregion

        #region Logical Properties

        /// <summary>
        /// The collection of roles that are explicitly assigned to this user.
        /// This does not include anything that comes from a RoleGroup. 
        /// </summary>
        public virtual IList<Role> Roles { get; set; }
        
        /// <summary>
        /// The collection of all roles that are assigned to this user, including
        /// both individual user roles and those that come from any assigned
        /// RoleGroups. This is the property you want to use to determine
        /// if a user has role access to something.
        /// </summary>
        public virtual IList<AggregateRole> AggregateRoles { get; set; }
        public virtual IList<RoleGroup> RoleGroups { get; set; }
        public virtual IList<AssetUpload> AssetUploads { get; set; }

        // This is an unnecssary property. It's just used with a formula
        // that returns FullName. Just use FullName.
        [Obsolete("Don't use this. Just use FullName.")]
        public virtual string Description { get; set; }

        #endregion

        #endregion

        #region Constructors

        public User()
        {
            AggregateRoles = new List<AggregateRole>();
            Roles = new List<Role>();
            RoleGroups = new List<RoleGroup>();
            AssetUploads = new List<AssetUpload>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a RoleMatch object that creates an IQueryable filter that can be used with other database queries.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="action"></param>
        /// <param name="opCenter"></param>
        /// <returns></returns>
        /// <remarks>
        /// 
        /// ONLY use this if you need to use RoleMatch.Matches as part of an IQueryable object. 
        /// 
        /// </remarks>
        public virtual RoleMatch GetQueryableMatchingRoles(IRepository<AggregateRole> roleRepo, RoleModules module,
            RoleActions? action = RoleActions.Read, OperatingCenter opCenter = null)
        {
            // Why was a null user check added? Role.User is not nullable. -Ross 10/26/2018
            var roles = roleRepo.Where(r => r.User != null && r.User.Id == Id);

            return new RoleMatch(roles, module, action.Value, opCenter);
        }

        /// <summary>
        /// Returns a RoleMatch object that uses the existing Roles property instead of querying the database. 
        /// </summary>
        /// <param name="module"></param>
        /// <param name="action"></param>
        /// <param name="opCenter"></param>
        /// <returns></returns>
        /// <remarks>
        /// 
        /// 1. No additional database queries are generated by this method unless the Roles property has not been initialized.
        /// 2. ALWAYS use this method if you are not going to use RoleMatch.Matches as part of another IQueryable query.
        /// 
        /// </remarks>
        public virtual RoleMatch GetCachedMatchingRoles(RoleModules module, RoleActions? action = RoleActions.Read,
            OperatingCenter opCenter = null)
        {
            // RoleMatch calls AsQueryable internally. As of NHibernate 5, this converts the child Roles collection
            // back to an actual database-queryable object, completely defeating the purpose of using a cached
            // collection of roles. GetCachedMatchingRoles is used in a lot of places, including with menu rendering,
            // so we don't really want to hit the database over and over when the Roles property is already fully
            // initialized. 
            //
            // Use a Collection to bypass the AsQueryable thing. Collections only act as wrappers around whatever
            // list reference it's given, rather than copying all the items internally.
            var cachedCollection = new Collection<AggregateRole>(AggregateRoles);
            return new RoleMatch(cachedCollection, module, action.Value, opCenter);
        }

        public virtual UserAdministrativeRoleAccess GetUserAdministrativeRoleAccess()
        {
            return new UserAdministrativeRoleAccess(this);
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override string ToString()
        {
            return UserName;
        }

        #endregion
    }
}
