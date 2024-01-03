using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DataAnnotationsExtensions;
using FluentNHibernate.Automapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Models.ViewModels
{
    public class TrainingRequirementViewModel : ViewModel<TrainingRequirement>
    {
        #region Properties

        [Required]
        [StringLength(TrainingRequirement.DESCRIPTION_MAX_LENGTH)] // Use DESCRIPTION_MAX_LENGTH instead of StringLengths.DESCRIPTION because it is a longer value.
        public string Description { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(Regulation))]
        public int? Regulation { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(Regulation)), RequiredWhen("IsOSHARequirement", true, ErrorMessage = "The OSHA Standard field is required for OSHA-required training.")]
        public int? OSHAStandard { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(TrainingModule))]
        public int? ActiveInitialTrainingModule { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(TrainingModule))]
        public int? ActiveRecurringTrainingModule { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(TrainingModule))]
        public int? ActiveInitialAndRecurringTrainingModule { get; set; }
        
        [Required, DisplayName("Is DOT Requirement")]
        public bool? IsDOTRequirement { get; set; }

        [Required, DisplayName("Is OSHA Requirement")]
        public bool? IsOSHARequirement { get; set; }

        [Required, DisplayName("Is DPCC Requirement")]
        public bool? IsDPCCRequirement { get; set; }

        [Required, DisplayName("Is PSM / TCPA Requirement")]
        public bool? IsPSMTCPARequirement { get; set; }

        [Required]
        public bool? IsProductionRequirement { get; set; }

        [Required]
        public bool? IsFieldOperationsRequirement { get; set; }

        [Required]
        public bool? IsActive { get; set; }

        [DisplayName("Has Combined Module"), DoesNotAutoMap("Manually mapped for display.")]
        public bool HasCombinedTrainingModule { get; set; }

        public virtual RecurrenceFrequency TrainingFrequency { get; set; }

        #endregion

        #region Constructors

        public TrainingRequirementViewModel(IContainer container) : base(container) { }
        
        #endregion

        #region Exposed Methods

        public override void Map(TrainingRequirement entity)
        {
            HasCombinedTrainingModule = entity.ActiveInitialAndRecurringTrainingModule != null;
            TrainingFrequency = new RecurrenceFrequency(entity.TrainingFrequency, entity.TrainingFrequencyUnit);

            base.Map(entity);
        }

        public override TrainingRequirement MapToEntity(TrainingRequirement entity)
        {
            entity = base.MapToEntity(entity);
            if (TrainingFrequency != null)
            {
                entity.TrainingFrequency = TrainingFrequency.Frequency;
                entity.TrainingFrequencyUnit = TrainingFrequency.UnitStr;
            }

            return entity;
        }

        #endregion
    }

    public class SearchTrainingRequirement : SearchSet<TrainingRequirement>
    {
        #region Properties

        public virtual string Description { get; set; }
        public virtual bool? HasActiveTrainingModules { get; set; }
        public virtual bool? HasTrainingModules { get; set; }
        public virtual bool? IsActive { get; set; }
        [DisplayName("OSHA Requirement")]
        public bool? IsOSHARequirement { get; set; }

        #endregion
    }
}