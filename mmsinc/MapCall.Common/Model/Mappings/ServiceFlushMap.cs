using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ServiceFlushMap : ClassMap<ServiceFlush>
    {
        public ServiceFlushMap()
        {
            Id(x => x.Id);
            Map(x => x.PremiseContactDate).Nullable(); // Nullable because it's not set until SampleStatus == Received Results
            Map(x => x.SampleDate).Not.Nullable();
            Map(x => x.NotifiedCustomerServiceCenter).Nullable(); // Nullable because it's not set until SampleStatus == Received Resultss
            Map(x => x.HasSentNotification).Not.Nullable();
            Map(x => x.SampleResultPassed).Nullable(); // Nullable because it's not set until SampleStatus == Received Results
            Map(x => x.FlushingNotes).Length(int.MaxValue).Nullable(); // ntext field
            Map(x => x.SampleId).Nullable();

            References(x => x.Service, "ServiceId").Not.Nullable();
            References(x => x.FlushType, "ServiceFlushFlushTypeId").Not.Nullable();
            References(x => x.SampleType, "ServiceFlushSampleTypeId").Not.Nullable();
            References(x => x.SampleStatus, "ServiceFlushSampleStatusId").Not.Nullable();
            References(x => x.TakenBy, "ServiceFlushSampleTakenByTypeId").Not.Nullable();
            References(x => x.ReplacementType, "ServiceFlushReplacementTypeId").Not.Nullable();
            References(x => x.CreatedBy, "CreatedByUserId").Not.Nullable();

            // Nullable because the ContactMethod won't be set until the SampleStatus == Received Results
            References(x => x.ContactMethod, "ServiceFlushPremiseContactMethodId").Nullable();
        }
    }
}
