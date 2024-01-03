using MapCall.Common.Model.Entities;
using MapCall.Common.Utility;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.ClassExtensions.DateTimeExtensions;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class MarkoutViewModel : ViewModel<Markout>
    {
        #region Constructors

        public MarkoutViewModel(IContainer container) : base(container) { }

        #endregion

        #region Consts

        private const string MARKOUT_NUMBER_LENGTH_MUST_BE_GREATER_THAN_9 = "Must be 9 or more characters.";
        private const string NOTE_LENGTH_MUST_BE_GREATER_THAN_10 = "Please enter valid notes for the Markout Type.";

        #endregion

        #region Properties

        [Required, EntityMap, EntityMustExist(typeof(WorkOrder))]
        public int? WorkOrder { get; set; }

        [ClientCallback("MarkoutForm.validateMarkoutNumber", ErrorMessage = MARKOUT_NUMBER_LENGTH_MUST_BE_GREATER_THAN_9)]
        [Required, StringLength(Markout.StringLengths.MARKOUT_NUMBER_MAX_LENGTH, MinimumLength = Markout.StringLengths.MARKOUT_NUMBER_MIN_LENGTH)]
        public string MarkoutNumber { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(MarkoutType))]
        public int? MarkoutType { get; set; }
        
        [Multiline, RequiredWhen(nameof(MarkoutType), MapCall.Common.Model.Entities.MarkoutType.Indices.NONE, ErrorMessage = "Required."), MinLength(10, ErrorMessage = NOTE_LENGTH_MUST_BE_GREATER_THAN_10)]
        public string Note { get; set; }

        [Required]
        public DateTime? DateOfRequest { get; set; }

        [RequiredWhen(nameof(WorkOrderOperatingCenterMarkoutEditable), true), DoesNotAutoMap, DateTimePicker]
        public DateTime? ReadyDate { get; set; }

        [RequiredWhen(nameof(WorkOrderOperatingCenterMarkoutEditable), true), DoesNotAutoMap, DateTimePicker]
        public DateTime? ExpirationDate { get; set; }

        [DoesNotAutoMap]
        public bool WorkOrderOperatingCenterMarkoutEditable
        {
            get
            {
                if (!WorkOrder.HasValue)
                {
                    return false;
                }
                var order = _container.GetInstance<IRepository<WorkOrder>>().Find(WorkOrder.Value);
                if (order == null)
                {
                    return false;
                }
                return order.OperatingCenter.MarkoutsEditable;
            }
        }

        #endregion

        #region Public Methods

        public override Markout MapToEntity(Markout entity)
        {
            base.MapToEntity(entity);

            // INC000000118985: Only allow users to enter ReadyDate/ExpirationDate when MarkoutsEditable.
            if (entity.WorkOrder.OperatingCenter.MarkoutsEditable)
            {
                entity.ReadyDate = ReadyDate;
                entity.ExpirationDate = ExpirationDate;
            }
            else
            {
                // This confuses me. In the view, a markout can not be created if MarkoutRequired == false, but apparently they can be edited regardless?
                if (entity.WorkOrder.MarkoutRequired)
                {
                    var markoutRequirementEnum = entity.WorkOrder.MarkoutRequirement.MarkoutRequirementEnum;

                    entity.ReadyDate = WorkOrdersWorkDayEngine.GetReadyDate(entity.DateOfRequest.Value, markoutRequirementEnum);

                    if (entity.WorkOrder.CrewAssignments.Count > 0)
                    {
                        var tmpExpDate = WorkOrdersWorkDayEngine.GetExpirationDate(entity.DateOfRequest.Value,
                            entity.WorkOrder.MarkoutRequirement.MarkoutRequirementEnum).EndOfDay();
                        // DateStarted version:
                        var workStarted = (from ca in entity.WorkOrder.CrewAssignments
                                           where ca.DateStarted >= ReadyDate &&
                                                 ca.DateStarted <= tmpExpDate
                                           select ca).Count() > 0;

                        entity.ExpirationDate = WorkOrdersWorkDayEngine.GetExpirationDate(entity.DateOfRequest.Value,
                            entity.WorkOrder.MarkoutRequirement.MarkoutRequirementEnum, workStarted);
                    }
                    else
                    {
                        entity.ExpirationDate = WorkOrdersWorkDayEngine.GetExpirationDate(entity.DateOfRequest.Value,
                                entity.WorkOrder.MarkoutRequirement.MarkoutRequirementEnum);
                    }
                }
            }

            return entity;
        }

        private IEnumerable<ValidationResult> ValidateMarkoutNumber()
        {
            // These are both handled by Required attributes. Break out early.
            if (string.IsNullOrEmpty(MarkoutNumber) || !WorkOrder.HasValue)
            {
                yield break;
            }

            // WorkOrder existence Handled by other validation.
            var workOrder = _container.GetInstance<IRepository<WorkOrder>>().Find(WorkOrder.Value);
            if (workOrder == null)
            {
                yield break;
            }

            if (!workOrder.OperatingCenter.MarkoutsEditable)
            {
                if (MarkoutNumber.Length < 9) // Markouts must have a length of 9.
                {
                    yield return new ValidationResult(MARKOUT_NUMBER_LENGTH_MUST_BE_GREATER_THAN_9, new[] { "MarkoutNumber" });
                }
            }
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateMarkoutNumber());
        }

        #endregion
    }
}
