using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Validation;
using StructureMap;
using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace MapCallMVC.Models.ViewModels
{
    public class GrievanceViewModel : ViewModel<Grievance>
    {
        #region Properties

        [View(Grievance.DisplayNames.GRIEVANCE_ID)]
        public override int Id { get; set; }
        [DropDown]
        [Required]
        [EntityMustExist(typeof(OperatingCenter))]
        [EntityMap]
        [View(Grievance.DisplayNames.OPERATING_CENTER)]
        public virtual int? OperatingCenter { get; set; }
        [EntityMustExist(typeof(UnionContract))]
        [EntityMap]
        [View(Grievance.DisplayNames.CONTRACT_ID)]
        [DropDown("UnionContract", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an Operating Center above.")]
        public virtual int? Contract { get; set; }
        [DropDown]
        [EntityMustExist(typeof(GrievanceCategory))]
        [EntityMap]
        public virtual int? GrievanceCategory { get; set; }
        [DropDown("GrievanceCategorization", "ByCategoryIdOrAll", DependsOn = "GrievanceCategory", PromptText = "Please select an Grievance Category above.")]
        [EntityMustExist(typeof(GrievanceCategorization))]
        [EntityMap]
        [View(Grievance.DisplayNames.GRIEVANCE_CATEGORIZATION)]
        public virtual int? Categorization { get; set; }
        [DropDown]
        [EntityMustExist(typeof(GrievanceStatus))]
        [EntityMap]
        [View(Grievance.DisplayNames.GRIEVANCE_STATUS)]
        public virtual int? Status { get; set; }
        [View(Grievance.DisplayNames.GRIEVANCE_NUM), StringLength(GrievanceMap.NUMBER_LENGTH)]
        public virtual string Number { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public virtual DateTime? DateReceived { get; set; }
        public virtual decimal? EstimatedImpactValue { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public virtual DateTime? IncidentDate { get; set; }
        [View(Grievance.DisplayNames.GRIEVANCE_STEP_1), DataType(DataType.MultilineText)]
        public virtual string Description { get; set; }
        [DataType(DataType.MultilineText)]
        public virtual string DescriptionOfOutcome { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public virtual DateTime? UnionDueDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public virtual DateTime? ManagementDueDate { get; set; }

        #endregion

        #region Constructors

        public GrievanceViewModel(IContainer container) : base(container) {}

        #endregion
    }

    public class EditGrievance : GrievanceViewModel
    {
        #region Constructors

        public EditGrievance(IContainer container) : base(container) {}

        #endregion

        [AutoComplete("", "Employee", "AllEmployeesByEmployeePartialFirstOrLastName", DisplayProperty = nameof(Employee.FullName), PlaceHolder = "Please type at least 3 characters of either first or last name to display the lookup list.")]
        [EntityMap, EntityMustExist(typeof(Employee))]
        public int? LaborRelationsBusinessPartner { get; set; }
    }

    public class CreateGrievance : GrievanceViewModel
    {
        #region Constructors

        public CreateGrievance(IContainer container) : base(container) { }

        #endregion

        // Why is LaborRelationsBusinessPartner required on the create model, but not the edit model? -Ross 9/29/2023

        [AutoComplete("", "Employee", "ActiveEmployeesByEmployeePartialFirstOrLastName", PlaceHolder = "Please type at least 3 characters of either first or last name to display the lookup list.")]
        [EntityMap, EntityMustExist(typeof(Employee))]
        [Required]
        public int? LaborRelationsBusinessPartner { get; set; }

        public override void Map(Grievance entity)
        {
            base.Map(entity);
            DateReceived = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
        }
    }

    public class SearchGrievance : SearchSet<Grievance>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(GrievanceCategory))]
        public int? GrievanceCategory { get; set; }

        [EntityMap, EntityMustExist(typeof(GrievanceCategorization))]
        [MultiSelect("", "GrievanceCategorization", "ByCategoryIdOrAll", DependsOn = "GrievanceCategory", DependentsRequired = DependentRequirement.None, PromptText = "Please select an Grievance Category above.")]
        [View(Grievance.DisplayNames.GRIEVANCE_CATEGORIZATION)]
        public int[] Categorization { get; set; }

        [MultiSelect]
        public int[] Status { get; set; }

        [DropDown]
        public int? OperatingCenter { get; set; }

        [DropDown("UnionContract", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an Operating Center above.")]
        public int? Contract { get; set; }

        [View(Grievance.DisplayNames.GRIEVANCE_ID)]
        public int? EntityId { get; set; }

        [View(Grievance.DisplayNames.GRIEVANCE_NUM)]
        public string Number { get; set; }

        public DateRange DateReceived { get; set; }
        public DateRange IncidentDate { get; set; }
        public DateRange UnionDueDate { get; set; }
        public DateRange ManagementDueDate { get; set; }

        [AutoComplete("", "Employee", "AllEmployeesByEmployeePartialFirstOrLastName")]
        [EntityMap, EntityMustExist(typeof(Employee))]
        public int? LaborRelationsBusinessPartner { get; set; }

        #endregion
    }
}