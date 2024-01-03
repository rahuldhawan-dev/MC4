using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ServiceRestorationContractorMap : ClassMap<ServiceRestorationContractor>
    {
        #region Constants

        public struct StringLengths
        {
            public const int CONTRACTOR = 30;
        }

        #endregion

        #region Constructors

        public ServiceRestorationContractorMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Not.Nullable();

            Map(x => x.Contractor).Not.Nullable().Length(StringLengths.CONTRACTOR);
            Map(x => x.FinalRestoration).Not.Nullable();
            Map(x => x.PartialRestoration).Not.Nullable();

            Map(x => x.HasRestorations)
               .Formula(
                    $"(CASE WHEN EXISTS (SELECT 1 FROM {nameof(ServiceRestoration)}s r WHERE r.FinalRestorationCompletionBy = Id OR r.PartialRestorationCompletionBy = Id) THEN 1 ELSE 0 END)");

            References(x => x.OperatingCenter).Not.Nullable();
        }

        #endregion
    }
}
