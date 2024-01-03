using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ServiceInstallationMap : ClassMap<ServiceInstallation>
    {
        #region Constructors

        public ServiceInstallationMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Not.Nullable();

            Map(x => x.MeterManufacturerSerialNumber).Not.Nullable();
            Map(x => x.Manufacturer).Nullable();
            Map(x => x.ServiceType).Nullable();
            Map(x => x.MeterSerialNumber).Nullable();
            Map(x => x.MaterialNumber).Nullable();
            Map(x => x.MeterSize).Nullable();
            Map(x => x.Register1Dials).Nullable();
            Map(x => x.Register1UnitOfMeasure).Nullable();
            Map(x => x.Register1RFMIU).Not.Nullable();
            Map(x => x.Register1Size).Nullable();
            Map(x => x.Register1TPEncoderID).Nullable();
            Map(x => x.Register1CurrentRead).Not.Nullable();
            Map(x => x.RegisterTwoDials, "Register2Dials").Nullable();
            Map(x => x.Register2UnitOfMeasure).Nullable();
            Map(x => x.Register2RFMIU).Nullable();
            Map(x => x.Register2Size).Nullable();
            Map(x => x.Register2TPEncoderID).Nullable();
            Map(x => x.Register2CurrentRead).Nullable();
            //// Has an SAPCode and CodeGroup (NO - C-A-ALL3,C01, YES - C-A-ALL3,C02
            Map(x => x.OperatedPointOfControl).Not.Nullable();
            Map(x => x.MeterLocationInformation).Not.Nullable();
            Map(x => x.SAPErrorCode).Nullable();
            Map(x => x.MeterDeviceCategory).Length(25).Nullable();
            Map(x => x.Register1DeviceCategory).Length(25).Nullable();
            Map(x => x.Register2DeviceCategory).Length(25).Nullable();

            References(x => x.WorkOrder).Nullable();
            References(x => x.MeterLocation).Not.Nullable();
            References(x => x.MeterPositionalLocation).Not.Nullable();
            References(x => x.MeterDirectionalLocation).Not.Nullable();
            References(x => x.ReadingDevicePosition).Not.Nullable();
            References(x => x.ReadingDeviceSupplemental).Not.Nullable();
            References(x => x.ReadingDeviceDirectionalInformation, "ReadingDeviceDirectionalLocationId").Not.Nullable();
            References(x => x.Register1ReadType).Not.Nullable();
            References(x => x.Register2ReadType).Nullable();

            ////Activity 1
            References(x => x.Activity1).Not.Nullable();
            References(x => x.Activity2).Nullable();
            References(x => x.Activity3).Nullable();
            References(x => x.AdditionalWorkNeeded).Nullable();
            References(x => x.Purpose).Nullable();
            References(x => x.ServiceFound).Not.Nullable();
            References(x => x.ServiceLeft).Not.Nullable();
            References(x => x.ServiceInstallationReason).Not.Nullable();
            References(x => x.MiuInstallReason).Nullable();
        }

        #endregion
    }
}
