using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DataAnnotationsExtensions;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class StandardOperatingProcedureViewModel : ViewModel<StandardOperatingProcedure>
    {
        public int? SopCrossRefId { get; set; }
        public virtual string Description { get; set; }
        public virtual int? EquipmentId { get; set; }
        public virtual DateTime? DateApproved { get; set; }
        public virtual DateTime? DateIssued { get; set; }
        public virtual string Revision { get; set; }
        public virtual string ReviewFrequencyDays { get; set; }
        public virtual bool? PsmTcpa { get; set; }
        public virtual bool? Dpcc { get; set; }
        public virtual bool? Osha { get; set; }
        public virtual bool? Company { get; set; }
        public virtual bool? Sox { get; set; }
        public virtual bool? Safety { get; set; }

        [DropDown, EntityMustExist(typeof(SOPSection)), EntityMap]
        public virtual int? Section { get; set; }
        [DropDown, EntityMustExist(typeof(SOPSubSection)), EntityMap]
        public virtual int? SubSection { get; set; }
        [DropDown, EntityMustExist(typeof(PolicyPractice)), EntityMap]
        public virtual int? PolicyPractice { get; set; }
        [DropDown, EntityMustExist(typeof(FunctionalArea)), EntityMap]
        public virtual int? FunctionalArea { get; set; }
        [DropDown, EntityMustExist(typeof(SOPStatus)), EntityMap]
        public virtual int? Status { get; set; }
        [DropDown, EntityMustExist(typeof(SOPCategory)), EntityMap]
        public virtual int? Category { get; set; }
        [DropDown, EntityMustExist(typeof(SOPSystem)), EntityMap]
        public virtual int? System { get; set; }
        [DropDown, EntityMustExist(typeof(OperatingCenter)), EntityMap]
        public virtual int? OperatingCenter { get; set; }
        [DropDown, EntityMustExist(typeof(Facility)), EntityMap]
        public virtual int? Facility { get; set; }

        public StandardOperatingProcedureViewModel(IContainer container) : base(container) { }
    }

    public class CreateStandardOperatingProcedure : StandardOperatingProcedureViewModel
    {
        public CreateStandardOperatingProcedure(IContainer container) : base(container) { }

        public override void SetDefaults()
        {
            base.SetDefaults();

            // bug-2662: Default SubSection to ".05". This is sloppy but will probably end up 
            // being reverted.
            var sub =
                _container
                    .GetInstance<IRepository<SOPSubSection>>()
                    .GetAll()
                    .SingleOrDefault(x => x.Description == ".05 SOP");

            if (sub != null)
            {
                SubSection = sub.Id;
            }
        }
    }

    public class EditStandardOperatingProcedure : StandardOperatingProcedureViewModel
    {
        public EditStandardOperatingProcedure(IContainer container) : base(container) { }
    }

    public class SearchStandardOperatingProcedure : SearchSet<StandardOperatingProcedure>
    {
        [DropDown]
        public int? Section { get; set; }
        [DropDown]
        public int? SubSection { get; set; }
        [DropDown]
        public int? OperatingCenter { get; set; }
        [DropDown]
        public int? FunctionalArea { get; set; }
        [DropDown]
        public int? Status { get; set; }
        [DropDown]
        public int? Category { get; set; }
        [DropDown]
        public int? System { get; set; }
        public DateRange DateApproved { get; set; }
        public DateRange DateIssued { get; set; }
        public bool? HasReviewRequirements { get; set; }
        public SearchString Description { get; set; }
    }

    public class AddStandardOperatingProcedureQuestion : ViewModel<StandardOperatingProcedure>
    {
        #region Properties

        [Multiline, Required, DoesNotAutoMap("Manually added to StandardOperatingProcedureQuestion")] // No string length, text field
        public string Question { get; set; }

        [Multiline, Required, DoesNotAutoMap("Manually added to StandardOperatingProcedureQuestion")] // No string length, text field
        public string Answer { get; set; }

        [Required, CheckBox, DoesNotAutoMap("Manually added to StandardOperatingProcedureQuestion")]
        public bool? IsActive { get; set; }

        #endregion

        #region Constructors

        public AddStandardOperatingProcedureQuestion(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override StandardOperatingProcedure MapToEntity(StandardOperatingProcedure entity)
        {
            // Do not call base method.

            var q = new StandardOperatingProcedureQuestion();
            q.Question = Question;
            q.Answer = Answer;
            q.StandardOperatingProcedure = entity;
            q.IsActive = IsActive.Value;
            entity.Questions.Add(q);
            return entity;
        }

        #endregion
    }

    public class RemoveStandardOperatingProcedureQuestion : ViewModel<StandardOperatingProcedure>
    {
        [Required, DoesNotAutoMap("Used to manually remove item from collection")]
        public int? QuestionId { get; set; }

        public RemoveStandardOperatingProcedureQuestion(IContainer container) : base(container) { }

        public override StandardOperatingProcedure MapToEntity(StandardOperatingProcedure entity)
        {
            // Do not call base

            var toRemove = entity.Questions.SingleOrDefault(x => x.Id == QuestionId.Value);

            if (toRemove != null)
            {
                entity.Questions.Remove(toRemove);
            }

            return entity;
        }

    }

    public class AddSOPPGCN : ViewModel<StandardOperatingProcedure>
    {
        [DoesNotAutoMap("Not a property on StandardOperatingProcedure")]
        [DropDown, Required, EntityMustExist(typeof(PositionGroupCommonName))]
        public int? PositionGroupCommonName { get; set; }

        [DoesNotAutoMap("Not a property on StandardOperatingProcedure")]
        [Required, Min(1)]
        public int? Frequency { get; set; }

        [DoesNotAutoMap("Not a property on StandardOperatingProcedure")]
        [Required, DropDown, EntityMustExist(typeof(RecurringFrequencyUnit))]
        public int? FrequencyUnit { get; set; }

        public AddSOPPGCN(IContainer container) : base(container) { }

        public override StandardOperatingProcedure MapToEntity(StandardOperatingProcedure entity)
        {
            // Do not call base.
            var requirement = new StandardOperatingProcedurePositionGroupCommonNameRequirement();
            requirement.StandardOperatingProcedure = entity;
            requirement.PositionGroupCommonName =
                _container.GetInstance<IPositionGroupCommonNameRepository>().Find(PositionGroupCommonName.Value);
            requirement.Frequency = Frequency.Value;
            requirement.FrequencyUnit =
                _container.GetInstance<IRecurringFrequencyUnitRepository>().Find(FrequencyUnit.Value);
            entity.PGCNRequirements.Add(requirement);
            return entity;

        }
    }

    public class RemoveSOPPGCN : ViewModel<StandardOperatingProcedure>
    {
        [Required, DoesNotAutoMap("Not a property on StandardOperatingProcedure")]
        public int? PositionGroupCommonNameRequirement { get; set; }

        public RemoveSOPPGCN(IContainer container) : base(container) { }

        public override StandardOperatingProcedure MapToEntity(StandardOperatingProcedure entity)
        {
            // Do not call base.
            var requirements =
                entity.PGCNRequirements.Where(x => x.Id == PositionGroupCommonNameRequirement.Value)
                    .ToArray();

            foreach (var req in requirements)
            {
                entity.PGCNRequirements.Remove(req);
            }
            return entity;

        }
    }

    public class AddSOPTrainingModule : ViewModel<StandardOperatingProcedure>
    {
        [DoesNotAutoMap("Not an entity property.")]
        [DropDown, Required, EntityMustExist(typeof(TrainingModule))]
        public int? TrainingModule { get; set; }

        public AddSOPTrainingModule(IContainer container) : base(container) { }

        public override StandardOperatingProcedure MapToEntity(StandardOperatingProcedure entity)
        {
            // Do not call base.

            var tm = _container.GetInstance<ITrainingModuleRepository>().Find(TrainingModule.Value);
            entity.TrainingModules.Add(tm);
            return entity;
        }
    }

    public class RemoveSOPTrainingModule : ViewModel<StandardOperatingProcedure>
    {
        [Required, DoesNotAutoMap("Not an entity property.")]
        public int? TrainingModule { get; set; }

        public RemoveSOPTrainingModule(IContainer container) : base(container) { }

        public override StandardOperatingProcedure MapToEntity(StandardOperatingProcedure entity)
        {
            // Do not call base.
            var train = _container.GetInstance<ITrainingModuleRepository>().Find(TrainingModule.Value);
            entity.TrainingModules.Remove(train);
            return entity;

        }
    }
}