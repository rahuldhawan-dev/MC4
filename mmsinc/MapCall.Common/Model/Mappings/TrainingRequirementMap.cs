using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class TrainingRequirementMap : ClassMap<TrainingRequirement>
    {
        public const string SQL_HAS_CURRENT_TRAINING_MODULES =
            "(CASE " +
            "WHEN (TrainingFrequency = 0 AND isNull(ActiveInitialTrainingModuleId, 0) > 0) THEN 1 " +
            "WHEN (TrainingFrequency > 0 AND isNull(ActiveInitialAndRecurringTrainingModuleId,0) > 0) THEN 1 " +
            "WHEN (TrainingFrequency > 0 AND isNull(ActiveInitialTrainingModuleId,0) > 1 AND isNull(ActiveRecurringTrainingModuleId, 0) > 0) THEN 1 " +
            "ELSE 0 " +
            "END)";

        public TrainingRequirementMap()
        {
            Id(x => x.Id);

            Map(x => x.Description)
                // Don't use StringLengths.Description here because this max length
                // is 100 instead of 50.
               .Length(TrainingRequirement.DESCRIPTION_MAX_LENGTH)
               .Unique()
               .Not.Nullable();
            Map(x => x.TrainingFrequency);
            Map(x => x.TrainingFrequencyUnit);

            Map(x => x.IsDOTRequirement).Not.Nullable();
            Map(x => x.IsDPCCRequirement).Not.Nullable();
            Map(x => x.IsFieldOperationsRequirement).Not.Nullable();
            Map(x => x.IsOSHARequirement).Not.Nullable();
            Map(x => x.IsProductionRequirement).Not.Nullable();
            Map(x => x.IsPSMTCPARequirement).Not.Nullable();
            Map(x => x.IsActive).Not.Nullable();

            HasManyToMany(x => x.PositionGroupCommonNames)
               .Table("PositionGroupCommonNamesTrainingRequirements")
               .ParentKeyColumn("TrainingRequirementID")
               .ChildKeyColumn("PositionGroupCommonNameID");

            HasMany(x => x.TrainingModules).KeyColumn("TrainingRequirementId");

            References(x => x.Regulation);
            References(x => x.OSHAStandard);

            References(x => x.ActiveInitialTrainingModule);
            References(x => x.ActiveRecurringTrainingModule);
            References(x => x.ActiveInitialAndRecurringTrainingModule);

            Map(x => x.HasTrainingModules)
               .Formula("(CASE WHEN (EXISTS (SELECT 1 FROM [" + TrainingModuleMap.TABLE_NAME +
                        "] tm where tm.TrainingRequirementID = Id)) THEN 1 ELSE 0 END)");
            Map(x => x.HasActiveTrainingModules)
               .DbSpecificFormula(SQL_HAS_CURRENT_TRAINING_MODULES,
                    SQL_HAS_CURRENT_TRAINING_MODULES.ToUpper().Replace("ISNULL", "IFNULL"));
        }
    }
}
