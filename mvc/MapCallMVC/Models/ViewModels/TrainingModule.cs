using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data.NHibernate;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Models.ViewModels
{
    public class TrainingModuleViewModel : ViewModel<TrainingModule>
    {
        #region Properties

        [DisplayName("Training Module Category"), DropDown]
        [EntityMustExist(typeof(TrainingModuleCategory))]
        [EntityMap]
        public virtual int? TrainingModuleCategory { get; set; }

        [DropDown,EntityMustExist(typeof(TrainingModuleRecurrantType)),EntityMap]
        public virtual int? TrainingModuleRecurrantType { get; set; }

        [DropDown, EntityMustExist(typeof(TrainingRequirement)), EntityMap]
        public virtual int? TrainingRequirement { get; set; }

        [DisplayName("Operating Center"), DropDown]
        [EntityMustExist(typeof(OperatingCenter))]
        [EntityMap]
        public virtual int? OperatingCenter { get; set; }

        [DisplayName("Facility")]
        [DropDown("", "Facility", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an Operating Center above")]
        [EntityMustExist(typeof(Facility))]
        [EntityMap]
        public virtual int? Facility { get; set; }

        [StringLength(TrainingModule.StringLengths.TITLE)]
        [RequiredWhen("TCHCertified", true)]
        public virtual string Title { get; set; }
        
        [DisplayName("NJDEP TCH COURSE APPROVAL NUMBER")]
        [StringLength(TrainingModule.StringLengths.COURSE_APPROVAL_NUMBER)]
        [RequiredWhen("TCHCertified", true)]
        public virtual string CourseApprovalNumber { get; set; }

        public virtual bool TCHCertified { get; set; }
        [RequiredWhen("TCHCertified", true)]
        public virtual float? TCHCreditValue { get; set; }
        [RequiredWhen("TCHCertified", true)]
        public virtual float? TotalHours { get; set; }
        
        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        public virtual string Description { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        public virtual string TrainingObjectives { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        public virtual string PracticalTestStandards { get; set; }
        
        [Required]
        public virtual bool? SafetyRelated { get; set; }
        public virtual bool Production { get; set; }
        public virtual int? EquipmentID { get; set; }
        public virtual bool IsActive { get; set; }

        [DisplayName("LEARN ID")]
        public virtual string AmericanWaterCourseNumber { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(LEARNItemType))]
        public virtual int? LEARNItemType { get; set; }

            #endregion

        #region Constructors
        
        public TrainingModuleViewModel(IContainer container) : base(container) {}

        #endregion
    }

    public class CreateTrainingModule : TrainingModuleViewModel
    {
        #region Constructors

        public CreateTrainingModule(IContainer container) : base(container) {}

        #endregion
    }

    public class EditTrainingModule : TrainingModuleViewModel
    {
        #region Constructors

        public EditTrainingModule(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override TrainingModule MapToEntity(TrainingModule entity)
        {
            entity = base.MapToEntity(entity);

            if (!IsActive)
            {
                if (entity.TrainingRequirement != null && entity.TrainingRequirement.ActiveInitialAndRecurringTrainingModule == entity)
                    entity.TrainingRequirement.ActiveInitialAndRecurringTrainingModule = null;
                if (entity.TrainingRequirement != null && entity.TrainingRequirement.ActiveInitialTrainingModule == entity)
                    entity.TrainingRequirement.ActiveInitialTrainingModule= null;
                if (entity.TrainingRequirement != null && entity.TrainingRequirement.ActiveRecurringTrainingModule == entity)
                    entity.TrainingRequirement.ActiveRecurringTrainingModule = null;
            }

            return entity;
        }

        #endregion
    }

    public abstract class SearchTrainingModule<TTrainingModule> : SearchSet<TTrainingModule> where TTrainingModule : TrainingModule
    {
        #region Properties

        [DisplayName("TrainingModuleID")]
        public int? EntityId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        [DropDown]
        public int? OperatingCenter { get; set; }
        [DropDown("", "Facility", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        public int? Facility { get; set; }
        [DisplayName("NJDEP TCH COURSE APPROVAL NUMBER")]
        public virtual string CourseApprovalNumber { get; set; }

        public virtual bool? TCHCertified { get; set; }
        public virtual bool? Production { get; set; }

        public virtual bool? IsActive { get; set; }

        [DisplayName("LEARN ID")]
        public virtual string AmericanWaterCourseNumber { get; set; }

        #endregion
    }

    public class SearchTrainingModule : SearchTrainingModule<TrainingModule>
    {
        [DropDown]
        public virtual int? TrainingModuleCategory { get; set; }
        public virtual bool? HasAttachedDocuments { get; set; }
    }

    public class SearchJobSiteSafetyAnalysis : SearchTrainingModule<JobSiteSafetyAnalysis>
    {
    }
}