using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class EditEmployeeAssignment : ViewModel<EmployeeAssignment>
    {
        #region Constructors

        public EditEmployeeAssignment(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [Required]
        public DateTime? AssignedFor { get; set; }
        [Required, DropDown, EntityMap, EntityMustExist(typeof(Employee))]
        public int? AssignedTo { get; set; }
        [DateTimePicker]
        public DateTime? DateStarted { get; set; }
        [DateTimePicker]
        public DateTime? DateEnded { get; set; }
        //public IList<Employee> Employees { get; set; }
        [DoesNotAutoMap("Set in MapToEntity, used by controller")]
        public bool ProgressWorkOrder { get; set; }
        [Required]
        [RegularExpression(@"(?:\d*\.\d{1,2}|\d+)$", ErrorMessage = EmployeeAssignment.MUST_BE_TWO_DECIMAL_PLACES)]
        [View("Hours Worked", FormatStyle.DecimalMaxTwoDecimalPlacesWithTrailingZeroes)]
        public decimal? HoursWorked { get; set; }

        #endregion

        public override EmployeeAssignment MapToEntity(EmployeeAssignment entity)
        {
            entity = base.MapToEntity(entity);

            if (entity.ProductionWorkOrder.ApprovedOn.HasValue || entity.ProductionWorkOrder.DateCancelled.HasValue || !entity.ProductionWorkOrder.OperatingCenter.CanSyncWithSAP)
                return entity;

            ProgressWorkOrder = true;
            return entity;
        }
    }
}
