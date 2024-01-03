using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations._2017;

namespace MapCall.Common.Model.Mappings
{
    public class LEARNItemTypeMap : ClassMap<LEARNItemType>
    {
        #region Constructors

        public LEARNItemTypeMap()
        {
            Id(x => x.Id);

            Map(x => x.Description);
            Map(x => x.Abbreviation);
        }

        #endregion
    }
}
