using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class UnitOfMeasureMap : ClassMap<UnitOfMeasure>
    {
        public const string TABLE_NAME = "UnitsOfMeasure";

        public UnitOfMeasureMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id).GeneratedBy.Identity().Column("UnitOfMeasureID");
            Map(x => x.Description).Not.Nullable().Unique().Length(UnitOfMeasure.StringLengths.DESCRIPTION);
            Map(x => x.SAPCode).Nullable();
        }
    }
}
