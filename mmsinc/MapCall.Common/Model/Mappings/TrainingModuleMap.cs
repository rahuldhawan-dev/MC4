using System;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;

namespace MapCall.Common.Model.Mappings
{
    public class TrainingModuleMap : ClassMap<TrainingModule>
    {
        #region Constants

        public const string TABLE_NAME = "tblTrainingModules";

        #endregion

        public TrainingModuleMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id, "TrainingModuleID");

            References(x => x.TrainingModuleCategory).Column("TrainingModuleCategory");
            References(x => x.OperatingCenter).Column("OpCode");
            References(x => x.Facility);
            References(x => x.TrainingRequirement);
            References(x => x.TrainingModuleRecurrantType);
            References(x => x.LEARNItemType).Nullable();

            Map(x => x.Title);
            Map(x => x.CourseApprovalNumber).Column("NJDEP_TCH_COURSE_APPROVAL_NUMBER");
            Map(x => x.TCHCertified).Column("TCH_Certified");
            Map(x => x.TCHCreditValue).Column("TCH_Credit_Value");
            Map(x => x.TotalHours);
            Map(x => x.Description);
            Map(x => x.TrainingObjectives);
            Map(x => x.PracticalTestStandards);
            Map(x => x.SafetyRelated).Not.Nullable();

            Map(x => x.Production);
            Map(x => x.EquipmentID);
            Map(x => x.IsActive);
            Map(x => x.AmericanWaterCourseNumber);
            Map(x => x.HasAttachedDocuments).ReadOnly()
                                            .Formula(String.Format(
                                                 "(CASE WHEN EXISTS (SELECT 1 FROM {0} dlv WHERE dlv.TableName = '{1}' AND dlv.LinkedId = TrainingModuleId) THEN 1 ELSE 0 END)",
                                                 CreateDocumentLinkView.VIEW_NAME, TABLE_NAME));

            HasMany(x => x.TrainingRecords)
               .KeyColumn("TrainingModuleID");
            HasMany(x => x.TrainingModuleDocuments)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None(); //.Cascade.DeleteOrphan();
            HasMany(x => x.TrainingModuleNotes)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None(); //.Cascade.DeleteOrphan();
            HasMany(x => x.Videos)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();

            DiscriminateSubClassesOnColumn("", 0)
               .AlwaysSelectWithValue()
               .Formula(String.Format("(CASE TrainingModuleCategory WHEN {0} THEN TrainingModuleCategory ELSE 0 END)",
                    JobSiteSafetyAnalysisMap.CATEGORY_ID));
        }
    }

    public class JobSiteSafetyAnalysisMap : SubclassMap<JobSiteSafetyAnalysis>
    {
        public const int CATEGORY_ID = 1221;

        public JobSiteSafetyAnalysisMap()
        {
            DiscriminatorValue(CATEGORY_ID);
        }
    }
}
