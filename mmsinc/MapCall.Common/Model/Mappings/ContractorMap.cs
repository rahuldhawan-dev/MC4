using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ContractorMap : ClassMap<Contractor>
    {
        public ContractorMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Column("ContractorID");

            References(x => x.Street);
            References(x => x.Town).Column("CityID");
            References(x => x.State);

            Map(x => x.Name).Not.Nullable().Length(Contractor.StringLengths.NAME);
            Map(x => x.HouseNumber).Length(Contractor.StringLengths.HOUSE_NUMBER);
            Map(x => x.ApartmentNumber).Length(Contractor.StringLengths.APARTMENT_NUMBER);
            Map(x => x.Zip).Length(Contractor.StringLengths.ZIP);
            Map(x => x.Phone).Length(Contractor.StringLengths.PHONE);
            Map(x => x.IsUnionShop).Not.Nullable();
            Map(x => x.IsBcpPartner).Not.Nullable();
            Map(x => x.IsActive).Not.Nullable();
            Map(x => x.QualityControlContactId);
            Map(x => x.SafetyContactId);
            Map(x => x.VendorId).Length(Contractor.StringLengths.VENDOR_ID);
            Map(x => x.CreatedBy).Not.Nullable();
            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.ContractorsAccess).Not.Nullable();
            Map(x => x.AWR).Nullable();

            HasMany(x => x.Crews).KeyColumn("ContractorID");
            HasMany(x => x.Users).KeyColumn("ContractorID");
            HasMany(x => x.WorkOrders).KeyColumn("AssignedContractorID");
            HasMany(x => x.Insurances).KeyColumn("ContractorID");
            HasMany(x => x.Contacts).KeyColumn("ContractorID").Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.Documents).KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Notes).KeyColumn("LinkedId").Inverse().Cascade.None();

            HasManyToMany(x => x.OperatingCenters)
               .Table("ContractorsOperatingCenters")
               .ParentKeyColumn("ContractorID")
               .ChildKeyColumn("OperatingCenterID")
               .Cascade.All();
            HasManyToMany(x => x.FrameworkOperatingCenters)
               .Table("ContractorsFrameworkOperatingCenters")
               .ParentKeyColumn("ContractorId")
               .ChildKeyColumn("OperatingCenterID")
               .Cascade.All();
            HasManyToMany(x => x.FunctionalAreas)
               .Table("ContractorsFunctionalAreas")
               .ParentKeyColumn("ContractorID")
               .ChildKeyColumn("ContractorFunctionalAreaTypeID")
               .Cascade.All();
            HasManyToMany(x => x.WorkCategories)
               .Table("ContractorsWorkCategories")
               .ParentKeyColumn("ContractorID")
               .ChildKeyColumn("ContractorWorkCategoryTypeID")
               .Cascade.All();
        }
    }
}
