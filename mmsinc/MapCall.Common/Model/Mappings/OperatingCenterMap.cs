using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class OperatingCenterMap : ClassMap<OperatingCenter>
    {
        #region Constructors

        public OperatingCenterMap()
        {
            Id(x => x.Id, "OperatingCenterID");

            Map(x => x.CompanyInfo, "CoInfo")
               .Length(OperatingCenter.MaxLengths.COMPANY_INFO);
            Map(x => x.FaxNumber, "FaxNum")
               .Length(OperatingCenter.MaxLengths.FAX_NUMBER);
            Map(x => x.HydrantInspectionFrequency)
               .Not.Nullable();
            Map(x => x.HydrantPaintingFrequency)
               .Nullable();
            Map(x => x.SewerOpeningInspectionFrequency)
               .Not.Nullable();
            Map(x => x.ZoneStartYear).Nullable();
            Map(x => x.PaintingZoneStartYear).Nullable();
            Map(x => x.LargeValveInspectionFrequency)
               .Not.Nullable();
            Map(x => x.MailingAddressStreet, "MailAdd")
               .Length(OperatingCenter.MaxLengths.MAILING_ADDRESS_STREET);
            Map(x => x.MailingAddressName, "MailCo")
               .Length(OperatingCenter.MaxLengths.MAILING_ADDRESS_NAME);
            Map(x => x.MailingAddressCityStateZip, "MailCSZ")
               .Length(OperatingCenter.MaxLengths.MAILING_ADDRESS_CITY_STATE_ZIP);
            Map(x => x.OperatingCenterCode)
               .Length(OperatingCenter.MaxLengths.OPERATING_CENTER_CODE)
               .Unique();
            Map(x => x.OperatingCenterName)
               .Length(OperatingCenter.MaxLengths.OPERATING_CENTER_NAME);
            Map(x => x.PermitsOMUserName)
               .Length(OperatingCenter.MaxLengths.PERMITS_OM_USER_NAME);
            Map(x => x.PermitsCapitalUserName)
               .Length(OperatingCenter.MaxLengths.PERMITS_CAPITAL_USER_NAME);
            Map(x => x.PhoneNumber, "CSNum")
               .Length(OperatingCenter.MaxLengths.PHONE_NUMBER);
            Map(x => x.ServiceContactPhoneNumber, "ServContactNum")
               .Length(OperatingCenter.MaxLengths.SERVICE_CONTACT_PHONE_NUMBER);
            Map(x => x.SmallValveInspectionFrequency)
               .Not.Nullable();
            Map(x => x.WorkOrdersEnabled).Not.Nullable();
            Map(x => x.HasWorkOrderInvoicing).Not.Nullable();
            Map(x => x.IsContractedOperations).Not.Nullable();
            Map(x => x.RSADivisionNumber);
            Map(x => x.IsActive).Not.Nullable();
            Map(x => x.MaximumOverflowGallons).Nullable();
            Map(x => x.InfoMasterMapId).Nullable().Length(OperatingCenter.MaxLengths.INFOMASTER_MAP_ID);
            Map(x => x.InfoMasterMapLayerName).Nullable();
            Map(x => x.SAPEnabled).Not.Nullable();
            Map(x => x.SAPWorkOrdersEnabled).Not.Nullable();
            Map(x => x.UsesValveInspectionFrequency).Not.Nullable();
            Map(x => x.MarkoutsEditable).Not.Nullable();
            Map(x => x.DataCollectionMapUrl).Nullable();
            Map(x => x.MapId).Nullable().Length(OperatingCenter.MaxLengths.MAP_ID);
            Map(x => x.ArcMobileMapId).Nullable().Length(OperatingCenter.MaxLengths.ARC_MOBILE_MAP_ID);

            References(x => x.HydrantInspectionFrequencyUnit)
               .Not.Nullable();
            References(x => x.HydrantPaintingFrequencyUnit)
               .Nullable();
            References(x => x.SewerOpeningInspectionFrequencyUnit)
               .Not.Nullable();
            References(x => x.LargeValveInspectionFrequencyUnit)
               .Not.Nullable();
            References(x => x.SmallValveInspectionFrequencyUnit)
               .Not.Nullable();
            References(x => x.DefaultServiceReplacementWBSNumber)
               .Nullable();
            References(x => x.State)
               .Not.Nullable();
            References(x => x.OperatedByOperatingCenter).Nullable();
            References(x => x.StateRegion).Nullable();
            References(x => x.Coordinate).Nullable();
            References(x => x.TimeZone).Nullable();

            HasMany(x => x.AsBuiltImages).KeyColumn("OperatingCenterID").LazyLoad();
            HasMany(x => x.BusinessUnits).KeyColumn("OperatingCenterID").LazyLoad();
            HasMany(x => x.BappTeams).KeyColumn("OperatingCenterId").LazyLoad();
            HasMany(x => x.Employees).KeyColumn("OperatingCenterId").LazyLoad();
            HasMany(x => x.Facilities).KeyColumn("OperatingCenterId").LazyLoad();
            HasMany(x => x.PlanningPlants).KeyColumn("OperatingCenterId").LazyLoad();
            HasMany(x => x.StockLocations).KeyColumn("OperatingCenterID");

            HasManyToMany(x => x.Contractors)
               .Table("ContractorsOperatingCenters")
               .ParentKeyColumn("OperatingCenterID")
               .ChildKeyColumn("ContractorID")
               .Cascade.All();

            HasManyToMany(x => x.ContractorLaborCosts)
               .Table("ContractorLaborCostsOperatingCenters")
               .ParentKeyColumn("OperatingCenterId")
               .ChildKeyColumn("ContractorLaborCostId")
               .Cascade.All();

            HasMany(x => x.OperatingCenterTowns)
               .KeyColumn("OperatingCenterId")
               .Cascade.All()
               .Inverse();

            HasMany(x => x.OperatingCenterPublicWaterSupplies)
               .KeyColumn("OperatingCenterId")
               .Cascade.All()
               .Inverse();

            HasMany(x => x.OperatingCenterServiceMaterials)
               .KeyColumn("OperatingCenterId")
               .Cascade.All().Inverse();

            HasMany(x => x.OperatingCenterAssetTypes)
               .KeyColumn("OperatingCenterID")
               .Cascade.AllDeleteOrphan()
               .Inverse();

            HasManyToMany(x => x.WaterSystems)
               .Table("OperatingCenterWaterSystems")
               .ParentKeyColumn("OperatingCenterID")
               .ChildKeyColumn("WaterSystemID")
               .Cascade.All();

            HasMany(x => x.StockedMaterials)
               .KeyColumn("OperatingCenterId")
               .Cascade.All()
               .Inverse();
        }

        #endregion
    }
}
