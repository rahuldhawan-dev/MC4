using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class StreetOpeningPermitMap : ClassMap<StreetOpeningPermit>
    {
        #region Constructors

        public StreetOpeningPermitMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("StreetOpeningPermitID");

            References(x => x.WorkOrder).Not.Nullable();

            Map(x => x.StreetOpeningPermitNumber)
               .Length(StreetOpeningPermit.StringLengths.STREET_OPENING_PERMIT_NUMBER)
               .Not.Nullable();
            Map(x => x.DateRequested).Not.Nullable();
            Map(x => x.DateIssued).Nullable();
            Map(x => x.ExpirationDate).Nullable();
            Map(x => x.Notes).Nullable();
            Map(x => x.PermitId).Nullable();
            Map(x => x.HasMetDrawingRequirement).Nullable();
            Map(x => x.IsPaidFor).Nullable();
        }

        #endregion
    }
}
