using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class MeterChangeOutMap : ClassMap<MeterChangeOut>
    {
        public const string STREET_ADDRESS_COMBINED =
            "(isNull(ServiceStreetNumber + ' ', '') + isNull(ServiceStreet, ''))";

        public MeterChangeOutMap()
        {
            Id(x => x.Id);

            Map(x => x.AccountNumber).Length(MeterChangeOut.StringLengths.ACCOUNT_NUMBER).Nullable();
            Map(x => x.Canvassed).Nullable();
            Map(x => x.ClickAdvantexUpdated).Not.Nullable();
            Map(x => x.ContractorDrilledLid).Nullable();
            Map(x => x.CustomerName).Length(MeterChangeOut.StringLengths.CUSTOMER_NAME).Not.Nullable();
            Map(x => x.CutBolts).Not.Nullable();
            Map(x => x.DateScheduled).Nullable();
            Map(x => x.DateStatusChanged).Nullable();
            Map(x => x.Email).Length(MeterChangeOut.StringLengths.EMAIL).Nullable();
            Map(x => x.EquipmentID).Length(MeterChangeOut.StringLengths.EQUIPMENT_ID).Nullable();
            Map(x => x.FieldNotes).Nullable();
            Map(x => x.InstalledJumperBar).Nullable();
            Map(x => x.IsNeptuneRFOnly).Nullable();
            Map(x => x.IsHotRodRFOnly).Nullable();
            Map(x => x.IsMuellerMeter).Nullable();
            Map(x => x.MeterCommentsPremise).Length(MeterChangeOut.StringLengths.METER_COMMENTS_PREMISE).Nullable();
            Map(x => x.MeterReadCode1).Length(MeterChangeOut.StringLengths.METER_READ_CODE).Nullable();
            Map(x => x.MeterReadCode2).Length(MeterChangeOut.StringLengths.METER_READ_CODE).Nullable();
            Map(x => x.MeterReadCode3).Length(MeterChangeOut.StringLengths.METER_READ_CODE).Nullable();
            Map(x => x.MeterReadCode4).Length(MeterChangeOut.StringLengths.METER_READ_CODE).Nullable();
            Map(x => x.MeterReadComment1).Length(MeterChangeOut.StringLengths.METER_READ_COMMENT).Nullable();
            Map(x => x.MeterReadComment2).Length(MeterChangeOut.StringLengths.METER_READ_COMMENT).Nullable();
            Map(x => x.MeterReadComment3).Length(MeterChangeOut.StringLengths.METER_READ_COMMENT).Nullable();
            Map(x => x.MeterReadComment4).Length(MeterChangeOut.StringLengths.METER_READ_COMMENT).Nullable();
            Map(x => x.MovedMeterToPit).Nullable();
            Map(x => x.NewRFNumber).Length(MeterChangeOut.StringLengths.NEW_RF_NUMBER).Nullable();
            Map(x => x.NewSerialNumber).Length(MeterChangeOut.StringLengths.NEW_SERIAL_NUMBER).Nullable();
            Map(x => x.NumberOfDials).Nullable();
            Map(x => x.OperatedPointOfControlAtAnyValve).Nullable();
            Map(x => x.OutRead).Nullable();
            Map(x => x.OwnerLocationOther).Length(MeterChangeOut.StringLengths.OWNER_LOCATION_OTHER).Nullable();
            Map(x => x.PartialExcavation).Not.Nullable();
            Map(x => x.PremiseNumber).Length(MeterChangeOut.StringLengths.PREMISE_NUMBER).Nullable();
            Map(x => x.RanNewWire).Nullable();
            Map(x => x.RemovedSerialNumber).Length(MeterChangeOut.StringLengths.REMOVED_SERIAL_NUMBER).Nullable();
            Map(x => x.RouteNumber).Length(MeterChangeOut.StringLengths.ROUTE_NUMBER).Nullable();
            Map(x => x.SAPOrderNumber).Length(MeterChangeOut.StringLengths.SAP_ORDER_NUMBER).Nullable();
            Map(x => x.ServiceStreet).Length(MeterChangeOut.StringLengths.SERVICE_STREET).Not.Nullable();
            Map(x => x.ServiceStreetNumber).Length(MeterChangeOut.StringLengths.SERVICE_STREET_NUMBER).Not.Nullable();
            Map(x => x.ServiceStreetAddressCombined)
               .DbSpecificFormula(STREET_ADDRESS_COMBINED, STREET_ADDRESS_COMBINED.ToSqlite());
            Map(x => x.ServicePhone).Length(MeterChangeOut.StringLengths.SERVICE_PHONE).Nullable();
            Map(x => x.ServicePhoneExtension).Length(MeterChangeOut.StringLengths.SERVICE_PHONE_EXTENSION).Nullable();
            Map(x => x.ServicePhone2).Length(MeterChangeOut.StringLengths.SERVICE_PHONE).Nullable();
            Map(x => x.ServicePhone2Extension).Length(MeterChangeOut.StringLengths.SERVICE_PHONE_EXTENSION).Nullable();
            Map(x => x.ServiceZip).Length(MeterChangeOut.StringLengths.SERVICE_ZIP).Nullable();
            Map(x => x.StartRead).Nullable();
            Map(x => x.TurnedOffWater).Nullable();
            Map(x => x.MeterTurnedOnAfterHours).Nullable();
            Map(x => x.YearInstalled).Nullable();
            Map(x => x.ProjectYear).Nullable();

            References(x => x.EndStatus, "EndStatus").Nullable();
            References(x => x.StartStatus, "StartStatus").Nullable();
            References(x => x.AssignedContractorMeterCrew).Nullable();
            References(x => x.CalledInByContractorMeterCrew).Nullable();
            References(x => x.MeterChangeOutStatus).Nullable();
            References(x => x.MeterDirection).Nullable();
            References(x => x.MeterLocation).Nullable();
            References(x => x.MeterSize).Nullable();
            References(x => x.MeterSupplementalLocation).Nullable();
            References(x => x.Contract, "MeterChangeOutContractId").Not.Nullable();
            References(x => x.OwnerCustomerMeterLocation).Nullable();
            References(x => x.RFDirection).Nullable();
            References(x => x.RFLocation).Nullable();
            References(x => x.RFSupplementalLocation).Nullable();
            References(x => x.MeterScheduleTime, "ScheduledTimeId").Nullable();
            References(x => x.ServiceCity).Nullable();
            References(x => x.WorkScope, "MeterChangeOutWorkScopeId").Nullable();
            References(x => x.TypeOfPlumbing).Nullable();

            HasMany(x => x.Documents).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.Notes).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }
    }
}
