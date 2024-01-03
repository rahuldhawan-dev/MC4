using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.ProjectManagement.Models.ViewModels
{
    public class DevelopmentProjectViewModel : ViewModel<DevelopmentProject>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter)), Required]
        public int? OperatingCenter { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(DevelopmentProjectCategory)), Required]
        public int? Category { get; set; }
        [DropDown("", "BusinessUnit", "FindByOperatingCenterId", DependsOn = "OperatingCenter"), EntityMap, EntityMustExist(typeof(BusinessUnit)), Required]
        public int? BusinessUnit { get; set; }
        [DropDown("", "Employee", "ActiveEmployeesByOperatingCenterId", DependsOn = "OperatingCenter"), EntityMap, EntityMustExist(typeof(Employee))]
        public int? ProjectManager { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(PublicWaterSupply)), Required]
        public int? PublicWaterSupply { get; set; }
        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter"), EntityMap, EntityMustExist(typeof(Town))]
        public int? Town { get; set; }

        [StringLength(DevelopmentProject.StringLengths.DEVELOPER_SERVICES_ID)]
        [DisplayName("Developer Services Reference Id")]
        public string DeveloperServicesId { get; set; }
        [StringLength(DevelopmentProject.StringLengths.WBS_NUMBER), Required]
        public virtual string WBSNumber { get; set; } 
        [Multiline, Required]
        public virtual string ProjectDescription { get; set; } 
        [StringLength(DevelopmentProject.StringLengths.STREET_NAME)]
        public virtual string StreetName { get; set; } 

        [Required]
        public virtual DateTime? ForecastedInServiceDate { get; set; } 
        public virtual DateTime? InServiceDate { get; set; }

        [Required, Range(0, 999)]
        public virtual int? DomesticWaterServices { get; set; }
        [Required, Range(0, 999)]
        public virtual int? FireServices { get; set; }
        [Required, Range(0, 999)]
        public virtual int? DomesticSanitaryServices { get; set; }

        #endregion

        #region Constructors

        public DevelopmentProjectViewModel(IContainer container) : base(container) {}

        #endregion
    }

    public class CreateDevelopmentProject : DevelopmentProjectViewModel
    {
        #region Constructors

		public CreateDevelopmentProject(IContainer container) : base(container) {}

        #endregion
    }

    public class EditDevelopmentProject : DevelopmentProjectViewModel
    {
        #region Constructors

		public EditDevelopmentProject(IContainer container) : base(container) {}

        #endregion
    }

    public class SearchDevelopmentProject : SearchSet<DevelopmentProject>
    {
        #region Properties

        [DropDown, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }
        [DropDown,  EntityMustExist(typeof(DevelopmentProjectCategory))]
        public int? Category { get; set; }
        [DropDown("", "BusinessUnit", "FindByOperatingCenterId", DependsOn = "OperatingCenter"), EntityMap, EntityMustExist(typeof(BusinessUnit))]
        public int? BusinessUnit { get; set; }
        [DropDown("", "Employee", "ActiveEmployeesByOperatingCenterId", DependsOn = "OperatingCenter"), EntityMap, EntityMustExist(typeof(Employee))]
        public int? ProjectManager { get; set; }
        [DropDown,  EntityMustExist(typeof(PublicWaterSupply))]
        public int? PublicWaterSupply { get; set; }
        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter"), EntityMustExist(typeof(Town))]
        public int? Town { get; set; }

        public SearchString DeveloperServicesId { get; set; }
        public SearchString WBSNumber { get; set; } 
        public SearchString StreetName { get; set; }
        [SearchAlias("CreatedBy", "UserName")]
        public SearchString CreatedBy { get; set; } 

        public virtual DateRange ForecastedInServiceDate { get; set; } 
        public virtual DateRange InServiceDate { get; set; }

        #endregion
    }
}