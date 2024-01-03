using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class StandardOperatingProcedureMap : ClassMap<StandardOperatingProcedure>
    {
        public const string TABLE_NAME = "tblSOP";

        public StandardOperatingProcedureMap()
        {
            Table(TABLE_NAME);
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("SOP_ID");

            References(x => x.Section).Column("Section_Number").Nullable();
            References(x => x.SubSection).Column("Sub_Section_Number").Nullable();
            References(x => x.PolicyPractice).Column("PP_ID").Nullable();
            References(x => x.OperatingCenter).Column("OpCode").Nullable();
            References(x => x.FunctionalArea).Column("Functional_Area").Nullable();
            References(x => x.Status).Column("SOP_Status").Nullable();
            References(x => x.Category).Column("SOP_Category").Nullable();
            References(x => x.System).Column("SOP_System").Nullable();
            References(x => x.Facility).Column("Facility_ID").Nullable();

            Map(x => x.SopCrossRefId).Column("SOP_Cross_Ref_ID").Precision(10);
            Map(x => x.Description).Length(255);
            Map(x => x.EquipmentId).Column("Equipment_ID").Precision(10);
            Map(x => x.DateApproved).Column("Date_Approved");
            Map(x => x.DateIssued).Column("Date_Issued");
            Map(x => x.Revision).Length(255);
            Map(x => x.ReviewFrequencyDays).Column("Review_Frequency_Days").Length(255);
            Map(x => x.PsmTcpa).Column("PSM_TCPA");
            Map(x => x.Dpcc).Column("DPCC");
            Map(x => x.Osha).Column("OSHA");
            Map(x => x.Company);
            Map(x => x.Sox).Column("SOX");
            Map(x => x.Safety);
            Map(x => x.HasReviewRequirements)
               .Formula(
                    @"(CASE WHEN EXISTS (select null from StandardOperatingProcedurePositionGroupCommonNameRequirements soppgcnr where soppgcnr.StandardOperatingProcedureId = SOP_ID) THEN 1 ELSE 0 END)");

            HasMany(x => x.StandardOperatingProcedureNotes).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.StandardOperatingProcedureDocuments)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.Questions).KeyColumn("StandardOperatingProcedureId").Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.PGCNRequirements).KeyColumn("StandardOperatingProcedureId").Inverse().Cascade
                                            .AllDeleteOrphan();
            HasMany(x => x.Videos).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();

            HasManyToMany(x => x.TrainingModules)
               .Table("StandardOperatingProceduresTrainingModules")
               .ParentKeyColumn("StandardOperatingProcedureId")
               .ChildKeyColumn("TrainingModuleId")
               .Cascade.All();
        }
    }
}
