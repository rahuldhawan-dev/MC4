using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Migrations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class TrainingRequirement : ReadOnlyEntityLookup
    {
        #region Consts

        /// <summary>
        /// Use this because this description is larger than EntityLookup descriptions.
        /// </summary>
        public const int DESCRIPTION_MAX_LENGTH = CreatePositionGroupCommonNamesTableBug2108.DESCRIPTION_LENGTH;

        #endregion

        #region Properties

        [DisplayName("Is DOT Requirement")]
        public virtual bool IsDOTRequirement { get; set; }

        [DisplayName("Is OSHA Requirement")]
        public virtual bool IsOSHARequirement { get; set; }

        [DisplayName("Is DPCC Requirement")]
        public virtual bool IsDPCCRequirement { get; set; }

        [DisplayName("Is PSM / TCPA Requirement")]
        public virtual bool IsPSMTCPARequirement { get; set; }

        public virtual bool IsProductionRequirement { get; set; }
        public virtual bool IsFieldOperationsRequirement { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual int? TrainingFrequency { get; set; }
        public virtual string TrainingFrequencyUnit { get; set; }

        public virtual Regulation Regulation { get; set; }

        [DisplayName("OSHA Standard")]
        public virtual Regulation OSHAStandard { get; set; }

        public virtual bool HasRecurringTrainingRequirement => (TrainingFrequency > 0);

        public virtual IList<PositionGroupCommonName> PositionGroupCommonNames { get; set; }
        public virtual IList<TrainingModule> TrainingModules { get; set; }

        public virtual IEnumerable<TrainingModule> InitialTrainingModules
        {
            get
            {
                return TrainingModules.Where(x =>
                    x.TrainingModuleRecurrantType != null && x.TrainingModuleRecurrantType.Id ==
                    TrainingModuleRecurrantType.Indices.INITIAL);
            }
        }

        public virtual IEnumerable<TrainingModule> RecurringTrainingModules
        {
            get
            {
                return TrainingModules.Where(x =>
                    x.TrainingModuleRecurrantType != null && x.TrainingModuleRecurrantType.Id ==
                    TrainingModuleRecurrantType.Indices.RECURRING);
            }
        }

        public virtual IEnumerable<TrainingModule> InitialAndRecurringTrainingModules
        {
            get
            {
                return TrainingModules.Where(x =>
                    x.TrainingModuleRecurrantType != null && x.TrainingModuleRecurrantType.Id ==
                    TrainingModuleRecurrantType.Indices.INITIAL_RECURRING);
            }
        }

        public virtual TrainingModule ActiveInitialTrainingModule { get; set; }
        public virtual TrainingModule ActiveRecurringTrainingModule { get; set; }
        public virtual TrainingModule ActiveInitialAndRecurringTrainingModule { get; set; }

        #region Logical Properties

        public virtual bool HasTrainingModules { get; set; }
        public virtual bool HasActiveTrainingModules { get; set; }

        [Display(Name = "Training Frequency")]
        public virtual RecurrenceFrequency TrainingFrequencyObj =>
            new RecurrenceFrequency(TrainingFrequency, TrainingFrequencyUnit);

        #endregion

        #endregion

        #region Constructor

        public TrainingRequirement()
        {
            TrainingModules = new List<TrainingModule>();
            PositionGroupCommonNames = new List<PositionGroupCommonName>();
        }

        #endregion
    }
}
