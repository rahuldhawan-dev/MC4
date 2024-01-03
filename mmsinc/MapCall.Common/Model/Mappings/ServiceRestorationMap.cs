using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ServiceRestorationMap : ClassMap<ServiceRestoration>
    {
        #region Constructors

        public ServiceRestorationMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.ApprovedOn).Nullable();
            Map(x => x.Cancel).Not.Nullable();
            Map(x => x.EstimatedRestorationAmount).Nullable();
            Map(x => x.FinalRestorationAmount).Nullable();
            Map(x => x.FinalRestorationCost).Nullable();
            Map(x => x.FinalRestorationDate).Nullable();
            Map(x => x.FinalRestorationInvoiceNumber).Nullable();
            Map(x => x.FinalRestorationTrafficControlHours).Nullable();
            Map(x => x.Notes).Nullable();
            Map(x => x.PartialRestorationAmount).Nullable();
            Map(x => x.PartialRestorationCost).Nullable();
            Map(x => x.PartialRestorationDate).Nullable();
            Map(x => x.PartialRestorationInvoiceNumber).Nullable();
            Map(x => x.PartialRestorationTrafficControlHours).Nullable();
            Map(x => x.RejectedOn).Nullable();
            Map(x => x.PurchaseOrderNumber).Nullable();
            Map(x => x.EstimatedValue).Nullable();

            References(x => x.ApprovedBy).Nullable();
            References(x => x.RejectedBy).Nullable();
            References(x => x.FinalRestorationMethod).Nullable();
            References(x => x.PartialRestorationMethod).Nullable();
            References(x => x.RestorationType).Nullable();
            References(x => x.Service).Nullable();
            References(x => x.InitiatedBy).Nullable();
            References(x => x.FinalRestorationCompletionBy, "FinalRestorationCompletionBy").Nullable();
            References(x => x.PartialRestorationCompletionBy, "PartialRestorationCompletionBy").Nullable();

            Map(x => x.Approved).Formula("(CASE WHEN (ApprovedOn is not null) THEN 1 ELSE 0 END)");
        }

        #endregion
    }
}
