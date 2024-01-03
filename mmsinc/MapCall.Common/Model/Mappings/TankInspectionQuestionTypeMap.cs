using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class TankInspectionQuestionTypeMap : ClassMap<TankInspectionQuestionType>
    {
        #region Constructors

        public TankInspectionQuestionTypeMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Description).Length(TankInspectionQuestionType.StringLengths.DESCRIPTION).Not.Nullable(); 
            References(x => x.TankInspectionQuestionGroup, "GroupId").Not.Nullable();
        }

        #endregion
    }
}
