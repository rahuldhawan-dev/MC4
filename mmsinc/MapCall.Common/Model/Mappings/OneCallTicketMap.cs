using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class OneCallTicketMap : ClassMap<OneCallTicket>
    {
        #region Constructor

        public OneCallTicketMap()
        {
            Id(x => x.Id);

            Map(x => x.County)
               .Length(OneCallTicket.StringLengths.COUNTY)
               .Nullable();
            Map(x => x.Excavator)
               .Length(OneCallTicket.StringLengths.EXCAVATOR)
               .Nullable();
            Map(x => x.ExcavatorAddress)
               .Length(OneCallTicket.StringLengths.EXCAVATOR_ADDRESS)
               .Nullable();
            Map(x => x.ExcavatorPhone, "ExcavatorPh")
               .Length(OneCallTicket.StringLengths.EXCAVATOR_PHONE)
               .Nullable();
            Map(x => x.NearestCrossStreet, "NearIntersect")
               .Length(OneCallTicket.StringLengths.NEAREST_CROSS_STREET)
               .Nullable();
            Map(x => x.RequestNumber, "RequestNum")
               .Length(OneCallTicket.StringLengths.REQUEST_NUMBER);
            Map(x => x.State)
               .Length(OneCallTicket.StringLengths.STATE)
               .Nullable();
            Map(x => x.Street)
               .Length(OneCallTicket.StringLengths.STREET)
               .Nullable();
            Map(x => x.Town)
               .Length(OneCallTicket.StringLengths.TOWN)
               .Nullable();
        }

        #endregion
    }
}
