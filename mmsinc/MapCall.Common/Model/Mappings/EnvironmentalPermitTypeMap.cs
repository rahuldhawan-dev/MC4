using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;

namespace MapCall.Common.Model.Mappings
{
    public class EnvironmentalPermitTypeMap : ClassMap<EnvironmentalPermitType>
    {
        #region Constructors

        public EnvironmentalPermitTypeMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Column("EnvironmentalPermitTypeID");

            Map(x => x.Description)
               .Not.Nullable()
               .Unique().Length(UpdateEnvironmentalPermitTablesForBug1627.StringLengths.DESCRIPTION);
        }

        #endregion
    }
}
