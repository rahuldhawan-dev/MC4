using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class TankInspectionQuestionMap : ClassMap<TankInspectionQuestion>
    {
        #region Constructors

        public TankInspectionQuestionMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            References(x => x.TankInspection).Nullable();
            References(x => x.TankInspectionQuestionType).Nullable();
            Map(x => x.ObservationAndComments).Length(TankInspection.StringLengths.COMMENT_STRING_LENGTH).Nullable();
            Map(x => x.RepairsNeeded).Nullable();
            Map(x => x.TankInspectionAnswer).Nullable();
            Map(x => x.CorrectiveWoDateCreated).Nullable();
            Map(x => x.CorrectiveWoDateCompleted).Nullable();
        }

        #endregion
    }
}