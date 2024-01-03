using MapCall.Common.Model.Entities;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;
using StructureMap;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Models.ViewModels
{
    public abstract class BusinessUnitViewModel : ViewModel<BusinessUnit>
    {
        #region Properties

        [Required]
        [DropDown, EntityMap, EntityMustExist(typeof(Department))]
        public int? Department { get; set; }

        [Required, StringLength(BusinessUnit.StringLengths.BU)]
        public string BU { get; set; }

        [Required]
        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }
        
        [DropDown, EntityMap, EntityMustExist(typeof(BusinessUnitArea))]
        public int? Area { get; set; }

        [Required]
        public int? Order { get; set; }

        [StringLength(BusinessUnit.StringLengths.DESCRIPTION)]
        public string Description { get; set; }

        [Required]
        public bool? Is271Visible { get; set; }
        
        // This can't be filtered by OperatingCenter.
        [DropDown, EntityMap, EntityMustExist(typeof(Employee))]
        public int? EmployeeResponsible { get; set; }

        public bool? IsActive { get; set; }
        public int? AuthorizedStaffingLevelTotal { get; set; }
        public int? AuthorizedStaffingLevelManagement { get; set; }
        public int? AuthorizedStaffingLevelNonBargainingUnit { get; set; }
        public int? AuthorizedStaffingLevelBargainingUnit { get; set; }

        #endregion

        #region Constructor

        protected BusinessUnitViewModel(IContainer container) : base(container) { }

        #endregion
    }
}