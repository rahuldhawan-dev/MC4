using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Data.NHibernate;
using MapIcons = MapCall.Common.Model.Entities.MapIcon.Indices;

namespace MapCall.Common.Model.Mappings
{
    public class PremiseMap : ClassMap<Premise>
    {
        #region Constants

        public const string
            SQL_FULL_STREET_NUMBER =
                "Replace(isNull(ServiceAddressHouseNumber + ' ', '') + isNull(ServiceAddressFraction + ' ', ''), '  ', ' ')",
            SQL_FULL_STREET_ADDRESS = "Replace(" + SQL_FULL_STREET_NUMBER +
                                      " + isNull(ServiceAddressStreet + ' ', ''), '  ', ' ')",
            SQL_NEXT_METER_READING_DATE =
                "(SELECT Min(md.ReadingDate) FROM MeterReadingRouteReadingDates md WHERE md.MeterReadingRouteId = RouteNumberId AND md.ReadingDate > getDate())",
            TABLE_NAME = "Premises";
        #endregion

        #region Constructors

        public PremiseMap()
        {
            Id(x => x.Id);

            References(x => x.OperatingCenter).Nullable();
            References(x => x.Coordinate).Nullable();
            References(x => x.ServiceCity).Nullable();
            References(x => x.State, "ServiceStateId").Nullable();
            References(x => x.ServiceDistrict).Nullable();
            References(x => x.AreaCode).Nullable();
            References(x => x.RegionCode).Nullable();
            References(x => x.StatusCode).Nullable();
            References(x => x.CriticalCareType).Nullable();
            References(x => x.RouteNumber).Nullable();
            References(x => x.ServiceUtilityType).Nullable();
            References(x => x.MeterSize).Nullable();
            References(x => x.MeterLocation).Nullable();
            References(x => x.PublicWaterSupply).Nullable();
            References(x => x.PlanningPlant).Nullable();
            References(x => x.PremiseType).Nullable();
            References(x => x.Icon)
               .Formula(
                    "(CASE " +
                    // blue orange
                    $"WHEN ({nameof(Premise.IsMajorAccount)} = 1 AND " +
                    $"{nameof(Premise.CriticalCareType)}Id IS NOT NULL) THEN " +
                    $"{MapIcons.PREMISE_BLUE_ORANGE} " +
                    // mablue
                    $"WHEN {nameof(Premise.IsMajorAccount)} = 1 THEN {MapIcons.PREMISE_MABLUE} " +
                    // orange
                    $"WHEN {nameof(Premise.CriticalCareType)}Id IS NOT NULL THEN " +
                    $"{MapIcons.PREMISE_ORANGE} " +
                    // blue
                    $"ELSE {MapIcons.PREMISE_BLUE} " +
                    "END)");
            
            Map(x => x.PremiseNumber)
               .CustomType("AnsiString")
               .Length(Premise.StringLengths.PREMISE_NUMBER)
               .Not.Nullable();
            Map(x => x.ConnectionObject).Length(Premise.StringLengths.CONNECTION_OBJECT).Nullable();
            Map(x => x.DeviceCategory).Length(Premise.StringLengths.DEVICE_CATEGORY).Nullable();
            Map(x => x.DeviceLocation).Length(Premise.StringLengths.DEVICE_LOCATION).Nullable();
            Map(x => x.Equipment).Length(Premise.StringLengths.EQUIPMENT).Nullable();
            Map(x => x.NextMeterReadingDate)
               .DbSpecificFormula(SQL_NEXT_METER_READING_DATE, SQL_NEXT_METER_READING_DATE.ToSqlite());
            Map(x => x.DeviceSerialNumber).Length(Premise.StringLengths.SERIAL_NUMBER).Nullable();
            Map(x => x.ServiceAddressHouseNumber).Length(Premise.StringLengths.SERVICE_ADDRESS_HOUSE_NUMBER).Nullable();
            Map(x => x.ServiceAddressFraction).Length(Premise.StringLengths.SERVICE_ADDRESS_FRACTION).Nullable();
            Map(x => x.ServiceAddressApartment).Length(Premise.StringLengths.SERVICE_ADDRESS_APARTMENT).Nullable();
            Map(x => x.ServiceAddressStreet).Length(Premise.StringLengths.SERVICE_ADDRESS_STREET).Nullable();
            Map(x => x.ServiceZip).Length(Premise.StringLengths.SERVICE_ZIP).Nullable();
            Map(x => x.MeterLocationFreeText).Length(int.MaxValue).Nullable();
            Map(x => x.Installation).Nullable();
            Map(x => x.MeterSerialNumber).Nullable();
            
            Map(x => x.IsMajorAccount).Not.Nullable();
            Map(x => x.MajorAccountManager).Length(Premise.StringLengths.MAJOR_ACCOUNT_MANAGER).Nullable();
            Map(x => x.AccountManagerContactNumber).Length(Premise.StringLengths.ACCOUNT_MANAGER_CONTACT_NUMBER)
                                                   .Nullable();
            Map(x => x.AccountManagerEmail).Length(Premise.StringLengths.ACCOUNT_MANAGER_EMAIL).Nullable();
            Map(x => x.FullStreetNumber).DbSpecificFormula(SQL_FULL_STREET_NUMBER, SQL_FULL_STREET_NUMBER.ToSqlite());
            Map(x => x.FullStreetAddress)
               .DbSpecificFormula(SQL_FULL_STREET_ADDRESS, SQL_FULL_STREET_ADDRESS.ToSqlite());

            HasOne(x => x.MostRecentService);

            HasMany(x => x.Services).KeyColumn("PremiseId");
            HasMany(x => x.ShortCycleCustomerMaterials).KeyColumn("PremiseId");
            HasMany(x => x.SampleSites).KeyColumn("PremiseId").Inverse().Cascade.None();
            HasMany(x => x.Documents).KeyColumn("LinkedId").Inverse().Cascade.None();
        }

        #endregion
    }
}
