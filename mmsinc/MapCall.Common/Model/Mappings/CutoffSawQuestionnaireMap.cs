using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;

namespace MapCall.Common.Model.Mappings
{
    public class CutoffSawQuestionnaireMap : ClassMap<CutoffSawQuestionnaire>
    {
        #region Constructors

        public CutoffSawQuestionnaireMap()
        {
            Id(x => x.Id, "CutoffSawQuestionnaireID");

            References(x => x.LeadPerson).Not.Nullable();
            References(x => x.SawOperator).Not.Nullable();
            References(x => x.PipeDiameter);
            References(x => x.PipeMaterial);
            References(x => x.WorkOrder);

            Map(x => x.WorkOrderSAP);
            Map(x => x.OperatedOn).Not.Nullable();
            Map(x => x.Comments);
            Map(x => x.CreatedBy);
            Map(x => x.CreatedAt).Not.Nullable();

            HasManyToMany(x => x.CutoffSawQuestions)
               .Table(AddCutoffSawQuestions.Tables.CUTOFF_SAW_QUESTIONNAIRES_CUTOFF_SAW_QUESTIONS)
               .ParentKeyColumn(AddCutoffSawQuestions.Columns.CUTOFF_SAW_QUESTIONNAIRE_ID)
               .ChildKeyColumn(AddCutoffSawQuestions.Columns.CUTOFF_SAW_QUESTION_ID);

            HasMany(x => x.CutoffSawQuestionnaireNotes)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None(); //.Cascade.DeleteOrphan();

            HasMany(x => x.CutoffSawQuestionnaireDocuments)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None(); //.Cascade.DeleteOrphan();
        }

        #endregion
    }
}
