using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class TownMap : ClassMap<Town>
    {
        #region Constructors

        public TownMap()
        {
            Id(x => x.Id, "TownID");

            Map(x => x.Address)
               .Length(Town.StringLengths.ADDRESS);
            Map(x => x.ContactName)
               .Length(Town.StringLengths.CONTACT_NAME);
            Map(x => x.CountyName, "County")
               .Length(Town.StringLengths.COUNTY);
            Map(x => x.DistrictId);
            Map(x => x.EmergencyContact, "EmergContact")
               .Length(Town.StringLengths.EMERGENCY_CONTACT);
            Map(x => x.EmergencyFax, "EmergFax")
               .Length(Town.StringLengths.EMERGENCY_FAX);
            Map(x => x.EmergencyPhone, "EmergPhone")
               .Length(Town.StringLengths.EMERGENCY_PHONE);
            Map(x => x.Fax)
               .Length(Town.StringLengths.FAX);
            Map(x => x.FD1Contact)
               .Length(Town.StringLengths.FD1_CONTACT);
            Map(x => x.FD1Fax)
               .Length(Town.StringLengths.FD1_FAX);
            Map(x => x.FD1Phone)
               .Length(Town.StringLengths.FD1_PHONE);
            Map(x => x.Phone)
               .Length(Town.StringLengths.PHONE);
            Map(x => x.StateAbbreviation, "State")
               .Length(Town.StringLengths.STATE);
            Map(x => x.ShortName, "Town")
               .Length(Town.StringLengths.TOWN);
            Map(x => x.FullName, "TownName")
               .Length(Town.StringLengths.TOWN_NAME);
            Map(x => x.Zip)
               .Length(Town.StringLengths.ZIP);
            Map(x => x.Link)
               .Length(Town.StringLengths.LINK);
            Map(x => x.Lat)
               .Length(Town.StringLengths.LAT);
            Map(x => x.Lon)
               .Length(Town.StringLengths.LON);
            Map(x => x.OperatingSection)
               .Length(Town.StringLengths.OPERATING_SECTION);
            Map(x => x.SharedOpCenter);

            Map(x => x.DBA)
               .Length(Town.StringLengths.DBA);

            Map(x => x.CriticalMainBreakNotes).Length(Town.StringLengths.CRITICAL_MAIN_BREAK_NOTES).Nullable();

            References(x => x.State);
            References(x => x.County);
            References(x => x.AbbreviationType)
               .Nullable();

            //HasManyToMany(x => x.OperatingCenters)
            //    .Table("OperatingCentersTowns")
            //    .ParentKeyColumn("TownID")
            //    .ChildKeyColumn("OperatingCenterID")
            //    .Cascade.All();

            HasManyToMany(x => x.PublicWaterSupplies)
               .Table("PublicWaterSuppliesTowns")
               .ParentKeyColumn("TownID")
               .ChildKeyColumn("PublicWaterSupplyID")
               .Cascade.All();

            HasManyToMany(x => x.WasteWaterSystems)
               .Table(nameof(WasteWaterSystem) + "s" + nameof(Town) + "s")
               .ParentKeyColumn("TownId")
               .ChildKeyColumn("WasteWaterSystemId")
               .Cascade.All();

            HasManyToMany(x => x.Gradients)
               .Table("GradientTowns")
               .ParentKeyColumn("TownId")
               .ChildKeyColumn("GradientID");

            HasMany(x => x.AsBuiltImages)
               .KeyColumn("TownID").LazyLoad();
            HasMany(x => x.TownDocuments)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.TownNotes)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.TownFireDistricts)
               .KeyColumn("TownId");
            HasMany(x => x.Facilities)
               .KeyColumn("TownId").LazyLoad();

            HasMany(x => x.TownContacts)
               .KeyColumn("TownId")
               .Cascade.AllDeleteOrphan()
               .Inverse();

            HasMany(x => x.TownSections)
               .KeyColumn("TownId")
               .Cascade.AllDeleteOrphan()
               .Inverse();

            HasMany(x => x.OperatingCentersTowns)
               .KeyColumn("TownId")
               .Cascade.All()
               .Inverse();

            HasMany(x => x.FunctionalLocations)
               .KeyColumn("TownID").LazyLoad();
            HasMany(x => x.Streets)
               .KeyColumn("TownID").LazyLoad();
        }

        #endregion
    }
}
