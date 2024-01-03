using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;
using System;
using System.ComponentModel.DataAnnotations;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class AddEmployeeAssignmentProductionWorkOrder : ViewModel<ProductionWorkOrder>
    {
        #region Properties

        //[Required, EntityMustExist(typeof(EmployeeAssignment))]
        //public int? EmployeeAssignment { get; set; }

        [DoesNotAutoMap("Mapped manually")]
        [Required, DropDown, EntityMustExist(typeof(OperatingCenter))]
        public virtual int? OperatingCenter { get; set; }

        [EntityMap(MapDirections.None), EntityMustExist(typeof(ProductionSkillSet))]
        public int? ProductionSkillSet { get; set; }

        [DoesNotAutoMap("Mapped manually")]
        [Required, EntityMustExist(typeof(Employee))]
        [DropDown(Area = "", Controller = "Employee", Action = "GetEmployeesForProductionWorkOrderSchedulingByOperatingCenterAndProductionSkillSet", DependsOn = "OperatingCenter, ProductionSkillSet", PromptText = "Please select an operating center above", DependentsRequired = DependentRequirement.One)]
        public int? AssignedTo { get; set; }

        [Required, DoesNotAutoMap("Mapped manually")]
        public DateTime? AssignedFor { get; set; }
        
        [DoesNotAutoMap("Set in MapToEntity for controller to use.")]
        public bool ProgressWorkOrder { get; set; }

        #endregion

        #region Constructors

        public AddEmployeeAssignmentProductionWorkOrder(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override ProductionWorkOrder MapToEntity(ProductionWorkOrder entity)
        {
            var assignedTo = _container.GetInstance<IEmployeeRepository>().Find(AssignedTo.Value);
            entity.EmployeeAssignments.Add(new EmployeeAssignment {
                ProductionWorkOrder = entity,
                AssignedFor = AssignedFor.Value,
                AssignedBy =_container.GetInstance<IAuthenticationService<User>>().CurrentUser.Employee,
                AssignedTo = assignedTo,
                AssignedOn = _container.GetInstance<IDateTimeProvider>().GetCurrentDate()
            });

            if (entity.ApprovedOn.HasValue || entity.DateCancelled.HasValue || !entity.OperatingCenter.CanSyncWithSAP)
                return entity;

            ProgressWorkOrder = true;
            return entity;
        }

        #endregion
    }
}