using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities.Users;

namespace MapCall.Common.Model.Mappings.Users
{
    public class UserMap : ClassMap<User>
    {
        #region Constants

        public const string TABLE_NAME = "tblPermissions";

        #endregion

        #region Constructors

        public UserMap()
        {
            Table(TABLE_NAME);

            Id(x => x.Id, "RecId");

            Map(x => x.UserName)
               .Length(User.StringLengths.USERNAME)
               .Unique()
               .Not.Nullable();
            Map(x => x.HasAccess)
               .Not.Nullable();
            Map(x => x.FullName)
               .Length(User.StringLengths.MAX_FULL_NAME)
               .Nullable();
            Map(x => x.LastName).Length(User.StringLengths.MAX_LAST_NAME)
                                .Nullable();
            Map(x => x.Address, "Add1")
               .Length(User.StringLengths.ADDRESS)
               .Nullable();
            Map(x => x.CellPhoneNumber, "CellNum")
               .Length(User.StringLengths.ALL_PHONE_NUMBERS)
               .Nullable();
            Map(x => x.City)
               .Length(User.StringLengths.CITY)
               .Nullable();
            Map(x => x.FaxNumber, "FaxNum")
               .Length(User.StringLengths.ALL_PHONE_NUMBERS)
               .Nullable();
            Map(x => x.PhoneNumber, "PhoneNum")
               .Length(User.StringLengths.ALL_PHONE_NUMBERS)
               .Nullable();
            Map(x => x.State, "St")
               .Length(User.StringLengths.STATE)
               .Nullable();
            Map(x => x.ZipCode, "Zip")
               .Length(User.StringLengths.ZIP_CODE)
               .Nullable();
            Map(x => x.LastLoggedInAt)
               .Nullable();

            //Authorize.Net values
            Map(x => x.CustomerProfileId)
               .Nullable();
            Map(x => x.ProfileLastVerified)
               .Nullable();

            Map(x => x.Email)
               .Length(User.StringLengths.EMAIL)
               .Nullable();

            // NOTE: THIS DOES NOT USE THE ISADMINISTRATOR COLUMN
            //       IT IS A BAD COLUMN FULL OF BAD THINGS
            //       THIS IS AKIN TO ISAKEVIN.
            Map(x => x.IsAdmin, "IsSiteAdministrator")
               .Not.Nullable();
            Map(x => x.IsUserAdmin, "IsUserAdministrator")
               .Not.Nullable();
            References(x => x.DefaultOperatingCenter)
               .Not.Nullable();
            References(x => x.UserType)
               .Not.Nullable();

            // NOTE: Not all users are employees.
            // DOUBLE NOTE: User.Employee is constrained to be unique in the SQL Server
            // database, however it's done using a special index that allows for multiple
            // null values. Fluent NHibernate doesn't support that so there's no Unique constraint
            // added to the NHibernate map.
            References(x => x.Employee).Nullable();

            // TODO: This is an unncessary formula. Just make the Description
            // property return FullName in the first place.
            Map(x => x.Description)
               .Formula("FullName");

            HasMany(x => x.Roles).Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.AggregateRoles).Cascade.None(); // This is a view, so nothing should happen here.
            HasMany(x => x.AuthenticationLogs);

            HasMany(x => x.AssetUploads).KeyColumn("CreatedById");
            
            HasManyToMany(x => x.RoleGroups)
               .Table("RoleGroupsUsers")
               .ParentKeyColumn("UserId")
               .ChildKeyColumn("RoleGroupId");
            HasMany(x => x.Documents)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Notes)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }

        #endregion
    }
}
