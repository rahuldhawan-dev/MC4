using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class MarkoutMap : ClassMap<Markout>
    {
        #region Constructors

        public MarkoutMap()
        {
            Id(x => x.Id, "MarkoutID");

            Map(x => x.MarkoutNumber)
               .Length(Markout.StringLengths.MARKOUT_NUMBER_MAX_LENGTH);
            Map(x => x.Note).Nullable();
            Map(x => x.DateOfRequest)
               .Nullable();
            Map(x => x.ReadyDate).Nullable();
            Map(x => x.ExpirationDate).Nullable();

            References(x => x.WorkOrder)
               .Not.Nullable();
            References(x => x.MarkoutType).Nullable();
        }

        #endregion
    }
}
