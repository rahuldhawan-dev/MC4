using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class BodyPartMap : ClassMap<BodyPart>
    {
        #region Constructors

        public BodyPartMap()
        {
            Table("BodyParts");
            Id(x => x.Id);
            Map(x => x.Description)
               .Column("Description")
                // ReSharper disable once AccessToStaticMemberViaDerivedType
               .Length(IncidentType.StringLengths.DESCRIPTION)
               .Not.Nullable();
        }

        #endregion
    }
}
